$(document).ready(function () {
    $('#dvSearchData').hide();

    $("#btnSearch").click(function () {
        $('#dvSearchData').show();
        jsfn_bindTable();
        return false;
    });

    $("#btnExportToExcel").click(function () {
        var vBillType = $("#cboBillType").val() !== '' ? $("#cboBillType").val() : '0';
        var vStageID = $("#cboStageList").val() !== '' ? $("#cboStageList").val() : '0';

        var vUrl = '/OperationManager/BillAgingByRevenueStreamExportToExcel?BillTypeID=' + vBillType + '&StageID=' + vStageID;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });
});

var vBillAgingByRevenueStream;
function jsfn_bindTable() {
    if (vBillAgingByRevenueStream !== undefined) {
        vBillAgingByRevenueStream.draw();
    }
    else {
        vBillAgingByRevenueStream = $("#tblBillAgingByRevenueStream").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/BillAgingByRevenueStreamLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.BillTypeID = $("#cboBillType").val() !== '' ? $("#cboBillType").val() : '0';
                    data.StageID = $("#cboStageList").val() !== '' ? $("#cboStageList").val() : '0';
                },
                "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "RevenueStreamName", "orderable": true, "name": "RevenueStreamName" },
                {
                    "data": "Amount", "orderable": true, "name": "Amount", "render": function (data, type, st) {
                        return st.Amount.formatMoney();
                    }
                },
                { "data": "TransactionCount", "orderable": true, "name": "TransactionCount" },
                {
                    "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                            + '</button><ul class="dropdown-menu">'
                            + '<li><a href="/OperationManager/BillAgingDetailsByRevenueStream?btypeId=' + ($("#cboBillType").val() !== '' ? $("#cboBillType").val() : '0') + '&stgid=' + ($("#cboStageList").val() !== '' ? $("#cboStageList").val() : '0') + '&rstrmID=' + st.RevenueStreamID + '">View Details</a></li>'
                            + '<li><a target="_blank" href="/OperationManager/BillAgingDetailsByRevenueStreamExportToExcel?BillTypeID=' + ($("#cboBillType").val() !== '' ? $("#cboBillType").val() : '0') + '&StageID=' + ($("#cboStageList").val() !== '' ? $("#cboStageList").val() : '0') + '&RevenueStreamID=' + st.RevenueStreamID + '">Export Details</a></li>'
                            + '</ul></div>';
                    }, "width": "10%"
                },
            ],
            "order": [[0, "asc"]],
            "footerCallback": function (row, ddata, start, end, display) {
                var vFooterTotal = this.api().ajax.json().FooterTotal;
                if (vFooterTotal) {
                    var $th = $(row).find('th');
                    $th.eq(1).html(vFooterTotal['Amount'].formatMoney());
                    $th.eq(2).html(vFooterTotal['TransactionCount']);
                }
            }
        });
    }
}












