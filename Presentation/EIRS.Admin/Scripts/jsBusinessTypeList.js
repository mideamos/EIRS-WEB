var vBusinessTypeDataTable;

jQuery(document).ready(function () {
    vBusinessTypeDataTable = jsfn_ConvertToDatatable($('#tbBusinessType'));
});

function jsfn_ChangeStatus(BusinessTypeid) {
    var vData = {
        BusinessTypeID: BusinessTypeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/BusinessType/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBusinessTypeDataTable.destroy();
        $('#tbody').html(data.BusinessTypeList);
        vBusinessTypeDataTable = jsfn_ConvertToDatatable($('#tbBusinessType'));
        $("#tbBusinessType thead th input[type=text]").each(function () {
            vBusinessTypeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

