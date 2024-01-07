$(document).ready(function () {
    $('#dvSearchData').hide();

    var vfrmAllocatedTaxPayer = $("#frmAllocatedTaxPayer");
    vfrmAllocatedTaxPayer.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            TaxOfficer: {
                required: true
            },
            TaxOffice: {
                required: true
            }
        },
        messages: {
            TaxOfficer: {
                required: "Select Tax Officer"
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
        if (vfrmAllocatedTaxPayer.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });

    $('#cboTaxOffice').on("change", jsfn_onTaxOfficeChange);

    $("#btnAdd").click(function () {
        $("#dvTaxPayerTypeModal").modal('show');
    });

    $("#btnProceed").click(function () {
        var vTaxPayerTypeID = $("#cboTaxPayerType").val();

        if (vTaxPayerTypeID !== '') {
            window.location.href = '/OperationManager/AllocateTaxPayerToTaxOfficer?tptid=' + vTaxPayerTypeID + '&toid=' + $("#cboTaxOffice").val() + '&tofid=' + $("#cboTaxOfficer").val();
        }
    });

});

function jsfn_onTaxOfficeChange() {
    var vTaxOfficeID = $('#cboTaxOffice').val();

    if (vTaxOfficeID != '') {

        var vData = {
            TaxOfficeID: vTaxOfficeID == '' ? 0 : vTaxOfficeID,
            showmanager: false
        };

        jsfn_ajaxPost('/Base/GetTaxOfficeManger', vData, jsfn_onTaxOfficeChangeResponse);
    }
}

function jsfn_onTaxOfficeChangeResponse(data) {
    $("#cboTaxOfficer").html("");

    $("#cboTaxOfficer").append(
        $('<option></option>').val("").html("Select Manager"));

    if (data !== undefined) {
        $.each(data, function (i, tit) {
            $("#cboTaxOfficer").append(
                $('<option></option>').val(tit.id).html(tit.text));
        });

        $('#cboTaxOfficer').selectpicker('refresh');
    }
}

var vAllocatedTaxPayerTable;
function jsfn_bindTable() {
    if (vAllocatedTaxPayerTable !== undefined) {
        vAllocatedTaxPayerTable.draw();
    }
    else {
        vAllocatedTaxPayerTable = $("#tblAllocatedTaxPayer").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/AllocateTaxPayerLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.TaxOfficeID = $("#cboTaxOffice").val();
                    data.TaxOfficerID = $("#cboTaxOfficer").val();
                },
                "error": function (jqXHR, exception) {
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