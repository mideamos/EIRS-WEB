jQuery(document).ready(function () {
    $('#tbPoAOperation').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "responsive" : true,
        "ajax": {
            "url": "/OperationManager/PoAOperationLoadData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [{
            "data": "OperationDate",
            "orderable": true,
            "name": "OperationDate"
        }, {
            "data": "Operation_TypesName",
            "orderable": true,
            "name": "Operation_TypesName"
        }, {
            "data": "FromTaxPayer",
            "orderable": true,
            "name": "FromTaxPayer"
        }, {
            "data": "FromName",
            "orderable": true,
            "name": "FromName"
        }, {
            "data": "ToTaxPayer",
            "orderable": true,
            "name": "ToTaxPayer"
        }, {
            "data": "ToName",
            "orderable": true,
            "name": "ToName"
        }, {
            "data": "Amount",
            "orderable": true,
            "name": "Amount"
        }],
        "order": [[0, "desc"]],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var vOperationDate = new Date(parseInt(aData["OperationDate"].substr(6)));
            $('td:eq(0)', nRow).html(vOperationDate.format("dd-mmm-yyyy"));
            $('td:eq(6)', nRow).html(aData["Amount"].formatMoney());
        },
    });
});

