using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIRS.Repository
{
    public class ScratchCardDealerRespository : IScratchCardDealerRespository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateScratchCardDealer(Scratch_Card_Dealers pObjScratchCardDealer)
        {
            using (_db = new EIRSEntities())
            {
                Scratch_Card_Dealers mObjInsertUpdateScratchCardDealer;
                FuncResponse mObjFuncRespsonse = new FuncResponse();
                if (pObjScratchCardDealer == null)
                {
                    mObjFuncRespsonse.Success = false;
                    mObjFuncRespsonse.Message = "No Data";
                    return mObjFuncRespsonse;
                }
                else
                {
                    var vduplicateScratchCardDealer = (from ScrDealer in _db.Scratch_Card_Dealers where ScrDealer.ScratchCardDealerName == pObjScratchCardDealer.ScratchCardDealerName && ScrDealer.ScratchCardDealerID != pObjScratchCardDealer.ScratchCardDealerID select ScrDealer);
                    if (vduplicateScratchCardDealer != null && vduplicateScratchCardDealer.Count() > 0)
                    {
                        mObjFuncRespsonse.Success = false;
                        mObjFuncRespsonse.Message = "Scratch Card Dealer already exists";
                        return mObjFuncRespsonse;
                    }
                    else
                    {
                        // Update Scratch Card Dealer
                        if (pObjScratchCardDealer.ScratchCardDealerID != 0)
                        {
                            mObjInsertUpdateScratchCardDealer = (from ScrDealer in _db.Scratch_Card_Dealers where (ScrDealer.ScratchCardDealerID == pObjScratchCardDealer.ScratchCardDealerID) select ScrDealer).FirstOrDefault();

                            if (mObjInsertUpdateScratchCardDealer != null)
                            {
                                mObjInsertUpdateScratchCardDealer.ModifiedBy = pObjScratchCardDealer.ModifiedBy;
                                mObjInsertUpdateScratchCardDealer.ModifiedDate = pObjScratchCardDealer.ModifiedDate;
                            }
                            else
                            {
                                mObjInsertUpdateScratchCardDealer = new Scratch_Card_Dealers()
                                {
                                    CreatedBy = pObjScratchCardDealer.CreatedBy,
                                    CreatedDate = pObjScratchCardDealer.CreatedDate
                                };
                            }
                        }
                        // Add Scratch Card Dealer
                        else
                        {
                            mObjInsertUpdateScratchCardDealer = new Scratch_Card_Dealers()
                            {
                                CreatedBy = pObjScratchCardDealer.CreatedBy,
                                CreatedDate = pObjScratchCardDealer.CreatedDate,
                            };
                        }
                        mObjInsertUpdateScratchCardDealer.Active = pObjScratchCardDealer.Active;
                        mObjInsertUpdateScratchCardDealer.ScratchCardDealerName = pObjScratchCardDealer.ScratchCardDealerName;
                        mObjInsertUpdateScratchCardDealer.CompanyID = pObjScratchCardDealer.CompanyID;
                        mObjInsertUpdateScratchCardDealer.AgreedCommissionPercentage = pObjScratchCardDealer.AgreedCommissionPercentage;
                        mObjInsertUpdateScratchCardDealer.DealerTypeID = pObjScratchCardDealer.DealerTypeID;

                        if (pObjScratchCardDealer.ScratchCardDealerID == 0)
                        {
                            _db.Scratch_Card_Dealers.Add(mObjInsertUpdateScratchCardDealer);
                        }

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncRespsonse.Success = true;
                            mObjFuncRespsonse.Message = pObjScratchCardDealer.ScratchCardDealerID == 0 ? "Added Successfully" : "Updated Successfully";
                        }
                        catch (Exception ex)
                        {
                            mObjFuncRespsonse.Success = false;
                            mObjFuncRespsonse.Exception = ex;
                            mObjFuncRespsonse.Message = pObjScratchCardDealer.ScratchCardDealerID == 0 ? "Addition Failed" : "Updation Failed";
                        }
                        return mObjFuncRespsonse;
                    }
                }
            }
        }


        public IList<usp_GetScratchCardDealerList_Result> REP_GetScratchCardDealerList(Scratch_Card_Dealers pObjScratchCardDealer)
        {
            using (_db = new EIRSEntities())
            {
                var vlstScratchCardDealer = _db.usp_GetScratchCardDealerList(pObjScratchCardDealer.ScratchCardDealerID, pObjScratchCardDealer.ScratchCardDealerName, pObjScratchCardDealer.ScratchCardDealerIds, pObjScratchCardDealer.intStatus, pObjScratchCardDealer.IncludeScratchCardDealerIds, pObjScratchCardDealer.ExcludeScratchCardDealerIds, pObjScratchCardDealer.CompanyID).ToList();
                return vlstScratchCardDealer;
            }
        }

        public usp_GetScratchCardDealerList_Result REP_GetScratchCardDealerDetails(Scratch_Card_Dealers pObjScratchCardDealer)
        {
            using (_db = new EIRSEntities())
            {
                var vScratchCardDealerDetails = _db.usp_GetScratchCardDealerList(pObjScratchCardDealer.ScratchCardDealerID, pObjScratchCardDealer.ScratchCardDealerName, pObjScratchCardDealer.ScratchCardDealerIds, pObjScratchCardDealer.intStatus, pObjScratchCardDealer.IncludeScratchCardDealerIds, pObjScratchCardDealer.ExcludeScratchCardDealerIds, pObjScratchCardDealer.CompanyID).FirstOrDefault();
                return vScratchCardDealerDetails;
            }
        }

        public FuncResponse REP_UpdateStatus(Scratch_Card_Dealers pObjScratchCardDealer)
        {
            using (_db = new EIRSEntities())
            {
                Scratch_Card_Dealers mObjInsertUpdateScratchCardDealer; //Scratch Card Dealer Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Update Load Scratch Card Dealer
                if (pObjScratchCardDealer.ScratchCardDealerID != 0)
                {
                    mObjInsertUpdateScratchCardDealer = (from NotifType in _db.Scratch_Card_Dealers
                                                          where NotifType.ScratchCardDealerID == pObjScratchCardDealer.ScratchCardDealerID
                                                          select NotifType).FirstOrDefault();

                    if (mObjInsertUpdateScratchCardDealer != null)
                    {
                        mObjInsertUpdateScratchCardDealer.Active = !mObjInsertUpdateScratchCardDealer.Active;
                        mObjInsertUpdateScratchCardDealer.ModifiedBy = pObjScratchCardDealer.ModifiedBy;
                        mObjInsertUpdateScratchCardDealer.ModifiedDate = pObjScratchCardDealer.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Scratch Card Dealer Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetScratchCardDealerList(0, pObjScratchCardDealer.ScratchCardDealerName, pObjScratchCardDealer.ScratchCardDealerIds, pObjScratchCardDealer.intStatus, pObjScratchCardDealer.IncludeScratchCardDealerIds, pObjScratchCardDealer.ExcludeScratchCardDealerIds, pObjScratchCardDealer.CompanyID).ToList();
                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Scratch Card Dealer Updation Failed";
                        }
                    }
                }
                return mObjFuncResponse;
            }
        }

    }
}