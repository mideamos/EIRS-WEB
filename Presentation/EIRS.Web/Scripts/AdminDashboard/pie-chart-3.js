var PieChart3 = {
    
    chart: null,
    defaultChart: null,
    chartType: null,
    
    getData: function()
    {
        var Result = this.defaultChart;
//        console.log(Result);
        var listData = [];
        var tempListData = [];
        
        for (var a = 0; a < Result.length; a++)
        {
            if (Result[a].Active)
            {
                var isExists = false;
                
                for (var b = 0; b < tempListData.length; b++)
                {
                    if (tempListData[b].indexLabel == this.compareBy(Result[a]))
                    {
                        isExists = true;
                        break;
                    }
                }

                if (!isExists)
                {
                    tempListData.push({
                        indexLabel: this.compareBy(Result[a]),
                        y: 0
                    });
                }
            }
        }
        
        if (this.chartType == "all")
        {
            listData.push({
                indexLabel: "All Buildings",
                y: Result.length
            });
        }
        else
        {
            for (var a = 0; a < tempListData.length; a++)
            {
                var totalRecord = 0;

                for (var b = 0; b < Result.length; b++)
                {
                    if (Result[b].Active)
                    {
                        if (tempListData[a].indexLabel == this.compareBy(Result[b]))
                        {
                            totalRecord++;
                        }
                    }
                }

                listData.push({
                    indexLabel: tempListData[a].indexLabel,
                    y: totalRecord
                });
            }
        }

        return listData;
    },
    
    updateChart: function()
    {
        if (this.chart != null)
        {
            this.chart.options.data = [];
            this.chart.render();

            this.chartType = document.getElementById("pie-chart-type-3").value;
            
            this.defaultChart = buildingListResult;
            this.chart.options.data.push({
                type: "pie", //try changing to column, area
                showInLegend: true,
                toolTipContent: "{y} - #percent %",
                yValueFormatString: "# Buildings",
                legendText: "{indexLabel}",
                dataPoints: this.getData()
            });

            this.chart.render();
        }
    },
    
    compareBy: function(Result)
    {
        if (this.chartType == "type")
            return Result.BuildingTypeName;
        else if (this.chartType == "completion")
            return Result.BuildingCompletionName;
        else if (this.chartType == "purpose")
            return Result.BuildingPurposeName;
        else if (this.chartType == "ownership")
            return Result.BuildingOwnershipName;
    }
};