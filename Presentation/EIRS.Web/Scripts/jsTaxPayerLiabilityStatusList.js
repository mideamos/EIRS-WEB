////QC_PD_T-ERAS-_GetTaxPayerLiabilityStatus_12
$(document).ready(function () {
    $('#dvSearchData').hide();
    $.fn.datepicker.defaults.format = "yyyy-mm-dd";

    $("#txtFromDate").datepicker({
        autoclose: true
    }).on('changeDate', function (selected) {
        var minDate = new Date(selected.date.valueOf());
        $('#txtToDate').datepicker('setStartDate', minDate);
    });

    $("#txtToDate").datepicker({
        autoclose: true,
    }).on('changeDate', function (selected) {
        var maxDate = new Date(selected.date.valueOf());
        $('#txtFromDate').datepicker('setEndDate', maxDate);
    });
    $("#txtFromDate").attr('disabled', 'disabled');
    $("#txtToDate").attr('disabled', 'disabled');

    $("#cboYear").on('change', function () {
        var vYear = $("#cboYear").val();

        vStartDate = new Date(parseInt(vYear), 0, 1);
        vEndDate = new Date(parseInt(vYear), 11, 31);

        $("#txtFromDate").removeAttr('disabled');
        $("#txtToDate").removeAttr('disabled');

        $('#txtFromDate').datepicker('setStartDate', vStartDate);
        $('#txtFromDate').datepicker('setEndDate', vEndDate);
        $('#txtToDate').datepicker('setStartDate', vStartDate);
        $('#txtToDate').datepicker('setEndDate', vEndDate);

        $('#txtFromDate').datepicker('setDate', vStartDate);
        $('#txtToDate').datepicker('setDate', vEndDate);

    });

    var vfrmTaxPayerLiabilityStatus = $("#frmTaxPayerLiabilityStatus");
    vfrmTaxPayerLiabilityStatus.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            cboTaxYear: {
                required: true
            },
            txtTaxPayerRIN: {
                required :true
            }

        },
        messages: {
            cboTaxYear: {
                required: "Select Year"
            },
            txtTaxPayerRIN: {
                required: 'Enter Tax Payer RIN'
            }
        },
        errorPlacement: function (error, element) {
            if (element.is('select')) {
                error.insertAfter(element[0].parentElement);
            }
            else {
                error.insertAfter(element);
            }
        },
        highlight: function (element) { // hightlight error inputs
            $(element).closest('.input').addClass('error'); // set error class to the control group
        },
        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.input').removeClass('error'); // set error class to the control group
        }
    });

    $("#btnSearch").click(function () { return vfrmTaxPayerLiabilityStatus.valid() });
   

});

function jsfn_ShowData() {
    $('#dvSearchData').show();
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
    },{
        "orderable": true
    },{
        "orderable": false
    },];

    var vSortOrder = [];

    jsfn_ConvertToDatableWithCustomSort($('#tblTaxPayerLiabilityStatus'), vColumnsList, 'No Tax Payer Liability Status Found', vSortOrder);
}