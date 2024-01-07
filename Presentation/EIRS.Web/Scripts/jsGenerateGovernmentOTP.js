function jsfn_ResendOTP() {
    jsfn_ajaxPost('/SignupGovernment/ResendOTP', null, jsfn_ResendOTPResponse);
}

function jsfn_ResendOTPResponse(data) {
    if (data.success) {
        jsfn_ShowAlert('OTP Sent Successfully');
    }
    else {
        jsfn_ShowAlert('Error Occurred Will resending OTP', 'danger');
    }
}