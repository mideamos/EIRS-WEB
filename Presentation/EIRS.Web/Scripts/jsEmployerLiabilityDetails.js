$(document).ready(function () {
    jsfn_ConvertToDatableWithCustomSort($('#tblAssetList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Assoicated Assets Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblBillList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Assoicated Bills Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblAssessmentRules'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Assoicated Rules Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblPaymentList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Assoicated Payments Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblStaffList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Assoicated Staff Found');
});