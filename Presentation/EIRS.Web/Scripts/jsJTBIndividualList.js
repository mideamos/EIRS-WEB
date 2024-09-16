$(document).ready(function () {
    $("#tblIndividual").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/JTB/GetIndividualData",
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
            { "data": "bvn", "orderable": true, "name": "bvn", "width": "25%" },
            {
                "data": "SBIRt_name", "orderable": true, "name": "SBIRt_name", "width": "25%", "render": function (data, type, st) {
                    return st.SBIRt_name +  '  ' +  st.last_name;
                }
            },
            {
                "data": "street_name", "orderable": true, "name": "street_name", "width": "25%", "render": function (data, type, st) {
                    return st.house_number + ',' + st.street_name;
                }
            },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_JTBIndividualInformation(' + st.JTBIndividualID + ')">'
                        + 'Quick View</a></li><li><a href="IndividualETLData?id=' + st.JTBIndividualID + '">ETL Data</a></li></ul></div>';
                }, "width": "10%"
            },
        ],
        "order": [[1, "asc"]]
    });
});

function jsfn_JTBIndividualInformation(indId) {

    var vData = {
        IndividualID: indId,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/JTB/GetJTBIndividualDetails', vData, jsfn_JTBIndividualInformationResponse);
}

function jsfn_JTBIndividualInformationResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvIndividualModal').modal('show');
        $('#dvtin').html(data.TaxPayerDetails.tin == null ? '-' : data.TaxPayerDetails.tin);
        $('#dvbvn').html(data.TaxPayerDetails.bvn == null ? '-' : data.TaxPayerDetails.bvn);
        $('#dvnin').html(data.TaxPayerDetails.nin == null ? '-' : data.TaxPayerDetails.nin);
        $('#dvTitle').html(data.TaxPayerDetails.Title == null ? '-' : data.TaxPayerDetails.Title);
        $('#dvSBIRt_name').html(data.TaxPayerDetails.SBIRt_name == null ? '-' : data.TaxPayerDetails.SBIRt_name);
        $('#dvmiddle_name').html(data.TaxPayerDetails.middle_name == null ? '-' : data.TaxPayerDetails.middle_name);
        $('#dvlast_name').html(data.TaxPayerDetails.last_name == null ? '-' : data.TaxPayerDetails.last_name);
        $('#dvGenderName').html(data.TaxPayerDetails.GenderName == null ? '-' : data.TaxPayerDetails.GenderName);
        $('#dvStateOfOrigin').html(data.TaxPayerDetails.StateOfOrigin == null ? '-' : data.TaxPayerDetails.StateOfOrigin);
        $('#dvdate_of_birth').html(data.TaxPayerDetails.date_of_birth == null ? '-' : jsfn_FormatJsonDate(data.TaxPayerDetails.date_of_birth));
        $('#dvMaritalStatus').html(data.TaxPayerDetails.MaritalStatus == null ? '-' : data.TaxPayerDetails.MaritalStatus);
        $('#dvOccupation').html(data.TaxPayerDetails.Occupation == null ? '-' : data.TaxPayerDetails.Occupation);
        $('#dvnationality_name').html(data.TaxPayerDetails.nationality_name == null ? '-' : data.TaxPayerDetails.nationality_name);
        $('#dvphone_no_1').html(data.TaxPayerDetails.phone_no_1 == null ? '-' : data.TaxPayerDetails.phone_no_1);
        $('#dvphone_no_2').html(data.TaxPayerDetails.phone_no_2 == null ? '-' : data.TaxPayerDetails.phone_no_2);
        $('#dvtaxpayer_photo').html(data.TaxPayerDetails.taxpayer_photo == null ? '-' : data.TaxPayerDetails.taxpayer_photo);
        $('#dvemail_address').html(data.TaxPayerDetails.email_address == null ? '-' : data.TaxPayerDetails.email_address);
        $('#dvdate_of_registration').html(data.TaxPayerDetails.date_of_registration == null ? '-' : jsfn_FormatJsonDate(data.TaxPayerDetails.date_of_registration));
        $('#dvhouse_number').html(data.TaxPayerDetails.house_number == null ? '-' : data.TaxPayerDetails.house_number);
        $('#dvstreet_name').html(data.TaxPayerDetails.street_name == null ? '-' : data.TaxPayerDetails.street_name);
        $('#dvcity').html(data.TaxPayerDetails.city == null ? '-' : data.TaxPayerDetails.city);
        $('#dvLGAName').html(data.TaxPayerDetails.LGAName == null ? '-' : data.TaxPayerDetails.LGAName);
        $('#dvStateName').html(data.TaxPayerDetails.StateName == null ? '-' : data.TaxPayerDetails.StateName);
        $('#dvCountryName').html(data.TaxPayerDetails.CountryName == null ? '-' : data.TaxPayerDetails.CountryName);
        $('#dvTaxAuthorityCode').html(data.TaxPayerDetails.TaxAuthorityCode == null ? '-' : data.TaxPayerDetails.TaxAuthorityCode);
        $('#dvTaxAuthorityName').html(data.TaxPayerDetails.TaxAuthorityName == null ? '-' : data.TaxPayerDetails.TaxAuthorityName);
        $('#dvTaxpayerStatus').html(data.TaxPayerDetails.TaxpayerStatus == null ? '-' : data.TaxPayerDetails.TaxpayerStatus);

    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
