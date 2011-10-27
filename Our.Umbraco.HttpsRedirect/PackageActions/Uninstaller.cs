using System.Text;
using System.Xml;
using umbraco.cms.businesslogic.packager;
using umbraco.cms.businesslogic.packager.standardPackageActions;
using umbraco.interfaces;

namespace Our.Umbraco.HttpsRedirect.PackageActions
{
	public class Uninstaller : IPackageAction
	{
		public string Alias()
		{
			return "HttpsRedirect_Uninstaller";
		}

		public bool Execute(string packageName, XmlNode xmlData)
		{
			return true;
		}

		public XmlNode SampleXml()
		{
			string sample = "<Action runat=\"install\" undo=\"true\" alias=\"HttpsRedirect_Uninstaller\" />";
			return helper.parseStringToXmlNode(sample);
		}

		public bool Undo(string packageName, XmlNode xmlData)
		{
			bool result = true;

			// build XML string for all the installable components
			var sb = new StringBuilder("<Actions>");
			
			// loop through each of the appSettings keys
			foreach (var appKey in Settings.AppKeys)
			{
				sb.AppendFormat("<Action runat=\"install\" undo=\"true\" alias=\"HttpsRedirect_AddAppConfigKey\" key=\"{0}\" value=\"false\" />", appKey.Key);
			}

			// remove the dashboard control (if exists)
			sb.Append("<Action runat=\"install\" undo=\"true\" alias=\"addDashboardSection\" dashboardAlias=\"HttpsRedirectInstaller\" />");

			// append the closing tag
			sb.Append("</Actions>");

			// load the XML string into an XML document
			var actionsXml = new XmlDocument();
			actionsXml.LoadXml(sb.ToString());

			// loop through each of the installable components
			foreach (XmlNode node in actionsXml.DocumentElement.SelectNodes("//Action"))
			{
				// uninstall the components
				PackageAction.UndoPackageAction("HttpsRedirect", node.Attributes["alias"].Value, node);
			}

			return result;
		}
	}
}
