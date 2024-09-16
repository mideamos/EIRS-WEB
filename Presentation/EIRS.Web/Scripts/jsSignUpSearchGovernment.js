$(document).ready(function () {
    $('#dvSearchData').hide();
    $("#dvShowAddGovernmentAlert").hide();
    jsfn_bindTable();

    $("#btnSearch").click(function () { return jsfn_SearchValidation('frmSearchGovernment') });

    $("#btnModalContinue").click(function () {
        var vData = {
            GovernmentID: $("#hdnTaxPayerID").val()
        };

        jsfn_ajaxPost('/SignupGovernment/SetSelectedGovernment', vData, jsfn_ModalContinueResponse);
    });

    $("#btnAddNewGovernment").click(function () {
        if ($("#txtAddGovernment_MobileNumber").val() != '') {
            var vData = {
                MobileNumber: $("#txtAddGovernment_MobileNumber").val()
            };

            jsfn_ajaxPost('/SignupGovernment/CheckMobileNumber', vData, jsfn_ModalContinueResponse);
        }
        else {
            $("#dvShowAddGovernmentAlert").html('Please Enter Mobile Number');
            $("#dvShowAddGovernmentAlert").show();
        }
    });
});

function jsfn_ModalContinueResponse(data) {
    if (data.success) {
        window.location.href = data.RedirectUrl;
    }
    else {
        $("#dvShowAddGovernmentAlert").html(data.Message);
        $("#dvShowAddGovernmentAlert").show();
    }
}

function jsfn_ShowData() {
    $('#dvSearchData').show();
    jsfn_bindTable();
}

function jsfn_bindTable() {
    var vColumnsList = [{
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": false
    }];

    var vSortOrder = [];

    jsfn_ConvertToDatableWithCustomSort($('#tblGovernment'), vColumnsList, 'No Government Found', vSortOrder);
    $(".collapse").click();
}

function jsfn_SelectGovernment(IndID) {

    var vData = {
        GovernmentID: IndID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/SignupGovernment/GetGovernmentDetails', vData, jsfn_GovernmentDetailResponse);
}

function jsfn_GovernmentDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvGovernmentModal').modal('show');
        $('#dvGovernmentRIN').html(data.TaxPayerDetails.GovernmentRIN);
        $('#dvGTIN').html(data.TaxPayerDetails.TIN == null ? '-' : data.TaxPayerDetails.TIN);
        $('#dvGovernmentName').html(data.TaxPayerDetails.GovernmentName);
        $('#dvGovernmentType').html(data.TaxPayerDetails.GovernmentTypeName);
        $('#dvGTaxOffice').html(data.TaxPayerDetails.TaxOfficeName == null ? '-' : data.TaxPayerDetails.TaxOfficeName);
        $('#dvGContactName').html(data.TaxPayerDetails.ContactName);
        $('#dvGContactEmail').html(data.TaxPayerDetails.ContactEmail);
        $('#dvGContactNumber').html(data.TaxPayerDetails.ContactNumber);
        $('#dvGStatus').html(data.TaxPayerDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ConfirmationModal(indId) {
    $('#dvModalConfirmation').modal('show');
    $("#hdnTaxPayerID").val(indId);
}
