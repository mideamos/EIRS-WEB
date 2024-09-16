$(document).ready(function () {
    jsfn_BindUploader();
});

function jsfn_BindUploader() {
    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: document.getElementById('tab_images_uploader_pickfiles'),
        container: document.getElementById('tab_images_uploader_container'),
        url: "/Base/UploadMessageAttachment",
        filters: {
            max_file_size: '10mb',
            mime_types: [
                { title: "document", extensions: "pdf" }
            ]
        },
        // Flash settings
        flash_swf_url: '/Content/Plugins/plupload/js/Moxie.swf',
        // Silverlight settings
        silverlight_xap_url: '/Content/Plugins/plupload/js/Moxie.xap',
        init: {
            FilesAdded: function (up, files) {
                up.start();
                jsfn_ShowLoading();
            },
            FileUploaded: function (up, file, response) {
                jsfn_HideLoading();
                var vResponseData = $.parseJSON(response.response);
                if (vResponseData.Status && vResponseData.Status === 'Ok') {
                    $('#dvUDMessage').html('');
                    $('#dvAttachmentDocument').html(vResponseData.Data);
                }
                else {
                    $('#dvUDMessage').html(vResponseData.Message);
                }
            }
        }
    });
    uploader.init();
}

function jsfn_RemoveDocument(fileId) {
    var vFileID = fileId;

    var data = {
        id: vFileID
    };

    jsfn_ajaxPost("/Base/RemoveMessageAttachment", data, jsfn_RemoveDocumentResponse);
}

function jsfn_RemoveDocumentResponse(data) {
    if (data.success) {
        $('#dvAttachmentDocument').html(data.DocumentList);
    }
}

function jsfn_SendMessage() {
    var vMessage = $('#txtMessage').val();
    var vTaxPayerID = $('#hdnTaxPayerID').val();
    var vTaxPayerTypeID = $('#hdnTaxPayerTypeID').val();

    if (vMessage === '') {
        $('#dvSMessage').html("<div class='alert alert-danger'> Please enter message. </div>");
    }
    else {
        jsfn_ShowLoading();
        var data = { TaxPayerID: vTaxPayerID, TaxPayerTypeID: vTaxPayerTypeID,  Message: vMessage };
        jsfn_ajaxPost('/Base/SendMessage', data, jsfn_SendMessageResponse);
    }
}

function jsfn_SendMessageResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#txtMessage').val('');
        $('#dvSMessage').html(data.Message);
        setTimeout(function () {
            $('#dvSMessage').html('');
            $('#dvMessaging').html(data.MessageList);
            jsfn_BindUploader();
        }, 2000);
    }
    else {
        $('#dvSMessage').html(data.Message);
    }
}