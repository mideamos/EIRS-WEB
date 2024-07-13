var vEconomicActivitiesDataTable;

jQuery(document).ready(function () {
    vEconomicActivitiesDataTable = jsfn_ConvertToDatatable($('#tbEconomicActivities'));
});

function jsfn_ChangeStatus(EconomicActivitiesid) {
    var vData = {
        EconomicActivitiesID: EconomicActivitiesid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/EconomicActivities/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vEconomicActivitiesDataTable.destroy();
        $('#tbody').html(data.EconomicActivitiesList);
        vEconomicActivitiesDataTable = jsfn_ConvertToDatatable($('#tbEconomicActivities'));
        $("#tbEconomicActivities thead th input[type=text]").each(function () {
            vEconomicActivitiesDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

