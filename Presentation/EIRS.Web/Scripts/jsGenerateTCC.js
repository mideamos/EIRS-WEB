$(document).ready(function () {
    $.fn.datepicker.defaults.format = "dd/mm/yyyy";
    $("#chkIsExpirable").on("ifChecked", function () {
        jsfn_OnExpirableChange(true);
    });

    $("#chkIsExpirable").on("ifUnchecked", function () {
        jsfn_OnExpirableChange(false);
    });

    $("#txtExpiryDate").datepicker({
        autoclose: true
    });


});




function jsfn_OnExpirableChange(isChecked) {
    if (isChecked) {
        $("#txtExpiryDate").attr("disabled", "true");
    }
    else {
        $("#txtExpiryDate").removeAttr("disabled");
    }
}