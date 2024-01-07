$(document).ready(function () {

    $('#cboTaxPayerType').on("change", jsfn_onTaxPayerTypeChange);
    $('#cboProfile').on("change", jsfn_onProfileChange);
   
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
        jsfn_onTaxPayerChange();
    };

    vTaxPayerTypeHead.on('typeahead:selected', vTaxPayerSelectedHandler);

})

function jsfn_onTaxPayerTypeChange() {
    $("#hdnTaxPayerID").val('');
    $("#txtTaxPayer").val('');

    $("#cboProfile").html("");
    $("#cboProfile").append($('<option></option>').val("").html("Select Profile"));
    $('#cboProfile').val('').selectpicker('refresh');

    $('#cboAssetType').val('').selectpicker('refresh');
    $("#cboAsset").html("");
    $("#cboAsset").append($('<option></option>').val("").html("Select Asset"));
    $('#cboAsset').val('').selectpicker('refresh');
}

function jsfn_onTaxPayerChange() {
    var vTaxPayerID = $("#hdnTaxPayerID").val();
    var vTaxPayerTypeID = $('#cboTaxPayerType').val();
    var vCertificateTypeID = $('#cboCertificateType').val();

    var vData = {
        TaxPayerID: vTaxPayerID !== '' ? vTaxPayerID : -1,
        TaxPayerTypeID: vTaxPayerTypeID !== '' ? vTaxPayerTypeID : -1,
        CertificateTypeID: vCertificateTypeID != '' ? vCertificateTypeID : -1
    };

    jsfn_ajaxPost('/Certificate/GetProfileDropDown', vData, jsfn_onTaxPayerChangeResponse);
}

function jsfn_onTaxPayerChangeResponse(data) {
    $("#cboProfile").html(""); // clear before appending new list

    $("#cboProfile").append(
        $('<option></option>').val("").html("Select Profile"));

    $.each(data, function (i, prf) {
        $("#cboProfile").append(
            $('<option></option>').val(prf.id).html(prf.text));
    });

    if (data.length === 0) {
        jsfn_ShowAlert('There are no matching profile for selected tax payer', 'danger');
    }

    $('#cboProfile').val('').selectpicker('refresh');
    $('#cboAssetType').val('').selectpicker('refresh');
    $("#cboAsset").html("");
    $("#cboAsset").append($('<option></option>').val("").html("Select Asset"));
    $('#cboAsset').val('').selectpicker('refresh');
}

function jsfn_onProfileChange() {
    var vProfileID = $("#cboProfile").val();
    var vTaxPayerID = $("#hdnTaxPayerID").val();
    var vTaxPayerTypeID = $('#cboTaxPayerType').val();

    var vData = {
        TaxPayerID: vTaxPayerID !== '' ? vTaxPayerID : -1,
        TaxPayerTypeID: vTaxPayerTypeID !== '' ? vTaxPayerTypeID : -1,
        ProfileID: vProfileID !== '' ? vProfileID : -1
    };

    jsfn_ajaxPost('/Certificate/GetAssetDropDown', vData, jsfn_onProfileChangeResponse);
}


function jsfn_onProfileChangeResponse(data) {
    $("#cboAsset").html(""); // clear before appending new list

    $("#cboAsset").append(
        $('<option></option>').val("").html("Select Asset"));

    $.each(data, function (i, asst) {
        $("#cboAsset").append(
            $('<option></option>').val(asst.id).html(asst.text));
    });

    $('#cboAsset').val('').selectpicker('refresh');
}