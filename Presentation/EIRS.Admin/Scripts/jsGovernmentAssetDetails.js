$(document).ready(function () {
    $("#fsStep2").hide();
    $("#fsStep3").hide();
    $("#btnSave").hide();
    $("#btnStep2Back").hide();
    $("#btnStep3Back").hide();
    $("#btnStep2Next").hide();

    var frmAsset = $('#frmAsset');

    // init validator obj and set the rules
    frmAsset.validate({
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
            $("#dvFooter").hide();

            //If Asset Type = Building then show next else show save
            if ($('#cboAssetType').val() == '1') {
                $("#btnStep2Back").show();
                $("#btnStep2Next").show();
            }
            else {
                $("#btnSave").show();
                $("#btnStep2Back").show();
            }

            jsfn_BindAssetTable();
        }
    });

    $("#btnStep2Back").click(function () {
        $("#fsStep2").hide();
        $("#fsStep1").show();
        $("#dvFooter").show();

        //If Asset Type = Building then show next else show save
        if ($('#cboAssetType').val() == '1') {
            $("#btnStep2Back").hide();
            $("#btnStep2Next").hide();
        }
        else {
            $("#btnSave").hide();
            $("#btnStep2Back").hide();
        }

        $("#artTaxPayerAsset").addClass("col-lg-6");
        return false;
    });

    $("#btnStep3Back").click(function () {
        $("#fsStep3").hide();
        $("#fsStep2").show();
        $("#btnSave").hide();
        $("#btnStep3Back").hide();
        $("#btnStep2Next").show();
        $("#btnStep2Back").show();

        return false;
    });

    $("#btnStep2Next").click(function () {
        if ($("#hdnAssetIds").val() != '' && $("#hdnAssetIds").val() != '0') {
            jsfn_BindBuildingUnitTable();
        }
        else {
            jsfn_ShowAlert('Select atleast one building', 'danger');
        }

        return false;
    });

    $('#btnSave').click(function () {
        if ($("#hdnAssetIds").val() != '') {
            $("#frmAsset").submit();
        }
        else {
            jsfn_ShowAlert('Select Atleast one Asset', 'danger');
        }

        return false;
    });

    $('#cboAssetType').on("change", jsfn_onAssetTypeChange);
});

function jsfn_onAssetTypeChange() {
    var vAssetTypeID = $('#cboAssetType').val();
    var vTaxPayerTypeID = $('#hdnTaxPayerTypeID').val();

    var vData = {
        AssetTypeID: vAssetTypeID == '' ? 0 : vAssetTypeID,
        TaxPayerTypeID: vTaxPayerTypeID == '' ? 0 : vTaxPayerTypeID
    };

    jsfn_ajaxPost('/Individual/GetTaxPayerRole_ATTP', vData, jsfn_onAssetTypeChangeResponse);
}

function jsfn_onAssetTypeChangeResponse(data) {
    $("#cboTaxPayerRole").html(""); // clear before appending new list

    $("#cboTaxPayerRole").append(
        $('<option></option>').val("").html("Select Tax Payer Role"));

    $.each(data, function (i, tprol) {
        $("#cboTaxPayerRole").append(
            $('<option></option>').val(tprol.id).html(tprol.text));
    });

    $('#cboTaxPayerRole').val('').select2({ "width": "100%" });
}

function jsfn_BindAssetTable() {
    var vData = {
        AssetTypeID: $("#cboAssetType").val()
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Government/GetAssetList', vData, jsfn_BindAssetTableResponse);
}

function jsfn_BindAssetTableResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#fsStep2').html(data.AssetList);
        $("#artTaxPayerAsset").removeClass("col-lg-6");
        if ($("#cboAssetType").val() == '1') {
            jsfn_BuildBuildingDataTable();
        }
        else if ($("#cboAssetType").val() == '2') {
            jsfn_BuildVehicleDataTable();
        }
        else if ($("#cboAssetType").val() == '3') {
            jsfn_BuildBusinessDataTable();
        }
        else if ($("#cboAssetType").val() == '4') {
            jsfn_BuildLandDataTable();
        }
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BuildBuildingDataTable() {
    vBuildingDataTable = jsfn_ConvertToDatatable($('#tbAsset'));
    //Hide Columns
    vBuildingDataTable.column(2).visible(false);
    vBuildingDataTable.column(3).visible(false);
    vBuildingDataTable.column(5).visible(false);
    vBuildingDataTable.column(6).visible(false);
    vBuildingDataTable.column(8).visible(false);
    vBuildingDataTable.column(10).visible(false);
    vBuildingDataTable.column(11).visible(false);
    vBuildingDataTable.column(12).visible(false);
    vBuildingDataTable.column(13).visible(false);
    vBuildingDataTable.column(14).visible(false);
    vBuildingDataTable.column(15).visible(false);
    vBuildingDataTable.column(16).visible(false);
    vBuildingDataTable.column(17).visible(false);
}

function jsfn_BuildVehicleDataTable() {
    vVehicleDataTable = jsfn_ConvertToDatatable($('#tbAsset'));
    //Hide Columns
    vVehicleDataTable.column(2).visible(false);
    vVehicleDataTable.column(4).visible(false);
    vVehicleDataTable.column(5).visible(false);
    vVehicleDataTable.column(6).visible(false);

}

function jsfn_BuildBusinessDataTable() {
    vBusinessDataTable = jsfn_ConvertToDatatable($('#tbAsset'));
    //Hide Columns
    vBusinessDataTable.column(2).visible(false);
    vBusinessDataTable.column(3).visible(false);
    vBusinessDataTable.column(6).visible(false);
    vBusinessDataTable.column(8).visible(false);
}

function jsfn_BuildLandDataTable() {
    vLandDataTable = jsfn_ConvertToDatatable($('#tbAsset'));
    //Hide Columns
    vLandDataTable.column(4).visible(false);
    vLandDataTable.column(5).visible(false);
    vLandDataTable.column(6).visible(false);
    vLandDataTable.column(7).visible(false);
    vLandDataTable.column(8).visible(false);
    vLandDataTable.column(9).visible(false);
    vLandDataTable.column(10).visible(false);
    vLandDataTable.column(11).visible(false);
}


function jsfn_BindBuildingUnitTable() {
    var vBuildingID = $("#hdnAssetIds").val();
    var vData = {
        BuildingID: vBuildingID
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Individual/GetBuildingUnitList', vData, jsfn_BindBuildingUnitTableResponse);
}

function jsfn_BindBuildingUnitTableResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#fsStep3').html(data.BuildingUnitList);
        jsfn_ConvertToDatatable($('#tbBuildingUnit'));
        $("#fsStep3").show();
        $("#fsStep2").hide();
        $("#dvFooter").hide();
        $("#btnSave").show();
        $("#btnStep3Back").show();
        $("#btnStep2Next").hide();
        $("#btnStep2Back").hide();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_SelectAsset(chkAsset) {
    var vArrAsset = $('#hdnAssetIds').val().split(',');
    if (chkAsset.checked) {
        if ($('#hdnAssetIds').val() != "")
            vArrAsset.push(chkAsset.dataset["assetid"]);
        else {
            vArrAsset = new Array();
            vArrAsset.push(chkAsset.dataset["assetid"]);
        }
    }
    else {
        vArrAsset.splice($.inArray(chkAsset.dataset["assetid"], vArrAsset), 1);
    }

    $('#hdnAssetIds').val(vArrAsset.toString());
}

function jsfn_SelectSingleAsset(rbAsset) {
    $("#hdnAssetIds").val(rbAsset.dataset["assetid"]);
}

function jsfn_SelectSingleBuildingUnit(rbBuildingUnit) {
    $("#hdnBuildingUnitId").val(rbBuildingUnit.dataset["buid"]);
}