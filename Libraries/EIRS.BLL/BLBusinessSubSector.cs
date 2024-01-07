using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLBusinessSubSector
    {
        IBusinessSubSectorRepository _BusinessSubSectorRepository;

        public BLBusinessSubSector()
        {
            _BusinessSubSectorRepository = new BusinessSubSectorRepository();
        }

        public IList<usp_GetBusinessSubSectorList_Result> BL_GetBusinessSubSectorList(Business_SubSector pObjBusinessSubSector)
        {
            return _BusinessSubSectorRepository.REP_GetBusinessSubSectorList(pObjBusinessSubSector);
        }

        public FuncResponse BL_InsertUpdateBusinessSubSector(Business_SubSector pObjBusinessSubSector)
        {
            return _BusinessSubSectorRepository.REP_InsertUpdateBusinessSubSector(pObjBusinessSubSector);
        }

        public usp_GetBusinessSubSectorList_Result BL_GetBusinessSubSectorDetails(Business_SubSector pObjBusinessSubSector)
        {
            return _BusinessSubSectorRepository.REP_GetBusinessSubSectorDetails(pObjBusinessSubSector);
        }

        public IList<DropDownListResult> BL_GetBusinessSubSectorDropDownList(Business_SubSector pObjBusinessSubSector)
        {
            return _BusinessSubSectorRepository.REP_GetBusinessSubSectorDropDownList(pObjBusinessSubSector);
        }

        public FuncResponse BL_UpdateStatus(Business_SubSector pObjBusinessSubSector)
        {
            return _BusinessSubSectorRepository.REP_UpdateStatus(pObjBusinessSubSector);
        }
    }
}
