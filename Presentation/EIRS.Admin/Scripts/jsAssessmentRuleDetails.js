var vProfileDataTable;
$(document).ready(function () {
    $("#fsStep2").hide();
    $("#hdStep2").hide();
    $("#btnSave").hide();
    $("#btnBack").hide();

    $("#dvAssessmentRuleProfileList").hide();
    $("#dvAssessmentRuleItemList").hide();
    $("#dvARAddProfile").hide();
    $("#dvARAddAssessmentItem").hide();

    jsfn_BuildProfileDataTable();
    jsfn_BuildAssessmentItemDataTable();

    if ($("#hdnAssessmentRuleID").val() != '-1') {

        $('#btnSave').click(function () {
            if (ValidatedForm()) {
                $('#dvUpdateDialog').dialog('open');
            }
            return false;

        });
    }
    else {
        $('#btnSave').click(function () {
            if (ValidatedForm()) {
                $("#frmMainAssessment").submit();
            }

            return false;
        });
    }

    $('#dvUpdateDialog').removeClass('hide');
    $('#dvUpdateDialog').dialog({
        autoOpen: false,
        width: 600,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Edit Record Confirmation</h4></div>",
        buttons: [{
            html: "<i class='fa fa-check'></i>&nbsp; Yes",
            "class": "btn btn-primary",
            click: function () {
                $("#frmMainAssessment").submit();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    var frmMainAssessment = $('#frmMainAssessment');

    // init validator obj and set the rules
    frmMainAssessment.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-inline', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        rules: {
            // the rules, as usual
        },

        highlight: function (element) { // hightlight error inputs
            $(element).closest('.control-group').addClass('error'); // set error class to the control group
        },

        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.control-group').removeClass('error'); // set error class to the control group
        }
    });

    $("#btnNext").on('click', function () {
        if ($('#fsStep1 :input').valid()) {
            $("#fsStep2").show();
            $("#fsStep1").hide();
            $("#btnSave").show();
            $("#btnBack").show();
            $("#btnNext").hide();
            $("#btnMainBack").hide();
        }
    });

    $("#btnBack").on('click', function () {
        $("#fsStep2").hide();
        $("#fsStep1").show();
        $("#btnSave").hide();
        $("#btnBack").hide();
        $("#btnNext").show();
        $("#btnMainBack").show();
    });

    $("#aAssessmentRuleProfile").click(function () {
        $("#frmMainAssessment").hide();
        $("#dvAssessmentRuleProfileList").show();
    });

    $("#aAssessmentRuleItem").click(function () {
        $("#frmMainAssessment").hide();
        $("#dvAssessmentRuleItemList").show();
    });

    $("#btnARPBack,#btnARIBack").click(function () {
        $("#frmMainAssessment").show();
        $("#dvAssessmentRuleProfileList").hide();
        $("#dvAssessmentRuleItemList").hide();
    });

    $("#btnBackAssessmentItems").click(function () {
        $("#artAssessmentRule").addClass("col-lg-6");
        $("#dvARAddAssessmentItem").hide();
        $("#dvAssessmentRuleItemList").show();
    });

    $("#btnBackProfile").click(function () {
        $("#artAssessmentRule").addClass("col-lg-6");
        $("#dvARAddProfile").hide();
        $("#dvAssessmentRuleProfileList").show();
    });

    $("#btnARPAdd").click(function () {
        $("#artAssessmentRule").removeClass("col-lg-6");
        $("#dvARAddProfile").show();
        $("#dvAssessmentRuleProfileList").hide();
    });

    $("#btnARIAdd").click(function () {
        $("#artAssessmentRule").removeClass("col-lg-6");
        $("#dvARAddAssessmentItem").show();
        $("#dvAssessmentRuleItemList").hide();
    });

    $("#btnAddAssessmentItems").click(function () {
        if ($("#hdnAssessmentItemIds").val() != "") {
            var vData = {
                AssessmentItemIds: $("#hdnAssessmentItemIds").val(),
            };

            jsfn_ShowLoading();
            jsfn_ajaxPost('/AssessmentRule/AddAssessmentRuleItem', vData, jsfn_AddAssessmentRuleItemResponse);

        } else {
            jsfn_ShowAlert('Please Select Assessment Items', 'info');
        }
    });

    var vSettlementMethodData = new Array();
    var vcboSettlementMethodValue = $('#cboSettlementMethod').attr('data-initvalue');
    if (vcboSettlementMethodValue != '')
        vSettlementMethodData = vcboSettlementMethodValue.split(',');

    $('#cboSettlementMethod').val(vSettlementMethodData).select2({ "width": "100%" });


    $('#dvProfileDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Profile Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#dvAssessmentItemDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Assessment Item Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });
});

function ValidatedForm() {
    var vProfileCount = $('#spnARPRecordCount').html();
    var vItemsCount = $('#spnARIRecordCount').html();
    var isValid = false;

    if (parseInt(vProfileCount) == 0 && parseInt(vItemsCount) == 0) {
        isValid = false;
        $("#ddliProfile").addClass('state-error');
        $("#AssessmentRuleProfile-error").removeClass('hide');
        $("#ddliAssessmentItem").addClass('state-error');
        $("#AssessmentRuleItem-error").removeClass('hide');
    }
    else if (parseInt(vProfileCount) == 0) {
        $("#ddliProfile").addClass('state-error');
        $("#AssessmentRuleProfile-error").removeClass('hide');
        isValid = false;

    }
    else if (parseInt(vItemsCount) == 0) {
        $("#ddliAssessmentItem").addClass('state-error');
        $("#AssessmentRuleItem-error").removeClass('hide');
        isValid = false;
    }
    else {
        $("#ddliProfile").removeClass('state-error');
        $("#AssessmentRuleProfile-error").addClass('hide');
        $("#ddliAssessmentItem").removeClass('state-error');
        $("#AssessmentRuleItem-error").addClass('hide');
        isValid = true;
    }

    return isValid;
}

function jsfn_AddAssessmentRuleProfile(prfid, prfrefno, assettypename) {
    var vData = {
        ProfileID: prfid,
        ProfileReferenceNo: prfrefno,
        AssetTypeName: assettypename,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/AssessmentRule/AddAssessmentRuleProfile', vData, jsfn_AddAssessmentRuleProfileResponse);
}

function jsfn_AddAssessmentRuleProfileResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        $('#tbodyAssessmentRuleProfileList').html(data.AssessmentRuleProfileList);
        $('#spnARPRecordCount').html(data.AssessmentRuleProfileCount);
        $("#dvARAddProfile").hide();
        $("#dvAssessmentRuleProfileList").show();
        $("#artAssessmentRule").addClass("col-lg-6");
        $("#ddliProfile").removeClass('state-error');
        $("#AssessmentRuleProfile-error").addClass('hide');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_RemoveProfileDetails(rowid) {
    var vData = {
        RowID: rowid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/AssessmentRule/RemoveAssessmentRuleProfile', vData, jsfn_AddAssessmentRuleProfileResponse)
}

function jsfn_SelectAssessmentItem(chkAssessmentItem) {

    var vArrAssessmentItem = $('#hdnAssessmentItemIds').val().split(',');
    if (chkAssessmentItem.checked) {
        if ($('#hdnAssessmentItemIds').val() != "")
            vArrAssessmentItem.push(chkAssessmentItem.dataset["assessmentitemid"]);
        else {
            vArrAssessmentItem = new Array();
            vArrAssessmentItem.push(chkAssessmentItem.dataset["assessmentitemid"]);
        }
    }
    else {
        vArrAssessmentItem.splice($.inArray(chkAssessmentItem.dataset["assessmentitemid"], vArrAssessmentItem), 1);
    }

    $('#hdnAssessmentItemIds').val(vArrAssessmentItem.toString());
}

function jsfn_AddAssessmentRuleItemResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        $('#tbodyAssessmentRuleItemList').html(data.AssessmentRuleItemList);
        $('#spnARIRecordCount').html(data.AssessmentRuleItemCount);
        $("#dvARAddAssessmentItem").hide();
        $("#dvAssessmentRuleItemList").show();
        $("#artAssessmentRule").addClass("col-lg-6");
        $("#ddliAssessmentItem").removeClass('state-error');
        $("#AssessmentRuleItem-error").addClass('hide');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_RemoveAssessmentItem(rowid) {
    var vData = {
        RowID: rowid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/AssessmentRule/RemoveAssessmentRuleItem', vData, jsfn_AddAssessmentRuleItemResponse)
}



function jsfn_ShowProfileDetails(ProfileID) {

    var vData = {
        ProfileID: ProfileID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/AssessmentRule/GetProfileDetails', vData, jsfn_ShowProfileDetailResponse)
}

function jsfn_ShowProfileDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvProfileDetails').dialog('open');
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

function jsfn_ShowAssessmentItemDetails(AssessmentItemID) {

    var vData = {
        AssessmentItemID: AssessmentItemID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/AssessmentRule/GetAssessmentItemDetails', vData, jsfn_ShowAssessmentItemDetailResponse)
}

function jsfn_ShowAssessmentItemDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentItemDetails').dialog('open');
        $('#dvAssessmentItemRefNo').html(data.AssessmentItemDetails.AssessmentItemReferenceNo);
        $('#dvAIAssetType').html(data.AssessmentItemDetails.AssetTypeName);
        $('#dvAssessmentGroup').html(data.AssessmentItemDetails.AssessmentGroupName);
        $('#dvAssessmentSubGroup').html(data.AssessmentItemDetails.AssessmentSubGroupName);
        $('#dvRevenueStream').html(data.AssessmentItemDetails.RevenueStreamName);
        $('#dvRevenueSubStream').html(data.AssessmentItemDetails.RevenueSubStreamName);
        $('#dvAssessmentItemCategory').html(data.AssessmentItemDetails.AssessmentItemCategoryName);
        $('#dvAssessmentItemSubCategory').html(data.AssessmentItemDetails.AssessmentItemSubCategoryName);
        $('#dvRevenueAgency').html(data.AssessmentItemDetails.AgencyName);
        $('#dvAssessmentItemName').html(data.AssessmentItemDetails.AssessmentItemName);
        $('#dvComputation').html(data.AssessmentItemDetails.ComputationName);
        $('#dvTaxBaseAmount').html(data.AssessmentItemDetails.TaxBaseAmount);
        $('#dvPercentage').html(data.AssessmentItemDetails.Percentage);
        $('#dvTaxAmount').html(data.AssessmentItemDetails.TaxAmount);

        if (data.AssessmentItemDetails.ComputationID == 1) {
            $('#dvCPercentage').hide();
        }
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BuildProfileDataTable() {
    vProfileDataTable = $("#tbProfile").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Profile/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.ProfileReferenceNo = $("#txtProfileReferenceNo").val();
                data.AssetTypeName = $("#txtAssetType").val();
                data.ProfileSectorName = $("#txtProfileSector").val();
                data.ProfileSubSectorName = $("#txtProfileSubSector").val();
                data.ProfileGroupName = $("#txtProfileGroup").val();
                data.ProfileSubGroupName = $("#txtProfileSubGroup").val();
                data.ProfileSectorElementName = $("#txtProfileSectorElement").val();
                data.ProfileSectorSubElementName = $("#txtProfileSectorSubElement").val();
                data.ProfileAttributeName = $("#txtProfileAttribute").val();
                data.ProfileSubAttributeName = $("#txtProfileSubAttribute").val();
                data.TaxPayerTypeName = $("#txtTaxPayerType").val();
                data.TaxPayerRoleName = $("#txtTaxPayerRole").val();
                data.ProfileDescription = $("#txtProfileDescription").val();
                data.ActiveText = $("#txtStatus").val();
            },
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",
        "columns": [
            { "data": "ProfileReferenceNo", "orderable": true, "name": "ProfileReferenceNo" },
            { "data": "AssetTypeName", "orderable": true, "name": "AssetTypeName" },
            { "data": "ProfileSectorNames", "orderable": true, "name": "ProfileSectorNames" },
            { "data": "ProfileSubSectorNames", "orderable": true, "name": "ProfileSubSectorNames" },
            { "data": "ProfileGroupNames", "orderable": true, "name": "ProfileGroupNames" },
            { "data": "ProfileSubGroupNames", "orderable": true, "name": "ProfileSubGroupNames" },
            { "data": "ProfileSectorElementNames", "orderable": true, "name": "ProfileSectorElementNames" },
            { "data": "ProfileSectorSubElementNames", "orderable": true, "name": "ProfileSectorSubElementNames" },
            { "data": "ProfileAttributeNames", "orderable": true, "name": "ProfileAttributeNames" },
            { "data": "ProfileSubAttributeNames", "orderable": true, "name": "ProfileSubAttributeNames" },
            { "data": "TaxPayerTypeNames", "orderable": true, "name": "TaxPayerTypeNames" },
            { "data": "TaxPayerRoleNames", "orderable": true, "name": "TaxPayerRoleNames" },
            { "data": "ProfileDescription", "orderable": true, "name": "ProfileDescription" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, pro) {
                    return '<a onclick="javascript:jsfn_AddAssessmentRuleProfile(' + pro.ProfileID + ',\'' + pro.ProfileReferenceNo + '\',\'' + pro.AssetTypeName + '\')"> Select Profile </a>'
                }
            }
        ],

        "order": [[1, "asc"]]
    });

    $("#tbProfile thead th input[type=text]").on('keyup change', function () {
        vProfileDataTable
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });
    //Hide Columns
    vProfileDataTable.column(2).visible(false);
    vProfileDataTable.column(3).visible(false);
    vProfileDataTable.column(4).visible(false);
    vProfileDataTable.column(5).visible(false);
    vProfileDataTable.column(6).visible(false);
    vProfileDataTable.column(7).visible(false);
    vProfileDataTable.column(8).visible(false);
    vProfileDataTable.column(9).visible(false);
    vProfileDataTable.column(11).visible(false);
}

function jsfn_BuildAssessmentItemDataTable() {
    vAssessmentItemDataTable = $("#tbAssessmentItem").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/AssessmentItem/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.AssessmentItemReferenceNo = $("#txtAssessmentItemRefNo").val();
                data.AssetTypeName = $("#txtAssetTypeName").val();
                data.AssessmentGroupName = $("#txtAssessmentGroupName").val();
                data.AssessmentSubGroupName = $("#txtAssessmentSubGroupName").val();
                data.RevenueStreamName = $("#txtRevenueStreamName").val();
                data.RevenueSubStreamName = $("#txtRevenueSubStreamName").val();
                data.AssessmentItemCategoryName = $("#txtItemCategory").val();
                data.AssessmentItemSubCategoryName = $("#txtItemSubCategory").val();
                data.AgencyName = $("#txtAgencyName").val();
                data.AssessmentItemName = $("#txtAssessmentItemName").val();
                data.ComputationName = $("#txtComputationName").val();
                data.TaxBaseAmount = $("#txtTaxBaseAmount").val();
                data.Percentage = $("#txtPercentage").val();
                data.TaxAmount = $("#txtTaxAmount").val();
                data.ActiveText = $("#txtStatus").val();
            },
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "AssessmentItemReferenceNo", "orderable": true, "name": "AssessmentItemReferenceNo" },
            { "data": "AssetTypeName", "orderable": true, "name": "AssetTypeName" },
            { "data": "AssessmentGroupName", "orderable": true, "name": "AssessmentGroupName" },
            { "data": "AssessmentSubGroupName", "orderable": true, "name": "AssessmentSubGroupName" },
            { "data": "RevenueStreamName", "orderable": true, "name": "RevenueStreamName" },
            { "data": "RevenueSubStreamName", "orderable": true, "name": "RevenueSubStreamName" },
            { "data": "AssessmentItemCategoryName", "orderable": true, "name": "AssessmentItemCategoryName" },
            { "data": "AssessmentItemSubCategoryName", "orderable": true, "name": "AssessmentItemSubCategoryName" },
            { "data": "AgencyName", "orderable": true, "name": "AgencyName" },
            { "data": "AssessmentItemName", "orderable": true, "name": "AssessmentItemName" },
            { "data": "ComputationName", "orderable": true, "name": "ComputationName" },
            {
                "data": "TaxBaseAmount", "orderable": true, "name": "TaxBaseAmount", "render": function (data, type, asitem) {
                    return asitem.TaxBaseAmount.formatMoney();
                }
            },
            {
                "data": "Percentage", "orderable": true, "name": "Percentage"
            },
            {
                "data": "TaxAmount", "orderable": true, "name": "TaxAmount", "render": function (data, type, asitem) {
                    return asitem.TaxAmount.formatMoney();
                }
            },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, asitem) {
                    return '<input type="checkbox" name="chkAssessmentItemChecked_' + asitem.AssessmentItemID + '" onchange="jsfn_SelectAssessmentItem(this)" data-assessmentitemid="' + asitem.AssessmentItemID + '" title = "Tick To Select">';
                }
            },
        ],

        "order": [[1, "asc"]]
    });

    $("#tbAssessmentItem thead th input[type=text]").on('change', function () {
        vAssessmentItemDataTable
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });

    //Hide Columns
    vAssessmentItemDataTable.column(2).visible(false);
    vAssessmentItemDataTable.column(3).visible(false);
    vAssessmentItemDataTable.column(4).visible(false);
    vAssessmentItemDataTable.column(5).visible(false);
    vAssessmentItemDataTable.column(6).visible(false);
    vAssessmentItemDataTable.column(7).visible(false);
    vAssessmentItemDataTable.column(8).visible(false);
    vAssessmentItemDataTable.column(12).visible(false);
    vAssessmentItemDataTable.column(14).visible(false);
    //vAssessmentItemDataTable.column(15).visible(false);
}
