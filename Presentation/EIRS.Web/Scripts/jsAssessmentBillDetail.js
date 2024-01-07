
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
    $("#dvEditAssessmentRuleItemModalBody input.tosettleamount").each(function () {
        if (parseInt($(this).data('tbachanged')) === 1) {
            console.log($(this).data);
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
        jsfn_ajaxPost('/Base/UpdateAssessmentSettleAmount2', vData, jsfn_UpdateToSettleAmountResponse);
    }
    else {
        $('#dvEditAssessmentRuleItemModal').modal('hide');
    }

}

function jsfn_UpdateToSettleAmountResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        console.log(data);
        $('#dvEditAssessmentRuleItemModal').modal('hide');
        $("#dvAssessmentRule").html(data.AssessmentRuleDetails);
        $("#dvAmountToPay").html("₦" + data.NewHolder);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowAssessmentItems(arowId) {
    var vData = {
        AssessmentRuleRowID: arowId,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetAssessmentRuleItemView', vData, jsfn_ShowAssessmentItemResponse);
}

function jsfn_ShowAssessmentItemResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvViewAssessmentRuleItemModal').modal('show');
        $('#spViewAssessmentRuleName').html(data.AssessmentRuleName);
        $("#dvViewAssessmentRuleItemModalBody").html(data.AssessmentRuleItemList);

    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_EditAssessmentItems(arowId) {
    var vData = {
        AssessmentRuleRowID: arowId,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetAssessmentRuleItemForSettlement2', vData, jsfn_EditAssessmentItemResponse);
}

function jsfn_EditAssessmentItemResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        console.log(data);
        $('#dvEditAssessmentRuleItemModal').modal('show');
        $('#spEditAssessmentRuleName').html(data.AssessmentRuleName);
        $("#dvEditAssessmentRuleItemModalBody").html(data.AssessmentRuleItemList);
        jsfn_BindPendingAmount();
        $("#dvModalFooter").show();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowAsessmentRulePaymentDetail(arowId) {
    var vData = {
        AssessmentRuleRowID: arowId,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetAssessmentRulePaymentView', vData, jsfn_ShowAsessmentRulePaymentDetailResponse);
}

function jsfn_ShowAsessmentRulePaymentDetailResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvViewAssessmentRulePaymentModal').modal('show');
        $('#spViewAssessmentRulePaymentName').html(data.AssessmentRuleName);
        $("#dvViewAssessmentRulePaymentModalBody").html(data.AssessmentRulePaymentList);

    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowPaymentDetail(aid) {
    var vData = {
        AssessmentID: aid,
        ServiceID: -1
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
    debugger
    $(".tosettleamount").blur(function () {
        var vRowID = $(this).data('rowid');
        //debugger
        var vPendingAmount = $(this).data('pendingamount');
        debugger
        var vToSettleAmount = $(this).val();

        var vRowrefNo = $(this).data('assref');
        debugger

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
////    $("#dvEditAssessmentRuleItemModalBody input.tosettleamount").each(function () {
////        if (parseInt($(this).data('tbachanged')) === 1) {

////            var vRowID = $(this).data('rowid');
////            var vID = $(this).data('id');
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
////        jsfn_ShowLoading();
////        $('#dvEditAssessmentRuleItemModal').modal('hide');
////        var vData = {
////            rowdata: vRowList,
////        };
////        jsfn_HideLoading();
////        //enter(vRowList);
////        jsfn_ajaxPost('/OperationManager/UpdateAssessmentNewItem', vData);

////        var url = "/OperationManager/UpdateAssessmentNewItem?rowdata=" + c;
////        window.location.href = url;

////    }
////    else {
////        $('#dvEditAssessmentRuleItemModal').modal('hide');
////    }

////}
////function jsfn_UpdateToSettleAmountResponse(data) {
////    console.log(data);
////    jsfn_HideLoading();
////    if (data.success) {
////        //enter2();
////        /////* $('#mayShow').html('hide');*/
////        // $("#dvAssessmentRule2").html(data.AssessmentRuleDetails);
////        //$("#mayShow").hide();

////    }
////    else {
////        jsfn_ShowAlert(data.Message, 'danger');
////    }
////}
////function enter(c)
////{
////    // var NestId = $(this).data('id');
////    var url = "/OperationManager/UpdateAssessmentNewItem?rowdata=" + c;

////};
////function enter2()
////{
////    var vRowList = 0;
////    var url = "/OperationManager/UpdateAssessmentNewItem?rowdata=" + vRowList;
////    window.location.href = url;
////};


////function jsfn_ShowAssessmentItems(arowId) {
////    var vData = {
////        AssessmentRuleRowID: arowId,
////    };

////    jsfn_ShowLoading();
////    jsfn_ajaxPost('/Base/GetAssessmentRuleItemView', vData, jsfn_ShowAssessmentItemResponse);
////}

////function jsfn_ShowAssessmentItemResponse(data) {
////    jsfn_HideLoading();
////    if (data.success) {
////        $('#dvViewAssessmentRuleItemModal').modal('show');
////        $('#spViewAssessmentRuleName').html(data.AssessmentRuleName);
////        $("#dvViewAssessmentRuleItemModalBody").html(data.AssessmentRuleItemList);

////    }
////    else {
////        jsfn_ShowAlert(data.Message, 'danger');
////    }
////}

////function jsfn_EditAssessmentItems(arowId) {
////    var vData = {
////        AssessmentRuleRowID: arowId,
////    };

////    jsfn_ShowLoading();
////    jsfn_ajaxPost('/Base/GetAssessmentRuleItemForSettlement', vData, jsfn_EditAssessmentItemResponse);
////}

////function jsfn_EditAssessmentItemResponse(data) {
////    jsfn_HideLoading();
////    console.log("new data" + data);
////    if (data.success) {
////        $('#dvEditAssessmentRuleItemModal').modal('show');
////        $('#spEditAssessmentRuleName').html(data.AssessmentRuleName);
////        $("#dvEditAssessmentRuleItemModalBody").html(data.AssessmentRuleItemList);
////        jsfn_BindPendingAmount();
////        $("#dvModalFooter").show();
////    }
////    else {
////        jsfn_ShowAlert(data.Message, 'danger');
////    }
////}

////function jsfn_ShowAsessmentRulePaymentDetail(arowId) {
////    var vData = {
////        AssessmentRuleRowID: arowId,
////    };

////    jsfn_ShowLoading();
////    jsfn_ajaxPost('/Base/GetAssessmentRulePaymentView', vData, jsfn_ShowAsessmentRulePaymentDetailResponse);
////}

////function jsfn_ShowAsessmentRulePaymentDetailResponse(data) {
////    jsfn_HideLoading();
////    if (data.success) {
////        $('#dvViewAssessmentRulePaymentModal').modal('show');
////        $('#spViewAssessmentRulePaymentName').html(data.AssessmentRuleName);
////        $("#dvViewAssessmentRulePaymentModalBody").html(data.AssessmentRulePaymentList);

////    }
////    else {
////        jsfn_ShowAlert(data.Message, 'danger');
////    }
////}

////function jsfn_ShowPaymentDetail(aid) {
////    var vData = {
////        AssessmentID: aid,
////        ServiceID : -1
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