using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIRS.Repository
{
    public class DAOutputRepository : IDAOutputRepository
    {
        ERASDWEntities _db;

        public FuncResponse REP_InsertUpdateDAOutput(DAOutput pObjDAOutput)
        {
            using (_db = new ERASDWEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();
                DAOutput mObjDAOutput;
                var vDuplicate = (from py in _db.DAOutputs
                                  where py.DAOutputID != pObjDAOutput.DAOutputID
                                  && py.Taxpayer_RIN == pObjDAOutput.Taxpayer_RIN && py.Taxpayer_TIN == pObjDAOutput.Taxpayer_TIN &&
                                  py.Assessment_Year == pObjDAOutput.Assessment_Year
                                  select py);

                if (vDuplicate.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "DA Output Already Exists";
                    return mObjResponse;

                }
                if (pObjDAOutput.DAOutputID != 0)
                {
                    mObjDAOutput = (from py in _db.DAOutputs where py.DAOutputID == pObjDAOutput.DAOutputID select py).FirstOrDefault();
                    if (mObjDAOutput != null)
                    {
                        mObjDAOutput.ModifiedBy = pObjDAOutput.ModifiedBy;
                        mObjDAOutput.ModifiedDate = pObjDAOutput.ModifiedDate;
                    }
                    else
                    {
                        mObjDAOutput = new DAOutput();
                        mObjDAOutput.CreatedBy = pObjDAOutput.CreatedBy;
                        mObjDAOutput.CreatedDate = pObjDAOutput.CreatedDate;
                    }
                }
                else
                {
                    mObjDAOutput = new DAOutput()
                    {
                        CreatedBy = pObjDAOutput.CreatedBy,
                        CreatedDate = pObjDAOutput.CreatedDate
                    };
                }

                mObjDAOutput.Assessment_Year = pObjDAOutput.Assessment_Year;
                mObjDAOutput.Business_RIN = pObjDAOutput.Business_RIN;
                mObjDAOutput.Business_TIN = pObjDAOutput.Business_TIN;
                mObjDAOutput.Employee_PAYE_Contribution = pObjDAOutput.Employee_PAYE_Contribution;
                mObjDAOutput.Life_Assurance = pObjDAOutput.Life_Assurance;
                mObjDAOutput.NHF_Declared = pObjDAOutput.NHF_Declared;
                mObjDAOutput.NHIS_Declared = pObjDAOutput.NHIS_Declared;
                mObjDAOutput.PAYE_Income = pObjDAOutput.PAYE_Income;
                mObjDAOutput.PAYE_NHF = pObjDAOutput.PAYE_NHF;
                mObjDAOutput.PAYE_NHIS = pObjDAOutput.PAYE_NHIS;
                mObjDAOutput.PAYE_Pension = pObjDAOutput.PAYE_Pension;
                mObjDAOutput.Pension_Contribution_Declared = pObjDAOutput.Pension_Contribution_Declared;
                mObjDAOutput.Share_of_Assessment = pObjDAOutput.Share_of_Assessment;
                mObjDAOutput.Taxpayer_RIN = pObjDAOutput.Taxpayer_RIN;
                mObjDAOutput.Taxpayer_TIN = pObjDAOutput.Taxpayer_TIN;
                mObjDAOutput.tax_office = pObjDAOutput.tax_office;
                mObjDAOutput.Total_Income = pObjDAOutput.Total_Income;
                mObjDAOutput.transaction_date = pObjDAOutput.transaction_date;

                if (pObjDAOutput.DAOutputID == 0)
                {
                    _db.DAOutputs.Add(mObjDAOutput);
                }
                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                    if (pObjDAOutput.DAOutputID == 0)
                    {
                        mObjResponse.Message = "DA Output Added Successfully";
                    }
                    else
                    {
                        mObjResponse.Message = "DA Output Updated Successfully";
                    }
                }
                catch (Exception ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Exception = ex;
                    if (pObjDAOutput.DAOutputID == 0)
                    {
                        mObjResponse.Message = "DA Output Addition Failed";
                    }
                    else
                    {
                        mObjResponse.Message = "DA Output Updation Failed";
                    }
                }

                return mObjResponse;
            }
        }

        public IList<usp_GetDAOutputList_Result> REP_GetOutputList(DAOutput pObjDAOutput)
        {
            using (_db = new ERASDWEntities())
            {
                return _db.usp_GetDAOutputList(pObjDAOutput.DAOutputID, pObjDAOutput.Taxpayer_RIN).ToList();
            }
        }
    }
}
