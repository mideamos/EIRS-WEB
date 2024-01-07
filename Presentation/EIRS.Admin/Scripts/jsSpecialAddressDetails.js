$(document).ready(function () {
    $("#fsStep2").hide();
    $("#btnSave").hide();
    $("#btnBack").hide();

    jsfn_BuildBuildingDataTable();

    var frmAddress = $('#frmAddress');

    // init validator obj and set the rules
    frmAddress.validate({
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
            $("#dvFooter").hide();
            $("#btnSave").show();
            $("#btnBack").show();
            $("#artAddressInformation").removeClass("col-lg-6");
        }
    });

    $("#btnBack").click(function () {
        $("#fsStep2").hide();
        $("#fsStep1").show();
        $("#dvFooter").show();
        $("#btnSave").hide();
        $("#btnBack").hide();
        $("#artAddressInformation").addClass("col-lg-6");
        return false;
    });

    $('#btnSave').click(function () {
        if ($("#hdnBuildingID").val() != '') {
            $("#frmAddress").submit();
        }
        else {
            jsfn_ShowAlert('Select atleast one building', 'danger');
        }

        return false;
    });

});


function jsfn_BuildBuildingDataTable() {
    vBuildingDataTable = jsfn_ConvertToDatatable($('#tbBuilding'));
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

function jsfn_SelectBuilding(rbBuilding) {
    $("#hdnBuildingID").val(rbBuilding.dataset["bldid"]);
}