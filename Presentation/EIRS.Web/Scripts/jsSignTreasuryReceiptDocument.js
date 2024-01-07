// The workerSrc property shall be specified.
PDFJS.workerSrc = '/Content/Plugins/PDFJS/pdf.worker.js';
var vSignaturePad, cnvSignaturePad;

var pdfDoc = null,
    pageNum = 1,
    pageRendering = false,
    pageNumPending = null,
    scale = 1,
    cnvDocument = document.getElementById('cnvDocument'),
    ctx = cnvDocument.getContext('2d');

cnvSignaturePad = document.getElementById('cnvSignaturePad');
vSignaturePad = new SignaturePad(cnvSignaturePad, {
    // It's Necessary to use an opaque color when saving image as JPEG;
    // this option can be omitted if only saving as PNG or SVG
    //backgroundColor: 'rgb(255, 255, 255)'
});

// Adjust canvas coordinate space taking into account pixel ratio,
// to make it look crisp on mobile devices.
// This also causes canvas to be cleared.
function resizeSignPadCanvas() {
    // When zoomed out to less than 100%, for some very strange reason,
    // some browsers report devicePixelRatio as less than 1
    // and only part of the canvas is cleared then.
    var ratio = Math.max(window.devicePixelRatio || 1, 1);

    // This part causes the canvas to be cleared
    cnvSignaturePad.width = cnvSignaturePad.offsetWidth * ratio;
    cnvSignaturePad.height = cnvSignaturePad.offsetHeight * ratio;
    cnvSignaturePad.getContext("2d").scale(ratio, ratio);

    // This library does not listen for canvas changes, so after the canvas is automatically
    // cleared by the browser, SignaturePad#isEmpty might still return false, even though the
    // canvas looks empty, because the internal data of this library wasn't cleared. To make sure
    // that the state of this library is consistent with visual state of the canvas, you
    // have to clear it manually.
    vSignaturePad.clear();
}

window.onresize = resizeSignPadCanvas;
resizeSignPadCanvas();

$(document).ready(function () {

    $("#btnPrev").click(onPrevPage);
    $("#btnNext").click(onNextPage);

    openDocument($("#hdnFilePreview").val());

    $("#dvSignPadModel").on('shown.bs.modal', function (e) {
        resizeSignPadCanvas();
    });

    $("#dvSignPadModel").on('hidden.bs.modal', function (e) {
        $(".signer-element").removeClass("selected-element");
        var vSignaturePath = $("#hdnSignaturePadPath").val();

        if (vSignaturePath === '') {
            //jsfn_ShowAlert('Saved Signature not available for this user', 'danger');
        }
        else {
            //imageWidth = parseInt($("#cnvDocument").width() - 30);
            appendFixedSignature(vSignaturePath);
            disableTools();
        }
    });

    $("#btnUCSave").click(function () {
        $("#dvUploadSignModel").modal('hide');
        $("#hdnSignType").val('2');

        //Save Image
        var vdvUCFilePreview = $("#dvUCFilePreview");
        var vSignaturePath = vdvUCFilePreview.find('img').attr('src');
        $("#hdnUploadSignaturePath").val(vSignaturePath);

        if (vSignaturePath === undefined) {
            jsfn_ShowAlert('Please Upload Signature', 'danger');
        }
        else {
            //imageWidth = parseInt($("#cnvDocument").width() - 30);
            appendFixedSignature(vSignaturePath);
            disableTools();

            $("#btnUCRemove").trigger('click');
        }
    });



    $("#btnUndoSignature").click(function () {
        openDocument($("#hdnFilePreview").val());
        //$('.signer-builder').children().last().remove();
        enableTools();
    });

    $("#btnSavedSignature").click(function () {
        $(".signer-element").removeClass("selected-element");
        var vSignaturePath = $("#hdnSavedSignaturePath").val();

        if (vSignaturePath === '') {
            jsfn_ShowAlert('Saved Signature not available for this user', 'danger');
        }
        else {
            $("#hdnSignType").val('1');
            appendFixedSignature(vSignaturePath);
            disableTools();
        }
    });


    $("#btnSignaturePad").click(function () {
        $("#dvSignPadModel").modal('show');
    });

    $("#btnUploadSignature").click(function () {
        $("#dvUploadSignModel").modal('show');
    });

    $("#btnSPClear").click(function () {
        vSignaturePad.clear();
    });


    $("#btnSPUndo").click(function () {
        var data = vSignaturePad.toData();

        if (data) {
            data.pop(); // remove the last dot or line
            vSignaturePad.fromData(data);
        }
    });

    $("#btnSPSave").click(function () {
        var vSignaturePath = vSignaturePad.toDataURL();
        $("#hdnSignaturePadPath").val(vSignaturePath);
        vSignaturePad.off();
        $("#dvSignPadModel").modal('hide');
        $("#hdnSignType").val('3');

    });

    $("#btnSave").click(function () {
        jsfn_PrepareData();

    });
});

/**
 * Get page info from document, resize canvas accordingly, and render page.
 * @param num Page number.
 */
//required
function renderPage(num) {

    $(".signer-element").hide();
    jsfn_ShowLoading();
    pageRendering = true;
    // Using promise to fetch the page
    pdfDoc.getPage(num).then(function (page) {
        var viewport = page.getViewport(scale);
        cnvDocument.height = viewport.height;
        cnvDocument.width = viewport.width;
        $("#cnvDocument").css("width", $(".document-pagination").width());
        // Render PDF page into canvas context
        var renderContext = {
            canvasContext: ctx,
            viewport: viewport
        };
        var renderTask = page.render(renderContext);

        // Wait for rendering to finish
        renderTask.promise.then(function () {
            jsfn_HideLoading();
            $("[page=" + pageNum + "]").show();
            if (pageNum === pdfDoc.numPages) {
                $("#btnNext").addClass("disabled");
            } else {
                $("#btnNext").removeClass("disabled");
            }

            if (pageNum === 1) {
                $("#btnPrev").addClass("disabled");
            } else {
                $("#btnPrev").removeClass("disabled");
            }

            pageRendering = false;
            if (pageNumPending !== null) {
                // New page rendering is pending
                renderPage(pageNumPending);
                pageNumPending = null;
            }
        });
    });

    // Update page counters
    $("#spnCurrentPage").html(pageNum);
}

function jsfn_PrepareData() {
    /*var vArrSignaturePosition = [];
    if ($(".signer-builder").length > 0) {
        $('.signer-builder .signer-element').each(function (index, value) {
            var signerElement = $(this), actionType = signerElement.attr('type'), thisImage, thissignsource;
            signerElement.show();
            viewerPosition = $("#cnvDocument").offset();
            group = signerElement.attr('group');
            pageNumber = parseInt(signerElement.attr('page'));
            thisImage = signerElement.find("img").attr('src');
            thissignsource = signerElement.find("img").attr('data-signtype');
            elementWidth = signerElement.find("img").width();
            elementHeight = signerElement.find("img").height();
            elementPosition = signerElement.find("img").offset();
            elementXpos = elementPosition.left - viewerPosition.left;
            elementYpos = elementPosition.top - viewerPosition.top;
            vArrSignaturePosition.push({
                "pageNumber": pageNumber,
                "xPosition": elementXpos,
                "yPosition": elementYpos,
                "width": elementWidth,
                "height": elementHeight,
                "imgSrc": thisImage,
                "signsource": thissignsource
            });
            if (pageNumber === pageNum) {
                signerElement.show();
            } else {
                signerElement.hide();
            }
        });
    }

    console.log(JSON.stringify(vArrSignaturePosition));
    */
    var vImgData = '';


    if ($("#hdnSignType").val() === '1') {
        vImgData = $("#hdnSavedSignaturePath").val();
    }
    else if ($("#hdnSignType").val() === '2') {
        vImgData = $("#hdnUploadSignaturePath").val();
    }
    else if ($("#hdnSignType").val() === '3') {
        vImgData = $("#hdnSignaturePadPath").val();
    }

    var vData = {
        ReceiptID: $("#hdnReceiptID").val(),
        ImgSrc: vImgData,
        SignNotes: $("#txtNotes").val(),
        SignSourceID: $("#hdnSignType").val()
    };
    jsfn_ShowLoading();
    jsfn_ajaxPost('/TreasuryReceipt/SignReceipt', vData, jsfnSignatureResponse);

}

function jsfnSignatureResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        setTimeout(function () {
            window.location.href = '/Settle/Receipt';
        }, 500);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function disableTools() {
    $("#btnSavedSignature").attr('disabled', 'disabled');
    $("#btnUploadSignature").attr('disabled', 'disabled');
    $("#btnSignaturePad").attr('disabled', 'disabled');
}

function enableTools() {
    $("#btnSavedSignature").removeAttr("disabled");
    $("#btnUploadSignature").removeAttr("disabled");
    $("#btnSignaturePad").removeAttr("disabled");
}


function queueRenderPage(num) {
    if (pageRendering) {
        pageNumPending = num;
    } else {
        renderPage(num);
    }
}

function onPrevPage() {
    if (pageNum <= 1) {
        return;
    }
    pageNum--;
    queueRenderPage(pageNum);
}

/**
 * Displays next page.
 */
function onNextPage() {
    if (pageNum >= pdfDoc.numPages) {
        return;
    }
    pageNum++;
    queueRenderPage(pageNum);
}


function openDocument(url) {

    PDFJS.getDocument({ url: url }).then(function (pdfDoc_) {
        pdfDoc = pdfDoc_;
        $("#spnPageCount").html(pdfDoc.numPages);

        // Initial/first page rendering
        renderPage(pageNum);

        if (pdfDoc.numPages === 1) {
            $("#btnNext, #btnPrev").addClass("disabled");
        }
    }).catch(function (error) {
        console.log(error);
    });
}

function toDataURL(url, callback) {
    var xhr = new XMLHttpRequest();
    xhr.onload = function () {
        var reader = new FileReader();
        reader.onloadend = function () {
            callback(reader.result);
        };
        reader.readAsDataURL(xhr.response);
    };
    xhr.open('GET', url);
    xhr.responseType = 'blob';
    xhr.send();
}

function appendFixedSignature(imgData) {
    var vData = {
        imgSrc: imgData,
        SignSourceID: $("#hdnSignType").val(),
        ReceiptID: $("#hdnReceiptID").val()
    }

    jsfn_ShowLoading();
    jsfn_ajaxPost('/TreasuryReceipt/FixedSign', vData, jsfnFixedSignResponse);

    //var vApproverNo = ;
    //var vLeftPx;
    //var vTopPx;
    //var scale = cnvDocument.width / $("#cnvDocument").width();

    //if (vApproverNo === '1') {
    //    vLeftPx = 910;
    //    vTopPx = 1392;
    //}
    //else if (vApproverNo === '2') {
    //    vLeftPx = 1350 * scale;
    //    vTopPx = 1392 * scale;
    //}
    //else if (vApproverNo === '3') {
    //    vLeftPx = 1095;
    //    vTopPx = 1540 * scale;
    //}




    //$('<div class="signer-element" type="signature" page="' + pageNum + '" style="left: ' + vLeftPx + 'px;top:' + vTopPx + 'px;position:absolute"><img src="' + imgData + '" style="max-width:' + imageWidth + 'px;width:200px;opacity:1;"></div>').appendTo(".signer-builder");
}

function jsfnFixedSignResponse(data) {
    jsfn_HideLoading();
    debugger
   // console.log(data);
    openDocument(data['PDFPath']);
    //window.location.href = Url.Action("GISSearchParameter", "OperationManager");
   // window.localtion.href = '/TreasuryReceipt/SignReceipt/' + id;
}
