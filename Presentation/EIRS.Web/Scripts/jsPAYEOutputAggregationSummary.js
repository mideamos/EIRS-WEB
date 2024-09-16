$(document).ready(function () {
    $('#dvSearchData').hide();
    

    var vfrmPAYEOutputAggregation = $("#frmPAYEOutputAggregation");
    vfrmPAYEOutputAggregation.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            Year: {
                required: true
            }
        },
        messages: {
            Year: {
                required: "Select Year"
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
        if (vfrmPAYEOutputAggregation.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });

    $("#btnExportToExcel").click(function () {
        var vYear = $("#cboYear").val();
        var vTaxOfficeID = $("#cboTaxOffice").val() !== '' ? $("#cboTaxOffice").val() : '0';
        
        var vUrl = '/OperationManager/PAYEOutputAggregationExportToExcel?TaxYear=' + vYear + '&TaxOfficeID=' + vTaxOfficeID;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });

});


var vPAYEOutputAggregation;
function jsfn_bindTable() {
    if (vPAYEOutputAggregation !== undefined) {
        vPAYEOutputAggregation.draw();
    }
    else {
        vPAYEOutputAggregation = $("#tblPAYEOutputAggregation").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/PAYEOutputAggregationLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.TaxYear = $("#cboYear").val() !== '' ? $("#cboYear").val() : '0';
                    data.TaxOfficeID = $("#cboTaxOffice").val() !== '' ? $("#cboTaxOffice").val() : '0';
                }, "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "EmployerRIN", "orderable": true, "name": "EmployerRIN" },
                { "data": "EmployerName", "orderable": true, "name": "EmployerName" },
                { "data": "EmployeeCount", "orderable": true, "name": "EmployeeCount" },
                {
                    "data": "AmountCollected", "orderable": true, "name": "AmountCollected", "render": function (data, type, st) {
                        return st.AmountCollected.formatMoney();
                    }
                },
                {
                    "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                            + '</button><ul class="dropdown-menu">'
                            + '<li><a href="/OperationManager/PAYEOutputAggregationDetail?tyear=' + $("#cboYear").val() + '&toffID=' + $("#cboTaxOffice").val() + '&erin=' + st.EmployerRIN + '">View Employee List</a></li>'
                            + '<li><a href="/OperationManager/PAYEOutputAggregationDetailExportToExcel?TaxYear=' + $("#cboYear").val() + '&TaxOfficeID=' + $("#cboTaxOffice").val() + '&EmployerRIN=' + st.EmployerRIN + '">Export Employee List</a></li>'
                            + '</ul></div>';
                    }, "width": "10%"
                },
            ],
            "order": [[0, "asc"]],
        });
    }
}


