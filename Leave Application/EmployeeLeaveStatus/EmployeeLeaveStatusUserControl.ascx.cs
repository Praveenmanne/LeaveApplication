using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;

namespace Leave_Application.EmployeeLeaveStatus
{
    public partial class EmployeeLeaveStatusUserControl : UserControl
    {
        //protected void Page_Load(object sender, EventArgs e) { }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                var currentYear = SPContext.Current.Web.Lists.TryGetList(Utilities.CurrentYear).GetItems();
                foreach (SPListItem currentYearValue in currentYear)
                {
                    hdnCurrentYear.Value = currentYearValue["Title"].ToString();
                }
                LoadDetails();
            }
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


        protected void BtnApproveClick(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                var leaves = new List<LeaveStatus>();

                foreach (var key in Request.Form.Keys)
                {
                    var leave = new LeaveStatus();
                    if (key.ToString().IndexOf("Chk") == 0)
                    {
                        if (Request.Form[key.ToString()].ToString().ToLower() == "on")
                        {
                            count++;
                            var Id = key.ToString().Substring(key.ToString().IndexOf("Chk") + 3);
                            var comments = Request.Form["txt" + Id].ToString();
                            leave.Id = int.Parse(Id);
                            leave.Reason = comments;
                            leaves.Add(leave);
                        }
                    }
                }

                //if (false)
                //{
                //    foreach (GridViewRow row in grid.Rows)
                //    {
                //        var checkbox = (CheckBox)row.FindControl("chkbox");
                //        if (checkbox.Checked)
                //        {
                //            count++;
                //            var txtRemark = (TextBox)row.FindControl("txtreason");
                //            var leave = new LeaveStatus();
                //            count++;
                //            leave.Id = int.Parse(row.Cells[0].Text);
                //            leave.Reason = txtRemark.Text;
                //            leaves.Add(leave);
                //        }
                //    }
                //}
                if (leaves.Count != 0)
                {
                    ChangeStatus(leaves, "Approved");
                    LoadDetails();
                }
                else
                {
                    lblErr.Text = "Please select the request(s) to approve";
                }
            }
            catch (Exception ex)
            {
                lblErr.Text = ex.Message;
            }
        }

        protected void BtnRejectClick(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                var leaves = new List<LeaveStatus>();

                foreach (var key in Request.Form.Keys)
                {
                    var leave = new LeaveStatus();

                    if (key.ToString().IndexOf("Chk") == 0)
                    {
                        if (Request.Form[key.ToString()].ToString().ToLower() == "on")
                        {
                            count++;
                            var Id = key.ToString().Substring(key.ToString().IndexOf("Chk") + 3);
                            var comments = Request.Form["txt" + Id].ToString();
                            leave.Id = int.Parse(Id);
                            leave.Reason = comments;
                            leaves.Add(leave);
                        }
                    }
                }

                //int count = 0;
                //var leaves = new List<LeaveStatus>();
                //foreach (GridViewRow row in grid.Rows)
                //{
                //    var checkbox = (CheckBox)row.FindControl("chkbox");
                //    if (checkbox.Checked)
                //    {
                //        var txtRemark = (TextBox)row.FindControl("txtreason");
                //        var leave = new LeaveStatus();
                //        count++;
                //        leave.Id = int.Parse(row.Cells[0].Text);
                //        leave.Reason = txtRemark.Text;
                //        leaves.Add(leave);
                //    }
                //}
                if (leaves.Count != 0)
                {
                    ChangeStatus(leaves, "Rejected");
                    LoadDetails();
                }
                else
                {
                    lblErr.Text = "Please select the request(s) to reject";
                }
            }
            catch (Exception ex)
            {
                lblErr.Text = ex.Message;
            }
        }

        public void LoadDetails()
        {
            try
            {
                using (var site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (var web = site.OpenWeb())
                    {
                        var leavestatus = new DataTable();
                        leavestatus.Columns.Add("Id");
                        leavestatus.Columns.Add("RequestedFrom");
                        leavestatus.Columns.Add("Leave Type");
                        leavestatus.Columns.Add("Starting Date");
                        leavestatus.Columns.Add("Employee Type");
                        leavestatus.Columns.Add("Ending Date");
                        leavestatus.Columns.Add("Duration");
                        leavestatus.Columns.Add("Reason");
                        leavestatus.Columns.Add("Remarks");
                        leavestatus.Columns.Add("Status");
                        var emplist = web.Lists.TryGetList(Utilities.LeaveRequest);

                        if (emplist != null)
                        {
                            SPUser user = web.CurrentUser;

                            string currentUser = user.Name;
                            SPListItemCollection currentUserDetails;

                            if (IsMemberInGroup("Admin"))
                            {
                                currentUserDetails = GetListItemCollection(emplist, "Status", "Pending", "Year",
                                                                          hdnCurrentYear.Value, "Employee Status", "Active");
                            }
                            else
                            {
                                currentUserDetails = GetListItemCollection(emplist, "RequestedTo",
                                                                           currentUser, "Status", "Pending", "Year",
                                                                           hdnCurrentYear.Value, "Employee Status", "Active");
                            }
                            if (currentUserDetails.Count > 0)
                            {
                                foreach (SPListItem item in currentUserDetails)
                                {
                                    var spv = new SPFieldLookupValue(item["RequestedFrom"].ToString());

                                    DataRow dataRow = leavestatus.NewRow();
                                    dataRow["Id"] = item.ID;
                                    dataRow["RequestedFrom"] = spv.LookupValue;//item["RequestedFrom"].ToString();
                                    dataRow["Leave Type"] = item["Leave Type"].ToString();

                                    //dataRow["Applied Date"] = item["Starting Date"].ToString();
                                    dataRow["Starting Date"] = DateTime.Parse(item["Starting Date"].ToString()).ToShortDateString();
                                    dataRow["Ending Date"] = DateTime.Parse(item["Ending Date"].ToString()).ToShortDateString();
                                    dataRow["Duration"] = item["Leave Days"].ToString();

                                    if (item["Purpose of Leave"] != null)
                                    {
                                        SPFieldMultiLineText mlt = item.Fields.GetField("Purpose of Leave") as SPFieldMultiLineText;

                                        dataRow["Reason"] = mlt.GetFieldValueAsText(item["Purpose of Leave"]);
                                    }

                                    //dataRow["Reason"] = (item["Purpose of Leave"] == null) ? "" : item["Purpose of Leave"].ToString();
                                    dataRow["Remarks"] = (item["Remarks"] == null)
                                                             ? ""
                                                             : item["Remarks"].ToString();

                                    //dataRow["App/Rej by Remarks"] = item.ToString();
                                    dataRow["Status"] = item["Status"].ToString();

                                    leavestatus.Rows.Add(dataRow);
                                }
                            }
                            else
                            {
                                BtnApprove.Visible = false;
                                BtnReject.Visible = false;
                                //  lblErr.Text = "No records found";
                            }
                        }

                        DataView dataView = new DataView(leavestatus);
                        dataView.Sort = "Id DESC";

                        //grid.DataSource = leavestatus;

                        //grid.DataBind();
                        ViewState["Results"] = leavestatus;
                    }
                }
            }
            catch (Exception ex)
            {
                lblErr.Text = ex.Message;
            }
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

        internal void ChangeStatus(List<LeaveStatus> leaves, string status)
        {
            using (var site = new SPSite(SPContext.Current.Site.Url))
            {
                using (var web = site.OpenWeb())
                {
                    foreach (var leave in leaves)
                    {
                        web.AllowUnsafeUpdates = true;
                        var leavelist = web.Lists.TryGetList(Utilities.LeaveRequest);
                        SPListItem item = leavelist.GetItemById(leave.Id);

                        string empid = item["EmpID"].ToString();

                        // var spv = new SPFieldLookupValue(item["Leave Type"].ToString());
                        string leaveType = item["Leave Type"].ToString().Trim();
                        decimal duration = decimal.Parse(item["Leave Days"].ToString());

                        var empLeaveList = GetListItemCollection(web.Lists.TryGetList(Utilities.EmployeeLeaves), "Employee ID",
                                                                 empid, "Leave Type", leaveType);
                        if (status.ToLower() == "approved")
                        {
                            if (empLeaveList.Count != 0)
                            {
                                foreach (SPListItem empLeaveitem in empLeaveList)
                                {
                                    decimal leavesBal = decimal.Parse(empLeaveitem["Leave Requested"].ToString()) - duration;

                                    if (leavesBal % 1 == 0)
                                    {
                                        int noOfleaves = Convert.ToInt16(leavesBal);
                                        empLeaveitem["Leave Requested"] = noOfleaves;
                                    }
                                    else
                                    {
                                        empLeaveitem["Leave Requested"] = leavesBal;
                                    }

                                    decimal leavesReq = decimal.Parse(empLeaveitem["Leave utilized"].ToString()) + duration;
                                    if (leavesReq % 1 == 0)
                                    {
                                        int noOfleaves = Convert.ToInt16(leavesReq);
                                        empLeaveitem["Leave utilized"] = noOfleaves;
                                    }
                                    else
                                    {
                                        empLeaveitem["Leave utilized"] = leavesReq;
                                    }
                                    empLeaveitem.Update();
                                }
                            }
                        }
                        else if (status.ToLower() == "rejected")
                        {
                            if (empLeaveList.Count != 0)
                            {
                                foreach (SPListItem empLeaveitem in empLeaveList)
                                {
                                    decimal leavesBal = decimal.Parse(empLeaveitem["Leave Requested"].ToString()) - duration;

                                    if (leavesBal % 1 == 0)
                                    {
                                        int noOfleaves = Convert.ToInt16(leavesBal);
                                        empLeaveitem["Leave Requested"] = noOfleaves;
                                    }
                                    else
                                    {
                                        empLeaveitem["Leave Requested"] = leavesBal;
                                    }
                                    if (leaveType.ToLower() != "lop")
                                    {
                                        decimal leavesReq = decimal.Parse(empLeaveitem["Leave Balance"].ToString()) +
                                                            duration;
                                        if (leavesReq % 1 == 0)
                                        {
                                            int noOfleaves = Convert.ToInt16(leavesReq);
                                            empLeaveitem["Leave Balance"] = noOfleaves;
                                        }
                                        else
                                        {
                                            empLeaveitem["Leave Balance"] = leavesReq;
                                        }
                                    }
                                    empLeaveitem.Update();
                                }
                            }
                        }

                        item["Status"] = status;
                        item["Remarks"] = leave.Reason;
                        item.Update();
                    }
                }
            }
        }

        public class LeaveStatus
        {
            public int Id { get; set; }

            public string Reason { get; set; }
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

        internal SPListItemCollection GetListItemCollection(SPList spList, string keyOne, string valueOne, string keyTwo, string valueTwo, string keyThree, string valueThree, string keyFour, string valueFour)
        {
            // Return list item collection based on the lookup field

            SPField spFieldOne = spList.Fields[keyOne];
            SPField spFieldTwo = spList.Fields[keyTwo];
            SPField spFieldThree = spList.Fields[keyThree];
            SPField spFieldFour = spList.Fields[keyFour];
            var query = new SPQuery
            {
                Query = @"<Where>
                          <And>
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
                             <Eq>
                                <FieldRef Name=" + spFieldFour.InternalName + @" />
                                <Value Type=" + spFieldFour.Type.ToString() + @">" + valueFour + @"</Value>
                             </Eq>
                          </And>
                       </Where>"
            };

            return spList.GetItems(query);
        }
    }
}