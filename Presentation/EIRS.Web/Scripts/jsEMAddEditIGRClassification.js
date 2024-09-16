$(document).ready(function () {
    $('#cboCategory').on("change", jsfn_onCategoryChange);
});

function jsfn_onCategoryChange() {
    var vCategoryID = $('#cboCategory').val();

    if (vCategoryID !== '') {

        var vData = {
            CategoryID: vCategoryID === '' ? 0 : vCategoryID
        };

        jsfn_ajaxPost('/Base/GetEMRevenueHead', vData, jsfn_onCategoryChangeResponse);
    }
}

function jsfn_onCategoryChangeResponse(data) {
    $("#cboRevenueHead").html("");

    $("#cboRevenueHead").append(
        $('<option></option>').val("").html("Select Revenue Head"));

    if (data !== undefined) {
        $.each(data, function (i, rhead) {
            $("#cboRevenueHead").append(
                $('<option></option>').val(rhead.id).html(rhead.text));
        });

        $('#cboRevenueHead').selectpicker('refresh');
    }
}
