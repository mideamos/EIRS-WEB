$(document).ready(function () {
    $("#dvItemDetails").hide();
    var vfrmAddSettlementRevocation = $("#frmAddSettlementRevocation");
    vfrmAddSettlementRevocation.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            cboSettlement: {
                required: true
            },
            cboSettlementItem: {
                required: true
            },
            cboRevenueStream: {
                required: true
            },
            cboRevenueSubStream: {
                required: true
            },
            cboAgency: {
                required: true
            },
            txtReversalAmount: {
                required: true
            },
            txtNotes: {
                required: false
            }

        },
        messages: {
            cboSettlement: {
                required: 'Select Settlement'
            },
            cboSettlementItem: {
                required: 'Select Settlement Item'
            },
            cboRevenueStream: {
                required: 'Select Revenue Stream'
            },
            cboRevenueSubStream: {
                required: 'Select Revenue Sub Stream'
            },
            cboAgency: {
                required: 'Select Agency'
            },
            txtReversalAmount: {
                required: 'Enter Amount'
            },
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
        }
    });


    $("#btnSaveSettlementRevocation").click(function (e) {
        e.preventDefault();
        if (vfrmAddSettlementRevocation.valid()) {
            var vSettledAmount = $("#hdnSettledAmount").val();
            var vReversalAmount = $("#txtReversalAmount").val();

            if (parseFloat(vReversalAmount) <= 0) {
                jsfn_ShowAlert('Reversal amount cannot be less than 0', 'danger', true, '#dvSRAlert');
            }
            else if (parseFloat(vReversalAmount) > parseFloat(vSettledAmount)) {
                jsfn_ShowAlert('Reversal amount cannot be greater than paid amount', 'danger', true, '#dvSRAlert');
            }
            else {
                var vData = {
                    AssessmentID: $("#hdnAssessmentID").val(),
                    SettlementID: $("#cboSettlement").val(),
                    SIID: $("#cboSettlementItem").val(),
                    RevenueStreamID: $("#cboRevenueStream").val(),
                    RevenueSubStreamID: $("#cboRevenueSubStream").val(),
                    AgencyID: $("#cboAgency").val(),
                    ReversalAmount: $("#txtReversalAmount").val(),
                    PoANotes: $("#txtNotes").val(),
                };

                jsfn_ShowLoading();
                jsfn_ajaxPost('/SettlementRevocation/AddABSettlementRevocation', vData, jsfn_PostSettlementRevocationResponse);
            }
        }
    });

    $('#cboSettlement').on("change", jsfn_onSettlementChange);
    $('#cboSettlementItem').on("change", jsfn_onSettlementItemChange);
    $('#cboRevenueStream').on("change", jsfn_onRevenueStreamChange);
});

function jsfn_PostSettlementRevocationResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success', true, '#dvSRAlert');
        setTimeout(() => {
            location.reload(true);
        }, 500);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger', true, '#dvSRAlert');
    }
}

function jsfn_onSettlementChange() {
    var vData = {
        SettlementID: $("#cboSettlement").val()
    };

    jsfn_ajaxPost('/SettlementRevocation/GetSettlementItemList', vData, jsfn_onSettlementChangeResponse);
}

function jsfn_onSettlementChangeResponse(data) {
    $("#cboSettlementItem").html("");

    $("#cboSettlementItem").append(
        $('<option></option>').val("").html("Select Settlement Item"));

    $.each(data, function (i, si) {
        $("#cboSettlementItem").append(
            $('<option></option>').val(si.SIID).html(si.AssessmentItemName));
    });

    $('#cboSettlementItem').val('').selectpicker('refresh');
}

function jsfn_onSettlementItemChange() {
    var vData = {
        SIID: $("#cboSettlementItem").val()
    };

    jsfn_ajaxPost('/SettlementRevocation/GetSettlementItemDetails', vData, jsfn_onSettlementItemChangeResponse);
}

function jsfn_onSettlementItemChangeResponse(data) {
    console.log(data);
    $("#spnAmountCharged").html(data.TotalAmount.formatMoney());
    $("#spnAmountPaid").html(data.SettlementAmount.formatMoney());
    $("#txtReversalAmount").val(data.SettlementAmount);
    $("#hdnSettledAmount").val(data.SettlementAmount);
    $("#dvItemDetails").show();
}

function jsfn_onRevenueStreamChange() {
    var vRevenueStreamID = $('#cboRevenueStream').val();

    if (vRevenueStreamID != '') {

        var vData = {
            RevenueStreamID: vRevenueStreamID
        };

        jsfn_ajaxPost('/SettlementRevocation/GetRevenueSubStream', vData, jsfn_onRevenueStreamChangeResponse);
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

    $('#cboRevenueSubStream').val('').selectpicker('refresh');
}

