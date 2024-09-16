$(document).ready(function () {
    $("#btnSetTaxOfficeTarget").click(function () {
        var vTaxYearID = $("#cboTaxYear").val();

        if (vTaxYearID != '') {
            window.location.href = '/OperationManager/SetTaxOfficeTarget?toid=' + $("#hdnTaxOfficeID").val() + '&year=' + vTaxYearID;
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

function jsfn_SelectTaxYear(toid) {
    $("#hdnTaxOfficeID").val(toid);
    $("#dvTaxYear").modal('show');
}