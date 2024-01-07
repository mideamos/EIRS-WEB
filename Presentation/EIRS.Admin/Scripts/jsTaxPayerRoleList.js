var vTaxPayerRoleDataTable;

jQuery(document).ready(function () {
    vTaxPayerRoleDataTable = jsfn_ConvertToDatatable($('#tbTaxPayerRole'));
});

function jsfn_ChangeStatus(TaxPayerRoleid) {
    var vData = {
        TaxPayerRoleID: TaxPayerRoleid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/TaxPayerRole/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vTaxPayerRoleDataTable.destroy();
        $('#tbody').html(data.TaxPayerRoleList);
        vTaxPayerRoleDataTable = jsfn_ConvertToDatatable($('#tbTaxPayerRole'));
        $("#tbTaxPayerRole thead th input[type=text]").each(function () {
            vTaxPayerRoleDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

