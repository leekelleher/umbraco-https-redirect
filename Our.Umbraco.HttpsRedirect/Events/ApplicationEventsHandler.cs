using System;
using System.Linq;
using System.Web.Configuration;
using umbraco;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic;
using umbraco.cms.businesslogic.web;

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
			const string HTTP = "http://";
			const string HTTPS = "https://";
			var url = e.Context.Request.Url.ToString().ToLower();

			// check for matches
			if (this.HasMatch(e.Page))
			{
				// if the doc-type matches and is NOT on HTTPS...
				if (!e.Context.Request.IsSecureConnection)
				{
					// ... then redirect the URL to HTTPS.
					e.Context.Response.Redirect(url.Replace(HTTP, HTTPS), true);
					return;
				}
			}

			// otherwise if the URL is on HTTPS...
			if (e.Context.Request.IsSecureConnection)
			{
				// ... redirect the URL back to HTTP.
				e.Context.Response.Redirect(url.Replace(HTTPS, HTTP), true);
				return;
			}
		}

		private bool HasMatch(page page)
		{
			return MatchesDocTypeAlias(page.NodeTypeAlias) || MatchesNodeId(page.PageID);
		}

		private bool MatchesDocTypeAlias(string docTypeAlias)
		{
			var appSetting = WebConfigurationManager.AppSettings["HttpsRedirect:DocTypes"];
			if (!string.IsNullOrEmpty(appSetting))
			{
				var docTypes = appSetting.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim());
				if (docTypes != null && docTypes.Contains(docTypeAlias))
				{
					return true;
				}
			}

			return false;
		}

		private bool MatchesNodeId(int pageId)
		{
			var appSetting = WebConfigurationManager.AppSettings["HttpsRedirect:PageIds"];
			if (!string.IsNullOrEmpty(appSetting))
			{
				var pageIds = Array.ConvertAll(appSetting.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries), int.Parse);

				if (pageIds != null && pageIds.Contains(pageId))
				{
					return true;
				}
			}

			return false;
		}
	}
}
