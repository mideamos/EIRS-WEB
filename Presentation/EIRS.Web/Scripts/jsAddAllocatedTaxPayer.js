var vAllSelected = true;
$(document).ready(function () {
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
                data.TaxOfficeID = $("#hdnTaxOffice").val();
                data.TaxPayerTypeID = $("#hdnTaxPayerType").val();
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
                    var vArrTaxPayers = $('#hdnTaxPayerIds').val().split(',');
                    if (jQuery.inArray(st.TaxPayerID.toString(), vArrTaxPayers) !== -1) {
                        return '<input type="checkbox" name="chkTaxPayers" checked="checked" class="icheck" data-checkbox="icheckbox_square-green" id="chkSelect_' + st["TaxPayerID"] + '" data-tpid="' + st["TaxPayerID"] + '">';
                    }
                    else {
                        if (vAllSelected === true)
                            vAllSelected = false;
                        return '<input type="checkbox" name="chkTaxPayers" class="icheck" data-checkbox="icheckbox_square-green" id="chkSelect_' + st["TaxPayerID"] + '" data-tpid="' + st["TaxPayerID"] + '">';

                    }
                }, "width": "10%"
            },
        ],
        "order": [[1, "asc"]],
        "drawCallback": function (settings) {
            $("#chkSelectAll").iCheck('destroy');
            BindCheckBoxEvent(vAllSelected);
        }
    });

    $('#btnSave').click(function () {
        if ($("#hdnTaxPayerIds").val() != '') {
            $("#frmAllocateTaxPayer").submit();
        }
        else {
            jsfn_ShowAlert('Select atleast one tax payer', 'danger');
        }

        return false;
    });

});

function BindCheckBoxEvent(vSelectAllChecked) {
    handleiCheck();
    if (vSelectAllChecked === true) {
        $("#chkSelectAll").iCheck('check');
    }
    else {
        $("#chkSelectAll").iCheck('uncheck');
    }

    $("input[name=chkTaxPayers]").on("ifChecked", function () {
        jsfn_onTaxPayerSelected($(this).data("tpid").toString(), true);
    });

    $("input[name=chkTaxPayers]").on("ifUnchecked", function () {
        jsfn_onTaxPayerSelected($(this).data("tpid").toString(), false);
    });

    $("#chkSelectAll").on("ifChecked", function () {
        $("input[name=chkTaxPayers]").iCheck('check');
    });

    $("#chkSelectAll").on("ifUnchecked", function () {
        $("input[name=chkTaxPayers]").iCheck('uncheck');
    });

    vAllSelected = true;
}

function jsfn_onTaxPayerSelected(aruleid, isChecked) {
    var vArrTaxPayer = $('#hdnTaxPayerIds').val().split(',');
    if (isChecked) {
        if ($('#hdnTaxPayerIds').val() != "")
            vArrTaxPayer.push(aruleid);
        else {
            vArrTaxPayer = new Array();
            vArrTaxPayer.push(aruleid);
        }
    }
    else {
        vArrTaxPayer.splice($.inArray(aruleid, vArrTaxPayer), 1);
    }

    $('#hdnTaxPayerIds').val(vArrTaxPayer.toString());
}