$(document).ready(function () {
    $("#fsStep2").hide();
    $("#btnSave").hide();
    $("#btnBack").hide();

    var frmTaxPayer = $('#frmTaxPayer');

    // init validator obj and set the rules
    frmTaxPayer.validate({
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
            jsfn_BindTaxPayerTable();
        }
    });

    $("#btnBack").click(function () {
        $("#fsStep2").hide();
        $("#fsStep1").show();
        $("#dvFooter").show();
        $("#btnSave").hide();
        $("#btnBack").hide();
        $("#artBusinessTaxPayer").addClass("col-lg-6");
        return false;
    });

    $('#btnSave').click(function () {
        if ($("#hdnTaxPayerIds").val() != '') {
            $("#frmTaxPayer").submit();
        }
        else {
            jsfn_ShowAlert('Select Atleast one Tax Payer', 'danger');
        }

        return false;
    });

    $('#cboTaxPayerType').on("change", jsfn_onTaxPayerTypeChange);
});

function jsfn_onTaxPayerTypeChange() {
    var vAssetTypeID = $('#hdnAssetType').val();
    var vTaxPayerTypeID = $('#cboTaxPayerType').val();

    var vData = {
        AssetTypeID: vAssetTypeID == '' ? 0 : vAssetTypeID,
        TaxPayerTypeID: vTaxPayerTypeID == '' ? 0 : vTaxPayerTypeID
    };

    jsfn_ajaxPost('/Business/GetTaxPayerRole_ATTP', vData, jsfn_onTaxPayerTypeChangeResponse);
}

function jsfn_onTaxPayerTypeChangeResponse(data) {
    $("#cboTaxPayerRole").html(""); // clear before appending new list

    $("#cboTaxPayerRole").append(
        $('<option></option>').val("").html("Select Tax Payer Role"));

    $.each(data, function (i, tprol) {
        $("#cboTaxPayerRole").append(
            $('<option></option>').val(tprol.id).html(tprol.text));
    });

    $('#cboTaxPayerRole').val('').select2({ "width": "100%" });
}

function jsfn_BindTaxPayerTable() {
    var vData = {
        TaxPayerTypeID: $("#cboTaxPayerType").val(),
        AssetID: $("#hdnAssetID").val(),
        TaxPayerRoleID: $("#cboTaxPayerRole").val()
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Business/GetTaxPayerList', vData, jsfn_BindTaxPayerTableResponse);
}
function jsfn_BindTaxPayerTableResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#fsStep2').html(data.TaxPayerList);
        $("#artBuildingTaxPayer").removeClass("col-lg-6");
        if ($("#cboTaxPayerType").val() == '1') {
            jsfn_BuildIndividualDataTable();
        }
        else if ($("#cboTaxPayerType").val() == '2') {
            jsfn_BuildCompanyDataTable();
        }
        else if ($("#cboTaxPayerType").val() == '4') {
            jsfn_BuildGovernmentDataTable();
        }
        else if ($("#cboTaxPayerType").val() == '3') {
            jsfn_BuildSpecialDataTable();
        }

        $("#fsStep2").show();
        $("#fsStep1").hide();
        $("#dvFooter").hide();
        $("#btnSave").show();
        $("#btnBack").show();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BuildIndividualDataTable() {
    vIndividualDataTable = jsfn_ConvertToDatatable($('#tbTaxPayer'));
    //Hide Columns
    vIndividualDataTable.column(1).visible(false);
    vIndividualDataTable.column(2).visible(false);
    vIndividualDataTable.column(5).visible(false);
    vIndividualDataTable.column(6).visible(false);
    vIndividualDataTable.column(7).visible(false);
    vIndividualDataTable.column(9).visible(false);
    vIndividualDataTable.column(11).visible(false);
    vIndividualDataTable.column(12).visible(false);
    vIndividualDataTable.column(14).visible(false);
    vIndividualDataTable.column(15).visible(false);
    vIndividualDataTable.column(16).visible(false);
    vIndividualDataTable.column(17).visible(false);
}

function jsfn_BuildCompanyDataTable() {
    vCompanyDataTable = jsfn_ConvertToDatatable($('#tbTaxPayer'));
    //Hide Columns
    vCompanyDataTable.column(4).visible(false);
    vCompanyDataTable.column(6).visible(false);
    vCompanyDataTable.column(8).visible(false);
}

function jsfn_BuildGovernmentDataTable() {
    vGovernmentDataTable = jsfn_ConvertToDatatable($('#tbTaxPayer'));
    //Hide Columns
    //vGovernmentDataTable.column(4).visible(false);
    //vGovernmentDataTable.column(6).visible(false);
    //vGovernmentDataTable.column(8).visible(false);
}

function jsfn_BuildSpecialDataTable() {
    vSpecialDataTable = jsfn_ConvertToDatatable($('#tbTaxPayer'));
    //Hide Columns
    //vSpecialDataTable.column(4).visible(false);
    //vSpecialDataTable.column(6).visible(false);
    //vSpecialDataTable.column(8).visible(false);
}

function jsfn_SelectMultiTaxPayer(chkTaxPayer) {
    var vArrTaxPayer = $('#hdnTaxPayerIds').val().split(',');
    if (chkTaxPayer.checked) {
        if ($('#hdnTaxPayerIds').val() != "")
            vArrTaxPayer.push(chkTaxPayer.dataset["taxpayerid"]);
        else {
            vArrTaxPayer = new Array();
            vArrTaxPayer.push(chkTaxPayer.dataset["taxpayerid"]);
        }
    }
    else {
        vArrTaxPayer.splice($.inArray(chkTaxPayer.dataset["taxpayerid"], vArrTaxPayer), 1);
    }

    $('#hdnTaxPayerIds').val(vArrTaxPayer.toString());
}

function jsfn_SelectSingleTaxPayer(rbTaxPayer) {
    $("#hdnTaxPayerIds").val(rbTaxPayer.dataset["taxpayerid"]);
}