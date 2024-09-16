﻿//QC_VP_T-EARS-56/63/64_OperationManagerMakeRevenueStreamAssessmentsbyTaxOffice
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

    var vfrmRevenueStreamAssessmentsbyTaxOffice = $("#frmRevenueStreamAssessmentsbyTaxOffice");
    vfrmRevenueStreamAssessmentsbyTaxOffice.validate({
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
        if (vfrmRevenueStreamAssessmentsbyTaxOffice.valid()) {
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

        var vUrl = '/OperationManager/RevenueStreamAssessmentsbyTaxOfficeExportToExcel?RevenueStreamID=' + vRevenueStreamID + '&TaxYear=' + vYear + '&FromDate=' + vFromDate + '&ToDate=' + vToDate;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });

});


var vRevenueStreamAssessmentsbyTaxOffice;
function jsfn_bindTable() {
    if (vRevenueStreamAssessmentsbyTaxOffice !== undefined) {
        vRevenueStreamAssessmentsbyTaxOffice.draw();
    }
    else {
        vRevenueStreamAssessmentsbyTaxOffice = $("#tblRevenueStreamAssessmentsbyTaxOffice").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/RevenueStreamAssessmentsbyTaxOfficeLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.TaxYear = $("#cboYear").val() !== '' ? $("#cboYear").val() : '0';
                    data.FromDate = $('#txtFromDate').val() !== '' ? $('#txtFromDate').val() : '';
                    data.ToDate = $('#txtToDate').val() !== '' ? $('#txtToDate').val() : '';
                    data.RevenueStreamID = $("#cboRevenueStream").val() !== '' ? $("#cboRevenueStream").val() : '0';
                }, "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "TaxOfficeName", "orderable": true, "name": "TaxOfficeName" },
                { "data": "TaxPayerCount", "orderable": true, "name": "TaxPayerCount" },
                { "data": "AssetCount", "orderable": true, "name": "AssetCount" },
                {
                    "data": "BillAmount", "orderable": true, "name": "BillAmount", "render": function (data, type, st) {
                        return st.BillAmount.formatMoney();
                    }
                },
                {
                    "data": "SettlementAmount", "orderable": true, "name": "SettlementAmount", "render": function (data, type, st) {
                        return st.SettlementAmount.formatMoney();
                    }
                },
                {
                    "data": "PoAAmount", "orderable": true, "name": "PoAAmount", "render": function (data, type, st) {
                        return st.PoAAmount.formatMoney();
                    }
                },
                {
                    "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                            + '</button><ul class="dropdown-menu">'
                            + '<li><a href="/OperationManager/RevenueStreamBillDetailByTaxOffice?RevenueStreamID= ' + ($("#cboRevenueStream").val() !== '' ? $("#cboRevenueStream").val() : 0) + '&TaxYear=' + $("#cboYear").val() + '&TaxOfficeID=' + st.TaxOfficeID + '&FromDate=' + $('#txtFromDate').val() + '&ToDate=' + $('#txtToDate').val() + '">View Details</a></li>'
                                + '<li><a href="/OperationManager/RevenueStreamBillDetailByTaxOfficeExportToExcel?RevenueStreamID= ' + ($("#cboRevenueStream").val() !== '' ? $("#cboRevenueStream").val() : 0) + '&TaxYear=' + $("#cboYear").val() + '&TaxOfficeID=' + st.TaxOfficeID + '&FromDate=' + $('#txtFromDate').val() + '&ToDate=' + $('#txtToDate').val() + '">Export Details</a></li>'
                                + '</ul></div>';
                    }, "width": "10%"
                },
            ],
            "order": [[0, "asc"]],
            "footerCallback": function (row, ddata, start, end, display) {
                var vFooterTotal = this.api().ajax.json().FooterTotal;
                if (vFooterTotal) {
                    var $th = $(row).find('th');
                    $th.eq(1).html(vFooterTotal['TaxPayerCount']);
                    $th.eq(2).html(vFooterTotal['AssetCount']);
                    $th.eq(3).html(vFooterTotal['BillAmount'].formatMoney());
                    $th.eq(4).html(vFooterTotal['SettlementAmount'].formatMoney());
                    $th.eq(5).html(vFooterTotal['PoAAmount'].formatMoney());
                }
            }
        });
    }
}