var vTable;
$(document).ready(function () {

    vTable = $("#tblData").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/EMPDMVAPending/LoadData",
            "type": "POST",
            "datatype": "json",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "PaymentRefNumber", "orderable": true, "name": "PaymentRefNumber", "width": "25%" },
            { "data": "PaymentDateTime", "orderable": true, "name": "PaymentDateTime", "width": "25%" },
            { "data": "CustomerName", "orderable": true, "name": "CustomerName", "width": "25%" },
            { "data": "RIN", "orderable": true, "name": "RIN", "width": "25%" },
            { "data": "Bank", "orderable": true, "name": "Bank", "width": "25%" },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "width": "25%", "render": function (data, type, st) {
                    return st.Amount.formatMoney();
                }
            },
            {
                "data": "", "orderable": false, "name": "Action",
                "render": function (data, type, st) {
                    var vArrEntry = $('#hdnEntryIds').val().split(',');
                    if (jQuery.inArray(st.PDMVPID.toString(), vArrEntry) !== -1) {
                        return '<input type="checkbox" name="chkEntry" checked="checked" class="icheck" data-checkbox="icheckbox_square-green" id="chkSelect_' + st["PDMVPID"] + '" data-eid="' + st["PDMVPID"] + '">';
                    }
                    else {
                        return '<input type="checkbox" name="chkEntry" class="icheck" data-checkbox="icheckbox_square-green" id="chkSelect_' + st["PDMVPID"] + '" data-eid="' + st["PDMVPID"] + '">';

                    }
                }
            }
        ],
        "order": [[1, "desc"]],
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "drawCallback": function (settings) {
            BindCheckBoxEvent();
        }
    });

    $('#btnSave').click(function () {
        if ($("#hdnEntryIds").val() !== '') {
            $("#frmEntry").submit();
        }
        else {
            jsfn_ShowAlert('Select atleast one entry', 'danger');
        }

        return false;
    });
});

function BindCheckBoxEvent() {
    handleiCheck();

    $("input[name=chkEntry]").on("ifChecked", function () {
        jsfn_onEntrySelected($(this).data("eid").toString(), true);
    });

    $("input[name=chkEntry]").on("ifUnchecked", function () {
        jsfn_onEntrySelected($(this).data("eid").toString(), false);
    });

}

function jsfn_onEntrySelected(eid, isChecked) {
    var vArrEntry = $('#hdnEntryIds').val().split(',');
    if (isChecked) {
        if ($('#hdnEntryIds').val() !== "")
            vArrEntry.push(eid);
        else {
            vArrEntry = new Array();
            vArrEntry.push(eid);
        }
    }
    else {
        vArrEntry.splice($.inArray(eid, vArrEntry), 1);
    }

    $('#hdnEntryIds').val(vArrEntry.toString());
}