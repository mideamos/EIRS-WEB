using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLAssetType
    {
        IAssetTypeRepository _AssetTypeRepository;

        public BLAssetType()
        {
            _AssetTypeRepository = new AssetTypeRepository();
        }

        public IList<usp_GetAssetTypeList_Result> BL_GetAssetTypeList(Asset_Types pObjAssetType)
        {
            return _AssetTypeRepository.REP_GetAssetTypeList(pObjAssetType);
        }

        public FuncResponse BL_InsertUpdateAssetType(Asset_Types pObjAssetType)
        {
            return _AssetTypeRepository.REP_InsertUpdateAssetType(pObjAssetType);
        }

        public usp_GetAssetTypeList_Result BL_GetAssetTypeDetails(Asset_Types pObjAssetType)
        {
            return _AssetTypeRepository.REP_GetAssetTypeDetails(pObjAssetType);
        }

        public IList<DropDownListResult> BL_GetAssetTypeDropDownList(Asset_Types pObjAssetType)
        {
            return _AssetTypeRepository.REP_GetAssetTypeDropDownList(pObjAssetType);
        }

        public FuncResponse BL_UpdateStatus(Asset_Types pObjAssetType)
        {
            return _AssetTypeRepository.REP_UpdateStatus(pObjAssetType);
        }
    }
}
