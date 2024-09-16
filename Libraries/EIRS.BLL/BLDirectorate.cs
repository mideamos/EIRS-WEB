using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLDirectorate
    {
        IDirectorateRepository _DirectorateRepository;

        public BLDirectorate()
        {
            _DirectorateRepository = new DirectorateRepository();
        }

        public IList<usp_GetDirectorateList_Result> BL_GetDirectorateList(Directorate pObjDirectorate)
        {
            return _DirectorateRepository.REP_GetDirectorateList(pObjDirectorate);
        }

        public FuncResponse<Directorate> BL_InsertUpdateDirectorate(Directorate pObjDirectorate)
        {
            return _DirectorateRepository.REP_InsertUpdateDirectorate(pObjDirectorate);
        }

        public usp_GetDirectorateList_Result BL_GetDirectorateDetails(Directorate pObjDirectorate)
        {
            return _DirectorateRepository.REP_GetDirectorateDetails(pObjDirectorate);
        }

        public IList<DropDownListResult> BL_GetDirectorateDropDownList(Directorate pObjDirectorate)
        {
            return _DirectorateRepository.REP_GetDirectorateDropDownList(pObjDirectorate);
        }

        public FuncResponse BL_UpdateStatus(Directorate pObjDirectorate)
        {
            return _DirectorateRepository.REP_UpdateStatus(pObjDirectorate);
        }

        public FuncResponse BL_InsertRevenueStream(MAP_Directorates_RevenueStream pObjRevenueStream)
        {
            return _DirectorateRepository.REP_InsertRevenueStream(pObjRevenueStream);
        }

        public FuncResponse BL_RemoveRevenueStream(MAP_Directorates_RevenueStream pObjRevenueStream)
        {
            return _DirectorateRepository.REP_RemoveRevenueStream(pObjRevenueStream);
        }

        public IList<MAP_Directorates_RevenueStream> BL_GetRevenueStream(int pIntRevenueStreamID)
        {
            return _DirectorateRepository.REP_GetRevenueStream(pIntRevenueStreamID);
        }
    }
}
