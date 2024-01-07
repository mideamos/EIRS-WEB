using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIRS.Repository
{
    public class NotificationMethodRepository : INotificationMethodRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateNotificationMethod(Notification_Method pObjNotificationMethod)
        {
            using (_db = new EIRSEntities())
            {
                Notification_Method mObjInsertUpdateNotificationMethod;
                FuncResponse mObjFuncRespsonse = new FuncResponse();
                if(pObjNotificationMethod==null)
                {
                    mObjFuncRespsonse.Success = false;
                    mObjFuncRespsonse.Message = "No Data";
                    return mObjFuncRespsonse;
                }else
                {
                    var vduplicateNotificationMethod = (from notiMethod in _db.Notification_Method where notiMethod.NotificationMethodName == pObjNotificationMethod.NotificationMethodName && notiMethod.NotificationMethodID != pObjNotificationMethod.NotificationMethodID select notiMethod);
                    if(vduplicateNotificationMethod!=null && vduplicateNotificationMethod.Count()>0)
                    {
                        mObjFuncRespsonse.Success = false;
                        mObjFuncRespsonse.Message = "Notification method already exists";
                        return mObjFuncRespsonse;
                    }else
                    {
                        // Update Notification Method
                        if(pObjNotificationMethod.NotificationMethodID !=0)
                        {
                            mObjInsertUpdateNotificationMethod = (from notiMethod in _db.Notification_Method where (notiMethod.NotificationMethodID == pObjNotificationMethod.NotificationMethodID) select notiMethod).FirstOrDefault();

                            if(mObjInsertUpdateNotificationMethod !=null)
                            {
                                mObjInsertUpdateNotificationMethod.ModifiedBy = pObjNotificationMethod.ModifiedBy;
                                mObjInsertUpdateNotificationMethod.ModifiedDate = pObjNotificationMethod.ModifiedDate;
                            }else
                            {
                                mObjInsertUpdateNotificationMethod = new Notification_Method()
                                {
                                    CreatedBy = pObjNotificationMethod.CreatedBy,
                                    CreatedDate = pObjNotificationMethod.CreatedDate
                                };
                            }
                        }
                        // Add Notification Method
                        else
                        {
                            mObjInsertUpdateNotificationMethod = new Notification_Method()
                            {
                                CreatedBy = pObjNotificationMethod.CreatedBy,
                                CreatedDate = pObjNotificationMethod.CreatedDate,
                            };
                        }
                        mObjInsertUpdateNotificationMethod.Active = pObjNotificationMethod.Active;
                        mObjInsertUpdateNotificationMethod.NotificationMethodName = pObjNotificationMethod.NotificationMethodName;

                        if(pObjNotificationMethod.NotificationMethodID==0)
                        {
                            _db.Notification_Method.Add(mObjInsertUpdateNotificationMethod);
                        }

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncRespsonse.Success = true;
                            mObjFuncRespsonse.Message = pObjNotificationMethod.NotificationMethodID == 0 ? "Added Successfully" : "Updated Successfully";
                        }
                        catch(Exception ex)
                        {
                            mObjFuncRespsonse.Success = false;
                            mObjFuncRespsonse.Message= pObjNotificationMethod.NotificationMethodID == 0 ? "Addition Failed" : "Updation Failed";

                        }
                        return mObjFuncRespsonse;
                    }
                }
            }
        }

        public IList<usp_GetNotificationMethodList_Result> REP_GetNotificationMethodList(Notification_Method pObjNotificationMethod)
        {
            using (_db = new EIRSEntities())
            {
                var vlstNotificationMethod = _db.usp_GetNotificationMethodList(pObjNotificationMethod.NotificationMethodID, pObjNotificationMethod.NotificationMethodName, pObjNotificationMethod.NotificationMethodIds, pObjNotificationMethod.intStatus, pObjNotificationMethod.IncludeNotificationMethodIds, pObjNotificationMethod.ExcludeNotificationMethodIds).ToList();
                return vlstNotificationMethod;
            }
        }

        public usp_GetNotificationMethodList_Result REP_GetNotificationMethodDetails(Notification_Method pObjNotificationMethod)
        {
            using (_db = new EIRSEntities())
            {
                var vNotificationMethodDetails = _db.usp_GetNotificationMethodList(pObjNotificationMethod.NotificationMethodID, pObjNotificationMethod.NotificationMethodName, pObjNotificationMethod.NotificationMethodIds, pObjNotificationMethod.intStatus, pObjNotificationMethod.IncludeNotificationMethodIds, pObjNotificationMethod.ExcludeNotificationMethodIds).FirstOrDefault();
                return vNotificationMethodDetails;
            }
        }

        public FuncResponse REP_UpdateStatus(Notification_Method pObjNotificationMethod)
        {
            using (_db = new EIRSEntities())
            {
                Notification_Method mObjInsertUpdateNotificationMethod; //Notification Method Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Notification Method
                if (pObjNotificationMethod.NotificationMethodID != 0)
                {
                    mObjInsertUpdateNotificationMethod = (from notM in _db.Notification_Method
                                                 where notM.NotificationMethodID == pObjNotificationMethod.NotificationMethodID
                                                 select notM).FirstOrDefault();

                    if (mObjInsertUpdateNotificationMethod != null)
                    {
                        mObjInsertUpdateNotificationMethod.Active = !mObjInsertUpdateNotificationMethod.Active;
                        mObjInsertUpdateNotificationMethod.ModifiedBy = pObjNotificationMethod.ModifiedBy;
                        mObjInsertUpdateNotificationMethod.ModifiedDate = pObjNotificationMethod.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Notification Method Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetNotificationMethodList(0,pObjNotificationMethod.NotificationMethodName, pObjNotificationMethod.NotificationMethodIds, pObjNotificationMethod.intStatus, pObjNotificationMethod.IncludeNotificationMethodIds, pObjNotificationMethod.ExcludeNotificationMethodIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Notification Method Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<DropDownListResult> REP_GetNotificationMethodDropDownList(Notification_Method pObjNotificationMethod)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = from pro in _db.Notification_Method
                              //where (pro.TaxPayerTypeID == pObjEconomicActivities.TaxPayerTypeID)
                              select new DropDownListResult() { id = pro.NotificationMethodID, text = pro.NotificationMethodName };

                return vResult.ToList();
                //var vResult = (from popt in _db.usp_GetNotificationMethodList(pObjNotificationMethod.NotificationMethodID, pObjNotificationMethod.NotificationMethodName, pObjNotificationMethod.NotificationMethodIds, pObjNotificationMethod.intStatus, pObjNotificationMethod.IncludeNotificationMethodIds, pObjNotificationMethod.ExcludeNotificationMethodIds)
                //               select new DropDownListResult()
                //               {
                //                   id = popt.NotificationMethodID.GetValueOrDefault(),
                //                   text = popt.NotificationMethodName
                //               }).ToList();

                //return vResult;
            }
        }
    }
}
