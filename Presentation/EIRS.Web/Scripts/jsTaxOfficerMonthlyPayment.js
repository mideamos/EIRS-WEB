$(document).ready(function () {
    $('#dvSearchData').hide();

    $("#btnSearch").click(function () {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
    });

    $('#cboTaxOffice').on("change", jsfn_onTaxOfficeChange);
});

function jsfn_onTaxOfficeChange() {
    var vTaxOfficeID = $('#cboTaxOffice').val();

    if (vTaxOfficeID != '') {

        var vData = {
            TaxOfficeID: vTaxOfficeID == '' ? 0 : vTaxOfficeID,
            showmanager: false
        };

        jsfn_ajaxPost('/Base/GetTaxOfficeManger', vData, jsfn_onTaxOfficeChangeResponse);
    }
}

function jsfn_onTaxOfficeChangeResponse(data) {
    $("#cboTaxOfficer").html("");

    $("#cboTaxOfficer").append(
        $('<option></option>').val("").html("All Tax Officer"));

    if (data !== undefined) {
        $.each(data, function (i, tit) {
            $("#cboTaxOfficer").append(
                $('<option></option>').val(tit.id).html(tit.text));
        });

        $('#cboTaxOfficer').selectpicker('refresh');
    }
}

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
                "url": "/OperationManager/TaxOfficerMonthlyPaymentLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.TaxOfficeID = $("#cboTaxOffice").val() !== '' ? $("#cboTaxOffice").val() : '0';
                    data.TaxOfficerID = $("#cboTaxOfficer").val() !== '' ? $("#cboTaxOfficer").val() : '0';
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