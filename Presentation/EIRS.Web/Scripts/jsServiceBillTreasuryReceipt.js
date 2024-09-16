$(document).ready(function () {
    jsfn_ConvertToDatableWithCustomSort($('#tblReceiptList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Receipt Found');
    $("#dvShowGenerateReceiptAlert").hide();
    $("#btnGenerateReceipt").click(function () {
        if ($("#hdnSettlementIds").val() !== '') {
            var vReceiptAmount = $("#hdnReceiptAmount").val();
            var vSettlementIds = $("#hdnSettlementIds").val();

            var vData = {
                ReceiptAmount: vReceiptAmount,
                SettlementIds: vSettlementIds,
                ServiceBillID: $("#hdnServiceBillID").val()
            };

            $("#btnGenerateReceipt").attr('disabled', 'disabled');
            jsfn_ajaxPost('/TreasuryReceipt/AddReceipt', vData, jsfn_GenerateReceiptResponse);

        }
        else {
            $("#dvShowGenerateReceiptAlert").html('Please select settlement for generating receipt');
            $("#dvShowGenerateReceiptAlert").show();
        }
    });

    $(".chksettlement").on("ifChecked", function () {
        jsfn_onSettlementSelected($(this).data("stmtid").toString(), parseFloat($(this).data("stmtamt")), true);
    });

    $(".chksettlement").on("ifUnchecked", function () {
        jsfn_onSettlementSelected($(this).data("stmtid").toString(), parseFloat($(this).data("stmtamt")), false);
    });

    $("#chkAllSettlement").on("ifChecked", function () {
        $("input[name=chkSettlement]").iCheck('check');
    });

    $("#chkAllSettlement").on("ifUnchecked", function () {
        $("input[name=chkSettlement]").iCheck('uncheck');
    });
});

function jsfn_ShowReceiptModal() {
    $("#dvGenerateReceiptModal").modal('show');
}

function jsfn_GenerateReceiptResponse(data) {
    if (data.success) {
        $("#btnGenerateReceipt").removeAttr('disabled', 'disabled');
        $("#dvGenerateReceiptModal").modal('hide');
        location.reload(true);
    }
    else {
        $("#dvShowGenerateReceiptAlert").html(data.Message);
        $("#dvShowGenerateReceiptAlert").show();
    }
}

function jsfn_onSettlementSelected(stmtid, stmtamount, isChecked) {
    var vArrSettlement = $('#hdnSettlementIds').val().split(',');
    if (isChecked) {
        if ($('#hdnSettlementIds').val() !== "")
            vArrSettlement.push(stmtid);
        else {
            vArrSettlement = new Array();
            vArrSettlement.push(stmtid);
        }
    }
    else {
        vArrSettlement.splice($.inArray(stmtid, vArrSettlement), 1);
    }

    $('#hdnSettlementIds').val(vArrSettlement.toString());

    //Update Receipt Amount
    var vTotalReceiptAmount = parseFloat($('#hdnReceiptAmount').val());

    if (isChecked) {
        vTotalReceiptAmount = vTotalReceiptAmount + stmtamount;
    }
    else {
        vTotalReceiptAmount = vTotalReceiptAmount - stmtamount;
    }

    $('#hdnReceiptAmount').val(vTotalReceiptAmount);
    $("#lblTotalAmount").html(vTotalReceiptAmount.formatMoney(2));
}