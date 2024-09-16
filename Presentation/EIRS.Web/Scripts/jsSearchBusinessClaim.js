$(document).ready(function () {
    $("#dvDataPortlet").hide();
    $('#btnSearch').attr('disabled', true);
    $('#txtBusinessName').on('keyup', function () {
        var txtBusinessName = $("#txtBusinessName").val();

        if (txtBusinessName != '' && txtBusinessName.length >= 3) {
            $('#btnSearch').attr('disabled', false);
        } else {
            $('#btnSearch').attr('disabled', true);
        }
    });

});

function jsfn_ShowDataPortlet() {
    $("#dvDataPortlet").show();

    $(".btnviewdetail").click(function () {
        var vBusinessID = $(this).data('bid');

        var vData = {
            BusinessID : vBusinessID
        };

        jsfn_ajaxPost('/CBS/GetBusinessDetails', vData, jsfn_GetBusinessDetailResponse);
    });
}

function jsfn_GetBusinessDetailResponse(data) {
    if (data != undefined) {
        $("#divBusinessModal").modal('show');
        $('#dvBusinessName').html(data.BusinessName);
        $('#dvBusinessAddress').html(data.BusinessAddress);
        $('#dvBusinessSector').html(data.BusinessSector);
        $('#dvBusinessSubSector').html(data.BusinessSubSector);
        $('#dvBusinessType').html(data.BusinessType);
        $('#dvBusinessCategory').html(data.BusinessCategory);
        $('#dvAssetType').html(data.AssetType);
        $('#dvBusinessStructure').html(data.BusinessStructure);
        $('#dvBusinessOperations').html(data.BusinessOperations);
        $('#dvSize').html(data.Size);
        $('#dvLGA').html(data.LGA);
        $('#dvTIN').html(data.TIN);
        $('#dvTaxOffice').html(data.TaxOffice);
    }
    else {

    }
}