var LineChart4 = {
    
    lineChart: null,
    defaultLineChart: null,
    billType: null,
    taxPayerType: null,
    agedGroup: null,
    
    getData: function()
    {
        var Result = this.defaultLineChart;
//        console.log(Result);
        var listMonths = [];

        for (var a = 0; a < Result.length; a++)
        {
            if (Result[a].Active && (this.taxPayerType == Result[a].TaxPayerTypeName || this.taxPayerType == "all") && this.isDatePassed(Result[a]))
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
    
    updateChart: function()
    {
        if (this.lineChart != null)
        {
            this.lineChart.options.data = [];
            this.lineChart.render();

            this.billType = document.getElementById("bill-type-4").value;
            this.taxPayerType = document.getElementById("tax-payer-type-4").value;
            this.agedGroup = parseInt(document.getElementById("aged-group-4").value);

            if (this.billType == "all")
            {
                this.defaultLineChart = serviceBillListResult;
                this.lineChart.options.data.push({
                    type: "line", //try changing to column, area
                    toolTipContent: "{label}: &#8358; {y}",
                    dataPoints: this.getLineChartData(),
                    showInLegend: true,
                    legendText: "service"
                });

                this.defaultLineChart = assessmentBillListResult;
                this.lineChart.options.data.push({
                    type: "line", //try changing to column, area
                    toolTipContent: "{label}: &#8358; {y}",
                    dataPoints: this.getLineChartData(),
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
                    dataPoints: this.getLineChartData(),
                    showInLegend: true,
                    legendText: this.billType
                });
            }

            this.lineChart.render();
        }
    },
    
    getLineChartData: function()
    {
        return this.getData();
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
    },
    
    isDatePassed: function(Result)
    {
        var startingDate;
        var endingDate;
        
        switch (this.agedGroup)
        {
            case 0:
                startingDate = 0;
                endingDate = 3;
                break;
                
            case 1:
                startingDate = 3;
                endingDate = 6;
                break;
                
            case 2:
                startingDate = 6;
                endingDate = 12;
                break;
            
            case 3:
                startingDate = 12;
                endingDate = 13;
                break;
        }
        
        var settlementDate = new Date(Result.SettlementDueDate);
        
        var startingSettlementDate = new Date(Result.SettlementDueDate);
        var endingSettlementDate = new Date(Result.SettlementDueDate);
        
        startingSettlementDate.setMonth(settlementDate.getMonth() - startingDate);
        endingSettlementDate.setMonth(settlementDate.getMonth() - endingDate);
        
        return settlementDate <= startingSettlementDate && settlementDate >= endingSettlementDate;
    }
};