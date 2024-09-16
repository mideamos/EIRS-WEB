var vBankDataTable;

var vColumnsList = [{
    "orderable": true
}, {
    "orderable": true
},{
    "orderable": true
}, {
    "orderable": false
}];

var vSortOrder = [];

jQuery(document).ready(function () {
    vBankDataTable = jsfn_ConvertToDatableWithCustomSort($('#tblBank'), vColumnsList, 'No Banks Found', vSortOrder);
});

function jsfn_ChangeStatus(Bankid) {
    var vData = {
        BankID: Bankid,
        intStatus: 2
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/EMBank/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBankDataTable.fnDestroy();
        $('#tbody').html(data.BankList);
        vBankDataTable = jsfn_ConvertToDatableWithCustomSort($('#tblBank'), vColumnsList, 'No Banks Found', vSortOrder);
        $("#tbBank thead th input[type=text]").each(function () {
            vBankDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

