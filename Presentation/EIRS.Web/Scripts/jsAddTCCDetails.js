$(document).ready(function () {
    if ($('#hdnTaxPayerID').val() !== '') {
        jsfn_GetLiabilityForTaxPayer();
    }
    $('#cboTaxPayerType').on("change", jsfn_onTaxPayerType);
    $('#cboTaxYear').on("change", jsfn_GetLiabilityForTaxPayer);
    

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
        jsfn_GetLiabilityForTaxPayer();
    };

    vTaxPayerTypeHead.on('typeahead:selected', vTaxPayerSelectedHandler);


});


function jsfn_onTaxPayerType() {
    $("#hdnTaxPayerID").val('');
    $("#txtTaxPayer").val('');
}

function jsfn_GetLiabilityForTaxPayer() {
    var vTaxPayerTypeID = $('#cboTaxPayerType').val();
    var vTaxPayerID = $('#hdnTaxPayerID').val();
    var vTaxYear = $('#cboTaxYear').val();

    if (vTaxYear !== '') {
        var vData = {
            TaxPayerTypeID: vTaxPayerTypeID === '' ? 0 : vTaxPayerTypeID,
            TaxPayerID: vTaxPayerID === '' ? 0 : vTaxPayerID,
            TaxYear: vTaxYear
        };

        jsfn_ajaxPost('/TCCDetail/GetTaxPayerLiabilityData', vData, jsfn_GetLiabilityForTaxPayerResponse);
    }
}

function jsfn_GetLiabilityForTaxPayerResponse(data) {
    if (data !== undefined) {
        $("#txtERASTaxPaid").val(data.TotalPaymentAmount);
        $("#txtERASAssessed").val(data.TotalAssessmentAmount);
    }
}