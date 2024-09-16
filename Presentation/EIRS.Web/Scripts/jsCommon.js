var TaxPayerType, AssetType;
$(document).ready(function () {
    $("input").attr("autocomplete", "off");
    $.validator.methods.date = function (value, element) {
        return this.optional(element) || moment(value, "DD/MM/YYYY", true).isValid();
    }

    $("#chkMenuSwitch").click(function () {
        window.location.href = $("#chkMenuSwitch")[0].parentNode.parentNode.href;
    });

    //$('.dropdown-menu li').on('click', function (event) {
    //    var $checkbox = $(this).find('.checkbox');
    //    if (!$checkbox.length) {
    //        return;
    //    }
    //    var $input = $checkbox.find('input');
    //    var $icon = $checkbox.find('span.fa');
    //    if ($input.is(':checked')) {
    //        $input.prop('checked', false);
    //        $icon.removeClass('fa-check-square-o').addClass('fa-square-o')
    //    } else {
    //        $input.prop('checked', true);
    //        $icon.removeClass('fa-square-o').addClass('fa-check-square-o')
    //    }
    //    return false;
    //});

    function jsfn_OnEnterPress(p_Div, p_Button) {
        p_Div.bind('keydown', function (e) {
            if (e.keyCode === 13) {
                p_Button.click();
            }
        });
    }

    handleSelectPicker();
    handleSumoSelect();
    handleiCheck();
    handlePortletTools();
    handleTooltips();

    TaxPayerType = Object.freeze({ "Individual": 1, "Company": 2, "Government": 4, "Special": 3 });
    AssetType = Object.freeze({ "Building": 1, "Vehicle": 2, "Business": 3, "Land": 4 });

    $('#txtMasterSearch').keyup(function (e) {
        if (e.keyCode == 13) {
            jsfn_CheckRINExist();
        }
    });

});

function jsfn_ajaxPost(p_url, p_urlParameter, p_ResponseFunction) {
    $.ajax({
        url: p_url,
        type: 'POST',
        dataType: 'json',
        data: p_urlParameter,
        success: p_ResponseFunction,
        error: function (jqXHR, exception) {
            if (jqXHR.status == 401) {
                window.location.href = '/Login/Individual';
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
                window.location.href = '/Login/Individual';
            }
        }
    });
}


function jsfn_ConvertToDatableWithCustomSort(p_Table, p_ColumnsList, p_EmptyTableMsg, p_SortOrder, p_columnDefs) {

    //$.extend(true, $.fn.DataTable.TableTools.classes, {
    //    "container": "btn-group tabletools-btn-group pull-right",
    //    "buttons": {
    //        "normal": "btn btn-sm default",
    //        "disabled": "btn btn-sm default disabled"
    //    }
    //});

    p_Table.dataTable({

        // Internationalisation. For more info refer to http://datatables.net/manual/i18n
        "language": {
            "aria": {
                "sortAscending": ": activate to sort column ascending",
                "sortDescending": ": activate to sort column descending"
            },
            "emptyTable": "No data available in table",
            "info": "Showing _START_ to _END_ of _TOTAL_ entries",
            "infoEmpty": "No entries found",
            "infoFiltered": "(filtered1 from _MAX_ total entries)",
            "lengthMenu": "Show _MENU_ entries",
            "search": "Search:",
            "zeroRecords": "No matching records found"
        },

        "dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

        "bStateSave": false, // save datatable state(pagination, sort, etc) in cookie.

        "columns": p_ColumnsList,
        "lengthMenu": [
            [10, 20, 30, 40, 50, -1],
            [10, 20, 30, 40, 50, "All"] // change per page values here
        ],
        // set the initial value
        "pageLength": 10,
        "pagingType": "bootstrap_full_number",
        "language": {
            "search": "Search : ",
            "emptyTable": p_EmptyTableMsg,
            "lengthMenu": "  _MENU_ records",
            "paginate": {
                "previous": "Prev",
                "next": "Next",
                "last": "Last",
                "first": "First"
            }
        },
        "responsive": true,
        "columnDefs": p_columnDefs,
        "order": p_SortOrder // set first column as a default sort by asc
    });

    return p_Table;
}

function jsfn_ConvertToDatableWithCustomPaging(p_Table, p_ColumnsList, p_EmptyTableMsg, p_DefaultPageLength, p_columnDefs) {

    //$.extend(true, $.fn.DataTable.TableTools.classes, {
    //    "container": "btn-group tabletools-btn-group pull-right",
    //    "buttons": {
    //        "normal": "btn btn-sm default",
    //        "disabled": "btn btn-sm default disabled"
    //    }
    //});

    p_Table.dataTable({

        // Internationalisation. For more info refer to http://datatables.net/manual/i18n
        "language": {
            "aria": {
                "sortAscending": ": activate to sort column ascending",
                "sortDescending": ": activate to sort column descending"
            },
            "emptyTable": "No data available in table",
            "info": "Showing _START_ to _END_ of _TOTAL_ entries",
            "infoEmpty": "No entries found",
            "infoFiltered": "(filtered1 from _MAX_ total entries)",
            "lengthMenu": "Show _MENU_ entries",
            "search": "Search:",
            "zeroRecords": "No matching records found"
        },

        "dom": "<'row' <'col-md-12'T>><'row'<'col-md-6 col-sm-12'l><'col-md-6 col-sm-12'f>r><'table-scrollable't><'row'<'col-md-5 col-sm-12'i><'col-md-7 col-sm-12'p>>", // horizobtal scrollable datatable

        "bStateSave": false, // save datatable state(pagination, sort, etc) in cookie.

        "columns": p_ColumnsList,
        "lengthMenu": [
            [5, 10, 20, 30, 40, 50, -1],
            [5, 10, 20, 30, 40, 50, "All"] // change per page values here
        ],
        // set the initial value
        "pageLength": p_DefaultPageLength,
        "pagingType": "bootstrap_full_number",
        "language": {
            "search": "Search : ",
            "emptyTable": p_EmptyTableMsg,
            "lengthMenu": "  _MENU_ records",
            "paginate": {
                "previous": "Prev",
                "next": "Next",
                "last": "Last",
                "first": "First"
            }
        },
        "responsive": true,
        "columnDefs": p_columnDefs,
        "order": [] // set first column as a default sort by asc
    });

    return p_Table;
}

function jsfn_ShowLoading() {
    var messageElement = $('#domMessage')[0];
    $.blockUI({
        message: messageElement,
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
        window.location.href = '/Login/Individual';
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

var handleSelectPicker = function () {
    if (!$().selectpicker) {
        return;
    }
    $('.bs-select').attr('data-live-search', true);
    $('.bs-select').selectpicker();
};

var handleSumoSelect = function () {
    if (!$().SumoSelect) {
        return;
    }

    $('.sumo-select').SumoSelect({
        search: true,
        locale: ['Apply', 'Cancel', 'Select All'],
        okCancelInMulti: true,
        triggerChangeCombined: true,
        forceCustomRendering: true
    });
}

var handleiCheck = function () {
    if (!$().iCheck) {
        return;
    }

    $('.icheck').each(function () {
        var checkboxClass = $(this).attr('data-checkbox') ? $(this).attr('data-checkbox') : 'icheckbox_minimal-grey';
        var radioClass = $(this).attr('data-radio') ? $(this).attr('data-radio') : 'iradio_minimal-grey';

        if (checkboxClass.indexOf('_line') > -1 || radioClass.indexOf('_line') > -1) {
            $(this).iCheck({
                checkboxClass: checkboxClass,
                radioClass: radioClass,
                insert: '<div class="icheck_line-icon"></div>' + $(this).attr("data-label")
            });
        } else {
            $(this).iCheck({
                checkboxClass: checkboxClass,
                radioClass: radioClass
            });
        }
    });
};

var handleTooltips = function () {
    // global tooltips
    $('.tooltips').tooltip();

    // portlet tooltips
    $('.portlet > .portlet-title .fullscreen').tooltip({
        trigger: 'hover',
        container: 'body',
        title: 'Fullscreen'
    });
    $('.portlet > .portlet-title > .tools > .reload').tooltip({
        trigger: 'hover',
        container: 'body',
        title: 'Reload'
    });
    $('.portlet > .portlet-title > .tools > .remove').tooltip({
        trigger: 'hover',
        container: 'body',
        title: 'Remove'
    });
    $('.portlet > .portlet-title > .tools > .config').tooltip({
        trigger: 'hover',
        container: 'body',
        title: 'Settings'
    });
    $('.portlet > .portlet-title > .tools > .collapse, .portlet > .portlet-title > .tools > .expand').tooltip({
        trigger: 'hover',
        container: 'body',
        title: 'Collapse/Expand'
    });
};

// Handles portlet tools & actions
var handlePortletTools = function () {
    // handle portlet remove
    $('body').on('click', '.portlet > .portlet-title > .tools > a.remove', function (e) {
        e.preventDefault();
        var portlet = $(this).closest(".portlet");

        if ($('body').hasClass('page-portlet-fullscreen')) {
            $('body').removeClass('page-portlet-fullscreen');
        }

        portlet.find('.portlet-title .fullscreen').tooltip('destroy');
        portlet.find('.portlet-title > .tools > .reload').tooltip('destroy');
        portlet.find('.portlet-title > .tools > .remove').tooltip('destroy');
        portlet.find('.portlet-title > .tools > .config').tooltip('destroy');
        portlet.find('.portlet-title > .tools > .collapse, .portlet > .portlet-title > .tools > .expand').tooltip('destroy');

        portlet.remove();
    });

    // handle portlet fullscreen
    $('body').on('click', '.portlet > .portlet-title .fullscreen', function (e) {
        e.preventDefault();
        var portlet = $(this).closest(".portlet");
        if (portlet.hasClass('portlet-fullscreen')) {
            $(this).removeClass('on');
            portlet.removeClass('portlet-fullscreen');
            $('body').removeClass('page-portlet-fullscreen');
            portlet.children('.portlet-body').css('height', 'auto');
        } else {
            var height = App.getViewPort().height -
                portlet.children('.portlet-title').outerHeight() -
                parseInt(portlet.children('.portlet-body').css('padding-top')) -
                parseInt(portlet.children('.portlet-body').css('padding-bottom'));

            $(this).addClass('on');
            portlet.addClass('portlet-fullscreen');
            $('body').addClass('page-portlet-fullscreen');
            portlet.children('.portlet-body').css('height', height);
        }
    });

    $('body').on('click', '.portlet > .portlet-title > .tools > .collapse, .portlet .portlet-title > .tools > .expand', function (e) {
        e.preventDefault();
        var el = $(this).closest(".portlet").children(".portlet-body");
        if ($(this).hasClass("collapse")) {
            $(this).removeClass("collapse").addClass("expand");
            el.slideUp(200);
        } else {
            $(this).removeClass("expand").addClass("collapse");
            el.slideDown(200);
        }
    });
};

function jsfn_ShowAlert(pMessage, pAlertType, pFocus = true, pContainerId = '#dvAlertConatiner') {
    HandleCustomAlert({
        container: pContainerId, // alerts parent container(by default placed after the page breadcrumbs)
        place: 'append', // append or prepent in container 
        type: pAlertType,  // alert's type
        message: pMessage,  // alert's message
        close: true, // make alert closable
        reset: true, // close all previouse alerts first
        focus: pFocus, // auto scroll to the alert after shown
        closeInSeconds: '5', // auto close after defined seconds
        icon: '' // put icon before the message
    });
}

function GetMonthName(monthNumber) {
    var months = ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'];
    return months[monthNumber - 1];
}

var HandleCustomAlert = function (options) {

    options = $.extend(true, {
        container: "", // alerts parent container(by default placed after the page breadcrumbs)
        place: "append", // "append" or "prepend" in container 
        type: 'success', // alert's type
        message: "", // alert's message
        close: true, // make alert closable
        reset: true, // close all previouse alerts first
        focus: true, // auto scroll to the alert after shown
        closeInSeconds: 0, // auto close after defined seconds
        icon: "" // put icon before the message
    }, options);

    var id = getUniqueID("App_alert");

    var html = '<div id="' + id + '" class="custom-alerts alert alert-' + options.type + ' fade in">' + (options.close ? '<button type="button" class="close" data-dismiss="alert" aria-hidden="true"></button>' : '') + (options.icon !== "" ? '<i class="fa-lg fa fa-' + options.icon + '"></i>  ' : '') + options.message + '</div>';

    if (options.reset) {
        $('.custom-alerts').remove();
    }

    if (!options.container) {
        if ($('.page-fixed-main-content').size() === 1) {
            $('.page-fixed-main-content').prepend(html);
        } else if (($('body').hasClass("page-container-bg-solid") || $('body').hasClass("page-content-white")) && $('.page-head').size() === 0) {
            $('.page-title').after(html);
        } else {
            if ($('.page-bar').size() > 0) {
                $('.page-bar').after(html);
            } else {
                $('.page-breadcrumb, .breadcrumbs').after(html);
            }
        }
    } else {
        if (options.place == "append") {
            $(options.container).append(html);
        } else {
            $(options.container).prepend(html);
        }
    }

    if (options.focus) {
        scrollTo($('#' + id));
    }

    if (options.closeInSeconds > 0) {
        setTimeout(function () {
            $('#' + id).remove();
        }, options.closeInSeconds * 1000);
    }

    return id;
};

var getUniqueID = function (prefix) {
    return prefix + '_' + Math.floor(Math.random() * (new Date()).getTime());
}

var scrollTo = function (el, offeset) {
    var pos = (el && el.size() > 0) ? el.offset().top : 0;

    if (el) {
        if ($('body').hasClass('page-header-fixed')) {
            pos = pos - $('.page-header').height();
        } else if ($('body').hasClass('page-header-top-fixed')) {
            pos = pos - $('.page-header-top').height();
        } else if ($('body').hasClass('page-header-menu-fixed')) {
            pos = pos - $('.page-header-menu').height();
        }
        pos = pos + (offeset ? offeset : -1 * el.height());
    }

    $('html,body').animate({
        scrollTop: pos
    }, 'slow');
}

function jsfn_FormatJsonDate(pJsonDate) {
    var vStringDate, vYear, vMonth, vDay, vFinalDate;

    vStringDate = pJsonDate.replace(/\D/g, "");
    vDate = new Date(parseInt(vStringDate));

    vYear = vDate.getFullYear();
    vMonth = ('0' + (vDate.getMonth() + 1)).slice(-2);
    vDay = ('0' + vDate.getDate()).slice(-2);

    vFinalDate = vDay + '/' + vMonth + "/" + vYear;
    return vFinalDate;
}

function jsfn_SearchValidation(pFormID) {

    var formFieldText = $('input[type="text"]', '#' + pFormID).filter(function () {
        return $.trim(this.value).length;
    }).length;

    if (!formFieldText) {
        jsfn_ShowAlert("Please fill atleast one search criteria", 'danger');
        return false;
    }
    else {
        return true;
    }
}

function jsfn_CompanyInformation(CompID) {

    var vData = {
        CompanyID: CompID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetCompanyDetails', vData, jsfn_CompanyDetailResponse);
}

function jsfn_CompanyDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvCompanyModal').modal('show');
        $('#dvCompanyRIN').html(data.TaxPayerDetails.CompanyRIN);
        $('#dvCompanyName').html(data.TaxPayerDetails.CompanyName);
        $('#dvCTIN').html(data.TaxPayerDetails.TIN == null ? '-' : data.TaxPayerDetails.TIN);
        $('#dvCMobileNumber1').html(data.TaxPayerDetails.MobileNumber1);
        $('#dvCMobileNumber2').html(data.TaxPayerDetails.MobileNumber2 == null ? '-' : data.TaxPayerDetails.MobileNumber2);
        $('#dvCEmailAddress1').html(data.TaxPayerDetails.EmailAddress1 == null ? '-' : data.TaxPayerDetails.EmailAddress1);
        $('#dvCEmailAddress2').html(data.TaxPayerDetails.EmailAddress2 == null ? '-' : data.TaxPayerDetails.EmailAddress2);
        $('#dvCTaxOffice').html(data.TaxPayerDetails.TaxOfficeName == null ? '-' : data.TaxPayerDetails.TaxOfficeName);
        $('#dvCEconomicActivities').html(data.TaxPayerDetails.EconomicActivitiesName);
        $('#dvCStatus').html(data.TaxPayerDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_IndividualInformation(IndID) {

    var vData = {
        IndividualID: IndID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetIndividualDetails', vData, jsfn_IndividualDetailResponse);
}

function jsfn_IndividualDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvIndividualModal').modal('show');
        $('#dvIndividualRIN').html(data.TaxPayerDetails.IndividualRIN);
        $('#dvGender').html(data.TaxPayerDetails.GenderName);
        $('#dvTitle').html(data.TaxPayerDetails.TitleName);
        $('#dvFirstName').html(data.TaxPayerDetails.FirstName);
        $('#dvLastName').html(data.TaxPayerDetails.LastName);
        $('#dvMiddleName').html(data.TaxPayerDetails.MiddleName == null ? '-' : data.TaxPayerDetails.MiddleName);
        $('#dvDateofBirth').html(jsfn_FormatJsonDate(data.TaxPayerDetails.DOB));
        $('#dvITIN').html(data.TaxPayerDetails.TIN == null ? '-' : data.TaxPayerDetails.TIN);
        $('#dvIMobileNumber1').html(data.TaxPayerDetails.MobileNumber1);
        $('#dvIMobileNumber2').html(data.TaxPayerDetails.MobileNumber2 == null ? '-' : data.TaxPayerDetails.MobileNumber2);
        $('#dvIEmailAddress1').html(data.TaxPayerDetails.EmailAddress1 == null ? '-' : data.TaxPayerDetails.EmailAddress1);
        $('#dvIEmailAddress2').html(data.TaxPayerDetails.EmailAddress2 == null ? '-' : data.TaxPayerDetails.EmailAddress2);
        $('#dvBiometricDetails').html(data.TaxPayerDetails.BiometricDetails == null ? '-' : data.TaxPayerDetails.BiometricDetails);
        $('#dvITaxOffice').html(data.TaxPayerDetails.TaxOfficeName == null ? '-' : data.TaxPayerDetails.TaxOfficeName);
        $('#dvMaritalStatus').html(data.TaxPayerDetails.MaritalStatusName == null ? '-' : data.TaxPayerDetails.MaritalStatusName);
        $('#dvNationality').html(data.TaxPayerDetails.NationalityName == null ? '-' : data.TaxPayerDetails.NationalityName);
        $('#dvIEconomicActivities').html(data.TaxPayerDetails.EconomicActivitiesName);
        $('#dvIStatus').html(data.TaxPayerDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_GovernmentInformation(GovID) {

    var vData = {
        GovernmentID: GovID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetGovernmentDetails', vData, jsfn_GovernmentDetailResponse);
}

function jsfn_GovernmentDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvGovernmentModal').modal('show');
        $('#dvGovernmentRIN').html(data.TaxPayerDetails.GovernmentRIN);
        $('#dvGTIN').html(data.TaxPayerDetails.TIN == null ? '-' : data.TaxPayerDetails.TIN);
        $('#dvGovernmentName').html(data.TaxPayerDetails.GovernmentName);
        $('#dvGovernmentType').html(data.TaxPayerDetails.GovernmentTypeName);
        $('#dvGTaxOffice').html(data.TaxPayerDetails.TaxOfficeName == null ? '-' : data.TaxPayerDetails.TaxOfficeName);
        $('#dvGContactName').html(data.TaxPayerDetails.ContactName);
        $('#dvGContactEmail').html(data.TaxPayerDetails.ContactEmail);
        $('#dvGContactNumber').html(data.TaxPayerDetails.ContactNumber);
        $('#dvGStatus').html(data.TaxPayerDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_SpecialInformation(spcID) {

    var vData = {
        SpecialID: spcID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetSpecialDetails', vData, jsfn_SpecialDetailResponse);
}

function jsfn_SpecialDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvSpecialModal').modal('show');
        $('#dvSpecialRIN').html(data.TaxPayerDetails.SpecialRIN);
        $('#dvSTIN').html(data.TaxPayerDetails.TIN == null ? '-' : data.TaxPayerDetails.TIN);
        $('#dvSpecialName').html(data.TaxPayerDetails.SpecialTaxPayerName);
        $('#dvSTaxOffice').html(data.TaxPayerDetails.TaxOfficeName == null ? '-' : data.TaxPayerDetails.TaxOfficeName);
        $('#dvSContactName').html(data.TaxPayerDetails.ContactName);
        $('#dvSContactEmail').html(data.TaxPayerDetails.ContactEmail);
        $('#dvSContactNumber').html(data.TaxPayerDetails.ContactNumber);
        $('#dvDescription').html(data.TaxPayerDetails.Description);
        $('#dvSStatus').html(data.TaxPayerDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BuildingInformation(bldid) {

    var vData = {
        BuildingID: bldid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetBuildingDetails', vData, jsfn_BuildingDetailResponse);
}

function jsfn_BuildingDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvBuildingModal').modal('show');
        $('#dvBuildingRIN').html(data.AssetDetails.BuildingRIN);
        $('#dvBuildingTagNumber').html(data.AssetDetails.BuildingTagNumber);
        $('#dvBuildingName').html(data.AssetDetails.BuildingName == null ? "-" : data.AssetDetails.BuildingName);
        $('#dvBuildingNumber').html(data.AssetDetails.BuildingNumber);
        $('#dvBuildStreetName').html(data.AssetDetails.StreetName);
        $('#dvBuildOffStreetName').html(data.AssetDetails.OffStreetName == null ? "-" : data.AssetDetails.OffStreetName);
        $('#dvBuildTown').html(data.AssetDetails.TownName);
        $('#dvBuildLGA').html(data.AssetDetails.LGAName);
        $('#dvBuildWard').html(data.AssetDetails.WardName);
        $('#dvBuildingType').html(data.AssetDetails.BuildingTypeName);
        $('#dvBuildingCompletion').html(data.AssetDetails.BuildingCompletionName);
        $('#dvBuildingPurpose').html(data.AssetDetails.BuildingPurposeName);
        $('#dvBuildingFunction').html(data.AssetDetails.BuildingFunctionNames);
        $('#dvBuildingOwnership').html(data.AssetDetails.BuildingOwnershipName == null ? "-" : data.AssetDetails.BuildingOwnershipName);
        $('#dvBuildingOccupancy').html(data.AssetDetails.BuildingOccupancyName);
        $('#dvBuildingOccupancyType').html(data.AssetDetails.BuildingOccupancyTypeName);
        $('#dvBuildingLatitude').html(data.AssetDetails.Latitude == null ? "-" : data.AssetDetails.Latitude);
        $('#dvBuildingLongitude').html(data.AssetDetails.Longitude == null ? "-" : data.AssetDetails.Longitude);
        $('#dvBuildingStatus').html(data.AssetDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BusinessInformation(busid) {

    var vData = {
        BusinessID: busid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetBusinessDetails', vData, jsfn_BusinessDetailResponse);
}

function jsfn_BusinessDetailResponse(data) {
    console.log(data);
    jsfn_HideLoading();
    if (data.success) {
        $('#dvBusinessModal').modal('show');
        $('#dvBusinessRIN').html(data.AssetDetails.BusinessRIN);
        $('#dvBusinessName').html(data.AssetDetails.BusinessName);
        $('#dvBusinessType').html(data.AssetDetails.BusinessTypeName);
        $('#dvBusinessLGA').html(data.AssetDetails.LGAName);
        $('#dvBusinessCategory').html(data.AssetDetails.BusinessCategoryName);
        $('#dvBusinessSector').html(data.AssetDetails.BusinessSectorName);
        $('#dvBusinessSubSector').html(data.AssetDetails.BusinessSubSectorName);
        $('#dvBusinessStructure').html(data.AssetDetails.BusinessStructureName);
        $('#dvBusinessOperations').html(data.AssetDetails.BusinessOperationName);
        $('#dvContactName').html(data.AssetDetails.ContactName);
        $('#dvZoneName').html(data.AssetDetails.ZoneName);
        $('#dvTaxOfficeName').html(data.AssetDetails.TaxOfficeName);
        $('#dvBusinessNumber').html(data.AssetDetails.BusinessNumber);
        $('#dvBusinessAddress').html(data.AssetDetails.BusinessAddress);
        $('#dvBusinessStatus').html(data.AssetDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_LandInformation(lndid) {

    var vData = {
        LandID: lndid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetLandDetails', vData, jsfn_LandDetailResponse);
}

function jsfn_LandDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvLandModal').modal('show');
        $('#dvLandRIN').html(data.AssetDetails.LandRIN);
        $('#dvLandStreetName').html(data.AssetDetails.StreetName);
        $('#dvLandTown').html(data.AssetDetails.TownName);
        $('#dvLandLGA').html(data.AssetDetails.LGAName);
        $('#dvLandWard').html(data.AssetDetails.WardName);
        $('#dvLandSize_Length').html(data.AssetDetails.LandSize_Length == null ? "-" : data.AssetDetails.LandSize_Length);
        $('#dvLandSize_Width').html(data.AssetDetails.LandSize_Width == null ? "-" : data.AssetDetails.LandSize_Width);
        $('#dvC_OF_O_Ref').html(data.AssetDetails.C_OF_O_Ref == null ? "-" : data.AssetDetails.C_OF_O_Ref);
        $('#dvLandPurpose').html(data.AssetDetails.LandPurposeName);
        $('#dvLandOwnership').html(data.AssetDetails.LandOwnershipName == null ? "-" : data.AssetDetails.LandOwnershipName);
        $('#dvLandLatitude').html(data.AssetDetails.Latitude == null ? "-" : data.AssetDetails.Latitude);
        $('#dvLandLongitude').html(data.AssetDetails.Longitude == null ? "-" : data.AssetDetails.Longitude);
        $('#dvLandStatus').html(data.AssetDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_VehicleInformation(vchid) {

    var vData = {
        VehicleID: vchid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetVehicleDetails', vData, jsfn_VehicleDetailResponse);
}

function jsfn_VehicleDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvVehicleModal').modal('show');
        $('#dvVehicleRIN').html(data.AssetDetails.VehicleRIN);
        $('#dvVehicleRegNumber').html(data.AssetDetails.VehicleRegNumber);
        $('#dvVIN').html(data.AssetDetails.VIN == null ? '-' : data.AssetDetails.VIN);
        $('#dvVehicleType').html(data.AssetDetails.VehicleTypeName);
        $('#dvVehicleSubType').html(data.AssetDetails.VehicleSubTypeName);
        $('#dvVLGA').html(data.AssetDetails.LGAName);
        $('#dvVehiclePurpose').html(data.AssetDetails.VehiclePurposeName);
        $('#dvVehicleFunction').html(data.AssetDetails.VehicleFunctionName);
        $('#dvVehicleOwnership').html(data.AssetDetails.VehicleOwnershipName == null ? "-" : data.AssetDetails.VehicleOwnershipName);
        $('#dvVStatus').html(data.AssetDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowTaxPayerDetails(tpaid) {

    var vData = {
        TPAID: tpaid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetTaxPayerDetails', vData, jsfn_ShowTaxPayerDetailResponse);
}

function jsfn_ShowTaxPayerDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        if (data.TaxPayerTypeID == '1') {
            $('#dvIndividualModal').modal('show');
            $('#dvIndividualRIN').html(data.TaxPayerDetails.IndividualRIN);
            $('#dvGender').html(data.TaxPayerDetails.GenderName);
            $('#dvTitle').html(data.TaxPayerDetails.TitleName);
            $('#dvFirstName').html(data.TaxPayerDetails.FirstName);
            $('#dvLastName').html(data.TaxPayerDetails.LastName);
            $('#dvMiddleName').html(data.TaxPayerDetails.MiddleName == null ? '-' : data.TaxPayerDetails.MiddleName);
            $('#dvDateofBirth').html(jsfn_FormatJsonDate(data.TaxPayerDetails.DOB));
            $('#dvITIN').html(data.TaxPayerDetails.TIN == null ? '-' : data.TaxPayerDetails.TIN);
            $('#dvIMobileNumber1').html(data.TaxPayerDetails.MobileNumber1);
            $('#dvIMobileNumber2').html(data.TaxPayerDetails.MobileNumber2 == null ? '-' : data.TaxPayerDetails.MobileNumber2);
            $('#dvIEmailAddress1').html(data.TaxPayerDetails.EmailAddress1 == null ? '-' : data.TaxPayerDetails.EmailAddress1);
            $('#dvIEmailAddress2').html(data.TaxPayerDetails.EmailAddress2 == null ? '-' : data.TaxPayerDetails.EmailAddress2);
            $('#dvBiometricDetails').html(data.TaxPayerDetails.BiometricDetails == null ? '-' : data.TaxPayerDetails.BiometricDetails);
            $('#dvITaxOffice').html(data.TaxPayerDetails.TaxOfficeName == null ? '-' : data.TaxPayerDetails.TaxOfficeName);
            $('#dvMaritalStatus').html(data.TaxPayerDetails.MaritalStatusName == null ? '-' : data.TaxPayerDetails.MaritalStatusName);
            $('#dvNationality').html(data.TaxPayerDetails.NationalityName == null ? '-' : data.TaxPayerDetails.NationalityName);
            $('#dvIEconomicActivities').html(data.TaxPayerDetails.EconomicActivitiesName);
            $('#dvIStatus').html(data.TaxPayerDetails.ActiveText);
        }
        else if (data.TaxPayerTypeID == '2') {
            $('#dvCompanyModal').modal('show');
            $('#dvCompanyRIN').html(data.TaxPayerDetails.CompanyRIN);
            $('#dvCompanyName').html(data.TaxPayerDetails.CompanyName);
            $('#dvCTIN').html(data.TaxPayerDetails.TIN == null ? '-' : data.TaxPayerDetails.TIN);
            $('#dvCMobileNumber1').html(data.TaxPayerDetails.MobileNumber1);
            $('#dvCMobileNumber2').html(data.TaxPayerDetails.MobileNumber2 == null ? '-' : data.TaxPayerDetails.MobileNumber2);
            $('#dvCEmailAddress1').html(data.TaxPayerDetails.EmailAddress1 == null ? '-' : data.TaxPayerDetails.EmailAddress1);
            $('#dvCEmailAddress2').html(data.TaxPayerDetails.EmailAddress2 == null ? '-' : data.TaxPayerDetails.EmailAddress2);
            $('#dvCTaxOffice').html(data.TaxPayerDetails.TaxOfficeName == null ? '-' : data.TaxPayerDetails.TaxOfficeName);
            $('#dvCEconomicActivities').html(data.TaxPayerDetails.EconomicActivitiesName);
            $('#dvCStatus').html(data.TaxPayerDetails.ActiveText);
        }
        else if (data.TaxPayerTypeID == '4') {
            $('#dvGovernmentModal').modal('show');
            $('#dvGovernmentRIN').html(data.TaxPayerDetails.GovernmentRIN);
            $('#dvGTIN').html(data.TaxPayerDetails.TIN == null ? '-' : data.TaxPayerDetails.TIN);
            $('#dvGovernmentName').html(data.TaxPayerDetails.GovernmentName);
            $('#dvGovernmentType').html(data.TaxPayerDetails.GovernmentTypeName);
            $('#dvGTaxOffice').html(data.TaxPayerDetails.TaxOfficeName == null ? '-' : data.TaxPayerDetails.TaxOfficeName);
            $('#dvGContactName').html(data.TaxPayerDetails.ContactName);
            $('#dvGContactEmail').html(data.TaxPayerDetails.ContactEmail);
            $('#dvGContactNumber').html(data.TaxPayerDetails.ContactNumber);
            $('#dvGStatus').html(data.TaxPayerDetails.ActiveText);
        }
        else if (data.TaxPayerTypeID == '3') {
            $('#dvSpecialModal').modal('show');
            $('#dvSpecialRIN').html(data.TaxPayerDetails.SpecialRIN);
            $('#dvSpecialName').html(data.TaxPayerDetails.SpecialTaxPayerName);
            $('#dvSTIN').html(data.TaxPayerDetails.TIN == null ? '-' : data.TaxPayerDetails.TIN);
            $('#dvSTaxOffice').html(data.TaxPayerDetails.TaxOfficeName == null ? '-' : data.TaxPayerDetails.TaxOfficeName);
            $('#dvSContactName').html(data.TaxPayerDetails.ContactName);
            $('#dvSContactEmail').html(data.TaxPayerDetails.ContactEmail);
            $('#dvSContactNumber').html(data.TaxPayerDetails.ContactNumber);
            $('#dvDescription').html(data.TaxPayerDetails.Description);
            $('#dvSStatus').html(data.TaxPayerDetails.ActiveText);
        }
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowAssetDetails(tpaid) {

    var vData = {
        TPAID: tpaid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetAssetDetails', vData, jsfn_AssetDetailResponse);
}

function jsfn_AssetDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        if (data.AssetTypeID == '1') {
            $('#dvBuildingModal').modal('show');
            $('#dvBuildingRIN').html(data.AssetDetails.BuildingRIN);
            $('#dvBuildingTagNumber').html(data.AssetDetails.BuildingTagNumber);
            $('#dvBuildingName').html(data.AssetDetails.BuildingName == null ? "-" : data.AssetDetails.BuildingName);
            $('#dvBuildingNumber').html(data.AssetDetails.BuildingNumber);
            $('#dvBuildStreetName').html(data.AssetDetails.StreetName);
            $('#dvBuildOffStreetName').html(data.AssetDetails.OffStreetName == null ? "-" : data.AssetDetails.OffStreetName);
            $('#dvBuildTown').html(data.AssetDetails.TownName);
            $('#dvBuildLGA').html(data.AssetDetails.LGAName);
            $('#dvBuildWard').html(data.AssetDetails.WardName);
            $('#dvBuildingType').html(data.AssetDetails.BuildingTypeName);
            $('#dvBuildingCompletion').html(data.AssetDetails.BuildingCompletionName);
            $('#dvBuildingPurpose').html(data.AssetDetails.BuildingPurposeName);
            $('#dvBuildingFunction').html(data.AssetDetails.BuildingFunctionNames);
            $('#dvBuildingOwnership').html(data.AssetDetails.BuildingOwnershipName == null ? "-" : data.AssetDetails.BuildingOwnershipName);
            $('#dvBuildingOccupancy').html(data.AssetDetails.BuildingOccupancyName);
            $('#dvBuildingOccupancyType').html(data.AssetDetails.BuildingOccupancyTypeName);
            $('#dvBuildingLatitude').html(data.AssetDetails.Latitude == null ? "-" : data.AssetDetails.Latitude);
            $('#dvBuildingLongitude').html(data.AssetDetails.Longitude == null ? "-" : data.AssetDetails.Longitude);
            $('#dvBuildingStatus').html(data.AssetDetails.ActiveText);
        }
        else if (data.AssetTypeID == '2') {
            $('#dvVehicleModal').modal('show');
            $('#dvVehicleRIN').html(data.AssetDetails.VehicleRIN);
            $('#dvVehicleRegNumber').html(data.AssetDetails.VehicleRegNumber);
            $('#dvVIN').html(data.AssetDetails.VIN == null ? '-' : data.AssetDetails.VIN);
            $('#dvVehicleType').html(data.AssetDetails.VehicleTypeName);
            $('#dvVehicleSubType').html(data.AssetDetails.VehicleSubTypeName);
            $('#dvVLGA').html(data.AssetDetails.LGAName);
            $('#dvVehiclePurpose').html(data.AssetDetails.VehiclePurposeName);
            $('#dvVehicleFunction').html(data.AssetDetails.VehicleFunctionName);
            $('#dvVehicleOwnership').html(data.AssetDetails.VehicleOwnershipName == null ? "-" : data.AssetDetails.VehicleOwnershipName);
            $('#dvVStatus').html(data.AssetDetails.ActiveText);
        }
        else if (data.AssetTypeID == '3') {
            $('#dvBusinessModal').modal('show');
            $('#dvBusinessRIN').html(data.AssetDetails.BusinessRIN);
            $('#dvBusinessName').html(data.AssetDetails.BusinessName);
            $('#dvBusinessType').html(data.AssetDetails.BusinessTypeName);
            $('#dvBusinessLGA').html(data.AssetDetails.LGAName);
            $('#dvBusinessCategory').html(data.AssetDetails.BusinessCategoryName);
            $('#dvBusinessSector').html(data.AssetDetails.BusinessSectorName);
            $('#dvBusinessSubSector').html(data.AssetDetails.BusinessSubSectorName);
            $('#dvBusinessStructure').html(data.AssetDetails.BusinessStructureName);
            $('#dvBusinessOperations').html(data.AssetDetails.BusinessOperationName);
            $('#dvContactName').html(data.AssetDetails.ContactName);
            $('#dvBusinessNumber').html(data.AssetDetails.BusinessNumber);
            $('#dvBusinessAddress').html(data.AssetDetails.BusinessAddress);
            $('#dvBusinessStatus').html(data.AssetDetails.ActiveText);
        }
        else if (data.AssetTypeID == '4') {
            $('#dvLandModal').modal('show');
            $('#dvLandRIN').html(data.AssetDetails.LandRIN);
            $('#dvLandStreetName').html(data.AssetDetails.StreetName);
            $('#dvLandTown').html(data.AssetDetails.TownName);
            $('#dvLandLGA').html(data.AssetDetails.LGAName);
            $('#dvLandWard').html(data.AssetDetails.WardName);
            $('#dvLandSize_Length').html(data.AssetDetails.LandSize_Length == null ? "-" : data.AssetDetails.LandSize_Length);
            $('#dvLandSize_Width').html(data.AssetDetails.LandSize_Width == null ? "-" : data.AssetDetails.LandSize_Width);
            $('#dvC_OF_O_Ref').html(data.AssetDetails.C_OF_O_Ref == null ? "-" : data.AssetDetails.C_OF_O_Ref);
            $('#dvLandPurpose').html(data.AssetDetails.LandPurposeName);
            $('#dvLandOwnership').html(data.AssetDetails.LandOwnershipName == null ? "-" : data.AssetDetails.LandOwnershipName);
            $('#dvLandLatitude').html(data.AssetDetails.Latitude == null ? "-" : data.AssetDetails.Latitude);
            $('#dvLandLongitude').html(data.AssetDetails.Longitude == null ? "-" : data.AssetDetails.Longitude);
            $('#dvLandStatus').html(data.AssetDetails.ActiveText);
        }
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ProfileDetails(prfID) {
    var vData = {
        ProfileID: prfID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetProfileDetails', vData, jsfn_ShowProfileDetailResponse);
}

function jsfn_ShowProfileDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvProfileModal').modal('show');
        $('#dvProfileRefNo').html(data.ProfileDetails.ProfileReferenceNo);
        $('#dvAssetType').html(data.ProfileDetails.AssetTypeName);
        $('#dvTaxPayerType').html(data.ProfileDetails.TaxPayerTypeNames);
        $('#dvTaxPayerRole').html(data.ProfileDetails.TaxPayerRoleNames);
        $('#dvProfileDescription').html(data.ProfileDetails.ProfileDescription);
        $('#dvAssetStatus').html(data.ProfileDetails.AssetTypeStatusName);
        $('#dvProfileStatus').html(data.ProfileDetails.ActiveText);
        $('#dvProfileSector').html(data.ProfileDetails.ProfileSectorNames);
        $('#dvProfileSubSector').html(data.ProfileDetails.ProfileSubSectorNames);
        $('#dvProfileGroup').html(data.ProfileDetails.ProfileGroupNames);
        $('#dvProfileSubGroup').html(data.ProfileDetails.ProfileSubGroupNames);
        $('#dvSectorElement').html(data.ProfileDetails.ProfileSectorElementNames);
        $('#dvSectorSubElement').html(data.ProfileDetails.ProfileSectorSubElementNames);
        $('#dvProfileAttribute').html(data.ProfileDetails.ProfileAttributeNames);
        $('#dvProfileSubAttribute').html(data.ProfileDetails.ProfileSubAttributeNames);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_AssessmentRuleDetails(aruleID) {
    var vData = {
        AssessmentRuleID: aruleID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetAssessmentRuleDetails', vData, jsfn_ShowAssessmentRuleDetailResponse);
}

function jsfn_ShowAssessmentRuleDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentRuleModal').modal('show');
        $('#dvAssessmentRuleRefNo').html(data.AssessmentRuleDetails.AssessmentRuleCode);
        $('#dvAssessmentRuleName').html(data.AssessmentRuleDetails.AssessmentRuleName);
        $('#dvRuleRun').html(data.AssessmentRuleDetails.RuleRunName);
        $('#dvFrequency').html(data.AssessmentRuleDetails.PaymentFrequencyName);
        $('#dvTaxYear').html(data.AssessmentRuleDetails.TaxYear);
        $('#dvSettlementMethod').html(data.AssessmentRuleDetails.SettlementMethodNames);
        $('#dvPaymentOption').html(data.AssessmentRuleDetails.PaymentOptionName);
        $('#dvStatus').html(data.AssessmentRuleDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_CheckRINExist() {
    var vMasterSearchText = $("#txtMasterSearch").val();

    if ($.trim(vMasterSearchText).length === 0) {
        jsfn_ShowAlert("Please enter RIN before clicking search", 'danger', false);
        return false;
    }
    else {
        var vData = {
            RIN: vMasterSearchText
        };

        jsfn_ajaxPost('/Base/SearchRIN', vData, jsfn_CheckRINExistResponse);
    }
}

function jsfn_CheckRINExistResponse(data) {
    if (data.success) {
        window.location.href = data.RedirectUrl;
    }
    else {
        jsfn_ShowAlert(data.Messsage, 'danger', false);
    }
}

