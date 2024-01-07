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
    jsfn_ConvertToDatableWithCustomSort($('#tblDirectAssessment'), vColumnList, 'No Direct Assessment Found', vSortOrder);
    $('.collapse').click();
}