using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace Leave_Application.LeaveInfo
{
    public partial class LeaveInfoUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentYear = SPContext.Current.Web.Lists.TryGetList(Utilities.CurrentYear).GetItems();
            foreach (SPListItem currentYearValue in currentYear)
            {
                hdnCurrentYear.Value = currentYearValue["Title"].ToString();
            }
            Employee employee = GetEmployeedetails();
            ViewState["Result"] = GetBalanceLeave(employee.EmpId);

            //grvBalanceLeave.DataBind();
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

                    SPListItemCollection currentUserDetails = GetListItemCollection(web.Lists[Utilities.EmployeeScreen], "Employee Name", hdnCurrentUsername.Value,"Status","Active");
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

        private DataTable GetBalanceLeave(string empId)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Leave Type");
            dataTable.Columns.Add("Balance Leave");
            dataTable.Columns.Add("Leave Requested");
            dataTable.Columns.Add("Leave utilized");
            using (var site = new SPSite(SPContext.Current.Site.Url))
            {
                using (var web = site.OpenWeb())
                {
                    var listItem = GetListItemCollection(web.Lists[Utilities.EmployeeLeaves], "Employee ID",
                                                         empId, "Year", hdnCurrentYear.Value);
                    foreach (SPListItem item in listItem)
                    {
                        var spv = new SPFieldLookupValue(item["Leave Type"].ToString());
                        DataRow dataRow = dataTable.NewRow();
                        dataRow["Leave Type"] = spv.LookupValue;
                        dataRow["Balance Leave"] = item["Leave Balance"].ToString();
                        dataRow["Leave Requested"] = item["Leave Requested"];
                        dataRow["Leave utilized"] = item["Leave utilized"];
                        dataTable.Rows.Add(dataRow);
                    }
                }
            }
            dataTable.DefaultView.Sort = "Leave Type ASC"; 

            return dataTable;
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
    }
}