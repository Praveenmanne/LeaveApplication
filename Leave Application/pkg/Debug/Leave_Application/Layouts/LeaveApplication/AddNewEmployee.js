$(document).ready(function () {
    var divDop = $("[id$='divdop']");
    /*  var ddlempTypee = $("[id$='DdlEmptype']");*/

    /*  divDop.hide();*/
    $("[id$='DdlEmptype']").change(function () {
        var selectedValue = $.trim($(this).val());
        if (selectedValue == "Permanent") {
            divDop.show();
        } else {
            divDop.hide();
        }
    });
});