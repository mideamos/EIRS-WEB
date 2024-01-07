var vReceiptTable;
$(document).ready(function () {
    $("#dvShowCancelReceiptAlert").hide();
    $("#dvShowReceiptNotesAlert").hide();
    vReceiptTable = $("#tblReceiptList").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Settle/LoadReceiptData",
            "type": "POST",
            "datatype": "json",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "ReceiptRefNo", "orderable": true, "name": "ReceiptRefNo", "width": "25%" },
            {
                "data": "ReceiptDate", "orderable": true, "name": "ReceiptDate", "width": "25%",
                "render": function (data, type, st) {
                    var vReceiptDate = new Date(parseInt(st.ReceiptDate.substr(6)));
                    return vReceiptDate.format("dd-mmm-yyyy");
                }
            },
            {
                "data": "ReceiptAmount", "orderable": true, "name": "ReceiptAmount", "width": "25%", "render": function (data, type, st) {
                    return st.ReceiptAmount.formatMoney();
                }
            },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName", "width": "25%" },
            { "data": "ASRefNo", "orderable": true, "name": "ASRefNo", "width": "25%" },
            { "data": "ReceiptStatusName", "orderable": true, "name": "ReceiptStatusName", "width": "25%" },
            {
                "data": "", "orderable": false, "name": "Action",
                "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                        + '<ul class="dropdown-menu">'
                        + (st.StatusID === 3 ? '<li><a href="/TreasuryReceipt/SignReceipt?id=' + st.ReceiptID + '&name=' + st.ReceiptRefNo+'">Sign Receipt</a></li>' : '')
                        + '<li><a href="/TreasuryReceipt/Details?id=' + st.ReceiptID + '&name=' + st.ReceiptRefNo +'">View Receipt</a></li>'
                        //+ '<li><a onclick="javascript:jsfn_ShowReceiptNotesModal(' + st.ReceiptID + ')">Update Notes</a></li>'
                        + (st.StatusID !== 2 ? '<li><a onclick="javascript:jsfn_ShowCancelModal(' + st.ReceiptID + ')">Cancel Receipt</a></li>' : '')
                        + '</ul>';
                }
            }
        ],
        "order": [[1, "desc"]],
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        }
    });

    $("#btnCancelReceipt").click(function () {
        if ($("#txtCancelNotes").val() !== '') {

            var vData = {
                CancelNotes: $("#txtCancelNotes").val(),
                ReceiptID: $("#hdnReceiptID").val()
            };

            jsfn_ajaxPost('/Settle/CancelReceipt', vData, jsfn_CancelReceiptResponse);

        }
        else {
            $("#dvShowCancelReceiptAlert").html('Please Enter Notes');
            $("#dvShowCancelReceiptAlert").show();
        }
    });

    $("#btnUpdateReceiptNotes").click(function () {
        if ($("#txtNotes").val() !== '') {

            var vData = {
                Notes: $("#txtNotes").val(),
                ReceiptID: $("#hdnRN_ReceiptID").val()
            };

            jsfn_ajaxPost('/Settle/UpdateReceiptNotes', vData, jsfn_UpdateReceiptNotesResponse);

        }
        else {
            $("#dvShowReceiptNotesAlert").html('Please Enter Notes');
            $("#dvShowReceiptNotesAlert").show();
        }
    });

});

function jsfn_ShowCancelModal(recId) {
    $("#dvShowCancelReceiptAlert").hide();
    $("#hdnReceiptID").val(recId);
    $("#dvCancelReceiptModal").modal('show');
}

function jsfn_ShowReceiptNotesModal(recId) {
    var vData = {
        ReceiptID: recId
    };
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Settle/GetReceiptNotes', vData, jsfn_ShowReceiptNotesResponse);
}

function jsfn_ShowReceiptNotesResponse(data) {
    jsfn_HideLoading();
    $("#dvShowReceiptNotesAlert").hide();
    $("#hdnRN_ReceiptID").val(data.ReceiptID);
    $("#txtNotes").val(data.Notes);
    $("#dvReceiptNotesModal").modal('show');
}

function jsfn_CancelReceiptResponse(data) {
    if (data.success) {
        $("#hdnReceiptID").val(0);
        $("#txtCancelNotes").val('');
        $("#dvCancelReceiptModal").modal('hide');
        jsfn_ShowAlert(data.Message, 'success', true);
        vReceiptTable.draw();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger', true);
    }
}

function jsfn_UpdateReceiptNotesResponse(data) {
    if (data.success) {
        $("#hdnRN_ReceiptID").val(0);
        $("#txtNotes").val('');
        $("#dvReceiptNotesModal").modal('hide');
        jsfn_ShowAlert(data.Message, 'success', true);
        
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger', true);
    }
}
