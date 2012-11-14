using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace Leave_Application.ResignationForm
{
    public partial class ResignationFormUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                dateTimeDate.SelectedDate = DateTime.Now;
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


                        string currentFnclYear = hdnCurrentYear.Value;


                        string finalcialEndYear =
                            currentFnclYear.Substring(
                                currentFnclYear.IndexOf("-", System.StringComparison.Ordinal) + 1);
                        string finalcialStartYear = currentFnclYear.Substring(0, 4);

                        hdnFinancialStratmonth.Value = string.Empty;
                        var financialStrartMonthllist =
                            SPContext.Current.Web.Lists.TryGetList(Utilities.Financialstartmonth).GetItems();
                        foreach (SPListItem financialStrartmonth in financialStrartMonthllist)
                        {
                            hdnFinancialStratmonth.Value = financialStrartmonth["Title"].ToString();
                        }

                        string tempfinStrtDate = hdnFinancialStratmonth.Value.Trim() + "/01/" + finalcialStartYear;
                        string tempfinEndDate = (int.Parse(hdnFinancialStratmonth.Value) - 1).ToString() + "/01/" +
                                                finalcialEndYear;

                        hdnFnclStarts.Value =
                            GetFirstDayOfMonth(DateTime.Parse(tempfinStrtDate)).ToShortDateString();
                        hdnFnclEnds.Value =
                            GetLastDayOfMonth(DateTime.Parse(tempfinEndDate)).ToShortDateString();

                        var list = SPContext.Current.Web.Lists.TryGetList(Utilities.EmployeeScreen);

                        if (list != null)
                        {
                            ddlEmployee.Items.Clear();
                            ddlEmployee.Items.Add("--Select--");
                            var collection = GetListItemCollection(list, "Status", "Active");//, "Employee Type", "Permanent");
                            foreach (SPListItem item in collection)
                            {
                                var spv = new SPFieldLookupValue(item["Employee Name"].ToString());
                                var listItem = new ListItem(spv.LookupValue,
                                                                 item["Employee Name"].ToString());
                                ddlEmployee.Items.Add(listItem);
                            }
                        }
                    }
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

        internal Employee GetEmployeedetails(string name)
        {
            var empEntity = new Employee();
            using (var site = new SPSite(SPContext.Current.Site.Url))
            {
                using (var web = site.OpenWeb())
                {
                    SPUser user = web.CurrentUser;

                    hdnCurrentUsername.Value = user.Name;

                    SPListItemCollection currentUserDetails = GetListItemCollection(web.Lists[Utilities.EmployeeScreen], "Employee Name", name, "Status", "Active");
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

        protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            if (ddlEmployee.SelectedIndex > 0)
            {
                Employee employee = GetEmployeedetails(ddlEmployee.SelectedItem.Text);
                lblEmpID.Text = employee.EmpId;
                lblDesgination.Text = employee.Desigination;
                lblDepartment.Text = employee.Department;
                hdnEmployeeType.Value = employee.EmployeeType;
                var employeeleavelist = SPContext.Current.Web.Lists.TryGetList(Utilities.EmployeeLeaves);

                var empleaveCollection = GetListItemCollection(employeeleavelist, "Employee ID", lblEmpID.Text, "Leave Type",
                                                               "Paid Leave", "Year",hdnCurrentYear.Value);
                foreach (SPListItem emploeave in empleaveCollection)
                {
                    lblplCurrent.Text = emploeave["Leave Balance"].ToString();

                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblError.Text = string.Empty;
            string rDate = ((TextBox)(dateTimeDate.Controls[0])).Text;

            if (ddlEmployee.SelectedIndex < 1)
            {
                lblError.Text = "Please select Emplopyee Name.";
                lblError.Text += "<br/>";
                return;
            }
            if (!ValidateDate(rDate, "MM/dd/yyyy"))
            {
                lblError.Text = "Date format should be in 'MM/DD/YYYY' format.";
                lblError.Text += "<br/>";
                dateTimeDate.SelectedDate = DateTime.Now;
                return;

            }

            try
            {

                
                var employeeleavelist = SPContext.Current.Web.Lists.TryGetList(Utilities.EmployeeLeaves);

                var empleaveCollection = GetListItemCollection(employeeleavelist, "Employee ID", lblEmpID.Text, "Leave Type",
                                                              "Paid Leave", "Year", hdnCurrentYear.Value);
                var leavelist = SPContext.Current.Web.Lists.TryGetList(Utilities.LeaveDays);
                var leavelistitems = GetListItemCollection(leavelist, "Leave Type", "Paid Leave", "Employee Type",
                                                           hdnEmployeeType.Value);
                decimal leaveday = 0;


                foreach (SPListItem leavelistitem in leavelistitems)
                {
                    leaveday = decimal.Parse(leavelistitem["Leave Days"].ToString());
                }
                int dopMonth = DateTime.Parse(GetFormatedDate(rDate)).Month; // DtDOP.SelectedDate.Month;

                int monthDiff = GetMonthDifference(dopMonth);

                decimal lvsNeedtopay = leaveday * monthDiff;
                foreach (SPListItem empLeaveitem in empleaveCollection)
                {
                    {
                        decimal leavesBal = decimal.Parse(empLeaveitem["Leave Balance"].ToString()) -
                                            lvsNeedtopay;
                        if (leavesBal%1 == 0)
                        {
                            int noOfleaves = Convert.ToInt16(leavesBal);
                            empLeaveitem["Leave Balance"] = noOfleaves;
                        }
                        else
                        {
                            empLeaveitem["Leave Balance"] = leavesBal;
                        }
                        empLeaveitem.Update();
                        lblplneedtopay.Text = leavesBal.ToString();
                    }
                    SPListItemCollection currentUserDetails = GetListItemCollection(SPContext.Current.Web.Lists[Utilities.EmployeeScreen], "Employee Name", ddlEmployee.SelectedItem.Text, "Status", "Active");
                     foreach (SPListItem currentUserDetail in currentUserDetails)
                     {
                         currentUserDetail["Status"] = "In Active";
                         currentUserDetail.Update();
                     }

                     SPListItemCollection emplyoeeLeaverequestsfrom = GetListItemCollection(SPContext.Current.Web.Lists[Utilities.LeaveRequest], "RequestedFrom", ddlEmployee.SelectedItem.Text);
                     foreach (SPListItem currentUserDetail in emplyoeeLeaverequestsfrom)
                     {
                         currentUserDetail["Employee Status"] = "In Active";
                         currentUserDetail.Update();
                     }
                     //SPListItemCollection emplyoeeLeaverequeststo = GetListItemCollection(SPContext.Current.Web.Lists[Utilities.LeaveRequest], "RequestedTo", ddlEmployee.SelectedItem.Text);
                     //foreach (SPListItem currentUserDetail in emplyoeeLeaverequeststo)
                     //{
                     //    currentUserDetail["Employee Status"] = "In Active";
                     //    currentUserDetail.Update();
                     //}
                   
                    btnSubmit.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }
        private int GetMonthDifference(int currentMonth)
        {
            int strFinacialStratMonth = int.Parse(hdnFinancialStratmonth.Value);
            int returnValue;

            if (currentMonth <= strFinacialStratMonth)
                returnValue = strFinacialStratMonth - currentMonth;
            else
            {
                returnValue = (12 + strFinacialStratMonth) - currentMonth;
            }

            return (returnValue == 0 ? 12 : returnValue);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ddlEmployee.SelectedIndex = 0;
            dateTimeDate.SelectedDate = DateTime.Now;
            lblDepartment.Text = string.Empty;
            lblDesgination.Text = string.Empty;
            lblEmpID.Text = string.Empty;
            lblError.Text = string.Empty;
            lblplCurrent.Text = string.Empty;
            lblplneedtopay.Text = string.Empty;
        }

    }
}
