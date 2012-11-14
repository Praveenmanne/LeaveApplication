<%@ Assembly Name="Leave Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=56165cd852456de2" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeInformationUserControl.ascx.cs"
    Inherits="Leave_Application.EmployeeInformation.EmployeeInformationUserControl" %>
<link href="/_layouts/LeaveApplication/StyleSheet.css" rel="stylesheet" type="text/css" />
<script src="/_layouts/LeaveApplication/SPOpenDialog.js" type="text/javascript"></script>
<% System.Data.DataTable result = ViewState["Result"] as System.Data.DataTable;

   if (result != null && result.Rows.Count> 0)
   {
%>
<div class="Container">
    <table>
        <tr class="header">
            <th>
                Employee Id
            </th>
            <th>
                Employee Name
            </th>
            <th>
                Employee Type
            </th>
            <%--<th>
                Sum of Leave Balance
            </th>--%>
            <th>
                Edit
            </th>
        </tr>
        <% foreach (System.Data.DataRow row in result.Rows)
           {%>
        <tr class="data">
            <td>
                <%=row["Employee Id"].ToString()%>
            </td>
            <td>
                <%=row["Employee Name"].ToString()%>
            </td>
            <td>
                <%=row["Employee Type"].ToString()%>
            </td>
            <%--<td>
                <%=row["Sum of Balance Leave"].ToString()%>
            </td>--%>
            <td>
                <%=row["Edit"].ToString()%>
            </td>
        </tr>
        <% } %>
        <tr class="newOption">
            <td>
                <a href="JavaScript:openDialog('<%= SPContext.Current.Site.Url %>/_layouts/LeaveApplication/AddNewEmployees.aspx');">
                    New Employee</a>
            </td>
        </tr>
    </table>
</div>
<% }
   else
   { %>
<div class="Container">
    <table>
        <tr class="header">
            <th>
                There is no employee list.
            </th>
        </tr>
        <tr class="newOption">
            <td>
                <a href="JavaScript:openDialog('<%= SPContext.Current.Site.Url %>/_layouts/LeaveApplication/AddNewEmployees.aspx');">
                    Add
                New Employee</a>
            </td>
        </tr>
    </table>
</div>
<% } %><br />
<br />
<asp:Label runat="server" ID="lblError"></asp:Label>
<asp:HiddenField runat="server" ID="hdncurrentURl" />
<asp:HiddenField runat="server" ID="hdnCurrentYear"></asp:HiddenField>
