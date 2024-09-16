var vDataSubmissionDataTable;

var vColumnsList = [{
    "orderable": true
}, {
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
    vDataSubmissionDataTable = jsfn_ConvertToDatableWithCustomSort($('#tblDataSubmission'), vColumnsList, 'No Data Submission Found', vSortOrder);
});