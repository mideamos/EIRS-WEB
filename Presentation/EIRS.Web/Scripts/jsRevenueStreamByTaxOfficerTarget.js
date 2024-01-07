$(document).ready(function () {
    $('#dvSearchData').hide();

    var vfrmRevenueStreamByTaxOfficerTarget = $("#frmRevenueStreamByTaxOfficerTarget");
    vfrmRevenueStreamByTaxOfficerTarget.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            Year: {
                required: true
            },
            Month: {
                required: true
            },
            TaxOffice: {
                required: true
            },
            RevenueStream: {
                required: true
            }

        },
        messages: {
            Year: {
                required: "Select Year"
            },
            Month: {
                required: "Select Month"
            },
            TaxOffice: {
                required: "Select Tax Office"
            },
            RevenueStream: {
                required: "Select Revenue Stream"
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
        if (vfrmRevenueStreamByTaxOfficerTarget.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });

    $("#btnExportToExcel").click(function () {
        var vYear = $("#cboYear").val();
        var vMonth = $("#cboMonth").val();
        var vTaxOfficeID = $("#cboTaxOffice").val() !== '' ? $("#cboTaxOffice").val() : '0';
        var vRevenueStreamID = $("#cboRevenueStream").val() !== '' ? $("#cboRevenueStream").val() : '0';

        var vUrl = '/OperationManager/RevenueStreamByTaxOfficerTargetExportToExcel?TaxOfficeID=' + vTaxOfficeID + '&Year=' + vYear + '&Month=' + vMonth + '&RevenueStreamID=' + vRevenueStreamID;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });
});


var vRevenueStreamByTaxOfficerTarget;
function jsfn_bindTable() {
    if (vRevenueStreamByTaxOfficerTarget !== undefined) {
        vRevenueStreamByTaxOfficerTarget.draw();
    }
    else {
        vRevenueStreamByTaxOfficerTarget = $("#tblRevenueStreamByTaxOfficerTarget").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/RevenueStreamByTaxOfficerTargetLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.TaxOfficeID = $("#cboTaxOffice").val() !== '' ? $("#cboTaxOffice").val() : '0';
                    data.RevenueStreamID = $("#cboRevenueStream").val() !== '' ? $("#cboRevenueStream").val() : '0';
                    data.Year = $("#cboYear").val() !== '' ? $("#cboYear").val() : '0';
                    data.Month = $("#cboMonth").val() !== '' ? $("#cboMonth").val() : '0';
                }, "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "TaxOfficerName", "orderable": true, "name": "TaxOfficerName", "width": "25%" },
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
                }, {
                    "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                            + '</button><ul class="dropdown-menu">'
                            + '<li><a href="/OperationManager/RevenueStreamByTaxOfficerTargetDetailExportToExcel?TaxOfficerID=' + st.TaxOfficerID + '&RevenueStreamID=' + $("#cboRevenueStream").val() + '&Year=' + $("#cboYear").val() + '&Month=' + $("#cboMonth").val() + '">Extract Details</a></li>'
                            + '</ul></div>';
                    }, "width": "10%"
                }
            ],
            "order": [[1, "asc"]]
        });
    }
}