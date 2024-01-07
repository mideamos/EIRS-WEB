var vBusinessOperationDataTable;

jQuery(document).ready(function () {
    vBusinessOperationDataTable = jsfn_ConvertToDatatable($('#tbBusinessOperation'));
});

function jsfn_ChangeStatus(BusinessOperationid) {
    var vData = {
        BusinessOperationID: BusinessOperationid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/BusinessOperation/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBusinessOperationDataTable.destroy();
        $('#tbody').html(data.BusinessOperationList);
        vBusinessOperationDataTable = jsfn_ConvertToDatatable($('#tbBusinessOperation'));
        $("#tbBusinessOperation thead th input[type=text]").each(function () {
            vBusinessOperationDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

