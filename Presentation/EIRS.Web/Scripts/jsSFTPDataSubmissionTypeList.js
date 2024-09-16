var vDataSubmissionTypeDataTable;

var vColumnsList = [{
    "orderable": true
}, {
    "orderable": true
}, {
    "orderable": false
}];

var vSortOrder = [];

jQuery(document).ready(function () {
    vDataSubmissionTypeDataTable = jsfn_ConvertToDatableWithCustomSort($('#tblDataSubmissionType'), vColumnsList, 'No Data Submission Types Found', vSortOrder);
});