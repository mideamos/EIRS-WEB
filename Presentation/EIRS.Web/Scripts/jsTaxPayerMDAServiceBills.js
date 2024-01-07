jQuery(document).ready(function () {
    jsfn_bindTable();
});

function jsfn_bindTable() {
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

    jsfn_ConvertToDatableWithCustomSort($('#tblServiceBills'), vColumnsList, 'No Service Bills Found', vSortOrder);
}


function jsfn_FilterMDAServiceTable(vTaxYear) {
    vMDAServiceTable.search(vTaxYear).draw();
}

function jsfn_MDAServiceDetails(MDAServiceID) {

    var vData = {
        MDAServiceID: MDAServiceID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/TaxPayerPanel/GetMDAServiceDetails', vData, jsfn_MDAServiceDetailResponse)
}

function jsfn_MDAServiceDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvMDAServiceModal').modal('show');
        $('#dvMDAServiceCode').html(data.MDAServiceDetails.MDAServiceCode);
        $('#dvMDAServiceName').html(data.MDAServiceDetails.MDAServiceName);
        $('#dvRuleRun').html(data.MDAServiceDetails.RuleRunName);
        $('#dvFrequency').html(data.MDAServiceDetails.PaymentFrequencyName);
        $('#dvTaxYear').html(data.MDAServiceDetails.TaxYear);
        $('#dvSettlementMethod').html(data.MDAServiceDetails.SettlementMethodNames);
        $('#dvPaymentOption').html(data.MDAServiceDetails.PaymentOptionName);
        $('#dvStatus').html(data.MDAServiceDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_MDAServiceItemList(MDAServiceID) {
    var vData = {
        MDAServiceID: MDAServiceID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/TaxPayer/GetMDAServiceItemList', vData, jsfn_MDAServiceItemResponse);
}

function jsfn_MDAServiceItemResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvMDAServiceItemModal').modal('show');
        $('#dvMDAServiceItemData').html(data.MDAServiceItemList);
        jsfn_ConvertToDatableWithCustomSort($('#tblMDAServiceItem'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }], 'No MDA Service Items Found');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}