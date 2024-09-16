$(document).ready(function () {
    $('#dvSearchData').hide();
    $("#dvShowAddCorporateAlert").hide();
    jsfn_bindTable();

    $("#btnSearch").click(function () { return jsfn_SearchValidation('frmSearchCorporate') });

    $("#btnModalContinue").click(function () {
        var vData = {
            CompanyID: $("#hdnTaxPayerID").val()
        };

        jsfn_ajaxPost('/SignupCorporate/SetSelectedCompany', vData, jsfn_ModalContinueResponse);
    });

    $("#btnAddNewCorporate").click(function () {
        if ($("#txtAddCorporate_MobileNumber").val() != '') {
            var vData = {
                MobileNumber: $("#txtAddCorporate_MobileNumber").val()
            };

            jsfn_ajaxPost('/SignupCorporate/CheckMobileNumber', vData, jsfn_ModalContinueResponse);
        }
        else {
            $("#dvShowAddCorporateAlert").html('Please Enter Mobile Number');
            $("#dvShowAddCorporateAlert").show();
        }
    });
});

function jsfn_ModalContinueResponse(data) {
    if (data.success) {
        window.location.href = data.RedirectUrl;
    }
    else {
        $("#dvShowAddCorporateAlert").html(data.Message);
        $("#dvShowAddCorporateAlert").show();
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

    jsfn_ConvertToDatableWithCustomSort($('#tblCompany'), vColumnsList, 'No Corporate Found', vSortOrder);
    $(".collapse").click();
}

function jsfn_SelectCorporate(compId) {

    var vData = {
        CompanyID: compId,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/SignupCorporate/GetCompanyDetails', vData, jsfn_CorporateDetailResponse);
}

function jsfn_CorporateDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvCompanyModal').modal('show');
        $('#dvCompanyRIN').html(data.TaxPayerDetails.CompanyRIN);
        $('#dvCompanyName').html(data.TaxPayerDetails.CompanyName);
        $('#dvCTIN').html(data.TaxPayerDetails.TIN == null ? '-' : data.TaxPayerDetails.TIN);
        $('#dvCMobileNumber1').html(data.TaxPayerDetails.MobileNumber1);
        $('#dvCMobileNumber2').html(data.TaxPayerDetails.MobileNumber2 == null ? '-' : data.TaxPayerDetails.MobileNumber2);
        $('#dvCEmailAddress1').html(data.TaxPayerDetails.EmailAddress1 == null ? '-' : data.TaxPayerDetails.EmailAddress1);
        $('#dvCEmailAddress2').html(data.TaxPayerDetails.EmailAddress2 == null ? '-' : data.TaxPayerDetails.EmailAddress2);
        $('#dvCTaxOffice').html(data.TaxPayerDetails.TaxOfficeName == null ? '-' : data.TaxPayerDetails.TaxOfficeName);
        $('#dvCEconomicActivities').html(data.TaxPayerDetails.EconomicActivitiesName);
        $('#dvCStatus').html(data.TaxPayerDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ConfirmationModal(indId) {
    $('#dvModalConfirmation').modal('show');
    $("#hdnTaxPayerID").val(indId);
}
