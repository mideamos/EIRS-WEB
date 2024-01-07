$(document).ready(function () {
    $('#dvSearchData').hide();

    $("#btnSearch").click(function () {
        $('#dvSearchData').show();
        jsfn_bindTable();
        return false;
    });
});

var vTaxOfficerMonthlyPayments;
function jsfn_bindTable() {
    if (vTaxOfficerMonthlyPayments !== undefined) {
        vTaxOfficerMonthlyPayments.draw();
    }
    else {
        vTaxOfficerMonthlyPayments = $("#tblTaxOfficerMonthlyPayments").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/Staff/TaxOfficerMonthlyPaymentLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.Year = $("#cboYear").val() !== '' ? $("#cboYear").val() : '0';
                }, "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "StartMonthName", "orderable": true, "name": "StartMonthName", "width": "25%" },
                {
                    "data": "Settlement", "orderable": true, "name": "Settlement", "width": "25%", "render": function (data, type, st) {
                        return st.Settlement.formatMoney();
                    }
                },
                {
                    "data": "PoA", "orderable": true, "name": "PoA", "width": "25%", "render": function (data, type, st) {
                        return st.PoA.formatMoney();
                    }
                },
                {
                    "data": "TotalPayment", "orderable": true, "name": "TotalPayment", "width": "25%", "render": function (data, type, st) {
                        return st.TotalPayment.formatMoney();
                    }
                }
            ],
            "order": [[1, "asc"]]
        });
    }
}