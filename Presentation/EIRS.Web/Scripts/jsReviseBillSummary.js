//QC_VP_T-EARS-56/63/64_OperationManagerMakeReviseBill
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
    $("#txtFromDate").attr('disabled', 'disabled');
    $("#txtToDate").attr('disabled', 'disabled');

    $("#cboYear").on('change', function () {
        var vYear = $("#cboYear").val();

        vStartDate = new Date(parseInt(vYear), 0, 1);
        vEndDate = new Date(parseInt(vYear), 11, 31);

        $("#txtFromDate").removeAttr('disabled');
        $("#txtToDate").removeAttr('disabled');

        $('#txtFromDate').datepicker('setStartDate', vStartDate);
        $('#txtFromDate').datepicker('setEndDate', vEndDate);
        $('#txtToDate').datepicker('setStartDate', vStartDate);
        $('#txtToDate').datepicker('setEndDate', vEndDate);

        $('#txtFromDate').datepicker('setDate', vStartDate);
        $('#txtToDate').datepicker('setDate', vEndDate);

    });

    var vfrmReviseBill = $("#frmReviseBill");
    vfrmReviseBill.validate({
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
        if (vfrmReviseBill.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });

    $("#btnExportToExcel").click(function () {
        var vYear = $("#cboYear").val();
        var vRevenueStreamID = $("#cboRevenueStream").val() !== '' ? $("#cboRevenueStream").val() : '0';
        var vFromDate = $("#txtFromDate").val() !== '' ? $("#txtFromDate").val() : '';
        var vToDate = $("#txtToDate").val() !== '' ? $("#txtToDate").val() : '';
        var vBillTypeID = $("#cboBillType").val() !== '' ? $("#cboBillType").val() : '0';

        var vUrl = '/OperationManager/ReviseBillSummaryExportToExcel?BillTypeID=' + vBillTypeID + '&RevenueStreamID=' + vRevenueStreamID + '&TaxYear=' + vYear + '&FromDate=' + vFromDate + '&ToDate=' + vToDate;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });

});


var vReviseBill;
function jsfn_bindTable() {
    if (vReviseBill !== undefined) {
        vReviseBill.draw();
    }
    else {
        vReviseBill = $("#tblReviseBill").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/ReviseBillSummaryLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.TaxYear = $("#cboYear").val() !== '' ? $("#cboYear").val() : '0';
                    data.FromDate = $('#txtFromDate').val() !== '' ? $('#txtFromDate').val() : '';
                    data.ToDate = $('#txtToDate').val() !== '' ? $('#txtToDate').val() : '';
                    data.BillTypeID = $("#cboBillType").val() !== '' ? $("#cboBillType").val() : '0';
                    data.RevenueStreamID = $("#cboRevenueStream").val() !== '' ? $("#cboRevenueStream").val() : '0';
                }, "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "BillRef", "orderable": true, "name": "BillRef" },
                {
                    "data": "BillDate", "orderable": true, "name": "BillDate", "render": function (data, type, bill) {
                        if (bill.BillDate != undefined || bill.BillDate != null) {
                            return jsfn_FormatJsonDate(bill.BillDate, "dd-mmm-yyyy");
                        }
                        else {
                            return '';
                        }
                    }
                },
                {
                    "data": "BillAmount", "orderable": true, "name": "BillAmount", "render": function (data, type, st) {
                        return st.BillAmount.formatMoney();
                    }
                },
                {
                    "data": "RevisedDate", "orderable": true, "name": "RevisedDate", "render": function (data, type, bill) {
                        if (bill.RevisedDate != undefined || bill.RevisedDate != null) {
                            return jsfn_FormatJsonDate(bill.RevisedDate, "dd-mmm-yyyy");
                        }
                        else {
                            return '';
                        }
                    }
                },
                {
                    "data": "RevisedAmount", "orderable": true, "name": "RevisedAmount", "render": function (data, type, st) {
                        return st.RevisedAmount.formatMoney();
                    }
                },
                { "data": "BillStatusName", "orderable": true, "name": "BillStatusName" }
            ],
            "order": [[0, "asc"]],
        });
    }
}