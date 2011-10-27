<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HttpsRedirectInstaller.ascx.cs" Inherits="Our.Umbraco.HttpsRedirect.Install.HttpsRedirectInstaller" EnableViewState="false" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umb" %>

<div style="padding: 10px 10px 0;">

	<p><img src="<%= Logo %>" alt="HTTPS Redirect" /></p>

	<asp:PlaceHolder runat="server">
		
		<umb:Feedback runat="server" ID="Success" type="success" Text="HTTPS Redirect successfully installed!" />
		<umb:Feedback runat="server" ID="Failure" type="error" Visible="false" />
		
		<p>Now that <strong>HTTPS Redirect</strong> has been installed, you can configure the settings.</p>

		<h2>Document Types</h2>
		<asp:CheckBoxList runat="server" ID="cblDocTypes"></asp:CheckBoxList>

		<h2>Page/Node Ids</h2>
		<asp:TextBox runat="server" ID="txtPageIds" />

		<p>
            <asp:button id="btnInstall" runat="server" Text="Save configuration settings" onclick="btnActivate_Click" onclientclick="jQuery(this).hide(); jQuery('#installingMessage').show(); return true;" />
            <div style="display: none;" id="installingMessage">
                <umb:ProgressBar runat="server" />
                <br />
                <em>&nbsp; &nbsp;Saving configuration settings, please wait...</em><br />
            </div>
        </p>

	</asp:PlaceHolder>

</div>