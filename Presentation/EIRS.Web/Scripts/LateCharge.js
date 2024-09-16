$(document).ready(function () {
   
});

let lastUrl = location.href;
$("#btnSubmit").click(function () {
    debugger

    jsfn_ShowLoading();
   /* $("#divLoader").show();*/
    $("#divButton").hide();
    $.ajax
        ({
            url: '/OperationManager/LateChargeDetailII/1',
            dataType: "json",
            type: "Get",
            contentType: 'application/json; charset=utf-8',
            data: {},
            //async: true,
            //processData: false,
            // cache: false,
            success: function (data) {
              //  $("#divLoader").hide();
                jsfn_HideLoading();
            },
            error: function () {
                console.log("na here i enter");
               // $("#divLoader").hide();
                const url = location.href;
                if (url !== lastUrl) {
                    lastUrl = url;
                    onUrlChange();
                }
                jsfn_HideLoading();
                $("#divLoader").show();
            }
        })
});
$("#btnSubmitII").click(function () {
    debugger

    jsfn_ShowLoading();
   /* $("#divLoader").show();*/
    $("#divButton").hide();
    $.ajax
        ({
            url: '/OperationManager/LateChargeDetailStep4',
            dataType: "json",
            type: "Get",
            contentType: 'application/json; charset=utf-8',
            data: {},
            //async: true,
            //processData: false,
            // cache: false,
            success: function (data) {
              //  $("#divLoader").hide();
                jsfn_HideLoading();
            },
            error: function () {
                console.log("na here i enter");
               // $("#divLoader").hide();
                const url = location.href;
                if (url !== lastUrl) {
                    lastUrl = url;
                    onUrlChange();
                }
                jsfn_HideLoading();
                $("#divLoader").show();
            }
        })
});
$("#btnSubmitIII").click(function () {
    debugger

    jsfn_ShowLoading();
   /* $("#divLoader").show();*/
    $("#divButton").hide();
    $.ajax
        ({
            url: '/OperationManager/LateChargeDetailII/2',
            dataType: "json",
            type: "Get",
            contentType: 'application/json; charset=utf-8',
            data: {},
            //async: true,
            //processData: false,
            // cache: false,
            success: function (data) {
              //  $("#divLoader").hide();
                jsfn_HideLoading();
            },
            error: function () {
                console.log("na here i enter");
               // $("#divLoader").hide();
                const url = location.href;
                if (url !== lastUrl) {
                    lastUrl = url;
                    onUrlChange();
                }
                jsfn_HideLoading();
                $("#divLoader").show();
            }
        })
});