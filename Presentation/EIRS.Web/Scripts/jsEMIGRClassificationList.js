var vIGRClassificationTable;

var vColumnsList = [{
    "orderable": true
}, {
    "orderable": true
}, {
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
    vIGRClassificationTable = jsfn_ConvertToDatableWithCustomSort($('#tblIGRClassification'), vColumnsList, 'No IGR Classification Found', vSortOrder);
});

function jsfn_ChangeStatus(igrid) {
    var vData = {
        IGRClassificationID: igrid,
        intStatus: 2
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/EMIGRClassification/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vIGRClassificationTable.fnDestroy();
        $('#tbody').html(data.IGRClassificationList);
        vIGRClassificationTable = jsfn_ConvertToDatableWithCustomSort($('#tblIGRClassification'), vColumnsList, 'No IGR Classification Found', vSortOrder);
        $("#tbIGRClassification thead th input[type=text]").each(function () {
            vIGRClassificationTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

