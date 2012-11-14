using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Leave_Application.MyTeamMemberDetails
{
    public partial class MyTeamMemberDetailsUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var currentYear = SPContext.Current.Web.Lists.TryGetList(Utilities.CurrentYear).GetItems();
            foreach (SPListItem currentYearValue in currentYear)
            {
                hdnCurrentYear.Value = currentYearValue["Title"].ToString();
            }
            ViewState["Result"] = GetEmployeeLeaves();

            // grdvwEmployeeLeaves.DataBind();
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

        internal DataTable GetEmployeeLeaves()
        {
            DataTable dataTable = EmployeeLeavesStructure();

            try
            {
                SPListItemCollection employeeList;
                SPUser user = SPContext.Current.Web.CurrentUser;

                string currentUser = user.Name;
                if (IsMemberInGroup("Admin"))
                {
                    employeeList = SPContext.Current.Web.Lists.TryGetList(Utilities.EmployeeScreen).GetItems();
                }
                else
                {
                    employeeList =
                        GetListItemCollection(SPContext.Current.Web.Lists.TryGetList(Utilities.EmployeeScreen),
                                              Utilities.Manager, currentUser);
                }

                foreach (SPListItem employee in employeeList)
                {
                    DataRow dataRow = dataTable.NewRow();

                    dataRow["Employee Id"] = employee["Employee ID"].ToString();
                    var entity = new PickerEntity
                                     {
                                         DisplayText =
                                             new SPFieldUserValue(SPContext.Current.Web,
                                                                  employee["Employee Name"].ToString()).User.
                                             Name
                                     };
                    var entityRep = new PickerEntity
                                        {
                                            DisplayText =
                                                new SPFieldUserValue(SPContext.Current.Web,
                                                                     employee[Utilities.Manager].ToString()).User.
                                                Name
                                        };
                    dataRow["Employee Name"] = entity.DisplayText.ToString();
                    dataRow["Reporting To"] = entityRep.DisplayText.ToString();

                    // var spvemptype = new SPFieldLookupValue(employee[Utilities.EmployeeType].ToString());
                    dataRow["Employee Type"] = employee[Utilities.EmployeeType].ToString();
                    var employeeLeaveList =
                        GetListItemCollection(SPContext.Current.Web.Lists.TryGetList(Utilities.EmployeeLeaves),
                                              "Employee ID", employee["Employee ID"].ToString(), "Year",
                                              hdnCurrentYear.Value);
                    foreach (SPListItem empleaves in employeeLeaveList)
                    {
                        var spv = new SPFieldLookupValue(empleaves[Utilities.LeaveType].ToString());
                        decimal leaveBalance = 0;
                        decimal total = 0;

                        leaveBalance = decimal.Parse(empleaves["Leave Balance"].ToString());
                        total = decimal.Parse(empleaves["Leave Balance"].ToString()) +
                                decimal.Parse(empleaves["Leave Requested"].ToString()) +
                                decimal.Parse(empleaves["Leave utilized"].ToString());
                        dataRow[spv.LookupValue.Trim()] = leaveBalance + "/" + total;
                    }
                    dataTable.Rows.Add(dataRow);
                }
            }

            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
            return dataTable;
        }

        internal DataTable EmployeeLeavesStructure()
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Employee Id");
            dataTable.Columns.Add("Employee Name");
            dataTable.Columns.Add("Employee Type");
            dataTable.Columns.Add("Reporting To");
            var items = SPContext.Current.Web.Lists.TryGetList(Utilities.LeaveType).GetItems();
            foreach (SPListItem item in items)
            {
                //  var spv = new SPFieldLookupValue(item[Utilities.LeaveType].ToString());
                dataTable.Columns.Add(item[Utilities.LeaveType].ToString());
            }
            return dataTable;
        }

        public bool IsMemberInGroup(string groupName)
        {
            bool memberInGroup;
            using (var site = new SPSite(SPContext.Current.Site.Url))
            {
                using (var web = site.OpenWeb())
                {
                    memberInGroup = web.IsCurrentUserMemberOfGroup(web.Groups[groupName].ID);
                }
            }

            return memberInGroup;
        }
    }
}