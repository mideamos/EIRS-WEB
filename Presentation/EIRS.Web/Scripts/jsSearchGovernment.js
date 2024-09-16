$(document).ready(function () {
    $('#dvSearchData').hide();
    jsfn_bindTable();

    $("#btnSearch").click(function () { return jsfn_SearchValidation('frmSearchGovernment') });
});

function jsfn_ShowData() {
    $('#dvSearchData').show();
    jsfn_bindTable();
}

function jsfn_bindTable() {
    var vColumnsList = [{
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": false
    }];

    var vSortOrder = [];

    jsfn_ConvertToDatableWithCustomSort($('#tblGovernment'), vColumnsList, 'No Government Found', vSortOrder);
    $(".collapse").click();
}
