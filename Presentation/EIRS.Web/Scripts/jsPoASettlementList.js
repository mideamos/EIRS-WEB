jQuery(document).ready(function () {
    $('#tbPOASettlement').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "responsive": true,
        "ajax": {
            "url": "/OperationManager/PoASettlementData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [{
            "data": "BillDate",
            "orderable": true,
            "name": "BillDate"
        }, {
            "data": "BillRefNo",
            "orderable": true,
            "name": "BillRefNo"
        }, {
            "data": "TaxPayerName",
            "orderable": true,
            "name": "TaxPayerName"
        }, {
            "data": "BillAmount",
            "orderable": true,
            "name": "BillAmount"
        }, {
            "data": "SettlementStatusName",
            "orderable": true,
            "name": "SettlementStatusName"
        }, {
            "data": "BillID",
            "orderable": false,
            "name": "BillID"
        }],
        "order": [[0, "desc"]],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var vBillDate = new Date(parseInt(aData["BillDate"].substr(6)));
            $('td:eq(0)', nRow).html(vBillDate.format("dd-mmm-yyyy"));
            $('td:eq(3)', nRow).html(aData["BillAmount"].formatMoney());
            $('td:eq(5)', nRow).html('<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                + '</button><ul class="dropdown-menu"><li><a href="' + jsfn_GetBillDetailUrl(aData["BillTypeID"], aData["BillID"], aData["BillRefNo"]) + '">'
                + 'View Bill</a></li></ul></div>');
        }
    });
});


function jsfn_GetBillDetailUrl(BillTypeID, BillID, BillRefNo) {
    if (BillTypeID === 1) {
        return '/OperationManager/AssessmentBillDetail?id=' + BillID + '&name=' + toSeoUrl(BillRefNo);
    }
    else if (BillTypeID === 2) {
        return '/OperationManager/ServiceBillDetail?id=' + BillID + '&name=' + toSeoUrl(BillRefNo);
    }

}

