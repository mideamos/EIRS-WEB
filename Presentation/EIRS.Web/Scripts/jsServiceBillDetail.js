$(document).ready(function () {
    $("#btnSaveItems").click(function () { jsfn_UpdateToSettleAmount(); });
});

$(document).on('submit', 'form', function () {
    var button = $("#btnFinalSave");
    setTimeout(function () {
        button.attr('disabled', 'disabled');
    }, 0);
});

function jsfn_UpdateToSettleAmount() {
    var vRowList = '';
    $("#dvEditMDAServiceItemModalBody input.tosettleamount").each(function () {
        if (parseInt($(this).data('tbachanged')) === 1) {
            var vRowID = $(this).data('rowid');
            var vToSettleAmount = $(this).val();

            if (vRowList == '') {
                vRowList = vRowID + '@@' + vToSettleAmount;
            }
            else {
                vRowList = vRowList + '~~' + vRowID + '@@' + vToSettleAmount;
            }
        }
    });

    if (vRowList != '') {

        var vData = {
            rowdata: vRowList,
        };

        jsfn_ShowLoading();
        jsfn_ajaxPost('/Base/UpdateServiceBillSettleAmount', vData, jsfn_UpdateToSettleAmountResponse);
    }
    else {
        $('#dvEditMDAServiceItemModal').modal('hide');
    }
}

function jsfn_UpdateToSettleAmountResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvEditMDAServiceItemModal').modal('hide');
        $("#dvMDAService").html(data.MDAServiceDetails);
        $("#dvAmountToPay").html(data.NewHolder.formatMoney(2, '₦'));
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowMDAServiceItems(rowId) {
    var vData = {
        MDAServiceRowID: rowId,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetMDAServiceItemView', vData, jsfn_ShowMDAServiceItemResponse);
}

function jsfn_ShowMDAServiceItemResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvViewMDAServiceItemModal').modal('show');
        $('#spViewMDAServiceName').html(data.MDAServiceName);
        $("#dvViewMDAServiceItemModalBody").html(data.MDAServiceItemList);

    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_EditMDAServiceItems(rowId) {
    var vData = {
        MDAServiceRowID: rowId,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetMDAServiceItemEditForSettlement', vData, jsfn_EditMDAServiceItemResponse);
}

function jsfn_EditMDAServiceItemResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvEditMDAServiceItemModal').modal('show');
        $('#spEditMDAServiceName').html(data.MDAServiceName);
        $("#dvEditMDAServiceItemModalBody").html(data.MDAServiceItemList);
        jsfn_BindPendingAmount();
        $("#dvModalFooter").show();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowMDAServicePaymentDetail(rowId) {
    var vData = {
        MDAServiceRowID: rowId,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetMDAServicePaymentView', vData, jsfn_ShowAsessmentRulePaymentDetailResponse);
}

function jsfn_ShowAsessmentRulePaymentDetailResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvViewMDAServicePaymentModal').modal('show');
        $('#spViewMDAServicePaymentName').html(data.MDAServiceName);
        $("#dvViewMDAServicePaymentModalBody").html(data.MDAServicePaymentList);

    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}



function jsfn_ShowPaymentDetail(sbid) {
    var vData = {
        AssessmentID: -1,
        ServiceID: sbid
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetSettlementList', vData, jsfn_ShowPaymentDetailResponse);
}

function jsfn_ShowPaymentDetailResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvViewSettlementModal').modal('show');
        $("#dvSettlementModalBody").html(data.SettlementList);

    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BindPendingAmount() {
    $(".tosettleamount").blur(function () {
        var vRowID = $(this).data('rowid');
        var vPendingAmount = $(this).data('pendingamount');
        var vToSettleAmount = $(this).val();

        if (parseFloat(vToSettleAmount) > parseFloat(vPendingAmount)) {
            $("#spn_" + vRowID).html('Please enter amount less than or equal to settlement amount');
        }
        else {
            $("#spn_" + vRowID).html('');
            $(this).attr('data-tbachanged', '1');
        }
    });
}





////$(document).ready(function () {
////    $("#btnSaveItems").click(function () { jsfn_UpdateToSettleAmount(); });
////});

////$(document).on('submit', 'form', function () {
////    var button = $("#btnFinalSave");
////    setTimeout(function () {
////        button.attr('disabled', 'disabled');
////    }, 0);
////});

////function jsfn_UpdateToSettleAmount() {
////    var url_string = window.location.href;
////    var url = new URL(url_string);
////    var c = url.searchParams.get("id");
////    console.log(c);
////    var vRowList = '';
////    $("#dvEditMDAServiceItemModalBody input.tosettleamount").each(function () {
////        if (parseInt($(this).data('tbachanged')) === 1) {
////            var vRowID = $(this).data('rowid');
////            var vToSettleAmount = $(this).val();

////            if (vRowList == '') {
////                vRowList = vRowID + '@@' + vToSettleAmount;
////            }
////            else {
////                vRowList = vRowList + '~~' + vRowID + '@@' + vToSettleAmount;
////            }
////        }
////    });

////    if (vRowList != '') {

////        var vData = {
////            rowdata: vRowList,
////        };

////        jsfn_ShowLoading();
////        jsfn_ajaxPost('/OperationManager/UpdateServiceBillNewItem', vData);

////        var url = "/OperationManager/UpdateServiceBillNewItem?rowdata=" + c;
////        window.location.href = url;
////    }
////    else {
////        $('#dvEditMDAServiceItemModal').modal('hide');
////    }
////}

////function jsfn_UpdateToSettleAmountResponse(data) {
////    jsfn_HideLoading();
////    if (data.success) {
////        $('#dvEditMDAServiceItemModal').modal('hide');
////        $("#dvMDAService").html(data.MDAServiceDetails);
////        $("#dvAmountToPay").html(data.AmountToPay.formatMoney(2, '₦'));
////    }
////    else {
////        jsfn_ShowAlert(data.Message, 'danger');
////    }
////}

////function jsfn_ShowMDAServiceItems(rowId) {
////    var vData = {
////        MDAServiceRowID: rowId,
////    };

////    jsfn_ShowLoading();
////    jsfn_ajaxPost('/Base/GetMDAServiceItemView', vData, jsfn_ShowMDAServiceItemResponse);
////}

////function jsfn_ShowMDAServiceItemResponse(data) {
////    jsfn_HideLoading();
////    if (data.success) {
////        $('#dvViewMDAServiceItemModal').modal('show');
////        $('#spViewMDAServiceName').html(data.MDAServiceName);
////        $("#dvViewMDAServiceItemModalBody").html(data.MDAServiceItemList);

////    }
////    else {
////        jsfn_ShowAlert(data.Message, 'danger');
////    }
////}

////function jsfn_EditMDAServiceItems(rowId) {
////    var vData = {
////        MDAServiceRowID: rowId,
////    };

////    jsfn_ShowLoading();
////    jsfn_ajaxPost('/Base/GetMDAServiceItemEditForSettlement', vData, jsfn_EditMDAServiceItemResponse);
////}

////function jsfn_EditMDAServiceItemResponse(data) {
////    jsfn_HideLoading();
////    if (data.success) {
////        $('#dvEditMDAServiceItemModal').modal('show');
////        $('#spEditMDAServiceName').html(data.MDAServiceName);
////        $("#dvEditMDAServiceItemModalBody").html(data.MDAServiceItemList);
////        jsfn_BindPendingAmount();
////        $("#dvModalFooter").show();
////    }
////    else {
////        jsfn_ShowAlert(data.Message, 'danger');
////    }
////}

////function jsfn_ShowMDAServicePaymentDetail(rowId) {
////    var vData = {
////        MDAServiceRowID: rowId,
////    };

////    jsfn_ShowLoading();
////    jsfn_ajaxPost('/Base/GetMDAServicePaymentView', vData, jsfn_ShowAsessmentRulePaymentDetailResponse);
////}

////function jsfn_ShowAsessmentRulePaymentDetailResponse(data) {
////    jsfn_HideLoading();
////    if (data.success) {
////        $('#dvViewMDAServicePaymentModal').modal('show');
////        $('#spViewMDAServicePaymentName').html(data.MDAServiceName);
////        $("#dvViewMDAServicePaymentModalBody").html(data.MDAServicePaymentList);

////    }
////    else {
////        jsfn_ShowAlert(data.Message, 'danger');
////    }
////}



////function jsfn_ShowPaymentDetail(sbid) {
////    var vData = {
////        AssessmentID: -1,
////        ServiceID: sbid
////    };

////    jsfn_ShowLoading();
////    jsfn_ajaxPost('/Base/GetSettlementList', vData, jsfn_ShowPaymentDetailResponse);
////}

////function jsfn_ShowPaymentDetailResponse(data) {
////    jsfn_HideLoading();
////    if (data.success) {
////        $('#dvViewSettlementModal').modal('show');
////        $("#dvSettlementModalBody").html(data.SettlementList);

////    }
////    else {
////        jsfn_ShowAlert(data.Message, 'danger');
////    }
////}

////function jsfn_BindPendingAmount() {
////    $(".tosettleamount").blur(function () {
////        var vRowID = $(this).data('rowid');
////        var vPendingAmount = $(this).data('pendingamount');
////        var vToSettleAmount = $(this).val();

////        if (parseFloat(vToSettleAmount) > parseFloat(vPendingAmount)) {
////            $("#spn_" + vRowID).html('Please enter amount less than or equal to settlement amount');
////        }
////        else {
////            $("#spn_" + vRowID).html('');
////            $(this).attr('data-tbachanged', '1');
////        }
////    });
////}

