jQuery(document).ready(function () {
    $('#cboTaxOffice').on("change", jsfn_onTaxOfficeChange);
});

function jsfn_onTaxOfficeChange() {
    var vTaxOfficeID = $('#cboTaxOffice').val();

    if (vTaxOfficeID !== '') {

        var vData = {
            TaxOfficeID: vTaxOfficeID === '' ? 0 : vTaxOfficeID,
            showmanager: true
        };

        jsfn_ajaxPost('/Base/GetTaxOfficeManger', vData, jsfn_onTaxOfficeChangeResponse);
    }
}

function jsfn_onTaxOfficeChangeResponse(data) {
    $("#TaxOfficeManagerID").html("");

    $("#TaxOfficeManagerID").append(
        $('<option></option>').val("").html("Select Tax Office Manager"));

    if (data !== undefined) {
        $.each(data, function (i, tit) {
            $("#TaxOfficeManagerID").append(
                $('<option></option>').val(tit.id).html(tit.text));
        });

        $('#TaxOfficeManagerID').selectpicker('refresh');
    }

    $("#ReplacementManagerID").html("");

    $("#ReplacementManagerID").append(
        $('<option></option>').val("").html("Select Replacement"));

    if (data !== undefined) {
        $.each(data, function (i, tit) {
            $("#ReplacementManagerID").append(
                $('<option></option>').val(tit.id).html(tit.text));
        });

        $('#ReplacementManagerID').selectpicker('refresh');
    }
}