$(document).ready(function () {
    $('#dvSearchData').hide();
    $('#cboBusinessType').on("change", jsfn_onBusinessTypeChange);
    var vfrmBusinessSectorAssessment = $("#frmBusinessSectorAssessment");
    vfrmBusinessSectorAssessment.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            Year: {
                required: true
            },
            BusinessType: {
                required: true
            },
            BusinessCategory: {
                required: false
            }

        },
        messages: {
            Year: {
                required: "Select Year"
            },
            BusinessType: {
                required: "Select Business Type"
            },
            BusinessCategory: {
                required: "Select Business Category"
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
        if (vfrmBusinessSectorAssessment.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });

    $("#btnExportToExcel").click(function () {
        var vYear = $("#cboYear").val();
        var vBusinessTypeID = $("#cboBusinessType").val() !== '' ? $("#cboBusinessType").val() : '0';
        var vBusinessCategoryID = $("#cboBusinessCategory").val() !== '' ? $("#cboBusinessCategory").val() : '0';

        var vUrl = '/OperationManager/BusinessSectorAssessmentSummaryExportToExcel?TaxYear=' + vYear + '&BusinessTypeID=' + vBusinessTypeID + '&BusinessCategoryID=' + vBusinessCategoryID;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });
});

var vBusinessSectorAssessment;
function jsfn_bindTable() {
    if (vBusinessSectorAssessment !== undefined) {
        vBusinessSectorAssessment.draw();
    }
    else {
        vBusinessSectorAssessment = $("#tblBusinessSectorAssessment").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/OperationManager/BusinessSectorAssessmentSummaryLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.TaxYear = $("#cboYear").val() !== '' ? $("#cboYear").val() : '0';
                    data.BusinessTypeID = $("#cboBusinessType").val() !== '' ? $("#cboBusinessType").val() : '0';
                    data.BusinessCategoryID = $("#cboBusinessCategory").val() !== '' ? $("#cboBusinessCategory").val() : '0';
                },
                "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "BusinessSectorName", "orderable": true, "name": "BusinessSectorName", "width": "25%" },
                { "data": "BusinessCategoryName", "orderable": true, "name": "BusinessCategoryName", "width": "25%" },
                {
                    "data": "BillAmount", "orderable": true, "name": "BillAmount", "width": "25%", "render": function (data, type, st) {
                        return st.BillAmount.formatMoney();
                    }
                },
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
                    "data": "OutstandingAmount", "orderable": true, "name": "OutstandingAmount", "width": "25%", "render": function (data, type, st) {
                        return st.OutstandingAmount.formatMoney();
                    }
                }, {
                    "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                            + '</button><ul class="dropdown-menu">'
                            + '<li><a href="/OperationManager/BusinessSectorAssessmentDetail?year=' + $("#cboYear").val() + '&bsid=' + st.BusinessSectorID + '">View Details</a></li>'
                            + '</ul></div>';
                    }, "width": "10%"
                }
            ],
            "order": [[0, "asc"]],
            "footerCallback": function (row, ddata, start, end, display) {
                var vFooterTotal = this.api().ajax.json().FooterTotal;
                if (vFooterTotal) {
                    var $th = $(row).find('th');
                    $th.eq(2).html(vFooterTotal['BillAmount'].formatMoney());
                    $th.eq(3).html(vFooterTotal['SettlementAmount'].formatMoney());
                    $th.eq(4).html(vFooterTotal['PoAAmount'].formatMoney());
                    $th.eq(5).html(vFooterTotal['OutstandingAmount'].formatMoney());
                }
            }
        });
    }
}

function jsfn_onBusinessTypeChange() {
    var vBusinessTypeID = $('#cboBusinessType').val();

    var vData = {
        BusinessTypeID: vBusinessTypeID === '' ? 0 : vBusinessTypeID
    };

    jsfn_ajaxPost('/Base/BusinessTypeChange', vData, jsfn_onBusinessTypeChangeResponse);
}

function jsfn_onBusinessTypeChangeResponse(data) {
    $("#cboBusinessCategory").html("");

    $("#cboBusinessCategory").append(
        $('<option></option>').val("").html("All Business Category"));

    $.each(data.BusinessCategory, function (i, bcat) {
        $("#cboBusinessCategory").append(
            $('<option></option>').val(bcat.id).html(bcat.text));
    });

    $('#cboBusinessCategory').selectpicker('refresh');
}