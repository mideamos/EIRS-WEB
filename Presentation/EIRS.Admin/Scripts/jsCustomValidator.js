$(document).ready(function () {

    $.validator.methods.date = function (value, element) {
        return this.optional(element) || moment(value, "DD/MM/YYYY", true).isValid();
    }

    $('form').each(function (index, item) {
        if ($.data(item, 'validator') != undefined) {
            var vSettings = $.data(item, 'validator').settings;

            var vErrorFunction = vSettings.errorPlacement;

            vSettings.errorPlacement = function (error, inputElement) {
                if (error.text() == "") {
                    inputElement.closest(".input").removeClass("state-error");
                    inputElement.closest(".textarea").removeClass("state-error");
                }
                else {
                    inputElement.closest(".input").addClass("state-error");
                    inputElement.closest(".textarea").addClass("state-error");
                }

                vErrorFunction.call(item, error, inputElement);
            };
        }
    });
});



$(function () {

    jQuery.validator.addMethod('computationvalidator', function (value, element, params) {
        var vComputationID = $('#cboComputation').val();

        if (vComputationID == '2' && value == '') {
            return false;
        }
        else if (vComputationID == '2' && (parseFloat(value) < 0 || parseFloat(value) > 100)) {
            return false;
        }

        return true;
    }, 'Enter Required Value');


    jQuery.validator.unobtrusive.adapters.add('computationvalidator', function (options) {
        options.rules['computationvalidator'] = {};
        options.messages['computationvalidator'] = options.message;
    });

    jQuery.validator.addMethod('checksettleamount', function (value, element, params) {
        var vSettlementAmount = $('#hdnUnSettledAmount').val();

        if (parseFloat(value) > parseFloat(vSettlementAmount)) {
            return false;
        }
        else {
            return true;
        }

    }, 'Please enter amount less than or equal to settlement amount');

}(jQuery));