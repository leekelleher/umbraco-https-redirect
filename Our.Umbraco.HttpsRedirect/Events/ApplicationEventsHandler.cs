using System;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.NodeFactory;
using umbraco.cms.businesslogic.template;

namespace Our.Umbraco.HttpsRedirect.Events
{
	public class EventsHandler : ApplicationBase
	{
		public EventsHandler()
		{
			UmbracoDefault.AfterRequestInit += new UmbracoDefault.RequestInitEventHandler(this.UmbracoDefaultAfterRequestInit);
		}

		private void UmbracoDefaultAfterRequestInit(object sender, RequestInitEventArgs e)
		{
			var url = e.Context.Request.Url.ToString(); // .ToLower(); also lowercases query string which caused us issues (DF)

			var page = e.Page;

			if (page == null)
				return;

			// check if the port should be stripped.
			if (ShouldStripPort())
				url = StripPortFromUrl(url, e.Context.Request.Url);

			// check for matches
			if (HasMatch(page))
			{
				// if the doc-type matches and is NOT on HTTPS...
				if (!e.Context.Request.IsSecureConnection)
				{
					// ... then redirect the URL to HTTPS.
					e.Context.Response.Redirect(url.Replace(Settings.HTTP, Settings.HTTPS), true);
				}

				return;
			}

			// otherwise if the URL is on HTTPS...
			if (e.Context.Request.IsSecureConnection)
			{
				// ... redirect the URL back to HTTP.
				e.Context.Response.Redirect(url.Replace(Settings.HTTPS, Settings.HTTP), true);
				return;
			}
		}

		private static string StripPortFromUrl(string url, Uri contextUri)
		{
			return url.Replace(string.Format(":{0}", contextUri.Port), string.Empty);
		}

		private static bool ShouldStripPort()
		{
			bool strip;
			var appSetting = Settings.GetValueFromKey(Settings.AppKey_StripPort);

			if (!string.IsNullOrWhiteSpace(appSetting) && bool.TryParse(appSetting, out strip))
				return strip;

			return false;
		}

		private static bool HasMatch(page page)
		{
			return MatchesDocTypeAlias(page.NodeTypeAlias)
				|| MatchesNodeId(page.PageID)
				|| MatchesTemplate(page.Template)
				|| MatchesPropertyValue((page.PageID));
		}

		private static bool MatchesDocTypeAlias(string docTypeAlias)
		{
			return Settings.KeyContainsValue(Settings.AppKey_DocTypes, docTypeAlias);
		}

		private static bool MatchesNodeId(int pageId)
		{
			return Settings.KeyContainsValue(Settings.AppKey_PageIds, pageId);
		}

		private static bool MatchesTemplate(int templateId)
		{
			var template = new Template(templateId);

			return template.Id != 0 && Settings.KeyContainsValue(Settings.AppKey_Templates, template.Alias);
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

	}
}