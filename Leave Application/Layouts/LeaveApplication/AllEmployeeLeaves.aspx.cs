using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace Leave_Application.Layouts.LeaveApplication
{
    public partial class AllEmployeeLeaves : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SPUser user = SPContext.Current.Web.CurrentUser;
            string empid = string.Empty;
            SPListItemCollection currentUserDetails = GetListItemCollection(SPContext.Current.Web.Lists[Utilities.EmployeeScreen], "Employee Name", user.Name, "Status", "Active");
            foreach (SPListItem currentUserDetail in currentUserDetails)
            {
                empid = currentUserDetail[Utilities.EmployeeId].ToString();
            }
            if (string.IsNullOrEmpty(empid))
            {
                if (!IsMemberInGroup("Admin"))
                {
                    Response.Redirect("/_layouts/LeaveApplication/unauthorised.aspx");
                }
            }
            else if (IsMemberInGroup("Employee"))//|| !IsMemberInGroup("Managers"))
            {
                Exception ex = new Exception("You do not have permission to view this page");
                SPUtility.TransferToErrorPage(ex.Message);

            }
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