using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Our.Umbraco.HttpsRedirect.Install
{
	public partial class HttpsRedirectInstaller : UserControl
	{
		protected string Logo
		{
			get
			{
				return this.Page.ClientScript.GetWebResourceUrl(typeof(HttpsRedirectInstaller), Settings.LOGO);
			}
		}

		protected void Page_Init(object sender, EventArgs e)
		{
			// bind the doc-types
			this.cblDocTypes.DataSource = Settings.AppKeys;
			this.cblDocTypes.DataTextField = "Value";
			this.cblDocTypes.DataValueField = "Key";
			this.cblDocTypes.DataBind();
		}

		protected void btnActivate_Click(object sender, EventArgs e)
		{
			var failures = new List<string>();
			var successes = new List<string>();
			
			var settings = new Dictionary<string, string>();
			var xml = new XmlDocument();
			
			// loops through the selected doc-types
			var docTypes = new List<string>();
			foreach (ListItem item in this.cblDocTypes.Items)
			{
				if (item.Selected)
				{
					docTypes.Add(item.Value);
				}
			}

			// adds the appSettings keys for the doc-types
			if (docTypes.Count > 0)
			{
				settings.Add(Settings.AppKey_DocTypes, string.Join(Settings.COMMA.ToString(), docTypes.ToArray()));
			}

			// adds the appSettings keys for the page-ids
			if (!string.IsNullOrWhiteSpace(this.txtPageIds.Text))
			{
				settings.Add(Settings.AppKey_PageIds, this.txtPageIds.Text.Trim());
			}

			foreach (var setting in settings)
			{
				var title = Settings.AppKeys[setting.Key];
				xml.LoadXml(string.Format("<Action runat=\"install\" undo=\"true\" alias=\"HttpsRedirect_AddAppConfigKey\" key=\"{0}\" value=\"{1}\" />", setting.Key, setting.Value));
				umbraco.cms.businesslogic.packager.PackageAction.RunPackageAction(title, "HttpsRedirect_AddAppConfigKey", xml.FirstChild);
				successes.Add(title);
			}


			// set the feedback controls to hidden
			this.Failure.Visible = this.Success.Visible = false;

			// display failure messages
			if (failures.Count > 0)
			{
				this.Failure.type = umbraco.uicontrols.Feedback.feedbacktype.error;
				this.Failure.Text = "There were errors with the following settings:<br />" + string.Join("<br />", failures.ToArray());
				this.Failure.Visible = true;
			}

			// display success messages
			if (successes.Count > 0)
			{
				this.Success.type = umbraco.uicontrols.Feedback.feedbacktype.success;
				this.Success.Text = "Successfully installed the following settings: " + string.Join(", ", successes.ToArray());
				this.Success.Visible = true;
			}
		}
	}
}