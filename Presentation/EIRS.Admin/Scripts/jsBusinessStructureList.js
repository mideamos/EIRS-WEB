var vBusinessStructureDataTable;

jQuery(document).ready(function () {
    vBusinessStructureDataTable = jsfn_ConvertToDatatable($('#tbBusinessStructure'));
});

function jsfn_ChangeStatus(BusinessStructureid) {
    var vData = {
        BusinessStructureID: BusinessStructureid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/BusinessStructure/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBusinessStructureDataTable.destroy();
        $('#tbody').html(data.BusinessStructureList);
        vBusinessStructureDataTable = jsfn_ConvertToDatatable($('#tbBusinessStructure'));
        $("#tbBusinessStructure thead th input[type=text]").each(function () {
            vBusinessStructureDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

