var vCompanyDataTable, vIndividualDataTable;

jQuery(document).ready(function () {
    jsfn_BuildDataTable();
});

function jsfn_BuildDataTable() {
    vCompanyDataTable = jsfn_ConvertToDatatable($('#tbCompany'));
    //Hide Columns
    vCompanyDataTable.column(4).visible(false);
    vCompanyDataTable.column(6).visible(false);
    vCompanyDataTable.column(8).visible(false);

    vIndividualDataTable = jsfn_ConvertToDatatable($('#tbIndividual'));
    //Hide Columns
    vIndividualDataTable.column(1).visible(false);
    vIndividualDataTable.column(2).visible(false);
    vIndividualDataTable.column(5).visible(false);
    vIndividualDataTable.column(6).visible(false);
    vIndividualDataTable.column(7).visible(false);
    vIndividualDataTable.column(9).visible(false);
    vIndividualDataTable.column(11).visible(false);
    vIndividualDataTable.column(12).visible(false);
    vIndividualDataTable.column(14).visible(false);
    vIndividualDataTable.column(15).visible(false);
    vIndividualDataTable.column(16).visible(false);
    vIndividualDataTable.column(17).visible(false);
}

