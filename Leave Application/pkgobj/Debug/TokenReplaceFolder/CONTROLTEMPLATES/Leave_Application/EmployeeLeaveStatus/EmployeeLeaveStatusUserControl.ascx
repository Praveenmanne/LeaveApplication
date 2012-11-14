<%@ Assembly Name="Leave Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=56165cd852456de2" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmployeeLeaveStatusUserControl.ascx.cs"
    Inherits="Leave_Application.EmployeeLeaveStatus.EmployeeLeaveStatusUserControl" %>
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
        <tr class="header"><th colspan="9"><h3> Leave Requests</h3></th></tr>
        <tr class="header">
            <th>
                <input type="checkbox" onclick="checkAll(this);" />
            </th>
            <th>
                Requested From
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
                Duration
             </th>
            <th>
                Reason
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
                <%= dataRow["Leave Type"].ToString()%>
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
                <input type="text" style="width: 70px" name='<%= "txt" + dataRow["Id"].ToString() %>'
                    id='<%= "txt" + dataRow["Id"].ToString() %>' />
            </td>
        </tr>
        <%} %>
    </table>
    <!--
    <asp:GridView runat="server" EnableModelValidation="True" ID="grid" AutoGenerateColumns="False"
        Width="600">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="Id" HeaderStyle-CssClass="hideGridColumn"
                ItemStyle-CssClass="hideGridColumn" />
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:CheckBox ID="checkAll" runat="server" onclick="checkAll(this);" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:CheckBox ID="chkbox" runat="server" onclick="Check_Click(this)" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="RequestedFrom" HeaderText="Requested From" />
            <asp:BoundField DataField="Leave Type" HeaderText="Leave Type" />
            <asp:BoundField DataField="Starting Date" HeaderText="Starting Date" />
            <asp:BoundField DataField="Ending Date" HeaderText="Ending Date" />
            <asp:BoundField DataField="Duration" HeaderText="Duration" />
            <asp:BoundField DataField="Reason" HeaderText="Reason" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:TemplateField>
                <HeaderTemplate>
                    <header> Remark </header>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:TextBox ID="txtreason" runat="server" Width="125px" Height="50px" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView> -->
    <ul>
        <li>
            <div class="controls">
                <asp:Button runat="server" Text="Approve" ID="BtnApprove" OnClick="BtnApproveClick" />
                <asp:Button runat="server" Text="Reject" ID="BtnReject" OnClick="BtnRejectClick" />
                <asp:Label runat="server" ID="lblErr"></asp:Label>
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
                There is no pending leave requests to approve.
            </th>
        </tr>
    </table>
</div>
<% } %><br />
<br />
