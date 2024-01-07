using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface INotificationRepository
    {
        usp_GetNotificationList_Result REP_GetNotificationDetails(Notification pObjNotification);
        IList<usp_GetNotificationList_Result> REP_GetNotificationList(Notification pObjNotification);
        FuncResponse REP_InsertUpdateNotification(Notification pObjNotification);
        IList<vw_Notifications> REP_GetNotificationList();

        IDictionary<string, object> REP_SearchNotification(Notification pObjNotification);
    }
}