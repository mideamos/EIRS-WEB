using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIRS.Repository
{
    public class SettlementStatusRepository : ISettlementStatusRepository
    {
        EIRSEntities _db;
        public FuncResponse REP_InsertUpdateSettlementStatus(Settlement_Status pObjSettlementStatus)
        {
            using (_db = new EIRSEntities())
            {
                Settlement_Status mObjInsertUpdateSettlementStatus;
                FuncResponse mObjFuncRespsonse = new FuncResponse();

                if (pObjSettlementStatus == null)
                {
                    mObjFuncRespsonse.Success = false;
                    mObjFuncRespsonse.Message = "No Data";
                    return mObjFuncRespsonse;
                }
                else
                {
                    var vduplicateSettlementStatus = (from settst in _db.Settlement_Status where settst.SettlementStatusName == pObjSettlementStatus.SettlementStatusName && settst.SettlementStatusID != pObjSettlementStatus.SettlementStatusID select settst);
                    if (vduplicateSettlementStatus != null && vduplicateSettlementStatus.Count() > 0)
                    {
                        mObjFuncRespsonse.Success = false;
                        mObjFuncRespsonse.Message = "Settlement Status already exists";
                        return mObjFuncRespsonse;
                    }
                    else
                    {
                        // Update Settlement Status
                        if (pObjSettlementStatus.SettlementStatusID != 0)
                        {
                            mObjInsertUpdateSettlementStatus = (from settSt in _db.Settlement_Status where (settSt.SettlementStatusID == pObjSettlementStatus.SettlementStatusID) select settSt).FirstOrDefault();

                            if (mObjInsertUpdateSettlementStatus != null)
                            {
                                mObjInsertUpdateSettlementStatus.ModifiedBy = pObjSettlementStatus.ModifiedBy;
                                mObjInsertUpdateSettlementStatus.ModifiedDate = pObjSettlementStatus.ModifiedDate;
                            }
                            else
                            {
                                mObjInsertUpdateSettlementStatus = new Settlement_Status()
                                {
                                    CreatedBy = pObjSettlementStatus.CreatedBy,
                                    CreatedDate = pObjSettlementStatus.CreatedDate
                                };
                            }
                        }
                        // Add Settlement Status
                        else
                        {
                            mObjInsertUpdateSettlementStatus = new Settlement_Status()
                            {
                                CreatedBy = pObjSettlementStatus.CreatedBy,
                                CreatedDate = pObjSettlementStatus.CreatedDate,
                            };
                        }
                        mObjInsertUpdateSettlementStatus.Active = pObjSettlementStatus.Active;
                        mObjInsertUpdateSettlementStatus.SettlementStatusName = pObjSettlementStatus.SettlementStatusName;
                        mObjInsertUpdateSettlementStatus.StatusDescription = pObjSettlementStatus.StatusDescription;

                        if (pObjSettlementStatus.SettlementStatusID == 0)
                        {
                            _db.Settlement_Status.Add(mObjInsertUpdateSettlementStatus);
                        }

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncRespsonse.Success = true;
                            mObjFuncRespsonse.Message = pObjSettlementStatus.SettlementStatusID == 0 ? "Added Successfully" : "Updated Successfully";
                        }
                        catch (Exception ex)
                        {
                            mObjFuncRespsonse.Success = false;
                            mObjFuncRespsonse.Exception = ex;
                            mObjFuncRespsonse.Message = pObjSettlementStatus.SettlementStatusID == 0 ? "Addition Failed" : "Updation Failed";
                        }
                        return mObjFuncRespsonse;
                    }
                }
            }
        }


        public IList<usp_GetSettlementStatusList_Result> REP_GetSettlementStatusList(Settlement_Status pObjSettlementStatus)
        {
            using (_db = new EIRSEntities())
            {
                var vlstSettlementStatus = _db.usp_GetSettlementStatusList(pObjSettlementStatus.SettlementStatusID, pObjSettlementStatus.SettlementStatusName, pObjSettlementStatus.SettlementStatusIds, pObjSettlementStatus.intStatus, pObjSettlementStatus.IncludeSettlementStatusIds, pObjSettlementStatus.ExcludeSettlementStatusIds).ToList();
                return vlstSettlementStatus;
            }
        }

        public usp_GetSettlementStatusList_Result REP_GetSettlementStatusDetails(Settlement_Status pObjSettlementStatus)
        {
            using (_db = new EIRSEntities())
            {
                var vSettlementStatusDetails = _db.usp_GetSettlementStatusList(pObjSettlementStatus.SettlementStatusID, pObjSettlementStatus.SettlementStatusName, pObjSettlementStatus.SettlementStatusIds, pObjSettlementStatus.intStatus, pObjSettlementStatus.IncludeSettlementStatusIds, pObjSettlementStatus.ExcludeSettlementStatusIds).FirstOrDefault();
                return vSettlementStatusDetails;
            }
        }

        public IList<DropDownListResult> REP_GetSettlementStatusDropDownList(Settlement_Status pObjSettlementStatus)
        {
            using (_db = new EIRSEntities())
            {
                //_db.Connection.Open();
                var vResult = (from ss in _db.usp_GetSettlementStatusList(pObjSettlementStatus.SettlementStatusID, pObjSettlementStatus.SettlementStatusName, pObjSettlementStatus.SettlementStatusIds, pObjSettlementStatus.intStatus, pObjSettlementStatus.IncludeSettlementStatusIds, pObjSettlementStatus.ExcludeSettlementStatusIds)
                               select new DropDownListResult()
                               {
                                   id = ss.SettlementStatusID.GetValueOrDefault(),
                                   text = ss.SettlementStatusName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Settlement_Status pObjSettlementStatus)
        {
            using (_db = new EIRSEntities())
            {
                Settlement_Status mObjInsertUpdateSettlementStatus; //Settlement Status Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Update Load Settlement Status
                if (pObjSettlementStatus.SettlementStatusID != 0)
                {
                    mObjInsertUpdateSettlementStatus = (from SettSt in _db.Settlement_Status
                                                        where SettSt.SettlementStatusID == pObjSettlementStatus.SettlementStatusID
                                                        select SettSt).FirstOrDefault();

                    if (mObjInsertUpdateSettlementStatus != null)
                    {
                        mObjInsertUpdateSettlementStatus.Active = !mObjInsertUpdateSettlementStatus.Active;
                        mObjInsertUpdateSettlementStatus.ModifiedBy = pObjSettlementStatus.ModifiedBy;
                        mObjInsertUpdateSettlementStatus.ModifiedDate = pObjSettlementStatus.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Settlement Status Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetSettlementStatusList(0, pObjSettlementStatus.SettlementStatusName, pObjSettlementStatus.SettlementStatusIds, pObjSettlementStatus.intStatus, pObjSettlementStatus.IncludeSettlementStatusIds, pObjSettlementStatus.ExcludeSettlementStatusIds).ToList();
                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Settlement Status Updation Failed";
                        }
                    }
                }
                return mObjFuncResponse;
            }
        }

    }
}
