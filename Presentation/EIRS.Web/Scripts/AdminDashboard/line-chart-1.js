var billStage1 = null;
var billType1 = null;

function getMonthlyData()
{
    var Result = defaultLineChart1;
//    console.log(Result);
    var listMonths = [];
    
    for (var a = 0; a < Result.length; a++)
    {
        if (Result[a].Active && (billStage1 == Result[a].SettlementStatusName || billStage1 == "all"))
        {
            var billDate = getDate1(Result[a]);
            var ServiceBillAmount = getAmount1(Result[a]);

            var isExists = false;
            var fullDate = monthNames[billDate.getMonth()];

            var totalServiceBillAmount = 0;

            for (var b = 0; b < listMonths.length; b++)
            {
                if (listMonths[b].label == fullDate)
                {
                    isExists = true;
                    break;
                }
            }

            for (var b = 0; b < Result.length; b++)
            {
                var tempBillDate = getDate1(Result[b]);
                if (billDate.getMonth() == tempBillDate.getMonth())
                {
                    totalServiceBillAmount += getAmount1(Result[b]);
                }
            }

            if (!isExists)
            {
                listMonths.push({
                    label: fullDate,
                    y: totalServiceBillAmount
                });
            }
        }
    }
    
    return listMonths;
}

function getYearlyData()
{
    var Result = defaultLineChart1;
    var listYears = [];
    
    for (var a = 0; a < Result.length; a++)
    {
        if (Result[a].Active && (billStage1 == Result[a].SettlementStatusName || billStage1 == "all"))
        {
            var billDate = getDate1(Result[a]);
            var ServiceBillAmount = getAmount1(Result[a]);

            var isExists = false;
            var fullDate = billDate.getFullYear();

            var totalServiceBillAmount = 0;

            for (var b = 0; b < listYears.length; b++)
            {
                if (listYears[b].label == fullDate)
                {
                    isExists = true;
                    break;
                }
            }

            for (var b = 0; b < Result.length; b++)
            {
                var tempBillDate = getDate1(Result[b]);
                if (fullDate == tempBillDate.getFullYear())
                {
                    totalServiceBillAmount += getAmount1(Result[b]);
                }
            }

            if (!isExists)
            {
                listYears.push({
                    label: fullDate,
                    y: totalServiceBillAmount
                });
            }
        }
    }
    
    return listYears;
}

function getWeeklyData()
{
    var Result = defaultLineChart1;
    var listWeeks = [];
    
    for (var a = 0; a < Result.length; a++)
    {
        if (Result[a].Active && (billStage1 == Result[a].SettlementStatusName || billStage1 == "all"))
        {
            var billDate = getDate1(Result[a]);
            var ServiceBillAmount = getAmount1(Result[a]);

            var isExists = false;
            var fullDate = "Week " + billDate.getWeek();

            var totalServiceBillAmount = 0;

            for (var b = 0; b < listWeeks.length; b++)
            {
                if (listWeeks[b].label == fullDate)
                {
                    isExists = true;
                    break;
                }
            }

            for (var b = 0; b < Result.length; b++)
            {
                var tempBillDate = getDate1(Result[b]);
                if (billDate.getWeek() == tempBillDate.getWeek())
                {
                    totalServiceBillAmount += getAmount1(Result[b]);
                }
            }

            if (!isExists)
            {
                listWeeks.push({
                    label: fullDate,
                    y: totalServiceBillAmount
                });
            }
        }
    }
    
    return listWeeks;
}

function getDailyData()
{
    var Result = defaultLineChart1;
    var listDates = [];
    
    for (var a = 0; a < Result.length; a++)
    {
        if (Result[a].Active && (billStage1 == Result[a].SettlementStatusName || billStage1 == "all"))
        {
            var billDate = getDate1(Result[a]);
            var ServiceBillAmount = getAmount1(Result[a]);

            var isExists = false;
            var fullDate = billDate.getDate() + " " + monthNames[billDate.getMonth()];

            var totalServiceBillAmount = 0;

            for (var b = 0; b < listDates.length; b++)
            {
                if (listDates[b].label == fullDate)
                {
                    isExists = true;
                    break;
                }
            }

            for (var b = 0; b < Result.length; b++)
            {
                var tempBillDate = getDate1(Result[b]);
                var tempFullDate = tempBillDate.getDate() + " " + monthNames[tempBillDate.getMonth()];
                
                if (fullDate == tempFullDate)
                {
                    totalServiceBillAmount += getAmount1(Result[b]);
                }
            }

            if (!isExists)
            {
                listDates.push({
                    label: fullDate,
                    y: totalServiceBillAmount
                });
            }
        }
    }
    
    return listDates;
}

function updateChart1(newValue, btn)
{
    if (lineChart1 != null)
    {
        lineChart1.options.data = [];
        lineChart1.render();
        
        billStage1 = document.getElementById("bill-stage-1").value;
        billType1 = document.getElementById("bill-type-1").value;
        
        if (billType1 == "all")
        {
            defaultLineChart1 = serviceBillListResult;
            lineChart1.options.data.push({
                type: "line", //try changing to column, area
                toolTipContent: "{label}: &#8358; {y}",
                dataPoints: getLineChart1Data(newValue),
                showInLegend: true,
                legendText: "service"
            });
            
            defaultLineChart1 = assessmentBillListResult;
            lineChart1.options.data.push({
                type: "line", //try changing to column, area
                toolTipContent: "{label}: &#8358; {y}",
                dataPoints: getLineChart1Data(newValue),
                showInLegend: true,
                legendText: "assessment"
            });
        }
        else 
        {
            if (billType1 == "service")
                defaultLineChart1 = serviceBillListResult;
            else
                defaultLineChart1 = assessmentBillListResult;
            
            lineChart1.options.data.push({
                type: "line", //try changing to column, area
                toolTipContent: "{label}: &#8358; {y}",
                dataPoints: getLineChart1Data(newValue),
                showInLegend: true,
                legendText: billType1
            });
        }
        
        lineChart1.render();
    }
    
    var buttons = document.getElementById("chart-1").getElementsByTagName("button");
    for (var a = 0; a < buttons.length; a++)
    {
        buttons[a].classList.remove("btn-selected");
    }
    
    if (btn == null)
    {
        btn = buttons[1];
    }
    
    btn.classList.add("btn-selected");    
}

function getLineChart1Data(period)
{
    var newData = [];
    if (period == "yearly")
    {
        newData = getYearlyData();
    }
    else if (period == "monthly")
    {
        newData = getMonthlyData();
    }
    else if (period == "weekly")
    {
        newData = getWeeklyData();
    }
    else if (period == "daily")
    {
        newData = getDailyData();
    }
    return newData;
}

function getDate1(Result)
{
    if (defaultLineChart1 == serviceBillListResult)
        return new Date(Result.ServiceBillDate);
    else
        return new Date(Result.AssessmentDate);
}

function getAmount1(Result)
{
    if (defaultLineChart1 == serviceBillListResult)
        return Result.ServiceBillAmount;
    else
        return Result.AssessmentAmount;
}