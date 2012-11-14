using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace Leave_Application.Layouts.LeaveApplication
{
    public partial class UpdateLeaveDays : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsMemberInGroup("Admin"))
            {
                Exception ex = new Exception("You do not have permission to view this page");
                SPUtility.TransferToErrorPage(ex.Message);

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
    }
}