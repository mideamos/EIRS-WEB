$(document).ready(function () {
    $('#dvSearchData').hide();

    var vfrmTaxOfficerSummary = $("#frmTaxOfficerSummary");
    vfrmTaxOfficerSummary.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            TaxOffice: {
                required: true
            }
        },
        messages: {
            TaxOffice: {
                required: "Select Tax Office"
            }
        },
        errorPlacement: function (error, element) {
            if (element.is('select')) {
                error.insertAfter(element[0].parentElement);
            }
            else {
                error.insertAfter(element);
            }
        },
        highlight: function (element) { // hightlight error inputs
            $(element).closest('.input').addClass('error'); // set error class to the control group
        },
        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.input').removeClass('error'); // set error class to the control group
        }
    });

    $("#btnSearch").click(function () {
        if (vfrmTaxOfficerSummary.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });

    $("#btnExportToExcel").click(function () {
        var vTaxOfficeID = $("#cboTaxOffice").val();

        var vUrl = '/OperationManager/TaxOfficerSummaryExportToExcel?TaxOfficeID=' + vTaxOfficeID;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });

});

var vTaxOfficerSummary;
function jsfn_bindTable() {
    if (vTaxOfficerSummary !== undefined) {
        vTaxOfficerSummary.draw();
    }
    else {
        vTaxOfficerSummary = $("#tblTaxOfficerSummary").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/TaxOfficerSummaryLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.TaxOfficeID = $("#cboTaxOffice").val();
                }, "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "TaxOfficerName", "orderable": true, "name": "TaxOfficerName", "width": "25%" },
                { "data": "TotalTaxPayerCount", "orderable": true, "name": "TotalTaxPayerCount", "width": "25%" },
                {
                    "data": "TotalAssessmentAmount", "orderable": true, "name": "TotalAssessmentAmount", "width": "25%", "render": function (data, type, st) {
                        return st.TotalAssessmentAmount.formatMoney();
                    }
                },
                {
                    "data": "TotalPaymentAmount", "orderable": true, "name": "TotalPaymentAmount", "width": "25%", "render": function (data, type, st) {
                        return st.TotalPaymentAmount.formatMoney();
                    }
                },
                {
                    "data": "OutstandingAmount", "orderable": true, "name": "OutstandingAmount", "width": "25%", "render": function (data, type, st) {
                        return st.OutstandingAmount.formatMoney();
                    }
                },
                {
                    "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                            + '</button><ul class="dropdown-menu"><li><a href="/OperationManager/TaxOfficerTaxPayerList?toid=' + $("#cboTaxOffice").val()+'&tofid=' + st.TaxOfficerID + '">'
                            + 'Tax Payer List</a></li></ul></div>';
                    }, "width": "10%"
                }
            ],
            "order": [[1, "asc"]]
        });
    }
}