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

    var vfrmPaymentChannelByRevenueStream = $("#frmPaymentChannelByRevenueStream");
    vfrmPaymentChannelByRevenueStream.validate({
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
        if (vfrmPaymentChannelByRevenueStream.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });

    $("#btnExportToExcel").click(function () {
        var vYear = $("#cboYear").val();
        var vRevenueStreamID = $("#cboRevenueStream").val() !== '' ? $("#cboRevenueStream").val() : '0';
        var vFromDate = $("#txtFromDate").val() !== '' ? $("#txtFromDate").val() : '0';
        var vToDate = $("#txtToDate").val() !== '' ? $("#txtToDate").val() : '0';

        var vUrl = '/OperationManager/PaymentChannelByRevenueStreamExportToExcel?RevenueStreamID=' + vRevenueStreamID + '&Year=' + vYear + '&FromDate=' + vFromDate + '&ToDate=' + vToDate;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });
});

var vPaymentChannelByRevenueStream;
function jsfn_bindTable() {
    if (vPaymentChannelByRevenueStream !== undefined) {
        vPaymentChannelByRevenueStream.draw();
    }
    else {
        vPaymentChannelByRevenueStream = $("#tblPaymentChannelByRevenueStream").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/PaymentChannelByRevenueStreamLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.RevenueStreamID = $("#cboRevenueStream").val() !== '' ? $("#cboRevenueStream").val() : '0';
                    data.Year = $("#cboYear").val() !== '' ? $("#cboYear").val() : '0';
                    data.FromDate = $('#txtFromDate').val() !== '' ? $('#txtFromDate').val() : '';
                    data.ToDate = $('#txtToDate').val() !== '' ? $('#txtToDate').val() : '';
                }, "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "SettlementMethodName", "orderable": true, "name": "SettlementMethodName", "width": "25%" },
                {
                    "data": "PoAAmount", "orderable": true, "name": "PoAAmount", "width": "25%", "render": function (data, type, st) {
                        return st.PoAAmount.formatMoney();
                    }
                },
                {
                    "data": "SettlementAmount", "orderable": true, "name": "SettlementAmount", "width": "25%", "render": function (data, type, st) {
                        return st.SettlementAmount.formatMoney();
                    }
                }, {
                    "data": "TotalAmount", "orderable": true, "name": "TotalAmount", "width": "25%", "render": function (data, type, st) {
                        return st.TotalAmount.formatMoney();
                    }
                }, {
                    "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                            + '</button><ul class="dropdown-menu">'
                            + '<li><a href="/OperationManager/PaymentChannelByRevenueStreamDetail?smthId=' + st.SettlementMethodID + '&tyear=' + $("#cboYear").val() + '&rstrmID=' + ($("#cboRevenueStream").val() !== '' ? $("#cboRevenueStream").val() : 0) + '&fdate=' + $('#txtFromDate').val() + '&tdate=' + $('#txtToDate').val() + '">View Details</a></li>'
                            + '</ul></div>';
                    }, "width": "10%"
                }
            ],
            "order": [[0, "asc"]],
            "footerCallback": function (row, ddata, start, end, display) {
                var vFooterTotal = this.api().ajax.json().FooterTotal;
                if (vFooterTotal) {
                    var $th = $(row).find('th');
                    $th.eq(1).html(vFooterTotal['PoAAmount'].formatMoney());
                    $th.eq(2).html(vFooterTotal['SettlementAmount'].formatMoney());
                    $th.eq(3).html(vFooterTotal['TotalAmount'].formatMoney());
                }
            }
        });
    }
}