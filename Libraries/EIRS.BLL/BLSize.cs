using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLSize
    {
        ISizeRepository _SizeRepository;

        public BLSize()
        {
            _SizeRepository = new SizeRepository();
        }

        public IList<usp_GetSizeList_Result> BL_GetSizeList(Size pObjSize)
        {
            return _SizeRepository.REP_GetSizeList(pObjSize);
        }

        public FuncResponse BL_InsertUpdateSize(Size pObjSize)
        {
            return _SizeRepository.REP_InsertUpdateSize(pObjSize);
        }

        public usp_GetSizeList_Result BL_GetSizeDetails(Size pObjSize)
        {
            return _SizeRepository.REP_GetSizeDetails(pObjSize);
        }

        public IList<DropDownListResult> BL_GetSizeDropDownList(Size pObjSize)
        {
            return _SizeRepository.REP_GetSizeDropDownList(pObjSize);
        }

        public FuncResponse BL_UpdateStatus(Size pObjSize)
        {
            return _SizeRepository.REP_UpdateStatus(pObjSize);
        }
    }
}
