jQuery(document).ready(function () {
    jsfn_bindTable();
});

function jsfn_bindTable() {
    debugger
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

    jsfn_ConvertToDatableWithCustomSort($('#tblAssessmentRules'), vColumnsList, 'No Assessment Rule Found', vSortOrder);
}