   ////QC_VP_T-ERAS-66_GetPOATaxPayerWithoutAsset_01
jQuery(document).ready(function () {
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

    var vfrmPOATaxPayerWithoutAsset = $("#frmPOATaxPayerWithoutAsset");
    vfrmPOATaxPayerWithoutAsset.validate({
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
        if (vfrmPOATaxPayerWithoutAsset.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });

    $("#btnExportToExcel").click(function () {
        var vYear = $("#cboYear").val();
        var vFromDate = $("#txtFromDate").val() !== '' ? $("#txtFromDate").val() : '';
        var vToDate = $("#txtToDate").val() !== '' ? $("#txtToDate").val() : '';
        var vTaxPayerTypeID = $("#cboTaxPayerType").val() !== '' ? $("#cboTaxPayerType").val() : '0';
        var vTaxOfficeID = $("#cboTaxOffice").val() !== '' ? $("#cboTaxOffice").val() : '0';

        var vUrl = '/OperationManager/POATaxPayerWithoutAssetExportToExcel?TaxYear=' + vYear + '&FromDate=' + vFromDate + '&ToDate=' + vToDate + '&TaxPayerTypeID=' + vTaxPayerTypeID + '&TaxOfficeID=' + vTaxOfficeID;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });

   

});

var vPOATaxPayerWithoutAssetDataTable;
function jsfn_bindTable() {
    if (vPOATaxPayerWithoutAssetDataTable !== undefined) {
        vPOATaxPayerWithoutAssetDataTable.draw();
    }
    else {
        vPOATaxPayerWithoutAssetDataTable = $("#tblPOATaxPayerWithoutAsset").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/POATaxPayerWithoutAssetLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.TaxYear = $("#cboYear").val() !== '' ? $("#cboYear").val() : '0';
                    data.FromDate = $('#txtFromDate').val() !== '' ? $('#txtFromDate').val() : '';
                    data.ToDate = $('#txtToDate').val() !== '' ? $('#txtToDate').val() : '';
                    data.TaxPayerTypeID = $("#cboTaxPayerType").val() !== '' ? $("#cboTaxPayerType").val() : '0';
                    data.TaxOfficeID = $("#cboTaxOffice").val() !== '' ? $("#cboTaxOffice").val() : '0';
                }, "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "TaxPayerRIN", "orderable": true, "name": "TaxPayerRIN", "width": "25%" },
                { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName", "width": "25%" },
                { "data": "TaxPayerMobileNumber", "orderable": true, "name": "TaxPayerMobileNumber", "width": "25%" },
                { "data": "TaxPayerAddress", "orderable": true, "name": "TaxPayerAddress", "width": "25%" },
                {
                    "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                            + '</button><ul class="dropdown-menu"><li><a href="' + jsfn_GetTaxPayerDetailUrl(st.TaxPayerID, st.TaxPayerTypeID, st.TaxPayerName) + '">'
                            + 'Tax Payer Details</a></li></ul></div>';
                    }, "width": "10%"
                },
            ],
            "order": [[1, "asc"]]
        });
    }
}

function jsfn_GetTaxPayerDetailUrl(TaxPayerID, TaxPayerTypeID, TaxPayerName) {
    if (TaxPayerTypeID == TaxPayerType.Individual) {
        return '/CaptureIndividual/Details?id=' + TaxPayerID + '&name=' + toSeoUrl(TaxPayerName);
    }
    else if (TaxPayerTypeID == TaxPayerType.Company) {
        return '/CaptureCorporate/Details?id=' + TaxPayerID + '&name=' + toSeoUrl(TaxPayerName);
    }
    else if (TaxPayerTypeID == TaxPayerType.Government) {
        return '/CaptureGovernment/Details?id=' + TaxPayerID + '&name=' + toSeoUrl(TaxPayerName);
    }
    else if (TaxPayerTypeID == TaxPayerType.Special) {
        return '/CaptureSpecial/Details?id=' + TaxPayerID + '&name=' + toSeoUrl(TaxPayerName);
    }
}

function jsfn_RunAutoProfilerResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_bindTable();
    }
}
