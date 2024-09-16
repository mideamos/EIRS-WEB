//QC_VP_T-EARS-56/63/64_OperationManagerMakeTCCMonthlyCommission
$(document).ready(function () {
    $('#dvSearchData').hide();

    var vfrmTCCMonthlyCommission = $("#frmTCCMonthlyCommission");
    vfrmTCCMonthlyCommission.validate({
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
        if (vfrmTCCMonthlyCommission.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });

    $("#btnExportToExcel").click(function () {
        var vYear = $("#cboYear").val();
        var vStatusID = $("#cboStatus").val() !== '' ? $("#cboStatus").val() : '0';

        var vUrl = '/OperationManager/TCCMonthlyCommissionExportToExcel?StatusID=' + vStatusID + '&TaxYear=' + vYear;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });

});


var vTCCMonthlyCommission;
function jsfn_bindTable() {
    if (vTCCMonthlyCommission !== undefined) {
        vTCCMonthlyCommission.draw();
    }
    else {
        vTCCMonthlyCommission = $("#tblTCCMonthlyCommission").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/TCCMonthlyCommissionLoadData",
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
                    "data": "CommissionAmount", "orderable": true, "name": "CommissionAmount", "render": function (data, type, st) {
                        return st.CommissionAmount.formatMoney();
                    } },
            ],
        });
    }
}