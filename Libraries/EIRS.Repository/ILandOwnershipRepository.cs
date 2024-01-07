using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ILandOwnershipRepository
    {
        usp_GetLandOwnershipList_Result REP_GetLandOwnershipDetails(Land_Ownership pObjLandOwnership);
        IList<DropDownListResult> REP_GetLandOwnershipDropDownList(Land_Ownership pObjLandOwnership);
        IList<usp_GetLandOwnershipList_Result> REP_GetLandOwnershipList(Land_Ownership pObjLandOwnership);
        FuncResponse REP_InsertUpdateLandOwnership(Land_Ownership pObjLandOwnership);
        FuncResponse REP_UpdateStatus(Land_Ownership pObjLandOwnership);
    }
}