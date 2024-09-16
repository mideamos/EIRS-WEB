$(document).ready(function () {
    $('#dvSearchData').hide();

    var vfrmTaxOfficeAssessmentSummary = $("#frmTaxOfficeAssessmentSummary");
    vfrmTaxOfficeAssessmentSummary.validate({
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
        if (vfrmTaxOfficeAssessmentSummary.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });

    $("#btnExportToExcel").click(function () {
        var vYear = $("#cboYear").val();
        
        var vUrl = '/OperationManager/TaxOfficeAssessmentSummaryExportToExcel?TaxYear=' + vYear;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });


    
});

var vTaxOfficeAssessmentSummary;
function jsfn_bindTable() {
    if (vTaxOfficeAssessmentSummary !== undefined) {
        vTaxOfficeAssessmentSummary.draw();
    }
    else {
        vTaxOfficeAssessmentSummary = $("#tblTaxOfficeAssessmentSummary").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/TaxOfficeAssessmentSummaryLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.TaxYear = $("#cboYear").val() !== '' ? $("#cboYear").val() : '0';
                }, "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "TaxOfficeName", "orderable": true, "name": "TaxOfficeName", "width": "25%" },
                {
                    "data": "BillAmount", "orderable": true, "name": "BillAmount", "width": "25%", "render": function (data, type, st) {
                        return st.BillAmount.formatMoney();
                    }
                },
                {
                    "data": "SettlementAmount", "orderable": true, "name": "SettlementAmount", "width": "25%", "render": function (data, type, st) {
                        return st.SettlementAmount.formatMoney();
                    }
                },
                {
                    "data": "PoAAmount", "orderable": true, "name": "PoAAmount", "width": "25%", "render": function (data, type, st) {
                        return st.PoAAmount.formatMoney();
                    }
                },
                {
                    "data": "OutstandingAmount", "orderable": true, "name": "OutstandingAmount", "width": "25%", "render": function (data, type, st) {
                        return st.OutstandingAmount.formatMoney();
                    }
                }, {
                    "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                            + '</button><ul class="dropdown-menu">'
                            + '<li><a href="/OperationManager/TaxOfficeAssessmentDetail?tofid=' + st.TaxOfficeID + '&tyear=' + $("#cboYear").val() + '">View Details</a></li>'
                            + '</ul></div>';
                    }, "width": "10%"
                }
            ],
            "order": [[0, "asc"]],
        }); 
    }
}