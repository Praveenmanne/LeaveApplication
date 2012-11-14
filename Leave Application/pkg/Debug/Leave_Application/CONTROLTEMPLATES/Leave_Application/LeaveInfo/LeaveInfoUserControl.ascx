<%@ Assembly Name="Leave Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=56165cd852456de2" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LeaveInfoUserControl.ascx.cs"
    Inherits="Leave_Application.LeaveInfo.LeaveInfoUserControl" %>
<link href="/_layouts/LeaveApplication/StyleSheet.css" rel="stylesheet" type="text/css" />
<%--<h3>
    Leave Info</h3>--%>
<% System.Data.DataTable result = ViewState["Result"] as System.Data.DataTable;

   if (result != null && result.Rows.Count> 0)
   {
%>
<div class="Container">
    <table>
         <tr class="header">
            
            <th colspan="4"><h3>Leave Information</h3></th></tr>
        <tr class="header">
            <th>
                Leave Type
            </th>
            <th>
                Leave Balance
            </th>
            <th>
                Leave Requested
            </th>
            <th>
                Leave Utilized
            </th>
        </tr>
        <% foreach (System.Data.DataRow row in result.Rows)
           {%>
        <tr class="data">
            <td>
                <%=row["Leave Type"].ToString()%>
            </td>
            <td>
                <%=row["Balance Leave"].ToString()%>
            </td>
            <td>
                <%=row["Leave Requested"].ToString()%>
            </td>
            <td>
                <%=row["Leave utilized"].ToString()%>
            </td>
        </tr>
        <% } %>
    </table>
</div>
<% }
   else
   { %>
<div class="Container">
    <table>
        <tr class="data">
            <td colspan="2">
                <asp:Label ID="lblErr" runat="server" ForeColor="red"></asp:Label>
            </td>
        </tr>
        <tr class="header">
            <th>
                There is no records.
            </th>
        </tr>
    </table>
</div>
<% } %><br />
<br />
<%-- <asp:GridView runat="server" ID="grvBalanceLeave">
    </asp:GridView>--%>
<asp:HiddenField runat="server" ID="hdnCurrentUsername" />
<asp:HiddenField runat="server" ID="hdnCurrentYear"></asp:HiddenField>
