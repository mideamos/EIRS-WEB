$(document).ready(function () {
    $("#fsStep2").hide();
    $("#hdStep2").hide();
    $("#btnSave").hide();
    $("#btnBack").hide();

    $("#dvAssessmentRuleList").hide();
    $("#frmARAddAssessmentRule").hide();


    $('#dvEditSettlementAmount').dialog({
        autoOpen: false,
        width: 500,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Edit Settlement Amount</h4></div>",
        buttons: [{
            html: "<i class='fa fa-check'></i>&nbsp; Update",
            "class": "btn btn-primary",
            click: function () {
                jsfn_UpdateSettlementAmount();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    var frmSettlementItem = $('#frmSettlementItem');

    // init validator obj and set the rules
    frmSettlementItem.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            SettlementAmount: {
                required: true,
                number: true,
                checksettleamount: true
            }
        },

        messages: {
            SettlementAmount: {
                required: 'Please enter settlement amount',
                number: 'Please enter proper settlement amount',
                checksettleamount: 'Please enter amount less than or equal to settlement amount'
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

    //$('#txtSettlementDate').datepicker('option', 'minDate', new Date());


});

function jsfn_EditSettlementAmount(rowId, settlementamount,tosettleamount) {
    $('#dvEditSettlementAmount').dialog('open');
    $("#hdnSettlementItemRowID").val(rowId);
    $("#hdnUnSettledAmount").val(settlementamount);
    $("#txtSettlementAmount").val(tosettleamount);
}

function jsfn_UpdateSettlementAmount() {
    if ($('#frmSettlementItem').valid()) {
        var vData = {
            SettlementItemRowID: $("#hdnSettlementItemRowID").val(),
            SettlementAmount: $("#txtSettlementAmount").val()
        };
        jsfn_ShowLoading();
        jsfn_ajaxPost('/Settlement/UpdateSettlementItem', vData, jsfn_UpdateSettlementAmountResponse);
    }
}

function jsfn_UpdateSettlementAmountResponse(data) {
    jsfn_HideLoading();
    $("#scSettlementItemList").html(data.SettlementItemList);
    $('#dvEditSettlementAmount').dialog('close');
}