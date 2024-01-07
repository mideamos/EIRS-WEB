$(document).ready(function () {
    jsfn_BindUploader();
});

function jsfn_BindUploader() {
    var uploader = new plupload.Uploader({
        runtimes: 'html5,flash,silverlight,html4',
        browse_button: document.getElementById('tab_images_uploader_pickfiles'),
        container: document.getElementById('tab_images_uploader_container'),
        url: "/ProcessTCCRequest/UploadNotesAttachment",
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
                    $('#dvUDNotes').html('');
                    $('#dvAttachmentDocument').html(vResponseData.Data);
                }
                else {
                    $('#dvUDNotes').html(vResponseData.Message);
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

    jsfn_ajaxPost("/ProcessTCCRequest/RemoveNotesAttachment", data, jsfn_RemoveDocumentResponse);
}

function jsfn_RemoveDocumentResponse(data) {
    if (data.success) {
        $('#dvAttachmentDocument').html(data.DocumentList);
    }
}

function jsfn_SendNotes() {
    var vNotes = $('#txtNotes').val();
    var vStageID = $('#hdnStageID').val();
    var vRequestID = $('#hdnRequestID').val();

    if (vNotes === '') {
        $('#dvSNotes').html("<div class='alert alert-danger'> Please enter notes. </div>");
    }
    else {
        jsfn_ShowLoading();
        var data = { RequestID: vRequestID, StageID: vStageID, Notes: vNotes };
        jsfn_ajaxPost('/ProcessTCCRequest/SendNotes', data, jsfn_SendNotesResponse);
    }
}

function jsfn_SendNotesResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#txtNotes').val('');
        $('#dvSNotes').html(data.Message);
        $('#dvAttachmentDocument').html('');
    }
    else {
        $('#dvSNotes').html(data.Message);
    }
}