using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IAssetTypeRepository
    {
        usp_GetAssetTypeList_Result REP_GetAssetTypeDetails(Asset_Types pObjAssetType);
        IList<DropDownListResult> REP_GetAssetTypeDropDownList(Asset_Types pObjAssetType);
        IList<usp_GetAssetTypeList_Result> REP_GetAssetTypeList(Asset_Types pObjAssetType);
        FuncResponse REP_InsertUpdateAssetType(Asset_Types pObjAssetType);
        FuncResponse REP_UpdateStatus(Asset_Types pObjAssetType);
    }
}