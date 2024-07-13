var vAssessmentItemCategoryDataTable;

jQuery(document).ready(function () {
    vAssessmentItemCategoryDataTable = jsfn_ConvertToDatatable($('#tbAssessmentItemCategory'));
});

function jsfn_ChangeStatus(AssessmentItemCategoryid) {
    var vData = {
        AssessmentItemCategoryID: AssessmentItemCategoryid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/AssessmentItemCategory/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vAssessmentItemCategoryDataTable.destroy();
        $('#tbody').html(data.AssessmentItemCategoryList);
        vAssessmentItemCategoryDataTable = jsfn_ConvertToDatatable($('#tbAssessmentItemCategory'));
        $("#tbAssessmentItemCategory thead th input[type=text]").each(function () {
            vAssessmentItemCategoryDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

