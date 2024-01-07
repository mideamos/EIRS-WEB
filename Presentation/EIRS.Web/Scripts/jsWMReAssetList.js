$(document).ready(function () {
    $('#tblWMRe_Asset').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/Profile/GetWMReAssetData?id=" + $("#hdnProfileID").val(),
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [{
            "data": "AssetTypeName"
        }, {
            "data": "AssetName"
        }, {
            "data": "AssetRIN"
            }, {
                "data": "TaxPayerCount"
            }, {
            "data": "AssetID"
        }],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(4)', nRow).html('<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                + '</button><ul class="dropdown-menu"><li><a href="' + jsfn_GetAssetDetailUrl(aData["AssetID"], aData["AssetTypeID"], aData["AssetName"]) + '">'
                + 'Asset Details</a></li></ul></div>');
        },
    });

});

function jsfn_GetAssetDetailUrl(AssetID, AssetTypeID, AssetName) {
    if (AssetTypeID == AssetType.Building) {
        return '/ProfileBuilding/Details?id=' + AssetID + '&name=' + toSeoUrl(AssetName);
    }
    else if (AssetTypeID == AssetType.Business) {
        return '/ProfileBusiness/Details?id=' + AssetID + '&name=' + toSeoUrl(AssetName);
    }
    else if (AssetTypeID == AssetType.Land) {
        return '/ProfileLand/Details?id=' + AssetID + '&name=' + toSeoUrl(AssetName);
    }
    else if (AssetTypeID == AssetType.Vehcile) {
        return '/ProfileVehicle/Details?id=' + AssetID + '&name=' + toSeoUrl(AssetName);
    }
}
