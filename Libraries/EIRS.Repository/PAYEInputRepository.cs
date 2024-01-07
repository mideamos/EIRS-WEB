using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class PAYEInputRepository : IPAYEInputRepository
    {
        ERASDWEntities _db;
        //VP_T-ERAS-8_PAYEInput

        /// <summary>
        /// Function Are Used To Insert New Data And Update Existing data in PAYEInput Table
        /// </summary>
        /// <param name="pObjPAYEInput"></param>
        /// <returns></returns>
        public FuncResponse REP_InertUpdatePAYEInput(PAYEInput pObjPAYEInput)
        {
            using (_db = new ERASDWEntities())
            {
                PAYEInput mObjInsertUpdatePAYEInput;

                FuncResponse mObjFuncResponse = new FuncResponse();

                var vDuplicateCheck = (from paye in _db.PAYEInputs
                                       where paye.Employer_RIN == pObjPAYEInput.Employer_RIN &&
                                       paye.Employee_RIN == pObjPAYEInput.Employee_RIN &&
                                       paye.Assessment_Year == pObjPAYEInput.Assessment_Year
                                       && paye.PAYEinputID != pObjPAYEInput.PAYEinputID
                                       select paye);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "PAYE Input already exists";
                    return mObjFuncResponse;
                }

                if(pObjPAYEInput.PAYEinputID!=0)
                {
                    mObjInsertUpdatePAYEInput = (from paye in _db.PAYEInputs
                                                 where paye.PAYEinputID == pObjPAYEInput.PAYEinputID
                                                 select paye).FirstOrDefault();

                    if(mObjInsertUpdatePAYEInput!=null)
                    {
                        mObjInsertUpdatePAYEInput.ModifiedBy = pObjPAYEInput.ModifiedBy;
                        mObjInsertUpdatePAYEInput.ModifiedDate = pObjPAYEInput.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdatePAYEInput = new PAYEInput();
                        mObjInsertUpdatePAYEInput.CreatedBy = pObjPAYEInput.CreatedBy;
                        mObjInsertUpdatePAYEInput.CreatedDate = pObjPAYEInput.CreatedDate;
                    }
                }
                else
                {
                    mObjInsertUpdatePAYEInput = new PAYEInput();
                    mObjInsertUpdatePAYEInput.CreatedBy = pObjPAYEInput.CreatedBy;
                    mObjInsertUpdatePAYEInput.CreatedDate = pObjPAYEInput.CreatedDate;
                }

                mObjInsertUpdatePAYEInput.TransactionDate = pObjPAYEInput.TransactionDate;
                mObjInsertUpdatePAYEInput.Employer_RIN = pObjPAYEInput.Employer_RIN;
                mObjInsertUpdatePAYEInput.Employee_RIN = pObjPAYEInput.Employee_RIN;
                mObjInsertUpdatePAYEInput.Assessment_Year = pObjPAYEInput.Assessment_Year;
                mObjInsertUpdatePAYEInput.Start_Month = pObjPAYEInput.Start_Month;
                mObjInsertUpdatePAYEInput.End_Month = pObjPAYEInput.End_Month;
                mObjInsertUpdatePAYEInput.Annual_Basic = pObjPAYEInput.Annual_Basic;
                mObjInsertUpdatePAYEInput.Annual_Rent = pObjPAYEInput.Annual_Rent;
                mObjInsertUpdatePAYEInput.Annual_Transport = pObjPAYEInput.Annual_Transport;
                mObjInsertUpdatePAYEInput.Annual_Utility = pObjPAYEInput.Annual_Utility;
                mObjInsertUpdatePAYEInput.Annual_Meal = pObjPAYEInput.Annual_Meal;
                mObjInsertUpdatePAYEInput.Other_Allowances_Annual = pObjPAYEInput.Other_Allowances_Annual;
                mObjInsertUpdatePAYEInput.Leave_Transport_Grant_Annual = pObjPAYEInput.Leave_Transport_Grant_Annual;
                mObjInsertUpdatePAYEInput.pension_contribution_declared = pObjPAYEInput.pension_contribution_declared;
                mObjInsertUpdatePAYEInput.nhf_contribution_declared = pObjPAYEInput.nhf_contribution_declared;
                mObjInsertUpdatePAYEInput.nhis_contribution_declared = pObjPAYEInput.nhis_contribution_declared;
                mObjInsertUpdatePAYEInput.Tax_Office = pObjPAYEInput.Tax_Office;

                if(pObjPAYEInput.PAYEinputID==0)
                {
                    _db.PAYEInputs.Add(mObjInsertUpdatePAYEInput);
                }
                try
                {
                    _db.SaveChanges();
                    
                    mObjFuncResponse.Success = true;
                    if (pObjPAYEInput.PAYEinputID == 0)
                    {
                        mObjFuncResponse.Message = "PAYE Input Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "PAYE Input Updated Successfully";
                    }
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjPAYEInput.PAYEinputID == 0)
                    {
                        mObjFuncResponse.Message = "PAYE Input Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "PAYE Input Updation Failed";
                    } 
                }
                return mObjFuncResponse;
            }
        }

        /// <summary>
        /// Function Are Used To gave List Of Data in PAYEInput List
        /// </summary>
        /// <param name="pObjPAYEInput"></param>
        /// <returns></returns>
        public IList<usp_GetPAYEInputList_Result> REP_GetPAYEInputList(PAYEInput pObjPAYEInput)
        {
            using (_db = new ERASDWEntities())
            {
                return _db.usp_GetPAYEInputList(pObjPAYEInput.PAYEinputID, pObjPAYEInput.Employer_RIN).ToList();
            }
        }
    }
}
