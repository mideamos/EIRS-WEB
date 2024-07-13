var vBusinessSubSectorDataTable;

jQuery(document).ready(function () {
    vBusinessSubSectorDataTable = jsfn_ConvertToDatatable($('#tbBusinessSubSector'));
});

function jsfn_ChangeStatus(BusinessSubSectorid) {
    var vData = {
        BusinessSubSectorID: BusinessSubSectorid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/BusinessSubSector/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBusinessSubSectorDataTable.destroy();
        $('#tbody').html(data.BusinessSubSectorList);
        vBusinessSubSectorDataTable = jsfn_ConvertToDatatable($('#tbBusinessSubSector'));
        $("#tbBusinessSubSector thead th input[type=text]").each(function () {
            vBusinessSubSectorDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

