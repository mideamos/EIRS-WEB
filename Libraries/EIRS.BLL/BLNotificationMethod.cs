using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLNotificationMethod
    {
        INotificationMethodRepository _NotificationMethodRepository;
        public BLNotificationMethod()
        {
            _NotificationMethodRepository = new NotificationMethodRepository();
        }

        public FuncResponse BL_InsertUpdateNotificationMethod(Notification_Method pObjNotifictionMethod)
        {
            return _NotificationMethodRepository.REP_InsertUpdateNotificationMethod(pObjNotifictionMethod);
        }

        public IList<usp_GetNotificationMethodList_Result> BL_GetNotificationMethodList(Notification_Method pObjNotificationMethod)
        {
            return _NotificationMethodRepository.REP_GetNotificationMethodList(pObjNotificationMethod);
        }

        public usp_GetNotificationMethodList_Result BL_GetNotificationMethodDetails(Notification_Method pObjNotificationMethod)
        {
            return _NotificationMethodRepository.REP_GetNotificationMethodDetails(pObjNotificationMethod);
        }

        public FuncResponse BL_UpdateStatus(Notification_Method pObjNotificationMethod)
        {
            return _NotificationMethodRepository.REP_UpdateStatus(pObjNotificationMethod);
        }

        public IList<DropDownListResult> BL_GetNotificationMethodDropDownList(Notification_Method pObjNotificationMethod)
        {
            return _NotificationMethodRepository.REP_GetNotificationMethodDropDownList(pObjNotificationMethod);
        }
    }
}
