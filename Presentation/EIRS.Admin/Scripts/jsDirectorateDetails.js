$(document).ready(function () {
    $('#btnSubmit').click(function () {
        if ($("#frmDirectorate").valid()) {
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
                $("#frmDirectorate").submit();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    var vRevenueStreamData = new Array();
    var vcboRevenueStreamValue = $('#cboRevenueStream').attr('data-initvalue');
    if (vcboRevenueStreamValue != '')
        vRevenueStreamData = vcboRevenueStreamValue.split(',');

    $('#cboRevenueStream').val(vRevenueStreamData).select2({ "width": "100%" });


});