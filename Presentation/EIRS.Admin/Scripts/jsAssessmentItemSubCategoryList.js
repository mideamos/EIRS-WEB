var vAssessmentItemSubCategoryDataTable;

jQuery(document).ready(function () {
    vAssessmentItemSubCategoryDataTable = jsfn_ConvertToDatatable($('#tbAssessmentItemSubCategory'));
});

function jsfn_ChangeStatus(AssessmentItemSubCategoryid) {
    var vData = {
        AssessmentItemSubCategoryID: AssessmentItemSubCategoryid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/AssessmentItemSubCategory/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vAssessmentItemSubCategoryDataTable.destroy();
        $('#tbody').html(data.AssessmentItemSubCategoryList);
        vAssessmentItemSubCategoryDataTable = jsfn_ConvertToDatatable($('#tbAssessmentItemSubCategory'));
        $("#tbAssessmentItemSubCategory thead th input[type=text]").each(function () {
            vAssessmentItemSubCategoryDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

