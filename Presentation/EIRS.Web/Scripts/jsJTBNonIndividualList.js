$(document).ready(function () {
    $("#tblNonIndividual").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/JTB/GetNonIndividualData",
            "type": "POST",
            "datatype": "json",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "tin", "orderable": true, "name": "tin", "width": "25%" },
            { "data": "registration_number", "orderable": true, "name": "registration_number", "width": "25%" },
            { "data": "registered_name", "orderable": true, "name": "registered_name", "width": "25%" },
            {
                "data": "street_name", "orderable": true, "name": "street_name", "width": "25%", "render": function (data, type, st) {
                    return st.house_number + ',' + st.street_name;
                }
            },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_JTBNonIndividualInformation(' + st.JTBNonIndividualID + ')">'
                        + 'Quick View</a></li></li></ul></div>';
                }, "width": "10%"
            },
        ],
        "order": [[1, "asc"]]
    });
});

function jsfn_JTBNonIndividualInformation(indId) {

    var vData = {
        IndividualID: indId,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/JTB/GetJTBNonIndividualDetails', vData, jsfn_JTBIndividualInformationResponse);
}

function jsfn_JTBIndividualInformationResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvNonIndividualModal').modal('show');
        $('#dvtin').html(data.TaxPayerDetails.tin == null ? '-' : data.TaxPayerDetails.tin);
        $('#dvregistered_name').html(data.TaxPayerDetails.registered_name == null ? '-' : data.TaxPayerDetails.registered_name);
        $('#dvmain_trade_name').html(data.TaxPayerDetails.main_trade_name == null ? '-' : data.TaxPayerDetails.main_trade_name);
        $('#dvorg_name').html(data.TaxPayerDetails.org_name == null ? '-' : data.TaxPayerDetails.org_name);
        $('#dvregistration_number').html(data.TaxPayerDetails.registration_number == null ? '-' : data.TaxPayerDetails.registration_number);
        $('#dvphone_no_1').html(data.TaxPayerDetails.phone_no_1 == null ? '-' : data.TaxPayerDetails.phone_no_1);
        $('#dvphone_no_2').html(data.TaxPayerDetails.phone_no_2 == null ? '-' : data.TaxPayerDetails.phone_no_2);
        $('#dvemail_address').html(data.TaxPayerDetails.email_address == null ? '-' : data.TaxPayerDetails.email_address);
        $('#dvline_of_business_code').html(data.TaxPayerDetails.line_of_business_code == null ? '-' : data.TaxPayerDetails.line_of_business_code);
        $('#dvlob_name').html(data.TaxPayerDetails.lob_name == null ? '-' : data.TaxPayerDetails.lob_name);
        $('#dvdate_of_registration').html(data.TaxPayerDetails.date_of_registration == null ? '-' : jsfn_FormatJsonDate(data.TaxPayerDetails.date_of_registration));
        $('#dvcommencement_date').html(data.TaxPayerDetails.commencement_date == null ? '-' : jsfn_FormatJsonDate(data.TaxPayerDetails.commencement_date));
        $('#dvdate_of_incorporation').html(data.TaxPayerDetails.date_of_incorporation == null ? '-' : jsfn_FormatJsonDate(data.TaxPayerDetails.date_of_incorporation));
        $('#dvhouse_number').html(data.TaxPayerDetails.house_number == null ? '-' : data.TaxPayerDetails.house_number);
        $('#dvstreet_name').html(data.TaxPayerDetails.street_name == null ? '-' : data.TaxPayerDetails.street_name);
        $('#dvcity').html(data.TaxPayerDetails.city == null ? '-' : data.TaxPayerDetails.city);
        $('#dvLGAName').html(data.TaxPayerDetails.LGAName == null ? '-' : data.TaxPayerDetails.LGAName);
        $('#dvStateName').html(data.TaxPayerDetails.StateName == null ? '-' : data.TaxPayerDetails.StateName);
        $('#dvCountryName').html(data.TaxPayerDetails.CountryName == null ? '-' : data.TaxPayerDetails.CountryName);
        $('#dvFinYrBegin').html(data.TaxPayerDetails.FinYrBegin == null ? '-' : data.TaxPayerDetails.FinYrBegin);
        $('#dvFinYrEnd').html(data.TaxPayerDetails.FinYrEnd == null ? '-' : data.TaxPayerDetails.FinYrEnd);
        $('#dvdirector_name').html(data.TaxPayerDetails.director_name == null ? '-' : data.TaxPayerDetails.director_name);
        $('#dvdirector_phone').html(data.TaxPayerDetails.director_phone == null ? '-' : data.TaxPayerDetails.director_phone);
        $('#dvdirector_email').html(data.TaxPayerDetails.director_email == null ? '-' : data.TaxPayerDetails.director_email);
        $('#dvTaxAuthorityCode').html(data.TaxPayerDetails.TaxAuthorityCode == null ? '-' : data.TaxPayerDetails.TaxAuthorityCode);
        $('#dvTaxAuthorityName').html(data.TaxPayerDetails.TaxAuthorityName == null ? '-' : data.TaxPayerDetails.TaxAuthorityName);
        $('#dvTaxpayerStatus').html(data.TaxPayerDetails.TaxpayerStatus == null ? '-' : data.TaxPayerDetails.TaxpayerStatus);


    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
