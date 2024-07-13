var vAddressTypeDataTable;

jQuery(document).ready(function () {
    vAddressTypeDataTable = jsfn_ConvertToDatatable($('#tbAddressType'));
});

function jsfn_ChangeStatus(addresstypeid) {
    var vData = {
        AddressTypeID: addresstypeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/AddressType/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vAddressTypeDataTable.destroy();
        $('#tbody').html(data.AddressTypeList);
        vAddressTypeDataTable = jsfn_ConvertToDatatable($('#tbAddressType'));
        $("#tbAddressType thead th input[type=text]").each(function () {
            vAddressTypeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

