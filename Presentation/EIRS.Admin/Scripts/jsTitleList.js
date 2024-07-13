var vTitleDataTable;

jQuery(document).ready(function () {
    vTitleDataTable = jsfn_ConvertToDatatable($('#tbTitle'));
});

function jsfn_ChangeStatus(Titleid) {
    var vData = {
        TitleID: Titleid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Title/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vTitleDataTable.destroy();
        $('#tbody').html(data.TitleList);
        vTitleDataTable = jsfn_ConvertToDatatable($('#tbTitle'));
        $("#tbTitle thead th input[type=text]").each(function () {
            vTitleDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

