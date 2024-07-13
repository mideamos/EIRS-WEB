$(document).ready(function () {

    $('#btnSubmit').click(function () {
        if ($("#frmBusinessSubSector").valid()) {
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
        BusinessCategory: "<div class='widget-header'><h4>Edit Record Confirmation</h4></div>",
        buttons: [{
            html: "<i class='fa fa-check'></i>&nbsp; Yes",
            "class": "btn btn-primary",
            click: function () {
                $("#frmBusinessSubSector").submit();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });


    $('#cboBusinessType').on("change", jsfn_onBusinessTypeChange);
    $('#cboBusinessCategory').on("change", jsfn_onBusinessCategoryChange);

});

function jsfn_onBusinessTypeChange() {
    var vBusinessTypeID = $('#cboBusinessType').val();

    if (vBusinessTypeID != '') {

        var vData = {
            BusinessTypeID: vBusinessTypeID == '' ? 0 : vBusinessTypeID
        };

        jsfn_ajaxPost('/BusinessSubSector/GetBusinessCategory', vData, jsfn_onBusinessTypeChangeResponse);
    }
    else {
        $("#cboBusinessCategory").html("");

        $("#cboBusinessCategory").append(
                $('<option></option>').val("").html("Select Business Category"));


        $('#cboBusinessCategory').val('').select2({ "width": "100%" });

        $("#cboBusinessSector").html("");

        $("#cboBusinessSector").append(
                $('<option></option>').val("").html("Select Business Sector"));
        $('#cboBusinessSector').val('').select2({ "width": "100%" });

    }
}

function jsfn_onBusinessTypeChangeResponse(data) {
    $("#cboBusinessCategory").html("");

    $("#cboBusinessCategory").append(
            $('<option></option>').val("").html("Select Business Category"));

    if (data != undefined) {
        $.each(data, function (i, bcat) {
            $("#cboBusinessCategory").append(
                $('<option></option>').val(bcat.id).html(bcat.text));
        });

        $('#cboBusinessCategory').val('').select2({ "width": "100%" });
        
        $("#cboBusinessSector").html("");

        $("#cboBusinessSector").append(
                $('<option></option>').val("").html("Select Business Sector"));
        $('#cboBusinessSector').val('').select2({ "width": "100%" });
    }
}

function jsfn_onBusinessCategoryChange() {
    var vBusinessCategoryID = $('#cboBusinessCategory').val();

    if (vBusinessCategoryID != '') {

        var vData = {
            BusinessCategoryID: vBusinessCategoryID == '' ? 0 : vBusinessCategoryID
        };

        jsfn_ajaxPost('/BusinessSubSector/GetBusinessSector', vData, jsfn_onBusinessCategoryChangeResponse);
    }
}

function jsfn_onBusinessCategoryChangeResponse(data) {
    $("#cboBusinessSector").html("");

    $("#cboBusinessSector").append(
            $('<option></option>').val("").html("Select Business Sector"));

    if (data != undefined) {
        $.each(data, function (i, bcat) {
            $("#cboBusinessSector").append(
                $('<option></option>').val(bcat.id).html(bcat.text));
        });

        $('#cboBusinessSector').val('').select2({ "width": "100%" });
    }
}