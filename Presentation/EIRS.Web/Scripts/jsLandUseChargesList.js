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
    jsfn_ConvertToDatableWithCustomSort($('#tblLand'), vColumnList, 'No Land Use Charges Found', vSortOrder);
    $('.collapse').click();
}