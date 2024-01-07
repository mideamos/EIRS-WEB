$(document).ready(function () {
    $("#btnSetTaxOfficeTarget").click(function () {
        var vTaxYearID = $("#cboTaxYear").val();

        if (vTaxYearID != '') {
            window.location.href = '/OperationManager/SetTaxOfficerTarget?toid=' + $("#hdnTaxOfficeID").val() + '&tofid=' + $("#hdnUserID").val() + '&year=' + vTaxYearID;
        }
        else {
            $("#dvMessage").html('<div class="alert alert-danger">Select Tax Year</div>');
        }
    });

    var vColumnsList = [{
        "orderable": true
    }, {
        "orderable": false
    }];

    var vSortOrder = [];

    jsfn_ConvertToDatableWithCustomSort($('#tblTaxOffice'), vColumnsList, 'No Tax Office Found', vSortOrder);
});

function jsfn_SelectTaxYear(usrid,toid) {
    $("#hdnTaxOfficeID").val(toid);
    $("#hdnUserID").val(usrid);
    $("#dvTaxYear").modal('show');
}