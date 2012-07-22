using System;
using System.Linq;
using System.Web.Configuration;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.template;

namespace Our.Umbraco.HttpsRedirect.Events
{
	public class EventsHandler : ApplicationBase
	{
		public EventsHandler()
		{
			UmbracoDefault.BeforeRequestInit += new UmbracoDefault.RequestInitEventHandler(this.UmbracoDefault_BeforeRequestInit);
		}

		private void UmbracoDefault_BeforeRequestInit(object sender, RequestInitEventArgs e)
		{
			var url = e.Context.Request.Url.ToString().ToLower();

			var page = e.Page;

			if (page == null)
				return;

			// check for matches
			if (this.HasMatch(page))
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

		private bool HasMatch(page page)
		{
			return MatchesDocTypeAlias(page.NodeTypeAlias) || MatchesNodeId(page.PageID) || MatchesTemplate(page.Template);
		}

		private bool MatchesDocTypeAlias(string docTypeAlias)
		{
			return Settings.KeyContainsValue(Settings.AppKey_DocTypes, docTypeAlias);
		}

		private bool MatchesNodeId(int pageId)
		{
			return Settings.KeyContainsValue(Settings.AppKey_PageIds, pageId);
		}

		private bool MatchesTemplate(int templateId)
		{
			var template = new Template(templateId);

			if (template.Id == 0)
				return false;

			return Settings.KeyContainsValue(Settings.AppKey_Templates, template.Alias);
		}

	}
}
