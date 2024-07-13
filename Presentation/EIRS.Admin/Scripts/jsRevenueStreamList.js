var vRevenueStreamDataTable;

jQuery(document).ready(function () {
    vRevenueStreamDataTable = jsfn_ConvertToDatatable($('#tbRevenueStream'));
});

function jsfn_ChangeStatus(RevenueStreamid) {
    var vData = {
        RevenueStreamID: RevenueStreamid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/RevenueStream/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vRevenueStreamDataTable.destroy();
        $('#tbody').html(data.RevenueStreamList);
        vRevenueStreamDataTable = jsfn_ConvertToDatatable($('#tbRevenueStream'));
        $("#tbRevenueStream thead th input[type=text]").each(function () {
            vRevenueStreamDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

