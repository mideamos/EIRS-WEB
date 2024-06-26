$(document).ready(function () {
    $('#dvSearchData').hide();

    var vfrmRevenueStreamByTaxOfficeTarget = $("#frmRevenueStreamByTaxOfficeTarget");
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
        vRevenueStreamByTaxOfficeTarget = $("#tblRevenueStreamByTaxOfficeTarget").DataTable({
            "processing": true, // show progress bar
            "serverSide": true, // process server side
            "filter": true,
            "orderMulti": false, // disable multiple column ordering
            "ajax": {
                "url": "/OperationManager/RevenueStreamByTaxOfficeTargetLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.RevenueStreamID = $("#cboRevenueStream").val() !== '' ? $("#cboRevenueStream").val() : '0';
                    data.Year = $("#cboYear").val() !== '' ? $("#cboYear").val() : '0';
                    data.Month = $("#cboMonth").val() !== '' ? $("#cboMonth").val() : '0';
                    data.taxofficeId = $("#cboTaxOfficeList").val() !== '' ? $("#cboTaxOfficeList").val() : '0';
                },
                "error": function (jqXHR, exception) {
                    if (jqXHR.status === 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "TaxOfficeName", "orderable": true, "name": "TaxOfficeName", "width": "20%" },
                {
                    "data": "TargetAmount", "orderable": true, "name": "TargetAmount", "width": "20%", "render": function (data, type, st) {
                        return st.TargetAmount.formatMoney();
                    }
                },
                {
                    "data": "RevenueAmount", "orderable": true, "name": "RevenueAmount", "width": "20%", "render": function (data, type, st) {
                        return st.RevenueAmount.formatMoney();
                    }
                },
                {
                    "data": "Differential", "orderable": true, "name": "Differential", "width": "20%", "render": function (data, type, st) {
                        return (st.RevenueAmount - st.TargetAmount).formatMoney();
                    }
                },
                {
                    "data": "Performance", "orderable": true, "name": "Performance", "width": "20%", "render": function (data, type, st) {
                        // Assuming Performance is a percentage or similar calculation, format as needed
                        return (st.RevenueAmount / st.TargetAmount * 100).toFixed(2) + '%';
                    }
                },
                {
                    "data": null, "orderable": false, "width": "20%", "render": function (data, type, st) {
                        return '<button class="btn btn-primary" onclick="viewDetails(' + st.taxofficeId + ')">View Details</button>';
                    }     }
            ],
            "order": [[1, "asc"]]
        });
    }
}

// Function to handle the view details button click
function viewDetails(taxofficeId) {
    // Replace with the actual URL or action you want to perform
    window.location.href = '/OperationManager/TaxOfficeDetails/' + taxofficeId;
}

function viewDetails(taxofficeId) {
    // Replace with the actual URL or action you want to perform
    window.location.href = '/OperationManager/TaxOfficeDetails/' + taxofficeId;
}