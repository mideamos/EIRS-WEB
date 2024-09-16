// The workerSrc property shall be specified.
PDFJS.workerSrc = '/Content/Plugins/PDFJS/pdf.worker.js';

var pdfDoc = null,
    pageNum = 1,
    pageRendering = false,
    pageNumPending = null,
    scale = 1,
    canvas = document.getElementById('cnvDocument'),
    ctx = canvas.getContext('2d');

$(document).ready(function () {
    $("#btnPrev").click(onPrevPage);
    $("#btnNext").click(onNextPage);

    openDocument($("#hdnFilePreview").val());
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
        canvas.height = viewport.height;
        canvas.width = viewport.width;
        $("canvas").css("width", $(".document-pagination").width());
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