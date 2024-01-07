var vAssessmentItemDataTable;

jQuery(document).ready(function () {
    jsfn_BuildDataTable();
});

function jsfn_ChangeStatus(AssessmentItemid) {
    var vData = {
        AssessmentItemID: AssessmentItemid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/AssessmentItem/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vAssessmentItemDataTable.draw();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BuildDataTable() {
    vAssessmentItemDataTable = $("#tbAssessmentItem").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/AssessmentItem/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.AssessmentItemReferenceNo = $("#txtAssessmentItemRefNo").val();
                data.AssetTypeName = $("#txtAssetTypeName").val();
                data.AssessmentGroupName = $("#txtAssessmentGroupName").val();
                data.AssessmentSubGroupName = $("#txtAssessmentSubGroupName").val();
                data.RevenueStreamName = $("#txtRevenueStreamName").val();
                data.RevenueSubStreamName = $("#txtRevenueSubStreamName").val();
                data.AssessmentItemCategoryName = $("#txtItemCategory").val();
                data.AssessmentItemSubCategoryName = $("#txtItemSubCategory").val();
                data.AgencyName = $("#txtAgencyName").val();
                data.AssessmentItemName = $("#txtAssessmentItemName").val();
                data.ComputationName = $("#txtComputationName").val();
                data.TaxBaseAmount = $("#txtTaxBaseAmount").val();
                data.Percentage = $("#txtPercentage").val();
                data.TaxAmount = $("#txtTaxAmount").val();
                data.ActiveText = $("#txtStatus").val();
            },
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "AssessmentItemReferenceNo", "orderable": true, "name": "AssessmentItemReferenceNo" },
            { "data": "AssetTypeName", "orderable": true, "name": "AssetTypeName" },
            { "data": "AssessmentGroupName", "orderable": true, "name": "AssessmentGroupName" },
            { "data": "AssessmentSubGroupName", "orderable": true, "name": "AssessmentSubGroupName" },
            { "data": "RevenueStreamName", "orderable": true, "name": "RevenueStreamName" },
            { "data": "RevenueSubStreamName", "orderable": true, "name": "RevenueSubStreamName" },
            { "data": "AssessmentItemCategoryName", "orderable": true, "name": "AssessmentItemCategoryName" },
            { "data": "AssessmentItemSubCategoryName", "orderable": true, "name": "AssessmentItemSubCategoryName" },
            { "data": "AgencyName", "orderable": true, "name": "AgencyName" },
            { "data": "AssessmentItemName", "orderable": true, "name": "AssessmentItemName" },
            { "data": "ComputationName", "orderable": true, "name": "ComputationName" },
            {
                "data": "TaxBaseAmount", "orderable": true, "name": "TaxBaseAmount", "render": function (data, type, asitem) {
                    return asitem.TaxBaseAmount.formatMoney();
                }
            },
            {
                "data": "Percentage", "orderable": true, "name": "Percentage"
            },
            {
                "data": "TaxAmount", "orderable": true, "name": "TaxAmount", "render": function (data, type, asitem) {
                    return asitem.TaxAmount.formatMoney();
                }
            },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, asitem) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/AssessmentItem/Details/' + asitem.AssessmentItemID + '/' + asitem.AssessmentItemName.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a href="/AssessmentItem/Edit/' + asitem.AssessmentItemID + '/' + asitem.AssessmentItemName.toSeoUrl() + '">Edit Details</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + asitem.AssessmentItemID + ')">' + (asitem.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],

        "order": [[1, "asc"]]
    });

    $("#tbAssessmentItem thead th input[type=text]").on('change', function () {
        vAssessmentItemDataTable
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });

    //Hide Columns
    vAssessmentItemDataTable.column(2).visible(false);
    vAssessmentItemDataTable.column(3).visible(false);
    vAssessmentItemDataTable.column(4).visible(false);
    vAssessmentItemDataTable.column(5).visible(false);
    vAssessmentItemDataTable.column(6).visible(false);
    vAssessmentItemDataTable.column(7).visible(false);
    vAssessmentItemDataTable.column(8).visible(false);
    vAssessmentItemDataTable.column(12).visible(false);
    vAssessmentItemDataTable.column(14).visible(false);
    //vAssessmentItemDataTable.column(15).visible(false);
}

