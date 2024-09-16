using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Repository
{
    public interface IPAYEOutputRepository
    {
        FuncResponse REP_InsertUpdatePayeOutput(PAYEOutput pObjPayeOutput);
        IList<usp_GETPAYEOutput_Result> REP_GetPayeOutputList(PAYEOutput pObjPayeOutput);
        IList<usp_RPT_PAYEOutputAggregationSummary_Result> REP_PAYEOutputAggregationSummary(int TaxYear, int? TaxOfficeID);
        IList<usp_RPT_PAYEOutputAggregationList_Result> REP_PAYEOutputAggregationList(string EmployerRIN, int TaxYear, int? TaxOfficeID);
    }
}
