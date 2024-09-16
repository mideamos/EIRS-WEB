$(document).ready(function () {
    $('.rsyearlyamount').on('change', function () {
        var vRSID = $(this).data('rsid');
        var vTargetAmount = $(this).val();

        $('#spnrsmonthamount_' + vRSID).html((parseFloat(vTargetAmount) / 12).formatMoney(2));

        var vYearTotal = 0, vMonthTotal = 0;
        $('.rsyearlyamount').each(function (index, element) {
            var vTargetAmount = $(element).val();
            if (vTargetAmount !== '') {
                vYearTotal = vYearTotal + parseFloat(vTargetAmount);
                vMonthTotal = vMonthTotal + parseFloat(vTargetAmount) / 12;
            }
        });

        $("#spnrsyearlytotal").html(vYearTotal.formatMoney(2));
        $("#spnrsmonthlytotal").html(vMonthTotal.formatMoney(2));
    });
});