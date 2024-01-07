var vSizeDataTable;

jQuery(document).ready(function () {
    vSizeDataTable = jsfn_ConvertToDatatable($('#tbSize'));
});

function jsfn_ChangeStatus(assettypeid) {
    var vData = {
        SizeID: assettypeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Size/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vSizeDataTable.destroy();
        $('#tbody').html(data.SizeList);
        vSizeDataTable = jsfn_ConvertToDatatable($('#tbSize'));
        $("#tbSize thead th input[type=text]").each(function () {
            vSizeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

