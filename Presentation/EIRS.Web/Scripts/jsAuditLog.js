$(document).ready(function () {
    $('#dvSearchData').hide();
    $.fn.datepicker.defaults.format = "yyyy-mm-dd";

    $("#txtFromDate").datepicker({
        autoclose: true
    }).on('changeDate', function (selected) {
        var minDate = new Date(selected.date.valueOf());
        $('#txtToDate').datepicker('setStartDate', minDate);
    });

    $("#txtToDate").datepicker({
        autoclose: true,
    }).on('changeDate', function (selected) {
        var maxDate = new Date(selected.date.valueOf());
        $('#txtFromDate').datepicker('setEndDate', maxDate);
    });
    
    var vfrmAuditLog = $("#frmAuditLog");
    vfrmAuditLog.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            Screen: {
                required: true
            },
            Staff: {
                required: true
            }
        },
        messages: {
            Screen: {
                required: "Select Screen"
            },
            Staff: {
                required: "Select Staff"
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
        if (vfrmAuditLog.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });

});

var vAuditLog;
function jsfn_bindTable() {
    if (vAuditLog !== undefined) {
        vAuditLog.draw();
    }
    else {
        vAuditLog = $("#tblAuditLog").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/AuditLogLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.StaffID = $("#cboStaff").val() !== '' ? $("#cboStaff").val() : '0';
                    data.ASLID = $("#cboScreen").val() !== '' ? $("#cboScreen").val() : '0';
                    data.FromDate = $('#txtFromDate').val() !== '' ? $('#txtFromDate').val() : '';
                    data.ToDate = $('#txtToDate').val() !== '' ? $('#txtToDate').val() : '';
                },
                "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                {
                    "data": "LogDate", "orderable": true, "name": "LogDate", "width": "20%", "render": function (data, type, st) {
                        var vLogDate = new Date(parseInt(st["LogDate"].substr(6)));
                        return vLogDate.format("dd-mmm-yyyy");
                    } },
                { "data": "IPAddress", "orderable": true, "name": "IPAddress", "width": "20%" },
                { "data": "Comment", "orderable": true, "name": "Comment", "width": "60%" },
            ],
            "order": [[0, "desc"]],
        });
    }
}