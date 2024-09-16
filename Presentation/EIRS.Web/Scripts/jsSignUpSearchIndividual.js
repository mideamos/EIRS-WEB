$(document).ready(function () {
    $('#dvSearchData').hide();
    $("#dvShowAddIndividualAlert").hide();
    jsfn_bindTable();

    $("#btnSearch").click(function () { return jsfn_SearchValidation('frmSearchIndividual') });

    $("#btnModalContinue").click(function () {
        var vData = {
            IndividualID: $("#hdnTaxPayerID").val()
        };

        jsfn_ajaxPost('/SignupIndividual/SetSelectedIndividual', vData, jsfn_ModalContinueResponse);
    });

    $("#btnAddNewIndividual").click(function () {
        if ($("#txtAddIndividual_MobileNumber").val() != '') {
            var vData = {
                MobileNumber: $("#txtAddIndividual_MobileNumber").val()
            };

            jsfn_ajaxPost('/SignupIndividual/CheckMobileNumber', vData, jsfn_ModalContinueResponse);
        }
        else {
            $("#dvShowAddIndividualAlert").html('Please Enter Mobile Number');
            $("#dvShowAddIndividualAlert").show();
        }
    });
});

function jsfn_ModalContinueResponse(data) {
    if (data.success) {
        window.location.href = data.RedirectUrl;
    }
    else {
        $("#dvShowAddIndividualAlert").html(data.Message);
        $("#dvShowAddIndividualAlert").show();
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

    jsfn_ConvertToDatableWithCustomSort($('#tblIndividual'), vColumnsList, 'No Individual Found', vSortOrder);
    $(".collapse").click();
}

function jsfn_SelectIndividual(IndID) {

    var vData = {
        IndividualID: IndID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/SignupIndividual/GetIndividualDetails', vData, jsfn_IndividualDetailResponse);
}

function jsfn_IndividualDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvIndividualModal').modal('show');
        $('#dvIndividualRIN').html(data.TaxPayerDetails.IndividualRIN);
        $('#dvGender').html(data.TaxPayerDetails.GenderName);
        $('#dvTitle').html(data.TaxPayerDetails.TitleName);
        $('#dvFirstName').html(data.TaxPayerDetails.FirstName);
        $('#dvLastName').html(data.TaxPayerDetails.LastName);
        $('#dvMiddleName').html(data.TaxPayerDetails.MiddleName == null ? '-' : data.TaxPayerDetails.MiddleName);
        $('#dvDateofBirth').html(jsfn_FormatJsonDate(data.TaxPayerDetails.DOB));
        $('#dvITIN').html(data.TaxPayerDetails.TIN == null ? '-' : data.TaxPayerDetails.TIN);
        $('#dvIMobileNumber1').html(data.TaxPayerDetails.MobileNumber1);
        $('#dvIMobileNumber2').html(data.TaxPayerDetails.MobileNumber2 == null ? '-' : data.TaxPayerDetails.MobileNumber2);
        $('#dvIEmailAddress1').html(data.TaxPayerDetails.EmailAddress1 == null ? '-' : data.TaxPayerDetails.EmailAddress1);
        $('#dvIEmailAddress2').html(data.TaxPayerDetails.EmailAddress2 == null ? '-' : data.TaxPayerDetails.EmailAddress2);
        $('#dvBiometricDetails').html(data.TaxPayerDetails.BiometricDetails == null ? '-' : data.TaxPayerDetails.BiometricDetails);
        $('#dvITaxOffice').html(data.TaxPayerDetails.TaxOfficeName == null ? '-' : data.TaxPayerDetails.TaxOfficeName);
        $('#dvMaritalStatus').html(data.TaxPayerDetails.MaritalStatusName == null ? '-' : data.TaxPayerDetails.MaritalStatusName);
        $('#dvNationality').html(data.TaxPayerDetails.NationalityName == null ? '-' : data.TaxPayerDetails.NationalityName);
        $('#dvIEconomicActivities').html(data.TaxPayerDetails.EconomicActivitiesName);
        $('#dvIStatus').html(data.TaxPayerDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ConfirmationModal(indId) {
    $('#dvModalConfirmation').modal('show');
    $("#hdnTaxPayerID").val(indId);
}
