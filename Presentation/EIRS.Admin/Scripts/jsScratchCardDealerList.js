var vScratchCardDealerDataTable;

jQuery(document).ready(function () {
    vScratchCardDealerDataTable = jsfn_ConvertToDatatable($('#tbScratchCardDealer'));
});

function jsfn_ChangeStatus(ScratchCardDealerid) {
    var vData = {
        ScratchCardDealerID: ScratchCardDealerid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/ScratchCardDealer/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vScratchCardDealerDataTable.destroy();
        $('#tbody').html(data.ScratchCardDealerList);
        vScratchCardDealerDataTable = jsfn_ConvertToDatatable($('#tbScratchCardDealer'));
        $("#tbScratchCardDealer thead th input[type=text]").each(function () {
            vScratchCardDealerDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

