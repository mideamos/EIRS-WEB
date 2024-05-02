$(document).ready(function () {
    console.log("i reach here ty");
    var vSortOrder = [];
    var vColumnsList = [{ "orderable": true },{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }];
    jsfn_ConvertToDatableWithCustomSort($('#tblNewRequest'), vColumnsList, 'No Tcc Record Found',0);

});
