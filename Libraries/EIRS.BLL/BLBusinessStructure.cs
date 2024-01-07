using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLBusinessStructure
    {
        IBusinessStructureRepository _BusinessStructureRepository;

        public BLBusinessStructure()
        {
            _BusinessStructureRepository = new BusinessStructureRepository();
        }

        public IList<usp_GetBusinessStructureList_Result> BL_GetBusinessStructureList(Business_Structure pObjBusinessStructure)
        {
            return _BusinessStructureRepository.REP_GetBusinessStructureList(pObjBusinessStructure);
        }

        public FuncResponse BL_InsertUpdateBusinessStructure(Business_Structure pObjBusinessStructure)
        {
            return _BusinessStructureRepository.REP_InsertUpdateBusinessStructure(pObjBusinessStructure);
        }

        public usp_GetBusinessStructureList_Result BL_GetBusinessStructureDetails(Business_Structure pObjBusinessStructure)
        {
            return _BusinessStructureRepository.REP_GetBusinessStructureDetails(pObjBusinessStructure);
        }

        public IList<DropDownListResult> BL_GetBusinessStructureDropDownList(Business_Structure pObjBusinessStructure)
        {
            return _BusinessStructureRepository.REP_GetBusinessStructureDropDownList(pObjBusinessStructure);
        }

        public FuncResponse BL_UpdateStatus(Business_Structure pObjBusinessStructure)
        {
            return _BusinessStructureRepository.REP_UpdateStatus(pObjBusinessStructure);
        }
    }
}
