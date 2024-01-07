var vGovernmentDataTable;

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

function jsfn_ChangeStatus(Governmentid) {
    var vData = {
        GovernmentID: Governmentid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Government/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vGovernmentDataTable.draw();
       
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BuildDataTable() {
    vGovernmentDataTable = $("#tbGovernment").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Government/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.RIN = $("#txtGovernmentRIN").val();
                data.GovernmentName = $("#txtGovernmentName").val();
                data.TIN = $("#txtGovernmentTIN").val();
                data.GovernmentTypeName = $("#txtGovernmentType").val();
                data.TaxOffice = $("#txtTaxOffice").val();
                data.TaxPayerType = $("#txtTaxPayerType").val();
                data.ContactName = $("#txtContactName").val();
                data.ContactEmail = $("#txtContactEmailAddress").val();
                data.ContactNumber = $("#txtContactNumber").val();
                data.NotificationMethod = $("#txtNotificationMethod").val();
                data.ActiveText = $("#txtStatus").val();
            },
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "GovernmentRIN", "orderable": true, "name": "GovernmentRIN" },
            { "data": "GovernmentName", "orderable": true, "name": "GovernmentName" },
            { "data": "TIN", "orderable": true, "name": "TIN" },
            { "data": "GovernmentTypeName", "orderable": true, "name": "GovernmentTypeName" },
            { "data": "TaxOfficeName", "orderable": true, "name": "TaxOfficeName" },
            { "data": "TaxPayerTypeName", "orderable": true, "name": "TaxPayerTypeName" },
            { "data": "ContactName", "orderable": true, "name": "ContactName" },
            { "data": "ContactEmail", "orderable": true, "name": "ContactEmail" },
            { "data": "ContactNumber", "orderable": true, "name": "ContactNumber" },
            { "data": "NotificationMethodName", "orderable": true, "name": "NotificationMethodName" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, gov) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/Government/Details/' + gov.GovernmentID + '/' + gov.GovernmentRIN.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a href="/Government/Edit/' + gov.GovernmentID + '/' + gov.GovernmentRIN.toSeoUrl() + '">Edit Details</a></li>'
                        + '<li><a href="/Government/AddressInformationList/' + gov.GovernmentID + '/' + gov.GovernmentRIN.toSeoUrl() + '">Address Information</a></li>'
                        + '<li><a href="/Government/AssetList/' + gov.GovernmentID + '/' + gov.GovernmentRIN.toSeoUrl() + '">Asset Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ShowProfileInformation(' + gov.GovernmentID + ',' + gov.GovernmentRIN + ',\'' + gov.GovernmentName + '\')">Profile Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ShowAssessmentRuleInformation(' + gov.GovernmentID + ',' + gov.GovernmentRIN + ',\'' + gov.GovernmentName + '\')">Assessment Rule Information</a></li>'
                        + '<li><a href="/Government/AssessmentList/' + gov.GovernmentID + '/' + gov.GovernmentRIN.toSeoUrl() + '">Assessment Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + gov.GovernmentID + ')">' + (gov.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],

        "order": [[1, "asc"]]
    });

    $("#tbGovernment thead th input[type=text]").on('change', function () {
        vGovernmentDataTable
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });
    //Hide Columns
    vGovernmentDataTable.column(4).visible(false);
    vGovernmentDataTable.column(6).visible(false);
    vGovernmentDataTable.column(8).visible(false);
}

function jsfn_ShowProfileInformation(compId, comprin, compname) {
    $('#dvPITaxPayerRIN').html(comprin);
    $('#dvPITaxPayerName').html(compname);
    var vData = {
        GovernmentID: compId,
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Government/GetProfileInformation', vData, jsfn_ProfileInformationResponse);
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
        GovernmentID: compId,
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Government/GetAssessmentRuleInformation', vData, jsfn_AssessmentRuleInformationResponse);
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

