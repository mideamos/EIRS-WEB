var vBusinessSectorDataTable;

jQuery(document).ready(function () {
    vBusinessSectorDataTable = jsfn_ConvertToDatatable($('#tbBusinessSector'));
});

function jsfn_ChangeStatus(BusinessSectorid) {
    var vData = {
        BusinessSectorID: BusinessSectorid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/BusinessSector/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBusinessSectorDataTable.destroy();
        $('#tbody').html(data.BusinessSectorList);
        vBusinessSectorDataTable = jsfn_ConvertToDatatable($('#tbBusinessSector'));
        $("#tbBusinessSector thead th input[type=text]").each(function () {
            vBusinessSectorDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

