var vBusinessDataTable;

jQuery(document).ready(function () {
    jsfn_BuildDataTable();
});

function jsfn_ChangeStatus(Businessid) {
    var vData = {
        BusinessID: Businessid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Business/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBusinessDataTable.draw();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BuildDataTable() {
    vBusinessDataTable = $("#tbBusiness").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Business/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.BusinessRIN = $("#txtBusinessRIN").val();
                data.BusinessName = $("#txtBusinessName").val();
                data.BusinessTypeName = $("#txtBusinessType").val();
                data.LGAName = $("#txtLGAName").val();
                data.BusinessCategoryName = $("#txtBusinessCategoryName").val();
                data.BusinessSectorName = $("#txtBusinessSectorName").val();
                data.BusinessSubSectorName = $("#txtBusinessSubSectorName").val();
                data.BusinessStructureName = $("#txtBusinessStructureName").val();
                data.BusinessOperationName = $("#txtBusinessOperationName").val();
                data.SizeName = $("#txtSizeName").val();
                data.ActiveText = $("#txtStatus").val();
            }
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "BusinessRIN", "orderable": true, "name": "BusinessRIN" },
            { "data": "BusinessName", "orderable": true, "name": "BusinessName" },
            { "data": "BusinessTypeName", "orderable": true, "name": "BusinessTypeName" },
            { "data": "LGAName", "orderable": true, "name": "LGAName" },
            { "data": "BusinessCategoryName", "orderable": true, "name": "BusinessCategoryName" },
            { "data": "BusinessSectorName", "orderable": true, "name": "BusinessSectorName" },
            { "data": "BusinessSubSectorName", "orderable": true, "name": "BusinessSubSectorName" },
            { "data": "BusinessStructureName", "orderable": true, "name": "BusinessStructureName" },
            { "data": "BusinessOperationName", "orderable": true, "name": "BusinessOperationName" },
            { "data": "SizeName", "orderable": true, "name": "SizeName" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, buss) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/Business/Details/' + buss.BusinessID + '/' + buss.BusinessRIN.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a href="/Business/Edit/' + buss.BusinessID + '/' + buss.BusinessRIN.toSeoUrl() + '">Edit Details</a></li>'
                        + '<li><a href="/Business/TaxPayerList/' + buss.BusinessID + '/' + buss.BusinessRIN.toSeoUrl() + '">Tax Payer Information</a></li>'
                        + '<li><a href="/Business/BuildingInformation/' + buss.BusinessID + '/' + buss.BusinessRIN.toSeoUrl() + '">Building Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + buss.BusinessID + ')">' + (buss.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],
        "order": [[1, "asc"]]
    });


    $("#tbBusiness thead th input[type=text]").on('change', function () {
        vBusinessDataTable
            .column($(this.data).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });

    //Hide Columns
    vBusinessDataTable.column(2).visible(false);
    vBusinessDataTable.column(3).visible(false);
    vBusinessDataTable.column(6).visible(false);
    vBusinessDataTable.column(8).visible(false);
    vBusinessDataTable.column(9).visible(false);
}

