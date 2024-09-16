$(document).ready(function () {
    jsfn_ConvertToDatableWithCustomSort($('#tblIncomeStream'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Income Streams Found');
});

function jsfn_DeleteIncomeStream(RowID) {
    var vData = {
        RowID: RowID === '' ? 0 : RowID
    };

    jsfn_ajaxPost('/RequestPersonalTCC/DeleteIncomeStream', vData, jsfn_DeleteIncomeStreamResponse);
}

function jsfn_DeleteIncomeStreamResponse(data) {
    if (data.success === true) {
        $("#dvIncomeStreamData").html(data.IncomeStreamData);
        jsfn_ConvertToDatableWithCustomSort($('#tblIncomeStream'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Income Streams Found');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

