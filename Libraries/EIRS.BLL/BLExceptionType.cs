using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLExceptionType
    {
        IExceptionTypeRepository _NotificationRepository;
        public BLExceptionType()
        {
            _NotificationRepository = new ExceptionTypeRepository();
        }

        public FuncResponse BL_InsertUpdateExceptionType(Exception_Type pObjNotifictionMethod)
        {
            return _NotificationRepository.REP_InsertUpdateExceptionType(pObjNotifictionMethod);
        }

        public IList<usp_GetExceptionTypeList_Result> BL_GetExceptionTypeList(Exception_Type pObjExceptionType)
        {
            return _NotificationRepository.REP_GetExceptionTypeList(pObjExceptionType);
        }

        public usp_GetExceptionTypeList_Result BL_GetExceptionTypeDetails(Exception_Type pObjExceptionType)
        {
            return _NotificationRepository.REP_GetExceptionTypeDetails(pObjExceptionType);
        }

        public FuncResponse BL_UpdateStatus(Exception_Type pObjExceptionType)
        {
            return _NotificationRepository.REP_UpdateStatus(pObjExceptionType);
        }
    }
}
