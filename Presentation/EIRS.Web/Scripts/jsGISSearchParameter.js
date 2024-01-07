$(document).ready(function () {
    vAssessmentRuleTable = jsfn_ConvertToDatableWithCustomSort($('#tblAssessmentRules'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Rules Found');
});
//function jsfn_AssessmentRuleDetails(aruleID,pageId) {
//    var vData = {
//        newId: aruleID,
//    };

//    jsfn_ShowLoading();
//    jsfn_ajaxPost('/OperationManager/GISDetails', vData, jsfn_ShowAssessmentRuleDetailResponse);
//}
//$(document).ready(function () {
//    debugger
//    console.log("i reach here");
//    $('#dvSearchData').hide();
//    $.fn.datepicker.defaults.format = "yyyy-mm-dd";

//    $("#txtFromDate").datepicker({
//        autoclose: true
//    }).on('changeDate', function (selected) {
//        var minDate = new Date(selected.date.valueOf());
//        $('#txtToDate').datepicker('setStartDate', minDate);
//    });

//    $("#txtToDate").datepicker({
//        autoclose: true,
//    }).on('changeDate', function (selected) {
//        var maxDate = new Date(selected.date.valueOf());
//        $('#txtFromDate').datepicker('setEndDate', maxDate);
//    });


//    $("#btnSearchNowNow").click(function () {
//        console.log("i reach here");
//        var vClientID = $("#ClientID").val();
//        var vpageNo = $("#pageNo").val();
//        var vFromDate = $("#txtFromDate").val() !== '' ? $("#txtFromDate").val() : '';
//        var vToDate = $("#txtToDate").val() !== '' ? $("#txtToDate").val() : '';

//        var vUrl = '/OperationManager/CallGISApi?fromDate=' + vFromDate + '&toDate=' + vToDate + '&pageNo=' + vpageNo + '&ClientID=' + vClientID;
//        var win = window.open(vUrl, '_blank');
//        win.focus();
//    });

//});

//function jsfn_ShowData() {
//    $('#dvSearchData').show();
//    jsfn_bindTable();
//}

//function jsfn_bindTable() {
//    var vColumnsList = [{
//        "orderable": true
//    }, {
//        "orderable": true
//    }, {
//        "orderable": true
//    }, {
//        "orderable": true
//    }, {
//        "orderable": false
//    }];

//    var vSortOrder = [];

//    jsfn_ConvertToDatableWithCustomSort($('#tblIndividual'), vColumnsList, 'No Individual Found', vSortOrder);
//    $(".collapse").click();
//}