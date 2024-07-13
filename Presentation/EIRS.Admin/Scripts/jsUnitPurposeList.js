var vUnitPurposeDataTable;

jQuery(document).ready(function () {
    vUnitPurposeDataTable = jsfn_ConvertToDatatable($('#tbUnitPurpose'));
});

function jsfn_ChangeStatus(UnitPurposeid) {
    var vData = {
        UnitPurposeID: UnitPurposeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/UnitPurpose/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vUnitPurposeDataTable.destroy();
        $('#tbody').html(data.UnitPurposeList);
        vUnitPurposeDataTable = jsfn_ConvertToDatatable($('#tbUnitPurpose'));
        $("#tbUnitPurpose thead th input[type=text]").each(function () {
            vUnitPurposeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

