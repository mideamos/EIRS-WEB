var vCompanyDataTable;

jQuery(document).ready(function () {
    jsfn_BuildDataTable();



    $('#dvProfileInformation').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Profile Information</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#dvAssessmentRuleInformation').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Assessment Rule Information</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });
});




function jsfn_BuildDataTable() {
    vCompanyDataTable = $("#tbCompany").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Company/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.RIN = $("#txtCompanyRIN").val();
                data.companyName = $("#txtCompanyName").val();
                data.TIN = $("#txtCompanyTIN").val();
                data.MobileNum1 = $("#txtMobileNum1").val();
                data.MobileNum2 = $("#txtMobileNum2").val();
                data.EmailAddress1 = $("#txtEmailAddress1").val();
                data.EmailAddress2 = $("#txtEmailAddress2").val();
                data.TaxOffice = $("#txtTaxOffice").val();
                data.TaxPayerType = $("#txtPayerType").val();
                data.EconomicActivities = $("#txtEconomicActivity").val();
                data.NotificationMethod = $("#txtNotificationMethod").val();
                data.ActiveText = $("#txtStatus").val();
            }
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",
        "columns": [
            { "data": "CompanyRIN", "orderable": true, "name": "CompanyRIN" },
            { "data": "CompanyName", "orderable": true, "name": "CompanyName" },
            { "data": "TIN", "orderable": true, "name": "TIN" },
            { "data": "MobileNumber1", "orderable": true, "name": "MobileNumber1" },
            {
                "data": "MobileNumber2", "orderable": true, "name": "MobileNumber2", "visible": false
            },
            { "data": "EmailAddress1", "orderable": true, "name": "EmailAddress1" },
            { "data": "EmailAddress2", "orderable": true, "name": "EmailAddress2" },
            { "data": "TaxOfficeName", "orderable": true, "name": "TaxOfficeName" },
            { "data": "TaxPayerTypeName", "orderable": true, "name": "TaxPayerTypeName" },
            { "data": "EconomicActivitiesName", "orderable": true, "name": "EconomicActivitiesName" },
            { "data": "NotificationMethodName", "orderable": true, "name": "NotificationMethodName" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, com) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/Company/Details/' + com.CompanyID + '/' + com.CompanyRIN.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a href="/Company/Edit/' + com.CompanyID + '/' + com.CompanyRIN.toSeoUrl() + '">Edit Details</a></li>'
                        + '<li><a href="/Company/AddressInformationList/' + com.CompanyID + '/' + com.CompanyRIN.toSeoUrl() + '">Address Information</a></li>'
                        + '<li><a href="/Company/AssetList/' + com.CompanyID + '/' + com.CompanyRIN.toSeoUrl() + '">Asset Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ShowProfileInformation(' + com.CompanyID + ',' + com.CompanyRIN + ',\'' + com.CompanyName + '\')">Profile Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ShowAssessmentRuleInformation(' + com.CompanyID + ',' + com.CompanyRIN + ',\'' + com.CompanyName + '\')">Assessment Rule Information</a></li>'
                        + '<li><a href="/Company/AssessmentList/' + com.CompanyID + '/' + com.CompanyRIN.toSeoUrl() + '">Assessment Information</a></li>'
                        + '<li><a href="/Company/ServiceBillList/' + com.CompanyID + '/' + com.CompanyRIN.toSeoUrl() + '">Service Bill Information</a></li>'
                        + '<li><a href="/Company/SettlementList/' + com.CompanyID + '/' + com.CompanyRIN.toSeoUrl() + '">Settlement Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + com.CompanyID + ')">' + (com.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],
        "order": [[1, "asc"]]
    });

    
    $("#tbCompany thead th input[type=text]").on('change', function () {
        vCompanyDataTable
            .column($(this.data).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });

    //Hide Columns

    vCompanyDataTable.column(6).visible(false);
    vCompanyDataTable.column(8).visible(false);
}


function jsfn_ChangeStatus(Companyid) {
    var vData = {
        CompanyID: Companyid
    };
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Company/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vCompanyDataTable.draw();

    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowProfileInformation(compId, comprin, compname) {
    $('#dvPITaxPayerRIN').html(comprin);
    $('#dvPITaxPayerName').html(compname);
    var vData = {
        CompanyID: compId,
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Company/GetProfileInformation', vData, jsfn_ProfileInformationResponse);
}

function jsfn_ProfileInformationResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvProfileList').html(data.ProfileInformationList);
        jsfn_ConvertToDatatable($('#tbProfileInformation'));
        $('#dvProfileInformation').dialog('open');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowAssessmentRuleInformation(compId, comprin, compname) {
    $('#dvARITaxPayerRIN').html(comprin);
    $('#dvARITaxPayerName').html(compname);
    var vData = {
        CompanyID: compId,
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Company/GetAssessmentRuleInformation', vData, jsfn_AssessmentRuleInformationResponse);
}

function jsfn_AssessmentRuleInformationResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentRuleList').html(data.AssessmentRuleInformationList);
        jsfn_ConvertToDatatable($('#tbAssessmentRuleInformation'));
        $('#dvAssessmentRuleInformation').dialog('open');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}



