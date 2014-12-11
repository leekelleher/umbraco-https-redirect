using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;
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

		public const string AppKey_StripPort = "HttpsRedirect:StripPort";

		public const string AppKey_Templates = "HttpsRedirect:Templates";

        public const string AppKey_XForwardedProto = "HttpsRedirect:XForwardedProto";

		public static readonly Dictionary<string, string> AppKeys = new Dictionary<string, string>()
		{
			{ AppKey_DocTypes, "Document Types" },
			{ AppKey_PageIds, "Page Ids" },
			{ AppKey_Templates, "Templates" },
			{ AppKey_StripPort, "Strip Port" },
            { AppKey_XForwardedProto, "X-Forwarded-Proto" }
		};

		public static Version Version
		{
			get
			{
				return Assembly.GetExecutingAssembly().GetName().Version;
			}
		}

		public static string GetValueFromKey(string appKey)
		{
			return WebConfigurationManager.AppSettings[appKey];
		}

		public static bool KeyContainsValue(string appKey, object value)
		{
			if (!string.IsNullOrWhiteSpace(appKey))
			{
				var appSetting = GetValueFromKey(appKey);
				if (!string.IsNullOrWhiteSpace(appSetting))
				{
					var values = appSetting.Split(new[] { COMMA }, StringSplitOptions.RemoveEmptyEntries);

					if (value is int)
					{
						var pageIds = Array.ConvertAll(values, int.Parse);
						return pageIds.Contains((int)value);
					}

					return values.Contains(value);
				}
			}

			return false;
		}
	}
}