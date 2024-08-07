var vRevenueStreamByTaxOfficeTarget;

function jsfn_bindTableII() {
    if (vRevenueStreamByTaxOfficeTarget !== undefined) {
        vRevenueStreamByTaxOfficeTarget.draw();
    } else {
        vRevenueStreamByTaxOfficeTarget = $("#tblRevenueStreamDetails").DataTable({
            "processing": true, 
            "serverSide": true, 
            "filter": true,
            "orderMulti": false,
            "ajax": {
                "url": "/OperationManager/RevenueStreamViewLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.RevenueStreamID = $("#cboRevenueStream").val() || '0';
                    data.AmountAssessed = $("#cboAmountAss").val() || '0';
                    data.TotalCollection = $("#cboTotalCollection").val() || '0';
                    data.Differential = $("#cboDifferential").val() || '0';
                    data.Performance = $("#cboPerformance").val() || '0';
                },
                "error": function (jqXHR) {
                    if (jqXHR.status === 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "AmountAssessed", "orderable": true, "name": "AmountAssessed", "width": "20%" },
                {
                    "data": "TargetAmount", "orderable": true, "name": "TargetAmount", "width": "20%", "render": function (data) {
                        return parseFloat(data).toFixed(2);
                    }
                },
                {
                    "data": "RevenueAmount", "orderable": true, "name": "RevenueAmount", "width": "20%", "render": function (data) {
                        return parseFloat(data).toFixed(2);
                    }
                },
                {
                    "data": "Differential", "orderable": true, "name": "Differential", "width": "20%", "render": function (data) {
                        return parseFloat(data).toFixed(2);
                    }
                },
                {
                    "data": "Performance", "orderable": true, "name": "Performance", "width": "20%", "render": function (data) {
                        return parseFloat(data).toFixed(2) + '%';
                    }
                },
                {
                    "data": null, "orderable": false, "width": "20%", "render": function (data, type, row) {
                        return '<button class="btn btn-primary" onclick="viewDetails(' + row.RevenueStreamID + ')">View Details</button>';
                    }
                }
            ],
            "order": [[1, "asc"]]
        });
    }
}

function viewDetails(id) {
    window.location.href = '/OperationManager/ViewDetails?RevenueStreamID=' + id;
}
