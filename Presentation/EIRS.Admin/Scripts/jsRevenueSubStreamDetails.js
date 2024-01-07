$(document).ready(function () {
    $('#btnSubmit').click(function () {
        if ($("#frmRevenueSubStream").valid()) {
            $('#dvUpdateDialog').dialog('open');
        }
        return false;

    });

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
                $("#frmRevenueSubStream").submit();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    //$('#cboAssetType').on("change", jsfn_onAssetTypeChange);


});

//function jsfn_onAssetTypeChange() {
//    var vAssetTypeID = $('#cboAssetType').val();

//    if (vAssetTypeID != '') {

//        var vData = {
//            AssetTypeID: vAssetTypeID == '' ? 0 : vAssetTypeID
//        };

//        jsfn_ajaxPost('/RevenueSubStream/GetRevenueStream', vData, jsfn_onAssetTypeChangeResponse);
//    }
//    else {
//        $("#cboRevenueStream").html("");

//        $("#cboRevenueStream").append(
//                $('<option></option>').val("").html("Select Revenue Stream"));


//        $('#cboRevenueStream').val('').select2({ "width": "100%" });

//    }
//}

//function jsfn_onAssetTypeChangeResponse(data) {
//    $("#cboRevenueStream").html("");

//    $("#cboRevenueStream").append(
//            $('<option></option>').val("").html("Select Revenue Stream"));

//    if (data != undefined) {
//        $.each(data, function (i, rstm) {
//            $("#cboRevenueStream").append(
//                $('<option></option>').val(rstm.id).html(rstm.text));
//        });

//        $('#cboRevenueStream').val('').select2({ "width": "100%" });
//   }
//}
