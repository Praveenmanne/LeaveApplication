<%@ Assembly Name="Leave Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=56165cd852456de2" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AddNewEmployeeUserControl.ascx.cs"
    Inherits="Leave_Application.AddNewEmployee.AddNewEmployeeUserControl" %>
<link href="/_layouts/LeaveApplication/StyleSheet.css" rel="stylesheet" type="text/css" />
<script src="/_layouts/LeaveApplication/jquery.min.js" type="text/javascript"></script>
<%--<script src="/_layouts/LeaveApplication/AddNewEmployee.js" type="text/javascript"></script>--%>
<script language="javascript" type="text/javascript">
    $(document).ready(function () {
        hideOrVisibleDivdop($.trim($('#<%= DdlEmptype.ClientID %>').val()));
        $('#<%= DdlEmptype.ClientID %>').change(function () {
            hideOrVisibleDivdop($.trim($(this).val()));
        });
        function hideOrVisibleDivdop(selectedValue) {
            if (selectedValue == "Permanent") {
                $('#<%= divdop.ClientID %>').show();
            } else {
                $('#<%= divdop.ClientID %>').hide();
            }
        }
    });
</script>
<div class="Container">
    <table>
        <tr class="header">
            <th colspan="2">
                <h3>
                    Please enter the following details.</h3>
            </th>
        </tr>
        <tr class="data">
            <td>
                <label>
                    Employee Id</label>
                <asp:Label runat="server" ID="lb1" Text="*" ForeColor="RED"></asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtempid"></asp:TextBox>
                <%--    <asp:RequiredFieldValidator runat="server" ID="reqEmpid"  ValidationGroup="save" ControlToValidate="txtempid" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
            </td>
        </tr>
        <tr class="data">
            <td>
                <label>
                    Employee Name</label>
                <asp:Label runat="server" ID="Label1" Text="*" ForeColor="RED"></asp:Label>
            </td>
            <td>
                <SharePoint:PeopleEditor ID="peoplepickeremp" Width="355px" Visible="true" AllowEmpty="false"
                    ValidatorEnabled="true" SelectionSet="User" MultiSelect="false" runat="server"
                    CssClass="NoBorder"></SharePoint:PeopleEditor>

            </td>
        </tr>
        <tr class="data">
            <td>
                <label>
                    First Name</label>
                <asp:Label runat="server" ID="Label2" Text="*" ForeColor="RED"></asp:Label>

            </td>
            <td>
                <asp:TextBox runat="server" ID="txtfristname"></asp:TextBox>
               <%--     <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ValidationGroup="save"  ControlToValidate="txtfristname" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
            </td>
        </tr>
        <tr class="data">
            <td>
                <label>
                    Last Name</label>
                <asp:Label runat="server" ID="Label3" Text="*" ForeColor="RED"></asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtlastname"></asp:TextBox>
              <%--      <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ValidationGroup="save" ControlToValidate="txtlastname" Display="Dynamic" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
            </td>
        </tr>
        <tr class="data">
            <td>
                <label>
                    Employee Type</label>
                <asp:Label runat="server" ID="Label4" Text="*" ForeColor="RED"></asp:Label>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="DdlEmptype" AutoPostBack="False">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="data">
            <td>
                <label>
                    Department</label>
                <asp:Label runat="server" ID="Label5" Text="*" ForeColor="RED"></asp:Label>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="DdlDep" AutoPostBack="False">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="data">
            <td>
                <label>
                    Designation</label>
                <asp:Label runat="server" ID="Label6" Text="*" ForeColor="RED"></asp:Label>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="DdlDesignation" AutoPostBack="False">
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="data">
            <td>
                <label>
                    Reporting To</label>
                <asp:Label runat="server" ID="Label7" Text="*" ForeColor="RED"></asp:Label>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="ddlReportingTo" AutoPostBack="False">
                </asp:DropDownList>
            </td>
        </tr>
        <%--<tr class="data">
            <td>
                <label>
                    Email</label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="Txtmail"></asp:TextBox>
                <asp:RegularExpressionValidator ID="regvalemailid" ControlToValidate="Txtmail" runat="server"
                    Display="Dynamic" ErrorMessage="Enter valid email id" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                    ValidationGroup="save"></asp:RegularExpressionValidator>
            </td>
        </tr>--%>
        <%-- <tr class="data">
            <td>
                <label>
                    Mobile
                </label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="TxtContact"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtContact"
                    ErrorMessage="Please enter valid Mobile number" ValidationExpression="^([0-9]{10})$"></asp:RegularExpressionValidator>
            </td>
        </tr>--%>
        <%--   <tr class="data">
            <td>
                <label>
                    Date of Birth (MM/DD/YYYY)</label>
            </td>
            <td class="NoBorder">
                <SharePoint:DateTimeControl ID="DtDOB" runat="server" DateOnly="true" MaxDate="3000-12-31"
                    LocaleId="1033" />
            </td>
        </tr>--%>
        <tr class="data">
            <td>
                <label>
                    Date of Join (MM/DD/YYYY)</label>
                <asp:Label runat="server" ID="Label8" Text="*" ForeColor="RED"></asp:Label>
            
            </td>
            <td class="NoBorder">
                <asp:Label runat="server" ID="lbldoj"></asp:Label>
                <SharePoint:DateTimeControl ID="DtDoj" runat="server" DateOnly="true" MaxDate="3000-12-31"
                    LocaleId="1033" />
            </td>
        </tr>
        <tr class="data" id="divdop" runat="server">
            <td>
                <label>
                    Date of Permanent (MM/DD/YYYY)</label>
                <asp:Label runat="server" ID="Label9" Text="*" ForeColor="RED"></asp:Label>
            </td>
            <td class="NoBorder">
                <SharePoint:DateTimeControl ID="DtDOP" runat="server" DateOnly="true" MaxDate="3000-12-31"
                    LocaleId="1033" />
            </td>
        </tr>
        <% if (!string.IsNullOrEmpty(LblError.Text.Trim()))
           {%>
        <tr class="data">
            <td colspan="2">
                <asp:Label runat="server" ID="LblError" CssClass="ErrorInfo" ForeColor="red"></asp:Label>
            </td>
        </tr>
        <% } %>
        <tr class="controls">
            <td class="noborders">
                <%-- <asp:CompareValidator ID="CompareValidator1" runat="server" ForeColor="Red" ControlToValidate="DtDoj$DtDojDate"
                    ControlToCompare="DtDOB$DtDOBDate" Type="Date" Operator="GreaterThan" ErrorMessage="* Please enter DOP Greater DOB."
                    Display="Dynamic"></asp:CompareValidator>
                <asp:CompareValidator ID="valDate" runat="server" ForeColor="Red" ControlToValidate="DtDOP$DtDOPDate"
                    ControlToCompare="DtDoj$DtDojDate" Type="Date" Operator="GreaterThanEqual" ErrorMessage="* Please enter DOP Greater or Equal to DOJ."
                    Display="Dynamic"></asp:CompareValidator>--%>
                <asp:HiddenField runat="server" ID="hdnCurrentYear"></asp:HiddenField>
                <asp:HiddenField runat="server" ID="hdnStrtFnclMnth"></asp:HiddenField>
            </td>
            <td class="noborders">
                <asp:Button runat="server" Text="Submit" ID="BtnRegister" OnClick="BtnRegisterClick" 
                    ValidationGroup="save" />
                <asp:Button runat="server" Text="Reset" ID="BtnCancel" OnClick="BtnResetClick" />
            </td>
        </tr>
    </table>
</div>
