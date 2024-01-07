var vDirectorateDataTable;

jQuery(document).ready(function () {
    vDirectorateDataTable = jsfn_ConvertToDatatable($('#tbDirectorate'));
});

function jsfn_ChangeStatus(Directorateid) {
    var vData = {
        DirectorateID: Directorateid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Directorate/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vDirectorateDataTable.destroy();
        $('#tbody').html(data.DirectorateList);
        vDirectorateDataTable = jsfn_ConvertToDatatable($('#tbDirectorate'));
        $("#tbDirectorate thead th input[type=text]").each(function () {
            vDirectorateDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

