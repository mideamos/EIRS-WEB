$(document).ready(function () {
    $("#dvItemDetails").hide();
    var vfrmAddAdjustment = $("#frmAddAdjustment");
    vfrmAddAdjustment.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            cboAssessmentItem: {
                required: true
            },
            txtAdjustmentLine: {
                required: true
            },
            cboAdjustmentType: {
                required: true
            },
            txtAdjustmentAmount: {
                required: true
            }

        },
        messages: {
            cboAssessmentItem: {
                required: "Select Assessment Item"
            },
            txtAdjustmentLine: {
                required: "Enter Adjustment Line"
            },
            cboAdjustmentType: {
                required: "Select Adjustment Type"
            },
            txtAdjustmentAmount: {
                required: "Enter Adjustment Amount"
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
        }
    });
    var vfrmAddAdjustmentII = $("#frmAddAdjustmentII");
    vfrmAddAdjustmentII.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            cboAssessmentItem: {
                required: true
            },
            txtAdjustmentLine: {
                required: true
            },
            cboAdjustmentType: {
                required: true
            },
            txtAdjustmentAmount: {
                required: true
            }

        },
        messages: {
            cboAssessmentItem: {
                required: "Select Assessment Item"
            },
            txtAdjustmentLine: {
                required: "Enter Adjustment Line"
            },
            cboAdjustmentType: {
                required: "Select Adjustment Type"
            },
            txtAdjustmentAmount: {
                required: "Enter Adjustment Amount"
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
        }
    });

    var vfrmPoA = $("#frmPoA");
    vfrmPoA.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            txtTransactionRefNo: {
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
            txtAmount: {
                required: true
            },
            cboPaymentMethod: {
                required: true
            },
            txtNotes: {
                required: false
            }

        },
        messages: {
            txtTransactionRefNo: {
                required: 'Enter Transaction Ref No'
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
            txtAmount: {
                required: 'Enter Amount'
            },
            cboPaymentMethod: {
                required: 'Select Payment Method'
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
        }
    });

    $("#btnSaveAdjustment").click(function (e) {

        e.preventDefault();
        if (vfrmAddAdjustment.valid()) {
            var vItemAmountCharged = $("#hdnItemAmountCharged").val();
            var vSettlementAmount = $("#hdnItemAmountPaid").val();
            var vAdjustmentAmount = $("#txtAdjustmentAmount").val();
            var vAdjustmentType = $("#cboAdjustmentType").val();
            var vId = $("#cboAssessmentItem").val();
            if (vAdjustmentType === '2') {
                vAdjustmentAmount = 0 - parseFloat(vAdjustmentAmount);
                if (parseFloat(vItemAmountCharged) + parseFloat(vAdjustmentAmount) <= 0) {
                    jsfn_ShowAlert('Asssement Item Amount cannot be zero', 'danger', true);
                }
                else if (parseFloat(vSettlementAmount) > parseFloat(vItemAmountCharged) + parseFloat(vAdjustmentAmount)) {
                    //Show PoA Popup
                    $("#dvPoAModal").modal('show');
                    $("#txtAmount").val(parseFloat(vSettlementAmount) - (parseFloat(vItemAmountCharged) + parseFloat(vAdjustmentAmount)));
                }
                else {
                    jsfn_PostAdjustment();
                }
            }
            else {
                jsfn_PostAdjustment();
            }
        }
    });
    $("#btnSaveAdjustmentII").click(function (e) {

        e.preventDefault();
        if (vfrmAddAdjustmentII.valid()) {
            var vItemAmountCharged = $("#hdnItemAmountCharged").val();
            var vSettlementAmount = $("#hdnItemAmountPaid").val();
            var vAdjustmentAmount = $("#txtAdjustmentAmount").val();
            var vAdjustmentNote = $("#txtAdjustmentNote").val();
            var vAdjustmentType = $("#cboAdjustmentType").val();
            var totalPen = $("#Value1").val();
            var totalInt = $("#Value1I").val();
            var totalTotal = $("#Value1II").val();
            var vId = $("#cboAssessmentItem").val();
            if (vAdjustmentNote === null && vAdjustmentNote.trim() === '') {
                jsfn_ShowAlert('Note is required', 'danger', true);

            }
            if (vAdjustmentType === '2') {
                totalInt = 0 - parseFloat(totalInt);
                totalPen = 0 - parseFloat(totalPen);
                totalTotal = 0 - parseFloat(totalTotal);
            }
            var vData = {
                AssessmentID: $("#hdnAssessmentID").val(),
                AAIID: $("#cboAssessmentItem").val(),
                AdjustmentLine: $("#txtAdjustmentLine").val(),
                AdjustmentTypeID: $("#cboAdjustmentType").val(),
                Penalty : totalPen,
                Interest: totalInt,
                TotalAmount: totalTotal,
            };
            console.log(vData);
            jsfn_ShowLoading();
            jsfn_ajaxPost('/Adjustment/AddABAdjustmentII', vData, jsfn_PostAdjustmentResponse);
        }
    });

    $("#btnSaveAdjustmentWithPoA").click(function (e) {
        e.preventDefault();
        var vAdjustmentAmount = $("#txtAdjustmentAmount").val();
        if (vfrmPoA.valid()) {
            var vData = {
                AssessmentID: $("#hdnAssessmentID").val(),
                AAIID: $("#cboAssessmentItem").val(),
                AdjustmentLine: $("#txtAdjustmentLine").val(),
                AdjustmentTypeID: $("#cboAdjustmentType").val(),
                AdjustmentAmount: $("#cboAdjustmentType").val() === '2' ? 0 - parseFloat(vAdjustmentAmount) : parseFloat(vAdjustmentAmount),
                TransactionRefNo: $("#txtTransactionRefNo").val(),
                RevenueStreamID: $("#cboRevenueStream").val(),
                RevenueSubStreamID: $("#cboRevenueSubStream").val(),
                AgencyID: $("#cboAgency").val(),
                PoAAmount: $("#txtAmount").val(),
                PaymentMethodID: $("#cboPaymentMethod").val(),
                PoANotes: $("#txtNotes").val(),
            };

            jsfn_ShowLoading();
            jsfn_ajaxPost('/Adjustment/AddABAdjustmentWithPoA', vData, jsfn_PostAdjustmentResponse);
        }
    });

    $('#cboAssessmentItem').on("change", jsfn_onAssessmentItemChange);
    $('#cboRevenueStream').on("change", jsfn_onRevenueStreamChange);
});

function jsfn_PostAdjustmentII() {
    var vAdjustmentAmount = $("#txtAdjustmentAmount").val();
    var vData = {
        AssessmentID: $("#hdnAssessmentID").val(),
        AAIID: $("#cboAssessmentItem").val(),
        AdjustmentLine: $("#txtAdjustmentLine").val(),
        AdjustmentTypeID: $("#cboAdjustmentType").val(),
        Amount: $("#cboAdjustmentType").val() === '2' ? 0 - parseFloat(vAdjustmentAmount) : parseFloat(vAdjustmentAmount),
    };
    jsfn_ShowLoading();
    //jsfn_ajaxPost('/Adjustment/Temitayo', vData, jsfn_PostAdjustmentResponse);
    jsfn_ajaxPost('/Adjustment/AddABAdjustment', vData, jsfn_PostAdjustmentResponse);
}
function jsfn_PostAdjustment() {
    var vAdjustmentAmount = $("#txtAdjustmentAmount").val();
    var vData = {
        AssessmentID: $("#hdnAssessmentID").val(),
        AAIID: $("#cboAssessmentItem").val(),
        AdjustmentLine: $("#txtAdjustmentLine").val(),
        AdjustmentTypeID: $("#cboAdjustmentType").val(),
        Amount: $("#cboAdjustmentType").val() === '2' ? 0 - parseFloat(vAdjustmentAmount) : parseFloat(vAdjustmentAmount),
    };
    jsfn_ShowLoading();
    //jsfn_ajaxPost('/Adjustment/Temitayo', vData, jsfn_PostAdjustmentResponse);
    jsfn_ajaxPost('/Adjustment/AddABAdjustment', vData, jsfn_PostAdjustmentResponse);
}

function jsfn_PostAdjustmentResponse(data) {
    if (data.success) {
        location.reload(true);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger', true);
    }
}

function jsfn_onAssessmentItemChange() {
    var vData = {
        AAIID: $("#cboAssessmentItem").val(),
    };

    jsfn_ajaxPost('/Adjustment/GetAssessmentItemDetails', vData, jsfn_onAssessmentItemChangeResponse);
}

function jsfn_onAssessmentItemChangeResponse(data) {
    $("#spnAmountCharged").html(data.TotalAmount.formatMoney());
    $("#spnAmountPaid").html(data.SettlementAmount.formatMoney());
    $("#hdnItemAmountCharged").val(data.TotalAmount);
    $("#hdnItemAmountPaid").val(data.SettlementAmount);
    $("#dvItemDetails").show();
}

function jsfn_onRevenueStreamChange() {
    var vRevenueStreamID = $('#cboRevenueStream').val();

    if (vRevenueStreamID != '') {

        var vData = {
            RevenueStreamID: vRevenueStreamID
        };

        jsfn_ajaxPost('/Adjustment/GetRevenueSubStream', vData, jsfn_onRevenueStreamChangeResponse);
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
