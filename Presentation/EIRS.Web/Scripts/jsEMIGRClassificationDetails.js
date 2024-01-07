$(document).ready(function () {

    $("#tblPDMainAuthorizedData").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/EMIGRClassification/LoadPDMainAuthorizedData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.ClassificationID = $("#hdnClassificationID").val();
            },
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "PaymentRefNumber", "orderable": true, "name": "PaymentRefNumber", "width": "25%" },
            { "data": "PaymentDateTime", "orderable": true, "name": "PaymentDateTime", "width": "25%" },
            { "data": "CustomerName", "orderable": true, "name": "CustomerName", "width": "25%" },
            { "data": "RIN", "orderable": true, "name": "RIN", "width": "25%" },
            { "data": "Bank", "orderable": true, "name": "Bank", "width": "25%" },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "width": "25%", "render": function (data, type, st) {
                    return st.Amount.formatMoney();
                }
            },
            {
                "data": "", "orderable": false, "name": "Action",
                "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                        + '<ul class="dropdown-menu">'
                        + '<li><a onclick="javascript:jsfn_ShowPDMainAuthorizedDetails(' + st["PDMAID"] +')">View Details</a></li>'
                        + '</ul>';
                }
            }
        ],
        "order": [[1, "desc"]],
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        }
    });

    $("#tblPDMainPendingData").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/EMIGRClassification/LoadPDMainPendingData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.ClassificationID = $("#hdnClassificationID").val();
            },
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "PaymentRefNumber", "orderable": true, "name": "PaymentRefNumber", "width": "25%" },
            { "data": "PaymentDateTime", "orderable": true, "name": "PaymentDateTime", "width": "25%" },
            { "data": "CustomerName", "orderable": true, "name": "CustomerName", "width": "25%" },
            { "data": "RIN", "orderable": true, "name": "RIN", "width": "25%" },
            { "data": "Bank", "orderable": true, "name": "Bank", "width": "25%" },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "width": "25%", "render": function (data, type, st) {
                    return st.Amount.formatMoney();
                }
            },
            {
                "data": "", "orderable": false, "name": "Action",
                "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                        + '<ul class="dropdown-menu">'
                        + '<li><a onclick="javascript:jsfn_ShowPDMainPendingDetails(' + st["PDMPID"] +')">View Details</a></li>'
                        + '</ul>';
                }
            }
        ],
        "order": [[1, "desc"]],
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        }
    });

    $("#tblPDMVAAuthorizedData").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/EMIGRClassification/LoadPDMVAAuthorizedData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.ClassificationID = $("#hdnClassificationID").val();
            },
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "PaymentRefNumber", "orderable": true, "name": "PaymentRefNumber", "width": "25%" },
            { "data": "PaymentDateTime", "orderable": true, "name": "PaymentDateTime", "width": "25%" },
            { "data": "CustomerName", "orderable": true, "name": "CustomerName", "width": "25%" },
            { "data": "RIN", "orderable": true, "name": "RIN", "width": "25%" },
            { "data": "Bank", "orderable": true, "name": "Bank", "width": "25%" },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "width": "25%", "render": function (data, type, st) {
                    return st.Amount.formatMoney();
                }
            },
            {
                "data": "", "orderable": false, "name": "Action",
                "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                        + '<ul class="dropdown-menu">'
                        + '<li><a onclick="javascript:jsfn_ShowPDMVAAuthorizedDetails(' + st["PDMVAID"] +')">View Details</a></li>'
                        + '</ul>';
                }
            }
        ],
        "order": [[1, "desc"]],
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        }
    });

    $("#tblPDMVAPendingData").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/EMIGRClassification/LoadPDMVAPendingData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.ClassificationID = $("#hdnClassificationID").val();
            },
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "PaymentRefNumber", "orderable": true, "name": "PaymentRefNumber", "width": "25%" },
            { "data": "PaymentDateTime", "orderable": true, "name": "PaymentDateTime", "width": "25%" },
            { "data": "CustomerName", "orderable": true, "name": "CustomerName", "width": "25%" },
            { "data": "RIN", "orderable": true, "name": "RIN", "width": "25%" },
            { "data": "Bank", "orderable": true, "name": "Bank", "width": "25%" },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "width": "25%", "render": function (data, type, st) {
                    return st.Amount.formatMoney();
                }
            },
            {
                "data": "", "orderable": false, "name": "Action",
                "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                        + '<ul class="dropdown-menu">'
                        + '<li><a onclick="javascript:jsfn_ShowPDMVAPendingDetails(' + st["PDMVPID"] +')">View Details</a></li>'
                        + '</ul>';
                }
            }
        ],
        "order": [[1, "desc"]],
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        }
    });

    $("#tblBankStatementData").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/EMIGRClassification/LoadBSData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.ClassificationID = $("#hdnClassificationID").val();
            },
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "PaymentRefNumber", "orderable": true, "name": "PaymentRefNumber", "width": "25%" },
            { "data": "PaymentDateTime", "orderable": true, "name": "PaymentDateTime", "width": "25%" },
            { "data": "CustomerName", "orderable": true, "name": "CustomerName", "width": "25%" },
            { "data": "Category", "orderable": true, "name": "Category", "width": "25%" },
            { "data": "RevenueHead", "orderable": true, "name": "RevenueHead", "width": "25%" },
            { "data": "Bank", "orderable": true, "name": "Bank", "width": "25%" },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "width": "25%", "render": function (data, type, st) {
                    return st.Amount.formatMoney();
                }
            },
            {
                "data": "", "orderable": false, "name": "Action",
                "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                        + '<ul class="dropdown-menu">'
                        + '<li><a onclick="javascript:jsfn_ShowBankStatementDetails(' + st["BSID"]+')">View Details</a></li>'
                        + '</ul>';
                }
            }
        ],
        "order": [[1, "desc"]],
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        }
    });
});

function jsfn_ShowPDMainAuthorizedDetails(eid) {
    var vData = {
        dsid: 1,
        entryid: eid
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/EMIGRClassification/GetEntryDetails', vData, jsfn_ShowPDMainAuthorizedDetailResponse);
}

function jsfn_ShowPDMainAuthorizedDetailResponse(data) {
    jsfn_HideLoading();
    $('#dvPayDirectModal').modal('show');
    $('#dvPayDirectModalLabel').html('Pay Direct - Main – Authorized Details');
    $('#dvPDTaxMonth').html(data.TaxYear);
    $('#dvPDTaxYear').html(GetMonthName(data.TaxMonth));
    $('#dvPDPaymentRefNumber').html(data.PaymentRefNumber);
    $('#dvPDPaymentDateTime').html(data.PaymentDateTime);
    $('#dvAssessmentReference').html(data.AssessmentReference);
    $('#dvReceiptNo').html(data.ReceiptNo);
    $('#dvRIN').html(data.RIN);
    $('#dvPDCustomerName').html(data.CustomerName);
    $('#dvRevenueItem').html(data.RevenueItem);
    $('#dvPDAmount').html(data.Amount.formatMoney(2,'₦'));
    $('#dvPaymentMethod').html(data.PaymentMethod);
    $('#dvDepositSlip').html(data.DepositSlip);
    $('#dvChequeValueDate').html(data.ChequeValueDate);
    $('#dvPDBank').html(data.Bank);
    $('#dvAdditionalInfo').html(data.AdditionalInfo);
    $('#dvBankBranch').html(data.BankBranch);
    $('#dvTaxPayerType').html(data.TaxPayerType);
    $('#dvPaymentCode').html(data.PaymentCode);
    $('#dvRetrievalRefNumber').html(data.RetrievalRefNumber);
    $('#dvAuthID').html(data.AuthID);

}

function jsfn_ShowPDMainPendingDetails(eid) {
    var vData = {
        dsid: 2,
        entryid: eid
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/EMIGRClassification/GetEntryDetails', vData, jsfn_ShowPDMainPendingDetailResponse);
}

function jsfn_ShowPDMainPendingDetailResponse(data) {
    jsfn_HideLoading();
    $('#dvPayDirectModal').modal('show');
    $('#dvPayDirectModalLabel').html('Pay Direct - Main – Pending Details');
    $('#dvPDTaxMonth').html(data.TaxYear);
    $('#dvPDTaxYear').html(GetMonthName(data.TaxMonth));
    $('#dvPDPaymentRefNumber').html(data.PaymentRefNumber);
    $('#dvPDPaymentDateTime').html(data.PaymentDateTime);
    $('#dvAssessmentReference').html(data.AssessmentReference);
    $('#dvReceiptNo').html(data.ReceiptNo);
    $('#dvRIN').html(data.RIN);
    $('#dvPDCustomerName').html(data.CustomerName);
    $('#dvRevenueItem').html(data.RevenueItem);
    $('#dvPDAmount').html(data.Amount.formatMoney(2, '₦'));
    $('#dvPaymentMethod').html(data.PaymentMethod);
    $('#dvDepositSlip').html(data.DepositSlip);
    $('#dvChequeValueDate').html(data.ChequeValueDate);
    $('#dvPDBank').html(data.Bank);
    $('#dvAdditionalInfo').html(data.AdditionalInfo);
    $('#dvBankBranch').html(data.BankBranch);
    $('#dvTaxPayerType').html(data.TaxPayerType);
    $('#dvPaymentCode').html(data.PaymentCode);
    $('#dvRetrievalRefNumber').html(data.RetrievalRefNumber);
    $('#dvAuthID').html(data.AuthID);

}

function jsfn_ShowPDMVAAuthorizedDetails(eid) {
    var vData = {
        dsid: 3,
        entryid: eid
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/EMIGRClassification/GetEntryDetails', vData, jsfn_ShowPDMVAAuthorizedDetailResponse);
}

function jsfn_ShowPDMVAAuthorizedDetailResponse(data) {
    jsfn_HideLoading();
    $('#dvPayDirectModal').modal('show');
    $('#dvPayDirectModalLabel').html('Pay Direct - MVA – Authorized Details');
    $('#dvPDTaxMonth').html(data.TaxYear);
    $('#dvPDTaxYear').html(GetMonthName(data.TaxMonth));
    $('#dvPDPaymentRefNumber').html(data.PaymentRefNumber);
    $('#dvPDPaymentDateTime').html(data.PaymentDateTime);
    $('#dvAssessmentReference').html(data.AssessmentReference);
    $('#dvReceiptNo').html(data.ReceiptNo);
    $('#dvRIN').html(data.RIN);
    $('#dvPDCustomerName').html(data.CustomerName);
    $('#dvRevenueItem').html(data.RevenueItem);
    $('#dvPDAmount').html(data.Amount.formatMoney(2, '₦'));
    $('#dvPaymentMethod').html(data.PaymentMethod);
    $('#dvDepositSlip').html(data.DepositSlip);
    $('#dvChequeValueDate').html(data.ChequeValueDate);
    $('#dvPDBank').html(data.Bank);
    $('#dvAdditionalInfo').html(data.AdditionalInfo);
    $('#dvBankBranch').html(data.BankBranch);
    $('#dvTaxPayerType').html(data.TaxPayerType);
    $('#dvPaymentCode').html(data.PaymentCode);
    $('#dvRetrievalRefNumber').html(data.RetrievalRefNumber);
    $('#dvAuthID').html(data.AuthID);

}

function jsfn_ShowPDMVAPendingDetails(eid) {
    var vData = {
        dsid: 4,
        entryid: eid
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/EMIGRClassification/GetEntryDetails', vData, jsfn_ShowPDMVAPendingDetailResponse);
}

function jsfn_ShowPDMVAPendingDetailResponse(data) {
    jsfn_HideLoading();
    $('#dvPayDirectModal').modal('show');
    $('#dvPayDirectModalLabel').html('Pay Direct - MVA – Authorized Details');
    $('#dvPDTaxMonth').html(data.TaxYear);
    $('#dvPDTaxYear').html(GetMonthName(data.TaxMonth));
    $('#dvPDPaymentRefNumber').html(data.PaymentRefNumber);
    $('#dvPDPaymentDateTime').html(data.PaymentDateTime);
    $('#dvAssessmentReference').html(data.AssessmentReference);
    $('#dvReceiptNo').html(data.ReceiptNo);
    $('#dvRIN').html(data.RIN);
    $('#dvPDCustomerName').html(data.CustomerName);
    $('#dvRevenueItem').html(data.RevenueItem);
    $('#dvPDAmount').html(data.Amount.formatMoney(2, '₦'));
    $('#dvPaymentMethod').html(data.PaymentMethod);
    $('#dvDepositSlip').html(data.DepositSlip);
    $('#dvChequeValueDate').html(data.ChequeValueDate);
    $('#dvPDBank').html(data.Bank);
    $('#dvAdditionalInfo').html(data.AdditionalInfo);
    $('#dvBankBranch').html(data.BankBranch);
    $('#dvTaxPayerType').html(data.TaxPayerType);
    $('#dvPaymentCode').html(data.PaymentCode);
    $('#dvRetrievalRefNumber').html(data.RetrievalRefNumber);
    $('#dvAuthID').html(data.AuthID);

}

function jsfn_ShowBankStatementDetails(eid) {
    var vData = {
        dsid: 5,
        entryid: eid
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/EMIGRClassification/GetEntryDetails', vData, jsfn_ShowBankStatementDetailResponse);
}

function jsfn_ShowBankStatementDetailResponse(data) {
    jsfn_HideLoading();
    $('#dvBankStatementModal').modal('show');
    $('#dvBSTaxMonth').html(data.TaxYear);
    $('#dvBSTaxYear').html(GetMonthName(data.TaxMonth));
    $('#dvBSPaymentRefNumber').html(data.PaymentRefNumber);
    $('#dvBSPaymentDateTime').html(data.PaymentDateTime);
    $('#dvBSCustomerName').html(data.CustomerName);
    $('#dvBSCategory').html(data.Category);
    $('#dvBSRevenueHead').html(data.RevenueHead);
    $('#dvBSAmount').html(data.Amount.formatMoney(2, '₦'));
    $('#dvBSBank').html(data.Bank);
    
}