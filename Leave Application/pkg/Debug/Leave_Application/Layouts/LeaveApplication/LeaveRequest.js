$(document).ready(function () {
    var optinalDates = $("[id$='optinalDates']");
    var ddlTypeofLeave = $("[id$='ddlTypeofLeave']");
    var selecteddates = $("[id$='Selecteddates']");
    optinalDates.hide();
    //  $('#<%= optinalDates.ClientID %>').hide();
    ddlTypeofLeave.change(function () {
        var selectedValue = $.trim($(this).val());
        if (selectedValue == "Optional") {
            optinalDates.show();
            selecteddates.hide();
            DateCompare();
        } else {
            selecteddates.show();
            optinalDates.hide();
            DateCompare();
        }
        //   $('.box').hide();
        // $('#div' + $(this).val()).show();
    });
});

/*
function openDialog() {
var options = SP.UI.$create_DialogOptions();
options.url = 'http://tspsrvr:42903/Pages/LeaveStatus.aspx';
options.height = 650;
options.dialogReturnValueCallback = function (result, returnValue) {
window.location.href = window.location.href;
};
SP.UI.ModalDialog.showModalDialog(options);
}*/