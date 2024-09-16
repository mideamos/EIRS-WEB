using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ILandDevelopmentRepository
    {
        usp_GetLandDevelopmentList_Result REP_GetLandDevelopmentDetails(Land_Development pObjLandDevelopment);
        IList<DropDownListResult> REP_GetLandDevelopmentDropDownList(Land_Development pObjLandDevelopment);
        IList<usp_GetLandDevelopmentList_Result> REP_GetLandDevelopmentList(Land_Development pObjLandDevelopment);
        FuncResponse REP_InsertUpdateLandDevelopment(Land_Development pObjLandDevelopment);
        FuncResponse REP_UpdateStatus(Land_Development pObjLandDevelopment);
    }
}