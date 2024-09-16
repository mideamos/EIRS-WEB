function jsfn_PullNotes() {
    $("#ReviewStatusID").val('').selectpicker('refresh');
    $("#Notes").val('');

    var vTaxPayerID = $("#hdnTaxPayerID").val();
    var vTaxPayerTypeID = $("#hdnTaxPayerTypeID").val();

    var data = { TaxPayerID: vTaxPayerID, TaxPayerTypeID: vTaxPayerTypeID };
    jsfn_ajaxPost('/Base/GetTaxPayerReview', data, jsfn_PullNotesResponse);
}

function jsfn_PullNotesResponse(data) {
    $('#dvMessageList').html(data.ReviewList);
}