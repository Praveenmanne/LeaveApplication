<%@ Assembly Name="Leave Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=56165cd852456de2" %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompoffRequestPage.aspx.cs"
    Inherits="Leave_Application.Layouts.LeaveApplication.CompoffRequestPage" DynamicMasterPageFile="~masterurl/default.master" %>

<%@ Register Src="/_CONTROLTEMPLATES/Leave_Application/CompoffRequestForm/CompoffRequestFormUserControl.ascx"
    TagName="WebUserControl1" TagPrefix="uc1" %>
<%@ Register Src="/_CONTROLTEMPLATES/Leave_Application/CompoffStatus/CompoffStatusUserControl.ascx"
    TagName="WebUserControl2" TagPrefix="uc2" %>
<%@ Register Src="/_CONTROLTEMPLATES/Leave_Application/CompoffRequests/CompoffRequestsUserControl.ascx"
    TagName="WebUserControl3" TagPrefix="uc3" %>
    <%@ Register Src="/_CONTROLTEMPLATES/Leave_Application/LeaveInfo/LeaveInfoUserControl.ascx"
    TagName="WebUserControl4" TagPrefix="uc4" %>
<asp:Content ID="PageHead" ContentPlaceHolderID="PlaceHolderAdditionalPageHead" runat="server">
</asp:Content>
<asp:Content ID="PageTitle" ContentPlaceHolderID="PlaceHolderPageTitle" runat="server">
    Compoff Page
</asp:Content>
<asp:Content ID="PageTitleInTitleArea" ContentPlaceHolderID="PlaceHolderPageTitleInTitleArea"
    runat="server">
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="PlaceHolderMain" runat="server">
    <table>
        <tr>
            <td>
                <uc1:WebUserControl1 ID="WebUserControl11" runat="server" />
            </td>
            <td>
                   <uc4:WebUserControl4 ID="WebUserControl14" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <uc2:WebUserControl2 ID="WebUserControl12" runat="server" />
            </td>
            <td>
                <uc3:WebUserControl3 ID="WebUserControl13" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
