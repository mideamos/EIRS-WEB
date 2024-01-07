jQuery(document).ready(function () {
    if ($('#tblAssessList').length > 0) {
        $("#tblAssessList").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/Assess/GetAllBills",
                "type": "POST",
                "datatype": "json",
                "error": function (jqXHR, exception) {
                    if (jqXHR.status === 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                {
                    "data": "BillDate", "orderable": true, "name": "BillDate",
                    "render": function (data, type, st) {
                        var vBillDate = new Date(parseInt(st.BillDate.substr(6)));
                        return vBillDate.format("dd-mmm-yyyy");
                    }
                },
                { "data": "BillRefNo", "orderable": true, "name": "BillRefNo" },
                { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
                {
                    "data": "BillAmount", "orderable": true, "name": "BillAmount",
                    "render": function (data, type, st) {
                        return st.BillAmount.formatMoney();
                    }
                },
                { "data": "SettlementStatusName", "orderable": true, "name": "SettlementStatusName" },
                {
                    "data": "", "orderable": false, "name": "Action",
                    "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                            + '<ul class="dropdown-menu"><li><a onclick="javascript:jsfn_BillDetails(' + st.BillID + ',' + st.BillTypeID + ')">Quick View</a></li>'
                            + '<li><a onclick="javascript:jsfn_BillPaymentDetails(' + st.BillID + ', ' + st.BillTypeID + ')">View Payments</a></li></ul>';
                    }
                }
            ],
            "language": {
                "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
            },
            "order": [[0, "desc"]]
        });
    }

    if ($('#tblSettledBillList').length > 0) {
        $("#tblSettledBillList").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/Assess/GetSettled",
                "type": "POST",
                "datatype": "json",
                "error": function (jqXHR, exception) {
                    if (jqXHR.status === 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                {
                    "data": "BillDate", "orderable": true, "name": "BillDate",
                    "render": function (data, type, st) {
                        var vBillDate = new Date(parseInt(st.BillDate.substr(6)));
                        return vBillDate.format("dd-mmm-yyyy");
                    }
                },
                { "data": "BillRefNo", "orderable": true, "name": "BillRefNo" },
                { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
                {
                    "data": "BillAmount", "orderable": true, "name": "BillAmount",
                    "render": function (data, type, st) {
                        return st.BillAmount.formatMoney();
                    }
                },
                { "data": "SettlementStatusName", "orderable": true, "name": "SettlementStatusName" },
                {
                    "data": "", "orderable": false, "name": "Action",
                    "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                            + '<ul class="dropdown-menu"><li><a onclick="javascript:jsfn_BillDetails(' + st.BillID + ',' + st.BillTypeID + ')">Quick View</a></li>'
                            + '<li><a onclick="javascript:jsfn_BillPaymentDetails(' + st.BillID + ', ' + st.BillTypeID + ')">View Payments</a></li></ul>';
                    }
                }
            ],
            "language": {
                "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
            },
            "order": [[0, "desc"]]
        });
    }

    if ($('#tblUnSettledBillList').length > 0) {
        $("#tblUnSettledBillList").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/Assess/GetUnsettled",
                "type": "POST",
                "datatype": "json",
                "error": function (jqXHR, exception) {
                    if (jqXHR.status === 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                {
                    "data": "BillDate", "orderable": true, "name": "BillDate",
                    "render": function (data, type, st) {
                        var vBillDate = new Date(parseInt(st.BillDate.substr(6)));
                        return vBillDate.format("dd-mmm-yyyy");
                    }
                },
                { "data": "BillRefNo", "orderable": true, "name": "BillRefNo" },
                { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
                {
                    "data": "BillAmount", "orderable": true, "name": "BillAmount",
                    "render": function (data, type, st) {
                        return st.BillAmount.formatMoney();
                    }
                },
                { "data": "SettlementStatusName", "orderable": true, "name": "SettlementStatusName" },
                {
                    "data": "", "orderable": false, "name": "Action",
                    "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                            + '<ul class="dropdown-menu"><li><a onclick="javascript:jsfn_BillDetails(' + st.BillID + ',' + st.BillTypeID + ')">Quick View</a></li>'
                            + '<li><a onclick="javascript:jsfn_BillPaymentDetails(' + st.BillID + ', ' + st.BillTypeID + ')">View Payments</a></li></ul>';
                    }
                }
            ],
            "language": {
                "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
            },
            "order": [[0, "desc"]]
        });
    }

    if ($('#tblPSettledBillList').length > 0) {
        $("#tblPSettledBillList").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/Assess/GetPartialSettled",
                "type": "POST",
                "datatype": "json",
                "error": function (jqXHR, exception) {
                    if (jqXHR.status === 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                {
                    "data": "BillDate", "orderable": true, "name": "BillDate",
                    "render": function (data, type, st) {
                        var vBillDate = new Date(parseInt(st.BillDate.substr(6)));
                        return vBillDate.format("dd-mmm-yyyy");
                    }
                },
                { "data": "BillRefNo", "orderable": true, "name": "BillRefNo" },
                { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
                {
                    "data": "BillAmount", "orderable": true, "name": "BillAmount",
                    "render": function (data, type, st) {
                        return st.BillAmount.formatMoney();
                    }
                },
                {
                    "data": "SettlementAmount", "orderable": true, "name": "SettlementAmount",
                    "render": function (data, type, st) {
                        return st.SettlementAmount.formatMoney();
                    }
                },
                { "data": "SettlementStatusName", "orderable": true, "name": "SettlementStatusName" },
                {
                    "data": "", "orderable": false, "name": "Action",
                    "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                            + '<ul class="dropdown-menu"><li><a onclick="javascript:jsfn_BillDetails(' + st.BillID + ',' + st.BillTypeID + ')">Quick View</a></li>'
                            + '<li><a onclick="javascript:jsfn_BillPaymentDetails(' + st.BillID + ', ' + st.BillTypeID + ')">View Payments</a></li></ul>';
                    }
                }
            ],
            "language": {
                "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
            },
            "order": [[0, "desc"]]
        });
    }
});

function jsfn_BillDetails(billid, billtype) {

    if (billtype === 1) {
        var vData = {
            AssessmentID: billid,
        };

        jsfn_ShowLoading();
        jsfn_ajaxPost('/Base/GetAssessmentDetails', vData, jsfn_AssessmentDetailResponse)
    }
    else if (billtype === 2) {
        var vData = {
            ServiceBillID: billid,
        };

        jsfn_ShowLoading();
        jsfn_ajaxPost('/Base/GetServiceBillDetails', vData, jsfn_ServiceBillDetailResponse)
    }


}

function jsfn_AssessmentDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentModal').modal('show');
        $('#dvAssessmentRefNo').html(data.AssessmentDetails.AssessmentRefNo);
        $('#dvAssessmentDate').html(jsfn_FormatJsonDate(data.AssessmentDetails.AssessmentDate));
        $('#dvATaxPayerTypeName').html(data.AssessmentDetails.TaxPayerTypeName);
        $('#dvATaxPayerName').html(data.AssessmentDetails.TaxPayerName);
        $('#dvAssessmentAmount').html(data.AssessmentDetails.AssessmentAmount.formatMoney(2, '₦'));
        $('#dvASettlementDueDate').html(jsfn_FormatJsonDate(data.AssessmentDetails.SettlementDueDate));
        $('#dvASettlementStatus').html(data.AssessmentDetails.SettlementStatusName);
        $('#dvASettlementDate').html(data.AssessmentDetails.SettlementDate != null ? jsfn_FormatJsonDate(data.AssessmentDetails.SettlementDate) : "-");
        $('#dvAssessmentNotes').html(data.AssessmentDetails.AssessmentNotes);
        $('#dvAStatus').html(data.AssessmentDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ServiceBillDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvServiceBillModal').modal('show');
        $('#dvServiceBillRefNo').html(data.ServiceBillDetails.ServiceBillRefNo);
        $('#dvServiceBillDate').html(jsfn_FormatJsonDate(data.ServiceBillDetails.ServiceBillDate));
        $('#dvSTaxPayerTypeName').html(data.ServiceBillDetails.TaxPayerTypeName);
        $('#dvSTaxPayerName').html(data.ServiceBillDetails.TaxPayerName);
        $('#dvServiceBillAmount').html(data.ServiceBillDetails.ServiceBillAmount.formatMoney(2, '₦'));
        $('#dvSSettlementDueDate').html(jsfn_FormatJsonDate(data.ServiceBillDetails.SettlementDueDate));
        $('#dvSSettlementStatus').html(data.ServiceBillDetails.SettlementStatusName);
        $('#dvSSettlementDate').html(data.ServiceBillDetails.SettlementDate != null ? jsfn_FormatJsonDate(data.ServiceBillDetails.SettlementDate) : "-");
        $('#dvSStatus').html(data.ServiceBillDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BillPaymentDetails(billid, billtype) {
    var vData = {
        BillID: billid,
        BillTypeID: billtype
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetBillPaymentList', vData, jsfn_BillPaymentDetailResponse)
}

function jsfn_BillPaymentDetailResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvBillPaymentModal').modal('show');
        $('#dvBillPaymentData').html(data.BillPaymentList);
        jsfn_ConvertToDatableWithCustomSort($('#tblBillPayment'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Payment History Found', []);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
