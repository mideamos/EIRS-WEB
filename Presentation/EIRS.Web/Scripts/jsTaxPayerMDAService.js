var vMDAServiceTable;

$(document).ready(function () {
    vMDAServiceTable = $('#tblMDAService').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/Base/GetMDAServiceData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [{
            "data": "TaxYear"
        }, {
            "data": "MDAServiceName"
        }, {
            "data": "RuleRunName"
        }, {
            "data": "PaymentFrequencyName"
        }, {
            "data": "ServiceAmount"
        }, {
            "data": "MDAServiceID"
        }],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(5)', nRow).html('<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_MDAServiceDetails(' + aData["MDAServiceID"] + ')">Quick View</a></li><li><a onclick="javascript:jsfn_MDAServiceItemList(' + aData["MDAServiceID"] + ')">View Items </a>'
                + '</li></ul ></div>');
            var vServiceAmount = aData["ServiceAmount"] != null ? aData["ServiceAmount"] : 0;
            $('td:eq(4)', nRow).html(vServiceAmount.formatMoney());
        },
    });
});

function jsfn_FilterMDAServiceTable(vTaxYear) {
    vMDAServiceTable.search(vTaxYear).draw();
}

function jsfn_MDAServiceDetails(MDAServiceID) {

    var vData = {
        MDAServiceID: MDAServiceID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetMDAServiceDetails', vData, jsfn_MDAServiceDetailResponse)
}

function jsfn_MDAServiceDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvMDAServiceModal').modal('show');
        $('#dvMDAServiceCode').html(data.MDAServiceDetails.MDAServiceCode);
        $('#dvMDAServiceName').html(data.MDAServiceDetails.MDAServiceName);
        $('#dvMDARuleRun').html(data.MDAServiceDetails.RuleRunName);
        $('#dvMDAFrequency').html(data.MDAServiceDetails.PaymentFrequencyName);
        $('#dvMDATaxYear').html(data.MDAServiceDetails.TaxYear);
        $('#dvMDASettlementMethod').html(data.MDAServiceDetails.SettlementMethodNames);
        $('#dvMDAPaymentOption').html(data.MDAServiceDetails.PaymentOptionName);
        $('#dvMDAStatus').html(data.MDAServiceDetails.ActiveText);
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
    jsfn_ajaxPost('/Base/GetMDAServiceItemList', vData, jsfn_MDAServiceItemResponse);
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
