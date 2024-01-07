using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLLandDevelopment
    {
        ILandDevelopmentRepository _LandDevelopmentRepository;

        public BLLandDevelopment()
        {
            _LandDevelopmentRepository = new LandDevelopmentRepository();
        }

        public IList<usp_GetLandDevelopmentList_Result> BL_GetLandDevelopmentList(Land_Development pObjLandDevelopment)
        {
            return _LandDevelopmentRepository.REP_GetLandDevelopmentList(pObjLandDevelopment);
        }

        public FuncResponse BL_InsertUpdateLandDevelopment(Land_Development pObjLandDevelopment)
        {
            return _LandDevelopmentRepository.REP_InsertUpdateLandDevelopment(pObjLandDevelopment);
        }

        public usp_GetLandDevelopmentList_Result BL_GetLandDevelopmentDetails(Land_Development pObjLandDevelopment)
        {
            return _LandDevelopmentRepository.REP_GetLandDevelopmentDetails(pObjLandDevelopment);
        }

        public IList<DropDownListResult> BL_GetLandDevelopmentDropDownList(Land_Development pObjLandDevelopment)
        {
            return _LandDevelopmentRepository.REP_GetLandDevelopmentDropDownList(pObjLandDevelopment);
        }

        public FuncResponse BL_UpdateStatus(Land_Development pObjLandDevelopment)
        {
            return _LandDevelopmentRepository.REP_UpdateStatus(pObjLandDevelopment);
        }
    }
}
