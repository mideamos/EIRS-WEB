jQuery(document).ready(function () {
    $('#tblAssessment').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/Assessment/LoadData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "AssessmentRefNo", "orderable": true, "name": "AssessmentRefNo" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            {
                "data": "AssessmentDate", "orderable": true, "name": "AssessmentDate", "render": function (data, type, ast) {
                    if (ast.AssessmentDate != undefined || ast.AssessmentDate != null) {
                        return jsfn_FormatJsonDate(ast.AssessmentDate, "dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            {
                "data": "AssessmentAmount", "orderable": true, "name": "AssessmentAmount", "render": function (data, type, ast) {
                    return ast.AssessmentAmount.formatMoney();
                }
            },
            { "data": "SettlementStatusName", "orderable": true, "name": "SettlementStatusName" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, aData) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_BillDetails(' + aData["AssessmentID"] + ')">Quick View</a></li><li><a onclick="javascript:jsfn_BillPaymentDetails(' + aData["AssessmentID"] + ')">View Payments </a></li></ul></div>';
                }
            }


        ],
        "order": [[2, "desc"]]
    });
});

function jsfn_BillDetails(billid) {

    var vData = {
        AssessmentID: billid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetAssessmentDetails', vData, jsfn_AssessmentDetailResponse)
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

function jsfn_BillPaymentDetails(billid) {
    var vData = {
        BillID: billid,
        BillTypeID: 1
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
