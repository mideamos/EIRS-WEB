$(document).ready(function () {
    $('#btnSubmit').click(function () {
        if ($("#frmBusiness").valid()) {
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
                $("#frmBusiness").submit();
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
    $('#cboBusinessSector').on("change", jsfn_onBusinessSectorChange);

});


function jsfn_onBusinessTypeChange() {
    var vBusinessTypeID = $('#cboBusinessType').val();

    var vData = {
        BusinessTypeID: vBusinessTypeID == '' ? 0 : vBusinessTypeID
    };

    jsfn_ajaxPost('/Business/BusinessTypeChange', vData, jsfn_onBusinessTypeChangeResponse);
}

function jsfn_onBusinessTypeChangeResponse(data) {
    $("#cboBusinessCategory").html("");

    $("#cboBusinessCategory").append(
            $('<option></option>').val("").html("Select Business Category"));

    $.each(data.BusinessCategory, function (i, bcat) {
        $("#cboBusinessCategory").append(
            $('<option></option>').val(bcat.id).html(bcat.text));
    });

    $('#cboBusinessCategory').val('').select2({ "width": "100%" });

    $("#cboBusinessStructure").html("");

    $("#cboBusinessStructure").append(
            $('<option></option>').val("").html("Select Business Structure"));

    $.each(data.BusinessStructure, function (i, bcat) {
        $("#cboBusinessStructure").append(
            $('<option></option>').val(bcat.id).html(bcat.text));
    });

    $('#cboBusinessStructure').val('').select2({ "width": "100%" });

    $("#cboBusinessOperation").html("");

    $("#cboBusinessOperation").append(
            $('<option></option>').val("").html("Select Business Operation"));

    $.each(data.BusinessOperation, function (i, bcat) {
        $("#cboBusinessOperation").append(
            $('<option></option>').val(bcat.id).html(bcat.text));
    });

    $('#cboBusinessOperation').val('').select2({ "width": "100%" });
}

function jsfn_onBusinessCategoryChange() {
    var vBusinessCategoryID = $('#cboBusinessCategory').val();

    if (vBusinessCategoryID != '') {

        var vData = {
            BusinessCategoryID: vBusinessCategoryID == '' ? 0 : vBusinessCategoryID
        };

        jsfn_ajaxPost('/Business/GetBusinessSector', vData, jsfn_onBusinessCategoryChangeResponse);
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


function jsfn_onBusinessSectorChange() {
    var vBusinessSectorID = $('#cboBusinessSector').val();

    if (vBusinessSectorID != '') {

        var vData = {
            BusinessSectorID: vBusinessSectorID == '' ? 0 : vBusinessSectorID
        };

        jsfn_ajaxPost('/Business/GetBusinessSubSector', vData, jsfn_onBusinessSectorChangeResponse);
    }
}

function jsfn_onBusinessSectorChangeResponse(data) {
    $("#cboBusinessSubSector").html("");

    $("#cboBusinessSubSector").append(
            $('<option></option>').val("").html("Select Business Sub Sector"));

    if (data != undefined) {
        $.each(data, function (i, bcat) {
            $("#cboBusinessSubSector").append(
                $('<option></option>').val(bcat.id).html(bcat.text));
        });

        $('#cboBusinessSubSector').val('').select2({ "width": "100%" });
    }
}