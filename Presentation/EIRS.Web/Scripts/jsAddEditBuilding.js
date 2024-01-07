$(document).ready(function () {
    $('#cboTown').on("change", jsfn_onTownChange);
    $('#cboLGA').on("change", jsfn_onLGAChange);
    $('#cboUnitPurpose').on("change", jsfn_onUnitPurposeChange);
    jsfn_BindTable();

    var vfrmBuildingUnit = $("#frmBuildingUnit");
    vfrmBuildingUnit.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            txtUnitNumber: {
                required: true
            },
            cboUnitPurpose: {
                required: true
            },
            cboUnitFunction: {
                required: true
            },
            cboUnitOccupancy: {
                required: true
            },
            cboSize: {
                required: true
            }

        },
        messages: {
            txtUnitNumber: {
                required: "Enter Unit Number"
            },
            cboUnitPurpose: {
                required: "Select Unit Purpose"
            },
            cboUnitFunction: {
                required: "Select Unit Function"
            },
            cboUnitOccupancy: {
                required: "Select Unit Occupancy"
            },
            cboSize: {
                required: "Select Unit Size"
            }
        },
        errorPlacement: function (error, element) {
            if (element.is('select')) {
                error.insertAfter(element[0].parentElement);
            }
            else {
                error.insertAfter(element);
            }
        },
        highlight: function (element) { // hightlight error inputs
            $(element).closest('.input').addClass('error'); // set error class to the control group
        },
        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.input').removeClass('error'); // set error class to the control group
        }
    });

    $("#btnSaveBuildingUnit").click(function () {
        if (vfrmBuildingUnit.valid()) {
            var vData = {
                UnitNumber: $("#txtUnitNumber").val(),
                UnitPurposeID: $("#cboUnitPurpose").val(),
                UnitPurposeName: $("#cboUnitPurpose option:selected").text(),
                UnitFunctionID: $("#cboUnitFunction").val(),
                UnitFunctionName: $("#cboUnitFunction option:selected").text(),
                UnitOccupancyID: $("#cboUnitOccupancy").val(),
                UnitOccupancyName: $("#cboUnitOccupancy option:selected").text(),
                UnitSizeID: $("#cboSize").val(),
                UnitSizeName: $("#cboSize option:selected").text(),
            }

            jsfn_ShowLoading();
            jsfn_ajaxPost('/Base/AddUnitInformation', vData, jsfn_onAddBuildingResponse);
        }
    });

    $('#btnSaveBuilding').click(function () {
        if (parseInt($("#hdnBuildingUnitCount").val()) > 0) {
            $("#frmAddBuilding").submit();
        }
        else {
            jsfn_ShowAlert('Add Atleast One Building Unit', 'danger');
        }

        return false;
    });
});

function jsfn_onAddBuildingResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $("#dvBuildingUnitData").html(data.BuildingUnitList);
        jsfn_BindTable();
        $("#txtUnitNumber").val('')
        $("#cboUnitPurpose").val('').selectpicker('refresh');
        $("#cboUnitFunction").val('').selectpicker('refresh');
        $("#cboUnitOccupancy").val('').selectpicker('refresh');
        $("#cboSize").val('').selectpicker('refresh');
        $("#dvBuildingUnitModal").modal('hide');
        $("#hdnBuildingUnitCount").val(parseInt($("#hdnBuildingUnitCount").val()) + 1);
        
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BindTable() {
    var vColumnsList = [{
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }];

    var vSortOrder = [];

    jsfn_ConvertToDatableWithCustomSort($('#tblBuildingUnitList'), vColumnsList, 'No Building Unit Found', vSortOrder);
}

function jsfn_onTownChange() {
    var vTownID = $('#cboTown').val();

    if (vTownID != '') {

        var vData = {
            TownID: vTownID == '' ? 0 : vTownID
        };

        jsfn_ajaxPost('/Base/GetTownDetails', vData, jsfn_onTownChangeResponse);
    }
}

function jsfn_onTownChangeResponse(data) {
    if (data != undefined) {
        $("#cboLGA").val(data.LGAID).selectpicker('refresh');
        jsfn_onLGAChange();
    }
}

function jsfn_onLGAChange() {
    var vLGAID = $('#cboLGA').val();

    var vData = {
        LGAID: vLGAID == '' ? 0 : vLGAID
    };

    jsfn_ajaxPost('/Base/GetWard', vData, jsfn_onLGAChangeResponse);
}

function jsfn_onLGAChangeResponse(data) {
    $("#cboWard").html(""); // clear before appending new list

    $("#cboWard").append(
        $('<option></option>').val("").html("Select Ward"));

    $.each(data, function (i, wrd) {
        $("#cboWard").append(
            $('<option></option>').val(wrd.id).html(wrd.text));
    });

    $('#cboWard').selectpicker('refresh');
}


function jsfn_onUnitPurposeChange() {
    var vUnitPurposeID = $('#cboUnitPurpose').val();

    var vData = {
        UnitPurposeID: vUnitPurposeID == '' ? 0 : vUnitPurposeID
    };

    jsfn_ajaxPost('/Base/GetUnitFunction', vData, jsfn_onUnitPurposeChangeResponse);
}

function jsfn_onUnitPurposeChangeResponse(data) {
    $("#cboUnitFunction").html(""); // clear before appending new list

    $("#cboUnitFunction").append(
        $('<option></option>').val("").html("Select Unit Function"));

    $.each(data, function (i, bfunc) {
        $("#cboUnitFunction").append(
            $('<option></option>').val(bfunc.id).html(bfunc.text));
    });

    $('#cboUnitFunction').selectpicker('refresh');
}

function jsfn_AddBuildingUnit() {
    $("#dvBuildingUnitModal").modal('show');
}