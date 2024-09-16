jQuery(document).ready(function () {
    jsfn_bindTable();
});

function jsfn_bindTable() {
    var vColumnList =
        [
            { 'orderable': true },
            { 'orderable': true }
        ];
    var vSortOrder = [];
    jsfn_ConvertToDatableWithCustomSort($('#tblWasteManagement'), vColumnList, 'No Waste Management Found', vSortOrder);
    $('.collapse').click();
}