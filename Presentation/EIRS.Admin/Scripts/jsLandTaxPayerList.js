var vTaxPayerAssetDataTable;

jQuery(document).ready(function () {
    vTaxPayerAssetDataTable = jsfn_ConvertToDatatable($('#tbTaxPayerAsset'));

    $('#dvIndividualDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Individual Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#dvCompanyDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Company Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#dvGovernmentDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Government Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#dvSpecialDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Special Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#dvRemoveDialog').removeClass('hide');
    $('#dvRemoveDialog').dialog({
        autoOpen: false,
        width: 600,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Remove Record Confirmation</h4></div>",
        buttons: [{
            html: "<i class='fa fa-check'></i>&nbsp; Yes",
            "class": "btn btn-primary",
            click: function () {
                var vData = {
                    TPAID: $("#hdnTBID").val(),
                    AssetID: $("#hdnOtherID").val()
                };

                jsfn_ShowLoading();
                jsfn_ajaxPost('/Land/RemoveTaxPayer', vData, jsfn_RemoveLinkResponse);
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

});

function jsfn_ChangeStatus(tpaid, assetid) {
    var vData = {
        TPAID: tpaid,
        AssetID: assetid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Land/UpdateTaxPayerStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vTaxPayerAssetDataTable.destroy();
        $('#tbody').html(data.AssetList);
        vTaxPayerAssetDataTable = jsfn_ConvertToDatatable($('#tbTaxPayerAsset'));
        $("#tbTaxPayerAsset thead th input[type=text]").each(function () {
            vTaxPayerAssetDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowDetails(tpaid) {

    var vData = {
        TPAID: tpaid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Land/GetTaxPayerDetails', vData, jsfn_TaxPayerDetailResponse);
}

function jsfn_TaxPayerDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        if (data.TaxPayerTypeID == '1') {
            $('#dvIndividualDetails').dialog('open');
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
        else if (data.TaxPayerTypeID == '2') {
            $('#dvCompanyDetails').dialog('open');
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
        else if (data.TaxPayerTypeID == '4') {
            $('#dvGovernmentDetails').dialog('open');
            $('#dvGovernmentRIN').html(data.TaxPayerDetails.GovernmentRIN);
            $('#dvGovernmentName').html(data.TaxPayerDetails.GovernmentName);
            $('#dvGovernmentType').html(data.TaxPayerDetails.GovernmentTypeName);
            $('#dvGTaxOffice').html(data.TaxPayerDetails.TaxOfficeName == null ? '-' : data.TaxPayerDetails.TaxOfficeName);
            $('#dvGContactName').html(data.TaxPayerDetails.ContactName);
            $('#dvGContactEmail').html(data.TaxPayerDetails.ContactEmail);
            $('#dvGContactNumber').html(data.TaxPayerDetails.ContactNumber);
            $('#dvGStatus').html(data.TaxPayerDetails.ActiveText);
        }
        else if (data.TaxPayerTypeID == '3') {
            $('#dvSpecialDetails').dialog('open');
            $('#dvSpecialRIN').html(data.TaxPayerDetails.SpecialRIN);
            $('#dvSpecialName').html(data.TaxPayerDetails.SpecialTaxPayerName);
            $('#dvSTaxOffice').html(data.TaxPayerDetails.TaxOfficeName == null ? '-' : data.TaxPayerDetails.TaxOfficeName);
            $('#dvSContactName').html(data.TaxPayerDetails.ContactName);
            $('#dvSContactEmail').html(data.TaxPayerDetails.ContactEmail);
            $('#dvSContactNumber').html(data.TaxPayerDetails.ContactNumber);
            $('#dvDescription').html(data.TaxPayerDetails.Description);
            $('#dvSStatus').html(data.TaxPayerDetails.ActiveText);
        }
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_RemoveLink(tpaid, assetid) {
    $('#dvRemoveDialog').dialog('open');
    $("#hdnTBID").val(tpaid);
    $("#hdnOtherID").val(assetid);
}

function jsfn_RemoveLinkResponse(data) {
    jsfn_HideLoading();
    $('#dvRemoveDialog').dialog('close');
    if (data.success) {

        $("#hdnTBID").val(0);
        $("#hdnOtherID").val(0);
        jsfn_ShowAlert(data.Message, 'success');
        vTaxPayerAssetDataTable.destroy();
        $('#tbody').html(data.AssetList);
        vTaxPayerAssetDataTable = jsfn_ConvertToDatatable($('#tbTaxPayerAsset'));
        $("#tbTaxPayerAsset thead th input[type=text]").each(function () {
            vTaxPayerAssetDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}


