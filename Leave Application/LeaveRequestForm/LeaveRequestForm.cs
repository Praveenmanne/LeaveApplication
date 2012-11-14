using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace Leave_Application.LeaveRequestForm
{
    [ToolboxItemAttribute(false)]
    public class LeaveRequestForm : WebPart
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string AscxPath = @"~/_CONTROLTEMPLATES/Leave_Application/LeaveRequestForm/LeaveRequestFormUserControl.ascx";

        protected override void CreateChildControls()
        {
            Control control = Page.LoadControl(AscxPath);
            Controls.Add(control);
        }
    }
}