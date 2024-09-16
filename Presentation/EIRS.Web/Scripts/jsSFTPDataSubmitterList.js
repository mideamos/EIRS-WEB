var vDataSubmitterDataTable;

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

jQuery(document).ready(function () {
    vDataSubmitterDataTable = jsfn_ConvertToDatableWithCustomSort($('#tblDataSubmitter'), vColumnsList, 'No Data Submitters Found', vSortOrder);
});