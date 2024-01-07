var vScratchCardPrinterDataTable;

jQuery(document).ready(function () {
    vScratchCardPrinterDataTable = jsfn_ConvertToDatatable($('#tbScratchCardPrinter'));
});

function jsfn_ChangeStatus(ScratchCardPrinterid) {
    var vData = {
        ScratchCardPrinterID: ScratchCardPrinterid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/ScratchCardPrinter/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vScratchCardPrinterDataTable.destroy();
        $('#tbody').html(data.ScratchCardPrinterList);
        vScratchCardPrinterDataTable = jsfn_ConvertToDatatable($('#tbScratchCardPrinter'));
        $("#tbScratchCardPrinter thead th input[type=text]").each(function () {
            vScratchCardPrinterDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

