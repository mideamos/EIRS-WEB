var LineChart3 = {
    
    lineChart: null,
    defaultLineChart: null,
    billType: null,
    taxPayerType: null,
    
    getMonthlyData: function()
    {
        var Result = this.defaultLineChart;
//        console.log(Result);
        var listMonths = [];

        for (var a = 0; a < Result.length; a++)
        {
            if (Result[a].Active && (this.taxPayerType == Result[a].TaxPayerTypeName || this.taxPayerType == "all"))
            {
                var billDate = this.getDate(Result[a]);
                var ServiceBillAmount = this.getAmount(Result[a]);

                var isExists = false;
                var fullDate = monthNames[billDate.getMonth()];

                var totalSettlementAmount = 0;

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
                    var tempBillDate = this.getDate(Result[b]);
                    if (billDate.getMonth() == tempBillDate.getMonth())
                    {
                        totalSettlementAmount += this.getAmount(Result[b]);
                    }
                }

                if (!isExists)
                {
                    listMonths.push({
                        label: fullDate,
                        y: totalSettlementAmount
                    });
                }
            }
        }

        return listMonths;
    },

    getYearlyData: function()
    {
        var Result = this.defaultLineChart;
        var listYears = [];

        for (var a = 0; a < Result.length; a++)
        {
            if (Result[a].Active && (this.taxPayerType == Result[a].TaxPayerTypeName || this.taxPayerType == "all"))
            {
                var billDate = this.getDate(Result[a]);
                var ServiceBillAmount = this.getAmount(Result[a]);

                var isExists = false;
                var fullDate = billDate.getFullYear();

                var totalSettlementAmount = 0;

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
                    var tempBillDate = this.getDate(Result[b]);
                    if (fullDate == tempBillDate.getFullYear())
                    {
                        totalSettlementAmount += this.getAmount(Result[b]);
                    }
                }

                if (!isExists)
                {
                    listYears.push({
                        label: fullDate,
                        y: totalSettlementAmount
                    });
                }
            }
        }

        return listYears;
    },

    getWeeklyData: function()
    {
        var Result = this.defaultLineChart;
        var listWeeks = [];

        for (var a = 0; a < Result.length; a++)
        {
            if (Result[a].Active && (this.taxPayerType == Result[a].TaxPayerTypeName || this.taxPayerType == "all"))
            {
                var billDate = this.getDate(Result[a]);
                var ServiceBillAmount = this.getAmount(Result[a]);

                var isExists = false;
                var fullDate = "Week " + billDate.getWeek();

                var totalSettlementAmount = 0;

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
                    var tempBillDate = this.getDate(Result[b]);
                    if (billDate.getWeek() == tempBillDate.getWeek())
                    {
                        totalSettlementAmount += this.getAmount(Result[b]);
                    }
                }

                if (!isExists)
                {
                    listWeeks.push({
                        label: fullDate,
                        y: totalSettlementAmount
                    });
                }
            }
        }

        return listWeeks;
    },

    getDailyData: function()
    {
        var Result = this.defaultLineChart;
        var listDates = [];

        for (var a = 0; a < Result.length; a++)
        {
            if (Result[a].Active && (this.taxPayerType == Result[a].TaxPayerTypeName || this.taxPayerType == "all"))
            {
                var billDate = this.getDate(Result[a]);
                var ServiceBillAmount = this.getAmount(Result[a]);

                var isExists = false;
                var fullDate = billDate.getDate() + " " + monthNames[billDate.getMonth()];

                var totalSettlementAmount = 0;

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
                    var tempBillDate = this.getDate(Result[b]);
                    var tempFullDate = tempBillDate.getDate() + " " + monthNames[tempBillDate.getMonth()];

                    if (fullDate == tempFullDate)
                    {
                        totalSettlementAmount += this.getAmount(Result[b]);
                    }
                }

                if (!isExists)
                {
                    listDates.push({
                        label: fullDate,
                        y: totalSettlementAmount
                    });
                }
            }
        }

        return listDates;
    },
    
    updateChart: function(newValue, btn)
    {
        if (this.lineChart != null)
        {
            this.lineChart.options.data = [];
            this.lineChart.render();

            this.billType = document.getElementById("bill-type-3").value;
            this.taxPayerType = document.getElementById("tax-payer-type-3").value;

            if (this.billType == "all")
            {
                this.defaultLineChart = serviceBillListResult;
                this.lineChart.options.data.push({
                    type: "line", //try changing to column, area
                    toolTipContent: "{label}: &#8358; {y}",
                    dataPoints: this.getLineChartData(newValue),
                    showInLegend: true,
                    legendText: "service"
                });

                this.defaultLineChart = assessmentBillListResult;
                this.lineChart.options.data.push({
                    type: "line", //try changing to column, area
                    toolTipContent: "{label}: &#8358; {y}",
                    dataPoints: this.getLineChartData(newValue),
                    showInLegend: true,
                    legendText: "assessment"
                });
            }
            else 
            {
                if (this.billType == "service")
                    this.defaultLineChart = serviceBillListResult;
                else
                    this.defaultLineChart = assessmentBillListResult;

                this.lineChart.options.data.push({
                    type: "line", //try changing to column, area
                    toolTipContent: "{label}: &#8358; {y}",
                    dataPoints: this.getLineChartData(newValue),
                    showInLegend: true,
                    legendText: this.billType
                });
            }

            this.lineChart.render();
        }

        var buttons = document.getElementById("chart-3").getElementsByTagName("button");
        for (var a = 0; a < buttons.length; a++)
        {
            buttons[a].classList.remove("btn-selected");
        }

        if (btn == null)
        {
            btn = buttons[1];
        }

        btn.classList.add("btn-selected");
    },
    
    getLineChartData: function(period)
    {
        var newData = [];
        if (period == "yearly")
            newData = this.getYearlyData();
        else if (period == "monthly")
            newData = this.getMonthlyData();
        else if (period == "weekly")
            newData = this.getWeeklyData();
        else if (period == "daily")
            newData = this.getDailyData();
        return newData;
    },
    
    getDate: function(Result)
    {
        if (this.defaultLineChart == serviceBillListResult)
            return new Date(Result.ServiceBillDate);
        else
            return new Date(Result.AssessmentDate);
    },
    
    getAmount: function(Result)
    {
        return Result.SettlementAmount;
    }
};