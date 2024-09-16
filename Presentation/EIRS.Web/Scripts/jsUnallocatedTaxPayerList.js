$(document).ready(function () {
    $('#dvSearchData').hide();

    var vfrmUnAllocatedTaxPayer = $("#frmUnAllocatedTaxPayer");
    vfrmUnAllocatedTaxPayer.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            TaxPayerType: {
                required: true
            },
            TaxOffice: {
                required: true
            }
        },
        messages: {
            TaxPayerType: {
                required: "Select Tax Payer Type"
            },
            TaxOffice: {
                required: "Select Tax Office"
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
        if (vfrmUnAllocatedTaxPayer.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });

    $("#btnExportToExcel").click(function () {
        var vTaxOfficeID = $("#cboTaxOffice").val();
        var vTaxPayerTypeID = $("#cboTaxPayerType").val();

        var vUrl = '/OperationManager/UnAllocatedTaxPayerExportToExcel?TaxOfficeID=' + vTaxOfficeID + '&TaxPayerTypeID=' + vTaxPayerTypeID;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });


});

var vUnallocatedTaxPayerTable;
function jsfn_bindTable() {
    if (vUnallocatedTaxPayerTable !== undefined) {
        vUnallocatedTaxPayerTable.draw();
    }
    else {
        vUnallocatedTaxPayerTable = $("#tblUnallocatedTaxPayer").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/UnAllocatedTaxPayerLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.TaxOfficeID = $("#cboTaxOffice").val();
                    data.TaxPayerTypeID = $("#cboTaxPayerType").val();
                }, "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "TaxPayerRIN", "orderable": true, "name": "TaxPayerRIN", "width": "25%" },
                { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName", "width": "25%" },
                { "data": "TaxPayerTypeName", "orderable": true, "name": "TaxPayerTypeName", "width": "25%" },
                {
                    "data": "TotalAssessmentAmount", "orderable": true, "name": "TotalAssessmentAmount", "width": "25%", "render": function (data, type, st) {
                        return st.TotalAssessmentAmount.formatMoney();
                    }
                },
                {
                    "data": "TotalPaymentAmount", "orderable": true, "name": "TotalPaymentAmount", "width": "25%", "render": function (data, type, st) {
                        return st.TotalPaymentAmount.formatMoney();
                    }
                },
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