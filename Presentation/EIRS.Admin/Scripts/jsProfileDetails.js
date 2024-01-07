$(document).ready(function () {
    $("#fsStep2").hide();
    $("#btnSave").hide();
    $("#btnBack").hide();

    if ($("#hdnProfileID").val() != '-1') {

        $('#btnSave').click(function () {
            if ($("#frmProfile").valid()) {
                $('#dvUpdateDialog').dialog('open');
            }
            return false;

        });
    }

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
                $("#frmProfile").submit();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    var frmProfile = $('#frmProfile');

    // init validator obj and set the rules
    frmProfile.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            // the rules, as usual
        },

        highlight: function (element) { // hightlight error inputs
            $(element).closest('.control-group').addClass('error'); // set error class to the control group
        },

        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.control-group').removeClass('error'); // set error class to the control group
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

    var vTaxPayerTypeData = new Array();
    var vcboTaxPayerTypeValue = $('#cboTaxPayerType').attr('data-initvalue');
    if (vcboTaxPayerTypeValue != '')
        vTaxPayerTypeData = vcboTaxPayerTypeValue.split(',');

    $('#cboTaxPayerType').val(vTaxPayerTypeData).select2({ "width": "100%" });

    var vTaxPayerRoleData = new Array();
    var vcboTaxPayerRoleValue = $('#cboTaxPayerRole').attr('data-initvalue');
    if (vcboTaxPayerRoleValue != '')
        vTaxPayerRoleData = vcboTaxPayerRoleValue.split(',');

    $('#cboTaxPayerRole').val(vTaxPayerRoleData).select2({ "width": "100%" });

    var vProfileSectorData = new Array();
    var vcboProfileSectorValue = $('#cboProfileSector').attr('data-initvalue');
    if (vcboProfileSectorValue != '')
        vProfileSectorData = vcboProfileSectorValue.split(',');

    $('#cboProfileSector').val(vProfileSectorData).select2({ "width": "100%" });

    var vProfileSubSectorData = new Array();
    var vcboProfileSubSectorValue = $('#cboProfileSubSector').attr('data-initvalue');
    if (vcboProfileSubSectorValue != '')
        vProfileSubSectorData = vcboProfileSubSectorValue.split(',');

    $('#cboProfileSubSector').val(vProfileSubSectorData).select2({ "width": "100%" });

    var vProfileGroupData = new Array();
    var vcboProfileGroupValue = $('#cboProfileGroup').attr('data-initvalue');
    if (vcboProfileGroupValue != '')
        vProfileGroupData = vcboProfileGroupValue.split(',');

    $('#cboProfileGroup').val(vProfileGroupData).select2({ "width": "100%" });

    var vProfileSubGroupData = new Array();
    var vcboProfileSubGroupValue = $('#cboProfileSubGroup').attr('data-initvalue');
    if (vcboProfileSubGroupValue != '')
        vProfileSubGroupData = vcboProfileSubGroupValue.split(',');

    $('#cboProfileSubGroup').val(vProfileSubGroupData).select2({ "width": "100%" });

    var vProfileSectorElementData = new Array();
    var vcboProfileSectorElementValue = $('#cboProfileSectorElement').attr('data-initvalue');
    if (vcboProfileSectorElementValue != '')
        vProfileSectorElementData = vcboProfileSectorElementValue.split(',');

    $('#cboProfileSectorElement').val(vProfileSectorElementData).select2({ "width": "100%" });

    var vProfileSectorSubElementData = new Array();
    var vcboProfileSectorSubElementValue = $('#cboProfileSectorSubElement').attr('data-initvalue');
    if (vcboProfileSectorSubElementValue != '')
        vProfileSectorSubElementData = vcboProfileSectorSubElementValue.split(',');

    $('#cboProfileSectorSubElement').val(vProfileSectorSubElementData).select2({ "width": "100%" });

    var vProfileAttributeData = new Array();
    var vcboProfileAttributeValue = $('#cboProfileAttribute').attr('data-initvalue');
    if (vcboProfileAttributeValue != '')
        vProfileAttributeData = vcboProfileAttributeValue.split(',');

    $('#cboProfileAttribute').val(vProfileAttributeData).select2({ "width": "100%" });

    var vProfileSubAttributeData = new Array();
    var vcboProfileSubAttributeValue = $('#cboProfileSubAttribute').attr('data-initvalue');
    if (vcboProfileSubAttributeValue != '')
        vProfileSubAttributeData = vcboProfileSubAttributeValue.split(',');

    $('#cboProfileSubAttribute').val(vProfileSubAttributeData).select2({ "width": "100%" });

    $("#cboAssetType").on('click', jsfn_onAssetTypeChange);
});

function jsfn_onAssetTypeChange() {
    var vAssetTypeID = $("#cboAssetType").val();

    var vData = {
        AssetTypeID: vAssetTypeID == '' ? 0 : vAssetTypeID
    };

    jsfn_ajaxPost('/Profile/GetDropDownList', vData, jsfn_onAssetTypeChangeResponse);

}

function jsfn_onAssetTypeChangeResponse(data) {
    if (data != undefined) {

        $("#cboTaxPayerRole").html("");

        if (data.TaxPayerRoleList != undefined) {
            $.each(data.TaxPayerRoleList, function (i, tprol) {
                $("#cboTaxPayerRole").append(
                    $('<option></option>').val(tprol.id).html(tprol.text));
            });
        }

        $('#cboTaxPayerRole').val('').select2({ "width": "100%" });


        $("#cboProfileSector").html("");

        $("#cboProfileSector").append(
            $('<option></option>').val("-1").html("All"));

        if (data.ProfileSectorList != undefined) {
            $.each(data.ProfileSectorList, function (i, psect) {
                $("#cboProfileSector").append(
                    $('<option></option>').val(psect.id).html(psect.text));
            });
        }

        $('#cboProfileSector').val('').select2({ "width": "100%" });


        $("#cboProfileSubSector").html("");

        $("#cboProfileSubSector").append(
            $('<option></option>').val("-1").html("All"));

        if (data.ProfileSubSectorList != undefined) {
            $.each(data.ProfileSubSectorList, function (i, pssect) {
                $("#cboProfileSubSector").append(
                    $('<option></option>').val(pssect.id).html(pssect.text));
            });
        }

        $('#cboProfileSubSector').val('').select2({ "width": "100%" });

        $("#cboProfileGroup").html("");

        $("#cboProfileGroup").append(
            $('<option></option>').val("-1").html("All"));

        if (data.ProfileGroupList != undefined) {
            $.each(data.ProfileGroupList, function (i, pgrp) {
                $("#cboProfileGroup").append(
                    $('<option></option>').val(pgrp.id).html(pgrp.text));
            });
        }

        $('#cboProfileGroup').val('').select2({ "width": "100%" });

        $("#cboProfileSubGroup").html("");

        $("#cboProfileSubGroup").append(
            $('<option></option>').val("-1").html("All"));

        if (data.ProfileSubGroupList != undefined) {
            $.each(data.ProfileSubGroupList, function (i, psgrp) {
                $("#cboProfileSubGroup").append(
                    $('<option></option>').val(psgrp.id).html(psgrp.text));
            });
        }

        $('#cboProfileSubGroup').val('').select2({ "width": "100%" });

        $("#cboProfileSectorElement").html("");

        $("#cboProfileSectorElement").append(
            $('<option></option>').val("-1").html("All"));

        if (data.ProfileSectorElementList != undefined) {
            $.each(data.ProfileSectorElementList, function (i, psel) {
                $("#cboProfileSectorElement").append(
                    $('<option></option>').val(psel.id).html(psel.text));
            });
        }

        $('#cboProfileSectorElement').val('').select2({ "width": "100%" });

        $("#cboProfileSectorSubElement").html("");

        $("#cboProfileSectorSubElement").append(
            $('<option></option>').val("-1").html("All"));

        if (data.ProfileSectorSubElementList != undefined) {
            $.each(data.ProfileSectorSubElementList, function (i, pssel) {
                $("#cboProfileSectorSubElement").append(
                    $('<option></option>').val(pssel.id).html(pssel.text));
            });
        }

        $('#cboProfileSectorSubElement').val('').select2({ "width": "100%" });


        $("#cboProfileAttribute").html("");

        $("#cboProfileAttribute").append(
            $('<option></option>').val("-1").html("All"));

        if (data.ProfileAttributeList != undefined) {
            $.each(data.ProfileAttributeList, function (i, patr) {
                $("#cboProfileAttribute").append(
                    $('<option></option>').val(patr.id).html(patr.text));
            });
        }

        $('#cboProfileAttribute').val('').select2({ "width": "100%" });

        $("#cboProfileSubAttribute").html("");

        $("#cboProfileSubAttribute").append(
            $('<option></option>').val("-1").html("All"));

        if (data.ProfileSubAttributeList != undefined) {
            $.each(data.ProfileSubAttributeList, function (i, psatr) {
                $("#cboProfileSubAttribute").append(
                    $('<option></option>').val(psatr.id).html(psatr.text));
            });
        }

        $('#cboProfileSubAttribute').val('').select2({ "width": "100%" });
    }
}
