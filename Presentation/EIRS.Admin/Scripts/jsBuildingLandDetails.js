$(document).ready(function () {

    jsfn_BuildDataTable();

    $('#btnSave').click(function () {
        if ($("#hdnLandId").val() != '' && $("#hdnLandId").val() != '0') {
            $("#frmLand").submit();
        }
        else {
            jsfn_ShowAlert('Select atleast one land', 'danger');
        }

        return false;
    });
});



function jsfn_SelectSingleLand(rbLand) {
    $("#hdnLandId").val(rbLand.dataset["lndid"]);
}

function jsfn_BuildDataTable() {
    vLandDataTable = jsfn_ConvertToDatatable($('#tbLand'));
    //Hide Columns
    vLandDataTable.column(4).visible(false);
    vLandDataTable.column(5).visible(false);
    vLandDataTable.column(6).visible(false);
    vLandDataTable.column(7).visible(false);
    vLandDataTable.column(8).visible(false);
    vLandDataTable.column(9).visible(false);
    vLandDataTable.column(10).visible(false);
    vLandDataTable.column(11).visible(false);
}