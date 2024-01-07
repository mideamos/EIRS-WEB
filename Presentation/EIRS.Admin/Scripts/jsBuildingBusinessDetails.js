$(document).ready(function () {
    $("#fsStep2").hide();
    $("#btnSave").hide();
    $("#btnBack").hide();


    jsfn_ConvertToDatatable($('#tbBuildingUnit'));


    $("#btnNext").on('click', function () {
        if ($("#hdnBuildingUnitId").val() != '' && $("#hdnBuildingUnitId").val() != '0') {
            jsfn_BindBusinessTable();
        }
        else {
            jsfn_ShowAlert('Select atleast one building unit', 'danger');
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
        if ($("#hdnBusinessId").val() != '' && $("#hdnBusinessId").val() != '0') {
            $("#frmBusiness").submit();
        }
        else {
            jsfn_ShowAlert('Select atleast one business', 'danger');
        }

        return false;
    });
});

function jsfn_BindBusinessTable() {
    var vData = {
       
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Building/GetBusinessList', vData, jsfn_BindBusinessTableResponse);
}

function jsfn_BindBusinessTableResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#fsStep2').html(data.BusinessList);
        jsfn_BuildBusinessDataTable();
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

function jsfn_BuildBusinessDataTable() {
    vBusinessDataTable = jsfn_ConvertToDatatable($('#tbAsset'));
    //Hide Columns
    vBusinessDataTable.column(2).visible(false);
    vBusinessDataTable.column(3).visible(false);
    vBusinessDataTable.column(6).visible(false);
    vBusinessDataTable.column(8).visible(false);
}


function jsfn_SelectSingleAsset(rbAsset) {
    $("#hdnBusinessId").val(rbAsset.dataset["assetid"]);
}

function jsfn_SelectSingleBuildingUnit(rbBuildingUnit) {
    $("#hdnBuildingUnitId").val(rbBuildingUnit.dataset["buid"]);
}