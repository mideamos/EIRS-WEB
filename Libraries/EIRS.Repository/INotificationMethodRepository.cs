using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface INotificationMethodRepository
    {
        usp_GetNotificationMethodList_Result REP_GetNotificationMethodDetails(Notification_Method pObjNotificationMethod);
        IList<usp_GetNotificationMethodList_Result> REP_GetNotificationMethodList(Notification_Method pObjNotificationMethod);
        FuncResponse REP_InsertUpdateNotificationMethod(Notification_Method pObjNotificationMethod);

        IList<DropDownListResult> REP_GetNotificationMethodDropDownList(Notification_Method pObjNotificationMethod);

        FuncResponse REP_UpdateStatus(Notification_Method pObjNotificationMethod);
    }
}