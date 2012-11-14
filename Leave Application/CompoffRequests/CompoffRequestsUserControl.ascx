<%@ Assembly Name="Leave Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=56165cd852456de2" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CompoffRequestsUserControl.ascx.cs" Inherits="Leave_Application.CompoffRequests.CompoffRequestsUserControl" %>

<link href="/_layouts/LeaveApplication/StyleSheet.css" rel="stylesheet" type="text/css" />
<script src="/_layouts/LeaveApplication/LeaveStatus.js" type="text/javascript"></script>
<%--<h3>
    Leave Requests</h3>--%>
<% DataTable results = (DataTable)ViewState["Results"];
   if (results != null && results.Rows.Count> 0)
   {
%>
<div class="Container">
    <table>
        <tr class="header"><th colspan="9">  Compoff Requests</th></tr>
        <tr class="header">
            <th>
                <input type="checkbox" onclick="checkAll(this);" />
            </th>
            <th>
                Requested From
            </th>
            <th>
                Start Date
            </th>
            <th>
                End Date
            </th>
            <th>
                Duration
             </th>
            <th>
               Worked For
            </th>
            <th>
                Status
            </th>
            <th>
                Remarks
            </th>
        </tr>
        <% foreach (DataRow dataRow in results.Rows)
           {
        %>
        <tr>
            <td>
                <input type="checkbox" onclick="Check_Click(this);" name='<%= "Chk" + dataRow["Id"].ToString() %>' />
            </td>
            <td>
                <%= dataRow["RequestedFrom"].ToString() %>
            </td>
           
            <td>
                <%= dataRow["Starting Date"].ToString()%>
            </td>
            <td>
                <%= dataRow["Ending Date"].ToString()%>
            </td>
            <td>
                <%= dataRow["Duration"].ToString()%>
            </td>
            <td>
                  <div class="wordWrap">
                <%= dataRow["Reason"].ToString()%></div>
            </td>
            <td>
                <%if (dataRow["Status"].ToString().Trim() == "Pending")
                  {%>
                <div style="color:#e84743 ">
                    <%= dataRow["Status"].ToString()%></div>
                <% }
                  else
                  { %>
                <div>
                    <%= dataRow["Status"].ToString()%></div>
                <% }%>
            </td>
            <td>
                <input type="text" style="width: 80px" name='<%= "txt" + dataRow["Id"].ToString() %>'
                    id='<%= "txt" + dataRow["Id"].ToString() %>' />
            </td>
        </tr>
        <%} %>
    </table>
   
    <ul>
        <li>
            <div class="controls">
                <asp:Button runat="server" Text="Approve" ID="BtnApprove" 
                    onclick="BtnApprove_Click" />
                <asp:Button runat="server" Text="Reject" ID="BtnReject" 
                    onclick="BtnReject_Click"  />
                <asp:Label runat="server" ID="lblErr" ForeColor="red"></asp:Label>
                <asp:HiddenField runat="server" ID="hdnCurrentYear"></asp:HiddenField>
            </div>
        </li>
    </ul>
</div>
<% }
   else
   { %>
<div class="Container">
    <table>
        <tr class="header">
            <th>
                There is no records.
            </th>
        </tr>
    </table>
</div>
<% } %><br />
<br />

