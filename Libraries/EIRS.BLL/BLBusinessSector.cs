using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLBusinessSector
    {
        IBusinessSectorRepository _BusinessSectorRepository;

        public BLBusinessSector()
        {
            _BusinessSectorRepository = new BusinessSectorRepository();
        }

        public IList<usp_GetBusinessSectorList_Result> BL_GetBusinessSectorList(Business_Sector pObjBusinessSector)
        {
            return _BusinessSectorRepository.REP_GetBusinessSectorList(pObjBusinessSector);
        }

        public FuncResponse BL_InsertUpdateBusinessSector(Business_Sector pObjBusinessSector)
        {
            return _BusinessSectorRepository.REP_InsertUpdateBusinessSector(pObjBusinessSector);
        }

        public usp_GetBusinessSectorList_Result BL_GetBusinessSectorDetails(Business_Sector pObjBusinessSector)
        {
            return _BusinessSectorRepository.REP_GetBusinessSectorDetails(pObjBusinessSector);
        }

        public IList<DropDownListResult> BL_GetBusinessSectorDropDownList(Business_Sector pObjBusinessSector)
        {
            return _BusinessSectorRepository.REP_GetBusinessSectorDropDownList(pObjBusinessSector);
        }

        public FuncResponse BL_UpdateStatus(Business_Sector pObjBusinessSector)
        {
            return _BusinessSectorRepository.REP_UpdateStatus(pObjBusinessSector);
        }
    }
}
