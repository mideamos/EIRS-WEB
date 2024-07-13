var vMDAServiceItemDataTable;

jQuery(document).ready(function () {
    jsfn_BuildDataTable();
});

function jsfn_ChangeStatus(MDAServiceItemid) {
    var vData = {
        MDAServiceItemID: MDAServiceItemid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/MDAServiceItem/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vMDAServiceItemDataTable.draw();
      
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BuildDataTable() {
    vMDAServiceItemDataTable = $("#tbMDAServiceItem").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/MDAServiceItem/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.MDAServiceItemReferenceNo = $("#txtMDAServiceItemReferenceNo").val();
                data.RevenueStreamName = $("#txtRevenueStramName").val();
                data.RevenueSubStreamName = $("#txtRevenueSubStramName").val();
                data.AssessmentItemCategoryName = $("#txtItemCategoryName").val();
                data.AssessmentItemSubCategoryName = $("#txtItemSubCategoryName").val();
                data.AgencyName = $("#txtRevenueAgencyName").val();
                data.MDAServiceItemName = $("#txtMDAServiceItemName").val();
                data.ComputationName = $("#txtComputationName").val();
                data.ServiceAmount = $("#txtServiceAmount").val();
                data.Percentage = $("#txtPercentage").val();
                data.ServiceBaseAmount = $("#txtServiceBaseAmount").val();
                data.ActiveText = $("#txtStatus").val();
            },
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",
        "columns": [
            { "data": "MDAServiceItemReferenceNo", "orderable": true, "name": "MDAServiceItemReferenceNo" },
            { "data": "RevenueStreamName", "orderable": true, "name": "RevenueStreamName" },
            { "data": "RevenueSubStreamName", "orderable": true, "name": "RevenueSubStreamName" },
            { "data": "AssessmentItemCategoryName", "orderable": true, "name": "AssessmentItemCategoryName" },
            { "data": "AssessmentItemSubCategoryName", "orderable": true, "name": "AssessmentItemSubCategoryName" },
            { "data": "AgencyName", "orderable": true, "name": "AgencyName" },
            { "data": "MDAServiceItemName", "orderable": true, "name": "MDAServiceItemName" },
            { "data": "ComputationName", "orderable": true, "name": "ComputationName" },
            {
                "data": "ServiceBaseAmount", "orderable": true, "name": "ServiceBaseAmount",
                "render": function (data, type, MDAservitem) {
                    return MDAservitem.ServiceBaseAmount.formatMoney();
                }
            },
            { "data": "Percentage", "orderable": true, "name": "Percentage" },
            {
                "data": "ServiceAmount", "orderable": true, "name": "ServiceAmount", "render": function (data, type, MDAservitem) {
                    return MDAservitem.ServiceAmount.formatMoney();
                }
            },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, MDAservitem) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/MDAServiceItem/Details/' + MDAservitem.MDAServiceItemID + '/' + MDAservitem.MDAServiceItemName.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a href="/MDAServiceItem/Edit/' + MDAservitem.MDAServiceItemID + '/' + MDAservitem.MDAServiceItemName.toSeoUrl() + '">Edit Details</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + MDAservitem.MDAServiceItemID + ')">' + (MDAservitem.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],

        "order": [[1, "asc"]]
    });

    $("#tbMDAServiceItem thead th input[type=text]").on('change', function () {
        vMDAServiceItemDataTable
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });
    //Hide Columns
    vMDAServiceItemDataTable.column(1).visible(false);
    vMDAServiceItemDataTable.column(2).visible(false);
    vMDAServiceItemDataTable.column(3).visible(false);
    vMDAServiceItemDataTable.column(4).visible(false);
    vMDAServiceItemDataTable.column(5).visible(false);
    vMDAServiceItemDataTable.column(9).visible(false);
}

