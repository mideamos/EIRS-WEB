$(document).ready(function () {
    $('#btnSubmit').click(function () {
        if ($("#frmAssessmentSubGroup").valid()) {
            $('#dvUpdateDialog').dialog('open');
        }
        return false;

    });

    $('#dvUpdateDialog').removeClass('hide');
    $('#dvUpdateDialog').dialog({
        autoOpen: false,
        width: 600,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Edit Record Confirmation</h4></div>",
        buttons: [{
            html: "<i class='fa fa-check'></i>&nbsp; Yes",
            "class": "btn btn-primary",
            click: function () {
                $("#frmAssessmentSubGroup").submit();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#cboAssetType').on("change", jsfn_onAssetTypeChange);


});

function jsfn_onAssetTypeChange() {
    var vAssetTypeID = $('#cboAssetType').val();

    if (vAssetTypeID != '') {

        var vData = {
            AssetTypeID: vAssetTypeID == '' ? 0 : vAssetTypeID
        };

        jsfn_ajaxPost('/AssessmentSubGroup/GetAssessmentGroup', vData, jsfn_onAssetTypeChangeResponse);
    }
    else {
        $("#cboAssessmentGroup").html("");

        $("#cboAssessmentGroup").append(
                $('<option></option>').val("").html("Select Assessment Group"));


        $('#cboAssessmentGroup').val('').select2({ "width": "100%" });

    }
}

function jsfn_onAssetTypeChangeResponse(data) {
    $("#cboAssessmentGroup").html("");

    $("#cboAssessmentGroup").append(
            $('<option></option>').val("").html("Select Assessment Group"));

    if (data != undefined) {
        $.each(data, function (i, rstm) {
            $("#cboAssessmentGroup").append(
                $('<option></option>').val(rstm.id).html(rstm.text));
        });

        $('#cboAssessmentGroup').val('').select2({ "width": "100%" });
   }
}
