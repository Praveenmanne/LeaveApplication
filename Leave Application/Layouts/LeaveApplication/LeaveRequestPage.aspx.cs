using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Leave_Application.Layouts.LeaveApplication
{
    public partial class LeaveRequestPage : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (var site = new SPSite(SPContext.Current.Site.Url))
                {
                    using (var web = site.OpenWeb())
                    {
                        var emplist = web.Lists.TryGetList(Utilities.ReportingTo);

                        if (emplist != null)
                        {
                            SPUser user = web.CurrentUser;

                            string currentUser = user.Name;

                            SPListItemCollection currentUserDetails = GetListItemCollection(emplist,
                                                                                            "Reporting Managers",
                                                                                            currentUser);

                            if (currentUserDetails.Count > 0)
                            {
                                WebUserControl12.Visible = true;
                            }
                            else
                            {
                                WebUserControl12.Visible = false;
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
                Query = @"<Where>
                                <Eq>
                                    <FieldRef Name='" + spField.InternalName + @"'/><Value Type='" + spField.Type.ToString() + @"'>" + value + @"</Value>
                                </Eq>
                                </Where>"
            };

            return spList.GetItems(query);
        }
    }
}