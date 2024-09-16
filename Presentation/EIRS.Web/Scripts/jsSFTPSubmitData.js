$(document).ready(function () {
    $("#cboDataSubmitter").on('change', jsfn_onDataSubmitterChange);
});

function jsfn_onDataSubmitterChange() {
    var vDataSubmitterID = $('#cboDataSubmitter').val();

    if (vDataSubmitterID != '') {

        var vData = {
            DataSubmitterID: vDataSubmitterID == '' ? 0 : vDataSubmitterID
        };

        jsfn_ajaxPost('/Base/GetDataSubmissionType', vData, jsfn_onDataSubmitterChangeResponse);
    }
}

function jsfn_onDataSubmitterChangeResponse(data) {
    $("#cboDataSubmissionType").html("");

    $("#cboDataSubmissionType").append(
        $('<option></option>').val("").html("Select Data Submission Type"));

    if (data != undefined) {
        $.each(data, function (i, dst) {
            $("#cboDataSubmissionType").append(
                $('<option></option>').val(dst.id).html(dst.text));
        });

        $('#cboDataSubmissionType').selectpicker('refresh');
    }
}

