jQuery(document).ready(function () {
    $(".staff").hide();

    $(".remove").click(function () {
        $("#fuSignatureFile").val('');
        $('.fileinput-filename').html('');
        $('.fileinput-preview').html('');
        $('.fileinput').addClass('fileinput-new');
        $('.fileinput').removeClass('fileinput-exists');
    });

    jsfn_OnUserTypeChange();
    $("#cboUserType").on('change', jsfn_OnUserTypeChange);

    jsfn_IsManagerSelected($("#chkIsManager").is(":checked"));
    $("#chkIsManager").on("ifChecked", function () {
        jsfn_IsManagerSelected(true);
    });

    $("#chkIsManager").on("ifUnchecked", function () {
        jsfn_IsManagerSelected(false);
    });

    $('#cboTaxOffice').on("change", jsfn_onTaxOfficeChange);
});

function jsfn_OnUserTypeChange() {
    var vUserTypeID = $("#cboUserType").val();

    if (vUserTypeID === '2') {
        $(".staff").show();
    }
    else {
        $(".staff").hide();
    }
}

function jsfn_IsManagerSelected(isChecked) {
    if (isChecked) {
        $(".manager").hide();
    }
    else {
        $(".manager").show();
    }
}

function jsfn_onTaxOfficeChange() {
    var vTaxOfficeID = $('#cboTaxOffice').val();

    if (vTaxOfficeID != '') {

        var vData = {
            TaxOfficeID: vTaxOfficeID == '' ? 0 : vTaxOfficeID,
            showmanager : true
        };

        jsfn_ajaxPost('/Base/GetTaxOfficeManger', vData, jsfn_onTaxOfficeChangeResponse);
    }
}

function jsfn_onTaxOfficeChangeResponse(data) {
    $("#cboManager").html("");

    $("#cboManager").append(
        $('<option></option>').val("").html("Select Manager"));

    if (data !== undefined) {
        $.each(data, function (i, tit) {
            $("#cboManager").append(
                $('<option></option>').val(tit.id).html(tit.text));
        });

        $('#cboManager').selectpicker('refresh');
    }
}