var vGovernmentTypeDataTable;

jQuery(document).ready(function () {
    vGovernmentTypeDataTable = jsfn_ConvertToDatatable($('#tbGovernmentType'));
});

function jsfn_ChangeStatus(Governmenttypeid) {
    var vData = {
        GovernmentTypeID: Governmenttypeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/GovernmentType/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vGovernmentTypeDataTable.destroy();
        $('#tbody').html(data.GovernmentTypeList);
        vGovernmentTypeDataTable = jsfn_ConvertToDatatable($('#tbGovernmentType'));
        $("#tbGovernmentType thead th input[type=text]").each(function () {
            vGovernmentTypeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

