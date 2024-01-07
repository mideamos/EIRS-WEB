var vRevenueHeadDataTable;

var vColumnsList = [{
    "orderable": true
}, {
    "orderable": true
}, {
    "orderable": true
}, {
    "orderable": false
}];

var vSortOrder = [];

jQuery(document).ready(function () {
    vRevenueHeadDataTable = jsfn_ConvertToDatableWithCustomSort($('#tblRevenueHead'), vColumnsList, 'No Classification Revenue Heads Found', vSortOrder);
});

function jsfn_ChangeStatus(rhid) {
    var vData = {
        RevenueHeadID: rhid,
        intStatus: 2
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/EMRevenueHead/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vRevenueHeadDataTable.fnDestroy();
        $('#tbody').html(data.RevenueHeadList);
        vRevenueHeadDataTable = jsfn_ConvertToDatableWithCustomSort($('#tblRevenueHead'), vColumnsList, 'No Classification Revenue Heads Found', vSortOrder);
        $("#tbRevenueHead thead th input[type=text]").each(function () {
            vRevenueHeadDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

