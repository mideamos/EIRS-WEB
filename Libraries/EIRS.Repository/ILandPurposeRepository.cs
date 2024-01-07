using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ILandPurposeRepository
    {
        usp_GetLandPurposeList_Result REP_GetLandPurposeDetails(Land_Purpose pObjLandPurpose);
        IList<DropDownListResult> REP_GetLandPurposeDropDownList(Land_Purpose pObjLandPurpose);
        IList<usp_GetLandPurposeList_Result> REP_GetLandPurposeList(Land_Purpose pObjLandPurpose);
        FuncResponse REP_InsertUpdateLandPurpose(Land_Purpose pObjLandPurpose);
        FuncResponse REP_UpdateStatus(Land_Purpose pObjLandPurpose);
    }
}