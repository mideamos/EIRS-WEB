$(document).ready(function () {
    jsfn_ConvertToDatableWithCustomSort($('#tblAssetList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Assets Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblProfileList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Profile Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblIncomeStream'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Income Streams Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblPayeIncomeStream'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Income Streams Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblAssessmentRules'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Rules Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblBillList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Assoicated Bill Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblPaymentList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Assoicated Payment Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblTCCDetail'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }], 'No TCC Detail Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblLiabilitySummary'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Records Found');

    $('#cboBusinessID').on("change", jsfn_onTaxPayerRoleChange);

    $("#btnAddNewIncomeStream").on('click', function () {
        console.log("i got here");
        $("#dvIncomeStreamModal").modal('show');
    });

    vfrmAddIncomeStream = $("#frmAddIncomeStream");
    vfrmAddIncomeStream.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            cboTaxYear: {
                required: true
            },
            txtTotalIncomeEarned: {
                required: true
            },
            cboTaxPayerRoleID: {
                required: true
            },
            cboBusinessID: {
                required: true
            }
        },
        messages: {
            cboTaxYear: {
                required: "Select Tax Year"
            },
            txtTotalIncomeEarned: {
                required: "Enter Total Income Earned"
            },
            cboTaxPayerRoleID: {
                required: "Select Tax Payer Role"
            },
            cboBusinessID: {
                required: "Select Asset"
            }
        },
        errorPlacement: function (error, element) {
            if (element.is('select')) {
                error.insertAfter(element[0].parentElement);
            }
            else {
                error.insertAfter(element);
            }
        },
        highlight: function (element) { // hightlight error inputs
            $(element).closest('.input').addClass('error'); // set error class to the control group
        },
        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.input').removeClass('error'); // set error class to the control group
        },
    });

    $("#btSaveIncomeStream").click(function (e) {
        if (vfrmAddIncomeStream.valid()) {
            var vTaxPayerTypeID = $('#hdnTaxPayerTypeID').val();
            var vTaxPayerID = $('#hdnTaxPayerID').val();

            var vData = {
                TaxPayerTypeID: vTaxPayerTypeID,
                TaxPayerID: vTaxPayerID,
                RowID: $("#hdnISRowID").val(),
                TaxYear: $("#cboTaxYear").val(),
                TotalIncomeEarned: $("#txtTotalIncomeEarned").val(),
                TaxPayerRoleID: $("#cboTaxPayerRoleID").val(),
                BusinessID: $("#cboBusinessID").val(),
                Notes: $("#txtNotes").val()
            }

            jsfn_ShowLoading();
            jsfn_ajaxPost('/ProcessTCCRequest/AddUpdateIncomeStream', vData, jsfn_onAddUpdateIncomeStreamResponse);
        }
    });
    //$("#btnApproveValidateAction").click(function (e) {
    //    $("#dvAddBusinesNameModal").modal('show');
    //});

    $("#btSavePayeIncomeStream").click(function (e) {
        if (vfrmAddIncomeStream.valid()) {
            debugger
            var vData = {
                RowID: $("#hdnPayeISRowID").val(),
                TaxYear: $("#cboPayeTaxYear").val(),
                TotalIncomeEarned: $("#txtTotalpayeIncomeEarned").val(),
                ReceiptDate: $("#txtPayeReceiptDate").val(),
                ReceiptReference: $("#txtPayeReceiptRef").val(),
                payeTaxPaid: $("#txtpayeTaxPaid").val(),
                payeAssessedIncome: $("#txtpayeAssessedIncome").val()
            }
            console.log(vData);
            jsfn_ShowLoading();
            jsfn_ajaxPost('/ProcessTCCRequest/AddUpdatePayeIncomeStream', vData, jsfn_onAddUpdatePayeIncomeStreamResponse);
        }
    });

    vfrmAddTCCDetail = $("#frmAddTCCDetail");
    vfrmAddTCCDetail.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            txtTCCTaxPaid: {
                required: true
            },
        },
        messages: {
            txtTCCTaxPaid: {
                required: "Enter TCC Tax Paid"
            }
        },
        errorPlacement: function (error, element) {
            if (element.is('select')) {
                error.insertAfter(element[0].parentElement);
            }
            else {
                error.insertAfter(element);
            }
        },
        highlight: function (element) { // hightlight error inputs
            $(element).closest('.input').addClass('error'); // set error class to the control group
        },
        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.input').removeClass('error'); // set error class to the control group
        },
    });

    $("#btSaveTCCDetail").click(function (e) {
        if (vfrmAddTCCDetail.valid()) {
            var vData = {
                RowID: $("#hdnTCDRowID").val(),
                TCCTaxPaid: $("#txtTCCTaxPaid").val(),
                ERASTaxPaid: $("#txtERASTaxPaid").val(),
            }

            jsfn_ShowLoading();
            jsfn_ajaxPost('/ProcessTCCRequest/UpdateTCCDetail', vData, jsfn_onUpdateTCCDetailResponse);
        }
    });
});

function jsfn_onAddUpdatePayeIncomeStreamResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $("#dvPayeIncomeStreamData").html(data.PayeIncomeStreamData);
        jsfn_ConvertToDatableWithCustomSort($('#tblPayeIncomeStream'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Income Streams Found');

        $("#dvTCCDetailData").html(data.TCCDetailData);
        jsfn_ConvertToDatableWithCustomSort($('#tblTCCDetail'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true },{ "orderable": true },{ "orderable": true }], 'No TCC Detail Found');

        $("#hdnISRowID").val('0')
        $("#cboPayeTaxYear").val('').selectpicker('refresh');
        $("#txtTotalpayeIncomeEarned").val('');
        $("#txtPayeReceiptDate").val('');
        $("#txtPayeReceiptRef").val('');
        $("#txtpayeTaxPaid").val('');
        $("#txtpayeAssessedIncome").val('');
        $("#dvPAYEIncomeStreamModal").modal('hide');

        $("#btSavePayeIncomeStream").html('Save');
        $("#dvPAYEIncomeStreamModalLabel").html('Add Paye Income Stream');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger', true, '#dvAISMessage');
    }
}
function jsfn_onAddUpdateIncomeStreamResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $("#dvIncomeStreamData").html(data.IncomeStreamData);
        jsfn_ConvertToDatableWithCustomSort($('#tblIncomeStream'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Income Streams Found');

        $("#dvTCCDetailData").html(data.TCCDetailData);
        jsfn_ConvertToDatableWithCustomSort($('#tblTCCDetail'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true },{ "orderable": true },{ "orderable": true }], 'No TCC Detail Found');

        $("#hdnISRowID").val('0')
        $("#cboTaxYear").val('').selectpicker('refresh');
        $("#txtTotalIncomeEarned").val('');
        $("#cboTaxPayerRoleID").val('').selectpicker('refresh');
        $("#cboBusinessID").val('').selectpicker('refresh');
        $("#txtNotes").val('');
        $("#dvIncomeStreamModal").modal('hide');

        $("#btSaveIncomeStream").html('Save');
        $("#dvIncomeStreamModalLabel").html('Add Income Stream');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger', true, '#dvAISMessage');
    }
}

function jsfn_onUpdateTCCDetailResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $("#dvTCCDetailData").html(data.TCCDetailData);
        jsfn_ConvertToDatableWithCustomSort($('#tblTCCDetail'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true },  { "orderable": true },  { "orderable": true }], 'No TCC Detail Found');

        $("#hdnTCDRowID").val('0')
        $("#txtTaxYear").val('');
        $("#txtAssessableIncome").val('');
        $("#txtTCCTaxPaid").val('');
        $("#txtERASAssessed").val('');
        $("#txtERASTaxPaid").val('');
        $("#dvTCCDetailModal").modal('hide');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger', true, '#dvTCDMessage');
    }
}

function jsfn_DeleteIncomeStream(RowID) {
    var vData = {
        RowID: RowID === '' ? 0 : RowID
    };

    jsfn_ajaxPost('/ProcessTCCRequest/DeleteIncomeStream', vData, jsfn_DeleteIncomeStreamResponse);
}

function jsfn_DeleteIncomeStreamResponse(data) {
    if (data.success === true) {
        $("#dvIncomeStreamData").html(data.IncomeStreamData);
        jsfn_ConvertToDatableWithCustomSort($('#tblIncomeStream'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Income Streams Found');

        $("#dvTCCDetailData").html(data.TCCDetailData);
        jsfn_ConvertToDatableWithCustomSort($('#tblTCCDetail'), [{ "orderable": true }, { "orderable": true },{ "orderable": true }, { "orderable": true }, { "orderable": true },{ "orderable": true }, { "orderable": true }], 'No TCC Detail Found');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
function jsfn_DeletePayeIncomeStream(RowID) {
    var vData = {
        RowID: RowID === '' ? 0 : RowID
    };

    jsfn_ajaxPost('/ProcessTCCRequest/DeletePayeIncomeStream', vData, jsfn_DeletePayeIncomeStreamResponse);
}

function jsfn_DeletePayeIncomeStreamResponse(data) {
    if (data.success === true) {
        $("#dvPayeIncomeStreamData").html(data.PayeIncomeStreamData);
        jsfn_ConvertToDatableWithCustomSort($('#tblPayeIncomeStream'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Income Streams Found');

       
        $("#dvTCCDetailData").html(data.TCCDetailData);
        jsfn_ConvertToDatableWithCustomSort($('#tblTCCDetail'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true },{ "orderable": true }, { "orderable": true }, { "orderable": true }], 'No TCC Detail Found');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_AssessmentRuleDetails(aruleID) {
    var vData = {
        AssessmentRuleID: aruleID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetAssessmentRuleDetails', vData, jsfn_ShowAssessmentRuleDetailResponse);
}

function jsfn_ShowAssessmentRuleDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentRuleModal').modal('show');
        $('#dvAssessmentRuleRefNo').html(data.AssessmentRuleDetails.AssessmentRuleCode);
        $('#dvAssessmentRuleName').html(data.AssessmentRuleDetails.AssessmentRuleName);
        $('#dvRuleRun').html(data.AssessmentRuleDetails.RuleRunName);
        $('#dvFrequency').html(data.AssessmentRuleDetails.PaymentFrequencyName);
        $('#dvTaxYear').html(data.AssessmentRuleDetails.TaxYear);
        $('#dvSettlementMethod').html(data.AssessmentRuleDetails.SettlementMethodNames);
        $('#dvPaymentOption').html(data.AssessmentRuleDetails.PaymentOptionName);
        $('#dvStatus').html(data.AssessmentRuleDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_AssessmentRuleItemList(aruleID) {
    var vData = {
        AssessmentRuleID: aruleID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetAssessmentRuleItemList', vData, jsfn_AssessmentRuleItemResponse);
}

function jsfn_AssessmentRuleItemResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentRuleItemModal').modal('show');
        $('#dvAssessmentRuleItemData').html(data.AssessmentRuleItemList);
        jsfn_ConvertToDatableWithCustomSort($('#tblAssessmentRuleItem'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Assessment Items Found');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ProfileDetails(prfID) {
    var vData = {
        ProfileID: prfID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetProfileDetails', vData, jsfn_ShowProfileDetailResponse);
}

function jsfn_ShowProfileDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvProfileModal').modal('show');
        $('#dvProfileRefNo').html(data.ProfileDetails.ProfileReferenceNo);
        $('#dvAssetType').html(data.ProfileDetails.AssetTypeName);
        $('#dvTaxPayerType').html(data.ProfileDetails.TaxPayerTypeNames);
        $('#dvTaxPayerRole').html(data.ProfileDetails.TaxPayerRoleNames);
        $('#dvProfileDescription').html(data.ProfileDetails.ProfileDescription);
        $('#dvAssetStatus').html(data.ProfileDetails.AssetTypeStatusName);
        $('#dvProfileStatus').html(data.ProfileDetails.ActiveText);
        $('#dvProfileSector').html(data.ProfileDetails.ProfileSectorNames);
        $('#dvProfileSubSector').html(data.ProfileDetails.ProfileSubSectorNames);
        $('#dvProfileGroup').html(data.ProfileDetails.ProfileGroupNames);
        $('#dvProfileSubGroup').html(data.ProfileDetails.ProfileSubGroupNames);
        $('#dvSectorElement').html(data.ProfileDetails.ProfileSectorElementNames);
        $('#dvSectorSubElement').html(data.ProfileDetails.ProfileSectorSubElementNames);
        $('#dvProfileAttribute').html(data.ProfileDetails.ProfileAttributeNames);
        $('#dvProfileSubAttribute').html(data.ProfileDetails.ProfileSubAttributeNames);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_onTaxPayerRoleChange() {
    var vTaxPayerTypeID = $('#hdnTaxPayerTypeID').val();
    var vTaxPayerID = $('#hdnTaxPayerID').val();
    var vTaxPayerRoleID = $('#cboBusinessID').val();

    var vData = {
        tpid: vTaxPayerID === '' ? 0 : vTaxPayerID,
        tptid: vTaxPayerTypeID === '' ? 0 : vTaxPayerTypeID,
        tprolid: vTaxPayerRoleID === '' ? 0 : vTaxPayerRoleID
    };

    jsfn_ajaxPost('/ProcessTCCRequest/GetAssetDropDownList', vData, jsfn_onTaxPayerRoleChangeResponse);
}

function jsfn_onTaxPayerRoleChangeResponse(data) {
    $("#cboTaxPayerRoleID").html("");

    $("#cboTaxPayerRoleID").append(
        $('<option></option>').val("").html("Select Tax Payer Role"));

    $.each(data, function (i, ast) {
        $("#cboTaxPayerRoleID").append(
            $('<option></option>').val(ast.id).html(ast.text));
    });

    $('#cboTaxPayerRoleID').selectpicker('refresh');
}

function jsfn_EditIncomeStream(RowID) {
    debugger;
    console.log(RowID)
    var vTaxPayerTypeID = $('#hdnTaxPayerTypeID').val();
    var vTaxPayerID = $('#hdnTaxPayerID').val();

    var vData = {
        RowID: RowID === '' ? 0 : RowID,
        tpid: vTaxPayerID === '' ? 0 : vTaxPayerID,
        tptid: vTaxPayerTypeID === '' ? 0 : vTaxPayerTypeID,
    };

    jsfn_ajaxPost('/ProcessTCCRequest/GetIncomeStreamDetails', vData, jsfn_EditIncomeStreamResponse);
}

function jsfn_EditIncomeStreamResponse(data) {
    if (data.success === true) {

        $("#cboBusinessID").html("");

        $("#cboBusinessID").append(
            $('<option></option>').val("").html("Select Asset"));

        $.each(data.AssetList, function (i, ast) {
            $("#cboBusinessID").append(
                $('<option></option>').val(ast.id).html(ast.text));
        });


        $("#hdnISRowID").val(data.IncomeStreamData.RowID)
        $("#cboTaxYear").val(data.IncomeStreamData.TaxYear).selectpicker('refresh');
        $("#txtTotalIncomeEarned").val(data.IncomeStreamData.TotalIncomeEarned);
        $("#cboTaxPayerRoleID").val(data.IncomeStreamData.TaxPayerRoleID).selectpicker('refresh');
        $("#cboBusinessID").val(data.IncomeStreamData.BusinessID).selectpicker('refresh');
        $("#txtNotes").val(data.IncomeStreamData.Notes);

        $("#btSaveIncomeStream").html('Update');
        $("#dvIncomeStreamModalLabel").html('Edit Income Stream');

        $("#dvIncomeStreamModal").modal('show');

    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
function jsfn_EditPAYEIncomeStream(RowID) {

    var vData = {
        RowID: RowID === '' ? 0 : RowID
    };

    jsfn_ajaxPost('/ProcessTCCRequest/GetPAYEIncomeStreamDetails', vData, jsfn_EditPAYEIncomeStreamResponse);
}

function jsfn_EditPAYEIncomeStreamResponse(data) {
    if (data.success === true) {
        console.log(data);
        $("#hdnPayeISRowID").val(data.IncomeStreamData.RowID)
        $("#cboPayeTaxYear").val(data.IncomeStreamData.TaxYear);
        $("#txtTotalpayeIncomeEarned").val(data.IncomeStreamData.ChargeableIncome);
        $("#txtPayeReceiptRef").val(data.IncomeStreamData.ReceiptRef);
        $("#txtPayeReceiptDate").val(data.IncomeStreamData.ReceiptDate);
        $("#txtpayeAssessedIncome").val(data.IncomeStreamData.AnnualTax);
        $("#txtpayeTaxPaid").val(data.IncomeStreamData.AnnualTax);
        $("#btSavePayeIncomeStream").html('Update');
        $("#dvPAYEIncomeStreamModalLabel").html('Edit Paye Income Stream');

        $("#dvPAYEIncomeStreamModal").modal('show');

    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_EditTCCDetail(RowID) {
    var vData = {
        RowID: RowID === '' ? 0 : RowID,
    };

    jsfn_ajaxPost('/ProcessTCCRequest/GetTCCDetail', vData, jsfn_EditTCCDetailResponse);
}

function jsfn_EditTCCDetailResponse(data) {
    if (data.success === true) {
        $("#hdnTCDRowID").val(data.TCCDetailData.RowID)
        $("#txtTaxYear").val(data.TCCDetailData.TaxYear);
        $("#txtAssessableIncome").val(data.TCCDetailData.AssessableIncome);
        $("#txtTCCTaxPaid").val(data.TCCDetailData.TCCTaxPaid);
        $("#txtERASAssessed").val(data.TCCDetailData.ERASAssessed);
        $("#txtERASTaxPaid").val(data.TCCDetailData.ERASTaxPaid);

        if (data.TCCDetailData.TCCTaxPaid === 0) {
            $("#txtTCCTaxPaid").val(data.TCCDetailData.ERASTaxPaid);
        }

        $("#dvTCCDetailModal").modal('show');

    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}