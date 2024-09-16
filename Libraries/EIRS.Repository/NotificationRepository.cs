using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EIRS.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateNotification(Notification pObjNotification)
        {
            using (_db = new EIRSEntities())
            {
                Notification mObjInsertUpdateNotification; //Notification Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object


                //If Update Load Notification
                if (pObjNotification.NotificationID != 0)
                {
                    mObjInsertUpdateNotification = (from pfreq in _db.Notifications
                                                    where pfreq.NotificationID == pObjNotification.NotificationID
                                                    select pfreq).FirstOrDefault();

                    if (mObjInsertUpdateNotification == null)
                    {
                        mObjInsertUpdateNotification = new Notification();
                    }
                }
                else // Else Insert Notification
                {
                    mObjInsertUpdateNotification = new Notification();
                }

                mObjInsertUpdateNotification.NotificationDate = pObjNotification.NotificationDate;
                mObjInsertUpdateNotification.NotificationMethodID = pObjNotification.NotificationMethodID;
                mObjInsertUpdateNotification.NotificationTypeID = pObjNotification.NotificationTypeID;
                mObjInsertUpdateNotification.EventRefNo = pObjNotification.EventRefNo;
                mObjInsertUpdateNotification.TaxPayerTypeID = pObjNotification.TaxPayerTypeID;
                mObjInsertUpdateNotification.TaxPayerID = pObjNotification.TaxPayerID;
                mObjInsertUpdateNotification.NotificationStatus = pObjNotification.NotificationStatus;
                mObjInsertUpdateNotification.NotificationContent = pObjNotification.NotificationContent;
                mObjInsertUpdateNotification.NotificationModeID = pObjNotification.NotificationModeID;
                mObjInsertUpdateNotification.CreatedBy = pObjNotification.CreatedBy;
                mObjInsertUpdateNotification.CreatedDate = pObjNotification.CreatedDate;

                if (pObjNotification.NotificationID == 0)
                {
                    _db.Notifications.Add(mObjInsertUpdateNotification);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjNotification.NotificationID == 0)
                    {
                        mObjFuncResponse.Message = "Notification Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Notification Updated Successfully";
                    }
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjNotification.NotificationID == 0)
                    {
                        mObjFuncResponse.Message = "Notification Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Notification Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetNotificationList_Result> REP_GetNotificationList(Notification pObjNotification)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetNotificationList(pObjNotification.NotificationID, pObjNotification.NotificationRefNo, pObjNotification.NotificationMethodID, pObjNotification.NotificationTypeID, pObjNotification.TaxPayerTypeID, pObjNotification.TaxPayerID, pObjNotification.IntStatus).ToList();
            }
        }

        public IList<vw_Notifications> REP_GetNotificationList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_Notifications.ToList();
            }
        }

        public usp_GetNotificationList_Result REP_GetNotificationDetails(Notification pObjNotification)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetNotificationList(pObjNotification.NotificationID, pObjNotification.NotificationRefNo, pObjNotification.NotificationMethodID, pObjNotification.NotificationTypeID, pObjNotification.TaxPayerTypeID, pObjNotification.TaxPayerID, pObjNotification.IntStatus).FirstOrDefault();
            }
        }

        public IDictionary<string, object> REP_SearchNotification(Notification pObjNotification)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["NotificationList"] = _db.usp_SearchNotification(pObjNotification.WhereCondition, pObjNotification.OrderBy, pObjNotification.OrderByDirection, pObjNotification.PageNumber, pObjNotification.PageSize, pObjNotification.MainFilter,
                                                                        pObjNotification.NotificationRefNo, pObjNotification.strNotificationDate, pObjNotification.NotificationMethodName, pObjNotification.NotificationTypeName, pObjNotification.EventRefNo, pObjNotification.TaxPayerTypeName, pObjNotification.TaxPayerName, pObjNotification.StatusName).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(NotificationID) FROM Notifications").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(NotificationID) ");
                sbFilteredCountQuery.Append(" FROM Notifications notf ");
                sbFilteredCountQuery.Append(" INNER JOIN Notification_Method nm ON notf.NotificationMethodID = nm.NotificationMethodID ");
                sbFilteredCountQuery.Append(" INNER JOIN Notification_Type nt ON notf.NotificationTypeID = nt.NotificationTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Notification_Mode nmod ON notf.NotificationModeID = nmod.NotificationModeID ");
                sbFilteredCountQuery.Append(" INNER JOIN TaxPayer_Types tpt ON notf.TaxPayerTypeID  = tpt.TaxPayerTypeID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjNotification.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjNotification.MainFilter),
                    new SqlParameter("@NotificationRefNo",pObjNotification.NotificationRefNo),
                    new SqlParameter("@NotificationDate",pObjNotification.strNotificationDate),
                    new SqlParameter("@NotificationMethodName",pObjNotification.NotificationMethodName),
                    new SqlParameter("@NotificationTypeName",pObjNotification.NotificationTypeName),
                    new SqlParameter("@EventRefNo",pObjNotification.EventRefNo),
                    new SqlParameter("@TaxPayerTypeName",pObjNotification.TaxPayerTypeName),
                    new SqlParameter("@TaxPayerName",pObjNotification.TaxPayerName),
                    new SqlParameter("@StatusName",pObjNotification.StatusName),

                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

    }
}