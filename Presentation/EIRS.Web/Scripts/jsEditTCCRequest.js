var vfrmAddIncomeStream;
$(document).ready(function () {
    jsfn_ConvertToDatableWithCustomSort($('#tblIncomeStream'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Income Streams Found');

    $('#cboBusinessTypeID').on("change", jsfn_onBusinessTypeChange);

    $("#btnAddNewIncomeStream").on('click', function () {
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
            txtBusinessName: {
                required: true
            },
            cboBusinessTypeID: {
                required: true
            },
            cboLGAID: {
                required: true
            },
            cboBusinessOperationID: {
                required: true
            },
            txtContactName: {
                required: true
            },
            txtBusinessAddress: {
                required: true
            },
            txtBusinessNumber: {
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
            txtBusinessName: {
                required: "Enter Business Name"
            },
            cboBusinessTypeID: {
                required: "Select Business Type"
            },
            cboLGAID: {
                required: "Select LGA"
            },
            cboBusinessOperationID: {
                required: "Select Business Operation"
            },
            txtContactName: {
                required: "Enter Contact Name"
            },
            txtBusinessAddress: {
                required: "Enter Business Address"
            },
            txtBusinessNumber: {
                required: "Enter Business Number"
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
      /*  if (vfrmAddIncomeStream.valid()) {*/
            var vData = {
                TaxYear: $("#cboTaxYear").val(),
                TotalIncomeEarned: $("#txtTotalIncomeEarned").val(),
                TaxPayerRoleID: $("#cboTaxPayerRoleID").val(),
                BusinessName: $("#txtBusinessName").val(),
                BusinessTypeID: $("#cboBusinessTypeID").val(),
                LGAID: $("#cboLGAID").val(),
                BusinessOperationID: $("#cboBusinessOperationID").val(),
                ContactName: $("#txtContactName").val(),
                BusinessAddress: $("#txtBusinessAddress").val(),
                BusinessNumber: $("#txtBusinessNumber").val()
            }

            jsfn_ShowLoading();
            jsfn_ajaxPost('/ProcessTCCRequest/AddIncomeStream', vData, jsfn_onAddIncomeStreamResponse);
       /* }*/
    });


});

function jsfn_onAddIncomeStreamResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $("#dvIncomeStreamData").html(data.IncomeStreamData);
        jsfn_ConvertToDatableWithCustomSort($('#tblIncomeStream'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Income Streams Found');

        $("#cboTaxYear").val('').selectpicker('refresh');
        $("#txtTotalIncomeEarned").val('');
        $("#cboTaxPayerRoleID").val('').selectpicker('refresh');
        $("#txtBusinessName").val('');
        $("#cboBusinessTypeID").val('').selectpicker('refresh');
        $("#LGAID").val('').selectpicker('refresh');
        $("#cboBusinessOperationID").val('').selectpicker('refresh');
        $("#txtContactName").val('');
        $("#txtBusinessAddress").val('');
        $("#txtBusinessNumber").val('');
        $("#dvIncomeStreamModal").modal('hide');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger', true, '#dvAISMessage');
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
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_onBusinessTypeChange() {
    var vBusinessTypeID = $('#cboBusinessTypeID').val();

    var vData = {
        BusinessTypeID: vBusinessTypeID === '' ? 0 : vBusinessTypeID
    };

    jsfn_ajaxPost('/Base/BusinessTypeChange', vData, jsfn_onBusinessTypeChangeResponse);
}

function jsfn_onBusinessTypeChangeResponse(data) {
    $("#cboBusinessOperationID").html("");

    $("#cboBusinessOperationID").append(
        $('<option></option>').val("").html("Select Business Operation"));

    $.each(data.BusinessOperation, function (i, bcat) {
        $("#cboBusinessOperationID").append(
            $('<option></option>').val(bcat.id).html(bcat.text));
    });

    $('#cboBusinessOperationID').selectpicker('refresh');
}

