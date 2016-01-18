using System;
using System.Web;
using umbraco.NodeFactory;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web.Routing;

namespace Our.Umbraco.HttpsRedirect.Events
{
    public class EventsHandler : ApplicationEventHandler
    {
        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            PublishedContentRequest.Prepared += PublishedContentRequest_Prepared;
        }

        private void PublishedContentRequest_Prepared(object sender, EventArgs e)
        {
            var request = sender as PublishedContentRequest;
            var currentContext = HttpContext.Current;
            var url = request.Uri.ToString();
            var isSecure = currentContext.Request.IsSecureConnection;
            var page = request.PublishedContent;

            if (page == null)
                return;

            // For load balanced environment with SSL termination. The X-Forwarded-Proto header
            // indicates what protocol was used with the original request.
            if (ShouldCheckForXForwardedProto())
            {
                if (currentContext.Request.Headers["X-Forwarded-Proto"] == "https")
                    isSecure = true;
            }

            // check if the port should be stripped.
            if (ShouldStripPort())
                url = StripPortFromUrl(url, currentContext.Request.Url);

            // check for matches
            if (HasMatch(page, request))
            {
                // if the doc-type matches and is NOT on HTTPS...
                if (!isSecure)
                {
                    // ... then redirect the URL to HTTPS.
                    PerformRedirect(url.Replace(Settings.HTTP, Settings.HTTPS), currentContext);
                }

                return;
            }

            // otherwise if the URL is on HTTPS...
            if (isSecure)
            {
                // ... redirect the URL back to HTTP.
                PerformRedirect(url.Replace(Settings.HTTPS, Settings.HTTP), currentContext);
                return;
            }
        }

        private static bool ShouldCheckForXForwardedProto()
        {
            bool check;
            var xForwardedProto = Settings.GetValueFromKey(Settings.AppKey_XForwardedProto);

            if (!string.IsNullOrWhiteSpace(xForwardedProto) && bool.TryParse(xForwardedProto, out check))
            {
                return check;
            }

            return false;
        }

        private static string StripPortFromUrl(string url, Uri contextUri)
        {
            return url.Replace(string.Format(":{0}", contextUri.Port), string.Empty);
        }

        private static bool ShouldStripPort()
        {
            return Settings.GetValueFromKey<bool>(Settings.AppKey_StripPort);
        }

        private static bool ShouldRedirectTemporary()
        {
            return Settings.GetValueFromKey<bool>(Settings.AppKey_UseTemporaryRedirects);
        }

        private static bool HasMatch(IPublishedContent page, PublishedContentRequest request)
        {
            return MatchesDocTypeAlias(page.DocumentTypeAlias)
                || MatchesNodeId(page.Id)
                || MatchesTemplate(request.TemplateAlias)
                || MatchesPropertyValue((page.Id));
        }

        private static bool MatchesDocTypeAlias(string docTypeAlias)
        {
            return Settings.KeyContainsValue(Settings.AppKey_DocTypes, docTypeAlias);
        }

        private static bool MatchesNodeId(int pageId)
        {
            return Settings.KeyContainsValue(Settings.AppKey_PageIds, pageId);
        }

        private static bool MatchesTemplate(string templateAlias)
        {
            return Settings.KeyContainsValue(Settings.AppKey_Templates, templateAlias);
        }

        private static bool MatchesPropertyValue(int pageId)
        {
            var appSetting = Settings.GetValueFromKey(Settings.AppKey_Properties);

            if (string.IsNullOrEmpty(appSetting))
                return false;

            var node = new Node(pageId);
            var items = appSetting.Split(new[] { Settings.COMMA }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var item in items)
            {
                var parts = item.Split(new[] { Settings.COLON }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0)
                    continue;

                var propertyAlias = parts[0];
                var propertyValue = Settings.CHECKBOX_TRUE;

                if (parts.Length > 1)
                    propertyValue = parts[1];

                var property = node.GetProperty(propertyAlias);
                if (property == null)
                    continue;

                var match = string.Equals(property.Value, propertyValue, StringComparison.InvariantCultureIgnoreCase);
                if (match)
                    return true;
            }

            return false;
        }

        private static void PerformRedirect(string targetUrl, HttpContext context)
        {
            if (ShouldRedirectTemporary())
                context.Response.Redirect(targetUrl, true);
            else
                context.Response.RedirectPermanent(targetUrl, true);
        }

    }
}