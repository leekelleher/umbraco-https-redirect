using System.Configuration;
using System.Web.Configuration;
using System.Xml;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;

namespace Our.Umbraco.HttpsRedirect.PackageActions
{
	public class AddAppConfigKey : IPackageAction
	{
		public string Alias()
		{
			return "HttpsRedirect_AddAppConfigKey";
		}

		public bool Execute(string packageName, XmlNode xmlData)
		{
			try
			{
				string addKey = xmlData.Attributes["key"].Value;
				string addValue = xmlData.Attributes["value"].Value;

				// as long as addKey has a value, create the key entry in web.config
				if (addKey != string.Empty)
				{
					this.CreateAppSettingsKey(addKey, addValue);
				}

				return true;
			}
			catch
			{
				return false;
			}
		}

		public XmlNode SampleXml()
		{
			return helper.parseStringToXmlNode("<Action runat=\"install\" undo=\"true/false\" alias=\"HttpsRedirect_AddAppConfigKey\" key=\"your key\" value=\"your value\" />");
		}

		public bool Undo(string packageName, XmlNode xmlData)
		{
			try
			{
				string addKey = xmlData.Attributes["key"].Value;

				// as long as addKey has a value, remove it from the key entry in web.config
				if (addKey != string.Empty)
				{
					this.RemoveAppSettingsKey(addKey);
				}

				return true;
			}
			catch
			{
				return false;
			}
		}

		private void CreateAppSettingsKey(string key, string value)
		{
			var config = WebConfigurationManager.OpenWebConfiguration("~");
			var appSettings = (AppSettingsSection)config.GetSection("appSettings");

			appSettings.Settings.Remove(key);
			appSettings.Settings.Add(key, value);

			config.Save(ConfigurationSaveMode.Modified);
		}

		private void RemoveAppSettingsKey(string key)
		{
			var config = WebConfigurationManager.OpenWebConfiguration("~");
			var appSettings = (AppSettingsSection)config.GetSection("appSettings");

			appSettings.Settings.Remove(key);

			config.Save(ConfigurationSaveMode.Modified);
		}
	}
}
