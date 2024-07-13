var vRevenueSubStreamDataTable;

jQuery(document).ready(function () {
    vRevenueSubStreamDataTable = jsfn_ConvertToDatatable($('#tbRevenueSubStream'));
});

function jsfn_ChangeStatus(RevenueSubStreamid) {
    var vData = {
        RevenueSubStreamID: RevenueSubStreamid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/RevenueSubStream/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vRevenueSubStreamDataTable.destroy();
        $('#tbody').html(data.RevenueSubStreamList);
        vRevenueSubStreamDataTable = jsfn_ConvertToDatatable($('#tbRevenueSubStream'));
        $("#tbRevenueSubStream thead th input[type=text]").each(function () {
            vRevenueSubStreamDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

