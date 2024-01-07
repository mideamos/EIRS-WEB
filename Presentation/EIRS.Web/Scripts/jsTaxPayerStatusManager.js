jQuery(document).ready(function () {
    var vColumnsList = [{
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": false
    }];

    var vSortOrder = [];

    jsfn_ConvertToDatableWithCustomSort($('#tblTaxPayerStatus'), vColumnsList, 'No Tax Office Found', vSortOrder);
});