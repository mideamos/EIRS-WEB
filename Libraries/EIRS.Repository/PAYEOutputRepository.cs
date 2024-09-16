using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Repository
{

    public class PAYEOutputRepository : IPAYEOutputRepository
    {
        ERASDWEntities _db;
        public FuncResponse REP_InsertUpdatePayeOutput(PAYEOutput pObjPayeOutput)
        {
            using (_db = new ERASDWEntities())
            {
                PAYEOutput mObjPAYEOutput;
                FuncResponse mObjResponse = new FuncResponse();
                var vDuplicate = (from py in _db.PAYEOutputs
                                  where py.PAYEOutputID != pObjPayeOutput.PAYEOutputID
      && py.Employee_Rin == pObjPayeOutput.Employee_Rin && py.Employer_Rin == pObjPayeOutput.Employer_Rin &&
     py.AssessmentYear == pObjPayeOutput.AssessmentYear && py.Assessment_Month == pObjPayeOutput.Assessment_Month
                                  select py);
                if (vDuplicate.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "PAYE Output Already Exists";
                    return mObjResponse;

                }
                if (pObjPayeOutput.PAYEOutputID != 0)
                {
                    mObjPAYEOutput = (from py in _db.PAYEOutputs where py.PAYEOutputID == pObjPayeOutput.PAYEOutputID select py).FirstOrDefault();
                    if (mObjPAYEOutput != null)
                    {
                        mObjPAYEOutput.ModifiedBy = pObjPayeOutput.ModifiedBy;
                        mObjPAYEOutput.ModifiedDate = pObjPayeOutput.ModifiedDate;
                    }
                    else
                    {
                        mObjPAYEOutput = new PAYEOutput();
                        mObjPAYEOutput.CreatedBy = pObjPayeOutput.CreatedBy;
                        mObjPAYEOutput.CreatedDate = pObjPayeOutput.CreatedDate;
                    }
                }
                else
                {
                    mObjPAYEOutput = new PAYEOutput()
                    {
                        CreatedBy = pObjPayeOutput.CreatedBy,
                        CreatedDate = pObjPayeOutput.CreatedDate
                    };
                }
                mObjPAYEOutput.Transaction_Date = pObjPayeOutput.Transaction_Date;
                mObjPAYEOutput.Employee_Rin = pObjPayeOutput.Employee_Rin;
                mObjPAYEOutput.Employer_Rin = pObjPayeOutput.Employer_Rin;
                mObjPAYEOutput.AssessmentYear = pObjPayeOutput.AssessmentYear;
                mObjPAYEOutput.Assessment_Month = pObjPayeOutput.Assessment_Month;
                mObjPAYEOutput.Monthly_CRA = pObjPayeOutput.Monthly_CRA;
                mObjPAYEOutput.Monthly_Gross = pObjPayeOutput.Monthly_Gross;
                mObjPAYEOutput.Monthly_ValidatedNHF = pObjPayeOutput.Monthly_ValidatedNHF;
                mObjPAYEOutput.Monthly_ValidatedNHIS = pObjPayeOutput.Monthly_ValidatedNHIS;
                mObjPAYEOutput.Monthly_ValidatedPension = pObjPayeOutput.Monthly_ValidatedPension;
                mObjPAYEOutput.Monthly_TaxFreePay = pObjPayeOutput.Monthly_TaxFreePay;
                mObjPAYEOutput.Monthly_ChargeableIncome = pObjPayeOutput.Monthly_ChargeableIncome;
                mObjPAYEOutput.Monthly_Tax = pObjPayeOutput.Monthly_Tax;
                mObjPAYEOutput.Tax_Office = pObjPayeOutput.Tax_Office;

                if (pObjPayeOutput.PAYEOutputID == 0)
                {
                    _db.PAYEOutputs.Add(mObjPAYEOutput);
                }
                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                    if (pObjPayeOutput.PAYEOutputID == 0)
                        mObjResponse.Message = "PAYE Output Added Successfully";
                    else
                        mObjResponse.Message = "PAYE Output Updated Successfully";
                }
                catch (Exception ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Exception = ex;
                    if (pObjPayeOutput.PAYEOutputID == 0)
                        mObjResponse.Message = "PAYE Output Addition Failed";
                    else
                        mObjResponse.Message = "PAYE Output Updation Failed";
                }

                return mObjResponse;
            }
        }

        public IList<usp_GETPAYEOutput_Result> REP_GetPayeOutputList(PAYEOutput pObjPayeOutput)
        {
            using (_db = new ERASDWEntities())
            {
                return _db.usp_GETPAYEOutput(pObjPayeOutput.PAYEOutputID, pObjPayeOutput.Employee_Rin).ToList();
            }
        }

        public IList<usp_RPT_PAYEOutputAggregationSummary_Result> REP_PAYEOutputAggregationSummary(int TaxYear, int? TaxOfficeID)
        {
            using (_db = new ERASDWEntities())
            {
                return _db.usp_RPT_PAYEOutputAggregationSummary(TaxYear, TaxOfficeID).ToList();
            }
        }

        public IList<usp_RPT_PAYEOutputAggregationList_Result> REP_PAYEOutputAggregationList(string EmployerRIN, int TaxYear, int? TaxOfficeID)
        {
            using (_db = new ERASDWEntities())
            {
                return _db.usp_RPT_PAYEOutputAggregationList(EmployerRIN, TaxYear, TaxOfficeID).ToList();
            }
        }

    }
}
