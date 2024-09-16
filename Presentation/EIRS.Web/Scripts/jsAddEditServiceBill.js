var vMDAServiceTable;
$(document).ready(function () {
    $("#btnSaveItems").click(function () { jsfn_UpdateServiceAmount(); });

    $("#btnFinalSave").click(function () {
        $("#frmAddServiceBill").submit();
    });

    $("#btnAddSelectedMDAService").click(function () {
        if ($("#hdnMDAServiceIds").val() != '') {
            var vData = {
                mdsIds: $("#hdnMDAServiceIds").val()
            };

            jsfn_ShowLoading();
            jsfn_ajaxPost('/Base/AddMDAService', vData, jsfn_AddMDAServiceResponse);
        }
        else {
            jsfn_ShowAlert('Select atleast one mda service', 'danger');
        }

        return false;
    });

    $("#btnAddMDAService").click(function () {
        jsfn_bindTable();
        $('#dvMDAServiceModal').modal('show');
    });

    $("#btnModalYes").click(function () {
        var vData = {
            RowID: $("#hdnTBID").val()
        };

        jsfn_ShowLoading();
        jsfn_ajaxPost('/Base/RemoveMDAService', vData, jsfn_RemoveMDAServiceResponse);
    });

});

function jsfn_AddMDAServiceResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $("#hdnMDAServiceIds").val("");
        $('#dvMDAServiceModal').modal('hide');
        $("#dvMDAService").html(data.MDAServiceDetails);
        vMDAServiceTable.destroy();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

$(document).on('submit', 'form', function () {
    var button = $("#btnFinalSave");
    setTimeout(function () {
        button.attr('disabled', 'disabled');
    }, 0);
});

function jsfn_UpdateServiceAmount() {
    var vRowList = '';
    $("#dvEditMDAServiceItemModalBody input.cbaseamount").each(function () {
        if (parseInt($(this).data('tbachanged')) === 1) {
            var vRowID = $(this).data('rowid');
            var vServiceBaseAmount = $(this).val();

            if (vRowList == '') {
                vRowList = vRowID + '@@' + vServiceBaseAmount;
            }
            else {
                vRowList = vRowList + '~~' + vRowID + '@@' + vServiceBaseAmount;
            }
        }
    });

    if (vRowList != '') {

        var vData = {
            rowdata: vRowList,
        };

        jsfn_ShowLoading();
        jsfn_ajaxPost('/Base/UpdateMDAServiceItem', vData, jsfn_UpdateServiceAmountResponse);
    }
    else {
        $('#dvEditMDAServiceItemModal').modal('hide');
    }
}

function jsfn_UpdateServiceAmountResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvEditMDAServiceItemModal').modal('hide');
        $("#dvMDAService").html(data.MDAServiceDetails);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}



function jsfn_ShowMDAServiceItems(rowId) {
    var vData = {
        MDAServiceRowID: rowId,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetMDAServiceItemView', vData, jsfn_ShowMDAServiceItemResponse);
}

function jsfn_ShowMDAServiceItemResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvViewMDAServiceItemModal').modal('show');
        $('#spViewMDAServiceName').html(data.MDAServiceName);
        $("#dvViewMDAServiceItemModalBody").html(data.MDAServiceItemList);

    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_EditMDAServiceItems(rowId) {
    var vData = {
        MDAServiceRowID: rowId,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetMDAServiceItemEdit', vData, jsfn_EditMDAServiceItemResponse);
}

function jsfn_EditMDAServiceItemResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvEditMDAServiceItemModal').modal('show');
        $('#spEditMDAServiceName').html(data.MDAServiceName);
        $("#dvEditMDAServiceItemModalBody").html(data.MDAServiceItemList);
        if ($("#dvEditMDAServiceItemModalBody input.cbaseamount").length > 0) {
            jsfn_BindServiceBaseAmount();
            $("#dvModalFooter").show();
        }
        else {
            $("#dvModalFooter").hide();
        }
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BindServiceBaseAmount() {
    $(".cbaseamount").blur(function () {
        var vComputationID = $(this).data('computationid');
        var vRowID = $(this).data('rowid');
        var vPercentage = $(this).data('percent');
        var vServiceBaseAmount = $(this).val();

        if (parseInt(vComputationID) === 2) {
            var vServiceAmount = parseFloat(vServiceBaseAmount) * (parseFloat(vPercentage)  / 100);
        }
        else if (parseInt(vComputationID) === 3) {
            var vServiceAmount = parseFloat(vServiceBaseAmount);
        }

        $("#td_" + vRowID).html(vServiceAmount.formatMoney(2, '₦'));

        $(this).attr('data-tbachanged', '1');
    });
}

function jsfn_bindTable() {

    vMDAServiceTable = $('#tblMDAServices').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/Base/GetMDAServiceData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            },
            "complete": function () {
                handleiCheck();

                $(".icheck").on("ifChecked", function () {
                    jsfn_onMDAServiceSelected($(this).data("mdaserid").toString(), true);
                });

                $(".icheck").on("ifUnchecked", function () {
                    jsfn_onMDAServiceSelected($(this).data("mdaserid").toString(), false);
                });
            }
        },
        "columns": [{
            "data": "TaxYear"
        }, {
            "data": "MDAServiceName"
        }, {
            "data": "ServiceAmount"
        }, {
            "data": "MDAServiceID"
        }],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(3)', nRow).html('<input type="checkbox" class = "icheck" data-checkbox = "icheckbox_square-green" title = "Tick To Select" data-mdaserid="' + aData["MDAServiceID"] + '" >');
            //console.log(aData);
            var vServiceAmount = aData["ServiceAmount"] != null ? aData["ServiceAmount"] : 0;
            $('td:eq(2)', nRow).html(vServiceAmount.formatMoney());
        }
    });


}

function jsfn_onMDAServiceSelected(mdaserid, isChecked) {
    var vArrMDAService = $('#hdnMDAServiceIds').val().split(',');
    if (isChecked) {
        if ($('#hdnMDAServiceIds').val() != "")
            vArrMDAService.push(mdaserid);
        else {
            vArrMDAService = new Array();
            vArrMDAService.push(mdaserid);
        }
    }
    else {
        vArrMDAService.splice($.inArray(mdaserid, vArrMDAService), 1);
    }

    $('#hdnMDAServiceIds').val(vArrMDAService.toString());
}

function jsfn_RemoveMDAService(msrowId) {
    $('#dvRemoveModal').modal('show');
    $("#hdnTBID").val(msrowId);
}

function jsfn_RemoveMDAServiceResponse(data) {
    jsfn_HideLoading();

    if (data.success) {
        $('#dvRemoveModal').modal('hide')
        $("#hdnTBID").val(0);
        $("#dvMDAService").html(data.MDAServiceDetails);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

