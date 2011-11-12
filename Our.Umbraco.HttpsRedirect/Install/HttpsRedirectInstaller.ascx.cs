using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using umbraco.cms.businesslogic.web;

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
			this.cblDocTypes.DataSource = DocumentType.GetAllAsList();
			this.cblDocTypes.DataTextField = "Text";
			this.cblDocTypes.DataValueField = "Alias";
			this.cblDocTypes.DataBind();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				// populate the doc-types
				var csv = WebConfigurationManager.AppSettings[Settings.AppKey_DocTypes];
				if (!string.IsNullOrWhiteSpace(csv))
				{
					var docTypes = csv.Split(new[] { Settings.COMMA }, StringSplitOptions.RemoveEmptyEntries).ToList();

					foreach (ListItem item in this.cblDocTypes.Items)
					{
						item.Selected = docTypes.Contains(item.Value);
					}

				}

				// populate the page-ids
				var pageIds = WebConfigurationManager.AppSettings[Settings.AppKey_PageIds];
				if (!string.IsNullOrWhiteSpace(pageIds))
				{
					this.txtPageIds.Text = pageIds;
				}
			}

            // disable the dashboard control checkbox
            try
            {
                var dashboardXml = umbraco.xmlHelper.OpenAsXmlDocument(umbraco.IO.SystemFiles.DashboardConfig);
                if (dashboardXml.SelectSingleNode("//section[@alias = 'HttpsRedirectInstaller']") != null)
                {
                    this.phDashboardControl.Visible = false;
                }
            }
            catch { }
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
            else
            {
                //Clear the field if there is nothing selected
                settings.Add(Settings.AppKey_DocTypes, string.Empty);
            }

			// adds the appSettings keys for the page-ids
			settings.Add(Settings.AppKey_PageIds, this.txtPageIds.Text.Trim());

			foreach (var setting in settings)
			{
				var title = Settings.AppKeys[setting.Key];
				xml.LoadXml(string.Format("<Action runat=\"install\" undo=\"true\" alias=\"HttpsRedirect_AddAppConfigKey\" key=\"{0}\" value=\"{1}\" />", setting.Key, setting.Value));
				umbraco.cms.businesslogic.packager.PackageAction.RunPackageAction(title, "HttpsRedirect_AddAppConfigKey", xml.FirstChild);
				successes.Add(title);
			}

			if (this.cbDashboardControl.Checked)
			{
				var title = "Dashboard control";
				xml.LoadXml("<Action runat=\"install\" undo=\"true\" alias=\"addDashboardSection\" dashboardAlias=\"HttpsRedirectInstaller\"><section><areas><area>developer</area></areas><tab caption=\"HttpsRedirect: Settings\"><control>/umbraco/plugins/HttpsRedirect/HttpsRedirectInstaller.ascx</control></tab></section></Action>");
				umbraco.cms.businesslogic.packager.PackageAction.RunPackageAction(title, "addDashboardSection", xml.FirstChild);
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