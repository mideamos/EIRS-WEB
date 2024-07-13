var vTaxOfficeDataTable;

jQuery(document).ready(function () {
    vTaxOfficeDataTable = jsfn_ConvertToDatatable($('#tbTaxOffice'));
});

function jsfn_ChangeStatus(toffid) {
    var vData = {
        TaxOfficeID: toffid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/TaxOffice/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vTaxOfficeDataTable.destroy();
        $('#tbody').html(data.TaxOfficeList);
        vTaxOfficeDataTable = jsfn_ConvertToDatatable($('#tbTaxOffice'));
        $("#tbTaxOffice thead th input[type=text]").each(function () {
            vTaxOfficeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

