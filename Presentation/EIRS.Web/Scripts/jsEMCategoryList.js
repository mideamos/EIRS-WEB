var vCategoryDataTable;

var vColumnsList = [{
    "orderable": true
}, {
    "orderable": true
}, {
    "orderable": false
    }];

var vSortOrder = [];

jQuery(document).ready(function () {
    vCategoryDataTable = jsfn_ConvertToDatableWithCustomSort($('#tblCategory'), vColumnsList, 'No Classification Categories Found', vSortOrder);
});

function jsfn_ChangeStatus(Categoryid) {
    var vData = {
        CategoryID: Categoryid,
        intStatus: 2
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/EMCategory/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vCategoryDataTable.fnDestroy();
        $('#tbody').html(data.CategoryList);
        vCategoryDataTable = jsfn_ConvertToDatableWithCustomSort($('#tblCategory'), vColumnsList, 'No Classification Categories Found', vSortOrder);
        $("#tbCategory thead th input[type=text]").each(function () {
            vCategoryDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

