var vProfileDataTable;

jQuery(document).ready(function () {
    jsfn_BuildDataTable();
});


function jsfn_BuildDataTable() {
    vProfileDataTable = $("#tbProfile").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Profile/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.ProfileReferenceNo = $("#txtProfileReferenceNo").val();
                data.AssetTypeName = $("#txtAssetType").val();
                data.ProfileSectorName = $("#txtProfileSector").val();
                data.ProfileSubSectorName = $("#txtProfileSubSector").val();
                data.ProfileGroupName = $("#txtProfileGroup").val();
                data.ProfileSubGroupName = $("#txtProfileSubGroup").val();
                data.ProfileSectorElementName = $("#txtProfileSectorElement").val();
                data.ProfileSectorSubElementName = $("#txtProfileSectorSubElement").val();
                data.ProfileAttributeName = $("#txtProfileAttribute").val();
                data.ProfileSubAttributeName = $("#txtProfileSubAttribute").val();
                data.TaxPayerTypeName = $("#txtTaxPayerType").val();
                data.TaxPayerRoleName = $("#txtTaxPayerRole").val();
                data.ProfileDescription = $("#txtProfileDescription").val();
                data.ActiveText = $("#txtStatus").val();
            },
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",
        "columns": [
            { "data": "ProfileReferenceNo", "orderable": true, "name": "ProfileReferenceNo" },
            { "data": "AssetTypeName", "orderable": true, "name": "AssetTypeName" },
            { "data": "ProfileSectorNames", "orderable": true, "name": "ProfileSectorNames" },
            { "data": "ProfileSubSectorNames", "orderable": true, "name": "ProfileSubSectorNames" },
            { "data": "ProfileGroupNames", "orderable": true, "name": "ProfileGroupNames" },
            { "data": "ProfileSubGroupNames", "orderable": true, "name": "ProfileSubGroupNames" },
            { "data": "ProfileSectorElementNames", "orderable": true, "name": "ProfileSectorElementNames" },
            { "data": "ProfileSectorSubElementNames", "orderable": true, "name": "ProfileSectorSubElementNames" },
            { "data": "ProfileAttributeNames", "orderable": true, "name": "ProfileAttributeNames" },
            { "data": "ProfileSubAttributeNames", "orderable": true, "name": "ProfileSubAttributeNames" },
            { "data": "TaxPayerTypeNames", "orderable": true, "name": "TaxPayerTypeNames" },
            { "data": "TaxPayerRoleNames", "orderable": true, "name": "TaxPayerRoleNames" },
            { "data": "ProfileDescription", "orderable": true, "name": "ProfileDescription" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, pro) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/Profile/Details/' + pro.ProfileID + '/' + pro.ProfileReferenceNo.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a href="/Profile/Edit/' + pro.ProfileID + '/' + pro.ProfileReferenceNo.toSeoUrl() + '">Edit Details</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + pro.ProfileID + ')">' + (pro.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],

        "order": [[1, "asc"]]
    });

    $("#tbProfile thead th input[type=text]").on('keyup change', function () {
        vProfileDataTable
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });
    //Hide Columns
    vProfileDataTable.column(2).visible(false);
    vProfileDataTable.column(3).visible(false);
    vProfileDataTable.column(4).visible(false);
    vProfileDataTable.column(5).visible(false);
    vProfileDataTable.column(6).visible(false);
    vProfileDataTable.column(7).visible(false);
    vProfileDataTable.column(8).visible(false);
    vProfileDataTable.column(9).visible(false);
    vProfileDataTable.column(11).visible(false);
}

function jsfn_ChangeStatus(Profileid) {
    var vData = {
        ProfileID: Profileid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Profile/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vProfileDataTable.draw();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
