$(document).ready(function () {
    $('#tblWMEC').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/Profile/GetWMECData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [{
            "data": "ProfileReferenceNo"
        }, {
            "data": "ProfileDescription"
        }, {
            "data": "TaxPayerCount"
            }, {
                "data": "AssetCount"
            }, {
            "data": "ProfileID"
        }],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(4)', nRow).html('<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_ProfileDetails(' + aData["ProfileID"] + ')">'
                + 'Quick View</a></li><li><a href="/Profile/WMEC_TaxPayer?id=' + aData["ProfileID"] + '&name=' + toSeoUrl(aData["ProfileDescription"]) + '">Tax Payer List</a></li><li><a href="/Profile/WMEC_Asset?id=' + aData["ProfileID"] + '&name=' + toSeoUrl(aData["ProfileDescription"]) + '">Asset List</a></li></ul></div>');
        },
    });

});
