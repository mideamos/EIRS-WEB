$(document).ready(function () {
    $('.bs-select').selectpicker();

    $('[name="txtToPayAmount"]').blur(function () {
        var vtxtToPayAmount = $(this).val();
        if ($.isNumeric(vtxtToPayAmount)) {
            var vMaxAmount = $(this).data('maxamount');
            if (parseFloat(vtxtToPayAmount) > parseFloat(vMaxAmount)) {
                $("#dvMessage").html('<p>Amount entered is more than pending amount.</p>');
                $("#dvMessageBox").modal('show');
                setTimeout(function () {
                    $("#dvMessage").html('');
                    $("#dvMessageBox").modal('hide');
                }, 2001);
            }
        }
        else {
            $("#dvMessage").html('<p>Please enter proper amount.</p>');
            $("#dvMessageBox").modal('show');
            setTimeout(function () {
                $("#dvMessage").html('');
                $("#dvMessageBox").modal('hide');
            }, 2001);
        }
    });

    $('[name="btnSaveAmount"]').click(function () {
        var vAmountToPay = 0;

        $('[name="txtToPayAmount"]').each(function () {
            var vtxtToPayAmount = $(this).val();
            vAmountToPay = parseFloat(vAmountToPay, 2) + parseFloat(vtxtToPayAmount, 2);
        });

        $("#dvAmountToPay").html(vAmountToPay.formatMoney(2, "₦"));
        $("#dvEditAmountModal_" + $(this).data('modalid')).modal('hide');
    });
});