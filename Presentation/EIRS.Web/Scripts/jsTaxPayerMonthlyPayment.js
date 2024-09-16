$(document).ready(function () {
    $('#dvSearchData').hide();

    $('#cboTaxPayerType').on("change", jsfn_onTaxPayerType);

    var vTaxPayer = new Bloodhound({
        datumTokenizer: function (d) {
            return Bloodhound.tokenizers.whitespace(d.text);
        },
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        limit: 10,
        remote: {
            url: '/Base/GetTaxPayer',
            replace: function (url, uriEncodedQuery) {
                var vRequestUrl = (url + "?TaxPayerTypeID=" + $("#cboTaxPayerType").val() + "&query="
                    + encodeURIComponent(uriEncodedQuery));
                return vRequestUrl;
            },
            filter: function (list) {
                return $.map(list, function (tp) {
                    return {
                        text: tp.text,
                        id: tp.id
                    };
                });
            }
        }
    });

    vTaxPayer.initialize();

    var vTaxPayerTypeHead = $('#txtTaxPayer');

    vTaxPayerTypeHead.typeahead(null, {
        name: 'txtTaxPayer',
        displayKey: 'text',
        source: vTaxPayer.ttAdapter()
    });

    var vTaxPayerSelectedHandler = function (eventObject, suggestionObject, suggestionDataset) {
        $("#hdnTaxPayerID").val(suggestionObject.id);
    };

    vTaxPayerTypeHead.on('typeahead:selected', vTaxPayerSelectedHandler);

    var vfrmTaxPayerMonthlyPayment = $("#frmTaxPayerMonthlyPayment");
    vfrmTaxPayerMonthlyPayment.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            Year: {
                required: true
            },
            TaxPayerTypeID: {
                required: true
            },
            TaxPayerName: {
                required: true
            },
            RevenueStream: {
                required: true
            }

        },
        messages: {
            Year: {
                required: "Select Year"
            },
            TaxPayerTypeID: {
                required: "Select Tax Payer Type"
            },
            TaxPayerName: {
                required: "Select Tax Payer"
            },
            RevenueStream: {
                required: "Select Revenue Stream"
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

    $("#btnSearch").click(function () {
        if (vfrmTaxPayerMonthlyPayment.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });

    $("#btnExportToExcel").click(function () {
        var vYear = $("#cboYear").val();
        var vTaxPayerTypeID = $("#cboTaxPayerType").val() !== '' ? $("#cboTaxPayerType").val() : '0';
        var vTaxPayerID = $("#hdnTaxPayerID").val() !== '' ? $("#hdnTaxPayerID").val() : '0';
        var vRevenueStreamID = $("#cboRevenueStream").val() !== '' ? $("#cboRevenueStream").val() : '0';

        var vUrl = '/OperationManager/TaxPayerMonthlyPaymentExportToExcel?TaxPayerTypeID=' + vTaxPayerTypeID + '&TaxPayerID=' + vTaxPayerID + '&Year=' + vYear + '&RevenueStreamID=' + vRevenueStreamID;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });
});


function jsfn_onTaxPayerType() {
    $("#hdnTaxPayerID").val('');
    $("#txtTaxPayer").val('');
}

var vTaxPayerMonthlyPayment;
function jsfn_bindTable() {
    if (vTaxPayerMonthlyPayment !== undefined) {
        vTaxPayerMonthlyPayment.draw();
    }
    else {
        vTaxPayerMonthlyPayment = $("#tblTaxPayerMonthlyPayment").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/TaxPayerMonthlyPaymentLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.TaxPayerTypeID = $("#cboTaxPayerType").val() !== '' ? $("#cboTaxPayerType").val() : '0';
                    data.TaxPayerID = $("#hdnTaxPayerID").val() !== '' ? $("#hdnTaxPayerID").val() : '0';
                    data.RevenueStreamID = $("#cboRevenueStream").val() !== '' ? $("#cboRevenueStream").val() : '0';
                    data.Year = $("#cboYear").val() !== '' ? $("#cboYear").val() : '0';
                }, "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "StartMonthName", "orderable": true, "name": "StartDate", "width": "25%" },
                {
                    "data": "SettlementAmount", "orderable": true, "name": "SettlementAmount", "width": "25%", "render": function (data, type, st) {
                        return st.SettlementAmount.formatMoney();
                    }
                },
                {
                    "data": "PoAAmount", "orderable": true, "name": "PoAAmount", "width": "25%", "render": function (data, type, st) {
                        return st.PoAAmount.formatMoney();
                    }
                },
                {
                    "data": "TotalAmount", "orderable": true, "name": "TotalAmount", "width": "25%", "render": function (data, type, st) {
                        return st.TotalAmount.formatMoney();
                    }
                }, {
                    "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                            + '</button><ul class="dropdown-menu">'
                            + '<li><a href="/OperationManager/TaxPayerMonthlyPaymentDetail?tptid=' + $("#cboTaxPayerType").val() + '&tpid=' + $("#hdnTaxPayerID").val() + '&rsid=' + $("#cboRevenueStream").val() + '&year=' + $("#cboYear").val() + '&month=' + st.MonthID + '">View Details</a></li>'
                            + '</ul></div>';
                    }, "width": "10%"
                }
            ],
            "order": [[1, "asc"]],
            "footerCallback": function (row, ddata, start, end, display) {
                var vFooterTotal = this.api().ajax.json().FooterTotal;
                if (vFooterTotal) {
                    var $th = $(row).find('th');
                    $th.eq(1).html(vFooterTotal['PoAAmount'].formatMoney());
                    $th.eq(2).html(vFooterTotal['SettlementAmount'].formatMoney());
                    $th.eq(3).html(vFooterTotal['TotalAmount'].formatMoney());
                }
            }
        });
    }
}