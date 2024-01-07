$(document).ready(function () {

    $.fn.datepicker.defaults.format = "dd/mm/yyyy";

    $("#txtPaymentDate").datepicker({
        autoclose: true,
        endDate : '0d'
    });

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

    $('#cboRevenueStream').on("change", jsfn_onRevenueStreamChange);

});

function jsfn_onTaxPayerType() {
    $("#hdnTaxPayerID").val('');
    $("#txtTaxPayer").val('');
}

function jsfn_onRevenueStreamChange() {
    var vRevenueStreamID = $('#cboRevenueStream').val();

    if (vRevenueStreamID != '') {

        var vData = {
            RevenueStreamID: vRevenueStreamID
        };

        jsfn_ajaxPost('/Base/GetRevenueSubStream', vData, jsfn_onRevenueStreamChangeResponse);
    }
    else {
        $("#cboRevenueSubStream").html("");
        $("#cboRevenueSubStream").append($('<option></option>').val("").html("Select Revenue Sub Stream"));
        $('#cboRevenueSubStream').val('').select2({ "width": "100%" });
    }
}

function jsfn_onRevenueStreamChangeResponse(data) {
    $("#cboRevenueSubStream").html("");

    $("#cboRevenueSubStream").append(
        $('<option></option>').val("").html("Select Revenue Sub Stream"));

    $.each(data, function (i, asgrp) {
        $("#cboRevenueSubStream").append(
            $('<option></option>').val(asgrp.id).html(asgrp.text));
    });

    $('#cboRevenueSubStream').val('').selectpicker('refresh');
}
