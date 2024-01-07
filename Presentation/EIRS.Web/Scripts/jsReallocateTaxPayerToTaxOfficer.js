$(document).ready(function () {
    //if ($('#hdnTaxPayerID').val() !== '') {
    //    jsfn_GetBalanceForTaxPayer();
    //}

    $('#cboTaxPayerType').on("change", jsfn_onTaxPayerType);
    
    var vTaxPayer = new Bloodhound({
        datumTokenizer: function (d) {
            return Bloodhound.tokenizers.whitespace(d.text);
        },
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        limit: 10,
        remote: {
            url: '/Base/GetTaxOfficeBasedTaxPayer',
            replace: function (url, uriEncodedQuery) {
                var vRequestUrl = (url + "?TaxPayerTypeID=" + $("#cboTaxPayerType").val() + "&TaxOfficeID=" + $("#cboTaxOffice").val() + "&query="
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

    $('#cboTaxOffice').on("change", jsfn_onTaxOfficeChange);

});

function jsfn_onTaxOfficeChange() {
    var vTaxOfficeID = $('#cboTaxOffice').val();

    if (vTaxOfficeID !== '') {

        var vData = {
            TaxOfficeID: vTaxOfficeID === '' ? 0 : vTaxOfficeID,
            showmanager: false
        };

        jsfn_ajaxPost('/Base/GetTaxOfficeManger', vData, jsfn_onTaxOfficeChangeResponse);
    }
}

function jsfn_onTaxOfficeChangeResponse(data) {
    $("#TaxOfficerID").html("");

    $("#TaxOfficerID").append(
        $('<option></option>').val("").html("Select Tax Officer"));

    if (data !== undefined) {
        $.each(data, function (i, tit) {
            $("#TaxOfficerID").append(
                $('<option></option>').val(tit.id).html(tit.text));
        });

        $('#TaxOfficerID').selectpicker('refresh');
    }
}

function jsfn_onTaxPayerType() {
    $("#hdnTaxPayerID").val('');
    $("#txtTaxPayer").val('');
}