var vLGADataTable;

jQuery(document).ready(function () {
    vLGADataTable = jsfn_ConvertToDatatable($('#tbLGA'));
});

function jsfn_ChangeStatus(lgaid) {
    var vData = {
        LGAID: lgaid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/LGA/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vLGADataTable.destroy();
        $('#tbody').html(data.LGAList);
        vLGADataTable = jsfn_ConvertToDatatable($('#tbLGA'));
        $("#tbLGA thead th input[type=text]").each(function () {
            vLGADataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

