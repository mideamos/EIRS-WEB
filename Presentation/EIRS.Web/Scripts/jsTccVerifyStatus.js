$(document).ready(function () {
    console.log("Javascript connected");
    $('#btnSearch').click(function () {
        debugger
        var formData = {
            txtName: $('#txtName').val()
        };
        $.ajax({
            url: '@Url.Action("VerifyTccRequestStatus", "Default")',
            type: 'POST',
            data: formData,
            success: function (response) {
                console.log("i reach here and about to redirect");
                window.location.href = "/Default/VerifyTccRequestStatusRes";
            },
            error: function () {

                console.log("i reach here and about to redirect");
                window.location.href = "/Default/VerifyTccRequestStatusRes";
            }
        });
    });

    
});

function cancelAction() {
    window.location.href = "/Default/VerifyTccRequestStatus";
};