var vPaymentFrequencyDataTable;

jQuery(document).ready(function () {
    vPaymentFrequencyDataTable = jsfn_ConvertToDatatable($('#tbPaymentFrequency'));
});

function jsfn_ChangeStatus(PaymentFrequencyid) {
    var vData = {
        PaymentFrequencyID: PaymentFrequencyid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/PaymentFrequency/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vPaymentFrequencyDataTable.destroy();
        $('#tbody').html(data.PaymentFrequencyList);
        vPaymentFrequencyDataTable = jsfn_ConvertToDatatable($('#tbPaymentFrequency'));
        $("#tbPaymentFrequency thead th input[type=text]").each(function () {
            vPaymentFrequencyDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

