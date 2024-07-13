$(document).ready(function () {
    $("#fsStep2").hide();
    $("#btnSave").hide();
    $("#btnBack").hide();

    jsfn_BuildBuildingDataTable();
    $("#btnNext").on('click', function () {
        if ($("#hdnBuildingId").val() != '' && $("#hdnBuildingId").val() != '0') {
            jsfn_BindBuildingUnitTable();
        }
        else {
            jsfn_ShowAlert('Select atleast one building', 'danger');
        }

        return false;
    });

    $("#btnBack").click(function () {
        $("#fsStep2").hide();
        $("#fsStep1").show();
        $("#btnSave").hide();
        $("#btnBack").hide();
        $("#btnNext").show();
        $("#btnCancel").show();
        
        return false;
    });

    
    $('#btnSave').click(function () {
        if ($("#hdnBuildingUnitId").val() != '' && $("#hdnBuildingUnitId").val() != '0') {
            $("#frmBuilding").submit();
        }
        else {
            jsfn_ShowAlert('Select atleast one building unit', 'danger');
        }

        return false;
    });
});



function jsfn_BuildBuildingDataTable() {
    vBuildingDataTable = jsfn_ConvertToDatatable($('#tbAsset'));
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


function jsfn_BindBuildingUnitTable() {
    var vBuildingID = $("#hdnBuildingId").val();
    var vData = {
        BuildingID: vBuildingID
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Business/GetBuildingUnitList', vData, jsfn_BindBuildingUnitTableResponse);
}

function jsfn_BindBuildingUnitTableResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#fsStep2').html(data.BuildingUnitList);
        jsfn_ConvertToDatatable($('#tbBuildingUnit'));
        $("#fsStep2").show();
        $("#fsStep1").hide();
        $("#btnSave").show();
        $("#btnBack").show();
        $("#btnNext").hide();
        $("#btnCancel").hide();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}


function jsfn_SelectSingleAsset(rbAsset) {
    $("#hdnBuildingId").val(rbAsset.dataset["assetid"]);
}

function jsfn_SelectSingleBuildingUnit(rbBuildingUnit) {
    $("#hdnBuildingUnitId").val(rbBuildingUnit.dataset["buid"]);
}