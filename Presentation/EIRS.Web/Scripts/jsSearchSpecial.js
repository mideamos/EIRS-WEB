﻿$(document).ready(function () {
    $('#dvSearchData').hide();
    jsfn_bindTable();

    $("#btnSearch").click(function () { return jsfn_SearchValidation('frmSearchSpecial') });
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

    jsfn_ConvertToDatableWithCustomSort($('#tblSpecial'), vColumnsList, 'No Special Found', vSortOrder);
    $(".collapse").click();
}