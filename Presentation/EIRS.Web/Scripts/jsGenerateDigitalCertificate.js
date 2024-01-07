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

    $(".custfield").each(function (index, element) {
        var vReadonly = element.dataset["readonly"];

        if (vReadonly === "true") {
            $(element).attr('readonly', 'readonly')
        }
        else {
            $(element).removeAttr('readonly');
        }
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