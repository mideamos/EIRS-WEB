var vCertificateItemTable;
$(document).ready(function () {
    $("#cboFieldType").change(jsfn_OnFieldTypeChange);

    var frmCustomField = $("#frmCustomField");
    frmCustomField.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-block', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        highlight: function (e) {
            $(e).closest('.form-group').removeClass('has-success has-error').addClass('has-error');
            $(e).closest('.help-block').remove();
        },
        success: function (e) {
            e.closest('.form-group').removeClass('has-success has-error');
            e.closest('.help-block').remove();
        },
        rules: {
            'txtFieldName': {
                required: true
            },
            'cboFieldType': {
                required: true
            },
            'txtCombovalue': {
                required: function (element) {
                    if ($("#cboFieldType").val() === '3') {
                        return true;
                    }
                    else {
                        return false;
                    }
                }
            }
        },
        messages: {
            'txtFieldName': {
                required: 'Please enter Field Name'
            },
            'cboFieldType': {
                required: 'Please select Field Type'
            },
            'txtCombovalue': {
                required: 'Please enter Combo Field value'
            }
        },

        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.control-group').removeClass('error'); // set error class to the control group
        }
    });

    $("#btnSaveCustomField").click(function (e) {
        e.preventDefault();
        if ($("#frmCustomField").valid()) {
            var vData = {
                FieldName: $("#txtFieldName").val(),
                FieldTypeID: $("#cboFieldType").val(),
                FieldTypeName: $("#cboFieldType option:selected").html(),
                IsRequired: $('#chkAttribute').iCheck('update')[0].checked,
                FieldComboValue: $("#txtCombovalue").val()
            };

            jsfn_ajaxPost('/CertificateType/AddCustomField', vData, jsfn_AddCustomFieldResponse);
        }
    });

    jsfn_BindCustomFieldTable();
    jsfn_BindCertificateItemTable();

    var frmCertificateItem = $("#frmCertificateItem");
    frmCertificateItem.validate({
        errorElement: 'span', //default input error message container
        errorClass: 'help-block', // default input error message class
        focusInvalid: false, // do not focus the last invalid input
        ignore: "",
        highlight: function (e) {
            $(e).closest('.form-group').removeClass('has-success has-error').addClass('has-error');
            $(e).closest('.help-block').remove();
        },
        success: function (e) {
            e.closest('.form-group').removeClass('has-success has-error');
            e.closest('.help-block').remove();
        },
        rules: {
            'cboCertificateItemType': {
                required: true
            },
            'txtCertificateItem': {
                required: true
            },
        },
        messages: {
            'cboCertificateItemType': {
                required: 'Please select certificate item type'
            },
            'txtCertificateItem': {
                required: 'Please enter certificate item'
            }
        },
        unhighlight: function (element) { // revert the change dony by hightlight
            $(element)
                .closest('.control-group').removeClass('error'); // set error class to the control group
        }
    });

    $("#btnSaveCertificateItem").click(function (e) {
        e.preventDefault();
        if ($("#frmCertificateItem").valid()) {
            var vData = {
                tyear: $("#cboTaxYear").val(),
                itemtypeid: $("#cboCertificateItemType").val(),
                itemname: $("#txtCertificateItem").val()
            };

            jsfn_ajaxPost('/CertificateType/AddCertificateItem', vData, jsfn_AddCertificateItemResponse);
        }
    });

    $('#btnSaveCertificateType').click(function () {
        $("#frmCertificateType").submit();
    });
});

function jsfn_OnFieldTypeChange() {
    var vFormselectValue = $("#cboFieldType").val();
    if (vFormselectValue === '3') {
        $('#dvComboValue').show();
    }
    else {
        $('#dvComboValue').hide();
    }
}

function jsfn_AddCustomFieldResponse(data) {
    if (data.Success === true) {
        $("#txtFieldName").val('');
        $("#txtCombovalue").val('');
        $('#dvComboValue').hide();
        $("#cboFieldType").val('').selectpicker('refresh');
        $("#chkAttribute").iCheck('uncheck');
        $("#dvCustomFieldData").html(data.CustomFieldTable);
        jsfn_BindCustomFieldTable();
        $("#dvCustomFieldModal").modal('hide');
    }
    else {
        jsfn_ShowAlert(data.Message, "danger");
    }
}

function jsfn_AddCertificateItemResponse(data) {
    if (data.Success === true) {
        $("#txtCertificateItem").val('');
        $("#cboCertificateItemType").val('').selectpicker('refresh');
        $("#dvCertificateItemModal").modal('hide');
        vCertificateItemTable.draw();
    }
    else {
        jsfn_ShowAlert(data.Message, "danger");
    }
}

function jsfn_BindCustomFieldTable() {
    var vColumnList =
        [
            { "orderable": true },
            { "orderable": true },
            { "orderable": true },
            { "orderable": false }
        ];
    var vSortOrder = [];
    jsfn_ConvertToDatableWithCustomSort($('#tblCustomField'), vColumnList, 'No Fields Found', vSortOrder);
}

function jsfn_AddCustomField() {
    $("#dvCustomFieldModal").modal('show');
}

function jsfn_RemoveCustomField(rowId) {
    var vData = {
        RowID: rowId
    };

    jsfn_ajaxPost('/CertificateType/RemoveCustomField', vData, jsfn_RemoveCustomFieldResponse);
}

function jsfn_RemoveCustomFieldResponse(data) {
    if (data.Success === true) {
        $("#dvCustomFieldData").html(data.CustomFieldTable);
        jsfn_BindFieldTable();
    }
    else {
        jsfn_ShowAlert(data.Message, "danger");
    }
}

function jsfn_AddCertificateItem() {
    $("#dvCertificateItemModal").modal('show');
}

function jsfn_BindCertificateItemTable() {
    vCertificateItemTable = $("#tblCertificateItem").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/CertificateType/LoadCertificateItem",
            "type": "POST",
            "datatype": "json",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            {
                "data": "ItemTypeName", "orderable": true, "name": "ItemTypeName", "width": "25%" },
            { "data": "ItemName", "orderable": true, "name": "ItemName", "width": "50%" },
            {
                "data": "", "orderable": false, "name": "Action", "width": "25%","render": function (data, type, item) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a onclick="jsfn_RemoveCertificateItem(' + item.RowID + ')">'
                        + 'Remove</a></li></ul></div>';
                }, "width": "10%"
            },
        ],
        "order": [[0, "asc"]]
    });
}

function jsfn_RemoveCertificateItem(rowId) {
    var vData = {
        RowID: rowId
    };

    jsfn_ajaxPost('/CertificateType/RemoveCertificateItem', vData, jsfn_RemoveCertificateItemResponse);
}

function jsfn_RemoveCertificateItemResponse(data) {
    if (data.Success === true) {
        vCertificateItemTable.draw();
    }
    else {
        jsfn_ShowAlert(data.Message, "danger");
    }
}