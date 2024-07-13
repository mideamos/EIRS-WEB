$(document).ready(function () {
    $("#fsStep2").hide();
    $("#btnBack").hide();
    $("#btnGenerate").hide();

    var frmGenerateAssessment = $('#frmGenerateAssessment');

    // init validator obj and set the rules
    frmGenerateAssessment.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            TaxPayerType: {
                required: true,
            }
            // the rules, as usual
        },

        messages: {
            TaxPayerType: {
                required: 'select tax payer type'
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
            jsfn_BindTaxPayerTable();
        }
    });

    $("#btnBack").click(function () {
        $("#fsStep2").hide();
        $("#fsStep1").show();
        $("#dvFooter").show();
        $("#btnGenerate").hide();
        $("#btnBack").hide();
        $("#artGenerateAssessment").addClass("col-lg-6");
        return false;
    });

    $('#btnGenerate').click(function () {
        if ($("#hdnTaxPayerId").val() != '') {
            window.location.href = '/assessment/add/' + $("#hdnTaxPayerId").val() + '/' + $("#hdnTaxPayerRIN").val() + '?tptype=' + $("#cboTaxPayerType").val();
        }
        else {
            jsfn_ShowAlert('Select Atleast one Tax Payer', 'danger');
        }

        return false;
    });

});

function jsfn_BindTaxPayerTable() {
    var vData = {
        TaxPayerTypeID: $("#cboTaxPayerType").val(),
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Assessment/GetTaxPayerList', vData, jsfn_BindTaxPayerTableResponse);
}

function jsfn_BindTaxPayerTableResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#fsStep2').html(data.TaxPayerList);
        $("#artGenerateAssessment").removeClass("col-lg-6");
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
        $("#btnBack").show();
        $("#btnGenerate").show();
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

function jsfn_SelectSingleTaxPayer(rbTaxPayer) {
    $("#hdnTaxPayerId").val(rbTaxPayer.dataset["taxpayerid"]);
    $("#hdnTaxPayerRIN").val(rbTaxPayer.dataset["taxpayerrin"]);
}