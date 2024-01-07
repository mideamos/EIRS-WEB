var vPaymentOptionDataTable;

jQuery(document).ready(function () {
    vPaymentOptionDataTable = jsfn_ConvertToDatatable($('#tbPaymentOption'));
});

function jsfn_ChangeStatus(PaymentOptionid) {
    var vData = {
        PaymentOptionID: PaymentOptionid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/PaymentOption/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vPaymentOptionDataTable.destroy();
        $('#tbody').html(data.PaymentOptionList);
        vPaymentOptionDataTable = jsfn_ConvertToDatatable($('#tbPaymentOption'));
        $("#tbPaymentOption thead th input[type=text]").each(function () {
            vPaymentOptionDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

