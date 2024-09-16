$(document).ready(function () {
    $("#cboBusinessFilter").on('change', function () {
        var vBusinessFilter = $("#cboBusinessFilter").val();
        jsfn_BindBusinessChart(vBusinessFilter);
    });

    $("#cboLandFilter").on('change', function () {
        var vLandFilter = $("#cboLandFilter").val();
        jsfn_BindLandChart(vLandFilter);
    });

    $("#cboBuildingFilter").on('change', function () {
        var vBuildingFilter = $("#cboBuildingFilter").val();
        jsfn_BindBuildingChart(vBuildingFilter);
    });

    $("#cboVehicleFilter").on('change', function () {
        var vVehicleFilter = $("#cboVehicleFilter").val();
        jsfn_BindVehicleChart(vVehicleFilter);
    });

    $("#cboB_BillType").on('change', function () { jsfn_BindBillChart(2); });
    $("#cboSettlementStatus").on('change', function () { jsfn_BindBillChart(2); });

    $("#btnB_Yearly").on('click', function () { jsfn_BindBillChart(1); });
    $("#btnB_Monthly").on('click', function () { jsfn_BindBillChart(2); });
    $("#btnB_Weekly").on('click', function () { jsfn_BindBillChart(3); });
    $("#btnB_Daily").on('click', function () { jsfn_BindBillChart(4); });

    $("#cboTPB_BillType").on('change', function () { jsfn_BindTaxPayerBillChart(2); });
    $("#cboTPB_TaxPayerType").on('change', function () { jsfn_BindTaxPayerBillChart(2); });

    $("#btnTPB_Yearly").on('click', function () { jsfn_BindTaxPayerBillChart(1); });
    $("#btnTPB_Monthly").on('click', function () { jsfn_BindTaxPayerBillChart(2); });
    $("#btnTPB_Weekly").on('click', function () { jsfn_BindTaxPayerBillChart(3); });
    $("#btnTPB_Daily").on('click', function () { jsfn_BindTaxPayerBillChart(4); });

    $("#cboTPS_BillType").on('change', function () { jsfn_BindTaxPayerSettlementChart(2); });
    $("#cboTPS_TaxPayerType").on('change', function () { jsfn_BindTaxPayerSettlementChart(2); });

    $("#btnTPS_Yearly").on('click', function () { jsfn_BindTaxPayerSettlementChart(1); });
    $("#btnTPS_Monthly").on('click', function () { jsfn_BindTaxPayerSettlementChart(2); });
    $("#btnTPS_Weekly").on('click', function () { jsfn_BindTaxPayerSettlementChart(3); });
    $("#btnTPS_Daily").on('click', function () { jsfn_BindTaxPayerSettlementChart(4); });

    $("#cboBA_BillType").on('change', jsfn_BindBillAgingChart);
    $("#cboBA_TaxPayerType").on('change', jsfn_BindBillAgingChart);
    $("#cboBAFilter").on('change', jsfn_BindBillAgingChart);


    jsfn_BindBillChart(2);
    jsfn_BindTaxPayerBillChart(2);
    jsfn_BindTaxPayerSettlementChart(2);
    jsfn_BindBillAgingChart();
    jsfn_BindBusinessChart(1);
    jsfn_BindLandChart(1);
    jsfn_BindBuildingChart(1);
    jsfn_BindVehicleChart(1);


});

function jsfn_BindBillChart(FilterTypeID) {
    var vBillTypeID = $("#cboB_BillType").val();
    var vStatusID = $("#cboSettlementStatus").val();

    var vData = {
        BillTypeID: vBillTypeID,
        StatusID: vStatusID === '' ? 0 : vStatusID,
        FilterTypeID: FilterTypeID === undefined ? 2 : FilterTypeID
    };

    jsfn_ajaxPost('/Home/GetBillChart', vData, jsfn_BindBillChartResponse);
}

function jsfn_BindBillChartResponse(data) {
    $("#chrtBillContainer").CanvasJSChart({
        animationEnabled: true,
        theme: "light2",
        axisY: {
            includeZero: false,
            lineThickness: 1
        },
        axisX: {
            valueFormatString: getValueFormatString(data.FilterTypeID),
            intervalType: getintervalType(data.FilterTypeID),
            interval: getinterval(data.FilterTypeID)
        },
        toolTip: {
            shared: true
        },
    });

    var vchrtBillContainer = $("#chrtBillContainer").CanvasJSChart();
    vchrtBillContainer.options.data = [];
    vchrtBillContainer.render();


    var vBillTypeID = $("#cboB_BillType").val();
    var vABDataPoints = [], vSBDataPoints = [];

    $.each(data.ChartData, function (i, item) {
        if (item.BillTypeID === 1) {
            vABDataPoints.push({
                x: parseJsonDate(item.indexLabel),
                y: item.y
            });
        } else {
            vSBDataPoints.push({
                x: parseJsonDate(item.indexLabel),
                y: item.y
            });
        }
    });

    if (vBillTypeID === '1' || vBillTypeID === '0') {
        vchrtBillContainer.options.data.push({
            type: "line",
            name: "Assessment Bill",
            showInLegend: true,
            xValueFormatString: getValueFormatString(data.FilterTypeID),
            yValueFormatString: "₦#,##0.##",
            xValueType: "dateTime",
            dataPoints: vABDataPoints
        });
    }

    if (vBillTypeID === '2' || vBillTypeID === '0') {
        vchrtBillContainer.options.data.push({
            type: "line",
            name: "Service Bill",
            showInLegend: true,
            yValueFormatString: "₦#,##0.##",
            xValueFormatString: getValueFormatString(data.FilterTypeID),
            xValueType: "dateTime",
            dataPoints: vSBDataPoints
        });
    }

    vchrtBillContainer.render();
}


function jsfn_BindTaxPayerBillChart(FilterTypeID) {
    var vBillTypeID = $("#cboTPB_BillType").val();
    var vTaxPayerTypeID = $("#cboTPB_TaxPayerType").val();

    var vData = {
        BillTypeID: vBillTypeID,
        TaxPayerTypeID: vTaxPayerTypeID === '' ? 0 : vTaxPayerTypeID,
        FilterTypeID: FilterTypeID === undefined ? 2 : FilterTypeID
    };

    jsfn_ajaxPost('/Home/GetTaxPayerBillChart', vData, jsfn_BindTaxPayerBillChartResponse);
}

function jsfn_BindTaxPayerBillChartResponse(data) {
    $("#chrtTaxPayerBillContainer").CanvasJSChart({
        animationEnabled: true,
        theme: "light2",
        axisY: {
            includeZero: false,
            lineThickness: 1
        },
        axisX: {
            valueFormatString: getValueFormatString(data.FilterTypeID),
            intervalType: getintervalType(data.FilterTypeID),
            interval: getinterval(data.FilterTypeID)
        },
        toolTip: {
            shared: true
        },
    });

    var vchrtTaxPayerBillContainer = $("#chrtTaxPayerBillContainer").CanvasJSChart();
    vchrtTaxPayerBillContainer.options.data = [];
    vchrtTaxPayerBillContainer.render();


    var vBillTypeID = $("#cboTPB_BillType").val();
    var vABDataPoints = [], vSBDataPoints = [];

    $.each(data.ChartData, function (i, item) {
        if (item.BillTypeID === 1) {
            vABDataPoints.push({
                x: parseJsonDate(item.indexLabel),
                y: item.y
            });
        } else {
            vSBDataPoints.push({
                x: parseJsonDate(item.indexLabel),
                y: item.y
            });
        }
    });

    if (vBillTypeID === '1' || vBillTypeID === '0') {
        vchrtTaxPayerBillContainer.options.data.push({
            type: "line",
            name: "Assessment Bill",
            showInLegend: true,
            xValueFormatString: getValueFormatString(data.FilterTypeID),
            yValueFormatString: "₦#,##0.##",
            xValueType: "dateTime",
            dataPoints: vABDataPoints
        });
    }

    if (vBillTypeID === '2' || vBillTypeID === '0') {
        vchrtTaxPayerBillContainer.options.data.push({
            type: "line",
            name: "Service Bill",
            showInLegend: true,
            yValueFormatString: "₦#,##0.##",
            xValueFormatString: getValueFormatString(data.FilterTypeID),
            xValueType: "dateTime",
            dataPoints: vSBDataPoints
        });
    }

    vchrtTaxPayerBillContainer.render();
}

function jsfn_BindTaxPayerSettlementChart(FilterTypeID) {
    var vBillTypeID = $("#cboTPS_BillType").val();
    var vTaxPayerTypeID = $("#cboTPS_TaxPayerType").val();

    var vData = {
        BillTypeID: vBillTypeID,
        TaxPayerTypeID: vTaxPayerTypeID === '' ? 0 : vTaxPayerTypeID,
        FilterTypeID: FilterTypeID === undefined ? 2 : FilterTypeID
    };

    jsfn_ajaxPost('/Home/GetTaxPayerSettlementChart', vData, jsfn_BindTaxPayerSettlementChartResponse);
}

function jsfn_BindTaxPayerSettlementChartResponse(data) {
    $("#chrtTaxPayerSettlementContainer").CanvasJSChart({
        animationEnabled: true,
        theme: "light2",
        axisY: {
            includeZero: false,
            lineThickness: 1
        },
        axisX: {
            valueFormatString: getValueFormatString(data.FilterTypeID),
            intervalType: getintervalType(data.FilterTypeID),
            interval: getinterval(data.FilterTypeID)
        },
        toolTip: {
            shared: true
        },
    });

    var vchrtTaxPayerSettlementContainer = $("#chrtTaxPayerSettlementContainer").CanvasJSChart();
    vchrtTaxPayerSettlementContainer.options.data = [];
    vchrtTaxPayerSettlementContainer.render();


    var vBillTypeID = $("#cboTPS_BillType").val();
    var vABDataPoints = [], vSBDataPoints = [];

    $.each(data.ChartData, function (i, item) {
        if (item.BillTypeID === 1) {
            vABDataPoints.push({
                x: parseJsonDate(item.indexLabel),
                y: item.y
            });
        } else {
            vSBDataPoints.push({
                x: parseJsonDate(item.indexLabel),
                y: item.y
            });
        }
    });

    if (vBillTypeID === '1' || vBillTypeID === '0') {
        vchrtTaxPayerSettlementContainer.options.data.push({
            type: "line",
            name: "Assessment Bill",
            showInLegend: true,
            xValueFormatString: getValueFormatString(data.FilterTypeID),
            yValueFormatString: "₦#,##0.##",
            xValueType: "dateTime",
            dataPoints: vABDataPoints
        });
    }

    if (vBillTypeID === '2' || vBillTypeID === '0') {
        vchrtTaxPayerSettlementContainer.options.data.push({
            type: "line",
            name: "Service Bill",
            showInLegend: true,
            yValueFormatString: "₦#,##0.##",
            xValueFormatString: getValueFormatString(data.FilterTypeID),
            xValueType: "dateTime",
            dataPoints: vSBDataPoints
        });
    }

    vchrtTaxPayerSettlementContainer.render();
}

function jsfn_BindBillAgingChart() {
    var vBillTypeID = $("#cboBA_BillType").val();
    var vTaxPayerTypeID = $("#cboBA_TaxPayerType").val();
    var vFilterTypeID = $("#cboBAFilter").val();

    var vData = {
        BillTypeID: vBillTypeID,
        TaxPayerTypeID: vTaxPayerTypeID === '' ? 0 : vTaxPayerTypeID,
        FilterTypeID: vFilterTypeID
    };

    jsfn_ajaxPost('/Home/GetBillAgingChart', vData, jsfn_BindBillAgingChartResponse);
}

function jsfn_BindBillAgingChartResponse(data) {
    $("#chrtBillAgingContainer").CanvasJSChart({
        animationEnabled: true,
        theme: "light2",
        axisY: {
            includeZero: false,
            lineThickness: 1
        },
        axisX: {
            valueFormatString: "MMM",
            intervalType: "month",
            interval: 1
        },
        toolTip: {
            shared: true
        },
    });

    var vchrtBillAging = $("#chrtBillAgingContainer").CanvasJSChart();
    vchrtBillAging.options.data = [];
    vchrtBillAging.render();


    var vBillTypeID = $("#cboBA_BillType").val();
    var vABDataPoints = [], vSBDataPoints = [];

    $.each(data, function (i, item) {
        if (item.BillTypeID === 1) {
            vABDataPoints.push({
                x: parseJsonDate(item.indexLabel),
                y: item.y
            });
        } else {
            vSBDataPoints.push({
                x: parseJsonDate(item.indexLabel),
                y: item.y
            });
        }
    });

    if (vBillTypeID === '1' || vBillTypeID === '0') {
        vchrtBillAging.options.data.push({
            type: "line",
            name: "Assessment Bill",
            showInLegend: true,
            xValueFormatString: "MMMM YYYY",
            yValueFormatString: "₦#,##0.##",
            xValueType: "dateTime",
            dataPoints: vABDataPoints
        });
    }

    if (vBillTypeID === '2' || vBillTypeID === '0') {
        vchrtBillAging.options.data.push({
            type: "line",
            name: "Service Bill",
            showInLegend: true,
            yValueFormatString: "₦#,##0.##",
            xValueFormatString: "MMMM YYYY",
            xValueType: "dateTime",
            dataPoints: vSBDataPoints
        });
    }

    vchrtBillAging.render();

}

function jsfn_BindBusinessChart(vBusinessFilter) {
    var vData = {
        FilterType: vBusinessFilter
    };

    jsfn_ajaxPost('/Home/GetBusinessChart', vData, jsfn_BindBusinessChartResponse);
}

function jsfn_BindBusinessChartResponse(data) {
    $("#chrtBusinessContainer").CanvasJSChart({
        theme: "light1",
        axisY: {
            title: "Businesses",
            includeZero: false,
            valueFormatString: "####"
        },
        axisX: {
            interval: 1,
            valueFormatString: "####",
        },
        data: [
            {
                type: "pie",
                showInLegend: true,
                toolTipContent: "{indexLabel} : {y} - #percent %",
                yValueFormatString: "#### Businesses",
                legendText: "{indexLabel}",
                dataPoints: data
            }
        ]
    });
}

function jsfn_BindLandChart(vLandFilter) {
    var vData = {
        FilterType: vLandFilter
    };

    jsfn_ajaxPost('/Home/GetLandChart', vData, jsfn_BindLandChartResponse);
}

function jsfn_BindLandChartResponse(data) {
    $("#chrtLandContainer").CanvasJSChart({
        theme: "light1",
        axisY: {
            title: "Lands",
            includeZero: false,
            valueFormatString: "####"
        },
        axisX: {
            interval: 1,
            valueFormatString: "####",
        },
        data: [
            {
                type: "pie",
                showInLegend: true,
                toolTipContent: "{indexLabel} : {y} - #percent %",
                yValueFormatString: "#### Lands",
                legendText: "{indexLabel}",
                dataPoints: data
            }
        ]
    });
}

function jsfn_BindBuildingChart(vBuildingFilter) {
    var vData = {
        FilterType: vBuildingFilter
    };

    jsfn_ajaxPost('/Home/GetBuildingChart', vData, jsfn_BindBuildingChartResponse);
}

function jsfn_BindBuildingChartResponse(data) {
    $("#chrtBuildingContainer").CanvasJSChart({
        theme: "light1",
        axisY: {
            title: "Buildings",
            includeZero: false,
            valueFormatString: "####"
        },
        axisX: {
            interval: 1,
            valueFormatString: "####",
        },
        data: [
            {
                type: "pie",
                showInLegend: true,
                toolTipContent: "{indexLabel} : {y} - #percent %",
                yValueFormatString: "#### Buildings",
                legendText: "{indexLabel}",
                dataPoints: data
            }
        ]
    });
}

function jsfn_BindVehicleChart(vVehicleFilter) {
    var vData = {
        FilterType: vVehicleFilter
    };

    jsfn_ajaxPost('/Home/GetVehicleChart', vData, jsfn_BindVehicleChartResponse);
}

function jsfn_BindVehicleChartResponse(data) {
    $("#chrtVehicleContainer").CanvasJSChart({
        theme: "light1",
        axisY: {
            title: "Vehicles",
            includeZero: false,
            valueFormatString: "####"
        },
        axisX: {
            interval: 1,
            valueFormatString: "####",
        },
        data: [
            {
                type: "pie",
                showInLegend: true,
                toolTipContent: "{indexLabel} : {y} - #percent %",
                yValueFormatString: "#### Vehicles",
                legendText: "{indexLabel}",
                dataPoints: data
            }
        ]
    });
}

function parseJsonDate(jsonDateString) {
    return parseInt(jsonDateString.replace('/Date(', ''));
}

function getValueFormatString(FilterTypeID) {
    switch (FilterTypeID) {
        case 1: return 'YYYY';
        case 2: return 'MMM YY';
        case 3: return 'DD MMM YYYY';
        case 4: return 'DD MMM YYYY';
        default: return 'MMM';
    }
}

function getintervalType(FilterTypeID) {
    switch (FilterTypeID) {
        case 1: return 'year';
        case 2: return 'month';
        case 3: return 'day';
        case 4: return 'day';
        default: return 'month';
    }
}

function getinterval(FilterTypeID) {
    switch (FilterTypeID) {
        case 1: return 1;
        case 2: return 1;
        case 3: return 7;
        case 4: return 5;
        default: return 1;
    }
}