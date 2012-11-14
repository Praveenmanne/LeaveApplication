function DateCompare() {
    var jsondata = document.getElementById("<%= hdnHolidayList.ClientID %>").value;
    var leaveType = document.getElementById("<%= txtTypeofLeave.ClientID %>");
    var fromdate = document.getElementById("<%= dateTimeStartDate.Controls[0].ClientID %>").value;
    var endDate = document.getElementById("<%= dateTimeEndDate.Controls[0].ClientID %>").value;
    var optionalDates = document.getElementById("<%= lstboxOptionalLeaves.ClientID %>");

    var obj = jQuery.parseJSON(jsondata);
    var fValue = new Date(fromdate);
    var eValue = new Date(endDate);

    var selectedLeave = leaveType.value;
    var tempfromdate = fValue;

    while (IsHoliday(tempfromdate, obj)) {
        //  alert(tempfromdate);

        tempfromdate.setDate(tempfromdate.getDate() + 1);
    }

    var tempenddate = eValue;

    while (IsHoliday(tempenddate, obj)) {
        tempenddate.setDate(tempenddate.getDate() - 1);
    }

    var leaveDays;
    leaveDays = DayDifference(tempfromdate, tempenddate);
    // alert(tempfromdate + "----" + tempenddate);
    if (tempfromdate.toString() != tempenddate.toString()) {
        if (leaveDays > 0)
            leaveDays = leaveDays + 1;
        else
            leaveDays = 0;
    } else {
        leaveDays = 1;
    }

    // alert(selectedLeave);
    var i;
    if (selectedLeave == "Comp off") {
        var countWorkingDays = 0;
        //  var tfdate = tempfromdate;
        for (i = tempfromdate; tempfromdate.getTime() != tempenddate.getTime(); i.setDate(i.getDate() + 1)) {
            if (!IsHoliday(tempfromdate, obj))
                countWorkingDays++;
        }
        document.getElementById("<%= txtDuration.ClientID %>").value = countWorkingDays + 1;
        //alert(countWorkingDays);
    }
    else if (selectedLeave == "Optional") {
        var leavesselected = 0;
        for (i = 0; i < optionalDates.options.length; i++) {
            var isSelected = optionalDates.options[i].selected;
            isSelected = (isSelected) ? "selected" : "not selected";

            if (isSelected == "selected")
                leavesselected++;
        }

        document.getElementById("<%= txtDuration.ClientID %>").value = leavesselected;
        // alert(selectedLeave);
    } else {
        // alert(selectedLeave);

        document.getElementById("<%= txtDuration.ClientID %>").value = leaveDays;
    }
}

function DayDifference(tempfromdate, tempenddate) {
    var oneDay = 1000 * 60 * 60 * 24;

    var dayDiff = (Math.ceil((tempenddate.getTime() - tempfromdate.getTime()) / (oneDay)));

    return dayDiff;
}

function IsHoliday(fValue, jsondata) {
    var fdate = new Date(fValue);
    var tdate = fdate.getMonth() + 1 + "/" + fdate.getDate() + "/" + fdate.getFullYear();

    if (jsondata.toString().indexOf(tdate) != -1) {
        return true;
    }

    return IsSatOrSun(tdate);
}
function IsSatOrSun(fValue) {
    var tdate = new Date(fValue);

    if (tdate.getDay() == 0 || tdate.getDay() == 6) {
        return true;
    } else {
        return false;
    }
}