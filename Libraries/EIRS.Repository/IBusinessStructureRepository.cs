using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IBusinessStructureRepository
    {
		FuncResponse REP_InsertUpdateBusinessStructure(Business_Structure pObjBusinessStructure);
		IList<usp_GetBusinessStructureList_Result> REP_GetBusinessStructureList(Business_Structure pObjBusinessStructure);
        usp_GetBusinessStructureList_Result REP_GetBusinessStructureDetails(Business_Structure pObjBusinessStructure);
        IList<DropDownListResult> REP_GetBusinessStructureDropDownList(Business_Structure pObjBusinessStructure);
        FuncResponse REP_UpdateStatus(Business_Structure pObjBusinessStructure);
    }
}