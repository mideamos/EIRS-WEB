var PieChart1 = {
    
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
                if (this.chartType != "all")
                {
                    for (var b = 0; b < tempListData.length; b++)
                    {
                        if (tempListData[b].indexLabel == this.compareBy(Result[a]))
                        {
                            isExists = true;
                            break;
                        }
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
                indexLabel: "All Businesses",
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

            this.chartType = document.getElementById("pie-chart-type-1").value;
            
            this.defaultChart = businessesListResult;
            this.chart.options.data.push({
                type: "pie", //try changing to column, area
                showInLegend: true,
                toolTipContent: "{y} - #percent %",
                yValueFormatString: "# Businesses",
                legendText: "{indexLabel}",
                dataPoints: this.getData()
            });

            this.chart.render();
        }
    },
    
    compareBy: function(Result)
    {
        if (this.chartType == "type")
            return Result.BusinessTypeName;
        else if (this.chartType == "category")
            return Result.BusinessCategoryName;
        else if (this.chartType == "structure")
            return Result.BusinessStructureName;
        else if (this.chartType == "sector")
            return Result.BusinessSectorName;
        else if (this.chartType == "sub_sector")
            return Result.BusinessSubSectorName;
        else if (this.chartType == "operations")
            return Result.BusinessOperationName;
    }
};