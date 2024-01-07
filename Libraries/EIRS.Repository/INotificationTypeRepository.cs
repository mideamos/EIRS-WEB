using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface INotificationTypeRepository
    {
        usp_GetNotificationTypeList_Result REP_GetNotificationTypeDetails(Notification_Type pObjNotificationType);
        IList<usp_GetNotificationTypeList_Result> REP_GetNotificationTypeList(Notification_Type pObjNotificationType);
        FuncResponse REP_InsertUpdateNotificationType(Notification_Type pObjNotificationType);
        FuncResponse REP_UpdateStatus(Notification_Type pObjNotificationType);
        IList<DropDownListResult> REP_GetNotificationTypeDropDownList(Notification_Type pObjNotificationType);
    }
}