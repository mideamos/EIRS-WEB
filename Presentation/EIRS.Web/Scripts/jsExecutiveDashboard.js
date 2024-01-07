var vchrtAssessmentTaxOfficeAssessed;
$(document).ready(function () {

    $('#dvATAReportRange').daterangepicker({
        separator: ' to ',
        startDate: moment().subtract(3, 'years').startOf('year'),
        endDate: moment().subtract(1, 'year').endOf('year'),
        showDropdowns: true,
        buttonClasses: ['btn'],
        applyClass: 'green',
        cancelClass: 'default',
        minDate: '01/01/2018',
        maxDate: moment(),
        ranges: {
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 6 Weeks': [moment().subtract(7, 'weeks'), moment()],
            'Last 6 Months': [moment().subtract(6, 'months').startOf('month'), moment().subtract(1, 'month').endOf('month')],
            'Last 3 Years': [moment().subtract(3, 'years').startOf('year'), moment().subtract(1, 'year').endOf('year')],
        },
        locale: {
            applyLabel: 'Apply',
            fromLabel: 'From',
            toLabel: 'To',
            customRangeLabel: 'Custom Range',
            daysOfWeek: ['Su', 'Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa'],
            monthNames: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
            firstDay: 1
        }
       
    },
        function (start, end) {
            $('#dvATAReportRange span').html(start.format('MMM D, YYYY') + ' - ' + end.format('MMM D, YYYY'));
            jsfn_LoadAssessmentTaxOfficeAssessedChart();
        }
    );

    $('#dvATAReportRange span').html(moment().subtract(3, 'years').startOf('year').format('MMM D, YYYY') + ' - ' + moment().subtract(1, 'year').endOf('year').format('MMM D, YYYY'));

    $('#chkTAC').change(jsfn_LoadAssessmentTaxOfficeAssessedChart);
    $("#cboATATaxOffice").on('change', jsfn_LoadAssessmentTaxOfficeAssessedChart);

    //$("#cboATAPeriod").on('change', jsfn_LoadAssessmentTaxOfficeAssessedChart);

    var options = {
        series: [],
        chart: {
            height: 350,
            type: 'bar',
            toolbar: {
                show: true,
                tools: {
                    download: true,
                },
                export: {
                    csv: {
                        filename: 'assessment_taxoffice_assessed',
                        columnDelimiter: ',',
                        headerCategory: 'taxoffice',
                        headerValue: 'value',
                        dateFormatter(timestamp) {
                            return new Date(timestamp).toDateString()
                        }
                    },
                    png: {
                        filename: 'assessment_taxoffice_assessed',
                    }

                }
            }
        },
        plotOptions: {
            bar: {
                horizontal: false,
                columnWidth: '55%',
                endingShape: 'rounded'
            },
        },
        dataLabels: {
            enabled: false
        },
        stroke: {
            show: true,
            width: 2,
            colors: ['transparent']
        },
        yaxis: {
            title: {
                text: 'Amount'
            }
        },
        fill: {
            opacity: 1
        },
        tooltip: {
            shared: false,
            y: {
                formatter: function (val) {
                    return val.formatMoney(2)
                }
            }
        },
        noData: {
            text: 'Loading...'
        },
    };

    vchrtAssessmentTaxOfficeAssessed = new ApexCharts(document.querySelector("#chrtAssessmentTaxOfficeAssessed"), options);
    vchrtAssessmentTaxOfficeAssessed.render();

    jsfn_LoadAssessmentTaxOfficeAssessedChart();

    $("#btnATADowload").click(function () {
        var vTaxOfficeIds = $("#cboATATaxOffice").val() == null ? '' : $("#cboATATaxOffice").val().toString();
        var vStatusId = 1;
        var vPeriodId = jsfn_GetPeriodId($("#dvATAReportRange").data('daterangepicker').chosenLabel);
        var vSelectedSeries = $("#cboATASeries").val() == null ? '' : $("#cboATASeries").val().toString();
        var vFromDate = $("#dvATAReportRange").data('daterangepicker').startDate.format('YYYY-MM-DD');
        var vToDate = $("#dvATAReportRange").data('daterangepicker').endDate.format('YYYY-MM-DD');

        var vUrl = '/Home/GetAssessmentTaxOfficeAssessedData?TaxOfficeIds=' + vTaxOfficeIds + '&StatusId=' + vStatusId + '&PeriodId=' + vPeriodId + '&SelectedSeries=' + vSelectedSeries + '&FromDate=' + vFromDate + '&ToDate=' + vToDate;
        var win = window.open(vUrl, '_blank');
        win.focus();
    });

});

function jsfn_LoadAssessmentTaxOfficeAssessedChart() {
    var PeriodId = jsfn_GetPeriodId($("#dvATAReportRange").data('daterangepicker').chosenLabel);

    var vData = {
        DisplayTypeId: $('#chkTAC').prop('checked') == true ? 1 : 2,
        TaxOfficeIds: $("#cboATATaxOffice").val() == null ? '' : $("#cboATATaxOffice").val().toString(),
        StatusId: 1,
        PeriodId: PeriodId,
        FromDate: $("#dvATAReportRange").data('daterangepicker').startDate.format('YYYY-MM-DD'),
        ToDate: $("#dvATAReportRange").data('daterangepicker').endDate.format('YYYY-MM-DD')
    };

    jsfn_ajaxPost('/Home/GetAssessmentTaxOfficeAssessedChart', vData, jsfn_BindAssessmentTaxOfficeAssessedResponse);
}

function jsfn_BindAssessmentTaxOfficeAssessedResponse(data) {

    if ($('#chkTAC').prop('checked')) {
        vchrtAssessmentTaxOfficeAssessed.updateOptions({
            xaxis: { categories: data.categories },
            yaxis: {
                title: {
                    text: 'Amount - (₦)'
                }
            },
            tooltip: {
                shared: true,
                y: {
                    formatter: function (val) {
                        return val == undefined ? val : val.formatMoney(2);
                    }
                }
            }
        });
    }
    else {
        vchrtAssessmentTaxOfficeAssessed.updateOptions({
            xaxis: { categories: data.categories },
            yaxis: {
                title: {
                    text: 'Count'
                }
            },
            tooltip: {
                shared: true,
                y: {
                    formatter: function (val) {
                        return val;
                    }
                }
            }
        });
    }

    vchrtAssessmentTaxOfficeAssessed.updateSeries(data.series);

    $("#cboATASeries").html("");
    $.each(data.seriesList, function (i, sl) {
        $("#cboATASeries").append($('<option></option>').val(sl.Id).html(sl.Text));
    });
    $('#cboATASeries').selectpicker('refresh');

    $("#dvAssessmentTaxOfficeAssessedTable").html(data.table_html);
    $("#tblAssessmentTaxOfficeAssessed").DataTable();
}

function jsfn_GetPeriodId(choosenLabel) {
    switch (choosenLabel) {
        case 'Last 3 Years' : return 1;
        case 'Last 6 Months' : return 2;
        case 'Last 6 Weeks' : return 3;
        case 'Last 7 Days': return 4;
        case 'Custom Range': return 5;
        default: return 1;
    }
}