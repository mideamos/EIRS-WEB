using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    
    public class LateChargeRepository : ILateChargeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateLateCharge(Late_Charges pObjLateCharge)
        {
            using (_db = new EIRSEntities())
            {
                Late_Charges mObjInsertUpdateLateCharge; //Late Charge Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from lc in _db.Late_Charges
                                       where lc.RevenueStreamID == pObjLateCharge.RevenueStreamID 
                                       && lc.TaxYear == pObjLateCharge.TaxYear && lc.LateChargeID
                                       != pObjLateCharge.LateChargeID
                                       select lc);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Late Charge already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Late Charge
                if (pObjLateCharge.LateChargeID != 0)
                {
                    mObjInsertUpdateLateCharge = (from agrp in _db.Late_Charges
                                                     where agrp.LateChargeID == pObjLateCharge.LateChargeID
                                                     select agrp).FirstOrDefault();

                    if (mObjInsertUpdateLateCharge != null)
                    {
                        mObjInsertUpdateLateCharge.ModifiedBy = pObjLateCharge.ModifiedBy;
                        mObjInsertUpdateLateCharge.ModifiedDate = pObjLateCharge.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateLateCharge = new Late_Charges();
                        mObjInsertUpdateLateCharge.CreatedBy = pObjLateCharge.CreatedBy;
                        mObjInsertUpdateLateCharge.CreatedDate = pObjLateCharge.CreatedDate;
                    }
                }
                else // Else Insert Late Charge
                {
                    mObjInsertUpdateLateCharge = new Late_Charges();
                    mObjInsertUpdateLateCharge.CreatedBy = pObjLateCharge.CreatedBy;
                    mObjInsertUpdateLateCharge.CreatedDate = pObjLateCharge.CreatedDate;
                }

                mObjInsertUpdateLateCharge.RevenueStreamID = pObjLateCharge.RevenueStreamID;
                mObjInsertUpdateLateCharge.TaxYear = pObjLateCharge.TaxYear;
                mObjInsertUpdateLateCharge.Penalty = pObjLateCharge.Penalty;
                mObjInsertUpdateLateCharge.Interest = pObjLateCharge.Interest;
                mObjInsertUpdateLateCharge.Active = pObjLateCharge.Active;

                if (pObjLateCharge.LateChargeID == 0)
                {
                    _db.Late_Charges.Add(mObjInsertUpdateLateCharge);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjLateCharge.LateChargeID == 0)
                        mObjFuncResponse.Message = "Late Charge Added Successfully";
                    else
                        mObjFuncResponse.Message = "Late Charge Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjLateCharge.LateChargeID == 0)
                        mObjFuncResponse.Message = "Late Charge Addition Failed";
                    else
                        mObjFuncResponse.Message = "Late Charge Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetLateChargeList_Result> REP_GetLateChargeList(Late_Charges pObjLateCharge)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLateChargeList(pObjLateCharge.LateChargeID, pObjLateCharge.RevenueStreamID).ToList();
            }
        }

        public usp_GetLateChargeList_Result REP_GetLateChargeDetails(Late_Charges pObjLateCharge)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLateChargeList(pObjLateCharge.LateChargeID, pObjLateCharge.RevenueStreamID).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateStatus(Late_Charges pObjLateCharge)
        {
            using (_db = new EIRSEntities())
            {
                Late_Charges mObjInsertUpdateLateCharge; //Late Charge Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load LateCharge
                if (pObjLateCharge.LateChargeID != 0)
                {
                    mObjInsertUpdateLateCharge = (from lc in _db.Late_Charges
                                                     where lc.LateChargeID == pObjLateCharge.LateChargeID
                                                     select lc).FirstOrDefault();

                    if (mObjInsertUpdateLateCharge != null)
                    {
                        mObjInsertUpdateLateCharge.Active = !mObjInsertUpdateLateCharge.Active;
                        mObjInsertUpdateLateCharge.ModifiedBy = pObjLateCharge.ModifiedBy;
                        mObjInsertUpdateLateCharge.ModifiedDate = pObjLateCharge.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Late Charge Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetLateChargeList(pObjLateCharge.LateChargeID, pObjLateCharge.RevenueStreamID).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Late Charge Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
