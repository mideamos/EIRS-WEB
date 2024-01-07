var vTaxPayerDataTable;

function jsfn_BuildDataTable() {
    $("#dvSearchResult").removeClass('hide');
    $('#dvSFTPWA a.jarviswidget-toggle-btn').trigger('click');

    vTaxPayerDataTable = jsfn_ConvertToDatatable($('#tbTaxPayerWithoutAsset'));
    //Hide Columns
    //vTaxPayerDataTable.column(2).visible(false);
    //vTaxPayerDataTable.column(3).visible(false);
    //vTaxPayerDataTable.column(5).visible(false);
    //vTaxPayerDataTable.column(6).visible(false);
    //vTaxPayerDataTable.column(8).visible(false);
    //vTaxPayerDataTable.column(10).visible(false);
    //vTaxPayerDataTable.column(11).visible(false);
    //vTaxPayerDataTable.column(12).visible(false);
    //vTaxPayerDataTable.column(13).visible(false);
    //vTaxPayerDataTable.column(14).visible(false);
    //vTaxPayerDataTable.column(15).visible(false);
    //vTaxPayerDataTable.column(16).visible(false);
    //vTaxPayerDataTable.column(17).visible(false);
}

