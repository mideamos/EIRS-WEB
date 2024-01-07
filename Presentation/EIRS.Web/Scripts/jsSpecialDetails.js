var vAssetTable, vAssessmentRuleTable;

$(document).ready(function () {
    vAssetTable = $('#tblAssetList').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/CaptureSpecial/GetTaxPayerAsset",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status === 401) {
                    window.location.href = '/Login/Special';
                }
            },
            "data": function (data) {
                data.TaxPayerID = $("#hdnTaxPayerID").val();
            }
        },
        "columns": [{
            "data": "AssetTypeName"
        }, {
            "data": "AssetName"
        }, {
            "data": "TaxPayerRoleName"
        }, {
            "data": "TPAID"
        }],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(3)', nRow).html('<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_ShowAssetDetails(' + aData["TPAID"] + ')">'
                + 'Quick View</a></li></ul></div>');
        },
    });
    vAssessmentRuleTable = $('#tblAssessmentRules').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/CaptureSpecial/GetAssessmentRuleInformation",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status === 401) {
                    window.location.href = '/Login/Special';
                }
            },
            "data": function (data) {
                data.TaxPayerID = $("#hdnTaxPayerID").val();
            }
        },
        "columns": [{
            "data": "TaxYear"
        }, {
            "data": "AssessmentRuleName"
        }, {
            "data": "AssessmentAmount"
        }, {
            "data": "BilledAmount"
        }, {
            "data": "AssessmentRuleID"
        }],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(2)', nRow).html(aData["AssessmentAmount"].formatMoney());
            $('td:eq(3)', nRow).html(aData["BilledAmount"].formatMoney());
            $('td:eq(6)', nRow).html('<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_AssessmentRuleDetails(' + aData["AssessmentRuleID"] + ')">Quick View</a></li>'
                + '<li> <a onclick="javascript:jsfn_AssessmentRuleItemList(' + aData["AssessmentRuleID"] + ')">View Items </a></li >'
                + '<li><a onclick="javascript:jsfn_ProfileDetails(' + aData["AssessmentRuleID"] + ')">View Profiles </a></li></ul>');
        }
    });
    vBillTable = $('#tblBillList').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/CaptureSpecial/GetTaxPayerBill",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status === 401) {
                    window.location.href = '/Login/Special';
                }
            },
            "data": function (data) {
                data.TaxPayerID = $("#hdnTaxPayerID").val();
            }
        },
        "columns": [{
            "data": "BillDate"
        }, {
            "data": "BillTypeName"
        }, {
            "data": "BillRefNo"
        }, {
            "data": "BillAmount"
        }, {
            "data": "SettlementStatusName"
        }, {
            "data": "BillID"
        }],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var vBillDate = new Date(parseInt(aData["BillDate"].substr(6)));
            $('td:eq(0)', nRow).html(vBillDate.format("dd-mmm-yyyy"));
            $('td:eq(3)', nRow).html(aData["BillAmount"].formatMoney());
            $('td:eq(5)', nRow).html('<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                + '<ul class="dropdown-menu"><li><a onclick="javascript:jsfn_BillDetails(' + aData["BillID"] + ',' + aData["BillTypeID"] + ')">Quick View</a></li>'
                + (aData["SettlementStatusID"] < 3 ? '<li>' + (aData["BillTypeID"] === 1 ? '<a href="/CaptureSpecial/EditAssessment/' + $("#hdnTaxPayerID").val() + '?name=' + $("#hdnTaxPayerRIN").val() + '&aid=' + aData["BillID"] + '">Edit Bill</a>' : '<a href="/CaptureSpecial/EditServiceBill/' + $("#hdnTaxPayerID").val() + '?name=' + $("#hdnTaxPayerRIN").val() + '&sbid=' + aData["BillID"] + '">Edit Bill</a>') + '</li>' : '')
                + '<li><a href="/CaptureSpecial/BillDetail/' + $("#hdnTaxPayerID").val() + '?name=' + $("#hdnTaxPayerRIN").val() + '&billid=' + aData["BillID"] + '&billrefno=' + aData["BillRefNo"] + '">View Bill</a></li>'
                + '<li><a href="/CaptureSpecial/GenerateBill/' + $("#hdnTaxPayerID").val() + '?name=' + $("#hdnTaxPayerRIN").val() + '&billid=' + aData["BillID"] + '&billrefno=' + aData["BillRefNo"] + '">Generate PDF</a></li>'
                + '<li><a onclick="javascript:jsfn_BillPaymentDetails(' + aData["BillID"] + ', ' + aData["BillTypeID"] + ')">View Payments</a></li></ul>'
            );
        }
    });
    vMDAServiceTable = $('#tblMDAServiceList').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/CaptureSpecial/GetMDAServiceInformation",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status === 401) {
                    window.location.href = '/Login/Special';
                }
            },
            "data": function (data) {
                data.TaxPayerID = $("#hdnTaxPayerID").val();
            }
        },
        "columns": [{
            "data": "TaxYear"
        }, {
            "data": "MDAServiceName"
        }, {
            "data": "ServiceAmount"
        }, {
            "data": "BilledAmount"
        }, {
            "data": "MDAServiceID"
        }],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(2)', nRow).html(aData["ServiceAmount"].formatMoney());
            $('td:eq(3)', nRow).html(aData["BilledAmount"].formatMoney());
            $('td:eq(4)', nRow).html('<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_MDAServiceDetails(' + aData["MDAServiceID"] + ')">Quick View</a></li>'
                + '<li> <a onclick="javascript:jsfn_MDAServiceItemList(' + aData["MDAServiceID"] + ')">View Items </a></li></ul>');
            
        }
    });

    vPaymentTable = $('#tblPaymentList').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/CaptureSpecial/GetTaxPayerPayment",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status === 401) {
                    window.location.href = '/Login/Special';
                }
            },
            "data": function (data) {
                data.TaxPayerID = $("#hdnTaxPayerID").val();
            }
        },
        "columns": [{
            "data": "PaymentDate"
        }, {
            "data": "PaymentTypeName"
        }, {
            "data": "PaymentRefNo"
        }, {
            "data": "Amount"
        }, {
            "data": "PaymentID"
        }],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var vBillDate = new Date(parseInt(aData["PaymentDate"].substr(6)));
            $('td:eq(0)', nRow).html(vBillDate.format("dd-mmm-yyyy"));
            $('td:eq(3)', nRow).html(aData["Amount"].formatMoney());
            $('td:eq(4)', nRow).html('<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                + '<ul class="dropdown-menu"><li><a onclick="javascript:jsfn_PaymentDetails(' + aData["PaymentID"] + ',' + aData["PaymentTypeID"] + ')">Quick View</a></li></ul>');
        },
    });
});

function jsfn_AssessmentRuleDetails(aruleID) {
    var vData = {
        AssessmentRuleID: aruleID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetAssessmentRuleDetails', vData, jsfn_ShowAssessmentRuleDetailResponse);
}

function jsfn_ShowAssessmentRuleDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentRuleModal').modal('show');
        $('#dvAssessmentRuleRefNo').html(data.AssessmentRuleDetails.AssessmentRuleCode);
        $('#dvAssessmentRuleName').html(data.AssessmentRuleDetails.AssessmentRuleName);
        $('#dvRuleRun').html(data.AssessmentRuleDetails.RuleRunName);
        $('#dvFrequency').html(data.AssessmentRuleDetails.PaymentFrequencyName);
        $('#dvTaxYear').html(data.AssessmentRuleDetails.TaxYear);
        $('#dvSettlementMethod').html(data.AssessmentRuleDetails.SettlementMethodNames);
        $('#dvPaymentOption').html(data.AssessmentRuleDetails.PaymentOptionName);
        $('#dvStatus').html(data.AssessmentRuleDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_FilterAssessmentRuleTable(vTaxYear) {
    vAssessmentRuleTable.fnFilter(vTaxYear, 1, false, false, false, false);
}

function jsfn_AssessmentRuleItemList(aruleID) {
    var vData = {
        AssessmentRuleID: aruleID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetAssessmentRuleItemList', vData, jsfn_AssessmentRuleItemResponse);
}

function jsfn_AssessmentRuleItemResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentRuleItemModal').modal('show');
        $('#dvAssessmentRuleItemData').html(data.AssessmentRuleItemList);
        jsfn_ConvertToDatableWithCustomSort($('#tblAssessmentRuleItem'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Assessment Items Found');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ProfileDetails(prfID) {
    var vData = {
        ProfileID: prfID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetProfileDetails', vData, jsfn_ShowProfileDetailResponse);
}

function jsfn_ShowProfileDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvProfileModal').modal('show');
        $('#dvProfileRefNo').html(data.ProfileDetails.ProfileReferenceNo);
        $('#dvAssetType').html(data.ProfileDetails.AssetTypeName);
        $('#dvTaxPayerType').html(data.ProfileDetails.TaxPayerTypeNames);
        $('#dvTaxPayerRole').html(data.ProfileDetails.TaxPayerRoleNames);
        $('#dvProfileDescription').html(data.ProfileDetails.ProfileDescription);
        $('#dvAssetStatus').html(data.ProfileDetails.AssetTypeStatusName);
        $('#dvProfileStatus').html(data.ProfileDetails.ActiveText);
        $('#dvProfileSector').html(data.ProfileDetails.ProfileSectorNames);
        $('#dvProfileSubSector').html(data.ProfileDetails.ProfileSubSectorNames);
        $('#dvProfileGroup').html(data.ProfileDetails.ProfileGroupNames);
        $('#dvProfileSubGroup').html(data.ProfileDetails.ProfileSubGroupNames);
        $('#dvSectorElement').html(data.ProfileDetails.ProfileSectorElementNames);
        $('#dvSectorSubElement').html(data.ProfileDetails.ProfileSectorSubElementNames);
        $('#dvProfileAttribute').html(data.ProfileDetails.ProfileAttributeNames);
        $('#dvProfileSubAttribute').html(data.ProfileDetails.ProfileSubAttributeNames);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_FilterMDAServiceTable(vTaxYear) {
    vMDAServiceTable.fnFilter(vTaxYear, 1, false, false, false, false);
}

function jsfn_MDAServiceDetails(MDAServiceID) {

    var vData = {
        MDAServiceID: MDAServiceID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetMDAServiceDetails', vData, jsfn_MDAServiceDetailResponse);
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

function jsfn_PaymentDetails(paymentid, paymenttype) {

    if (paymenttype === 1) {
        var vData = {
            SettlementID: paymentid,
        };

        jsfn_ShowLoading();
        jsfn_ajaxPost('/Base/GetSettlementDetails', vData, jsfn_SettlementDetailResponse);
    }
    else if (paymenttype === 2) {
        var vData = {
            PaymentAccountID: paymentid,
        };

        jsfn_ShowLoading();
        jsfn_ajaxPost('/Base/GetPoADetails', vData, jsfn_PaymentAccountDetailResponse);
    }


}

function jsfn_PaymentAccountDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvPaymentAccountModal').modal('show');
        $('#dvPaymentAccountRefNo').html(data.PaymentAccountDetails.PaymentRefNo);
        $('#dvPaymentAccountDate').html(jsfn_FormatJsonDate(data.PaymentAccountDetails.PaymentDate));
        $('#dvPATaxPayerTypeName').html(data.PaymentAccountDetails.TaxPayerTypeName);
        $('#dvPATaxPayerRIN').html(data.PaymentAccountDetails.TaxPayerRIN);
        $('#dvPATaxPayerName').html(data.PaymentAccountDetails.TaxPayerName);
        $('#dvPaymentAccountAmount').html(data.PaymentAccountDetails.Amount.formatMoney(2, '₦'));
        $('#dvPATransactionReferenceNo').html(data.PaymentAccountDetails.TransactionReferenceNo);
        $('#dvRevenueStreamName').html(data.PaymentAccountDetails.RevenueStreamName);
        $('#dvNotes').html(data.PaymentAccountDetails.Notes);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_SettlementDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvSettlementModal').modal('show');
        $('#dvSettlementRefNo').html(data.SettlementDetails.SettlementRefNo);
        $('#dvSettlementDate').html(jsfn_FormatJsonDate(data.SettlementDetails.SettlementDate));
        $('#dvTaxPayerTypeName').html(data.SettlementDetails.TaxPayerTypeName);
        $('#dvTaxPayerRIN').html(data.SettlementDetails.TaxPayerRIN);
        $('#dvTaxPayerName').html(data.SettlementDetails.TaxPayerName);
        $('#dvSettlementAmount').html(data.SettlementDetails.SettlementAmount.formatMoney(2, '₦'));
        $('#dvASBRefNo').html(data.SettlementDetails.ASRefNo);
        $('#dvTransactionReferenceNo').html(data.SettlementDetails.TransactionReferenceNo);
        $('#dvSettlementNotes').html(data.SettlementDetails.SettlementNotes);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}


function jsfn_ChangeStatus(Specialid) {
    var vData = {
        SpecialID: Specialid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/CaptureSpecial/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}