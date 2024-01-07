using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLNotification
    {
        INotificationRepository _NotificationRepository;

        public BLNotification()
        {
            _NotificationRepository = new NotificationRepository();
        }

        public IList<usp_GetNotificationList_Result> BL_GetNotificationList(Notification pObjNotification)
        {
            return _NotificationRepository.REP_GetNotificationList(pObjNotification);
        }

        public FuncResponse BL_InsertUpdateNotification(Notification pObjNotification)
        {
            return _NotificationRepository.REP_InsertUpdateNotification(pObjNotification);
        }

        public usp_GetNotificationList_Result BL_GetNotificationDetails(Notification pObjNotification)
        {
            return _NotificationRepository.REP_GetNotificationDetails(pObjNotification);
        }

        public IList<vw_Notifications> BL_GetNotificationList()
        {
            return _NotificationRepository.REP_GetNotificationList();
        }

        public IDictionary<string, object> BL_SearchNotification(Notification pObjNotification)
        {
            return _NotificationRepository.REP_SearchNotification(pObjNotification);
        }
    }
}
