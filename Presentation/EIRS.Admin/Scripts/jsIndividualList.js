var vIndividualDataTable;

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

    $("#txtDateOfBirth").datepicker({
        autoclose: true
    })
});

function jsfn_BuildDataTable() {
    vIndividualDataTable = $("#tbIndividual").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Individual/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.RIN = $("#txtUserRIN").val();
                data.GenderName = $("#txtGender").val();
                data.TitleName = $("#txtTitle").val();
                data.FirstName = $("#txtFirstName").val();
                data.MiddleName = $("#txtMiddleName").val();
                data.LastName = $("#txtLastName").val();
                data.DOB = $("#txtDateOfBirth").val();
                data.TIN = $("#txtTIN").val();
                data.MobileNum1 = $("#txtMobileNumber1").val();
                data.MobileNum2 = $("#txtMobileNumber2").val();
                data.EmailAddress1 = $("#txtEmailAddress1").val();
                data.EmailAddress2 = $("#txtEmailAddress2").val();
                data.BiometricDetails = $("#txtBiometricDetails").val();
                data.TaxOfficeName = $("#txtTaxOffice").val();
                data.MaritalStatusName = $("#txtMaritalStatus").val();
                data.NationalityName = $("#txtNationality").val();
                data.TaxPayerType = $("#txtTaxPayerType").val();
                data.EconomicActivities = $("#txtEconomicActivity").val();
                data.NotificationMethod = $("#txtNotificationMethod").val();
                data.ActiveText = $("#txtStatus").val();
            },
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "IndividualRIN", "orderable": true, "name": "IndividualRIN" },
            { "data": "GenderName", "orderable": true, "name": "GenderName" },
            { "data": "TitleName", "orderable": true, "name": "TitleName" },
            { "data": "FirstName", "orderable": true, "name": "FirstName" },
            { "data": "LastName", "orderable": true, "name": "LastName" },
            { "data": "MiddleName", "orderable": true, "name": "MiddleName" },
            {
                "data": "DOB", "orderable": true, "name": "DOB", "width": "25%", "render": function (data, type, ind) {
                    //var vDOB = new Date(parseInt(ind.DOB.substr(6)));
                    if (ind.DOB != undefined || ind.DOB != null) {
                        return jsfn_FormatJsonDate(ind.DOB, "dd-mmm-yy");
                        //return vDOB.format("dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            { "data": "TIN", "orderable": true, "name": "TIN" },
            { "data": "MobileNumber1", "orderable": true, "name": "MobileNumber1" },
            { "data": "MobileNumber2", "orderable": true, "name": "MobileNumber2" },
            { "data": "EmailAddress1", "orderable": true, "name": "EmailAddress1" },
            { "data": "EmailAddress2", "orderable": true, "name": "EmailAddress2" },
            { "data": "BiometricDetails", "orderable": true, "name": "BiometricDetails" },
            { "data": "TaxOfficeName", "orderable": true, "name": "TaxOfficeName" },
            { "data": "MaritalStatusName", "orderable": true, "name": "MaritalStatusName" },
            { "data": "NationalityName", "orderable": true, "name": "NationalityName" },
            { "data": "TaxPayerTypeName", "orderable": true, "name": "TaxPayerTypeName" },
            { "data": "EconomicActivitiesName", "orderable": true, "name": "EconomicActivitiesName" },
            { "data": "NotificationMethodName", "orderable": true, "name": "NotificationMethodName" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, ind) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/Individual/Details/' + ind.IndividualID + '/' + ind.IndividualRIN.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a href="/Individual/Edit/' + ind.IndividualID + '/' + ind.IndividualRIN.toSeoUrl() + '">Edit Details</a></li>'
                        + '<li><a href="/Individual/AddressInformationList/' + ind.IndividualID + '/' + ind.IndividualRIN.toSeoUrl() + '">Address Information</a></li>'
                        + '<li><a href="/Individual/AssetList/' + ind.IndividualID + '/' + ind.IndividualRIN.toSeoUrl() + '">Asset Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ShowProfileInformation(' + ind.IndividualID + ',\'' + ind.IndividualRIN + '\',\'' + ind.FirstName + ind.LastName + '\')">Profile Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ShowAssessmentRuleInformation(' + ind.IndividualID + ',\'' + ind.IndividualRIN + '\',\'' + ind.FirstName + ind.LastName + '\')">Assessment Rule Information</a></li>'
                        + '<li><a href="/Individual/AssessmentList/' + ind.IndividualID + '/' + ind.IndividualRIN.toSeoUrl() + '">Assessment Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + ind.IndividualID + ')">' + (ind.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],

        "order": [[0, "asc"]]
    });

    $("#tbIndividual thead th input[type=text]").on('change', function () {
        vIndividualDataTable
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });

    //Hide Columns
    vIndividualDataTable.column(1).visible(false);
    vIndividualDataTable.column(2).visible(false);
    vIndividualDataTable.column(5).visible(false);
    vIndividualDataTable.column(6).visible(false);
    vIndividualDataTable.column(7).visible(false);
    vIndividualDataTable.column(9).visible(false);
    vIndividualDataTable.column(11).visible(false);
    vIndividualDataTable.column(12).visible(false);
    vIndividualDataTable.column(14).visible(false);
    vIndividualDataTable.column(15).visible(false);
    vIndividualDataTable.column(16).visible(false);
    vIndividualDataTable.column(17).visible(false);
}




function jsfn_ChangeStatus(Individualid) {
    var vData = {
        IndividualID: Individualid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Individual/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vIndividualDataTable.draw();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowProfileInformation(indId,indrin,indname) {
    $('#dvPITaxPayerRIN').html(indrin);
    $('#dvPITaxPayerName').html(indname);
    var vData = {
        IndividualID: indId,
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Individual/GetProfileInformation', vData, jsfn_ProfileInformationResponse);
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

function jsfn_ShowAssessmentRuleInformation(indId, indrin, indname) {
    $('#dvARITaxPayerRIN').html(indrin);
    $('#dvARITaxPayerName').html(indname);
    var vData = {
        IndividualID: indId,
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Individual/GetAssessmentRuleInformation', vData, jsfn_AssessmentRuleInformationResponse);
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
