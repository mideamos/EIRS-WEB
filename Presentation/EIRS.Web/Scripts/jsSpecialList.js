$(document).ready(function () {
    $('#tblSpecial').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/CaptureSpecial/GetData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [{
            "data": "SpecialRIN"
        }, {
            "data": "SpecialTaxPayerName"
        }, {
            "data": "TIN"
        }, {
            "data": "ContactName"
        }, {
            "data": "SpecialID"
        }],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(4)', nRow).html('<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_SpecialInformation(' + aData["SpecialID"] + ')">'
                + 'Quick View</a></li><li><a href="/CaptureSpecial/Details?id=' + aData["SpecialID"] + '&name=' + aData["SpecialRIN"] + '">'
                + 'Tax Payer Details</a></li></ul></div>');
        },
    });

});
