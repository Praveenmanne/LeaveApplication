using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Leave_Application.EditEmployeeInformation
{
    public partial class EditEmployeeInformationUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DtDoj.MaxDate = DateTime.Now;
            //  DtDOB.MaxDate = DateTime.Now;
            DtDOP.MaxDate = DateTime.Now;
            if (!IsPostBack)
            {
                // divdop.Visible = false;
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
                    EmployeeDetails();
                }
                catch (Exception ex)
                {
                    LblError.Text = ex.Message;
                }
            }
        }

        public void EmployeeDetails()
        {
            try
            {
                if (Request.QueryString["EmpId"] != null)
                {
                    using (var site = new SPSite(SPContext.Current.Site.Url))
                    {
                        using (var web = site.OpenWeb())
                        {
                            web.AllowUnsafeUpdates = true;
                            var empid = Request.QueryString["EmpId"];

                            var emplist = web.Lists.TryGetList("Employee Screen");
                            var empitem = emplist.GetItemById(Convert.ToInt16(empid));
                            empSPid.Value = empid;

                            txtempid.Text = empitem["Title"].ToString();
                            txtfristname.Text = empitem["First Name"].ToString();
                            txtlastname.Text = empitem["Last Name"].ToString();
                            //TxtContact.Text = empitem["Mobile"].ToString();
                            var entity = new PickerEntity
                                             {
                                                 DisplayText =
                                                     new SPFieldUserValue(web, empitem["Employee Name"].ToString()).User
                                                     .
                                                     LoginName
                                             };

                            txtempusername.Text = entity.DisplayText;

                            //  peoplepickeremp.Entities.Add(entity);
                            DdlDep.SelectedItem.Text = empitem["Department"].ToString();

                            DdlDesignation.SelectedItem.Text = empitem["Designation"].ToString();
                            DdlEmptype.SelectedItem.Text = empitem["Employee Type"].ToString();
                            hdnCurrentEmpType.Value = empitem["Employee Type"].ToString();
                            //Txtmail.Text = empitem["Email"].ToString();
                            //      DtDOB.SelectedDate = Convert.ToDateTime(empitem["DOB"].ToString());
                            DtDoj.SelectedDate = Convert.ToDateTime(empitem["Date of Join"].ToString());

                            if (empitem["Employee Type"].ToString().Trim() == "Permanent")
                            {
                                if (empitem["Date of Permanent"] != null)
                                    DtDOP.SelectedDate = Convert.ToDateTime(empitem["Date of Permanent"].ToString());
                                //divdop.Visible = true;
                                chkPrePL.Enabled = false;
                            }
                            //else
                            //{
                            //    divdop.Visible = false;
                            //}
                            var spv = new SPFieldLookupValue(empitem["Manager"].ToString());

                            // ddlReportingTo.SelectedItem.Text = spv.LookupValue;
                            //DropDownList customer = 
                            ddlReportingTo.Items.FindByText(spv.LookupValue).Selected = true;
                            //if (customer != null)
                            //{
                            //    customer.Selected = true;

                            //}
                            if (empitem["Status"].ToString().Trim() != "Active")
                            {
                                BtnRegister.Enabled = false;
                            }

                            web.AllowUnsafeUpdates = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LblError.Text = ex.Message;
            }
        }

        protected void BtnUpdateClick(object sender, EventArgs e)
        {
            string dOP = ((TextBox)(DtDOP.Controls[0])).Text;
            string dOJ = ((TextBox)(DtDoj.Controls[0])).Text;
            //   string dOB = ((TextBox)(DtDOB.Controls[0])).Text;



            //if (DdlEmptype.SelectedItem.Value.Trim() == "Permanent")
            //{

            //    if (DtDOP.IsDateEmpty)
            //    {
            //        LblError.Text = "Please Enter the valid DOP.";
            //        LblError.Text += "<br/>";
            //        divdop.Visible = true;
            //        return;
            //    }
            //    else if (!ValidateDate(DtDOP.SelectedDate.ToShortDateString(), "MM/dd/yyyy"))
            //    {
            //        LblError.Text = "Date format should be in 'MM/DD/YYYY' format.";
            //        LblError.Text += "<br/>";
            //        divdop.Visible = true;
            //        DtDOP.ClearSelection();
            //        DtDOP.Focus();
            //        return;

            //    }
            //    else if (DtDOP.SelectedDate > DateTime.Now)
            //    {
            //        LblError.Text = "DOP should be Less than or equal to today.";
            //        LblError.Text += "<br/>";
            //        divdop.Visible = true;
            //        DtDOP.ClearSelection();
            //        DtDOP.Focus();
            //        return;
            //    }
            //}
            //if (!ValidateDate(DtDoj.SelectedDate.ToShortDateString(), "MM/dd/yyyy"))
            //{
            //    LblError.Text = "Date format should be in 'MM/DD/YYYY' format.";
            //    LblError.Text += "<br/>";
            //    DtDoj.ClearSelection();
            //    DtDoj.Focus();
            //    return;

            //}
            //else if (DtDoj.SelectedDate > DateTime.Now)
            //{
            //    LblError.Text = "DOP should be Less than or equal to today.";
            //    LblError.Text += "<br/>";
            //    divdop.Visible = true;
            //    DtDoj.ClearSelection();
            //    DtDoj.Focus();
            //    return;
            //}
            //if (!ValidateDate(DtDOB.SelectedDate.ToShortDateString(), "MM/dd/yyyy"))
            //{
            //    LblError.Text = "Date format should be in 'MM/DD/YYYY' format.";
            //    LblError.Text += "<br/>";
            //    DtDOB.ClearSelection();
            //    DtDOB.Focus();
            //    return;

            //}
            //else if (DtDOB.SelectedDate > DateTime.Now)
            //{
            //    LblError.Text = "DOP should be Less than or equal to today.";
            //    LblError.Text += "<br/>";
            //    divdop.Visible = true;
            //    DtDOB.ClearSelection();
            //    DtDOB.Focus();
            //    return;
            //}
            //if (DtDOP.SelectedDate.Date < DtDoj.SelectedDate.Date)
            //{
            //    LblError.Text = "DOP should be greater than or equal to DOJ.";
            //    LblError.Text += "<br/>";
            //    return;
            //}
            //else if (DtDoj.SelectedDate.Date < DtDOB.SelectedDate.Date)
            //{
            //    LblError.Text = "DOJ should be greater than DOB.";
            //    LblError.Text += "<br/>";
            //    return;
            //}
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
                LblError.Text = "Please Enter the valid DOJ";
                LblError.Text += "<br/>";
                // divdop.Visible = true;
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
                divdop.Visible = true;
                DtDoj.ClearSelection();
                DtDoj.Focus();
                return;
            }
            if (DdlEmptype.SelectedItem.Value.Trim() == "Permanent")
            {

                if (string.IsNullOrEmpty(dOP))
                {
                    LblError.Text = "Please Enter the valid DOP.";
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

            try
            {
                using (var site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (var web = site.OpenWeb())
                    {
                        web.AllowUnsafeUpdates = true;
                        var empid = empSPid.Value;
                        var emplist = web.Lists.TryGetList("Employee Screen");
                        var empitem = emplist.GetItemById(Convert.ToInt16(empid));

                        empitem["First Name"] = txtfristname.Text;
                        empitem["Last Name"] = txtlastname.Text;

                        // empitem["Employee Name"] = web.AllUsers[peoplepickeremp.Accounts[0].ToString()];
                        empitem["Employee Type"] = DdlEmptype.SelectedItem.Text;
                        empitem["Department"] = DdlDep.SelectedItem.Text;
                        empitem["Designation"] = DdlDesignation.SelectedItem.Text;
                        //empitem["Email"] = Txtmail.Text;
                        //empitem["Mobile"] = TxtContact.Text;
                        //empitem["Date of Join"] = DateTime.Parse(GetFormatedDate(dOJ)); 
                        //empitem["DOB"] = DtDOB.SelectedDate;
                        //empitem["Date of Permanent"] = DtDOP.SelectedDate;
                        empitem[Utilities.DateofJoin] = DateTime.Parse(GetFormatedDate(dOJ));// DtDoj.SelectedDate;
                        //  empitem[Utilities.DOB] = DateTime.Parse(GetFormatedDate(dOB)); //DtDOB.SelectedDate;
                        if (DdlEmptype.SelectedItem.Text.Trim() == "Permanent")
                        {
                            empitem["Date of Permanent"] = DateTime.Parse(GetFormatedDate(dOP));//DtDOP.SelectedDate;
                        }
                        PickerEntity entity = new PickerEntity();
                        entity.DisplayText = new SPFieldUserValue(web, ddlReportingTo.SelectedItem.Value).User.LoginName;
                        empitem[Utilities.Manager] = web.AllUsers[entity.DisplayText];

                        empitem.Update();

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

                        //int currentMonth = DateTime.Now.Month;
                        // int monthDiff = GetMonthDifference(currentMonth);


                        foreach (SPListItem leavedaItem in leaveDayscollection)
                        {
                            var emptype = new SPFieldLookupValue(leavedaItem["Employee Type"].ToString());
                            var leaveType = new SPFieldLookupValue(leavedaItem["Leave Type"].ToString());
                            var employeeleavelist = web.Lists.TryGetList(Utilities.EmployeeLeaves);

                            //var fieldempid = employeeleavelist.Fields["Employee ID"];
                            //var fieldempLeavetype = employeeleavelist.Fields["Leave Type"];
                            var lveType = new SPFieldLookupValue(leavedaItem["Leave Type"].ToString());

                            //var employeeleaveDaysQuery = new SPQuery
                            //{
                            //    Query = @"<Where><And><Eq><FieldRef Name=" + fieldempid.InternalName + " /><Value Type='Text'>" + txtempid.Text + "</Value></Eq><Eq><FieldRef Name=" + fieldempLeavetype.InternalName + " /><Value Type='Lookup'>" + leaveType.LookupValue
                            //    + "</Value></Eq></And></Where>"
                            //};

                            var empleaveCollection = GetListItemCollection(employeeleavelist, "Employee ID",
                                                                           txtempid.Text, "Leave Type",
                                                                           leaveType.LookupValue, "Year",
                                                                           hdnCurrentYear.Value);

                            //employeeleavelist.GetItems(employeeleaveDaysQuery);
                            if (empleaveCollection.Count == 0)
                            {
                                var empLeaveItem = employeeleavelist.Items.Add();

                                empLeaveItem["Employee ID"] = txtempid.Text;
                                empLeaveItem[Utilities.EmployeeName] = web.AllUsers[txtempusername.Text];
                                empLeaveItem["Leave Type"] = leavedaItem["Leave Type"].ToString();
                                if (lveType.LookupValue == "Comp off")
                                {
                                    empLeaveItem["Leave Balance"] = 0;
                                }
                                else if (lveType.LookupValue == "Optional")
                                {
                                    empLeaveItem["Leave Balance"] =
                                        decimal.Parse(leavedaItem["Leave Days"].ToString());
                                }
                                else if (lveType.LookupValue == "LOP")
                                {
                                    empLeaveItem["Leave Balance"] = 0;
                                }
                                else
                                {
                                    if (emptype.LookupValue.Trim() == "Permanent")
                                    {
                                        int dopMonth = DateTime.Parse(GetFormatedDate(dOP)).Month;// DtDOP.SelectedDate.Month;
                                        int monthDiff = GetMonthDifference(dopMonth);
                                        decimal leaves = decimal.Parse(leavedaItem["Leave Days"].ToString()) *
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

                                        //if (emptype.LookupValue.Trim() == "Permanent")
                                    //    empLeaveItem["Leave Balance"] = decimal.Parse(leaveType["Leave Days"].ToString()) * monthDiff;
                                    else
                                    {
                                        decimal leaves = decimal.Parse(leavedaItem["Leave Days"].ToString()) *
                                                         1;
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
                            else
                            {
                                if (hdnCurrentEmpType.Value.Trim() != "Permanent")
                                {
                                    foreach (SPListItem empleaveItem in empleaveCollection)
                                    {
                                        if (chkPrePL.Checked)
                                        {
                                            if (lveType.LookupValue != "Comp off" &&
                                                lveType.LookupValue != "Optional" && lveType.LookupValue != "LOP")
                                            {
                                                if (emptype.LookupValue.Trim() == "Permanent")
                                                {
                                                    int dopMonth = DateTime.Parse(GetFormatedDate(dOP)).Month;
                                                    //DtDOP.SelectedDate.Month;
                                                    int monthDiff = GetMonthDifference(dopMonth) - 1;
                                                    decimal leaves =
                                                        decimal.Parse(empleaveItem["Leave Balance"].ToString()) +
                                                        decimal.Parse(leavedaItem["Leave Days"].ToString()) *
                                                        monthDiff;
                                                    if (leaves % 1 == 0)
                                                    {
                                                        int noOfleaves = Convert.ToInt16(leaves);
                                                        empleaveItem["Leave Balance"] = noOfleaves;
                                                    }
                                                    else
                                                    {
                                                        empleaveItem["Leave Balance"] = leaves;
                                                    }
                                                }
                                                else
                                                {
                                                    decimal leaves =
                                                        decimal.Parse(empleaveItem["Leave Balance"].ToString()) +
                                                        decimal.Parse(leavedaItem["Leave Days"].ToString());
                                                    if (leaves % 1 == 0)
                                                    {
                                                        int noOfleaves = Convert.ToInt16(leaves);
                                                        empleaveItem["Leave Balance"] = noOfleaves;
                                                    }
                                                    else
                                                    {
                                                        empleaveItem["Leave Balance"] = leaves;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (lveType.LookupValue != "Comp off" &&
                                                lveType.LookupValue != "Optional" && lveType.LookupValue != "LOP")
                                            {
                                                if (emptype.LookupValue.Trim() == "Permanent")
                                                {
                                                    int dopMonth = DateTime.Parse(GetFormatedDate(dOP)).Month;
                                                    // DtDOP.SelectedDate.Month;
                                                    int monthDiff = GetMonthDifference(dopMonth);
                                                    decimal leaves =
                                                        decimal.Parse(leavedaItem["Leave Days"].ToString()) *
                                                        monthDiff;
                                                    if (leaves % 1 == 0)
                                                    {
                                                        int noOfleaves = Convert.ToInt16(leaves);
                                                        empleaveItem["Leave Balance"] = noOfleaves;
                                                    }
                                                    else
                                                    {
                                                        empleaveItem["Leave Balance"] = leaves;
                                                    }
                                                }

                                                else
                                                {
                                                    decimal leaves =
                                                        decimal.Parse(leavedaItem["Leave Days"].ToString());
                                                    if (leaves % 1 == 0)
                                                    {
                                                        int noOfleaves = Convert.ToInt16(leaves);
                                                        empleaveItem["Leave Balance"] = noOfleaves;
                                                    }
                                                    else
                                                    {
                                                        empleaveItem["Leave Balance"] = leaves;
                                                    }
                                                }
                                            }
                                        }
                                        empleaveItem[Utilities.Year] = hdnCurrentYear.Value;
                                        empleaveItem["Employee Type"] = DdlEmptype.SelectedValue;
                                        empleaveItem.Update();
                                    }
                                }
                            }
                        }
                        Context.Response.Write(
                            "<script type='text/javascript'>window.frameElement.commitPopup(); SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.OK, '1');</script>");
                    }
                }
            }
            catch (Exception ex)
            {
                LblError.Text = ex.Message;
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

        protected void BtnCancelClick(object sender, EventArgs e)
        {
            Context.Response.Write("<script type='text/javascript'>window.frameElement.commitPopup(); SP.UI.ModalDialog.commonModalDialogClose(SP.UI.DialogResult.OK, '1');</script>");

            // Response.Redirect(SPContext.Current.Site.Url);
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

        internal SPListItemCollection GetListItemCollection(SPList spList, string key, string value)
        {
            // Return list item collection based on the lookup field

            SPField spField = spList.Fields[key];
            var query = new SPQuery
            {
                Query = @"<Where>
                        <Eq>
                            <FieldRef Name='" + spField.InternalName + @"'/><Value Type='" + spField.Type.ToString() + @"'>" + value + @"</Value>
                        </Eq>
                        </Where>"
            };

            return spList.GetItems(query);
        }

        internal SPListItemCollection GetListItemCollection(SPList spList, string keyOne, string valueOne, string keyTwo, string valueTwo, string keyThree, string valueThree)
        {
            // Return list item collection based on the lookup field

            SPField spFieldOne = spList.Fields[keyOne];
            SPField spFieldTwo = spList.Fields[keyTwo];
            SPField spFieldThree = spList.Fields[keyThree];
            var query = new SPQuery
            {
                Query = @"<Where>
                          <And>
                             <And>
                                <Eq>
                                   <FieldRef Name=" + spFieldOne.InternalName + @" />
                                   <Value Type=" + spFieldOne.Type.ToString() + @">" + valueOne + @"</Value>
                                </Eq>
                                <Eq>
                                   <FieldRef Name=" + spFieldTwo.InternalName + @" />
                                   <Value Type=" + spFieldTwo.Type.ToString() + @">" + valueTwo + @"</Value>
                                </Eq>
                             </And>
                             <Eq>
                                <FieldRef Name=" + spFieldThree.InternalName + @" />
                                <Value Type=" + spFieldThree.Type.ToString() + @">" + valueThree + @"</Value>
                             </Eq>
                          </And>
                       </Where>"

                //                Query = @"<Where>
                //                          <And>
                //                                <Eq>
                //                                    <FieldRef Name=" + spFieldOne.InternalName + @" />
                //                                    <Value Type=" + spFieldOne.Type.ToString() + ">" + valueOne + @"</Value>
                //                                </Eq>
                //                                <Eq>
                //                                    <FieldRef Name=" + spFieldTwo.InternalName + @" />
                //                                    <Value Type=" + spFieldTwo.Type.ToString() + ">" + valueTwo + @"</Value>
                //                                </Eq>
                //                          </And>
                //                        </Where>"
            };

            return spList.GetItems(query);
        }
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
        protected void ddlReportingTo_SelectedIndexChanged(object sender, EventArgs e)
        {

            // ddlReportingTo.SelectedItem.Text = spv.LookupValue;
            //DropDownList customer = 
            // ddlReportingTo.Items.FindByText(spv.LookupValue).Selected = true;
            using (var site = new SPSite(SPContext.Current.Site.Url))
            {
                using (var web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;
                    var empid = Request.QueryString["EmpId"];

                    var emplist = web.Lists.TryGetList("Employee Screen");
                    var empitem = emplist.GetItemById(Convert.ToInt16(empid));
                    empSPid.Value = empid;
                    var entity = new PickerEntity
                    {
                        DisplayText =
                            new SPFieldUserValue(web, empitem["Employee Name"].ToString()).User
                            .
                            LoginName
                    };

                    //var spv = new SPFieldLookupValue(empitem["Manager"].ToString());
                    //ddlReportingTo.SelectedItem.Text = spv.LookupValue;

                   txtempusername.Text = web.AllUsers[entity.DisplayText].Name;

                 
                    //SPUser user = web.CurrentUser;
                    //hdnCurrentUsername.Value = user.Name;

                    var leavelist = SPContext.Current.Web.Lists.TryGetList(Utilities.LeaveRequest); 
                    var leavelistitems = GetListItemCollection(leavelist,  "RequestedFrom", txtempusername.Text,"Status","Pending");

                    if (leavelistitems != null && leavelistitems.Count > 0)
                    {
                        LblError.Text = "Make sure that no pending leave requests for previous manager";
                        BtnRegister.Enabled = false;
                    }
                    else
                    {
                        BtnRegister.Enabled = true;
                    }

                }
            }
        }


    }
}