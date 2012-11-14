<%@ Assembly Name="Leave Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=56165cd852456de2" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeLeavesListUserControl.ascx.cs"
    Inherits="Leave_Application.EmployeeLeavesList.EmployeeLeavesListUserControl" %>
<link href="/_layouts/LeaveApplication/StyleSheet.css" rel="stylesheet" type="text/css" />
<script src="/_layouts/LeaveApplication/SPOpenDialog.js" type="text/javascript"></script>
<%--<h3>
    My Leave</h3>--%>
<% System.Data.DataTable result = ViewState["Result"] as System.Data.DataTable;

   if (result != null && result.Rows.Count > 0)
   {
%>
<div class="Container">
    <table>
        <tr class="header">
            <th  colspan="9"><h3> My Leave</h3></th>
        </tr>
        <tr class="header">
            <th>
                Requested To
            </th>
            <th>
                Leave Type
            </th>
            <th>
                Start Date
            </th>
            <th>
                End Date
            </th>
            <th>
                Leave Days
            </th>
            <th>
                Reason
            </th>
            <th>
                Remarks
            </th>
            <th>
                Status
            </th>
            <th>
                Cancel
            </th>
        </tr>
        <% foreach (System.Data.DataRow row in result.Rows)
           {%>
        <tr class="data">
            <td>
                <%=row["Requested To"].ToString()%>
            </td>
            <td>
                <%=row["Leave Type"].ToString()%>
            </td>
            <td>
                <%=row["Starting Date"].ToString()%>
            </td>
            <td>
                <%=row["Ending Date"].ToString()%>
            </td>
            <td>
                <%=row["Leave Days"].ToString()%>
            </td>
            <td>
                
                <div class="wordWrap">
                    <%=row["Reason"].ToString()%></div>
            </td>
            <td>
                <%=row["Remarks"].ToString() %>
            </td>
             <td>
                <%if (row["Status"].ToString().Trim() == "Pending")
                  {%>
                <div style="color:#e84743 ">
                    <%= row["Status"].ToString()%></div>
                <% }
                  else
                  { %>
                <div>
                    <%= row["Status"].ToString()%></div>
                <% }%>
            </td>

            <td>
                <%=row["Cancel"].ToString() %>
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
                There is no Leave Requests.
            </th>
        </tr>
    </table>
</div>
<% } %><br />
<br />
<asp:HiddenField runat="server" ID="hdnCurrentYear"></asp:HiddenField>
