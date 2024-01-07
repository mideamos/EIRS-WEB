$(document).ready(function () {

    jsfn_ConvertToDatatable($('#tbBuildingUnit'));

    $('#btnSave').click(function () {
        if ($("#hdnUnitNumberIds").val() != '' && $("#hdnUnitNumberIds").val() != '0') {
            $("#frmUnitNumber").submit();
        }
        else {
            jsfn_ShowAlert('Select atleast one building unit', 'danger');
        }

        return false;
    });
});


function jsfn_MultiSelectBuildingUnit(chkBuildingUnit) {
    var vArrBuildingUnit = $('#hdnUnitNumberIds').val().split(',');
    if (chkBuildingUnit.checked) {
        if ($('#hdnUnitNumberIds').val() != "")
            vArrBuildingUnit.push(chkBuildingUnit.dataset["buid"]);
        else {
            vArrBuildingUnit = new Array();
            vArrBuildingUnit.push(chkBuildingUnit.dataset["buid"]);
        }
    }
    else {
        vArrBuildingUnit.splice($.inArray(chkBuildingUnit.dataset["buid"], vArrBuildingUnit), 1);
    }

    $('#hdnUnitNumberIds').val(vArrBuildingUnit.toString());
}