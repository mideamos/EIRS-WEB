using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IBusinessSectorRepository
    {
        usp_GetBusinessSectorList_Result REP_GetBusinessSectorDetails(Business_Sector pObjBusinessSector);
        IList<DropDownListResult> REP_GetBusinessSectorDropDownList(Business_Sector pObjBusinessSector);
        IList<usp_GetBusinessSectorList_Result> REP_GetBusinessSectorList(Business_Sector pObjBusinessSector);
        FuncResponse REP_InsertUpdateBusinessSector(Business_Sector pObjBusinessSector);
        FuncResponse REP_UpdateStatus(Business_Sector pObjBusinessSector);
    }
}