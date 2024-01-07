$(document).ready(function () {

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
    }];

    var vSortOrder = [];

    jsfn_ConvertToDatableWithCustomSort($('#tblBankPayment'), vColumnsList, 'No Banks Found', vSortOrder);

    $("#cboSettlementMethod").on('change', function () {
        var vSettlementMethod = $("#cboSettlementMethod").val();

        if (parseInt(vSettlementMethod) === 1) {
            $("#btnMakePayment").text('View Collecting Banks');
            $("#btnMakePayment").click(function () {
                $("#dvBankDetails").modal('show');
                return false;
            });
        }
        else if (parseInt(vSettlementMethod) === 2) {
            $("#btnMakePayment").text('Make Payment');
            $("#frmSettlement").submit();
        }
        else if (parseInt(vSettlementMethod) === 3) {
            $("#btnMakePayment").text('View Instructions');
        }
        else if (parseInt(vSettlementMethod) === 4) {
            $("#btnMakePayment").text('Make Payment');
        }
        else if (parseInt(vSettlementMethod) === 5) {
            $("#btnMakePayment").text('Make Payment');
        }
    });
});