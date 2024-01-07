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
    jsfn_ConvertToDatableWithCustomSort($('#tblPresumptiveTaxes'), vColumnList, 'No Presumptive Taxes Found', vSortOrder);
    $('.collapse').click();
}