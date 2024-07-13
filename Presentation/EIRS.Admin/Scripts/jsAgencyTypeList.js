var vAgencyTypeDataTable;

jQuery(document).ready(function () {
    vAgencyTypeDataTable = jsfn_ConvertToDatatable($('#tbAgencyType'));
});

function jsfn_ChangeStatus(Agencytypeid) {
    var vData = {
        AgencyTypeID: Agencytypeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/AgencyType/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vAgencyTypeDataTable.destroy();
        $('#tbody').html(data.AgencyTypeList);
        vAgencyTypeDataTable = jsfn_ConvertToDatatable($('#tbAgencyType'));
        $("#tbAgencyType thead th input[type=text]").each(function () {
            vAgencyTypeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

