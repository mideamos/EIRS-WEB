$(document).ready(function () {
    $("#btnRevoke").on('click', () => {
        $("#dvShowRevokeAlert").hide();
        $("#txtReason").val('');
        $("#dvRevokeModal").modal('show');
    });

    $("#btnSaveRevoke").click(function () {
        if ($("#txtReason").val() !== '') {

            var vData = {
                Reason: $("#txtReason").val(),
                RequestID: $("#hdnRequestID").val()
            };

            jsfn_ajaxPost('/ProcessTCCRequest/Revoke', vData, jsfn_SaveRevokeResponse);

        }
        else {
            $("#dvShowRevokeAlert").html('Please Enter Reason');
            $("#dvShowRevokeAlert").show();
        }
    });
});

function jsfn_SaveRevokeResponse(data) {
    if (data.success) {
        $("#txtReason").val('');
        $("#dvRevokeModal").modal('hide');
        jsfn_ShowAlert(data.Message, 'success', true);
        window.location.reload();

    }
    else {
        jsfn_ShowAlert(data.Message, 'danger', true);
    }
}