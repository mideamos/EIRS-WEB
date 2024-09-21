$(document).ready(function () {
  debugger;
  if ($("#hdnFromTaxPayerID").val() != "") {
    jsfn_GetBalanceForFromTaxPayer();
  }
  $("#cboFromTaxPayerType").on("change", jsfn_onFromTaxPayerType);
  $("#cboToTaxPayerType").on("change", jsfn_onToTaxPayerType);

  var vFromTaxPayer = new Bloodhound({
    datumTokenizer: function (d) {
      return Bloodhound.tokenizers.whitespace(d.text);
    },
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    limit: 10,
    remote: {
      url: "/Base/GetTaxPayer",
      replace: function (url, uriEncodedQuery) {
        var vRequestUrl =
          url +
          "?TaxPayerTypeID=" +
          $("#cboFromTaxPayerType").val() +
          "&query=" +
          encodeURIComponent(uriEncodedQuery);
        return vRequestUrl;
      },
      filter: function (list) {
        return $.map(list, function (tp) {
          return {
            text: tp.text,
            id: tp.id,
          };
        });
      },
    },
  });

  vFromTaxPayer.initialize();

  var vFromTaxPayerTypeHead = $("#txtFromTaxPayer");

  vFromTaxPayerTypeHead.typeahead(null, {
    name: "txtFromTaxPayer",
    displayKey: "text",
    source: vFromTaxPayer.ttAdapter(),
  });

  var vFromTaxPayerSelectedHandler = function (
    eventObject,
    suggestionObject,
    suggestionDataset
  ) {
    $("#hdnFromTaxPayerID").val(suggestionObject.id);
    jsfn_GetBalanceForFromTaxPayer();
  };

  vFromTaxPayerTypeHead.on("typeahead:selected", vFromTaxPayerSelectedHandler);

  //function formdata() {
  //    var firstname1 = document.getElementById("firstname").value;
  //    var lastname1 = document.getElementById("lastname").value;
  //    if (firstname1.Length >)
  //}

  $("#SaveToConfirm").click(function () {
    // debugger
    var vTaxpayerTypeID = $("#cboFromTaxPayerType").val();
    var vTaxPayerID = $("#hdnToTaxPayerID").val();
    var vid = $("#cboTransactionRefNumber").val();

    console.log(vTaxpayerTypeID, vTaxPayerID, vid);
    var vData = {
      pid: vid,
      pIntTaxPayerTypeID: vTaxpayerTypeID == "" ? 0 : vTaxpayerTypeID,
      pIntTaxPayerID: vTaxPayerID == "" ? 0 : vTaxPayerID,
    };
    // Check if pIntTaxPayerID is equal to 0
    //if (vData.pIntTaxPayerID === 0) {
    //    alert('Taxpayer Number not Found. Please Try again');
    //    // Stop the process or return from the function if needed
    //    return;
    //}
    //console.log(vData);
    jsfn_ajaxPost(
      "/OperationManager/PoATransferValidate/",
      vData,
      jsfn_onValidateResponse
    );
  });

  function jsfn_onValidateResponse(data) {
    if (data.success) {
      console.log(data);
      $("#spnShowTreansferAmount").show();
      $("#spnShowTreansferAmount").html(data.reciedAmount);
      $("#spnShowTransfer").show();
      $("#spnShowTransfer").html(data.sentAmount);
      $("#spnShowBalance").show();
      $("#spnShowBalance").html(data.newbalance);
    } else {
      if (data.noUser != null || data.noUser != "") {
        alert(data.noUser);
      }
    }
  }
  var vToTaxPayer = new Bloodhound({
    datumTokenizer: function (d) {
      return Bloodhound.tokenizers.whitespace(d.text);
    },
    queryTokenizer: Bloodhound.tokenizers.whitespace,
    limit: 10,
    remote: {
      url: "/Base/GetTaxPayer",
      replace: function (url, uriEncodedQuery) {
        var vRequestUrl =
          url +
          "?TaxPayerTypeID=" +
          $("#cboToTaxPayerType").val() +
          "&query=" +
          encodeURIComponent(uriEncodedQuery);
        return vRequestUrl;
      },
      filter: function (list) {
        return $.map(list, function (tp) {
          return {
            text: tp.text,
            id: tp.id,
          };
        });
      },
    },
  });

  vToTaxPayer.initialize();

  var vToTaxPayerTypeHead = $("#txtToTaxPayer");

  vToTaxPayerTypeHead.typeahead(null, {
    name: "txtToTaxPayer",
    displayKey: "text",
    source: vToTaxPayer.ttAdapter(),
  });

  var vToTaxPayerSelectedHandler = function (
    eventObject,
    suggestionObject,
    suggestionDataset
  ) {
    $("#hdnToTaxPayerID").val(suggestionObject.id);

    var vTaxpayerTypeID = $("#cboToTaxPayerType").val();
    var vTaxPayerID = $("#hdnToTaxPayerID").val();

    var vData = {
      pIntTaxPayerTypeID: vTaxpayerTypeID == "" ? 0 : vTaxpayerTypeID,
      pIntTaxPayerID: vTaxPayerID == "" ? 0 : vTaxPayerID,
    };

    jsfn_ajaxPost(
      "/OperationManager/GetBalance/",
      vData,
      jsfn_onGetToBalanceResponse
    );
  };

  vToTaxPayerTypeHead.on("typeahead:selected", vToTaxPayerSelectedHandler);
});

function jsfn_GetBalanceForFromTaxPayer() {
  var vTaxpayerTypeID = $("#cboFromTaxPayerType").val();
  var vTaxPayerID = $("#hdnFromTaxPayerID").val();

  var vData = {
    pIntTaxPayerTypeID: vTaxpayerTypeID == "" ? 0 : vTaxpayerTypeID,
    pIntTaxPayerID: vTaxPayerID == "" ? 0 : vTaxPayerID,
  };

  jsfn_ajaxPost(
    "/OperationManager/GetBalance/",
    vData,
    jsfn_onGetFromBalanceResponse
  );
}

function jsfn_onFromTaxPayerType() {
  $("#hdnFromTaxPayerID").val("");
  $("#txtFromTaxPayer").val("");
}
function jsfn_onToTaxPayerType() {
  $("#hdnToTaxPayerID").val("");
  $("#txtToTaxPayer").val("");
}

function jsfn_onGetFromBalanceResponse(data) {
  if (data.success) {
    $("#spnShowFromBalance").show();
    $("#spnShowFromBalance").html(data.Balance);
  }
}

function jsfn_onGetToBalanceResponse(data) {
  if (data.success) {
    $("#spnShowToBalance").show();
    $("#spnShowToBalance").html(data.Balance);
  }
}
