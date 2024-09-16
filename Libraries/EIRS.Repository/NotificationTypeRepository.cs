using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIRS.Repository
{
    public class NotificationTypeRepository : INotificationTypeRepository
    {
        EIRSEntities _db;
        public FuncResponse REP_InsertUpdateNotificationType(Notification_Type pObjNotificationType)
        {
            using (_db = new EIRSEntities())
            {
                Notification_Type mObjInsertUpdateNotificationType;
                FuncResponse mObjFuncRespsonse = new FuncResponse();
                if (pObjNotificationType == null)
                {
                    mObjFuncRespsonse.Success = false;
                    mObjFuncRespsonse.Message = "No Data";
                    return mObjFuncRespsonse;
                }
                else
                {
                    var vduplicateNotificationType = (from notiType in _db.Notification_Type where notiType.NotificationTypeName == pObjNotificationType.NotificationTypeName && notiType.NotificationTypeID != pObjNotificationType.NotificationTypeID select notiType);
                    if (vduplicateNotificationType != null && vduplicateNotificationType.Count() > 0)
                    {
                        mObjFuncRespsonse.Success = false;
                        mObjFuncRespsonse.Message = "Notification Type already exists";
                        return mObjFuncRespsonse;
                    }
                    else
                    {
                        // Update Notification Type
                        if (pObjNotificationType.NotificationTypeID != 0)
                        {
                            mObjInsertUpdateNotificationType = (from notiType in _db.Notification_Type where (notiType.NotificationTypeID == pObjNotificationType.NotificationTypeID) select notiType).FirstOrDefault();

                            if (mObjInsertUpdateNotificationType != null)
                            {
                                mObjInsertUpdateNotificationType.ModifiedBy = pObjNotificationType.ModifiedBy;
                                mObjInsertUpdateNotificationType.ModifiedDate = pObjNotificationType.ModifiedDate;
                            }
                            else
                            {
                                mObjInsertUpdateNotificationType = new Notification_Type()
                                {
                                    CreatedBy = pObjNotificationType.CreatedBy,
                                    CreatedDate = pObjNotificationType.CreatedDate
                                };
                            }
                        }
                        // Add Notification Type
                        else
                        {
                            mObjInsertUpdateNotificationType = new Notification_Type()
                            {
                                CreatedBy = pObjNotificationType.CreatedBy,
                                CreatedDate = pObjNotificationType.CreatedDate,
                            };
                        }
                        mObjInsertUpdateNotificationType.Active = pObjNotificationType.Active;
                        mObjInsertUpdateNotificationType.NotificationTypeName = pObjNotificationType.NotificationTypeName;
                        mObjInsertUpdateNotificationType.TypeDescription = pObjNotificationType.TypeDescription;

                        if (pObjNotificationType.NotificationTypeID == 0)
                        {
                            _db.Notification_Type.Add(mObjInsertUpdateNotificationType);
                        }

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncRespsonse.Success = true;
                            mObjFuncRespsonse.Message = pObjNotificationType.NotificationTypeID == 0 ? "Added Successfully" : "Updated Successfully";
                        }
                        catch (Exception ex)
                        {
                            mObjFuncRespsonse.Success = false;
                            mObjFuncRespsonse.Exception = ex;
                            mObjFuncRespsonse.Message = pObjNotificationType.NotificationTypeID == 0 ? "Addition Failed" : "Updation Failed";
                        }
                        return mObjFuncRespsonse;
                    }
                }
            }
        }


        public IList<usp_GetNotificationTypeList_Result> REP_GetNotificationTypeList(Notification_Type pObjNotificationType)
        {
            using (_db = new EIRSEntities())
            {
                var vlstNotificationType = _db.usp_GetNotificationTypeList(pObjNotificationType.NotificationTypeID, pObjNotificationType.NotificationTypeName, pObjNotificationType.NotificationTypeIds, pObjNotificationType.intStatus, pObjNotificationType.IncludeNotificationTypeIds, pObjNotificationType.ExcludeNotificationTypeIds).ToList();
                return vlstNotificationType;
            }
        }

        public usp_GetNotificationTypeList_Result REP_GetNotificationTypeDetails(Notification_Type pObjNotificationType)
        {
            using (_db = new EIRSEntities())
            {
                var vNotificationTypeDetails = _db.usp_GetNotificationTypeList(pObjNotificationType.NotificationTypeID, pObjNotificationType.NotificationTypeName, pObjNotificationType.NotificationTypeIds, pObjNotificationType.intStatus, pObjNotificationType.IncludeNotificationTypeIds, pObjNotificationType.ExcludeNotificationTypeIds).FirstOrDefault();
                return vNotificationTypeDetails;
            }
        }

        public FuncResponse REP_UpdateStatus(Notification_Type pObjNotificationType)
        {
            using (_db = new EIRSEntities())
            {
                Notification_Type mObjInsertUpdateNotificationType; //Notification Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Update Load Notification Type
                if (pObjNotificationType.NotificationTypeID != 0)
                {
                    mObjInsertUpdateNotificationType = (from NotifType in _db.Notification_Type
                                                          where NotifType.NotificationTypeID == pObjNotificationType.NotificationTypeID
                                                          select NotifType).FirstOrDefault();

                    if (mObjInsertUpdateNotificationType != null)
                    {
                        mObjInsertUpdateNotificationType.Active = !mObjInsertUpdateNotificationType.Active;
                        mObjInsertUpdateNotificationType.ModifiedBy = pObjNotificationType.ModifiedBy;
                        mObjInsertUpdateNotificationType.ModifiedDate = pObjNotificationType.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Notification Type Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetNotificationTypeList(0, pObjNotificationType.NotificationTypeName, pObjNotificationType.NotificationTypeIds, pObjNotificationType.intStatus, pObjNotificationType.IncludeNotificationTypeIds, pObjNotificationType.ExcludeNotificationTypeIds).ToList();
                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Notification Type Updation Failed";
                        }
                    }
                }
                return mObjFuncResponse;
            }
        }

        public IList<DropDownListResult> REP_GetNotificationTypeDropDownList(Notification_Type pObjNotificationType)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from popt in _db.usp_GetNotificationTypeList(pObjNotificationType.NotificationTypeID, pObjNotificationType.NotificationTypeName, pObjNotificationType.NotificationTypeIds, pObjNotificationType.intStatus, pObjNotificationType.IncludeNotificationTypeIds, pObjNotificationType.ExcludeNotificationTypeIds)
                               select new DropDownListResult()
                               {
                                   id = popt.NotificationTypeID.GetValueOrDefault(),
                                   text = popt.NotificationTypeName
                               }).ToList();

                return vResult;
            }
        }

    }
}
