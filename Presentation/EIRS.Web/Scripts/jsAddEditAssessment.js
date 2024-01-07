$(document).ready(function () {
   
    $("#btnSaveItems").click(function () { jsfn_UpdateTaxAmount(); });
    //debugger
    $("#btnFinalSave").click(function () {
        $("#frmAddAssessment").submit();
    });

    $("#btnAddSelectedAssessment").click(function () {
        debugger
        if ($("#hdnAssessmentRuleIds").val() != '') {
            var vData = {
                AssessmentRuleIds: $("#hdnAssessmentRuleIds").val()
            };

            jsfn_ShowLoading();
            jsfn_ajaxPost('/Base/AddAssessmentRules', vData, jsfn_AddAssessmentRuleResponse);
        }
        else {
            jsfn_ShowAlert('Select atleast one assessment rule', 'danger');
        }

        return false;
    });

    $("#btnAddAssessmentRule").click(function () {
        var vData = {
            TaxPayerTypeID: $("#hdnTaxPayerTypeID").val(),
            TaxPayerID: $("#hdnTaxPayerID").val()
        };

        jsfn_ShowLoading();
        jsfn_ajaxPost('/Base/GetTaxPayerAssessmentRules', vData, jsfn_ShowAssessmentRuleResponse);
    });

    $("#btnModalYes").click(function () {
        var vData = {
            RowID: $("#hdnTBID").val()
        };

        jsfn_ShowLoading();
        jsfn_ajaxPost('/Base/RemoveAssessmentRule', vData, jsfn_RemoveAssessmentRuleResponse);
    });

   

});

function jsfn_AddAssessmentRuleResponse(data) {
    debugger
    console.log( data )
    jsfn_HideLoading();
    if (data.success) {
        $("#hdnAssessmentRuleIds").val("");
        $('#dvAssessmentRuleModal').modal('hide');
        $("#dvAssessmentRule").html(data.AssessmentRuleDetails);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_onAssessmentRuleSelected(aruleid, isChecked) {
    var vArrAssessmentRule = $('#hdnAssessmentRuleIds').val().split(',');
    if (isChecked) {
        if ($('#hdnAssessmentRuleIds').val() != "")
            vArrAssessmentRule.push(aruleid);
        else {
            vArrAssessmentRule = new Array();
            vArrAssessmentRule.push(aruleid);
        }
    }
    else {
        vArrAssessmentRule.splice($.inArray(aruleid, vArrAssessmentRule), 1);
    }

    $('#hdnAssessmentRuleIds').val(vArrAssessmentRule.toString());
}

function jsfn_ShowAssessmentRuleResponse(data) {
    debugger
    jsfn_HideLoading();
    $("#dvAssessmentRuleList").html(data.AssessmentRuleList);
    $('#dvAssessmentRuleModal').modal('show');
    handleiCheck();
    debugger
    $(".icheck").on("ifChecked", function () {
        jsfn_onAssessmentRuleSelected($(this).data("aruleid").toString(), true);
    });

    $(".icheck").on("ifUnchecked", function () {
        jsfn_onAssessmentRuleSelected($(this).data("aruleid").toString(), false);
    });

    jsfn_bindTable();
   
}

function jsfn_bindTable() {
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

    jsfn_ConvertToDatableWithCustomPaging($('#tblAssessmentRules'), vColumnsList, 'No Assessment Rule Found', 5);
}

$(document).on('submit', 'form', function () {
    var button = $("#btnFinalSave");
    setTimeout(function () {
        button.attr('disabled', 'disabled');
    }, 0);
});


function jsfn_UpdateTaxAmount() {
    debugger
    var vRowList = '';
    $("#dvEditAssessmentRuleItemModalBody input.cbaseamount").each(function () {
        if (parseInt($(this).data('tbachanged')) === 1) {
            var vRowID = $(this).data('rowid');
            var vTaxBaseAmount = $(this).val();

            if (vRowList == '') {
                vRowList = vRowID + '@@' + vTaxBaseAmount;
            }
            else {
                vRowList = vRowList + '~~' + vRowID + '@@' + vTaxBaseAmount;
            }
        }
    });

    if (vRowList != '') {

        var vData = {
            rowdata: vRowList,
        };

        jsfn_ShowLoading();
        jsfn_ajaxPost('/Base/UpdateAssessmentItem', vData, jsfn_UpdateTaxAmountResponse);
    }
    else {
        $('#dvEditAssessmentRuleItemModal').modal('hide');
    }

}

function jsfn_UpdateTaxAmountResponse(data) {
    debugger
    jsfn_HideLoading();
    if (data.success) {
        $('#dvEditAssessmentRuleItemModal').modal('hide');
        $("#dvAssessmentRule").html(data.AssessmentRuleDetails);
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
 
    console.log("i got here ")
    debugger
    var vData = {
        AssessmentRuleRowID: arowId,
    };
    jsfn_ajaxPost('/Base/GetAssessmentRuleItemEdit', vData, jsfn_EditAssessmentItemResponse);
}

function jsfn_EditAssessmentItemResponse(data) {
    
    jsfn_HideLoading();
    if (data.success) {
        $('#dvEditAssessmentRuleItemModal').modal('show');
        $('#spEditAssessmentRuleName').html(data.AssessmentRuleName);
        $("#dvEditAssessmentRuleItemModalBody").html(data.AssessmentRuleItemList);
        if ($("#dvEditAssessmentRuleItemModalBody input.cbaseamount").length > 0) {
            jsfn_BindTaxBaseAmount();
            $("#dvModalFooter").show();
        }
        else {
            $("#dvModalFooter").hide();
        }
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BindTaxBaseAmount() {
    debugger
    $(".cbaseamount").blur(function () {
        var vComputationID = $(this).data('computationid');
        var vRowID = $(this).data('rowid');
        var vPercentage = $(this).data('percent');
        var vTaxBaseAmount = $(this).val();

        if (parseInt(vComputationID) === 2) {
            var vTaxAmount = parseFloat(vTaxBaseAmount) * (parseFloat(vPercentage) / 100);
        }
        else if (parseInt(vComputationID) === 3) {
            var vTaxAmount = parseFloat(vTaxBaseAmount);
        }

        $("#td_" + vRowID).html(vTaxAmount.formatMoney(2, '₦'));

        $(this).attr('data-tbachanged', '1');
    });
}

function jsfn_RemoveAssessmentRule(arowId) {
    $('#dvRemoveModal').modal('show');
    $("#hdnTBID").val(arowId);
}

function jsfn_RemoveAssessmentRuleResponse(data) {
    jsfn_HideLoading();

    if (data.success) {
        $('#dvRemoveModal').modal('hide')
        $("#hdnTBID").val(0);
        $("#dvAssessmentRule").html(data.AssessmentRuleDetails);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
