$(document).ready(function () {
    $('#dvSearchData').hide();

    var vfrmTaxOfficerByRevenueStreamTarget = $("#frmTaxOfficerByRevenueStreamTarget");
    vfrmTaxOfficerByRevenueStreamTarget.validate({
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
            TaxOfficer: {
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
            TaxOfficer: {
                required: "Select Tax Officer"
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

    $('#cboTaxOffice').on("change", jsfn_onTaxOfficeChange)

    $("#btnSearch").click(function () {
        if (vfrmTaxOfficerByRevenueStreamTarget.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });

    $("#btnExportToExcel").click(function () {
        var vYear = $("#cboYear").val();
        var vMonth = $("#cboMonth").val();
        var vTaxOfficerID = $("#cboTaxOfficer").val() !== '' ? $("#cboTaxOfficer").val() : '0';

        var vUrl = '/OperationManager/TaxOfficerByRevenueStreamTargetExportToExcel?TaxOfficerID=' + vTaxOfficerID + '&Year=' + vYear + '&Month=' + vMonth;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });
});

function jsfn_onTaxOfficeChange() {
    var vTaxOfficeID = $('#cboTaxOffice').val();

    if (vTaxOfficeID !== '') {

        var vData = {
            TaxOfficeID: vTaxOfficeID === '' ? 0 : vTaxOfficeID,
            showmanager: false
        };

        jsfn_ajaxPost('/Base/GetTaxOfficeManger', vData, jsfn_onTaxOfficeChangeResponse);
    }
}

function jsfn_onTaxOfficeChangeResponse(data) {
    $("#cboTaxOfficer").html("");

    $("#cboTaxOfficer").append(
        $('<option></option>').val("").html("Select Tax Officer"));

    if (data !== undefined) {
        $.each(data, function (i, tit) {
            $("#cboTaxOfficer").append(
                $('<option></option>').val(tit.id).html(tit.text));
        });

        $('#cboTaxOfficer').selectpicker('refresh');
    }
}

var vTaxOfficerByRevenueStreamTarget;
function jsfn_bindTable() {
    if (vTaxOfficerByRevenueStreamTarget !== undefined) {
        vTaxOfficerByRevenueStreamTarget.draw();
    }
    else {
        vTaxOfficerByRevenueStreamTarget = $("#tblTaxOfficerByRevenueStreamTarget").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/TaxOfficerByRevenueStreamTargetLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.TaxOfficerID = $("#cboTaxOfficer").val() !== '' ? $("#cboTaxOfficer").val() : '0';
                    data.Year = $("#cboYear").val() !== '' ? $("#cboYear").val() : '0';
                    data.Month = $("#cboMonth").val() !== '' ? $("#cboMonth").val() : '0';
                }, "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "RevenueStreamName", "orderable": true, "name": "RevenueStreamName", "width": "25%" },
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