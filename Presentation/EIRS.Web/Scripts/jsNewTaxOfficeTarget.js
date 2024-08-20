function viewDetails(year, month, taxofficeId) {
    if ( !year || !month || !taxofficeId) {
        console.error("Invalid parameters passed to viewDetails:", year, month, taxofficeId);
        return;
    }
    console.log("viewDetails called with:", year, month, taxofficeId);
    var url = '/OperationManager/NewTaxOfficeTargetDrillDrown/' +
        year + '/' +
        month + '/' +
        taxofficeId;
    window.location.href = url;
}


$(document).ready(function () {
    $('#dvSearchData').hide();

    var vfrmRevenueStreamByTaxOfficeTarget = $("#frmNewTaxOfficeTarget");
    vfrmRevenueStreamByTaxOfficeTarget.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false,
        ignore: ":hidden",
        rules: {
            Year: {
                required: true
            },
            Month: {
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
            Month: {
                required: "Select Month"
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
        if (vfrmRevenueStreamByTaxOfficeTarget.valid()) {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
        }
    });
});

var vRevenueStreamByTaxOfficeTarget;
function jsfn_bindTable() {
    if (vRevenueStreamByTaxOfficeTarget !== undefined) {
        vRevenueStreamByTaxOfficeTarget.draw();
    } else {
        vRevenueStreamByTaxOfficeTarget = $("#tblNewTaxOfficeTarget").DataTable({
            "processing": true, // show progress bar
            "serverSide": true, // process server side
            "filter": true,
            "orderMulti": false, // disable multiple column ordering
            "ajax": {
                "url": "/OperationManager/NewTaxOfficeTargetLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.Year = $("#cboYear").val() !== '' ? $("#cboYear").val() : '0';
                    data.Month = $("#cboMonth").val() !== '' ? $("#cboMonth").val() : '0';
                    data.taxofficeId = $("#cboTaxOfficeList").val() !== '' ? $("#cboTaxOfficeList").val() : '0';
                },
                "error": function (jqXHR, exception) {
                    console.log(jqXHR.responseText);
                    if (jqXHR.status === 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "RevenueStreamName", "orderable": true, "name": "RevenueStreamName", "width": "20%" },
                {
                    "data": "Targetamount", "orderable": true, "name": "Targetamount", "width": "20%", "render": function (data, type, st) {
                        return (data || 0).formatMoney();
                    }
                },
                {
                    "data": "Settlementamount", "orderable": true, "name": "Settlementamount", "width": "20%", "render": function (data, type, st) {
                        return (data || 0).formatMoney();
                    }
                },
                {
                    "data": "differenitial", "orderable": true, "name": "differenitial", "width": "20%", "render": function (data, type, st) {
                        return (data || 0).formatMoney();
                    }
                },
                {

                    "data": "Percentage", "orderable": true, "name": "Perc", "width": "20%", "render": function (data, type, st) {
                        if (data === null || data === undefined) {
                            return '0%';
                        } return data.toLocaleString(undefined, { style: 'percent', minimumFractionDigits: 2 });
                    }
                },
                {
                    "data": null, "orderable": false, "width": "20%", "render": function (data, type, st) {
                        console.log(data, st);
                        return '<button class="btn btn-primary" onclick="viewDetails(' + st.year + ', \'' + st.month + '\', ' + st.taxofficeId + ')">View Details</button>';
                    }
                }
            ],
            "order": [[1, "asc"]]
        });
    }
}


