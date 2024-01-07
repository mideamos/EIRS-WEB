window.onload = function ()
{
    showLoader();
    $.ajax({
        url: apiUrl + "Account/Login",
        method: "POST",
        data: {"grant_type": "password", "UserName": "dhdigital", "Password": "Admin321%%**"},
        success: function (response)
        {
            console.log(response);

            accessToken = response.access_token;
            getLandList();
            showCharts();
  //          getServiceBillList();
//            getVehiclesList();
        }
    });
    
    /*$(".chartContainer").CanvasJSChart({
                title: {
                    text: "Line chart 1"
                },
                axisY: {
                    title: "Bill amount",
                    includeZero: false
                },
                axisX: {
                    interval: 1
                },
                data: [
                    {
                        type: "line", //try changing to column, area
                        toolTipContent: "{label}: {y} $",
                        dataPoints: [{ label: "Jan",  y: 5.28 },
                            { label: "Feb",  y: 3.83 },
                            { label: "March",y: 6.55 },
                            { label: "April",y: 4.81 },
                            { label: "May",  y: 2.37 },
                            { label: "June", y: 2.33 },
                            { label: "July", y: 3.06 },
                            { label: "Aug",  y: 2.94 },
                            { label: "Sep",  y: 5.41 },
                            { label: "Oct",  y: 2.17 },
                            { label: "Nov",  y: 2.17 },
                            { label: "Dec",  y: 2.80 }]
                    }
                ]
            });*/
};

function getServiceBillList()
{
    $.ajax({
        url: apiUrl + "RevenueData/ServiceBill/List",
        method: "GET",
        headers: {'Authorization': 'Bearer ' + accessToken},
        success: function (response)
        {
            console.log(response);
            serviceBillListResult = response.Result;
            
            getAssessmentBillList();
        }
    });
}

function getAssessmentBillList()
{
    $.ajax({
        url: apiUrl + "RevenueData/Assessment/List",
        method: "GET",
        headers: {'Authorization': 'Bearer ' + accessToken},
        success: function (response)
        {
            console.log(response);
            assessmentBillListResult = response.Result;
            showCharts();
            //getBusinessesList();
        }
    });
}

function getBusinessesList()
{
    $.ajax({
        url: apiUrl + "Asset/Business/List",
        method: "POST",
        headers: {'Authorization': 'Bearer ' + accessToken},
        success: function (response)
        {
            console.log(response);
            businessesListResult = response.Result;
            
            getLandList();
        }
    });
}

function getLandList()
{
    $.ajax({
        url: apiUrl + "Asset/Land/List",
        method: "POST",
        headers: {'Authorization': 'Bearer ' + accessToken},
        success: function (response)
        {
            console.log(response);
            landListResult = response.Result;
            
            getBuildingsList();
        }
    });
}

function getBuildingsList()
{
    $.ajax({
        url: apiUrl + "Asset/Building/List",
        method: "POST",
        headers: {'Authorization': 'Bearer ' + accessToken},
        success: function (response)
        {
            console.log(response);
            buildingListResult = response.Result;
            
            getVehiclesList();
        }
    });
}

function getVehiclesList()
{
    $.ajax({
        url: apiUrl + "Asset/Vehicle/List",
        method: "POST",
        headers: {'Authorization': 'Bearer ' + accessToken},
        success: function (response)
        {
            console.log(response);
            vehicleListResult = response.Result;
            
            hideLoader();
            showCharts();
        }
    });
}

/*
    axisX: {
        interval: 1
    }    
*/

function showCharts()
{
    $(".chartContainer1").CanvasJSChart({
        axisY: {
            title: "Bill amount",
            includeZero: false
        },
        axisX: {
            
        },
        data: [
            {
                type: "line", //try changing to column, area
                toolTipContent: "{label}: {y} ?",
                dataPoints: []
            }
        ]
    });
    
    $(".chartContainer2").CanvasJSChart({
        axisY: {
            title: "Bill amount",
            includeZero: false
        },
        axisX: {
            
        },
        data: [
            {
                type: "line", //try changing to column, area
                toolTipContent: "{label}: {y} ?",
                dataPoints: []
            }
        ]
    });
    
    $(".chartContainer3").CanvasJSChart({
        axisY: {
            title: "Settlement amount",
            includeZero: false
        },
        axisX: {
            
        },
        data: [
            {
                type: "line", //try changing to column, area
                toolTipContent: "{label}: {y} ?",
                dataPoints: []
            }
        ]
    });
    
    $(".chartContainer4").CanvasJSChart({
        axisY: {
            title: "Bill Aging",
            includeZero: false
        },
        axisX: {
            
        },
        data: [
            {
                type: "line", //try changing to column, area
                toolTipContent: "{label}: {y} ?",
                dataPoints: []
            }
        ]
    });

    getDropdownValues();
    showDropdownValues();

    lineChart1 = $(".chartContainer1").CanvasJSChart();
    updateChart1('monthly');
    
    LineChart2.lineChart = $(".chartContainer2").CanvasJSChart();
    LineChart2.updateChart('monthly');
    
    LineChart3.lineChart = $(".chartContainer3").CanvasJSChart();
    LineChart3.updateChart('monthly');
    
    LineChart4.lineChart = $(".chartContainer4").CanvasJSChart();
    LineChart4.updateChart();
    
    $(".pieChartContainer1").CanvasJSChart({
        axisY: {
            title: "Businesses",
            includeZero: false
        },
        axisX: {
            interval: 1
        },
        data: [
            {
                type: "pie", //try changing to column, area
                showInLegend: true,
                toolTipContent: "{y} - #percent %",
                yValueFormatString: "#0.#,,. Businesses",
                legendText: "{indexLabel}",
                dataPoints: []
            }
        ]
    });
    
    $(".pieChartContainer2").CanvasJSChart({
        axisY: {
            title: "Lands",
            includeZero: false
        },
        axisX: {
            interval: 1
        },
        data: [
            {
                type: "pie", //try changing to column, area
                showInLegend: true,
                toolTipContent: "{y} - #percent %",
                yValueFormatString: "#0.#,,. Lands",
                legendText: "{indexLabel}",
                dataPoints: []
            }
        ]
    });
    
    $(".pieChartContainer3").CanvasJSChart({
        axisY: {
            title: "Buildings",
            includeZero: false
        },
        axisX: {
            interval: 1
        },
        data: [
            {
                type: "pie", //try changing to column, area
                showInLegend: true,
                toolTipContent: "{y} - #percent %",
                yValueFormatString: "#0.#,,. Buildings",
                legendText: "{indexLabel}",
                dataPoints: []
            }
        ]
    });
    
    $(".pieChartContainer4").CanvasJSChart({
        axisY: {
            title: "Vehicles",
            includeZero: false
        },
        axisX: {
            interval: 1
        },
        data: [
            {
                type: "pie", //try changing to column, area
                showInLegend: true,
                toolTipContent: "{y} - #percent %",
                yValueFormatString: "#0.#,,. Vehicles",
                legendText: "{indexLabel}",
                dataPoints: []
            }
        ]
    });
    
    PieChart1.chart = $(".pieChartContainer1").CanvasJSChart();
    PieChart1.updateChart();
    
    PieChart2.chart = $(".pieChartContainer2").CanvasJSChart();
    PieChart2.updateChart();
    
    PieChart3.chart = $(".pieChartContainer3").CanvasJSChart();
    PieChart3.updateChart();
    
    PieChart4.chart = $(".pieChartContainer4").CanvasJSChart();
    PieChart4.updateChart();
}

function getDropdownValues()
{
    ///////////////////// Line chart 1 /////////////////////
    for (var a = 0; a < serviceBillListResult.length; a++)
    {
        var flag = false;
        
        for (var b = 0; b < settlementStatusNames.length; b++)
        {
            if (settlementStatusNames[b] == serviceBillListResult[a].SettlementStatusName)
            {
                flag = true;
                break;
            }
        }
        
        if (flag == false)
        {
            settlementStatusNames.push(serviceBillListResult[a].SettlementStatusName);
        }
    }
    ////////////////////////////////////////////////////////
    
    ///////////////////// Line chart 2, 3 & 4 /////////////////////
    for (var a = 0; a < serviceBillListResult.length; a++)
    {
        var flag = false;
        
        for (var b = 0; b < taxPayerTypes.length; b++)
        {
            if (taxPayerTypes[b] == serviceBillListResult[a].TaxPayerTypeName)
            {
                flag = true;
                break;
            }
        }
        
        if (flag == false)
        {
            taxPayerTypes.push(serviceBillListResult[a].TaxPayerTypeName);
        }
    }
    ////////////////////////////////////////////////////////
}

function showDropdownValues()
{
    ///////////////////// Line chart 1 /////////////////////
    var html = "<option value='all'>All Settlement Status</option>";
    
    for (var a = 0; a < settlementStatusNames.length; a++)
    {
        html += "<option value='" + settlementStatusNames[a] + "'>" + settlementStatusNames[a] + "</option>";
    }
    
    document.getElementById("bill-stage-1").innerHTML = html;
    $("#bill-stage-1").selectpicker('refresh');
    ////////////////////////////////////////////////////////
    
    ///////////////////// Line chart 2, 3 & 4 /////////////////////
    var html = "<option value='all'>All Tax Payers</option>";
    
    for (var a = 0; a < taxPayerTypes.length; a++)
    {
        html += "<option value='" + taxPayerTypes[a] + "'>" + taxPayerTypes[a] + "</option>";
    }
    
    document.getElementById("tax-payer-type-2").innerHTML = html;
    document.getElementById("tax-payer-type-3").innerHTML = html;
    document.getElementById("tax-payer-type-4").innerHTML = html;
    $("#tax-payer-type-2").selectpicker('refresh');
    $("#tax-payer-type-3").selectpicker('refresh');
    $("#tax-payer-type-4").selectpicker('refresh');

    ////////////////////////////////////////////////////////////
}

function showLoader()
{
    $("body").addClass("loading");
}

function hideLoader()
{
    $("body").removeClass("loading");
}