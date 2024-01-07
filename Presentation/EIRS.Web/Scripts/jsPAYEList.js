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
    jsfn_ConvertToDatableWithCustomSort($('#tblPAYE'), vColumnList, 'No Paye Found', vSortOrder);
    $('.collapse').click();
}