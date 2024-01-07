$(document).ready(function () {


    vAssetTable = jsfn_ConvertToDatableWithCustomSort($('#tblAssetList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Assets Found');
    //jsfn_bindTable();

    $("#btnSaveLinkAsset").click(function () {
        var vTaxPayerRoleID = $("#cboTaxPayerRole").val();
        var vTaxPayerRoleName = $("#cboTaxPayerRole option:selected").text();
        var vBuildingUnitID = $("#cboBuildingUnit").val();
        var vRowID = $("#hdnRowID").val();
        var vAssetTypeId = $("#hdnAssetTypeID").val();

        if (vTaxPayerRoleID != '') {
            if (vAssetTypeId == AssetType.Building && vBuildingUnitID == '') {
                $("#dvLAMessage").html('<div class="alert alert-danger">Select Building Unit</div>');
            }
            else {
                var vData = {
                    //TaxPayerTypeID: $("#hdnTaxPayerTypeID").val(),
                    //TaxPayerID: $("#hdnTaxPayerID").val(),
                    AssetTypeID: $("#hdnAssetTypeID").val(),
                    TaxPayerRoleID: vTaxPayerRoleID,
                    TaxPayerRoleName: vTaxPayerRoleName,
                    AssetID: $("#hdnAssetID").val(),
                    BuildingUnitID: vBuildingUnitID,
                    RowID: vRowID
                };
                jsfn_ShowLoading();
                jsfn_ajaxPost('/ConvertTaxPayer/AssignAssetRole', vData, jsfn_LinkAssetResponse);
            }
        }
        else {
            $("#dvLAMessage").html('<div class="alert alert-danger">Select Tax Payer Role and Building Unit</div>');
        }
    });






    var frmGovernment = $('#frmAddGovernment');

    frmGovernment.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {

        },
        messages: {

        },
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent());
            element.parent().addClass("state-error");
        },
        highlight: function (element) { // hightlight error inputs
            $(element).closest('.input').addClass('error'); // set error class to the control group
        },

        unhighlight: function (element) { // revert the change done by hightlight
            $(element)
                .closest('.input').removeClass('error'); // set error class to the control group
        }
    });

    $("#btnStep1next").click(function () {
        if ($('#dvStep1 :input').valid()) {
            jsfn_nextStep2();
        }
        return false;
    });

    $("#btnStep2Next").click(function () {
        if ($('#dvStep2 :input').valid()) {
            jsfn_ajaxPost('/ConvertTaxPayer/DisplayData', null, jsfn_DisplayDataResponse);
        }
        return false;
    });

    $("#btnStep2Prev").click(function () {
        jsfn_prevStep2();
        return false;
    });

    $("#btnStep3Next").click(function () {
        return false;
    });

    $("#btnStep3Prev").click(function () {
        jsfn_prevStep3();
        return false;
    });

})

function jsfn_nextStep2() {
    $("#dvStep1").hide();
    $("#dvStep2").show();
}


function jsfn_prevStep2() {
    $("#dvStep1").show();
    $("#dvStep2").hide();
}

function jsfn_prevStep3() {
    $("#dvStep2").show();
    $("#dvStep3").hide();
}

function jsfn_LinkAssetInformation(astid, asttypeid, rowID) {
    $("#hdnAssetTypeID").val(asttypeid);
    $("#hdnAssetID").val(astid);
    $("#hdnRowID").val(rowID);

    var vData = {
        AssetTypeID: asttypeid,
        TaxPayerTypeID: TaxPayerType.Government,
    };

    jsfn_ajaxPost('/Base/GetTaxPayerRole_ATTP', vData, jsfn_BindTaxPayerRoleResponse);
    $("#dvBuildingUnit").hide();
    if (asttypeid === AssetType.Building) {
        $("#dvBuildingUnit").show();
        jsfn_ajaxPost('/Base/GetBuildingUnitList', { BuildingID: astid }, jsfn_BindBuildingUnitResponse);
    }
}

function jsfn_BindTaxPayerRoleResponse(data) {
    $("#cboTaxPayerRole").html(""); // clear before appending new list

    $("#cboTaxPayerRole").append(
        $('<option></option>').val("").html("Select Tax Payer Role"));

    $.each(data, function (i, tprol) {
        $("#cboTaxPayerRole").append(
            $('<option></option>').val(tprol.id).html(tprol.text));
    });

    $('#cboTaxPayerRole').selectpicker('refresh');
    $("#dvLinkAssetModal").modal('show');
}

function jsfn_BindBuildingUnitResponse(data) {
    $("#cboBuildingUnit").html(""); // clear before appending new list

    $("#cboBuildingUnit").append(
        $('<option></option>').val("").html("Select Building Unit"));

    $.each(data, function (i, bunt) {
        $("#cboBuildingUnit").append(
            $('<option></option>').val(bunt.BuildingUnitID).html(bunt.UnitNumber));
    });

    $('#cboBuildingUnit').selectpicker('refresh');

}

function jsfn_LinkAssetResponse(data) {
    jsfn_HideLoading();
    $("#dvLinkAssetModal").modal('hide');
    if (data.success == true) {
        $("#dvBindData").html(data.AssetData);
    }
    else {
        $("#dvMessage").html(data.Message).addClass("alert").addClass("alert-danger").show();
        setTimeout(function () {
            $("#dvMessage").hide().removeClass("alert-danger").removeClass("alert").html("");
        }, 2000);
    }
}

function jsfn_DisplayDataResponse(data) {
    if (data.success == true) {
        $("#dvStep2").hide();
        $("#dvStep3").show();
        $("#dvBindDetailData").html(data.AssetData);
        $('#txtGender').html($('#GenderID option:selected').text());
        $('#txtTitle').html($('#TitleID option:selected').text());
        $('#txtFirstName').html($('#FirstName').val());
        $('#txtMiddleName').html($('#MiddleName').val());
        $('#txtLastName').html($('#LastName').val());
        $('#txtDOB').html($('#DOB').val());
        $('#txtTIN').html($('#TIN').val());
        $('#txtMobile1').html($('#MobileNumber1').val());
        $('#txtMobile2').html($('#MobileNumber2').val());
        $('#txtEmail1').html($('#EmailAddress1').val());
        $('#txtEmail2').html($('#EmailAddress2').val());
        $('#txtBiometric').html($('#BiometricDetails').val());
        $('#txtTaxOfficeName').html($('#TaxOfficeID option:selected').text());
        $('#txtMaritalStatus').html($('#MaritalStatusID option:selected').text());
        $('#txtNationality').html($('#NationalityID option:selected').text());
        $('#txtEconomicActivities').html($('#EconomicActivitiesID option:selected').text());
        $('#txtNotificationMethod').html($('#NotificationMethodID option:selected').text());
        $('#txtContactAddress').html($('#ContactAddress').val());
        $('#txtPayerType').html("Individual");
    }
    else {
        $("#dvMessage").html(data.dvMessage).addClass("alert").addClass("alert-danger").show();
        setTimeout(function () {
            $("#dvMessage").hide().removeClass("alert-danger").removeClass("alert").html("");
        }, 2000);
    }
}

