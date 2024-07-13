$(document).ready(function () {
    $("#fsStep2").hide();
    $("#hdStep2").hide();
    $("#btnSave").hide();
    $("#btnBack").hide();

    $("#dvMDAServiceList").hide();
    $("#frmMSAddMDAService").hide();

    $('#dvMDAServiceDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>MDA Service Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#dvMDAServiceItemDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>MDA Service Item Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#dvEditServiceAmount').dialog({
        autoOpen: false,
        width: 500,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Edit Service Base Amount</h4></div>",
        buttons: [{
            html: "<i class='fa fa-check'></i>&nbsp; Update",
            "class": "btn btn-primary",
            click: function () {
                jsfn_UpdateServiceAmount();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    var frmServiceBillItem = $('#frmServiceBillItem');

    // init validator obj and set the rules
    frmServiceBillItem.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            TaxBaseAmount: {
                required: true,
                number: true
            }
        },

        messages: {
            TaxBaseAmount: {
                required: 'Please enter service base amount',
                number: 'Please enter proper service base amount'
            }
        },

        // Do not change code below
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

    var frmServiceBill = $('#frmServiceBill');

    // init validator obj and set the rules
    frmServiceBill.validate({
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

    var frmMSAddMDAService = $('#frmMSAddMDAService');

    // init validator obj and set the rules
    frmMSAddMDAService.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            MDAService: {
                required: true,
            }
            // the rules, as usual
        },

        messages: {
            MDAService: {
                required: 'Select MDA Service',
            }
        },
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent());
            element.parent().addClass("state-error");
        },
        highlight: function (element) { // hightlight error inputs
            $(element).closest('.input').addClass('error'); // set error class to the control group
        },

        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.input').removeClass('error'); // set error class to the control group
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

    $("#aMDAServices").click(function () {
        $("#frmServiceBill").hide();
        $("#dvMDAServiceList").show();
    });

    $("#btnMSBack").click(function () {
        $("#frmServiceBill").show();
        $("#dvMDAServiceList").hide();
    });

    $("#btnBackMDAService").click(function () {
        $("#frmMSAddMDAService").hide();
        $("#dvMDAServiceList").show();
    });

    $("#btnMSAdd").click(function () {
        $("#frmMSAddMDAService").show();
        $("#dvMDAServiceList").hide();

        $("#cboMDAService").val('').select2({ "width": "100%" });
        $("#lblTaxYear").html('');
        $("#tbodyServiceBillItemList").html("<tr><td colspan='4' align='center'>No Records Found.</td></tr>");
    });

    $("#btnAddMDAService").click(function () {
        if (frmMSAddMDAService.valid()) {
            var vData = {
                MDAServiceID: $("#cboMDAService").val(),
                MDAServiceName: $("#cboMDAService option:selected").text(),
                TaxYear: $("#lblTaxYear").html()
            };

            jsfn_ShowLoading();
            jsfn_ajaxPost('/ServiceBill/AddMDAService', vData, jsfn_AddMDAServiceResponse);
        }

        return false;
    });


    $("#cboMDAService").on('click', jsfn_onMDAServiceChange);
    $('#txtSettlementDueDate').datepicker('option', 'minDate', new Date());
});

$(document).on('submit', 'form', function () {
    var button = $(this).find('button[type="submit"]');
    setTimeout(function () {
        button.attr('disabled', 'disabled');
    }, 0);
});

function jsfn_onMDAServiceChange() {
    var vMDAServiceID = $("#cboMDAService").val();

    if (vMDAServiceID != '') {
        var vData = {
            MDAServiceID: vMDAServiceID == '' ? 0 : vMDAServiceID
        };

        jsfn_ajaxPost('/ServiceBill/GetMDAServiceItemList', vData, jsfn_onMDAServiceChangeResponse);
    }
    else {
        $("#lblTaxYear").html('');
        $("#tbServiceBillItem").html('<tr><td colspan="4" align="center"> No Records Found. </td></tr>');
    }
}

function jsfn_onMDAServiceChangeResponse(data) {
    if (data.success) {
        $("#lblTaxYear").html(data.TaxYear);
        $("#tbodyServiceBillItemList").html(data.ServiceBillItemList);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowMDAServiceItemDetails(MDAServiceItemID) {

    var vData = {
        MDAServiceItemID: MDAServiceItemID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/ServiceBill/GetMDAServiceItemDetails', vData, jsfn_ShowMDAServiceItemDetailResponse)
}

function jsfn_ShowMDAServiceItemDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvMDAServiceItemDetails').dialog('open');
        $('#dvMDAServiceItemRefNo').html(data.MDAServiceItemDetails.MDAServiceItemReferenceNo);
        $('#dvRevenueStream').html(data.MDAServiceItemDetails.RevenueStreamName);
        $('#dvRevenueSubStream').html(data.MDAServiceItemDetails.RevenueSubStreamName);
        $('#dvItemCategory').html(data.MDAServiceItemDetails.AssessmentItemCategoryName);
        $('#dvItemSubCategory').html(data.MDAServiceItemDetails.AssessmentItemSubCategoryName);
        $('#dvRevenueAgency').html(data.MDAServiceItemDetails.AgencyName);
        $('#dvMDAServiceItemName').html(data.MDAServiceItemDetails.MDAServiceItemName);
        $('#dvComputation').html(data.MDAServiceItemDetails.ComputationName);
        $('#dvServiceBaseAmount').html(data.MDAServiceItemDetails.ServiceBaseAmount);
        $('#dvPercentage').html(data.MDAServiceItemDetails.Percentage);
        $('#dvServiceAmount').html(data.MDAServiceItemDetails.ServiceAmount);

        if (data.MDAServiceItemDetails.ComputationID == 1) {
            $('#dvCPercentage').hide();
        }
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_EditServiceAmount(rowId, servicebaseamount) {
    $('#dvEditServiceAmount').dialog('open');
    $("#hdnServiceBillItemRowID").val(rowId);
    $("#txtServiceBaseAmount").val(servicebaseamount);
}

function jsfn_UpdateServiceAmount() {
    if ($('#frmServiceBillItem').valid()) {
        var vData = {
            ServiceBillItemRowID: $("#hdnServiceBillItemRowID").val(),
            ServiceBaseAmount: $("#txtServiceBaseAmount").val()
        };
        jsfn_ShowLoading();
        jsfn_ajaxPost('/ServiceBill/UpdateMDAServiceItem', vData, jsfn_UpdateServiceAmountResponse);
    }
}

function jsfn_UpdateServiceAmountResponse(data) {
    jsfn_HideLoading();
    $("#tbodyServiceBillItemList").html(data.ServiceBillItemList);
    $('#dvEditServiceAmount').dialog('close');
}

function jsfn_AddMDAServiceResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        $('#tbodyMDAServiceList').html(data.MDAServiceList);
        $('#spnMSRecordCount').html(data.MDAServiceCount);
        $("#frmMSAddMDAService").hide();
        $("#dvMDAServiceList").show();
        $("#ddliServiceBillService").removeClass('state-error');
        $("#ServiceBillService-error").addClass('hide');

        $("#cboMDAService").val('').select2({ "width": "100%" });
        $("#lblTaxYear").html('');
        $("#tbodyServiceBillItemList").html(data.ServiceBillItemList);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_RemoveMDAService(rowid) {
    var vData = {
        RowID: rowid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/ServiceBill/RemoveMDAService', vData, jsfn_RemoveMDAServiceResponse)
}

function jsfn_RemoveMDAServiceResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        $('#tbodyMDAServiceList').html(data.MDAServiceList);
        $('#spnMSRecordCount').html(data.MDAServiceCount);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowMDAServiceDetails(MDAServiceID) {

    var vData = {
        MDAServiceID: MDAServiceID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/ServiceBill/GetMDAServiceDetails', vData, jsfn_ShowMDAServiceDetailResponse)
}

function jsfn_ShowMDAServiceDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvMDAServiceDetails').dialog('open');
        $('#dvMDAServiceCode').html(data.MDAServiceDetails.MDAServiceCode);
        $('#dvMDAServiceName').html(data.MDAServiceDetails.MDAServiceName);
        $('#dvRuleRun').html(data.MDAServiceDetails.RuleRunName);
        $('#dvFrequency').html(data.MDAServiceDetails.PaymentFrequencyName);
        $('#dvTaxYear').html(data.MDAServiceDetails.TaxYear);
        $('#dvSettlementMethod').html(data.MDAServiceDetails.SettlementMethodNames);
        $('#dvPaymentOption').html(data.MDAServiceDetails.PaymentOptionName);
        $('#dvStatus').html(data.MDAServiceDetails.ActiveText);
        $('#dvMDAServiceItems').html(data.MDAServiceDetails.MDAServiceItemNames);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}