$(document).ready(function () {

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

    $("#cboAssetType").on('click', jsfn_onAssetTypeChange);
    $("#cboAsset").on('click', jsfn_onAssetChange);
    $("#cboProfile").on('click', jsfn_onProfileChange);
    $("#cboAssessmentRule").on('click', jsfn_onAssessmentRuleChange);
    $('#txtSettlementDueDate').datepicker('option', 'minDate', new Date());
});

function jsfn_onAssetTypeChange() {
    var vAssetTypeID = $("#cboAssetType").val();

    var vData = {
        AssetTypeID: vAssetTypeID == '' ? 0 : vAssetTypeID,
        TaxPayerID: $("#hdnTaxPayerID").val()
    };

    jsfn_ajaxPost('/Company/GetTaxPayerAssetList', vData, jsfn_onAssetTypeChangeResponse);

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
        AssetTypeID: vAssetTypeID == '' ? 0 : vAssetTypeID,
        TaxPayerID: $("#hdnTaxPayerID").val()
    };

    jsfn_ajaxPost('/Company/GetProfileList', vData, jsfn_onAssetChangeResponse);

}

function jsfn_onAssetChangeResponse(data) {
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
    };

    jsfn_ajaxPost('/Company/GetAssessmentRuleList', vData, jsfn_onProfileChangeResponse);

}

function jsfn_onProfileChangeResponse(data) {
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

        jsfn_ajaxPost('/Company/GetAssessmentRuleItemList', vData, jsfn_onAssessmentRuleChangeResponse);
    }
    else {
        $("#lblTaxYear").html('');
        $("#tbodyAssessmentItemList").html('<tr><td colspan="4" align="center"> No Records Found. </td></tr>');
    }
}

function jsfn_onAssessmentRuleChangeResponse(data) {
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
    jsfn_ajaxPost('/Company/GetAssessmentItemDetails', vData, jsfn_ShowAssessmentItemDetailResponse)
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
        jsfn_ajaxPost('/Company/UpdateAssessmentItem', vData, jsfn_UpdateTaxAmountResponse);
    }
}

function jsfn_UpdateTaxAmountResponse(data) {
    jsfn_HideLoading();
    $("#tbodyAssessmentItemList").html(data.AssessmentItemList);
    $('#dvEditTaxAmount').dialog('close');
}