var vSettlementStatusDataTable;

jQuery(document).ready(function () {
    vSettlementStatusDataTable = jsfn_ConvertToDatatable($('#tbSettlementStatus'));
});

function jsfn_ChangeStatus(SettlementStatusid) {
    var vData = {
        SettlementStatusID: SettlementStatusid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/SettlementStatus/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vSettlementStatusDataTable.destroy();
        $('#tbody').html(data.SettlementStatusList);
        vSettlementStatusDataTable = jsfn_ConvertToDatatable($('#tbSettlementStatus'));
        $("#tbSettlementStatus thead th input[type=text]").each(function () {
            vSettlementStatusDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

