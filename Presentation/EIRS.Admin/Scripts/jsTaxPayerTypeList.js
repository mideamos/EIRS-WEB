var vTaxPayerTypeDataTable;

jQuery(document).ready(function () {
    vTaxPayerTypeDataTable = jsfn_ConvertToDatatable($('#tbTaxPayerType'));
});

function jsfn_ChangeStatus(TaxPayerTypeid) {
    var vData = {
        TaxPayerTypeID: TaxPayerTypeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/TaxPayerType/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vTaxPayerTypeDataTable.destroy();
        $('#tbody').html(data.TaxPayerTypeList);
        vTaxPayerTypeDataTable = jsfn_ConvertToDatatable($('#tbTaxPayerType'));
        $("#tbTaxPayerType thead th input[type=text]").each(function () {
            vTaxPayerTypeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

