$(document).ready(function () {
    $('#dvSearchData').hide();
    $.fn.datepicker.defaults.format = "dd/mm/yyyy";


    $("#txtReportDate").datepicker({
        endDate: '+0d',
        autoclose: true,
        format: "mm-yyyy",
        viewMode: "months",
        minViewMode: "months"
    });

    var frmMonthlySummary = $('#frmMonthlySummary');

    frmMonthlySummary.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            txtReportDate: {
                required: true,
            },
            cboReference: {
                required: true
            }
        },

        // Messages for form validation
        messages: {
            txtReportDate: {
                required: 'Please select report date',
            },
            cboReference: {
                required: 'Please select reference'
            }
        },
        highlight: function (element) { // hightlight error inputs
            $(element).closest('.control-group').addClass('error'); // set error class to the control group
        },

        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.control-group').removeClass('error'); // set error class to the control group
        }


    });

    $("#btnSearch").click(function () { return frmMonthlySummary.valid(); });

});

function jsfn_ValidateSearch() {

    var vtxtReportDate = $.trim($("#txtReportDate").val());
    var vReference = $.trim($("#cboReference").val());

    var formFieldText = $('input[type="text"]', '#' + pFormID).filter(function () {
        return $.trim(this.value).length;
    }).length;

    if (!formFieldText) {
        jsfn_ShowAlert("Please fill atleast one search criteria", 'danger');
        return false;
    }
    else {
        return true;
    }
}

function jsfn_ShowData() {
    $('#dvSearchData').show();
    $("#spnReference").html($("#cboReference option:selected").text());
    $("#spnReportDate").html($("#txtReportDate").val());
    $("#btnDownload").attr("href", "/Report/DownloadMonthlySummary?reportdate=" + $("#txtReportDate").val() + "&referenceid=" + $("#cboReference").val());

    jsfn_bindTable();
}

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
    }];

    var vSortOrder = [];

    jsfn_ConvertToDatableWithCustomSort($('#tblMonthlySummaryReport'), vColumnsList, 'No Record Found', vSortOrder);
    $(".collapse").click();
}