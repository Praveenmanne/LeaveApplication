using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Leave_Application.EmployeeLeaveStatus
{
    [ToolboxItemAttribute(false)]
    public class EmployeeLeaveStatus : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/Leave_Application/EmployeeLeaveStatus/EmployeeLeaveStatusUserControl.ascx";

        protected override void CreateChildControls()
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
                            Control control = Page.LoadControl(_ascxPath);
                            Controls.Add(control);
                        }
                    }
                }
            }

            //Control control = Page.LoadControl(_ascxPath);
            //Controls.Add(control);
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