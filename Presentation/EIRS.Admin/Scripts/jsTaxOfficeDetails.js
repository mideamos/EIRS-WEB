var vBuildingDataTable;
$(document).ready(function () {
    $('#btnEditFinalSave').click(function () {
        if ($("#frmTaxOffice").valid()) {
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
                $("#frmTaxOfficeRecordLink").submit();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $("#frmTaxOfficeRecordLink").hide();
    $("#dvTaxOfficeAddressList").hide();
    $("#frmTaxOfficeAddressType").hide();
    $("#dvTaxOfficeBuilding").hide();
    jsfn_BuildDataTable();


    $("#frmTaxOfficeAddressType").validate({
        rules: {
            AddressType: {
                required: true,
            }
        },

        // Messages for form validation
        messages: {
            AddressType: {
                required: 'Please select address type',
            }
        },

        // Do not change code below
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent());
        }
    });

    $("#btnNext").click(function () {
        if ($("#frmTaxOfficeInformation").valid()) {
            debugger
            $("#lblTaxOfficeName").html($("#txtTaxOfficeName").val());
            $("#hdnTaxOfficeName").val($("#txtTaxOfficeName").val());
            $("#hdnApprover1").val($("#cboApprover1").val());
            $("#hdnApprover2").val($("#cboApprover2").val());
            $("#hdnApprover3").val($("#cboApprover3").val());
            $("#hdnZone").val($("#cboZone").val());
            $("#hdnActive").val($("#chkActive").is(":checked"));
            $("#frmTaxOfficeInformation").hide();
            $("#frmTaxOfficeRecordLink").show();

        }
        return false;
    });

    $("#btnRLBack").click(function () {
        $("#frmTaxOfficeInformation").show();
        $("#frmTaxOfficeRecordLink").hide();
    });

    $("#aTaxOfficeAddress").click(function () {
        $("#frmTaxOfficeRecordLink").hide();
        $("#dvTaxOfficeAddressList").show();
    });

    $("#btnTOABack").click(function () {
        $("#frmTaxOfficeRecordLink").show();
        $("#dvTaxOfficeAddressList").hide();
    });

    $("#btnTOAAdd").click(function () {
        $("#frmTaxOfficeAddressType").show();
        $("#dvTaxOfficeAddressList").hide();
    });

    $("#btnTOADBack").click(function () {
        $("#frmTaxOfficeAddressType").hide();
        $("#dvTaxOfficeAddressList").show();
    });

    $("#btnTOADNext").click(function () {
        if ($("#frmTaxOfficeAddressType").valid()) {
            $("#frmTaxOfficeAddressType").hide();
            $("#dvTaxOfficeBuilding").show();
            $("#artTaxOffice").removeClass("col-lg-6");
        }
        return false;
    });

    $("#btnBackTaxOfficeBuilding").click(function () {
        $("#dvTaxOfficeBuilding").hide();
        $("#dvTaxOfficeAddressList").show();
        $("#cboAddressType").val("").select2({ "width": "100%" });
        $("#artTaxOffice").addClass("col-lg-6");
    });

    $('#dvBuildingDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Building Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

});

function jsfn_AddTaxOfficeAddress(bldid, bldrin, bldname) {

    var vData = {
        BuildingID: bldid,
        BuildingRIN: bldrin,
        BuildingName: bldname,
        AddressTypeID: $("#cboAddressType").val(),
        AddressTypeName: $("#cboAddressType option:selected").text()
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/TaxOffice/AddTaxOfficeAddress', vData, jsfn_AddTaxOfficeAddressResponse);
}

function jsfn_AddTaxOfficeAddressResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        $('#tbodyTaxOfficeAddressList').html(data.TaxOfficeAddressList);
        $('#spnTOARecordCount').html(data.TaxOfficeAddressCount);
        $("#dvTaxOfficeBuilding").hide();
        $("#dvTaxOfficeAddressList").show();
        $("#cboAddressType").val("").select2({ "width": "100%" });
        $("#artTaxOffice").addClass("col-lg-6");
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowBuildingDetails(BuildingID) {

    var vData = {
        BuildingID: BuildingID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/TaxOffice/GetBuildingDetails', vData, jsfn_ShowBuildingDetailResponse)
}

function jsfn_ShowBuildingDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvBuildingDetails').dialog('open');
        $('#dvBuildingRIN').html(data.BuildingDetails.BuildingRIN);
        $('#dvBuildingTagNumber').html(data.BuildingDetails.BuildingTagNumber);
        $('#dvBuildingName').html(data.BuildingDetails.BuildingName == null ? "-" : data.BuildingDetails.BuildingName);
        $('#dvBuildingNumber').html(data.BuildingDetails.BuildingNumber);
        $('#dvStreetName').html(data.BuildingDetails.StreetName);
        $('#dvOffStreetName').html(data.BuildingDetails.OffStreetName == null ? "-" : data.BuildingDetails.OffStreetName);
        $('#dvTown').html(data.BuildingDetails.TownName);
        $('#dvLGA').html(data.BuildingDetails.LGAName);
        $('#dvWard').html(data.BuildingDetails.WardName);
        $('#dvAssetType').html(data.BuildingDetails.AssetTypeName);
        $('#dvBuildingType').html(data.BuildingDetails.BuildingTypeName);
        $('#dvBuildingCompletion').html(data.BuildingDetails.BuildingCompletionName);
        $('#dvBuildingPurpose').html(data.BuildingDetails.BuildingPurposeName);
        $('#dvBuildingFunction').html(data.BuildingDetails.BuildingFunctionNames);
        $('#dvBuildingOwnership').html(data.BuildingDetails.BuildingOwnershipName == null ? "-" : data.BuildingDetails.BuildingOwnershipName);
        $('#dvBuildingOccupancy').html(data.BuildingDetails.BuildingOccupancyName);
        $('#dvBuildingOccupancyType').html(data.BuildingDetails.BuildingOccupancyTypeName);
        $('#dvLatitude').html(data.BuildingDetails.Latitude == null ? "-" : data.BuildingDetails.Latitude);
        $('#dvLongitude').html(data.BuildingDetails.Longitude == null ? "-" : data.BuildingDetails.Longitude);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_RemoveAddressDetails(rowid) {
    var vData = {
        RowID: rowid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/TaxOffice/RemoveTaxOfficeAddress', vData, jsfn_AddTaxOfficeAddressResponse)
}

function jsfn_BuildDataTable() {
    vBuildingDataTable = $("#tbBuilding").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Building/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.BuildingRIN = $("#txtBuildingRIN").val();
                data.BuildingTagNumber = $("#txtBuildingTagNumber").val();
                data.BuildingName = $("#txtBuildingName").val();
                data.BuildingNumber = $("#txtBuildingNumber").val();
                data.StreetName = $("#txtStreetName").val();
                data.OffStreetName = $("#txtOffStreetName").val();
                data.TownName = $("#txtTownName").val();
                data.LGAName = $("#txtLGAName").val();
                data.WardName = $("#txtWardName").val();
                data.BuildingTypeName = $("#txtBuildingTypeName").val();
                data.BuildingCompletionName = $("#txtBuildingCompletionName").val();
                data.BuildingPurposeName = $("#txtBuildingPurposeName").val();
                data.BuildingOwnershipName = $("#txtBuildingOwnershipName").val();
                data.NoOfUnits = $("#txtNoOfUnits").val();
                data.BuildingSize_Length = $("#txtBuildingLength").val();
                data.BuildingSize_Width = $("#txtBuildingWidth").val();
                data.Latitude = $("#txtLatitude").val();
                data.Longitude = $("#txtLongitude").val();
                data.ActiveText = $("#txtStatus").val();
            }
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "BuildingRIN", "orderable": true, "name": "BuildingRIN" },
            { "data": "BuildingTagNumber", "orderable": true, "name": "BuildingTagNumber" },
            { "data": "BuildingName", "orderable": true, "name": "BuildingName" },
            { "data": "BuildingNumber", "orderable": true, "name": "BuildingNumber" },
            { "data": "StreetName", "orderable": true, "name": "StreetName" },
            { "data": "OffStreetName", "orderable": true, "name": "OffStreetName" },
            { "data": "TownName", "orderable": true, "name": "TownName" },
            { "data": "LGAName", "orderable": true, "name": "LGAName" },
            { "data": "WardName", "orderable": true, "name": "WardName" },
            { "data": "BuildingTypeName", "orderable": true, "name": "BuildingTypeName" },
            { "data": "BuildingCompletionName", "orderable": true, "name": "BuildingCompletionName" },
            { "data": "BuildingPurposeName", "orderable": true, "name": "BuildingPurposeName" },
            { "data": "BuildingOwnershipName", "orderable": true, "name": "BuildingOwnershipName" },
            { "data": "NoOfUnits", "orderable": true, "name": "NoOfUnits" },
            { "data": "BuildingSize_Length", "orderable": true, "name": "BuildingSize_Length" },
            { "data": "BuildingSize_Width", "orderable": true, "name": "BuildingSize_Width" },
            { "data": "Latitude", "orderable": true, "name": "Latitude" },
            { "data": "Longitude", "orderable": true, "name": "Longitude" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, Bui) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a onclick="javascript:jsfn_AddTaxOfficeAddress(' + Bui.BuildingID + ',"' + Bui.BuildingRIN + '","' + Bui.BuildingName+'")">Select Building</a></li>'
                        + '</ul></div>';
                }
            },
        ],
        "order": [[1, "asc"]]
    });


    $("#tbBuilding thead th input[type=text]").on('change', function () {
        vBuildingDataTable
            .column($(this.data).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });
    //Hide Columns
    vBuildingDataTable.column(2).visible(false);
    vBuildingDataTable.column(3).visible(false);
    vBuildingDataTable.column(5).visible(false);
    vBuildingDataTable.column(6).visible(false);
    vBuildingDataTable.column(8).visible(false);
    vBuildingDataTable.column(10).visible(false);
    vBuildingDataTable.column(11).visible(false);
    vBuildingDataTable.column(12).visible(false);
    vBuildingDataTable.column(13).visible(false);
    vBuildingDataTable.column(14).visible(false);
    vBuildingDataTable.column(15).visible(false);
    vBuildingDataTable.column(16).visible(false);
    vBuildingDataTable.column(17).visible(false);
}