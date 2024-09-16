using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.BLL
{
   public class BLPAYEOutput
    {
        IPAYEOutputRepository _PayeOutputRepository;
        public BLPAYEOutput()
        {
            _PayeOutputRepository = new PAYEOutputRepository();
        }
        public FuncResponse BL_InsertUpdatePayeOutput(PAYEOutput pObjPayeOutput)
        {
            return _PayeOutputRepository.REP_InsertUpdatePayeOutput(pObjPayeOutput);
        }
        public IList<usp_GETPAYEOutput_Result> BL_GetPayeOutputList(PAYEOutput pObjPayeOutput)
        {
            return _PayeOutputRepository.REP_GetPayeOutputList(pObjPayeOutput);
        }

        public IList<usp_RPT_PAYEOutputAggregationSummary_Result> BL_PAYEOutputAggregationSummary(int TaxYear, int? TaxOfficeID)
        {
            return _PayeOutputRepository.REP_PAYEOutputAggregationSummary(TaxYear, TaxOfficeID);
        }

        public IList<usp_RPT_PAYEOutputAggregationList_Result> BL_PAYEOutputAggregationList(string EmployerRIN, int TaxYear, int? TaxOfficeID)
        {
            return _PayeOutputRepository.REP_PAYEOutputAggregationList(EmployerRIN, TaxYear, TaxOfficeID);
        }

    }
}
