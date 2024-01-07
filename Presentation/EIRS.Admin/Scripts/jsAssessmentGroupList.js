var vAssessmentGroupDataTable;

jQuery(document).ready(function () {
    vAssessmentGroupDataTable = jsfn_ConvertToDatatable($('#tbAssessmentGroup'));
});

function jsfn_ChangeStatus(AssessmentGroupid) {
    var vData = {
        AssessmentGroupID: AssessmentGroupid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/AssessmentGroup/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vAssessmentGroupDataTable.destroy();
        $('#tbody').html(data.AssessmentGroupList);
        vAssessmentGroupDataTable = jsfn_ConvertToDatatable($('#tbAssessmentGroup'));
        $("#tbAssessmentGroup thead th input[type=text]").each(function () {
            vAssessmentGroupDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

