$(document).ready(function () {
    $('#btnSubmit').click(function () {
        if ($("#frmMDAServiceItem").valid()) {
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
                $("#frmMDAServiceItem").submit();
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
    $('#txtServiceBaseAmount').on('blur', jsfn_CalculateServiceAmount);
    $('#txtPercentage').on('blur', jsfn_CalculateServiceAmount);
    $('#cboRevenueStream').on("change", jsfn_onRevenueStreamChange);
    $('#cboItemCategory').on("change", jsfn_onItemCategoryChange);


    jsfn_onComputationChange();
});

function jsfn_onComputationChange() {
    var vComputation = $("#cboComputation").val();

    if (vComputation == '2') {
        $("#scServicePercentage").removeClass('hide');
    }
    else {
        $("#scServicePercentage").addClass('hide');
    }

    jsfn_CalculateServiceAmount();
}

function jsfn_CalculateServiceAmount() {
    var vServiceBaseAmount = $("#txtServiceBaseAmount").val();
    var vComputation = $("#cboComputation").val();
    var vPercentage = $("#txtPercentage").val();

    if (vComputation == '2') {
        $("#lblServiceAmount").html(parseFloat(vServiceBaseAmount) * parseFloat(vPercentage) / 100);
    }
    else {

        if (vServiceBaseAmount == '') {
            $("#lblServiceAmount").html("-");
        }
        else {
            $("#lblServiceAmount").html(vServiceBaseAmount);
        }
    }
}

function jsfn_onRevenueStreamChange() {
    var vRevenueStreamID = $('#cboRevenueStream').val();

    if (vRevenueStreamID != '') {

        var vData = {
            RevenueStreamID: vRevenueStreamID
        };

        jsfn_ajaxPost('/MDAServiceItem/GetRevenueSubStream', vData, jsfn_onRevenueStreamChangeResponse);
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


function jsfn_onItemCategoryChange() {
    var vItemCategoryID = $('#cboItemCategory').val();

    if (vItemCategoryID != '') {

        var vData = {
            AssessmentItemCategoryID: vItemCategoryID
        };

        jsfn_ajaxPost('/MDAServiceItem/GetAssessmentItemSubCategory', vData, jsfn_onItemCategoryChangeResponse);
    }
    else {
        $("#cboItemSubCategory").html("");
        $("#cboItemSubCategory").append($('<option></option>').val("").html("Select Item Sub Category"));
        $('#cboItemSubCategory').val('').select2({ "width": "100%" });
    }
}

function jsfn_onItemCategoryChangeResponse(data) {
    $("#cboItemSubCategory").html("");

    $("#cboItemSubCategory").append(
            $('<option></option>').val("").html("Select Item Sub Category"));

    $.each(data, function (i, asgrp) {
        $("#cboItemSubCategory").append(
            $('<option></option>').val(asgrp.id).html(asgrp.text));
    });

    $('#cboItemSubCategory').val('').select2({ "width": "100%" });
}
