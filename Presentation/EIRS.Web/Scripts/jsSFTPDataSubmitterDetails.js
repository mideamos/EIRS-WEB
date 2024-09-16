jQuery(document).ready(function () {
    var vDataSubmissionTypeData = new Array();
    var vDataSubmissionTypeVal = $('#cboDataSubmissionType').attr('data-initvalue');
    if (vDataSubmissionTypeVal != '')
        vDataSubmissionTypeData = vDataSubmissionTypeVal.split(',');
    $('#cboDataSubmissionType').val(vDataSubmissionTypeData);
    $('#cboDataSubmissionType').selectpicker('refresh');
});