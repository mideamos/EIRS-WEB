$(document).ready(function () {
    $('#btnSubmit').click(function () {
        if ($("#frmAssessmentItem").valid()) {
            $('#dvUpdateDialog').dialog('open');
        }
        return false;

    });

    $('#dvUpdateDialog').removeClass('hide');
    $('#dvUpdateDialog').dialog({
        autoOpen: false,
        width: 600,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Edit Record Confirmation</h4></div>",
        buttons: [{
            html: "<i class='fa fa-check'></i>&nbsp; Yes",
            "class": "btn btn-primary",
            click: function () {
                $("#frmAssessmentItem").submit();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#cboComputation').on('change', jsfn_onComputationChange);
    $('#txtTaxBaseAmount').on('blur', jsfn_CalculateTaxAmount);
    $('#txtPercentage').on('blur', jsfn_CalculateTaxAmount);
    $('#cboAssetType').on("change", jsfn_onAssetTypeChange);
    $('#cboAssessmentGroup').on("change", jsfn_onAssessmentGroupChange);
    $('#cboRevenueStream').on("change", jsfn_onRevenueStreamChange);
    $('#cboAssessmentItemCategory').on("change", jsfn_onAssessmentItemCategoryChange);


    jsfn_onComputationChange();
});

function jsfn_onComputationChange() {
    var vComputation = $("#cboComputation").val();

    if (vComputation == '2') {
        $("#scTaxPercentage").removeClass('hide');
    }
    else {
        $("#scTaxPercentage").addClass('hide');
    }

    jsfn_CalculateTaxAmount();
}

function jsfn_CalculateTaxAmount() {
    var vTaxBaseAmount = $("#txtTaxBaseAmount").val();
    var vComputation = $("#cboComputation").val();
    var vPercentage = $("#txtPercentage").val();

    if (vComputation == '2') {
        $("#lblTaxAmount").html(parseFloat(vTaxBaseAmount) * parseFloat(vPercentage) / 100);
    }
    else {

        if (vTaxBaseAmount == '') {
            $("#lblTaxAmount").html("-");
        }
        else {
            $("#lblTaxAmount").html(vTaxBaseAmount);
        }
    }
}

function jsfn_onAssetTypeChange() {
    var vAssetTypeID = $('#cboAssetType').val();

    if (vAssetTypeID != '') {

        var vData = {
            AssetTypeID: vAssetTypeID == '' ? 0 : vAssetTypeID
        };

        jsfn_ajaxPost('/AssessmentItem/GetDropDownList', vData, jsfn_onAssetTypeChangeResponse);
    }
    else {
        $("#cboAssessmentGroup").html("");
        $("#cboAssessmentGroup").append($('<option></option>').val("").html("Select Assessment Group"));
        $('#cboAssessmentGroup').val('').select2({ "width": "100%" });
        //$("#cboRevenueStream").html("");
        //$("#cboRevenueStream").append($('<option></option>').val("").html("Select Revenue Stream"));
        //$('#cboRevenueStream').val('').select2({ "width": "100%" });
        jsfn_onAssessmentGroupChange();
        //jsfn_onRevenueStreamChange();
    }
}

function jsfn_onAssetTypeChangeResponse(data) {
    $("#cboAssessmentGroup").html("");

    $("#cboAssessmentGroup").append(
            $('<option></option>').val("").html("Select Assessment Group"));

    if (data != undefined) {
        $.each(data.AssessmentGroupList, function (i, agrp) {
            $("#cboAssessmentGroup").append(
                $('<option></option>').val(agrp.id).html(agrp.text));
        });

        $('#cboAssessmentGroup').val('').select2({ "width": "100%" });
    }

    jsfn_onAssessmentGroupChange();

    //$("#cboRevenueStream").html("");

    //$("#cboRevenueStream").append(
    //        $('<option></option>').val("").html("Select Revenue Stream"));

    //if (data != undefined) {
    //    $.each(data.RevenueStreamList, function (i, rstrm) {
    //        $("#cboRevenueStream").append(
    //            $('<option></option>').val(rstrm.id).html(rstrm.text));
    //    });

    //    $('#cboRevenueStream').val('').select2({ "width": "100%" });
    //}
    //jsfn_onRevenueStreamChange();
}

function jsfn_onAssessmentGroupChange() {
    var vAssessmentGroupID = $('#cboAssessmentGroup').val();

    if (vAssessmentGroupID != '') {

        var vData = {
            AssessmentGroupID: vAssessmentGroupID
        };

        jsfn_ajaxPost('/AssessmentItem/GetAssessmentSubGroup', vData, jsfn_onAssessmentGroupChangeResponse);
    }
    else {
        $("#cboAssessmentSubGroup").html("");
        $("#cboAssessmentSubGroup").append($('<option></option>').val("").html("Select Assessment Sub Group"));
        $('#cboAssessmentSubGroup').val('').select2({ "width": "100%" });
    }
}

function jsfn_onAssessmentGroupChangeResponse(data) {
    $("#cboAssessmentSubGroup").html("");

    $("#cboAssessmentSubGroup").append(
            $('<option></option>').val("").html("Select Assessment Sub Group"));

    $.each(data, function (i, asgrp) {
        $("#cboAssessmentSubGroup").append(
            $('<option></option>').val(asgrp.id).html(asgrp.text));
    });

    $('#cboAssessmentSubGroup').val('').select2({ "width": "100%" });
}

function jsfn_onRevenueStreamChange() {
    var vRevenueStreamID = $('#cboRevenueStream').val();

    if (vRevenueStreamID != '') {

        var vData = {
            RevenueStreamID: vRevenueStreamID
        };

        jsfn_ajaxPost('/AssessmentItem/GetRevenueSubStream', vData, jsfn_onRevenueStreamChangeResponse);
    }
    else {
        $("#cboRevenueSubStream").html("");
        $("#cboRevenueSubStream").append($('<option></option>').val("").html("Select Revenue Sub Stream"));
        $('#cboRevenueSubStream').val('').select2({ "width": "100%" });
    }
}

function jsfn_onRevenueStreamChangeResponse(data) {
    $("#cboRevenueSubStream").html("");

    $("#cboRevenueSubStream").append(
            $('<option></option>').val("").html("Select Revenue Sub Stream"));

    $.each(data, function (i, asgrp) {
        $("#cboRevenueSubStream").append(
            $('<option></option>').val(asgrp.id).html(asgrp.text));
    });

    $('#cboRevenueSubStream').val('').select2({ "width": "100%" });
}


function jsfn_onAssessmentItemCategoryChange() {
    var vAssessmentItemCategoryID = $('#cboAssessmentItemCategory').val();

    if (vAssessmentItemCategoryID != '') {

        var vData = {
            AssessmentItemCategoryID: vAssessmentItemCategoryID
        };

        jsfn_ajaxPost('/AssessmentItem/GetAssessmentItemSubCategory', vData, jsfn_onAssessmentItemCategoryChangeResponse);
    }
    else {
        $("#cboAssessmentItemSubCategory").html("");
        $("#cboAssessmentItemSubCategory").append($('<option></option>').val("").html("Select Assessment Item Sub Category"));
        $('#cboAssessmentItemSubCategory').val('').select2({ "width": "100%" });
    }
}

function jsfn_onAssessmentItemCategoryChangeResponse(data) {
    $("#cboAssessmentItemSubCategory").html("");

    $("#cboAssessmentItemSubCategory").append(
            $('<option></option>').val("").html("Select Assessment Item Sub Category"));

    $.each(data, function (i, asgrp) {
        $("#cboAssessmentItemSubCategory").append(
            $('<option></option>').val(asgrp.id).html(asgrp.text));
    });

    $('#cboAssessmentItemSubCategory').val('').select2({ "width": "100%" });
}
