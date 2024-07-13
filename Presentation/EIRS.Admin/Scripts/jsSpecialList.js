var vSpecialDataTable;

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

function jsfn_ChangeStatus(Specialid) {
    var vData = {
        SpecialID: Specialid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Special/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vSpecialDataTable.draw();
       
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BuildDataTable() {
    vSpecialDataTable = $("#tbSpecial").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Special/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.RIN = $("#txtSpecialRIN").val();
                data.SpecialTaxPayerName = $("#txtSpecialTaxPayerName").val();
                data.TIN = $("#txtSpecialTIN").val();
                data.TaxOffice = $("#txtTaxOffice").val();
                data.TaxPayerType = $("#txtTaxPayerType").val();
                data.ContactName = $("#txtContactName").val();
                data.ContactEmail = $("#txtContactEmailAddress").val();
                data.ContactNumber = $("#txtContactNumber").val();
                data.Description = $("#txtDescription").val();
                data.NotificationMethodName = $("#txtNoitificationMethodName").val();
                data.ActiveText = $("#txtStatus").val();
            },
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",
        "columns": [
            { "data": "SpecialRIN", "orderable": true, "name": "SpecialRIN" },
            { "data": "SpecialTaxPayerName", "orderable": true, "name": "SpecialTaxPayerName" },
            { "data": "TIN", "orderable": true, "name": "TIN" },
            { "data": "TaxOfficeName", "orderable": true, "name": "TaxOfficeName" },
            { "data": "TaxPayerTypeName", "orderable": true, "name": "TaxPayerTypeName" },
            { "data": "ContactName", "orderable": true, "name": "ContactName" },
            { "data": "ContactEmail", "orderable": true, "name": "ContactEmail" },
            { "data": "ContactNumber", "orderable": true, "name": "ContactNumber" },
            { "data": "Description", "orderable": true, "name": "Description" },
            { "data": "NotificationMethodName", "orderable": true, "name": "NotificationMethodName" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, spec) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/Special/Details/' + spec.SpecialID + '/' + spec.SpecialRIN.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a href="/Special/Edit/' + spec.SpecialID + '/' + spec.SpecialRIN.toSeoUrl() + '">Edit Details</a></li>'
                        + '<li><a href="/Special/AddressInformationList/' + spec.SpecialID + '/' + spec.SpecialRIN.toSeoUrl() + '">Address Information</a></li>'
                        + '<li><a href="/Special/AssetList/' + spec.SpecialID + '/' + spec.SpecialRIN.toSeoUrl() + '">Asset Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ShowProfileInformation(' + spec.SpecialID + ',\'' + spec.SpecialRIN + '\',\'' + spec.SpecialTaxPayerName + '\')">Profile Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ShowAssessmentRuleInformation(' + spec.SpecialID + ',\'' + spec.SpecialRIN + '\',\'' + spec.SpecialTaxPayerName + '\')">Assessment Rule Information</a></li>'
                        + '<li><a href="/Special/AssessmentList/' + spec.SpecialID + '/' + spec.SpecialRIN.toSeoUrl() + '">Assessment Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + spec.SpecialID + ')">' + (spec.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],

        "order": [[1, "asc"]]
    });

    $("#tbSpecial thead th input[type=text]").on('keyup change', function () {
        vSpecialDataTable
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });

    //Hide Columns
    vSpecialDataTable.column(4).visible(false);
    vSpecialDataTable.column(6).visible(false);
    vSpecialDataTable.column(8).visible(false);
}

function jsfn_ShowProfileInformation(compId, comprin, compname) {
    $('#dvPITaxPayerRIN').html(comprin);
    $('#dvPITaxPayerName').html(compname);
    var vData = {
        SpecialID: compId,
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Special/GetProfileInformation', vData, jsfn_ProfileInformationResponse);
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
        SpecialID: compId,
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Special/GetAssessmentRuleInformation', vData, jsfn_AssessmentRuleInformationResponse);
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

