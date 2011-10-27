using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using Our.Umbraco.HttpsRedirect;

[assembly: WebResource(Settings.ICON, Settings.PNG_MIME)]
[assembly: WebResource(Settings.LOGO, Settings.PNG_MIME)]

namespace Our.Umbraco.HttpsRedirect
{
	public class Settings
	{
		public const char COMMA = ',';

		public const string HTTP = "http://";

		public const string HTTPS = "https://";

		public const string ICON = "Our.Umbraco.HttpsRedirect.Resources.Images.icon.png";

		public const string LOGO = "Our.Umbraco.HttpsRedirect.Resources.Images.logo.png";

		public const string PNG_MIME = "image/png";

		public const string AppKey_DocTypes = "HttpsRedirect:DocTypes";

		public const string AppKey_PageIds = "HttpsRedirect:PageIds";

		public static readonly Dictionary<string, string> AppKeys = new Dictionary<string, string>()
		{
			{ AppKey_DocTypes, "Document Types" },
			{ AppKey_PageIds, "Page Ids" }
		};

		public static Version Version
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version;
			}
		}
	}
}
