$(document).ready(function () {

    $('#dvMDAServiceItemDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>MDA Service Item Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

 });

function jsfn_ShowMDAServiceItemDetails(MDAServiceItemID) {

    var vData = {
        MDAServiceItemID: MDAServiceItemID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/ServiceBill/GetMDAServiceItemDetails', vData, jsfn_ShowMDAServiceItemDetailResponse)
}

function jsfn_ShowMDAServiceItemDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvMDAServiceItemDetails').dialog('open');
        $('#dvMDAServiceItemRefNo').html(data.MDAServiceItemDetails.MDAServiceItemReferenceNo);
        $('#dvRevenueStream').html(data.MDAServiceItemDetails.RevenueStreamName);
        $('#dvRevenueSubStream').html(data.MDAServiceItemDetails.RevenueSubStreamName);
        $('#dvItemCategory').html(data.MDAServiceItemDetails.AssessmentItemCategoryName);
        $('#dvItemSubCategory').html(data.MDAServiceItemDetails.AssessmentItemSubCategoryName);
        $('#dvRevenueAgency').html(data.MDAServiceItemDetails.AgencyName);
        $('#dvMDAServiceItemName').html(data.MDAServiceItemDetails.MDAServiceItemName);
        $('#dvComputation').html(data.MDAServiceItemDetails.ComputationName);
        $('#dvServiceBaseAmount').html(data.MDAServiceItemDetails.ServiceBaseAmount);
        $('#dvPercentage').html(data.MDAServiceItemDetails.Percentage);
        $('#dvServiceAmount').html(data.MDAServiceItemDetails.ServiceAmount);

        if (data.MDAServiceItemDetails.ComputationID == 1) {
            $('#dvCPercentage').hide();
        }
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
