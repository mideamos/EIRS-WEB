var vExceptionTypeDataTable;

jQuery(document).ready(function () {
    vExceptionTypeDataTable = jsfn_ConvertToDatatable($('#tbExceptionType'));
});

function jsfn_ChangeStatus(ExceptionTypeid) {
    var vData = {
        ExceptionTypeID: ExceptionTypeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/ExceptionType/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vExceptionTypeDataTable.destroy();
        $('#tbody').html(data.ExceptionTypeList);
        vExceptionTypeDataTable = jsfn_ConvertToDatatable($('#tbExceptionType'));
        $("#tbExceptionType thead th input[type=text]").each(function () {
            vExceptionTypeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

