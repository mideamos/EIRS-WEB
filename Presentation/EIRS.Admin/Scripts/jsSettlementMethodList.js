var vSettlementMethodDataTable;

jQuery(document).ready(function () {
    vSettlementMethodDataTable = jsfn_ConvertToDatatable($('#tbSettlementMethod'));
});

function jsfn_ChangeStatus(SettlementMethodid) {
    var vData = {
        SettlementMethodID: SettlementMethodid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/SettlementMethod/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vSettlementMethodDataTable.destroy();
        $('#tbody').html(data.SettlementMethodList);
        vSettlementMethodDataTable = jsfn_ConvertToDatatable($('#tbSettlementMethod'));
        $("#tbSettlementMethod thead th input[type=text]").each(function () {
            vSettlementMethodDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

