var vAssessmentSubGroupDataTable;

jQuery(document).ready(function () {
    vAssessmentSubGroupDataTable = jsfn_ConvertToDatatable($('#tbAssessmentSubGroup'));
});

function jsfn_ChangeStatus(AssessmentSubGroupid) {
    var vData = {
        AssessmentSubGroupID: AssessmentSubGroupid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/AssessmentSubGroup/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vAssessmentSubGroupDataTable.destroy();
        $('#tbody').html(data.AssessmentSubGroupList);
        vAssessmentSubGroupDataTable = jsfn_ConvertToDatatable($('#tbAssessmentSubGroup'));
        $("#tbAssessmentSubGroup thead th input[type=text]").each(function () {
            vAssessmentSubGroupDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

