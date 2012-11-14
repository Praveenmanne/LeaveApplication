<%@ Assembly Name="Leave Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=56165cd852456de2" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PendingStatusUserControl.ascx.cs" Inherits="Leave_Application.PendingStatus.PendingStatusUserControl" %>
<asp:HiddenField runat="server" ID="hdnCurrentYear"></asp:HiddenField>
 <asp:Label runat="server" ID="lblErr"></asp:Label>
<% System.Data.DataTable result = ViewState["Result"] as System.Data.DataTable;

   if (result != null && result.Rows.Count> 0)
   {
%>
<div class="Container">
    <table>
        <tr class="header">
            <% foreach (System.Data.DataColumn column in result.Columns)
               {%>
            <th>
                <%=column.ColumnName %>
            </th>
            <% } %>
        </tr>
        <% foreach (System.Data.DataRow row in result.Rows)
           {%>
        <tr class="data">
            <% foreach (System.Data.DataColumn column in result.Columns)
               {%>
            <td>
                <%= row[column.ColumnName].ToString() %>
            </td>
            <% } %>
        </tr>
        <% } %>
    </table>
</div>
<% } %>
<br />
