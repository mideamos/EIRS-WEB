$(document).ready(function () {
    $("#fsStep2").hide();
    $("#hdStep2").hide();
    $("#btnSave").hide();
    $("#btnBack").hide();

    $("#dvMDAServiceItemList").hide();
    $("#dvMSAddMDAServiceItem").hide();

    jsfn_BuildMDAServiceItemDataTable();

    if ($("#hdnMDAServiceID").val() != '-1') {

        $('#btnSave').click(function () {
            if (ValidatedForm()) {
                $('#dvUpdateDialog').dialog('open');
            }
            return false;

        });
    }
    else {
        $('#btnSave').click(function () {
            if (ValidatedForm()) {
                $("#frmMainMDAService").submit();
            }

            return false;
        });
    }

    $('#dvUpdateDialog').removeClass('hide');
    $('#dvUpdateDialog').dialog({
        autoOpen: false,
        width: 600,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Edit Record Confirmation</h4></div>",
        buttons: [{
            html: "<i class='fa fa-check'></i>&nbsp; Yes",
            "class": "btn btn-primary",
            click: function () {
                $("#frmMainMDAService").submit();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    var frmMainMDAService = $('#frmMainMDAService');

    // init validator obj and set the rules
    frmMainMDAService.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            // the rules, as usual
        },

        highlight: function (element) { // hightlight error inputs
            $(element).closest('.control-group').addClass('error'); // set error class to the control group
        },

        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.control-group').removeClass('error'); // set error class to the control group
        }
    });

    $("#btnNext").on('click', function () {
        if ($('#fsStep1 :input').valid()) {
            $("#fsStep2").show();
            $("#fsStep1").hide();
            $("#btnSave").show();
            $("#btnBack").show();
            $("#btnNext").hide();
            $("#btnMainBack").hide();
        }
    });

    $("#btnBack").on('click', function () {
        $("#fsStep2").hide();
        $("#fsStep1").show();
        $("#btnSave").hide();
        $("#btnBack").hide();
        $("#btnNext").show();
        $("#btnMainBack").show();
    });

    $("#aMDAServiceItem").click(function () {
        $("#frmMainMDAService").hide();
        $("#dvMDAServiceItemList").show();
    });

    $("#btnMSIBack").click(function () {
        $("#frmMainMDAService").show();
        $("#dvMDAServiceItemList").hide();
    });

    $("#btnBackMDAServiceItems").click(function () {
        $("#artMDAService").addClass("col-lg-6");
        $("#dvMSAddMDAServiceItem").hide();
        $("#dvMDAServiceItemList").show();
    });
    

    $("#btnMSIAdd").click(function () {
        $("#artMDAService").removeClass("col-lg-6");
        $("#dvMSAddMDAServiceItem").show();
        $("#dvMDAServiceItemList").hide();
    });

    $("#btnAddMDAServiceItems").click(function () {
        if ($("#hdnMDAServiceItemIds").val() != "") {
            var vData = {
                MDAServiceItemIds: $("#hdnMDAServiceItemIds").val(),
            };

            jsfn_ShowLoading();
            jsfn_ajaxPost('/MDAService/AddMDAServiceItem', vData, jsfn_AddMDAServiceItemResponse);

        } else {
            jsfn_ShowAlert('Please Select MDA Service Items', 'info');
        }
    });

    var vSettlementMethodData = new Array();
    var vcboSettlementMethodValue = $('#cboSettlementMethod').attr('data-initvalue');
    if (vcboSettlementMethodValue != '')
        vSettlementMethodData = vcboSettlementMethodValue.split(',');

    $('#cboSettlementMethod').val(vSettlementMethodData).select2({ "width": "100%" });


   
    $('#dvMDAServiceItemDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>MDA Service Item Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });
});

function ValidatedForm() {
    var vItemsCount = $('#spnMSIRecordCount').html();
    var isValid = false;

    if (parseInt(vItemsCount) == 0) {
        $("#ddliMDAServiceItem").addClass('state-error');
        $("#MDAServiceItem-error").removeClass('hide');
        isValid = false;
    }
    else {
        $("#ddliMDAServiceItem").removeClass('state-error');
        $("#MDAServiceItem-error").addClass('hide');
        isValid = true;
    }

    return isValid;
}


function jsfn_SelectMDAServiceItem(chkMDAServiceItem) {

    var vArrMDAServiceItem = $('#hdnMDAServiceItemIds').val().split(',');
    if (chkMDAServiceItem.checked) {
        if ($('#hdnMDAServiceItemIds').val() != "")
            vArrMDAServiceItem.push(chkMDAServiceItem.dataset["mdaserviceitemid"]);
        else {
            vArrMDAServiceItem = new Array();
            vArrMDAServiceItem.push(chkMDAServiceItem.dataset["mdaserviceitemid"]);
        }
    }
    else {
        vArrMDAServiceItem.splice($.inArray(chkMDAServiceItem.dataset["mdaserviceitemid"], vArrMDAServiceItem), 1);
    }

    $('#hdnMDAServiceItemIds').val(vArrMDAServiceItem.toString());
}

function jsfn_AddMDAServiceItemResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        $('#tbodyMDAServiceItemList').html(data.MDAServiceItemList);
        $('#spnMSIRecordCount').html(data.MDAServiceItemCount);
        $("#dvMSAddMDAServiceItem").hide();
        $("#dvMDAServiceItemList").show();
        $("#artMDAService").addClass("col-lg-6");
        $("#ddliMDAServiceItem").removeClass('state-error');
        $("#MDAServiceItem-error").addClass('hide');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_RemoveMDAServiceItem(rowid) {
    var vData = {
        RowID: rowid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/MDAService/RemoveMDAServiceItem', vData, jsfn_AddMDAServiceItemResponse)
}


function jsfn_BuildMDAServiceItemDataTable() {
    vMDAServiceItemDataTable = jsfn_ConvertToDatatable($('#tbMDAServiceItem'));
    //Hide Columns
    vMDAServiceItemDataTable.column(1).visible(false);
    vMDAServiceItemDataTable.column(2).visible(false);
    vMDAServiceItemDataTable.column(3).visible(false);
    vMDAServiceItemDataTable.column(4).visible(false);
    vMDAServiceItemDataTable.column(5).visible(false);
    vMDAServiceItemDataTable.column(9).visible(false);
}


function jsfn_ShowMDAServiceItemDetails(MDAServiceItemID) {

    var vData = {
        MDAServiceItemID: MDAServiceItemID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/MDAService/GetMDAServiceItemDetails', vData, jsfn_ShowMDAServiceItemDetailResponse)
}

function jsfn_ShowMDAServiceItemDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvMDAServiceItemDetails').dialog('open');
        $('#dvMDAServiceItemRefNo').html(data.MDAServiceItemDetails.MDAServiceItemReferenceNo);
        $('#dvRevenueStream').html(data.MDAServiceItemDetails.RevenueStreamName);
        $('#dvRevenueSubStream').html(data.MDAServiceItemDetails.RevenueSubStreamName);
        $('#dvItemCategory').html(data.MDAServiceItemDetails.AssessmentItemCategoryName);
        $('#dvItemSubCategory').html(data.MDAServiceItemDetails.AssessmentItemSubCategoryName);
        $('#dvRevenueAgency').html(data.MDAServiceItemDetails.AgencyName);
        $('#dvMDAServiceItemName').html(data.MDAServiceItemDetails.MDAServiceItemName);
        $('#dvComputation').html(data.MDAServiceItemDetails.ComputationName);
        $('#dvServiceBaseAmount').html(data.MDAServiceItemDetails.ServiceBaseAmount);
        $('#dvPercentage').html(data.MDAServiceItemDetails.Percentage);
        $('#dvServiceAmount').html(data.MDAServiceItemDetails.ServiceAmount);

        if (data.MDAServiceItemDetails.ComputationID == 1) {
            $('#dvCPercentage').hide();
        }
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}


