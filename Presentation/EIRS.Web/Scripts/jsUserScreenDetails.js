jQuery(document).ready(function () {

    $(".icheck").on("ifChecked", function () {
        jsfn_onScreenSelected($(this).data("screenid").toString(), true);
    });

    $(".icheck").on("ifUnchecked", function () {
        jsfn_onScreenSelected($(this).data("screenid").toString(), false);
    });

    jsfn_bindTable();

    $('#btnSave').click(function () {
        if ($("#hdnScreenIds").val() != '') {
            $("#frmScreen").submit();
        }
        else {
            jsfn_ShowAlert('Select Atleast Screen', 'danger');
        }

        return false;
    });

    $("#btnModalYes").click(function () {
        var vData = {
            USID: $("#hdnTBID").val(),
            UserID: $("#hdnOtherID").val()
        };

        jsfn_ShowLoading();
        jsfn_ajaxPost('/User/RemoveScreen', vData, jsfn_RemoveScreenResponse);
    });

});

function jsfn_bindTable() {
    var vColumnsList = [{
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": false
    }];

    var vSortOrder = [];

    jsfn_ConvertToDatableWithCustomSort($('#tblScreen'), vColumnsList, 'No Screen Found', vSortOrder);
}

function jsfn_onScreenSelected(screenid, isChecked) {
    var vArrScreen = $('#hdnScreenIds').val().split(',');
    if (isChecked) {
        if ($('#hdnScreenIds').val() != "")
            vArrScreen.push(screenid);
        else {
            vArrScreen = new Array();
            vArrScreen.push(screenid);
        }
    }
    else {
        vArrScreen.splice($.inArray(screenid, vArrScreen), 1);
    }

    $('#hdnScreenIds').val(vArrScreen.toString());
}

function jsfn_RemoveLink(usid, userid) {
    $('#dvRemoveModal').modal('show');
    $("#hdnTBID").val(usid);
    $("#hdnOtherID").val(userid);
}

function jsfn_RemoveScreenResponse(data) {
    jsfn_HideLoading();
    $('#dvRemoveModal').modal('hide');
    if (data.success) {

        $("#hdnTBID").val(0);
        $("#hdnOtherID").val(0);
        jsfn_ShowAlert(data.Message, 'success');

        $('#dvScreenData').html(data.ScreenList);
        jsfn_bindTable();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}


