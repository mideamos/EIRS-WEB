$(document).ready(function () {
    $("#tblAssetList").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/AssetTypeByTaxOfficeDetailLoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.TaxOfficeID = $("#hdnTaxOfficeID").val();
            },
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "AssetTypeName", "orderable": true, "name": "AssetTypeName", "width": "25%" },
            { "data": "AssetName", "orderable": true, "name": "AssetName", "width": "25%" },
            { "data": "AssetRIN", "orderable": true, "name": "AssetRIN", "width": "25%" },
            { "data": "AssetLGA", "orderable": true, "name": "AssetLGA", "width": "25%" },
        ],
        "order": [[1, "asc"]]
    });
});
