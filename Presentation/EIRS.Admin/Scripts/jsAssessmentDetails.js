$(document).ready(function () {
    $("#fsStep2").hide();
    $("#hdStep2").hide();
    $("#btnSave").hide();
    $("#btnBack").hide();

    $("#dvAssessmentRuleList").hide();
    $("#frmARAddAssessmentRule").hide();


    $('#dvAssessmentRuleDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Assessment Rule Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#dvAssessmentItemDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Assessment Item Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#dvEditTaxAmount').dialog({
        autoOpen: false,
        width: 500,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Edit Tax Base Amount</h4></div>",
        buttons: [{
            html: "<i class='fa fa-check'></i>&nbsp; Update",
            "class": "btn btn-primary",
            click: function () {
                jsfn_UpdateTaxAmount();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    var frmAssessmentItem = $('#frmAssessmentItem');

    // init validator obj and set the rules
    frmAssessmentItem.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            TaxBaseAmount: {
                required: true,
                number: true
            }
        },

        messages: {
            TaxBaseAmount: {
                required: 'Please enter tax base amount',
                number: 'Please enter proper tax base amount'
            }
        },

        // Do not change code below
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent());
            element.parent().addClass("state-error");
        },
        highlight: function (element) { // hightlight error inputs
            $(element).closest('.control-group').addClass('error'); // set error class to the control group
        },

        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.control-group').removeClass('error'); // set error class to the control group
        }
    });

    var frmAssessment = $('#frmAssessment');

    // init validator obj and set the rules
    frmAssessment.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            // the rules, as usual
        },
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent());
            element.parent().addClass("state-error");
        },
        highlight: function (element) { // hightlight error inputs
            $(element).closest('.control-group').addClass('error'); // set error class to the control group
        },

        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.control-group').removeClass('error'); // set error class to the control group
        }
    });

    var frmARAddAssessmentRule = $('#frmARAddAssessmentRule');

    // init validator obj and set the rules
    frmARAddAssessmentRule.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            AssetType: {
                required: true,
            },
            Asset: {
                required: true,
            },
            Profile: {
                required: true,
            },
            AssessmentRule: {
                required: true,
            }
            // the rules, as usual
        },

        messages: {
            AssetType: {
                required: 'Select Asset Type',
            },
            Asset: {
                required: 'Select Asset ',
            },
            Profile: {
                required: 'Select Profile',
            },
            AssessmentRule: {
                required: 'Select Assessment Rule',
            }
        },
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent());
            element.parent().addClass("state-error");
        },
        highlight: function (element) { // hightlight error inputs
            $(element).closest('.input').addClass('error'); // set error class to the control group
        },

        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.input').removeClass('error'); // set error class to the control group
        }
    });

    $("#btnNext").on('click', function () {
        if ($('#fsStep1 :input').valid()) {
            $("#fsStep2").show();
            $("#fsStep1").hide();
            $("#btnSave").show();
            $("#btnBack").show();
            $("#btnNext").hide();
            $("#btnMainBack").hide();
        }
    });

    $("#btnBack").on('click', function () {
        $("#fsStep2").hide();
        $("#fsStep1").show();
        $("#btnSave").hide();
        $("#btnBack").hide();
        $("#btnNext").show();
        $("#btnMainBack").show();
    });

    $("#aAssessmentRule").click(function () {
        $("#frmAssessment").hide();
        $("#dvAssessmentRuleList").show();
    });

    $("#btnARBack").click(function () {
        $("#frmAssessment").show();
        $("#dvAssessmentRuleList").hide();
    });

    $("#btnBackAssessmentRules").click(function () {
        $("#frmARAddAssessmentRule").hide();
        $("#dvAssessmentRuleList").show();
    });

    $("#btnARAdd").click(function () {
        $("#frmARAddAssessmentRule").show();
        $("#dvAssessmentRuleList").hide();

        $("#cboAssetType").val('').select2({ "width": "100%" });
        $("#cboAsset").html("").append($('<option></option>').val("").html("Select Asset")).val('').select2({ "width": "100%" });
        $("#cboProfile").html("").append($('<option></option>').val("").html("Select Profile")).val('').select2({ "width": "100%" });
        $("#cboAssessmentRule").html("").append($('<option></option>').val("").html("Select Assessment Rule")).val('').select2({ "width": "100%" });
        $("#lblTaxYear").html('');
        $("#tbodyAssessmentItemList").html("<tr><td colspan='4' align='center'>No Records Found.</td></tr>");

    });

    $("#btnAddAssessmentRules").click(function () {
        if (frmARAddAssessmentRule.valid()) {
            var vData = {
                AssetTypeID: $("#cboAssetType").val(),
                AssetTypeName: $("#cboAssetType option:selected").text(),
                AssetID: $("#cboAsset").val(),
                AssetRIN: $("#cboAsset option:selected").text(),
                ProfileID: $("#cboProfile").val(),
                ProfileDescription: $("#cboProfile option:selected").text(),
                AssessmentRuleID: $("#cboAssessmentRule").val(),
                AssessmentRuleName: $("#cboAssessmentRule option:selected").text(),
                TaxYear: $("#lblTaxYear").html()
            };

            jsfn_ShowLoading();
            jsfn_ajaxPost('/Assessment/AddAssessmentRule', vData, jsfn_AddAssessmentRuleResponse);
        }

        return false;
    });

    $("#cboAssetType").on('click', jsfn_onAssetTypeChange);
    $("#cboAsset").on('click', jsfn_onAssetChange);
    $("#cboProfile").on('click', jsfn_onProfileChange);
    $("#cboAssessmentRule").on('click', jsfn_onAssessmentRuleChange);
    $('#txtSettlementDueDate').datepicker('option', 'minDate', new Date());
});

$(document).on('submit', 'form', function () {
    var button = $(this).find('button[type="submit"]');
    setTimeout(function () {
        button.attr('disabled', 'disabled');
    }, 0);
});

function jsfn_onAssetTypeChange() {
    var vAssetTypeID = $("#cboAssetType").val();

    var vData = {
        AssetTypeID: vAssetTypeID == '' ? 0 : vAssetTypeID,
        TaxPayerID: $("#hdnTaxPayerID").val(),
        TaxPayerTypeID: $("#hdnTaxPayerTypeID").val()
    };

    jsfn_ajaxPost('/Assessment/GetTaxPayerAssetList', vData, jsfn_onAssetTypeChangeResponse);

}

function jsfn_onAssetTypeChangeResponse(data) {
    $("#cboAsset").html(""); // clear before appending new list

    $("#cboAsset").append(
        $('<option></option>').val("").html("Select Asset"));

    $.each(data, function (i, asst) {
        $("#cboAsset").append(
            $('<option></option>').val(asst.id).html(asst.text));
    });

    $('#cboAsset').val('').select2({ "width": "100%" });
}

function jsfn_onAssetChange() {
    var vAssetID = $("#cboAsset").val();
    var vAssetTypeID = $("#cboAssetType").val();

    var vData = {
        AssetID: vAssetID == '' ? 0 : vAssetID,
        TaxPayerID: $("#hdnTaxPayerID").val(),
        AssetTypeID: vAssetTypeID == '' ? 0 : vAssetTypeID,
        TaxPayerTypeID: $("#hdnTaxPayerTypeID").val()
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Assessment/GetProfileList', vData, jsfn_onAssetChangeResponse);

}

function jsfn_onAssetChangeResponse(data) {
    jsfn_HideLoading();
    $("#cboProfile").html(""); // clear before appending new list

    $("#cboProfile").append(
        $('<option></option>').val("").html("Select Profile"));

    $.each(data, function (i, prf) {
        $("#cboProfile").append(
            $('<option></option>').val(prf.id).html(prf.text));
    });

    $('#cboProfile').val('').select2({ "width": "100%" });
}

function jsfn_onProfileChange() {

    var vProfileID = $("#cboProfile").val();
    var vAssetID = $("#cboAsset").val();
    var vAssetTypeID = $("#cboAssetType").val();

    var vData = {
        ProfileID: vProfileID == '' ? 0 : vProfileID,
        AssetID: vAssetID == '' ? 0 : vAssetID,
        TaxPayerID: $("#hdnTaxPayerID").val(),
        AssetTypeID: vAssetTypeID == '' ? 0 : vAssetTypeID,
        TaxPayerTypeID: $("#hdnTaxPayerTypeID").val()
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Assessment/GetAssessmentRuleList', vData, jsfn_onProfileChangeResponse);

}

function jsfn_onProfileChangeResponse(data) {
    jsfn_HideLoading();
    $("#cboAssessmentRule").html(""); // clear before appending new list

    $("#cboAssessmentRule").append(
        $('<option></option>').val("").html("Select Assessment Rule"));

    $.each(data, function (i, arule) {
        $("#cboAssessmentRule").append(
            $('<option></option>').val(arule.id).html(arule.text));
    });

    $('#cboAssessmentRule').val('').select2({ "width": "100%" });
}

function jsfn_onAssessmentRuleChange() {
    var vAssessmentRuleID = $("#cboAssessmentRule").val();

    if (vAssessmentRuleID != '') {
        var vData = {
            AssessmentRuleID: vAssessmentRuleID == '' ? 0 : vAssessmentRuleID
        };
        jsfn_ShowLoading();
        jsfn_ajaxPost('/Assessment/GetAssessmentRuleItemList', vData, jsfn_onAssessmentRuleChangeResponse);
    }
    else {
        $("#lblTaxYear").html('');
        $("#tbodyAssessmentItemList").html('<tr><td colspan="4" align="center"> No Records Found. </td></tr>');
    }
}

function jsfn_onAssessmentRuleChangeResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $("#lblTaxYear").html(data.TaxYear);
        $("#tbodyAssessmentItemList").html(data.AssessmentItemList);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowAssessmentItemDetails(AssessmentItemID) {

    var vData = {
        AssessmentItemID: AssessmentItemID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Assessment/GetAssessmentItemDetails', vData, jsfn_ShowAssessmentItemDetailResponse)
}

function jsfn_ShowAssessmentItemDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentItemDetails').dialog('open');
        $('#dvAssessmentItemRefNo').html(data.AssessmentItemDetails.AssessmentItemReferenceNo);
        $('#dvAIAssetType').html(data.AssessmentItemDetails.AssetTypeName);
        $('#dvAssessmentGroup').html(data.AssessmentItemDetails.AssessmentGroupName);
        $('#dvAssessmentSubGroup').html(data.AssessmentItemDetails.AssessmentSubGroupName);
        $('#dvRevenueStream').html(data.AssessmentItemDetails.RevenueStreamName);
        $('#dvRevenueSubStream').html(data.AssessmentItemDetails.RevenueSubStreamName);
        $('#dvAssessmentItemCategory').html(data.AssessmentItemDetails.AssessmentItemCategoryName);
        $('#dvAssessmentItemSubCategory').html(data.AssessmentItemDetails.AssessmentItemSubCategoryName);
        $('#dvRevenueAgency').html(data.AssessmentItemDetails.AgencyName);
        $('#dvAssessmentItemName').html(data.AssessmentItemDetails.AssessmentItemName);
        $('#dvComputation').html(data.AssessmentItemDetails.ComputationName);
        $('#dvTaxBaseAmount').html(data.AssessmentItemDetails.TaxBaseAmount);
        $('#dvPercentage').html(data.AssessmentItemDetails.Percentage);
        $('#dvTaxAmount').html(data.AssessmentItemDetails.TaxAmount);
      
        if (data.AssessmentItemDetails.ComputationID == 1) {
            $('#dvCPercentage').hide();
        }
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_EditTaxAmount(rowId, taxbaseamount) {
    $('#dvEditTaxAmount').dialog('open');
    $("#hdnAssessmentItemRowID").val(rowId);
    $("#txtTaxBaseAmount").val(taxbaseamount);
}

function jsfn_UpdateTaxAmount() {
    if ($('#frmAssessmentItem').valid()) {
        var vData = {
            AssessmentItemRowID: $("#hdnAssessmentItemRowID").val(),
            TaxBaseAmount: $("#txtTaxBaseAmount").val()
        };
        jsfn_ShowLoading();
        jsfn_ajaxPost('/Assessment/UpdateAssessmentItem', vData, jsfn_UpdateTaxAmountResponse);
    }
}

function jsfn_UpdateTaxAmountResponse(data) {
    jsfn_HideLoading();
    $("#tbodyAssessmentItemList").html(data.AssessmentItemList);
    $('#dvEditTaxAmount').dialog('close');
}

function jsfn_AddAssessmentRuleResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        $('#tbodyAssessmentRuleList').html(data.AssessmentRuleList);
        $('#spnARRecordCount').html(data.AssessmentRuleCount);
        $("#frmARAddAssessmentRule").hide();
        $("#dvAssessmentRuleList").show();
        $("#ddliAssessmentRule").removeClass('state-error');
        $("#AssessmentRule-error").addClass('hide');

        $("#cboAssetType").val('').select2({ "width": "100%" });
        $("#cboAsset").html("").append($('<option></option>').val("").html("Select Asset")).val('').select2({ "width": "100%" });
        $("#cboProfile").html("").append($('<option></option>').val("").html("Select Profile")).val('').select2({ "width": "100%" });
        $("#cboAssessmentRule").html("").append($('<option></option>').val("").html("Select Assessment Rule")).val('').select2({ "width": "100%" });
        $("#lblTaxYear").html('');
        $("#tbodyAssessmentItemList").html(data.AssessmentItemList);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_RemoveAssessmentRule(rowid) {
    var vData = {
        RowID: rowid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Assessment/RemoveAssessmentRule', vData, jsfn_RemoveAssessmentRuleResponse)
}

function jsfn_RemoveAssessmentRuleResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        $('#tbodyAssessmentRuleList').html(data.AssessmentRuleList);
        $('#spnARRecordCount').html(data.AssessmentRuleCount);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowAssessmentRuleDetails(AssessmentRuleID) {

    var vData = {
        AssessmentRuleID: AssessmentRuleID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Assessment/GetAssessmentRuleDetails', vData, jsfn_ShowAssessmentRuleDetailResponse)
}

function jsfn_ShowAssessmentRuleDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentRuleDetails').dialog('open');
        $('#dvAssessmentRuleRefNo').html(data.AssessmentRuleDetails.AssessmentRuleCode);
        $('#dvAssessmentRuleName').html(data.AssessmentRuleDetails.AssessmentRuleName);
        $('#dvRuleRun').html(data.AssessmentRuleDetails.RuleRunName);
        $('#dvFrequency').html(data.AssessmentRuleDetails.PaymentFrequencyName);
        $('#dvTaxYear').html(data.AssessmentRuleDetails.TaxYear);
        $('#dvSettlementMethod').html(data.AssessmentRuleDetails.SettlementMethodNames);
        $('#dvPaymentOption').html(data.AssessmentRuleDetails.PaymentOptionName);
        $('#dvStatus').html(data.AssessmentRuleDetails.ActiveText);
        $('#dvProfileDescription').html(data.AssessmentRuleDetails.ProfileReferenceNo);
        $('#dvAssessmentItems').html(data.AssessmentRuleDetails.AssessmentItemNames);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}