//QC_VP_T-EARS-56/63/64_OperationManagerMakeTCCMonthlyRevoked
$(document).ready(function () {
    $('#dvSearchData').hide();

    var vfrmTCCMonthlyRevoked = $("#frmTCCMonthlyRevoked");
    vfrmTCCMonthlyRevoked.validate({
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
        if (vfrmTCCMonthlyRevoked.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });

    $("#btnExportToExcel").click(function () {
        var vYear = $("#cboYear").val();
        var vStatusID = $("#cboStatus").val() !== '' ? $("#cboStatus").val() : '0';

        var vUrl = '/OperationManager/TCCMonthlyRevokedExportToExcel?StatusID=' + vStatusID + '&TaxYear=' + vYear;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });

});


var vTCCMonthlyRevoked;
function jsfn_bindTable() {
    if (vTCCMonthlyRevoked !== undefined) {
        vTCCMonthlyRevoked.draw();
    }
    else {
        vTCCMonthlyRevoked = $("#tblTCCMonthlyRevoked").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/TCCMonthlyRevokedLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.TaxYear = $("#cboYear").val() !== '' ? $("#cboYear").val() : '0';
                    data.StatusID = $("#cboStatus").val() !== '' ? $("#cboStatus").val() : '0';
                }, "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "MonthID", "orderable": true, "name": "MonthID", "orderable": false },
                { "data": "StartMonthName", "orderable": true, "name": "StartMonthName", "orderable": false },
                { "data": "RequestCount", "orderable": true, "name": "RequestCount" },
                {
                    "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                            + '</button><ul class="dropdown-menu">'
                            + '<li><a href="/OperationManager/TCCMonthlyRevokedDetail?year=' + $("#cboYear").val() + '&month=' + st.MonthID + '&statid=' + ($("#cboStatus").val() !== "" ? $("#cboStatus").val() : 0) + '">View Details</a></li>'
                            + '<li><a href="/OperationManager/TCCMonthlyRevokedDetailExportToExcel?TaxYear=' + $("#cboYear").val() + '&TaxMonth=' + st.MonthID + '&StatusID=' + ($("#cboStatus").val() !== "" ? $("#cboStatus").val() : 0) + '">Export Details</a></li>'
                            + '</ul></div>';
                    }, "width": "10%"
                }
            ],
        });
    }
}