using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IBusinessSubSectorRepository
    {
        usp_GetBusinessSubSectorList_Result REP_GetBusinessSubSectorDetails(Business_SubSector pObjBusinessSubSector);
        IList<DropDownListResult> REP_GetBusinessSubSectorDropDownList(Business_SubSector pObjBusinessSubSector);
        IList<usp_GetBusinessSubSectorList_Result> REP_GetBusinessSubSectorList(Business_SubSector pObjBusinessSubSector);
        FuncResponse REP_InsertUpdateBusinessSubSector(Business_SubSector pObjBusinessSubSector);
        FuncResponse REP_UpdateStatus(Business_SubSector pObjBusinessSubSector);
    }
}