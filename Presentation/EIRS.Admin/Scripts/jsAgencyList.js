var vAgencyDataTable;

jQuery(document).ready(function () {
    vAgencyDataTable = jsfn_ConvertToDatatable($('#tbAgency'));
});

function jsfn_ChangeStatus(Agencyid) {
    var vData = {
        AgencyID: Agencyid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Agency/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vAgencyDataTable.destroy();
        $('#tbody').html(data.AgencyList);
        vAgencyDataTable = jsfn_ConvertToDatatable($('#tbAgency'));
        $("#tbAgency thead th input[type=text]").each(function () {
            vAgencyDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

