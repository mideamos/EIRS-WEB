var vLandStreetConditionDataTable;

jQuery(document).ready(function () {
    vLandStreetConditionDataTable = jsfn_ConvertToDatatable($('#tbLandStreetCondition'));
});

function jsfn_ChangeStatus(LandStreetConditionid) {
    var vData = {
        LandStreetConditionID: LandStreetConditionid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/LandStreetCondition/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vLandStreetConditionDataTable.destroy();
        $('#tbody').html(data.LandStreetConditionList);
        vLandStreetConditionDataTable = jsfn_ConvertToDatatable($('#tbLandStreetCondition'));
        $("#tbLandStreetCondition thead th input[type=text]").each(function () {
            vLandStreetConditionDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

