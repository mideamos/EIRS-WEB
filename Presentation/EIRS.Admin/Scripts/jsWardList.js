var vWardDataTable;

jQuery(document).ready(function () {
    vWardDataTable = jsfn_ConvertToDatatable($('#tbWard'));
});

function jsfn_ChangeStatus(wardid) {
    var vData = {
        WardID: wardid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Ward/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vWardDataTable.destroy();
        $('#tbody').html(data.WardList);
        vWardDataTable = jsfn_ConvertToDatatable($('#tbWard'));
        $("#tbWard thead th input[type=text]").each(function () {
            vWardDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

