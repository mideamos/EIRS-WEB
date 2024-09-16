using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLNotificationType
    {
        INotificationTypeRepository _NotificationRepository;
        public BLNotificationType()
        {
            _NotificationRepository = new NotificationTypeRepository();
        }

        public FuncResponse BL_InsertUpdateNotificationType(Notification_Type pObjNotificationMethod)
        {
            return _NotificationRepository.REP_InsertUpdateNotificationType(pObjNotificationMethod);
        }

        public IList<usp_GetNotificationTypeList_Result> BL_GetNotificationTypeList(Notification_Type pObjNotificationType)
        {
            return _NotificationRepository.REP_GetNotificationTypeList(pObjNotificationType);
        }

        public usp_GetNotificationTypeList_Result BL_GetNotificationTypeDetails(Notification_Type pObjNotificationType)
        {
            return _NotificationRepository.REP_GetNotificationTypeDetails(pObjNotificationType);
        }

        public FuncResponse BL_UpdateStatus(Notification_Type pObjNotificationType)
        {
            return _NotificationRepository.REP_UpdateStatus(pObjNotificationType);
        }
        public IList<DropDownListResult> BL_GetNotificationTypeDropDownList(Notification_Type pObjNotificationTyped)
        {
            return _NotificationRepository.REP_GetNotificationTypeDropDownList(pObjNotificationTyped);
        }
    }
}
