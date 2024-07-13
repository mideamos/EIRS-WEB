var responsiveHelper_datatable_fixed_column = undefined;

var breakpointDefinition = {
    tablet: 1024,
    phone: 480
};

function jsfn_ajaxPost(p_url, p_urlParameter, p_ResponseFunction) {
    $.ajax({
        url: p_url,
        type: 'POST',
        dataType: 'json',
        data: p_urlParameter,
        success: p_ResponseFunction,
        error: function (jqXHR, exception) {
            if (jqXHR.status == 401) {
                window.location.href = '/Login.aspx';
            }
        }
    });
}

function jsfn_ajaxPost_Html(p_url, p_urlParameter, p_ResponseFunction) {
    $.ajax({
        url: p_url,
        type: 'POST',
        dataType: 'html',
        data: p_urlParameter,
        success: p_ResponseFunction,
        error: function (jqXHR, exception) {
            if (jqXHR.status == 401) {
                window.location.href = '/Login.aspx';
            }
        }
    });
}

function jsfn_ShowLoading() {
    $.blockUI({
        message: $('#domMessage'),
        baseZ: 10099,
        css: {
            border: 'none',
            padding: '15px',
            backgroundColor: '#000',
            '-webkit-border-radius': '10px',
            '-moz-border-radius': '10px',
            opacity: .5,
            color: '#fff'
        }
    });
}

function jsfn_HideLoading() {
    $.unblockUI();
}

function jsfnAjaxFailure(ajaxerror) {
    if (ajaxerror.status == 401) {
        window.location.href = '/Login';
    }
}

function jsfn_ConvertToDatatable(p_Table) {

    responsiveHelper_datatable_fixed_column = undefined;

    var otable = p_Table.DataTable({
        "searching": true,
        "sDom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
        "t" +
        "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",
        "autoWidth": true,
        "preDrawCallback": function () {
            if (!responsiveHelper_datatable_fixed_column) {
                responsiveHelper_datatable_fixed_column = new ResponsiveDatatablesHelper(p_Table, breakpointDefinition);
            }
        },
        "rowCallback": function (nRow) {
            responsiveHelper_datatable_fixed_column.createExpandIcon(nRow);
        },
        "drawCallback": function (oSettings) {
            responsiveHelper_datatable_fixed_column.respond();
        }

    });

    $(p_Table.selector + " thead th input[type=text]").on('change', function () {

        otable
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();

    });

    return otable;

}

function jsfn_ShowAlert(pMessage, pAlertType) {
    App.alert({
        container: '#dvAlertConatiner', // alerts parent container(by default placed after the page breadcrumbs)
        place: 'append', // append or prepent in container 
        type: pAlertType,  // alert's type
        message: pMessage,  // alert's message
        close: true, // make alert closable
        reset: true, // close all previouse alerts first
        focus: false, // auto scroll to the alert after shown
        closeInSeconds: '5', // auto close after defined seconds
        icon: '' // put icon before the message
    });
}

function jsfn_FormatJsonDate(pJsonDate, mask) {
    if (pJsonDate != null && pJsonDate != undefined) {
        var vStringDate;

        vStringDate = pJsonDate.replace(/\D/g, "");
        vDate = new Date(parseInt(vStringDate));

        return vDate.format(mask);
    } else {
        return " ";
    }
}

// Extend the default Number object with a formatMoney() method:
// usage: someVar.formatMoney(decimalPlaces, symbol, thousandsSeparator, decimalSeparator)
// defaults: (2, "$", ",", ".")
Number.prototype.formatMoney = function (places, symbol, thousand, decimal) {
    places = !isNaN(places = Math.abs(places)) ? places : 2;
    symbol = symbol !== undefined ? symbol : "₦";
    thousand = thousand || ",";
    decimal = decimal || ".";
    var number = this,
        negative = number < 0 ? "-" : "",
        i = parseInt(number = Math.abs(+number || 0).toFixed(places), 10) + "",
        j = (j = i.length) > 3 ? j % 3 : 0;
    return symbol + negative + (j ? i.substr(0, j) + thousand : "") + i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousand) + (places ? decimal + Math.abs(number - i).toFixed(places).slice(2) : "");
};


String.prototype.toSeoUrl = function () {
    var url = this;
    return url.toString()               // Convert to string
        .normalize('NFD')               // Change diacritics
        .replace(/[\u0300-\u036f]/g, '') // Remove illegal characters
        .replace(/\s+/g, '-')            // Change whitespace to dashes
        .toLowerCase()                  // Change to lowercase
        .replace(/&/g, '-and-')          // Replace ampersand
        .replace(/[^a-z0-9\-]/g, '')     // Remove anything that is not a letter, number or dash
        .replace(/-+/g, '-')             // Remove duplicate dashes
        .replace(/^-*/, '')              // Remove starting dashes
        .replace(/-*$/, '');             // Remove trailing dashes
}

function toSeoUrl(url) {
    return url.toString()               // Convert to string
        .normalize('NFD')               // Change diacritics
        .replace(/[\u0300-\u036f]/g, '') // Remove illegal characters
        .replace(/\s+/g, '-')            // Change whitespace to dashes
        .toLowerCase()                  // Change to lowercase
        .replace(/&/g, '-and-')          // Replace ampersand
        .replace(/[^a-z0-9\-]/g, '')     // Remove anything that is not a letter, number or dash
        .replace(/-+/g, '-')             // Remove duplicate dashes
        .replace(/^-*/, '')              // Remove starting dashes
        .replace(/-*$/, '');             // Remove trailing dashes
}


