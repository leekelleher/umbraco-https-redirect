﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HttpsRedirectInstaller.ascx.cs" Inherits="Our.Umbraco.HttpsRedirect.Install.HttpsRedirectInstaller" EnableViewState="false" %>
<%@ Register Assembly="controls" Namespace="umbraco.uicontrols" TagPrefix="umb" %>

<div style="padding: 10px 10px 0;">

	<p><img src="<%= Logo %>" alt="HTTPS Redirect" /></p>

	<asp:PlaceHolder runat="server" ID="phInstaller">
		
		<umb:Feedback runat="server" ID="Success" type="success" Text="HTTPS Redirect successfully installed!" />
		<umb:Feedback runat="server" ID="Failure" type="error" Visible="false" />
		
		<p>Now that <strong>HTTPS Redirect</strong> has been installed, you can configure the settings.</p>
		
		<div style="float: left; margin-right: 15px;">
			<h2>Document Types</h2>
			<p>Select the document-types that will redirect to HTTPS:</p>
			<asp:CheckBoxList runat="server" ID="cblDocTypes"></asp:CheckBoxList>
		</div>
		
		<div style="float: left;">
			<h2>Templates</h2>
			<p>Select the templates that will redirect to HTTPS:</p>
			<asp:CheckBoxList runat="server" ID="cblTemplates"></asp:CheckBoxList>
		</div>
		
		<br style="clear: both;" />

		<h2>Page/Node Ids</h2>
		<p>Enter a comma-separated list of specific page/node ids:</p>
		<asp:TextBox runat="server" ID="txtPageIds" CssClass="umbEditorTextField" />

		<br style="clear: both;" />

		<h2>Strip Port Number</h2>
		<p>Choose whether the port number portion of the URL should be stripped. Useful if you use non-standard ports internal to your firewall:</p>
		<asp:CheckBox runat="server" ID="chkStripPort" Text="Strip port numbers?" />

		<br style="clear: both;" />

        <h2>Load Balancer with SSL Termination</h2>
        <p>Select if you are using a load balancer with SSL termination that adds the X-Forwarded-Proto header.</p>
        <asp:CheckBox runat="server" ID="chkXForwardedProto" Text="Check for X-Forwarded-Proto Header" />

        <br style="clear: both;" />

		<asp:PlaceHolder runat="server" ID="phDashboardControl">
			<h2>Dashboard control</h2>
			<p>If you would like to revisit this screen in future, you can add it as a dashboard control to the Developer section.</p>
			<asp:CheckBox runat="server" ID="cbDashboardControl" Text="Add as dashboard control?" />
		</asp:PlaceHolder>

		<p>
			<asp:Button id="btnInstall" runat="server" Text="Save configuration settings" onclick="btnActivate_Click" onclientclick="jQuery(this).hide(); jQuery('#installingMessage').show(); return true;" />
			<div style="display: none;" id="installingMessage">
				<umb:ProgressBar runat="server" />
				<br />
				<em>&nbsp; &nbsp;Saving configuration settings, please wait...</em>
				<br />
			</div>
		</p>

	</asp:PlaceHolder>

</div>