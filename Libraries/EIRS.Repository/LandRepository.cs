using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EIRS.Repository
{
    public class LandRepository : ILandRepository
    {
        EIRSEntities _db;

        public FuncResponse<Land> REP_InsertUpdateLand(Land pObjLand)
        {
            using (_db = new EIRSEntities())
            {
                Land mObjInsertUpdateLand; //Land Insert Update Object
                FuncResponse<Land> mObjFuncResponse = new FuncResponse<Land>(); //Return Object

                //If Update Load Land Completion

                if (pObjLand.LandID != 0)
                {
                    mObjInsertUpdateLand = (from Land in _db.Lands
                                            where Land.LandID == pObjLand.LandID
                                            select Land).FirstOrDefault();

                    if (mObjInsertUpdateLand != null)
                    {
                        mObjInsertUpdateLand.ModifiedBy = pObjLand.ModifiedBy;
                        mObjInsertUpdateLand.ModifiedDate = pObjLand.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateLand = new Land();
                        mObjInsertUpdateLand.CreatedBy = pObjLand.CreatedBy;
                        mObjInsertUpdateLand.CreatedDate = pObjLand.CreatedDate;
                    }
                }
                else // Else Insert Land Completion
                {
                    mObjInsertUpdateLand = new Land();
                    mObjInsertUpdateLand.CreatedBy = pObjLand.CreatedBy;
                    mObjInsertUpdateLand.CreatedDate = pObjLand.CreatedDate;
                }

                mObjInsertUpdateLand.PlotNumber = pObjLand.PlotNumber;
                mObjInsertUpdateLand.StreetName = pObjLand.StreetName;
                mObjInsertUpdateLand.TownID = pObjLand.TownID;
                mObjInsertUpdateLand.LGAID = pObjLand.LGAID;
                mObjInsertUpdateLand.WardID = pObjLand.WardID;
                mObjInsertUpdateLand.AssetTypeID = pObjLand.AssetTypeID;
                mObjInsertUpdateLand.LandSize_Length = pObjLand.LandSize_Length;
                mObjInsertUpdateLand.LandSize_Width = pObjLand.LandSize_Width;
                mObjInsertUpdateLand.C_OF_O_Ref = pObjLand.C_OF_O_Ref;
                mObjInsertUpdateLand.LandPurposeID = pObjLand.LandPurposeID;
                mObjInsertUpdateLand.LandFunctionID = pObjLand.LandFunctionID;
                mObjInsertUpdateLand.LandOwnershipID = pObjLand.LandOwnershipID;
                mObjInsertUpdateLand.LandDevelopmentID = pObjLand.LandDevelopmentID;
                mObjInsertUpdateLand.Latitude = pObjLand.Latitude;
                mObjInsertUpdateLand.Longitude = pObjLand.Longitude;
                mObjInsertUpdateLand.ValueOfLand = pObjLand.ValueOfLand;
                mObjInsertUpdateLand.LandStreetConditionID = pObjLand.LandStreetConditionID;
                mObjInsertUpdateLand.Neighborhood = pObjLand.Neighborhood;
                mObjInsertUpdateLand.Active = pObjLand.Active;

                if (pObjLand.LandID == 0)
                {
                    _db.Lands.Add(mObjInsertUpdateLand);
                }

                try
                {
                    _db.SaveChanges();

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);

                    mObjFuncResponse.Success = true;
                    if (pObjLand.LandID == 0)
                    {
                        mObjFuncResponse.Message = "Land Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Land Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateLand;

                    //if land is Updated , Check For Profile Update
                    if (pObjLand.LandID > 0)
                    {
                        var vTPAList = (from tpa in _db.MAP_TaxPayer_Asset
                                        where tpa.AssetID == pObjLand.LandID && tpa.AssetTypeID == pObjLand.AssetTypeID
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

                    if (pObjLand.LandID == 0)
                    {
                        mObjFuncResponse.Message = "Land Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Land Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetLandList_Result> REP_GetLandList(Land pObjLand)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLandList(pObjLand.LandID, pObjLand.LandRIN, pObjLand.PlotNumber, pObjLand.LandOccupier, pObjLand.C_OF_O_Ref, pObjLand.LGAID, pObjLand.TownID, pObjLand.WardID, pObjLand.intStatus).ToList();
            }
        }

        public IList<vw_Land> REP_GetLandList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_Land.ToList();
            }
        }

        public usp_GetLandList_Result REP_GetLandDetails(Land pObjLand)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLandList(pObjLand.LandID, pObjLand.LandRIN, pObjLand.PlotNumber, pObjLand.LandOccupier, pObjLand.C_OF_O_Ref, pObjLand.LGAID, pObjLand.TownID, pObjLand.WardID, pObjLand.intStatus).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateStatus(Land pObjLand)
        {
            using (_db = new EIRSEntities())
            {
                Land mObjInsertUpdateLand; //Land Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Land
                if (pObjLand.LandID != 0)
                {
                    mObjInsertUpdateLand = (from bcomp in _db.Lands
                                            where bcomp.LandID == pObjLand.LandID
                                            select bcomp).FirstOrDefault();

                    if (mObjInsertUpdateLand != null)
                    {
                        if (mObjInsertUpdateLand.Active == true)
                        {
                            //Check Conditions for marking inactive to company

                            var vCompanyList = (from tpa in _db.MAP_TaxPayer_Asset
                                                join comp in _db.Companies on new { CompanyID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { comp.CompanyID, comp.TaxPayerTypeID }
                                                where tpa.AssetID == mObjInsertUpdateLand.LandID && tpa.AssetTypeID == mObjInsertUpdateLand.AssetTypeID && comp.Active == true
                                                select tpa);
                            if (vCompanyList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }

                            var vIndividualList = (from tpa in _db.MAP_TaxPayer_Asset
                                                   join ind in _db.Individuals on new { IndividualID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { ind.IndividualID, ind.TaxPayerTypeID }
                                                   where tpa.AssetID == mObjInsertUpdateLand.LandID && tpa.AssetTypeID == mObjInsertUpdateLand.AssetTypeID && ind.Active == true
                                                   select tpa);
                            if (vIndividualList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }

                            var vGovernmentList = (from tpa in _db.MAP_TaxPayer_Asset
                                                   join gov in _db.Governments on new { GovernmentID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { gov.GovernmentID, gov.TaxPayerTypeID }
                                                   where tpa.AssetID == mObjInsertUpdateLand.LandID && tpa.AssetTypeID == mObjInsertUpdateLand.AssetTypeID && gov.Active == true
                                                   select tpa);
                            if (vGovernmentList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }

                            var vSpecialList = (from tpa in _db.MAP_TaxPayer_Asset
                                                join sp in _db.Specials on new { SpecialID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { sp.SpecialID, sp.TaxPayerTypeID }
                                                where tpa.AssetID == mObjInsertUpdateLand.LandID && tpa.AssetTypeID == mObjInsertUpdateLand.AssetTypeID && sp.Active == true
                                                select tpa);
                            if (vSpecialList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }
                        }

                        mObjInsertUpdateLand.Active = !mObjInsertUpdateLand.Active;
                        mObjInsertUpdateLand.ModifiedBy = pObjLand.ModifiedBy;
                        mObjInsertUpdateLand.ModifiedDate = pObjLand.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Land Updated Successfully";
                           // mObjFuncResponse.AdditionalData = _db.usp_GetLandList(0, pObjLand.LandRIN, pObjLand.PlotNumber, pObjLand.LandOccupier, pObjLand.C_OF_O_Ref, pObjLand.LGAID, pObjLand.TownID, pObjLand.WardID, pObjLand.intStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Land Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetSearchLandForEdoGIS_Result> REP_SearchLandForEdoGIS(Land pObjLand)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetSearchLandForEdoGIS(pObjLand.LandRIN, pObjLand.PlotNumber, pObjLand.LandOccupier, pObjLand.TaxPayerTypeID, pObjLand.TaxPayerID).ToList();
            }
        }

        //public IList<usp_GetLandChart_Result> REP_GetLandChart(int pIntChartType)
        //{
        //    using (_db = new EIRSEntities())
        //    {
        //        return _db.usp_GetLandChart(pIntChartType).ToList();
        //    }
        //}

        public IList<usp_SearchLandForRDMLoad_Result> REP_SearchLandDetails(Land pObjLand)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchLandForRDMLoad(pObjLand.LandRIN, pObjLand.PlotNumber, pObjLand.StreetName, pObjLand.TownName, pObjLand.LGAName, pObjLand.WardName,
                                       pObjLand.strLandSize_Length, pObjLand.strLandSize_Width, pObjLand.C_OF_O_Ref, pObjLand.LandPurposeName, pObjLand.LandFunctionName, pObjLand.LandOwnershipName,
                                       pObjLand.LandDevelopmentName, pObjLand.Latitude, pObjLand.Longitude, pObjLand.LandStreetConditionName, pObjLand.strValueOfLand, pObjLand.Neighborhood, pObjLand.ActiveText).ToList();
            }
        }

        public IDictionary<string, object> REP_SearchLand(Land pObjLand)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>();
                dcData["LandList"] = _db.usp_SearchLand(pObjLand.WhereCondition, pObjLand.OrderBy, pObjLand.OrderByDirection, pObjLand.PageNumber, pObjLand.PageSize, pObjLand.MainFilter, pObjLand.LandRIN, pObjLand.PlotNumber, pObjLand.StreetName, pObjLand.TownName, pObjLand.LGAName, pObjLand.WardName,
                                       pObjLand.strLandSize_Length, pObjLand.strLandSize_Width, pObjLand.C_OF_O_Ref, pObjLand.LandPurposeName, pObjLand.LandFunctionName, pObjLand.LandOwnershipName,
                                       pObjLand.LandDevelopmentName, pObjLand.Latitude, pObjLand.Longitude, pObjLand.LandStreetConditionName, pObjLand.strValueOfLand, pObjLand.Neighborhood, pObjLand.ActiveText).ToList();

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(LandID) FROM Land").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(LandID) ");
                sbFilteredCountQuery.Append(" FROM Land lnd ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Town twn ON lnd.TownID = twn.TownID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN LGA lga ON lnd.LGAID = lga.LGAID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Ward wrd ON lnd.WardID = wrd.WardID ");
                sbFilteredCountQuery.Append(" INNER JOIN Asset_Types atype ON lnd.AssetTypeID = atype.AssetTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Land_Purpose lpurp ON lnd.LandPurposeID = lpurp.LandPurposeID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Land_Function lfunc ON lnd.LandFunctionID = lfunc.LandFunctionID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Land_Ownership lowner ON lnd.LandOwnershipID = lowner.LandOwnershipID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Land_Development ldev ON lnd.LandDevelopmentID = ldev.LandDevelopmentID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Land_StreetCondition lscond ON lnd.LandStreetConditionID = lscond.LandStreetConditionID  WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjLand.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjLand.MainFilter),
                    new SqlParameter("@LandRIN",pObjLand.LandRIN),
                    new SqlParameter("@PlotNumber",pObjLand.PlotNumber),
                    new SqlParameter("@StreetName",pObjLand.StreetName),
                    new SqlParameter("@TownName",pObjLand.TownName),
                    new SqlParameter("@LGAName",pObjLand.LGAName),
                    new SqlParameter("@WardName",pObjLand.WardName),
                    new SqlParameter("@LandSize_Length",pObjLand.strLandSize_Length),
                    new SqlParameter("@LandSize_Width",pObjLand.strLandSize_Width),
                    new SqlParameter("@C_OF_O_Ref",pObjLand.C_OF_O_Ref),
                    new SqlParameter("@LandPurposeName",pObjLand.LandPurposeName),
                    new SqlParameter("@LandFunctionName",pObjLand.LandFunctionName),
                    new SqlParameter("@LandOwnershipName",pObjLand.LandOwnershipName),
                    new SqlParameter("@LandDevelopmentName",pObjLand.LandDevelopmentName),
                    new SqlParameter("@Latitude",pObjLand.Latitude),
                    new SqlParameter("@Longitude",pObjLand.Longitude),
                    new SqlParameter("@LandStreetConditionName",pObjLand.LandStreetConditionName),
                    new SqlParameter("@ValueOfLand",pObjLand.strValueOfLand),
                    new SqlParameter("@Neighborhood",pObjLand.Neighborhood),
                    new SqlParameter("@ActiveText",pObjLand.ActiveText),
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

        public IDictionary<string, object> REP_SearchLandForSideMenu(Land pObjLand)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>();
                dcData["LandList"] = _db.usp_SearchLandForSideMenu(pObjLand.WhereCondition, pObjLand.OrderBy, pObjLand.OrderByDirection, pObjLand.PageNumber, pObjLand.PageSize, pObjLand.MainFilter).ToList();

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(LandID) FROM Land lnd LEFT OUTER JOIN Land_Function lfunc ON lnd.LandFunctionID = lfunc.LandFunctionID").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(LandID) ");
                sbFilteredCountQuery.Append(" FROM Land lnd ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Land_Function lfunc ON lnd.LandFunctionID = lfunc.LandFunctionID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjLand.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjLand.MainFilter)
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
