jQuery(document).ready(function () {

    //function setCheckedItemId(id) {
    //    alert(id);
    //    document.getElementById("hdnMDAServiceIds").value = id;
    //}

    //function setCheckedItemId(id) {
    //    alert("test" + id);
    //    let checkboxId = "serviceCheckbox" + id;
    //    let checkbox = document.getElementById("checkboxId");

    //    if (checkbox?.checked) {
    //        document.getElementById("hdnMDAServiceIds").value = id;
    //    }
    //}

    //$('#tblMDAServices').on('click', 'icheck', function () {
    //    alert(this);
    //});

    jsfn_bindTable();

    //this.ischanged = function () {
    //    alert("Test");
    //}
});


function jsfn_bindTable() {
    var vColumnsList = [
        {
            "orderable": true
        },
        {
            "orderable": true
        },
        {
            "orderable": true
        },
        
        {
            "orderable": false
        }
    ];

    var vSortOrder = [];

    var vColumnDefs = [{
        //targets: [ 10 ],
        visible: false,
        searchable: false,
    }];

    jsfn_ConvertToDatableWithCustomSort($('#tblMDAServices'), vColumnsList, 'No Service Bill Found', vSortOrder, vColumnDefs);
}

//function jsfn_bindTable() {

//    $('#tblMDAServices').DataTable({
//        "language": {
//            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
//        },
//        "processing": true,
//        "serverSide": true,
//        "ajax": {
//            "url": "/Base/GetMDAServiceData",
//            "type": "POST",
//            "dataType": "JSON",
//            "error": function (jqXHR, exception) {
//                if (jqXHR.status == 401) {
//                    window.location.href = '/Login/Individual';
//                }
//            },
//            "complete": function () {
//                handleiCheck();
//            }
//        },
//        "columns": [{
//            "data": "TaxYear"
//        }, {
//            "data": "MDAServiceName"
//        }, {
//            "data": "ServiceAmount"
//        }, {
//            "data": "MDAServiceID"
//        }],
//        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
//            var test = aData["MDAServiceID"];
//            $('td:eq(3)', nRow).html('<input type="checkbox" class ="icheck" data-checkbox ="icheckbox_square-green" title ="Tick To Select" id="serviceCheckbox' + test + '" onclick="Change.ischanged()" value="' + test + '">');

//            //console.log(test); onchange="setCheckedItemId(' + test +')" 
//            var vServiceAmount = aData["ServiceAmount"] != null ? aData["ServiceAmount"] : 0;
//            $('td:eq(2)', nRow).html(vServiceAmount.formatMoney());
//        },
//    });


//}




