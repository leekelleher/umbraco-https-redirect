using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using umbraco;
using umbraco.cms.businesslogic.packager;
using umbraco.cms.businesslogic.template;
using umbraco.cms.businesslogic.web;
using umbraco.IO;
using umbraco.uicontrols;

namespace Our.Umbraco.HttpsRedirect.Install
{
	public partial class HttpsRedirectInstaller : UserControl
	{
		public string Logo
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

			// bind the templates
			this.cblTemplates.DataSource = Template.GetAllAsList();
			this.cblTemplates.DataTextField = "Text";
			this.cblTemplates.DataValueField = "Alias";
			this.cblTemplates.DataBind();
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				// populate the doc-types
				var csv = Settings.GetValueFromKey(Settings.AppKey_DocTypes);
				if (!string.IsNullOrWhiteSpace(csv))
				{
					var docTypes = csv.Split(new[] { Settings.COMMA }, StringSplitOptions.RemoveEmptyEntries).ToList();

					foreach (ListItem item in this.cblDocTypes.Items)
					{
						item.Selected = docTypes.Contains(item.Value);
					}

				}

				// populate the templates
				csv = Settings.GetValueFromKey(Settings.AppKey_Templates);
				if (!string.IsNullOrWhiteSpace(csv))
				{
					var templates = csv.Split(new[] { Settings.COMMA }, StringSplitOptions.RemoveEmptyEntries).ToList();

					foreach (ListItem item in this.cblTemplates.Items)
					{
						item.Selected = templates.Contains(item.Value);
					}

				}

				// populate the page-ids
				var pageIds = Settings.GetValueFromKey(Settings.AppKey_PageIds);
				if (!string.IsNullOrWhiteSpace(pageIds))
				{
					this.txtPageIds.Text = pageIds;
				}

				// populate the docType Properties
                var properties = Settings.GetValueFromKey(Settings.AppKey_Properties);
                if (!string.IsNullOrWhiteSpace(properties))
                {
                    this.txtProperties.Text = properties;
                }

				// populate strip port
				this.chkStripPort.Checked = Settings.GetValueFromKey<bool>(Settings.AppKey_StripPort);

				// populate use permanent redirects
				this.chkUseTemporaryRedirects.Checked = Settings.GetValueFromKey<bool>(Settings.AppKey_UseTemporaryRedirects);
			}

			// disable the dashboard control checkbox
			try
			{
				var dashboardXml = xmlHelper.OpenAsXmlDocument(SystemFiles.DashboardConfig);
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

			// adds the appSettings keys for doctypes, templates, pageIds
			settings.Add(Settings.AppKey_DocTypes, GetStringFromCheckboxList(this.cblDocTypes));
			settings.Add(Settings.AppKey_Templates, GetStringFromCheckboxList(this.cblTemplates));
			settings.Add(Settings.AppKey_PageIds, this.txtPageIds.Text.Trim());
			settings.Add(Settings.AppKey_Properties, this.txtProperties.Text.Trim());
			settings.Add(Settings.AppKey_StripPort, this.chkStripPort.Checked.ToString());
			settings.Add(Settings.AppKey_UseTemporaryRedirects, this.chkUseTemporaryRedirects.Checked.ToString());

			foreach (var setting in settings)
			{
				var title = Settings.AppKeys[setting.Key];
				xml.LoadXml(string.Format("<Action runat=\"install\" undo=\"true\" alias=\"HttpsRedirect_AddAppConfigKey\" key=\"{0}\" value=\"{1}\" />", setting.Key, setting.Value));
				PackageAction.RunPackageAction(title, "HttpsRedirect_AddAppConfigKey", xml.FirstChild);
				successes.Add(title);
			}

			if (this.cbDashboardControl.Checked)
			{
				var title = "Dashboard control";
				xml.LoadXml("<Action runat=\"install\" undo=\"true\" alias=\"addDashboardSection\" dashboardAlias=\"HttpsRedirectInstaller\"><section><areas><area>developer</area></areas><tab caption=\"HttpsRedirect: Settings\"><control>/umbraco/plugins/HttpsRedirect/HttpsRedirectInstaller.ascx</control></tab></section></Action>");
				PackageAction.RunPackageAction(title, "addDashboardSection", xml.FirstChild);
				successes.Add(title);
			}

			// set the feedback controls to hidden
			this.Failure.Visible = this.Success.Visible = false;

			// display failure messages
			if (failures.Count > 0)
			{
				this.Failure.type = Feedback.feedbacktype.error;
				this.Failure.Text = "There were errors with the following settings:<br />" + string.Join("<br />", failures.ToArray());
				this.Failure.Visible = true;
			}

			// display success messages
			if (successes.Count > 0)
			{
				this.Success.type = Feedback.feedbacktype.success;
				this.Success.Text = "Successfully installed the following settings: " + string.Join(", ", successes.ToArray());
				this.Success.Visible = true;
			}
		}

		private string GetStringFromCheckboxList(CheckBoxList cbl)
		{
			// loops through the selected list items
			var selectedItems = new List<string>();
			foreach (ListItem item in cbl.Items)
			{
				if (item.Selected)
				{
					selectedItems.Add(item.Value);
				}
			}

			// create a csv string
			if (selectedItems.Count > 0)
			{
				return string.Join(Settings.COMMA.ToString(), selectedItems.ToArray());
			}

			// clear the field if there is nothing selected
			return string.Empty;
		}
	}
}