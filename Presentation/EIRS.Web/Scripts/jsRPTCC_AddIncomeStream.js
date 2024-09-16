$(document).ready(function () {
    $('#cboBusinessType').on("change", jsfn_onBusinessTypeChange);
});



function jsfn_onBusinessTypeChange() {
    var vBusinessTypeID = $('#cboBusinessType').val();

    var vData = {
        BusinessTypeID: vBusinessTypeID == '' ? 0 : vBusinessTypeID
    };

    jsfn_ajaxPost('/Base/BusinessTypeChange', vData, jsfn_onBusinessTypeChangeResponse);
}

function jsfn_onBusinessTypeChangeResponse(data) {
    $("#cboBusinessOperation").html("");

    $("#cboBusinessOperation").append(
        $('<option></option>').val("").html("Select Business Operation"));

    $.each(data.BusinessOperation, function (i, bcat) {
        $("#cboBusinessOperation").append(
            $('<option></option>').val(bcat.id).html(bcat.text));
    });

    $('#cboBusinessOperation').selectpicker('refresh');
}
