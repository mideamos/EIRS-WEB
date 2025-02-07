﻿$(document).ready(function () {
    $('#dvSearchData').hide();

    var vfrmMonthlyTaxOfficeTarget = $("#frmMonthlyTaxOfficeTarget");
    vfrmMonthlyTaxOfficeTarget.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            Year: {
                required: true
            },
            TaxOffice: {
                required: true
            }
        },
        messages: {
            Year: {
                required: "Select Year"
            },
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
        if (vfrmMonthlyTaxOfficeTarget.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });
});

var vMonthlyTaxOfficeTarget;
function jsfn_bindTable() {
    if (vMonthlyTaxOfficeTarget !== undefined) {
        vMonthlyTaxOfficeTarget.draw();
    }
    else {
        vMonthlyTaxOfficeTarget = $("#tblMonthlyTaxOfficeTarget").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/MonthlyTaxOfficeTargetLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.TaxOfficeID = $("#cboTaxOffice").val() !== '' ? $("#cboTaxOffice").val() : '0';
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
                    "data": "TargetAmount", "orderable": true, "name": "TargetAmount", "width": "25%", "render": function (data, type, st) {
                        return st.TargetAmount.formatMoney();
                    }
                },
                {
                    "data": "AssessedAmount", "orderable": true, "name": "AssessedAmount", "width": "25%", "render": function (data, type, st) {
                        return st.AssessedAmount.formatMoney();
                    }
                },
                {
                    "data": "RevenueAmount", "orderable": true, "name": "RevenueAmount", "width": "25%", "render": function (data, type, st) {
                        return st.RevenueAmount.formatMoney();
                    }
                }
            ],
            "order": [[1, "asc"]]
        });
    }
}