using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.Globalization;
using Microsoft.SharePoint.WebControls;

namespace Leave_Application.CompoffRequestForm
{
    public partial class CompoffRequestFormUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                    dateTimeStartDate.SelectedDate = DateTime.Now;
                    dateTimeEndDate.SelectedDate = DateTime.Now;


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




                                // holidaysdateList.Contains(AbortTransaction=)

                                txtDuration.Value = "1"; lblDuration.InnerText = txtDuration.Value;

                                //var oSerializer = new JavaScriptSerializer();
                                //  string sJSON = oSerializer.Serialize(holidaysdateList.ToArray());

                                //      hdnHolidayList.Value = sJSON;

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

        internal SPListItemCollection GetListItemCollection(SPList spList, string key, string value)
        {
            // Return list item collection based on the lookup field

            SPField spField = spList.Fields[key];
            var query = new SPQuery
            {
                Query =
                    @"<Where>
                                <Eq>
                                    <FieldRef Name='" + spField.InternalName +
                    @"'/><Value Type='" + spField.Type.ToString() + @"'>" + value + @"</Value>
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

        internal void SetDate()
        {
            string leaveSDate = ((TextBox)(dateTimeStartDate.Controls[0])).Text;
            string leaveEDate = ((TextBox)(dateTimeEndDate.Controls[0])).Text;
            DateTime tempstartdate = DateTime.Parse(leaveSDate);

            DateTime tempenddate = DateTime.Parse(leaveEDate);// dateTimeEndDate.SelectedDate;

            var totaldays = (tempenddate - tempstartdate).TotalDays + 1;

            var duration = totaldays < 0 ? 0 : totaldays;

            txtDuration.Value = duration.ToString();
            lblDuration.InnerText = duration.ToString();



        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string leaveSDate = ((TextBox)(dateTimeStartDate.Controls[0])).Text;
            string leaveEDate = ((TextBox)(dateTimeEndDate.Controls[0])).Text;

            if (!ValidateDate(leaveSDate, "MM/dd/yyyy"))
            {
                lblError.Text = "Date format should be in 'MM/DD/YYYY' format.";
                lblError.Text += "<br/>";
                dateTimeStartDate.ClearSelection();
                dateTimeStartDate.Focus();
                return;

            }
            if (!ValidateDate(leaveEDate, "MM/dd/yyyy"))
            {
                lblError.Text = "Date format should be in 'MM/DD/YYYY' format.";
                lblError.Text += "<br/>";
                dateTimeEndDate.ClearSelection();
                dateTimeEndDate.Focus();
                return;

            }
            try
            {
                SetDate();

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
                                var list = web.Lists.TryGetList(Utilities.CompoffList);
                                web.AllowUnsafeUpdates = true;



                                SPListItem newItem = list.Items.Add();

                              
                                var entity = new PickerEntity
                                {
                                    DisplayText =
                                        new SPFieldUserValue(web, hdnReportingTo.Value)
                                        .
                                        User.
                                        LoginName
                                };


                                newItem["RequestedFrom"] = web.AllUsers[web.CurrentUser.LoginName];
                                newItem["RequestedTo"] = web.AllUsers[entity.DisplayText];

                                newItem["Worked for"] = txtPurpose.Text;
                                newItem["EmpID"] = lblEmpID.Text;
                               // newItem["Desgination"] = lblDesgination.Text;
                                //newItem["Department"] = lblDepartment.Text;
                                newItem["Duration"] = txtDuration.Value;
                                newItem["Year"] = hdnCurrentYear.Value;

                                newItem["Starting Date"] = DateTime.Parse(leaveSDate);
                                newItem["Ending Date"] = DateTime.Parse(leaveEDate);
                                //dateTimeEndDate.SelectedDate;

                                newItem.Update();



                                Server.Transfer(Request.RawUrl);




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
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtDuration.Value = "1";
            lblDuration.InnerText = "1";
            dateTimeStartDate.SelectedDate = DateTime.Now;
            dateTimeEndDate.SelectedDate = DateTime.Now;
            txtPurpose.Text = string.Empty;

        }
    }
}
