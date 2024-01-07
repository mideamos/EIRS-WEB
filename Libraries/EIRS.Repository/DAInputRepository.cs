using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Repository
{
  public class DAInputRepository: IDAInputRepository
    {

        ERASDWEntities _db;
        //PD_T-ERAS-10-DAInput


        /// <summary>
        /// Function Are Used To gave List Of Data in DAInput List
        /// </summary>
        /// <param name="pObjDAInput"></param>
        /// <returns></returns>
        public IList<usp_GetDAInputList_Result> REP_GetDAInputList(DAInput pObjDAInput)
        {
            using (_db = new ERASDWEntities())
            {
                return _db.usp_GetDAInputList(pObjDAInput.DAInputID, pObjDAInput.Taxpayer_RIN).ToList();
            }
        }


        /// <summary>
        /// Function Are Used To Insert New Data And Update Existing data in DAInput Table
        /// </summary>
        /// <param name="pObjDAInput"></param>
        /// <returns></returns>
        public FuncResponse REP_InertUpdateDAInput(DAInput pObjDAInput)
        {
            using (_db = new ERASDWEntities())
            {
                DAInput mObjInsertUpdateDAInput;

                FuncResponse mObjFuncResponse = new FuncResponse();

                var vDuplicateCheck = (from DA in _db.DAInputs
                                       where DA.Taxpayer_RIN == pObjDAInput.Taxpayer_RIN &&
                                       DA.Taxpayer_TIN == pObjDAInput.Taxpayer_TIN &&
                                       DA.Assessment_Year == pObjDAInput.Assessment_Year
                                       && DA.DAInputID != pObjDAInput.DAInputID
                                       select DA);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "DA Input already exists";
                    return mObjFuncResponse;
                }

                if (pObjDAInput.DAInputID != 0)
                {
                    mObjInsertUpdateDAInput = (from DA in _db.DAInputs
                                                 where DA.DAInputID == pObjDAInput.DAInputID
                                                 select DA).FirstOrDefault();

                    if (mObjInsertUpdateDAInput != null)
                    {
                        mObjInsertUpdateDAInput.ModifiedBy = pObjDAInput.ModifiedBy;
                        mObjInsertUpdateDAInput.ModifiedDate = pObjDAInput.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateDAInput = new DAInput();
                        mObjInsertUpdateDAInput.CreatedBy = pObjDAInput.CreatedBy;
                        mObjInsertUpdateDAInput.CreatedDate = pObjDAInput.CreatedDate;
                    }
                }
                else
                {
                    mObjInsertUpdateDAInput = new DAInput();
                    mObjInsertUpdateDAInput.CreatedBy = pObjDAInput.CreatedBy;
                    mObjInsertUpdateDAInput.CreatedDate = pObjDAInput.CreatedDate;
                }

                mObjInsertUpdateDAInput.transaction_date = pObjDAInput.transaction_date;
                mObjInsertUpdateDAInput.Taxpayer_RIN = pObjDAInput.Taxpayer_RIN;
                mObjInsertUpdateDAInput.Taxpayer_TIN = pObjDAInput.Taxpayer_TIN;
                mObjInsertUpdateDAInput.Start_Month = pObjDAInput.Start_Month;
                mObjInsertUpdateDAInput.Share_of_Partnership_Profit = pObjDAInput.Share_of_Partnership_Profit;
                mObjInsertUpdateDAInput.Salaries = pObjDAInput.Salaries;
                mObjInsertUpdateDAInput.Rent = pObjDAInput.Rent;
                mObjInsertUpdateDAInput.Pension_Contribution_Declared = pObjDAInput.Pension_Contribution_Declared;
                mObjInsertUpdateDAInput.PAYE_Pension = pObjDAInput.PAYE_Pension;
                mObjInsertUpdateDAInput.PAYE_NHIS = pObjDAInput.PAYE_NHIS;
                mObjInsertUpdateDAInput.PAYE_NHF = pObjDAInput.PAYE_NHF;
                mObjInsertUpdateDAInput.PAYE_Income = pObjDAInput.PAYE_Income;
                mObjInsertUpdateDAInput.Other_Incomes_Not_Included = pObjDAInput.Other_Incomes_Not_Included;
                mObjInsertUpdateDAInput.NHIS_Declared = pObjDAInput.NHIS_Declared;
                mObjInsertUpdateDAInput.NHF__Declared = pObjDAInput.NHF__Declared;
                mObjInsertUpdateDAInput.Life_Assurance = pObjDAInput.Life_Assurance;
                mObjInsertUpdateDAInput.Interest_on_Discount = pObjDAInput.Interest_on_Discount;
                mObjInsertUpdateDAInput.Gratuities = pObjDAInput.Gratuities;
                mObjInsertUpdateDAInput.Employee_PAYE_Contribution = pObjDAInput.Employee_PAYE_Contribution;
                mObjInsertUpdateDAInput.Commissions_Recieved = pObjDAInput.Commissions_Recieved;
                mObjInsertUpdateDAInput.Bonuses = pObjDAInput.Bonuses;
                mObjInsertUpdateDAInput.Annuity = pObjDAInput.Annuity;
                mObjInsertUpdateDAInput.Assessment_Year = pObjDAInput.Assessment_Year;
                mObjInsertUpdateDAInput.Business_RIN = pObjDAInput.Business_RIN;
                mObjInsertUpdateDAInput.Business_TIN = pObjDAInput.Business_TIN;
                mObjInsertUpdateDAInput.tax_office = pObjDAInput.tax_office;

                if (pObjDAInput.DAInputID == 0)
                {
                    _db.DAInputs.Add(mObjInsertUpdateDAInput);
                }
                try
                {
                    _db.SaveChanges();

                    mObjFuncResponse.Success = true;
                    if (pObjDAInput.DAInputID == 0)
                    {
                        mObjFuncResponse.Message = "DA Input Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "DA Input Updated Successfully";
                    }
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjDAInput.DAInputID == 0)
                    {
                        mObjFuncResponse.Message = "DA Input Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "DA Input Updation Failed";
                    }
                }
                return mObjFuncResponse;
            }
        }
    }
}
