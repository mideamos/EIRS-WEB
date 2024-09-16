jQuery(document).ready(function () {
    jsfn_bindTable();
});

function jsfn_bindTable() {
    var vColumnsList = [{
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": false
    }];

    var vSortOrder = [];

    jsfn_ConvertToDatableWithCustomSort($('#tblProfile'), vColumnsList, 'No Profile Found', vSortOrder);
}

function jsfn_ShowProfileDetails(prfID) {
    var vData = {
        ProfileID: prfID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Default/GetProfileDetails', vData, jsfn_ShowProfileDetailResponse);
}
