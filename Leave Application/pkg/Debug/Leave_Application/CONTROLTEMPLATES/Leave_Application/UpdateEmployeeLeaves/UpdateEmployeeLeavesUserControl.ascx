<%@ Assembly Name="Leave Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=56165cd852456de2" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateEmployeeLeavesUserControl.ascx.cs"
    Inherits="Leave_Application.UpdateEmployeeLeaves.UpdateEmployeeLeavesUserControl" %>
<link href="/_layouts/LeaveApplication/StyleSheet.css" rel="stylesheet" type="text/css" />
<asp:Button runat="server" ID="btnCalculate" Text="View Report" OnClick="btnCalculate_Click" />
<asp:Button runat="server" ID="btnUpdate" Text="Update" OnClick="btnUpdate_Click" />
<asp:HiddenField runat="server" ID="hdnCurrentYear"></asp:HiddenField>
<asp:HiddenField runat="server" ID="hdnUpdatingMonth"></asp:HiddenField>
<asp:Label runat="server" ID="lblError"></asp:Label>

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


<% System.Data.DataTable pendingResult = ViewState["PendingResult"] as System.Data.DataTable;

   if (pendingResult != null && pendingResult.Rows.Count> 0)
   {
%>
<div class="Container">
    <table>
        <tr class="header">
            <% foreach (System.Data.DataColumn column in pendingResult.Columns)
               {%>
            <th>
                <%=column.ColumnName %>
            </th>
            <% } %>
        </tr>
        <% foreach (System.Data.DataRow row in pendingResult.Rows)
           {%>
        <tr class="data">
            <% foreach (System.Data.DataColumn column in pendingResult.Columns)
               {%>
            <td>
                <%= row[column.ColumnName].ToString() %>
            </td>
            <% } %>
        </tr>
        <% } %>
    </table>
</div>
<% } %><br />
<br />



<%--
<asp:GridView runat="server" ID="grdvwEmployeeLeaves">
</asp:GridView>--%>
