using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EIRS.Repository
{
    public class BusinessRepository : IBusinessRepository
    {
        EIRSEntities _db;

        public FuncResponse<Business> REP_InsertUpdateBusiness(Business pObjBusiness)
        {
            using (_db = new EIRSEntities())
            {
                Business mObjInsertUpdateBusiness; //Business Insert Update Object
                FuncResponse<Business> mObjFuncResponse = new FuncResponse<Business>(); //Return Object

                //If Update Load Business Completion

                if (pObjBusiness.BusinessID != 0)
                {
                    mObjInsertUpdateBusiness = (from Business in _db.Businesses
                                                where Business.BusinessID == pObjBusiness.BusinessID
                                                select Business).FirstOrDefault();

                    if (mObjInsertUpdateBusiness != null)
                    {
                        mObjInsertUpdateBusiness.ModifiedBy = pObjBusiness.ModifiedBy;
                        mObjInsertUpdateBusiness.ModifiedDate = pObjBusiness.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateBusiness = new Business();
                        mObjInsertUpdateBusiness.CreatedBy = pObjBusiness.CreatedBy;
                        mObjInsertUpdateBusiness.CreatedDate = pObjBusiness.CreatedDate;
                    }
                }
                else // Else Insert Business Completion
                {
                    mObjInsertUpdateBusiness = new Business();
                    mObjInsertUpdateBusiness.CreatedBy = pObjBusiness.CreatedBy;
                    mObjInsertUpdateBusiness.CreatedDate = pObjBusiness.CreatedDate;
                }

                mObjInsertUpdateBusiness.AssetTypeID = pObjBusiness.AssetTypeID;
                mObjInsertUpdateBusiness.BusinessTypeID = pObjBusiness.BusinessTypeID;
                mObjInsertUpdateBusiness.BusinessName = pObjBusiness.BusinessName;
                mObjInsertUpdateBusiness.LGAID = pObjBusiness.LGAID;
                mObjInsertUpdateBusiness.ZoneId = pObjBusiness.ZoneId;
                mObjInsertUpdateBusiness.TaxOfficeID = pObjBusiness.TaxOfficeID;
                mObjInsertUpdateBusiness.BusinessCategoryID = pObjBusiness.BusinessCategoryID;
                mObjInsertUpdateBusiness.BusinessSectorID = pObjBusiness.BusinessSectorID;
                mObjInsertUpdateBusiness.BusinessSubSectorID = pObjBusiness.BusinessSubSectorID;
                mObjInsertUpdateBusiness.BusinessStructureID = pObjBusiness.BusinessStructureID;
                mObjInsertUpdateBusiness.BusinessOperationID = pObjBusiness.BusinessOperationID;
                mObjInsertUpdateBusiness.ContactName = pObjBusiness.ContactName;
                mObjInsertUpdateBusiness.BusinessNumber = pObjBusiness.BusinessNumber;
                mObjInsertUpdateBusiness.BusinessAddress = pObjBusiness.BusinessAddress;
                mObjInsertUpdateBusiness.SizeID = pObjBusiness.SizeID;
                mObjInsertUpdateBusiness.DataSourceID = pObjBusiness.DataSourceID;
                mObjInsertUpdateBusiness.DSRefID = pObjBusiness.DSRefID;
                mObjInsertUpdateBusiness.Active = pObjBusiness.Active;

                if (pObjBusiness.BusinessID == 0)
                {
                    _db.Businesses.Add(mObjInsertUpdateBusiness);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjBusiness.BusinessID == 0)
                    {
                        mObjFuncResponse.Message = "Business Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Business Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateBusiness;

                    //if business is Updated , Check For Profile Update
                    if (pObjBusiness.BusinessID > 0)
                    {
                        var vTPAList = (from tpa in _db.MAP_TaxPayer_Asset
                                        where tpa.AssetID == pObjBusiness.BusinessID && tpa.AssetTypeID == pObjBusiness.AssetTypeID
                                        select tpa).ToList();

                        foreach (var item in vTPAList)
                        {

                            //Delete Profile Information
                            var vDeleteTaxPayerAssetProfile = (from tpap in _db.MAP_TaxPayer_Asset_Profile
                                                               where tpap.TPAID == item.TPAID
                                                               select tpap);

                            _db.MAP_TaxPayer_Asset_Profile.RemoveRange(vDeleteTaxPayerAssetProfile);

                            _db.SaveChanges();

                            //Update Profile
                            _db.usp_InsertProfileInformation(item.TPAID);
                        }
                    }

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjBusiness.BusinessID == 0)
                    {
                        mObjFuncResponse.Message = "Business Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Business Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetBusinessListNewTy_Result> REP_GetBusinessList(Business pObjBusiness)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessListNewTy(pObjBusiness.BusinessID, pObjBusiness.BusinessRIN, pObjBusiness.BusinessName, pObjBusiness.BusinessAddress, pObjBusiness.LGAID, pObjBusiness.intStatus).ToList();
            }
        }

        public IList<vw_Business> REP_GetBusinessList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_Business.ToList();
            }
        }

        public usp_GetBusinessListNewTy_Result REP_GetBusinessDetails(Business pObjBusiness)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessListNewTy(pObjBusiness.BusinessID, pObjBusiness.BusinessRIN, pObjBusiness.BusinessName, pObjBusiness.BusinessAddress, pObjBusiness.LGAID, pObjBusiness.intStatus).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateStatus(Business pObjBusiness)
        {
            using (_db = new EIRSEntities())
            {
                Business mObjInsertUpdateBusiness; //Business Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Business
                if (pObjBusiness.BusinessID != 0)
                {
                    mObjInsertUpdateBusiness = (from bcomp in _db.Businesses
                                                where bcomp.BusinessID == pObjBusiness.BusinessID
                                                select bcomp).FirstOrDefault();

                    if (mObjInsertUpdateBusiness != null)
                    {
                        if (mObjInsertUpdateBusiness.Active == true)
                        {
                            //Check Conditions for marking inactive to company

                            var vCompanyList = (from tpa in _db.MAP_TaxPayer_Asset
                                                join comp in _db.Companies on new { CompanyID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { comp.CompanyID, comp.TaxPayerTypeID }
                                                where tpa.AssetID == mObjInsertUpdateBusiness.BusinessID && tpa.AssetTypeID == mObjInsertUpdateBusiness.AssetTypeID && comp.Active == true
                                                select tpa);
                            if (vCompanyList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }

                            var vIndividualList = (from tpa in _db.MAP_TaxPayer_Asset
                                                   join ind in _db.Individuals on new { IndividualID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { ind.IndividualID, ind.TaxPayerTypeID }
                                                   where tpa.AssetID == mObjInsertUpdateBusiness.BusinessID && tpa.AssetTypeID == mObjInsertUpdateBusiness.AssetTypeID && ind.Active == true
                                                   select tpa);
                            if (vIndividualList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }

                            var vGovernmentList = (from tpa in _db.MAP_TaxPayer_Asset
                                                   join gov in _db.Governments on new { GovernmentID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { gov.GovernmentID, gov.TaxPayerTypeID }
                                                   where tpa.AssetID == mObjInsertUpdateBusiness.BusinessID && tpa.AssetTypeID == mObjInsertUpdateBusiness.AssetTypeID && gov.Active == true
                                                   select tpa);
                            if (vGovernmentList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }

                            var vSpecialList = (from tpa in _db.MAP_TaxPayer_Asset
                                                join sp in _db.Specials on new { SpecialID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { sp.SpecialID, sp.TaxPayerTypeID }
                                                where tpa.AssetID == mObjInsertUpdateBusiness.BusinessID && tpa.AssetTypeID == mObjInsertUpdateBusiness.AssetTypeID && sp.Active == true
                                                select tpa);
                            if (vSpecialList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }
                        }

                        mObjInsertUpdateBusiness.Active = !mObjInsertUpdateBusiness.Active;
                        mObjInsertUpdateBusiness.ModifiedBy = pObjBusiness.ModifiedBy;
                        mObjInsertUpdateBusiness.ModifiedDate = pObjBusiness.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Business Updated Successfully";
                           // mObjFuncResponse.AdditionalData = _db.usp_GetBusinessList(0, pObjBusiness.BusinessRIN, pObjBusiness.BusinessName, pObjBusiness.BusinessAddress, pObjBusiness.LGAID, pObjBusiness.intStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Business Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }


        public FuncResponse REP_InsertBusinessBuilding(MAP_Business_Building pObjBusinessBuilding)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                //var vExists = (from tpa in _db.MAP_Business_Building
                //               where tpa.AssetTypeID == pObjTaxPayerAsset.AssetTypeID && tpa.AssetID == pObjTaxPayerAsset.AssetID
                //                  && tpa.TaxPayerTypeID == pObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == pObjTaxPayerAsset.TaxPayerID
                //                  && tpa.TaxPayerRoleID == pObjTaxPayerAsset.TaxPayerRoleID
                //               select tpa);

                //if (vExists.Count() > 0)
                //{
                //    mObjResponse.Success = false;
                //    mObjResponse.Message = "Link Already Exists";
                //    return mObjResponse;
                //}

                _db.MAP_Business_Building.Add(pObjBusinessBuilding);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public IList<usp_GetBusinessBuildingList_Result> REP_GetBusinessBuildingList(MAP_Business_Building pObjBusinessBuilding)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessBuildingList(pObjBusinessBuilding.BuildingID, pObjBusinessBuilding.BusinessID).ToList();
            }
        }

        public MAP_Business_Building REP_GetBusinessBuildingDetails(int pIntBBID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.MAP_Business_Building.Find(pIntBBID);
            }
        }

        public FuncResponse<IList<usp_GetBusinessBuildingList_Result>> REP_RemoveBusinessBuilding(MAP_Business_Building pObjBusinessBuilding)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<IList<usp_GetBusinessBuildingList_Result>> mObjFuncResponse = new FuncResponse<IList<usp_GetBusinessBuildingList_Result>>(); //Return Object

                MAP_Business_Building mObjDeleteBuilding;

                mObjDeleteBuilding = _db.MAP_Business_Building.Find(pObjBusinessBuilding.BBID);

                if (mObjDeleteBuilding == null)
                {
                    mObjFuncResponse.Success = false;
                    if (pObjBusinessBuilding.BusinessID != null)
                    {
                        mObjFuncResponse.Message = "Building Already Removed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Business Already Removed";
                    }
                }
                else
                {
                    _db.MAP_Business_Building.Remove(mObjDeleteBuilding);

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        if (pObjBusinessBuilding.BusinessID != null)
                        {
                            mObjFuncResponse.Message = "Building Removed Successfully";
                        }
                        else
                        {
                            mObjFuncResponse.Message = "Business Removed Successfully";
                        }

                        mObjFuncResponse.AdditionalData = _db.usp_GetBusinessBuildingList(pObjBusinessBuilding.BuildingID, pObjBusinessBuilding.BusinessID).ToList();
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = Ex.Message;
                    }
                }

                return mObjFuncResponse;
            }
        }

        //public IList<usp_GetBusinessChart_Result> REP_GetBusinessChart(int pIntChartType)
        //{
        //    using (_db = new EIRSEntities())
        //    {
        //        return _db.usp_GetBusinessChart(pIntChartType).ToList();
        //    }
        //}

        public IList<usp_SearchBusinessForRDMLoad_Result> REP_SearchBusinessDetails(Business pObjBusiness)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchBusinessForRDMLoad(pObjBusiness.BusinessRIN, pObjBusiness.BusinessName, pObjBusiness.BusinessTypeName, pObjBusiness.LGAName, pObjBusiness.BusinessCategoryName, pObjBusiness.BusinessSectorName, pObjBusiness.BusinessSubSectorName, pObjBusiness.BusinessStructureName, pObjBusiness.BusinessOperationName, pObjBusiness.SizeName, pObjBusiness.ActiveText).ToList();
            }
        }

        public IDictionary<string, object> REP_SearchBusiness(Business pObjBusiness)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>();
                dcData["BusinessList"] = _db.usp_SearchBusiness(pObjBusiness.WhereCondition, pObjBusiness.OrderBy, pObjBusiness.OrderByDirection, pObjBusiness.PageNumber, pObjBusiness.PageSize, pObjBusiness.MainFilter, pObjBusiness.BusinessRIN, pObjBusiness.BusinessName, pObjBusiness.BusinessTypeName, pObjBusiness.LGAName, pObjBusiness.BusinessCategoryName, pObjBusiness.BusinessSectorName, pObjBusiness.BusinessSubSectorName, pObjBusiness.BusinessStructureName, pObjBusiness.BusinessOperationName, pObjBusiness.SizeName, pObjBusiness.ActiveText).ToList();

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(BusinessID) FROM Business").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(BusinessID) ");
                sbFilteredCountQuery.Append(" FROM Business bus ");
                sbFilteredCountQuery.Append(" INNER JOIN Asset_Types atype ON bus.AssetTypeID = atype.AssetTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Business_Types btype ON bus.BusinessTypeID = btype.BusinessTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN LGA lga ON bus.LGAID = lga.LGAID ");
                sbFilteredCountQuery.Append(" INNER JOIN Business_Category bcat ON bus.BusinessCategoryID = bcat.BusinessCategoryID ");
                sbFilteredCountQuery.Append(" INNER JOIN Business_Sector bsect ON bus.BusinessSectorID = bsect.BusinessSectorID ");
                sbFilteredCountQuery.Append(" INNER JOIN Business_SubSector bssect ON bus.BusinessSubSectorID = bssect.BusinessSubSectorID ");
                sbFilteredCountQuery.Append(" INNER JOIN Business_Structure bstruct ON bus.BusinessStructureID = bstruct.BusinessStructureID ");
                sbFilteredCountQuery.Append(" INNER JOIN Business_Operation bopert ON bus.BusinessOperationID = bopert.BusinessOperationID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Sizes sz ON bus.SizeID = sz.SizeID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjBusiness.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@BusinessRIN",pObjBusiness.BusinessRIN),
                    new SqlParameter("@BusinessName",pObjBusiness.BusinessName),
                    new SqlParameter("@BusinessTypeName",pObjBusiness.BusinessTypeName),
                    new SqlParameter("@LGAName",pObjBusiness.LGAName),
                    new SqlParameter("@BusinessCategoryName",pObjBusiness.BusinessCategoryName),
                    new SqlParameter("@BusinessSectorName",pObjBusiness.BusinessSectorName),
                    new SqlParameter("@BusinessSubSectorName",pObjBusiness.BusinessSubSectorName),
                    new SqlParameter("@BusinessStructureName",pObjBusiness.BusinessStructureName),
                    new SqlParameter("@BusinessOperationName",pObjBusiness.BusinessOperationName),
                    new SqlParameter("@SizeName",pObjBusiness.SizeName),
                    new SqlParameter("@ActiveText",pObjBusiness.ActiveText),
                    new SqlParameter("@MainFilter", pObjBusiness.MainFilter)
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

        public IDictionary<string, object> REP_SearchBusinessForSideMenu(Business pObjBusiness)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>();
                dcData["BusinessList"] = _db.usp_SearchBusinessForSideMenu(pObjBusiness.WhereCondition, pObjBusiness.OrderBy, pObjBusiness.OrderByDirection, pObjBusiness.PageNumber, pObjBusiness.PageSize, pObjBusiness.MainFilter).ToList();

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(BusinessID) FROM Business bus INNER JOIN Business_SubSector bssect ON bus.BusinessSubSectorID = bssect.BusinessSubSectorID").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(BusinessID) ");
                sbFilteredCountQuery.Append(" FROM Business bus ");
                sbFilteredCountQuery.Append(" INNER JOIN Business_SubSector bssect ON bus.BusinessSubSectorID = bssect.BusinessSubSectorID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjBusiness.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter", pObjBusiness.MainFilter)
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }
    }
}
