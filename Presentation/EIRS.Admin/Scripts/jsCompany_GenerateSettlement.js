$(document).ready(function () {
    $("#fsStep2").hide();
    $("#btnBack").hide();
    $("#btnGenerate").hide();

    var frmGenerateSettlement = $('#frmGenerateSettlement');

    // init validator obj and set the rules
    frmGenerateSettlement.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            SettlementType: {
                required: true,
            }
            // the rules, as usual
        },

        messages: {
            SettlementType: {
                required: 'select settlement type'
            }
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

    $("#btnNext").on('click', function () {
        if ($('#fsStep1 :input').valid()) {
            if ($("#cboSettlementType").val() == '1') {
                jsfn_BindAssessmentTable();
            }
            else if ($("#cboSettlementType").val() == '2') {
                jsfn_BindServiceBillTable();
            }
        }
    });

    $("#btnBack").click(function () {
        $("#fsStep2").hide();
        $("#fsStep1").show();
        $("#dvFooter").show();
        $("#btnGenerate").hide();
        $("#btnBack").hide();
        $("#artGenerateSettlement").addClass("col-lg-6");
        return false;
    });

    $('#btnGenerate').click(function () {
        if ($("#hdnASID").val() != '') {
            window.location.href = '/Company/AddSettlement/' + $("#hdnTaxPayerID").val() + '/' + $("#hdnTaxPayerRIN").val() + '?stype=' + $("#cboSettlementType").val() + "&asid=" + $("#hdnASID").val();
        }
        else {
            jsfn_ShowAlert('Select Atleast One Assessment or Service Bill', 'danger');
        }

        return false;
    });

});

function jsfn_SelectSingleASB(rbASB) {
    $("#hdnASID").val(rbASB.dataset["asbid"]);
    $("#hdnASRefNo").val(rbASB.dataset["asbref"]);
}

function jsfn_BindAssessmentTable() {
    var vData = {
        TaxPayerID: $("#hdnTaxPayerID").val(),
    };
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Company/GetAssessmentList', vData, jsfn_BindAssessmentTableResponse);
}

function jsfn_BindAssessmentTableResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#fsStep2').html(data.AssessmentList);
        $("#artGenerateSettlement").removeClass("col-lg-6");
        vAssessmentDataTable = jsfn_ConvertToDatatable($('#tbAssessment'));
        $("#fsStep2").show();
        $("#fsStep1").hide();
        $("#dvFooter").hide();
        $("#btnBack").show();
        $("#btnGenerate").show();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BindServiceBillTable() {
    var vData = {
        TaxPayerID: $("#hdnTaxPayerID").val(),
    };
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Company/GetServiceBillList', vData, jsfn_BindServiceBillTableResponse);
}

function jsfn_BindServiceBillTableResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#fsStep2').html(data.ServiceBillList);
        $("#artGenerateSettlement").removeClass("col-lg-6");
        vServiceBillDataTable = jsfn_ConvertToDatatable($('#tbServiceBill'));
        $("#fsStep2").show();
        $("#fsStep1").hide();
        $("#dvFooter").hide();
        $("#btnBack").show();
        $("#btnGenerate").show();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}