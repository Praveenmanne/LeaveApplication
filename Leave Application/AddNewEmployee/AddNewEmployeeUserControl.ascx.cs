using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Leave_Application.AddNewEmployee
{
    public partial class AddNewEmployeeUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DtDoj.MaxDate = DateTime.Now;
            // DtDOB.MaxDate = DateTime.Now;
            DtDOP.MaxDate = DateTime.Now;

            try
            {


                if (!Page.IsPostBack)
                {
                    txtempid.Focus();
                    //divdop.Visible = false;
                    try
                    {
                        var deplist = SPContext.Current.Web.Lists.TryGetList(Utilities.Department);

                        if (deplist != null)
                        {
                            DdlDep.Items.Clear();
                            DdlDep.Items.Add("--Select--");
                            var collection = deplist.GetItems();
                            foreach (SPListItem item in collection)
                            {
                                DdlDep.Items.Add(item["Department"].ToString());
                            }
                        }
                        var desiglist = SPContext.Current.Web.Lists.TryGetList(Utilities.Designation);

                        if (desiglist != null)
                        {
                            DdlDesignation.Items.Clear();
                            DdlDesignation.Items.Add("--Select--");
                            var collection = desiglist.GetItems();
                            foreach (SPListItem item in collection)
                            {
                                DdlDesignation.Items.Add(item["Designation"].ToString());
                            }
                        }
                        var leavetypelist = SPContext.Current.Web.Lists.TryGetList(Utilities.EmployeeType);

                        if (leavetypelist != null)
                        {
                            DdlEmptype.Items.Clear();
                            DdlEmptype.Items.Add("--Select--");
                            var collection = leavetypelist.GetItems();
                            foreach (SPListItem item in collection)
                            {
                                DdlEmptype.Items.Add(item["Title"].ToString());
                            }
                        }
                        var managerlist = SPContext.Current.Web.Lists.TryGetList(Utilities.ReportingTo);

                        if (managerlist != null)
                        {
                            ddlReportingTo.Items.Clear();
                            ddlReportingTo.Items.Add("--Select--");
                            var collection = managerlist.GetItems();
                            foreach (SPListItem item in collection)
                            {
                                var spv = new SPFieldLookupValue(item["Reporting Managers"].ToString());
                                var listItem = new ListItem(spv.LookupValue,
                                                                 item["Reporting Managers"].ToString());
                                ddlReportingTo.Items.Add(listItem);
                            }
                        }
                        var currentYear = SPContext.Current.Web.Lists.TryGetList(Utilities.CurrentYear).GetItems();
                        foreach (SPListItem currentYearValue in currentYear)
                        {
                            hdnCurrentYear.Value = currentYearValue["Title"].ToString();
                        }
                        var financialStartMont = SPContext.Current.Web.Lists.TryGetList(Utilities.Financialstartmonth).GetItems();
                        foreach (SPListItem finStrtmonth in financialStartMont)
                        {
                            hdnStrtFnclMnth.Value = finStrtmonth["Title"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        LblError.Text = ex.Message;
                    }
                }

            }
            catch (Exception)
            {


            }
            //empGridView.DataSource = LoadEmpDetails();
            //empGridView.DataBind();
        }

        protected void BtnRegisterClick(object sender, EventArgs e)
        {
            string dOP = ((TextBox)(DtDOP.Controls[0])).Text;
            string dOJ = ((TextBox)(DtDoj.Controls[0])).Text;
            // string dOB = ((TextBox)(DtDOB.Controls[0])).Text;

            if (SPContext.Current != null)
            {
                if (txtfristname.Text == "")
                {
                    LblError.Text = "All Fields are required field.";
                    LblError.Text += "<br/>";
                    return;
                }
                if (txtlastname.Text == "")
                {
                    LblError.Text = "All Fields are required field.";
                    LblError.Text += "<br/>";
                    return;
                }
                if (txtempid.Text == "")
                {
                    LblError.Text = "All Fields are required field.";
                    LblError.Text += "<br/>";
                    return;
                }
                if (DdlDep.SelectedIndex < 1)
                {
                    LblError.Text = "All Fields are required field.";
                    LblError.Text += "<br/>";
                    return;
                }
                if (DdlDesignation.SelectedIndex < 1)
                {
                    LblError.Text = "All Fields are required field.";
                    LblError.Text += "<br/>";
                    return;
                }

                if (DdlEmptype.SelectedIndex < 1)
                {
                    LblError.Text = "All Fields are required field.";
                    LblError.Text += "<br/>";
                    return;
                }

              

                //if (!ValidateDate(dOB, "MM/dd/yyyy"))
                //{
                //    LblError.Text = "Date format should be in 'MM/DD/YYYY' format.";
                //    LblError.Text += "<br/>";
                //    DtDOB.ClearSelection();
                //    DtDOB.Focus();
                //    return;

                //}
                //else if (DateTime.Parse(dOB) > DateTime.Now)
                //{
                //    LblError.Text = "DOB should be Less than or equal to today.";
                //    LblError.Text += "<br/>";
                //    divdop.Visible = true;
                //    DtDOB.ClearSelection();
                //    DtDOB.Focus();
                //    return;
                //}
                if (string.IsNullOrEmpty(dOJ))
                {
                    LblError.Text = "All Fields are required field.";
                    LblError.Text += "<br/>";
               //     divdop.Visible = true;
                    return;
                }
                if (!ValidateDate(dOJ, "MM/dd/yyyy"))
                {
                    LblError.Text = "Date format should be in 'MM/DD/YYYY' format.";
                    LblError.Text += "<br/>";
                    DtDoj.ClearSelection();
                    DtDoj.Focus();
                    return;

                }
                else if (DateTime.Parse(dOJ) > DateTime.Now)
                {
                    LblError.Text = "DOJ should be Less than or equal to today.";
                    LblError.Text += "<br/>";
                   // divdop.Visible = true;
                    DtDoj.ClearSelection();
                    DtDoj.Focus();
                    return;
                }
                if (DdlEmptype.SelectedItem.Value.Trim() == "Permanent")
                {

                    if (string.IsNullOrEmpty(dOP))
                    {
                        LblError.Text = "All Fields are required field.";
                        LblError.Text += "<br/>";
                        divdop.Visible = true;
                        return;
                    }
                    else if (!ValidateDate(dOP, "MM/dd/yyyy"))
                    {
                        LblError.Text = "Date format should be in 'MM/DD/YYYY' format.";
                        LblError.Text += "<br/>";
                        divdop.Visible = true;
                        DtDOP.ClearSelection();
                        DtDOP.Focus();
                        return;

                    }
                    else if (DateTime.Parse(dOP) > DateTime.Now)
                    {
                        LblError.Text = "DOP should be Less than or equal to today.";
                        LblError.Text += "<br/>";
                        divdop.Visible = true;
                        DtDOP.ClearSelection();
                        DtDOP.Focus();
                        return;
                    }
                    else if (DateTime.Parse(dOP) < DateTime.Parse(dOJ))
                    {
                        LblError.Text = "DOP should be greater than or equal to DOJ.";
                        LblError.Text += "<br/>";
                        return;
                    }
                }

                //if (DateTime.Parse(dOJ) < DateTime.Parse(dOB))
                //{
                //    LblError.Text = "DOJ should be greater than DOB.";
                //    LblError.Text += "<br/>";
                //    return;
                //}
                //else
                //{
                try
                {
                    using (var site = new SPSite(SPContext.Current.Site.Url))
                    {
                        using (var web = site.OpenWeb())
                        {
                            var list = web.Lists.TryGetList(Utilities.EmployeeScreen);
                            web.AllowUnsafeUpdates = true;

                            var newItem = list.Items.Add();
                            var empdupID = txtempid.Text;
                            var query = new SPQuery
                                            {
                                                Query = @"<Where>
                                                            <Eq>
                                                        <FieldRef Name='Title' />
                                                                     <Value Type='Text'>" +
                                                        empdupID + @"</Value>
                                                            </Eq>
                                                     </Where>"
                                            };
                            var collection = list.GetItems(query);
                            if (collection.Count == 0)
                            {
                                newItem[Utilities.EmployeeId] = txtempid.Text;
                                newItem[Utilities.EmployeeName] =
                                    web.AllUsers[peoplepickeremp.Accounts[0].ToString()];
                                newItem[Utilities.FirstName] = txtfristname.Text;
                                newItem[Utilities.LastName] = txtlastname.Text;
                                newItem["Employee Type"] = DdlEmptype.SelectedValue;
                                newItem["Department"] = DdlDep.SelectedValue;
                                newItem["Designation"] = DdlDesignation.SelectedValue;
                                //newItem[Utilities.Email] = Txtmail.Text;
                                //newItem[Utilities.Mobile] = TxtContact.Text;
                                newItem[Utilities.DateofJoin] = DateTime.Parse(GetFormatedDate(dOJ));// DtDoj.SelectedDate;
                                //newItem[Utilities.DOB] = DateTime.Parse(GetFormatedDate(dOB)); //DtDOB.SelectedDate;
                                if (DdlEmptype.SelectedItem.Text.Trim() == "Permanent")
                                {
                                    newItem["Date of Permanent"] = DateTime.Parse(GetFormatedDate(dOP));//DtDOP.SelectedDate;
                                }
                                var entity = new PickerEntity
                                                 {
                                                     DisplayText =
                                                         new SPFieldUserValue(web, ddlReportingTo.SelectedItem.Value)
                                                         .
                                                         User.
                                                         LoginName
                                                 };
                                newItem[Utilities.Manager] = web.AllUsers[entity.DisplayText];
                                newItem.Update();

                                var leavelist = web.Lists.TryGetList(Utilities.LeaveDays);
                                var field = leavelist.Fields["Employee Type"];
                                var leaveDaysQuery = new SPQuery
                                                         {
                                                             Query =
                                                                 @"<Where>
                                                <Eq>
                                                    <FieldRef Name='" +
                                                                 field.InternalName + @"' />
                                                    <Value Type='Lookup'>" +
                                                                 DdlEmptype.SelectedItem.Text + @"</Value>
                                                    </Eq>
                                                     </Where>"
                                                         };
                                var leaveDayscollection = leavelist.GetItems(leaveDaysQuery);
                                

                                foreach (SPListItem leaveType in leaveDayscollection)
                                {
                                    var empLeavelist = web.Lists.TryGetList(Utilities.EmployeeLeaves);
                                    var empLeaveItem = empLeavelist.Items.Add();
                                    var emptype = new SPFieldLookupValue(leaveType["Employee Type"].ToString());
                                    var lveType = new SPFieldLookupValue(leaveType["Leave Type"].ToString());

                                    empLeaveItem["Employee ID"] = txtempid.Text;
                                    empLeaveItem[Utilities.EmployeeName] =
                                        web.AllUsers[peoplepickeremp.Accounts[0].ToString()];
                                    empLeaveItem["Leave Type"] = leaveType["Leave Type"].ToString();

                                    if (lveType.LookupValue == "Comp off")
                                    {
                                        empLeaveItem["Leave Balance"] = 0;
                                    }
                                    else if (lveType.LookupValue == "LOP")
                                    {
                                        empLeaveItem["Leave Balance"] = 0;
                                    }
                                    else if (lveType.LookupValue == "Optional")
                                    {
                                        empLeaveItem["Leave Balance"] =
                                            decimal.Parse(leaveType["Leave Days"].ToString());
                                    }
                                    else
                                    {
                                        if (emptype.LookupValue.Trim() == "Permanent")
                                        {
                                            int dopMonth = DateTime.Parse(GetFormatedDate(dOP)).Month;// DtDOP.SelectedDate.Month;
                                            int monthDiff = GetMonthDifference(dopMonth);
                                            decimal leaves = decimal.Parse(leaveType["Leave Days"].ToString()) *
                                                             monthDiff;
                                            if (leaves % 1 == 0)
                                            {
                                                int noOfleaves = Convert.ToInt16(leaves);
                                                empLeaveItem["Leave Balance"] = noOfleaves;
                                            }
                                            else
                                            {
                                                empLeaveItem["Leave Balance"] = leaves;
                                            }
                                        }
                                        else
                                        {
                                            decimal leaves = decimal.Parse(leaveType["Leave Days"].ToString());
                                            if (leaves % 1 == 0)
                                            {
                                                int noOfleaves = Convert.ToInt16(leaves);
                                                empLeaveItem["Leave Balance"] = noOfleaves;
                                            }
                                            else
                                            {
                                                empLeaveItem["Leave Balance"] = leaves;
                                            }
                                        }
                                    }

                                    empLeaveItem["Leave utilized"] = 0;
                                    empLeaveItem["Leave Requested"] = 0;
                                    empLeaveItem["Reporting Manager"] = web.AllUsers[entity.DisplayText];
                                    empLeaveItem[Utilities.Year] = hdnCurrentYear.Value;
                                    empLeaveItem["Employee Type"] = DdlEmptype.SelectedValue;
                                    empLeaveItem.Update();
                                  


                                }

                                Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup(); SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.OK, '1');</script>");

                                //Context.Response.Write("<script type='text/javascript'>window.Location = window.Location;</script>");
                                Context.Response.Flush();
                                Context.Response.End();

                                //Response.Redirect(SPContext.Current.Web.Url);

                                web.AllowUnsafeUpdates = false;
                            }

                            else
                            {
                                LblError.Text = "Employee id is already exists.";
                                LblError.Text += "<br/>";
                            }
                        }
                    }
                }

                catch (Exception ex)
                {
                    LblError.Text = ex.Message;
                }
                // }
            }
        }
        public bool ValidateDate(string date, string dateFormat)
        {
            DateTime Test;

            try
            {

            }
            catch (Exception)
            {
                { }
                throw;
            }

            if (DateTime.TryParseExact(GetFormatedDate(date), dateFormat, null, DateTimeStyles.None, out Test) == true)
            {
                try
                {
                    DateTime dt = DateTime.Parse(GetFormatedDate(date));
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
        }

        private string GetFormatedDate(string date)
        {

            string[] words = date.Split('/');
            string newdate = string.Empty;
            for (int i = 0; i < words.Length; i++)
            {
                string month = (words[i].Length == 1) ? "0" + words[i] : words[i];
                words[i] = month;
            }
            newdate = words[0] + "/" + words[1] + "/" + words[2];
            return newdate;
        }

        private int GetMonthDifference(int currentMonth)
        {
            int strFinacialStratMonth = int.Parse(hdnStrtFnclMnth.Value);
            int returnValue;

            if (currentMonth <= strFinacialStratMonth)
                returnValue = strFinacialStratMonth - currentMonth;
            else
            {
                returnValue = (12 + strFinacialStratMonth) - currentMonth;
            }

            return (returnValue == 0 ? 12 : returnValue);
        }

        protected void BtnResetClick(object sender, EventArgs e)
        {
            txtempid.Text = String.Empty;
            txtfristname.Text = String.Empty;
            txtlastname.Text = String.Empty;
            LblError.Text = string.Empty;
            DdlEmptype.SelectedItem.Text = "--Select--";
            DdlDep.SelectedItem.Text = "--Select--";
            DdlDesignation.SelectedItem.Text = "--Select--";
            ddlReportingTo.SelectedItem.Text = "--Select--";
            //    DtDOB.ClearSelection();
            DtDOP.ClearSelection();
            DtDoj.ClearSelection();
            //Txtmail.Text = String.Empty;
            //TxtContact.Text = String.Empty;
            peoplepickeremp.CommaSeparatedAccounts = null;
        }

        //        private DataTable employeeTableStructure()
        //        {
        //            DataTable eTable = new DataTable();

        //            eTable.Columns.Add("Employee Id");
        //            eTable.Columns.Add("Employee Name");
        //            eTable.Columns.Add("Employee Type");
        //            eTable.Columns.Add("Sum of Balance Leave");
        //            eTable.Columns.Add("Edit");
        //            eTable.Columns.Add("Delete");

        //            return eTable;
        //        }
        //        private DataTable LoadEmpDetails()
        //        {
        //            DataTable emptable = employeeTableStructure();

        //            using (var site = new SPSite(SPContext.Current.Site.Url))
        //            {
        //                using (var web = site.OpenWeb())
        //                {
        //                    var emplist = web.Lists.TryGetList(Utilities.EmployeeScreen);

        //                    var itemCollection = emplist.Items;
        //                    foreach (SPListItem spListItem in itemCollection)
        //                    {
        //                        float balanceLeave = 0;
        //                        DataRow dataRow = emptable.NewRow();
        //                        var empLeavelist = web.Lists.TryGetList(Utilities.EmployeeLeaves);
        //                        var empidField = empLeavelist.Fields["Employee ID"];
        //                        var query = new SPQuery()
        //                        {
        //                            Query = @"<Where>
        //                                                            <Eq>
        //                                                        <FieldRef Name='" + empidField.InternalName + @"' />
        //                                                                     <Value Type='Text'>" + spListItem[Utilities.EmployeeID] + @"</Value>
        //                                                            </Eq>
        //                                                     </Where>"

        //                        };
        //                        var collection = empLeavelist.GetItems(query);

        //                        foreach (SPListItem listItem in collection)
        //                        {
        //                            balanceLeave += float.Parse(listItem["Leave Balance"].ToString());
        //                        }
        //                        var empname = new SPFieldLookupValue(spListItem["Employee Name"].ToString());

        //                        dataRow["Employee Id"] = spListItem[Utilities.EmployeeID].ToString(); // .Replace(spListItem["Title"].ToString().Substring(0, 3),"")
        //                        dataRow["Employee Name"] = empname.LookupValue;
        //                        dataRow["Employee Type"] = spListItem[Utilities.EmployeeType].ToString();
        //                        dataRow["Sum of Balance Leave"] = balanceLeave;

        //                        emptable.Rows.Add(dataRow);

        //                    }

        //                }
        //            }

        //            return emptable;
        //        }

        //        protected void EmpGridViewRowEditing(object sender, GridViewEditEventArgs e)
        //        {
        //            empGridView.EditIndex = e.NewEditIndex;
        //            LoadEmpDetails();

        //        }

        internal SPListItemCollection GetListItemCollection(SPList spList, string keyOne, string valueOne, string keyTwo, string valueTwo)
        {
            // Return list item collection based on the lookup field

            SPField spFieldOne = spList.Fields[keyOne];
            SPField spFieldTwo = spList.Fields[keyTwo];
            var query = new SPQuery
            {
                Query = @"<Where>
                          <And>
                                <Eq>
                                    <FieldRef Name=" + spFieldOne.InternalName + @" />
                                    <Value Type=" + spFieldOne.Type.ToString() + ">" + valueOne + @"</Value>
                                </Eq>
                                <Eq>
                                    <FieldRef Name=" + spFieldTwo.InternalName + @" />
                                    <Value Type=" + spFieldTwo.Type.ToString() + ">" + valueTwo + @"</Value>
                                </Eq>
                          </And>
                        </Where>"
            };

            return spList.GetItems(query);
        }
    }
}