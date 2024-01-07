var vTaxPayerTable;

$(document).ready(function () {
    vTaxPayerTable = jsfn_ConvertToDatableWithCustomSort($('#tblTaxPayerList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Tax Payers Found');
});

function jsfn_ChangeTaxPayerLinkStatus(tpaid, assetid, asttypeid) {
    var vData = {
        TPAID: tpaid,
        AssetID: assetid,
        AssetTypeID: asttypeid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/ProfileBusiness/UpdateTaxPayerStatus', vData, jsfn_ChangeTaxPayerLinkStatusResponse);
}

function jsfn_ChangeTaxPayerLinkStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vTaxPayerTable.fnDestroy();
        $('#tbTaxPayer').html(data.TaxPayerList);
        vTaxPayerTable = jsfn_ConvertToDatableWithCustomSort($('#tblTaxPayerList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Tax Payers Found');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}


