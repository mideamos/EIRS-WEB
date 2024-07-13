var vTownDataTable;

jQuery(document).ready(function () {
    vTownDataTable = jsfn_ConvertToDatatable($('#tbTown'));
});

function jsfn_ChangeStatus(twnid) {
    var vData = {
        TownID: twnid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Town/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vTownDataTable.destroy();
        $('#tbody').html(data.TownList);
        vTownDataTable = jsfn_ConvertToDatatable($('#tbTown'));
        $("#tbTown thead th input[type=text]").each(function () {
            vTownDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

