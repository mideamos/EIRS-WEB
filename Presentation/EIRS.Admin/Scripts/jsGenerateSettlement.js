var vServiceBillDataTable;
var vAssessmentDataTable;

$(document).ready(function () {
    $("#fsStep2").hide();
    $("#btnBack").hide();
    $("#btnGenerate").hide();

    var frmGenerateSettlement = $('#frmGenerateSettlement');

    // init validator obj and set the rules
    frmGenerateSettlement.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            SettlementType: {
                required: true,
            }
            // the rules, as usual
        },

        messages: {
            SettlementType: {
                required: 'select settlement type'
            }
        },
        errorPlacement: function (error, element) {
            error.insertAfter(element.parent());
            element.parent().addClass("state-error");
        },
        highlight: function (element) { // hightlight error inputs
            $(element).closest('.control-group').addClass('error'); // set error class to the control group
        },

        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.control-group').removeClass('error'); // set error class to the control group
        }
    });

    $("#btnNext").on('click', function () {
        if ($('#fsStep1 :input').valid()) {
            if ($("#cboSettlementType").val() == '1') {
                jsfn_BindAssessmentTable();
            }
            else if ($("#cboSettlementType").val() == '2') {
                jsfn_BindServiceBillTable();
            }
        }
    });

    $("#btnBack").click(function () {
        $("#fsStep2").hide();
        $("#fsStep1").show();
        $("#dvFooter").show();
        $("#btnGenerate").hide();
        $("#btnBack").hide();
        $("#artGenerateSettlement").addClass("col-lg-6");
        return false;
    });

    $('#btnGenerate').click(function () {
        if ($("#hdnASID").val() != '') {
            window.location.href = '/Settlement/add/' + $("#hdnASID").val() + '/' + $("#hdnASRefNo").val() + '?stype=' + $("#cboSettlementType").val();
        }
        else {
            jsfn_ShowAlert('Select Atleast One Assessment or Service Bill', 'danger');
        }

        return false;
    });

});

function jsfn_SelectSingleASB(rbASB) {
    $("#hdnASID").val(rbASB.dataset["asbid"]);
    $("#hdnASRefNo").val(rbASB.dataset["asbref"]);
}

function jsfn_BindAssessmentTable() {
    $("#artGenerateSettlement").removeClass("col-lg-6");
    $("#fsStep2").show();
    $("#dvAssessmentBill").show();
    $("#dvServiceBill").hide();
    $("#fsStep1").hide();
    $("#dvFooter").hide();
    $("#btnBack").show();
    $("#btnGenerate").show();

    vAssessmentDataTable = $("#tbAssessment").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Settlement/GetAssessmentList",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.AssessmentRefNo = $("#txtAssessmentRefNo").val();
                data.AssessmentDate = $("#txtAssessmentDate").val();
                data.TaxPayerTypeName = $("#txtATaxPayerType").val();
                data.TaxPayerName = $("#txtATaxPayerName").val();
                data.TaxPayerRIN = $("#txtATaxPayerRIN").val();
                data.AssessmentAmount = $("#txtAssessmentAmount").val();
                data.SettlementDueDate = $("#txtASettlementDueDate").val();
                data.SettlementStatusName = $("#txtASettlementStatus").val();
                data.SettlementDate = $("#txtASettlementDate").val();
                data.AssessmentNotes = $("#txtAssessmentNotes").val();
                data.ActiveText = $("#txtAStatus").val();
            }
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "AssessmentRefNo", "orderable": true, "name": "AssessmentRefNo" },
            {
                "data": "AssessmentDate", "orderable": true, "name": "AssessmentDate", "render": function (data, type, ass) {
                    if (ass.AssessmentDate != undefined || ass.AssessmentDate != null) {
                        return jsfn_FormatJsonDate(ass.AssessmentDate, "dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            { "data": "TaxPayerTypeName", "orderable": true, "name": "TaxPayerTypeName" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            { "data": "TaxPayerRIN", "orderable": true, "name": "TaxPayerRIN" },
            {
                "data": "AssessmentAmount", "orderable": true, "name": "AssessmentAmount", "render": function (data, type, ass) {
                    return ass.AssessmentAmount.formatMoney();
                }
            },
            {
                "data": "SettlementDueDate", "orderable": true, "name": "SettlementDueDate", "render": function (data, type, ass) {
                    if (ass.SettlementDueDate != undefined || ass.SettlementDueDate != null) {
                        return jsfn_FormatJsonDate(ass.SettlementDueDate, "dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            { "data": "SettlementStatusName", "orderable": true, "name": "SettlementStatusName" },
            {
                "data": "SettlementDate", "orderable": true, "name": "SettlementDate", "render": function (data, type, ass) {
                    if (ass.SettlementDate != undefined || ass.SettlementDate != null) {
                        return jsfn_FormatJsonDate(ass.SettlementDate, "dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            { "data": "AssessmentNotes", "orderable": true, "name": "AssessmentNotes" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, abill) {
                    return '<label class="radio">' +
                        '<input data-asbid="' + abill.AssessmentID + '" data-asbref="' + abill.AssessmentRefNo + '" id="rbASB" name="rbASB" onchange="jsfn_SelectSingleASB(this)" title="Tick To Select" type="radio" value="' + abill.AssessmentID + '">' +
                        '<i></i>' +
                        '</label>';
                }
            },
        ],
        "order": [[1, "asc"]]
    });


    $("#tbAssessment thead th input[type=text]").on('change', function () {
        vAssessmentDataTable
            .column($(this.data).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });


    ////Hide Columns
    vAssessmentDataTable.column(4).visible(false);
    vAssessmentDataTable.column(5).visible(false);
    vAssessmentDataTable.column(6).visible(false);
    vAssessmentDataTable.column(7).visible(false);
    vAssessmentDataTable.column(8).visible(false);
    vAssessmentDataTable.column(10).visible(false);
    vAssessmentDataTable.column(12).visible(false);
}

function jsfn_BindServiceBillTable() {
    $("#artGenerateSettlement").removeClass("col-lg-6");
    $("#fsStep2").show();
    $("#fsStep1").hide();
    $("#dvFooter").hide();
    $("#btnBack").show();
    $("#btnGenerate").show();
    $("#dvAssessmentBill").hide();
    $("#dvServiceBill").show();

    vServiceBillDataTable = $("#tbServiceBill").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Settlement/GetServiceBillList",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.ServiceBillRefNo = $("#txtServiceBillRefNo").val();
                data.ServiceBillDate = $("#txtServiceBillDate").val();
                data.TaxPayerTypeName = $("#txtSTaxPayerType").val();
                data.TaxPayerName = $("#txtSTaxPayerName").val();
                data.TaxPayerRIN = $("#txtSTaxPayerRIN").val();
                data.ServiceBillAmount = $("#txtServiceBillAmount").val();
                data.SettlementDueDate = $("#txtSSettlementDueDate").val();
                data.SettlementStatusName = $("#txtSSettlementStatus").val();
                data.SettlementDate = $("#txtSSettlementDate").val();
                data.ActiveText = $("#txtSStatus").val();
                data.ServiceBillNotes = $("#txtServiceBillNotes").val();
            }
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "ServiceBillRefNo", "orderable": true, "name": "ServiceBillRefNo" },
            {
                "data": "ServiceBillDate", "orderable": true, "name": "ServiceBillDate", "render": function (data, type, servbill) {
                    if (servbill.ServiceBillDate != undefined || servbill.ServiceBillDate != null) {
                        return jsfn_FormatJsonDate(servbill.ServiceBillDate, "dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            { "data": "TaxPayerTypeName", "orderable": true, "name": "TaxPayerTypeName" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            { "data": "TaxPayerRIN", "orderable": true, "name": "TaxPayerRIN" },
            {
                "data": "ServiceBillAmount", "orderable": true, "name": "ServiceBillAmount", "render": function (data, type, servbill) {
                    return servbill.ServiceBillAmount.formatMoney();
                }
            },
            {
                "data": "SettlementDueDate", "orderable": true, "name": "SettlementDueDate", "render": function (data, type, servbill) {
                    if (servbill.SettlementDueDate != undefined || servbill.SettlementDueDate != null) {
                        return jsfn_FormatJsonDate(servbill.SettlementDueDate, "dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            { "data": "SettlementStatusName", "orderable": true, "name": "SettlementStatusName" },
            {
                "data": "SettlementDate", "orderable": true, "name": "SettlementDate", "render": function (data, type, servbill) {
                    if (servbill.SettlementDate != undefined || servbill.SettlementDate != null) {
                        return jsfn_FormatJsonDate(servbill.SettlementDate, "dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            { "data": "Notes", "orderable": true, "name": "Notes" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, servbill) {
                    return '<label class="radio">' +
                        '<input data-asbid="' + servbill.ServiceBillID + '" data-asbref="' + servbill.ServiceBillRefNo + '" id="rbASB" name="rbASB" onchange="jsfn_SelectSingleASB(this)" title="Tick To Select" type="radio" value="' + servbill.ServiceBillID + '">' +
                        '<i></i>' +
                        '</label>';
                }
            },
        ],
        "order": [[1, "asc"]]
    });

    $("#tbServiceBill thead th input[type=text]").on('change', function () {
        vServiceBillDataTable
            .column($(this.data).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });

    ////Hide Columns
    vServiceBillDataTable.column(4).visible(false);
    vServiceBillDataTable.column(5).visible(false);
    vServiceBillDataTable.column(6).visible(false);
    vServiceBillDataTable.column(7).visible(false);
    vServiceBillDataTable.column(8).visible(false);
    vServiceBillDataTable.column(10).visible(false);
}