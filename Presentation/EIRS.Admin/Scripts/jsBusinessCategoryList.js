var vBusinessCategoryDataTable;

jQuery(document).ready(function () {
    vBusinessCategoryDataTable = jsfn_ConvertToDatatable($('#tbBusinessCategory'));
});

function jsfn_ChangeStatus(BusinessCategoryid) {
    var vData = {
        BusinessCategoryID: BusinessCategoryid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/BusinessCategory/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBusinessCategoryDataTable.destroy();
        $('#tbody').html(data.BusinessCategoryList);
        vBusinessCategoryDataTable = jsfn_ConvertToDatatable($('#tbBusinessCategory'));
        $("#tbBusinessCategory thead th input[type=text]").each(function () {
            vBusinessCategoryDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

