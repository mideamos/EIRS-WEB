$(document).ready(function () {
    $('#cboTaxPayerType').on("change", jsfn_onTaxPayerTypeChange);
    jsfn_bindTable();
});

function jsfn_bindTable() {
    var vColumnsList = [{
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }];

    var vSortOrder = [];

    jsfn_ConvertToDatableWithCustomSort($('#tblGTPRL'), vColumnsList, 'No Report Found', vSortOrder);
}



function jsfn_onTaxPayerTypeChange() {
    var vTaxPayerTypeID = $('#cboTaxPayerType').val();

    var vData = {
        TaxPayerTypeID: vTaxPayerTypeID == '' ? 0 : vTaxPayerTypeID
    };

    jsfn_ajaxPost('/Base/GetTaxPayerList', vData, jsfn_onTaxPayerTypeChangeResponse);
}

function jsfn_onTaxPayerTypeChangeResponse(data) {
    if (data.success) {
        $("#cboTaxPayer").html(""); // clear before appending new list

        $("#cboTaxPayer").append(
            $('<option></option>').val("").html("Select Tax Payer"));

        $.each(data.TaxPayerList, function (i, tp) {
            $("#cboTaxPayer").append(
                $('<option></option>').val(tp.id).html(tp.text));
        });

        $('#cboTaxPayer').selectpicker('refresh');
    }
}