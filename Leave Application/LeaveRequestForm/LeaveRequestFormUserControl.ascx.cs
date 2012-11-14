using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Leave_Application.LeaveRequestForm
{
    public partial class LeaveRequestFormUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          if(!IsPostBack)
            {
                try
                {

                    LoadOptionalHolidays();
                    dateTimeStartDate.SelectedDate = DateTime.Now;
                    dateTimeEndDate.SelectedDate = DateTime.Now;
                    lstboxOptionalLeaves.Attributes.Add("onchange", "DateCompare();");

                    // txtLeaveDays.Text = "1";
                    // string currentUsername = string.Empty;

                    Employee employee = GetEmployeedetails();
                    lblEmpID.Text = employee.EmpId;
                    lblDesgination.Text = employee.Desigination;
                    lblDepartment.Text = employee.Department;
                    hdnEmployeeType.Value = employee.EmployeeType;
                    ddlReportingTo.Text = employee.Manager;
                    hdnReportingTo.Value = employee.ManagerWithID;
                    if (employee.EmpId != null)
                    {
                        using (var site = new SPSite(SPContext.Current.Site.Url))
                        {
                            using (var web = site.OpenWeb())
                            {
                                var currentYear =
                                    SPContext.Current.Web.Lists.TryGetList(Utilities.CurrentYear).GetItems();
                                foreach (SPListItem currentYearValue in currentYear)
                                {
                                    hdnCurrentYear.Value = currentYearValue["Title"].ToString();
                                }

                                var employeeLeaveTypes = GetListItemCollection(web.Lists[Utilities.LeaveDays],
                                                                               "Employee Type", hdnEmployeeType.Value);
                                ddlTypeofLeave.Items.Clear();
                                ddlTypeofLeave.Items.Add("--select--");
                                foreach (SPListItem employeeLeaveType in employeeLeaveTypes)
                                {
                                    var spv =
                                        new SPFieldLookupValue(employeeLeaveType["Leave Type"].ToString());
                                    ddlTypeofLeave.Items.Add(spv.LookupValue);
                                }

                                var holidayList = SPContext.Current.Web.Lists.TryGetList(Utilities.HolidayList);
                                var holidaysdateList = new List<string>();
                                if (holidayList != null)
                                {
                                    var holidays = GetListItemCollection(holidayList,
                                                                         "HolidayType", "Company Holiday", "Year",
                                                                         hdnCurrentYear.Value);
                                    holidaysdateList.AddRange(from SPListItem holiday in holidays
                                                              select
                                                                  DateTime.Parse(holiday["Date"].ToString()).
                                                                  ToShortDateString());
                                }

                                var tempList =
                                    holidaysdateList.Where(a => DateTime.Now.ToShortDateString().Contains(a)).ToList();

                                // holidaysdateList.Contains(AbortTransaction=)

                                txtDuration.Value = (tempList.Any()) ? "0" : "1";
                                txtDuration.Value = (DateTime.Now.DayOfWeek == DayOfWeek.Sunday ||
                                                     DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
                                                        ? "0"
                                                        : "1";
                                lblDuration.InnerText = txtDuration.Value;

                                var oSerializer = new JavaScriptSerializer();
                                string sJSON = oSerializer.Serialize(holidaysdateList.ToArray());

                                hdnHolidayList.Value = sJSON;

                                string currentFnclYear = hdnCurrentYear.Value;


                                string finalcialEndYear =
                                    currentFnclYear.Substring(
                                        currentFnclYear.IndexOf("-", System.StringComparison.Ordinal) + 1);
                                string finalcialStartYear = currentFnclYear.Substring(0, 4);

                                string financialStrartMonth = string.Empty;
                                var financialStrartMonthllist =
                                   SPContext.Current.Web.Lists.TryGetList(Utilities.Financialstartmonth).GetItems();
                                foreach (SPListItem financialStrartmonth in financialStrartMonthllist)
                                {
                                    financialStrartMonth = financialStrartmonth["Title"].ToString();
                                }

                                string tempfinStrtDate = financialStrartMonth.Trim() + "/01/" + finalcialStartYear;
                                string tempfinEndDate = (int.Parse(financialStrartMonth) - 1).ToString() + "/01/" + finalcialEndYear;

                                hdnFnclStarts.Value = GetFirstDayOfMonth(DateTime.Parse(tempfinStrtDate)).ToShortDateString();
                                hdnFnclEnds.Value = GetLastDayOfMonth(DateTime.Parse(tempfinEndDate)).ToShortDateString();

                            }
                        }

                        //    DropDownList customer =

                        //ddlReportingTo.SelectedItem.Text = employee.Manager;
                        //grvBalanceLeave.DataSource = GetBalanceLeave(employee.EmpId);
                        //grvBalanceLeave.DataBind();
                    }
                    else
                    {
                        btnReset.Enabled = false;
                        btnSubmit.Enabled = false;
                        Response.Redirect("/_layouts/LeaveApplication/unauthorised.aspx");
                    }
                }

                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                }
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

        internal Employee GetEmployeedetails()
        {
            var empEntity = new Employee();
            using (var site = new SPSite(SPContext.Current.Site.Url))
            {
                using (var web = site.OpenWeb())
                {
                    SPUser user = web.CurrentUser;
                    hdnCurrentUsername.Value = user.Name;

                    SPListItemCollection currentUserDetails = GetListItemCollection(web.Lists[Utilities.EmployeeScreen], "Employee Name", hdnCurrentUsername.Value, "Status", "Active");
                    foreach (SPListItem currentUserDetail in currentUserDetails)
                    {
                        empEntity.EmpId = currentUserDetail[Utilities.EmployeeId].ToString();
                        empEntity.EmployeeType = currentUserDetail[Utilities.EmployeeType].ToString();
                        empEntity.Department = currentUserDetail[Utilities.Department].ToString();
                        empEntity.Desigination = currentUserDetail[Utilities.Designation].ToString();
                        empEntity.DOJ = DateTime.Parse(currentUserDetail[Utilities.DateofJoin].ToString());
                        empEntity.ManagerWithID = currentUserDetail[Utilities.Manager].ToString();
                        var spv = new SPFieldLookupValue(currentUserDetail[Utilities.Manager].ToString());
                        empEntity.Manager = spv.LookupValue;
                    }
                }
            }
            return empEntity;
        }

        protected void BtnSubmitClick(object sender, EventArgs e)
        {
            string leaveSDate = ((TextBox)(dateTimeStartDate.Controls[0])).Text;
            string leaveEDate = ((TextBox)(dateTimeEndDate.Controls[0])).Text;
            if (ddlTypeofLeave.SelectedItem.Text == "Optional")
            {

                leaveSDate = lstboxOptionalLeaves.SelectedValue;
                leaveEDate = lstboxOptionalLeaves.SelectedValue;
            }
            if (ddlTypeofLeave.SelectedIndex < 1)
            {
                lblError.Text = "Please select leave type.";
                lblError.Text += "<br/>";
                return;
            }
            if (string.IsNullOrEmpty(leaveSDate))
            {
                lblError.Text = "Date format should be in 'MM/DD/YYYY' format.";
                lblError.Text += "<br/>";

                return;
            }
            if (string.IsNullOrEmpty(leaveEDate))
            {
                lblError.Text = "Date format should be in 'MM/DD/YYYY' format.";
                lblError.Text += "<br/>";

                return;
            }
            if (!ValidateDate(leaveSDate, "MM/dd/yyyy"))
            {
                lblError.Text = "Date format should be in 'MM/DD/YYYY' format.";
                lblError.Text += "<br/>";
                if (ddlTypeofLeave.SelectedItem.Text == "Optional")
                {
                    optinalDates.Visible = true;
                    Selecteddates.Visible = false;
                }
                else
                {
                    dateTimeStartDate.ClearSelection();
                    txtDuration.Value = string.Empty;
                    lblDuration.InnerText = string.Empty;
                    dateTimeStartDate.Focus();

                    optinalDates.Visible = false;
                    Selecteddates.Visible = true;
                }
                return;

            }
            if (!ValidateDate(leaveEDate, "MM/dd/yyyy"))
            {
                lblError.Text = "Date format should be in 'MM/DD/YYYY' format.";
                lblError.Text += "<br/>";
                if (ddlTypeofLeave.SelectedItem.Text == "Optional")
                {
                    optinalDates.Visible = true;
                    Selecteddates.Visible = false;
                }
                else
                {
                    dateTimeEndDate.ClearSelection();
                    txtDuration.Value = string.Empty;
                    lblDuration.InnerText = string.Empty;
                    optinalDates.Visible = false;
                    Selecteddates.Visible = true;
                    dateTimeEndDate.Focus();
                }
                return;

            }
            try
            {
                SetDate();
                if (DateTime.Parse(leaveSDate) >= DateTime.Parse(hdnFnclStarts.Value) && DateTime.Parse(leaveEDate) <= DateTime.Parse(hdnFnclEnds.Value))
                {

                    lblError.Text = string.Empty;

                    if (txtDuration.Value != "0" && txtDuration.Value != "")
                    {
                        if (decimal.Parse(txtDuration.Value) > 0)
                        {
                            using (var site = new SPSite(SPContext.Current.Site.Url))
                            {
                                using (var web = site.OpenWeb())
                                {
                                    SPUser user = web.CurrentUser;
                                    hdnCurrentUsername.Value = user.Name;
                                    var list = web.Lists.TryGetList(Utilities.LeaveRequest);
                                    web.AllowUnsafeUpdates = true;

                                    SPListItemCollection leaveDetails = GetListItemCollection(
                                        web.Lists[Utilities.EmployeeLeaves], "Employee ID", lblEmpID.Text,
                                        "Leave Type",
                                        ddlTypeofLeave.SelectedValue, "Year", hdnCurrentYear.Value);
                                    if (leaveDetails.Count != 0)
                                    {
                                        foreach (SPListItem leaveDetail in leaveDetails)
                                        {
                                            if (decimal.Parse(leaveDetail["Leave Balance"].ToString()) <
                                                decimal.Parse(txtDuration.Value) &&
                                                ddlTypeofLeave.SelectedItem.Text != "LOP")
                                            {
                                                lblError.Text = "You dont have enough leave balance in this leave type";
                                                SetDate();
                                            }
                                            else
                                            {


                                                SPListItem newItem = list.Items.Add();

                                                // var itemCollection = empitem.Items;
                                                var entity = new PickerEntity
                                                                 {
                                                                     DisplayText =
                                                                         new SPFieldUserValue(web, hdnReportingTo.Value)
                                                                         .
                                                                         User.
                                                                         LoginName
                                                                 };

                                                //   empitem[Utilities.Manager] =

                                                newItem["RequestedFrom"] = web.AllUsers[web.CurrentUser.LoginName];
                                                newItem["RequestedTo"] = web.AllUsers[entity.DisplayText];
                                                newItem["Leave Type"] = ddlTypeofLeave.SelectedValue;
                                                newItem["Purpose of Leave"] = txtPurpose.Text;
                                                newItem["EmpID"] = lblEmpID.Text;
                                                newItem["Desgination"] = lblDesgination.Text;
                                                newItem["Department"] = lblDepartment.Text;
                                                newItem["Leave Days"] = txtDuration.Value;
                                                newItem["Year"] = hdnCurrentYear.Value;
                                                //if (ddlTypeofLeave.SelectedItem.Text != "Optional")
                                                //{
                                                newItem["Starting Date"] = DateTime.Parse(leaveSDate);
                                                newItem["Ending Date"] = DateTime.Parse(leaveEDate);
                                                newItem["Employee Status"] = "Active";

                                                //dateTimeEndDate.SelectedDate;
                                                //}
                                                //else
                                                //{
                                                //    newItem["Starting Date"] =
                                                //        DateTime.Parse(lstboxOptionalLeaves.SelectedValue);
                                                //    newItem["Ending Date"] =
                                                //        DateTime.Parse(lstboxOptionalLeaves.SelectedValue);
                                                //}
                                                newItem.Update();

                                                decimal leavesBal =
                                                    decimal.Parse(leaveDetail["Leave Balance"].ToString()) -
                                                    decimal.Parse(txtDuration.Value);
                                                if (ddlTypeofLeave.SelectedItem.Text == "LOP")
                                                {
                                                    leaveDetail["Leave Balance"] = 0;
                                                }
                                                else
                                                {
                                                    if (leavesBal % 1 == 0)
                                                    {
                                                        int noOfleaves = Convert.ToInt16(leavesBal);
                                                        leaveDetail["Leave Balance"] = noOfleaves;
                                                    }
                                                    else
                                                    {
                                                        leaveDetail["Leave Balance"] = leavesBal;
                                                    }
                                                }

                                                decimal leavesReq =
                                                    decimal.Parse(leaveDetail["Leave Requested"].ToString()) +
                                                    decimal.Parse(txtDuration.Value);
                                                if (leavesReq % 1 == 0)
                                                {
                                                    int noOfleaves = Convert.ToInt16(leavesReq);
                                                    leaveDetail["Leave Requested"] = noOfleaves;
                                                }
                                                else
                                                {
                                                    leaveDetail["Leave Requested"] = leavesReq;
                                                }

                                                //leaveDetail["Leave Requested"] =
                                                //    decimal.Parse(leaveDetail["Leave Requested"].ToString()) +
                                                //    decimal.Parse(txtDuration.Value);
                                                leaveDetail.Update();

                                                //grvBalanceLeave.DataSource = GetBalanceLeave(lblEmpID.Text);
                                                //grvBalanceLeave.DataBind();
                                                ddlTypeofLeave.ClearSelection();
                                                Response.Redirect(site.Url);

                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblError.Text = "Select Valid dates";
                        }
                    }
                    else
                    {
                        lblError.Text = "Select Valid dates";
                    }
                }
                else
                {
                    lblError.Text = "You can't able to select previous/future financial year dates";

                }
                //if (SPContext.Current != null)
                //using (var site = new SPSite(SPContext.Current.Site.Url))
                //{
                //    using (var web = site.OpenWeb())
                //    {
                //        var list = web.Lists.TryGetList(Utilities.LeaveRequest);
                //        web.AllowUnsafeUpdates = true;
                //        SPListItem newItem = list.Items.Add();

                //       // var itemCollection = empitem.Items;

                //            newItem["Leave Type"] = ddlTypeofLeave.SelectedValue;
                //            newItem["Leave Type"] = ddlReportingTo.SelectedValue;
                //            newItem["EmpId"] = lblEmpID.Text;
                //            newItem["Desgination"] = lblDesgination.Text;
                //            newItem["Department"] = lblDepartment.Text;
                //            newItem["Starting Date"] = dateTimeStartDate.SelectedDate;
                //            newItem["Ending Date"] = dateTimeEndDate.SelectedDate;
                //            newItem.Update();

                //        web.AllowUnsafeUpdates = false;
                //    }
                //}
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        protected void BtnResetClick(object sender, EventArgs e)
        {
            txtPurpose.Text = String.Empty;
            //   txtDuration.Value = String.Empty;
            dateTimeStartDate.SelectedDate = DateTime.Now;
            dateTimeEndDate.SelectedDate = DateTime.Now;
            ddlTypeofLeave.SelectedIndex = 0;
            lblError.Text = string.Empty;
            SetDate();

        }

        internal void SetDate()
        {
            string leaveSDate = ((TextBox)(dateTimeStartDate.Controls[0])).Text;
            string leaveEDate = ((TextBox)(dateTimeEndDate.Controls[0])).Text;
            DateTime tempstartdate = DateTime.Parse(leaveSDate);
            while (IsTodayisHoliday(tempstartdate))
            {
                tempstartdate = tempstartdate.AddDays(1);


            }
            DateTime tempenddate = DateTime.Parse(leaveEDate);// dateTimeEndDate.SelectedDate;
            while (IsTodayisHoliday(tempenddate))
            {
                tempenddate = tempenddate.AddDays(-1);

            }
            double totaldays = 0;
            if (ddlTypeofLeave.SelectedItem.Text == "Comp off")
            {
                double leavcount = 0;

                for (DateTime temdate = tempstartdate; temdate <= tempenddate; temdate=temdate.AddDays(1))
                {
                    if (!IsTodayisHoliday(temdate))
                        leavcount++;
                }
                totaldays = leavcount;
            }
            else
            {
                totaldays = (tempenddate - tempstartdate).TotalDays + 1;
            }
            

            var duration = totaldays < 0 ? 0 : totaldays;

            txtDuration.Value = duration.ToString();
            lblDuration.InnerText = duration.ToString();



        }
        internal bool IsTodayisHoliday(DateTime date)
        {
            const bool returnValue = false;
            var holidayList = SPContext.Current.Web.Lists.TryGetList(Utilities.HolidayList);
            var holidaysdateList = new List<string>();
            if (holidayList != null)
            {
                var holidays = GetListItemCollection(holidayList,
                                                     "HolidayType", "Company Holiday", "Year",
                                                     hdnCurrentYear.Value);
                holidaysdateList.AddRange(from SPListItem holiday in holidays
                                          select
                                              DateTime.Parse(holiday["Date"].ToString()).
                                              ToShortDateString());
            }

            //var tempList =
            //    holidaysdateList.Where(a => date.ToShortDateString().Contains(a)).ToList();
            int holidayCount = 0;
            foreach (string holiday in holidaysdateList)
            {
             string holidayTemp=   GetFormatedDate(holiday);
              holidayTemp= holidayTemp.Replace("/", string.Empty);
              string datetemp = GetFormatedDate(date.ToShortDateString()).Replace("/", string.Empty);
                if(holidayTemp==datetemp)
                {
                    holidayCount = holidayCount + 1;
                }

            }
            if (holidayCount != 0)
            {
                return true;
            }
            else
            {
                return (date.DayOfWeek == DayOfWeek.Sunday ||
                                     date.DayOfWeek == DayOfWeek.Saturday)
                                        ? true
                                        : false;
            }
            // holidaysdateList.Contains(AbortTransaction=)


            return returnValue;
        }

        public void LoadOptionalHolidays()
        {
            lstboxOptionalLeaves.Items.Clear();
            using (var site = new SPSite(SPContext.Current.Site.Url))
            {
                using (var web = site.OpenWeb())
                {
                    SPUser user = web.CurrentUser;

                    hdnCurrentUsername.Value = user.Name;

                    SPListItemCollection optionalHolidays = GetListItemCollection(web.Lists[Utilities.HolidayList], "HolidayType", "Optional");
                    foreach (SPListItem optionalHoliday in optionalHolidays)
                    {
                        lstboxOptionalLeaves.Items.Add(DateTime.Parse(optionalHoliday["Date"].ToString()).ToShortDateString());
                    }
                }
            }
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



        private DateTime GetLastDayOfMonth(DateTime dtDate)
        {
            DateTime dtTo = dtDate;
            dtTo = dtTo.AddMonths(1);
            dtTo = dtTo.AddDays(-(dtTo.Day));

            return dtTo;

        }
        private DateTime GetFirstDayOfMonth(DateTime dtDate)
        {
            DateTime dtFrom = dtDate;
            dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1));

            return dtFrom;

        }


    }
}