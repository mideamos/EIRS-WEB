var vAssetTypeDataTable;

jQuery(document).ready(function () {
    vAssetTypeDataTable = jsfn_ConvertToDatatable($('#tbAssetType'));
});

function jsfn_ChangeStatus(assettypeid) {
    var vData = {
        AssetTypeID: assettypeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/AssetType/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vAssetTypeDataTable.destroy();
        $('#tbody').html(data.AssetTypeList);
        vAssetTypeDataTable = jsfn_ConvertToDatatable($('#tbAssetType'));
        $("#tbAssetType thead th input[type=text]").each(function () {
            vAssetTypeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

