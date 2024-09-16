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
            imageWidth = parseInt($("#cnvDocument").width() - 30);
            $('<div class="signer-element selected-element" status="drop" resizeable="true" type="signature" page="' + pageNum + '"><img src="' + vSignaturePath + '" style="max-width:' + imageWidth + 'px;width:200px;opacity:0.5;"></div>').appendTo(".signer-builder");
            $(document).mousemove(function (event) {
                $(".signer-element[status=drop]").css({ left: event.pageX + 1, top: event.pageY + 1 });
            });

            disableTools();
            highlightCanvas();
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

            imageWidth = parseInt($("#cnvDocument").width() - 30);
            $('<div class="signer-element selected-element" status="drop" resizeable="true" type="signature" page="' + pageNum + '"><img src="' + vSignaturePath + '" style="max-width:' + imageWidth + 'px;width:200px;opacity:0.5;"></div>').appendTo(".signer-builder");
            $(document).mousemove(function (event) {
                $(".signer-element[status=drop]").css({ left: event.pageX + 1, top: event.pageY + 1 });
            });

            disableTools();
            highlightCanvas();

            $("#btnUCRemove").trigger('click');
        }
    });



    $("#btnUndoSignature").click(function () {
        $('.signer-builder').children().last().remove();
    });

    $("#btnSavedSignature").click(function () {
        $(".signer-element").removeClass("selected-element");
        var vSignaturePath = $("#hdnSavedSignaturePath").val();

        if (vSignaturePath === '') {
            jsfn_ShowAlert('Saved Signature not available for this user', 'danger');
        }
        else {
            toDataURL(vSignaturePath, function (dataUrl) {
                $("#hdnSignType").val('1');
                imageWidth = parseInt($("#cnvDocument").width() - 30);
                $('<div class="signer-element selected-element" status="drop" resizeable="true" type="signature" page="' + pageNum + '"><img src="' + dataUrl + '" style="max-width:' + imageWidth + 'px;width:200px;opacity:0.5;"></div>').appendTo(".signer-builder");
                $(document).mousemove(function (event) {
                    $(".signer-element[status=drop]").css({ left: event.pageX + 1, top: event.pageY + 1 });
                });

                disableTools();
                highlightCanvas();
            });
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

    //$(".signer-builder").on("click", ".signer-element", function () {
    //    var vSignaturePath = $("#hdnSavedSignaturePath").val();
    //    $(".signer-element").removeClass("selected-element");
    //    $(this).addClass("selected-element");
    //    $(this).attr("signed", "true");
    //    $(this).find("img").attr("src", vSignaturePath);

    //});


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

                if ($("#hdnAdditionalSignatureLocation").val() !== '') {
                    var vSignaturePath = $("#hdnSavedSignaturePath").val();
                    var viewerPosition = $("#cnvDocument").offset();
                    var vArrSignaturePosition = jQuery.parseJSON($("#hdnAdditionalSignatureLocation").val());
                    $.each(vArrSignaturePosition, function (i, item) {
                        imageWidth = parseInt($("#cnvDocument").width() - 30);
                        $('<div class="signer-element selected-element" resizeable="true" type="signature" page="' + item.pageNumber + '" style="left: ' + (viewerPosition.left + item.xPosition - 6).toFixed(2) + 'px;top: ' + (viewerPosition.top + item.yPosition - 6).toFixed(2) + 'px; position: absolute; width: ' + (item.width + 10).toFixed(2) + 'px; height: ' + (item.height + 12).toFixed(2) + 'px;display:' + (item.pageNumber === 1 ? "block" : "none") + '"><img src="' + vSignaturePath + '" style="max-width:' + imageWidth + 'px;width:' + item.width + 'px;opacity:1;height:' + item.height + 'px;"></div>').appendTo(".signer-builder");
                    });

                    initElementsDrag();
                    initElementsResize();
                }

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
    var vArrSignaturePosition = [];
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

    var vData = {
        CertificateID: $("#hdnCertificateID").val(),
        CSVID: $("#hdnCSVID").val(),
        AdditionalSignatureLocation: JSON.stringify(vArrSignaturePosition),
        SignNotes: $("#txtNotes").val(),
        DocumentWidth: $("#cnvDocument").width(),
        SignSourceID: $("#hdnSignType").val()
    };
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Staff/SignCertificateVisible', vData, jsfnSignatureResponse);
}

function jsfnSignatureResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        setTimeout(function () {
            window.location.href = '/Staff/SignCertificateList';
        }, 500);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function disableTools() {
    $("#btnSavedSignature").addClass("disabled");
    $("#btnUploadSignature").addClass("disabled");
    $("#btnSignaturePad").addClass("disabled");
}

function enableTools() {
    $("#btnSavedSignature").removeClass("disabled");
    $("#btnUploadSignature").removeClass("disabled");
    $("#btnSignaturePad").removeClass("disabled");
}

function highlightCanvas() {
    $("#cnvDocument").addClass("active");
}

/*
 *  Un highlight document canvas
 */
function unHighlightCanvas() {
    $("#cnvDocument").removeClass("active");
}

/*
 *  When any area on the overlay is clicked
 */
$(".signer-body").click(function (event) {
    event.preventDefault();
    if ($(".signer-element[status=drop]").length > 0) {
        if (event.target.id === "cnvDocument") {
            $(".signer-element[status=drop]").css("top", parseInt(event.pageY + $(".signer-body").scrollTop()));
            $(".signer-element").removeAttr("status");
            $(".signer-element").css('position', 'absolute');
            $(".signer-element img").css('opacity', '1');
            enableTools();
            unHighlightCanvas();
            initElementsDrag();
            initElementsResize();
        }
    } else if ($(".signer-tool.active[tool=text]").length && event.target.id === "cnvDocument") {
        addText(event.pageX, event.pageY);
    }
});

function initElementsResize() {
    $(".signer-element[resizeable=true]").resizable({
        aspectRatio: true,
        autoHide: false,
        containment: "#cnvDocument",
        handles: "n, e, s, w, se, sw, nw, ne",
        resize: function (event, ui) {
            ui.helper.find("img").width(ui.size.width - 10);
            ui.helper.find("img").height(ui.size.height - 12);
        }
    });
}

function initElementsDrag() {
    $(".signer-element").draggable({
        containment: $("#cnvDocument"),
        drag: function () {
            highlightCanvas();
        },
        stop: function () {
            unHighlightCanvas();
        }
    });
}

/**
 * If another page rendering in progress, waits until the rendering is
 * finised. Otherwise, executes rendering immediately.
 */

function queueRenderPage(num) {
    if (pageRendering) {
        pageNumPending = num;
    } else {
        renderPage(num);
    }
}

/**
 * Displays previous page.
 */
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
