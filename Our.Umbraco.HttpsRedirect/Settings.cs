using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		public const string CHECKBOX_TRUE = "1";

		public const char COLON = ':';

		public const char COMMA = ',';

		public const string HTTP = "http://";

		public const string HTTPS = "https://";

		public const string ICON = "Our.Umbraco.HttpsRedirect.Resources.Images.icon.png";

		public const string LOGO = "Our.Umbraco.HttpsRedirect.Resources.Images.logo.png";

		public const string PNG_MIME = "image/png";

		public const string AppKey_DocTypes = "HttpsRedirect:DocTypes";

		public const string AppKey_PageIds = "HttpsRedirect:PageIds";

		public const string AppKey_StripPort = "HttpsRedirect:StripPort";

		public const string AppKey_Properties = "HttpsRedirect:Properties";

		public const string AppKey_Templates = "HttpsRedirect:Templates";

		public const string AppKey_UsePermanentRedirects = "HttpsRedirect:UsePermanentRedirects";

		public static readonly Dictionary<string, string> AppKeys = new Dictionary<string, string>()
		{
			{ AppKey_DocTypes, "Document Types" },
			{ AppKey_PageIds, "Page Ids" },
			{ AppKey_Templates, "Templates" },
			{ AppKey_Properties, "Properties" },
			{ AppKey_StripPort, "Strip Port" },
			{ AppKey_UsePermanentRedirects, "Permanent Redirects" },
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
			return GetValueFromKey<string>(appKey);
		}

		public static T GetValueFromKey<T>(string appKey)
		{
			var appKeyValue = WebConfigurationManager.AppSettings[appKey] ?? string.Empty;
			var typeConverter = TypeDescriptor.GetConverter(typeof(T));

			if (typeof (T) == typeof (bool))
			{
				if (appKeyValue == "1")
					return (T)(object) true;

				if (appKeyValue == "0")
					return (T) (object) false;

				bool result = false;
				bool.TryParse(appKeyValue, out result);

				return (T) (object) result;
			} 

			return (T) typeConverter.ConvertFrom(appKeyValue);
		}

		public static bool KeyContainsValue(string appKey, object value)
		{
			if (string.IsNullOrWhiteSpace(appKey))
				return false;

			var appSetting = GetValueFromKey(appKey);
			if (string.IsNullOrWhiteSpace(appSetting))
				return false;

			var values = appSetting.Split(new[] { COMMA }, StringSplitOptions.RemoveEmptyEntries);

			if (value is int)
			{
				var pageIds = Array.ConvertAll(values, int.Parse);
				return pageIds.Contains((int)value);
			}

			return values.Contains(value);
		}
	}
}