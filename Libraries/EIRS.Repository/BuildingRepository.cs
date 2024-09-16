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
    public class BuildingRepository : IBuildingRepository
    {
        EIRSEntities _db;

        public FuncResponse<Building> REP_InsertUpdateBuilding(Building pObjBuilding)
        {
            using (_db = new EIRSEntities())
            {
                Building mObjInsertUpdateBuilding; //Building Insert Update Object
                FuncResponse<Building> mObjFuncResponse = new FuncResponse<Building>(); //Return Object

                //If Update Load Building Completion

                if (pObjBuilding.BuildingID != 0)
                {
                    mObjInsertUpdateBuilding = (from Building in _db.Buildings
                                                where Building.BuildingID == pObjBuilding.BuildingID
                                                select Building).FirstOrDefault();

                    if (mObjInsertUpdateBuilding != null)
                    {
                        mObjInsertUpdateBuilding.ModifiedBy = pObjBuilding.ModifiedBy;
                        mObjInsertUpdateBuilding.ModifiedDate = pObjBuilding.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateBuilding = new Building();
                        mObjInsertUpdateBuilding.CreatedBy = pObjBuilding.CreatedBy;
                        mObjInsertUpdateBuilding.CreatedDate = pObjBuilding.CreatedDate;
                    }
                }
                else // Else Insert Building Completion
                {
                    mObjInsertUpdateBuilding = new Building();
                    mObjInsertUpdateBuilding.CreatedBy = pObjBuilding.CreatedBy;
                    mObjInsertUpdateBuilding.CreatedDate = pObjBuilding.CreatedDate;
                }

                mObjInsertUpdateBuilding.BuildingName = pObjBuilding.BuildingName;
                mObjInsertUpdateBuilding.BuildingNumber = pObjBuilding.BuildingNumber;
                mObjInsertUpdateBuilding.StreetName = pObjBuilding.StreetName;
                mObjInsertUpdateBuilding.OffStreetName = pObjBuilding.OffStreetName;
                mObjInsertUpdateBuilding.TownID = pObjBuilding.TownID;
                mObjInsertUpdateBuilding.LGAID = pObjBuilding.LGAID;
                mObjInsertUpdateBuilding.WardID = pObjBuilding.WardID;
                mObjInsertUpdateBuilding.AssetTypeID = pObjBuilding.AssetTypeID;
                mObjInsertUpdateBuilding.BuildingTypeID = pObjBuilding.BuildingTypeID;
                mObjInsertUpdateBuilding.BuildingCompletionID = pObjBuilding.BuildingCompletionID;
                mObjInsertUpdateBuilding.BuildingPurposeID = pObjBuilding.BuildingPurposeID;
                mObjInsertUpdateBuilding.BuildingOwnershipID = pObjBuilding.BuildingOwnershipID;
                mObjInsertUpdateBuilding.NoOfUnits = pObjBuilding.NoOfUnits;
                mObjInsertUpdateBuilding.Latitude = pObjBuilding.Latitude;
                mObjInsertUpdateBuilding.Longitude = pObjBuilding.Longitude;
                mObjInsertUpdateBuilding.BuildingSize_Length = pObjBuilding.BuildingSize_Length;
                mObjInsertUpdateBuilding.BuildingSize_Width = pObjBuilding.BuildingSize_Width;
                mObjInsertUpdateBuilding.Active = pObjBuilding.Active;

                if (pObjBuilding.BuildingID == 0)
                {
                    _db.Buildings.Add(mObjInsertUpdateBuilding);
                }

                try
                {
                    _db.SaveChanges();

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);

                    mObjFuncResponse.Success = true;
                    if (pObjBuilding.BuildingID == 0)
                    {
                        mObjFuncResponse.Message = "Building Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Building Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateBuilding;

                    //if Building is Updated , Check For Profile Update
                    if (pObjBuilding.BuildingID > 0)
                    {
                        var vTPAList = (from tpa in _db.MAP_TaxPayer_Asset
                                        where tpa.AssetID == pObjBuilding.BuildingID && tpa.AssetTypeID == pObjBuilding.AssetTypeID
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

                    if (pObjBuilding.BuildingID == 0)
                    {
                        mObjFuncResponse.Message = "Building Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Building Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetBuildingList_Result> REP_GetBuildingList(Building pObjBuilding)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingList(pObjBuilding.BuildingID, pObjBuilding.BuildingRIN, pObjBuilding.BuildingName, pObjBuilding.StreetName, pObjBuilding.LGAID, pObjBuilding.TownID, pObjBuilding.WardID, pObjBuilding.intStatus).ToList();
            }
        }

        public IList<vw_Building> REP_GetBuildingList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_Building.ToList();
            }
        }

        public usp_GetBuildingList_Result REP_GetBuildingDetails(Building pObjBuilding)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingList(pObjBuilding.BuildingID, pObjBuilding.BuildingRIN, pObjBuilding.BuildingName, pObjBuilding.StreetName, pObjBuilding.LGAID, pObjBuilding.TownID, pObjBuilding.WardID, pObjBuilding.intStatus).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateStatus(Building pObjBuilding)
        {
            using (_db = new EIRSEntities())
            {
                Building mObjInsertUpdateBuilding; //Building Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Building
                if (pObjBuilding.BuildingID != 0)
                {
                    mObjInsertUpdateBuilding = (from bcomp in _db.Buildings
                                                where bcomp.BuildingID == pObjBuilding.BuildingID
                                                select bcomp).FirstOrDefault();

                    if (mObjInsertUpdateBuilding != null)
                    {
                        if(mObjInsertUpdateBuilding.Active == true)
                        {
                            //Check Conditions for marking inactive to company

                            var vCompanyList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join comp in _db.Companies on new { CompanyID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { comp.CompanyID, comp.TaxPayerTypeID }
                                                 where tpa.AssetID == mObjInsertUpdateBuilding.BuildingID && tpa.AssetTypeID == mObjInsertUpdateBuilding.AssetTypeID && comp.Active == true
                                                 select tpa);
                            if (vCompanyList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }

                            var vIndividualList = (from tpa in _db.MAP_TaxPayer_Asset
                                                join ind in _db.Individuals on new { IndividualID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { ind.IndividualID, ind.TaxPayerTypeID }
                                                where tpa.AssetID == mObjInsertUpdateBuilding.BuildingID && tpa.AssetTypeID == mObjInsertUpdateBuilding.AssetTypeID && ind.Active == true
                                                select tpa);
                            if (vIndividualList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }

                            var vGovernmentList = (from tpa in _db.MAP_TaxPayer_Asset
                                                join gov in _db.Governments on new { GovernmentID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { gov.GovernmentID, gov.TaxPayerTypeID }
                                                where tpa.AssetID == mObjInsertUpdateBuilding.BuildingID && tpa.AssetTypeID == mObjInsertUpdateBuilding.AssetTypeID && gov.Active == true
                                                select tpa);
                            if (vGovernmentList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }

                            var vSpecialList = (from tpa in _db.MAP_TaxPayer_Asset
                                                join sp in _db.Specials on new { SpecialID = (int)tpa.TaxPayerID, tpa.TaxPayerTypeID } equals new { sp.SpecialID, sp.TaxPayerTypeID }
                                                where tpa.AssetID == mObjInsertUpdateBuilding.BuildingID && tpa.AssetTypeID == mObjInsertUpdateBuilding.AssetTypeID && sp.Active == true
                                                select tpa);
                            if (vSpecialList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active tax payer associated with asset";
                                return mObjFuncResponse;
                            }
                        }

                        mObjInsertUpdateBuilding.Active = !mObjInsertUpdateBuilding.Active;
                        mObjInsertUpdateBuilding.ModifiedBy = pObjBuilding.ModifiedBy;
                        mObjInsertUpdateBuilding.ModifiedDate = pObjBuilding.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Building Updated Successfully";
                            //mObjFuncResponse.AdditionalData = _db.usp_GetBuildingList(0, pObjBuilding.BuildingRIN, pObjBuilding.BuildingName, pObjBuilding.StreetName, pObjBuilding.LGAID, pObjBuilding.TownID, pObjBuilding.WardID, pObjBuilding.intStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Building Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertBuildingLand(MAP_Building_Land pObjBuildingLand)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                _db.MAP_Building_Land.Add(pObjBuildingLand);

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

        public IList<usp_GetBuildingLandList_Result> REP_GetBuildingLandList(MAP_Building_Land pObjBuildingLand)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingLandList(pObjBuildingLand.BuildingID, pObjBuildingLand.LandID, pObjBuildingLand.TaxPayerTypeID, pObjBuildingLand.TaxPayerID).ToList();
            }
        }

        public MAP_Building_Land REP_GetBuildingLandDetails(int pIntBBID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.MAP_Building_Land.Find(pIntBBID);
            }
        }

        public FuncResponse<IList<usp_GetBuildingLandList_Result>> REP_RemoveBuildingLand(MAP_Building_Land pObjBuildingLand)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<IList<usp_GetBuildingLandList_Result>> mObjFuncResponse = new FuncResponse<IList<usp_GetBuildingLandList_Result>>(); //Return Object

                MAP_Building_Land mObjDeleteBuilding;

                mObjDeleteBuilding = _db.MAP_Building_Land.Find(pObjBuildingLand.BLID);

                if (mObjDeleteBuilding == null)
                {
                    mObjFuncResponse.Success = false;
                    if (pObjBuildingLand.LandID != null)
                    {
                        mObjFuncResponse.Message = "Building Already Removed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Land Already Removed";
                    }
                }
                else
                {
                    _db.MAP_Building_Land.Remove(mObjDeleteBuilding);

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        if (pObjBuildingLand.LandID != null)
                        {
                            mObjFuncResponse.Message = "Building Removed Successfully";
                        }
                        else
                        {
                            mObjFuncResponse.Message = "Land Removed Successfully";
                        }

                        mObjFuncResponse.AdditionalData = _db.usp_GetBuildingLandList(pObjBuildingLand.BuildingID, pObjBuildingLand.LandID, pObjBuildingLand.TaxPayerTypeID, pObjBuildingLand.TaxPayerID).ToList();
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

        public FuncResponse REP_InsertBuildingUnitNumber(MAP_Building_BuildingUnit pObjBuildingUnitNumber)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                _db.MAP_Building_BuildingUnit.Add(pObjBuildingUnitNumber);

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

        public IList<usp_GetBuildingUnitNumberList_Result> REP_GetBuildingUnitNumberList(MAP_Building_BuildingUnit pObjBuildingUnitNumber)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingUnitNumberList(pObjBuildingUnitNumber.BuildingID, pObjBuildingUnitNumber.BuildingUnitID).ToList();
            }
        }

        public MAP_Building_BuildingUnit REP_GetBuildingUnitNumberDetails(int pIntBBID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.MAP_Building_BuildingUnit.Find(pIntBBID);
            }
        }

        public FuncResponse<IList<usp_GetBuildingUnitNumberList_Result>> REP_RemoveBuildingUnitNumber(MAP_Building_BuildingUnit pObjBuildingUnitNumber)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<IList<usp_GetBuildingUnitNumberList_Result>> mObjFuncResponse = new FuncResponse<IList<usp_GetBuildingUnitNumberList_Result>>(); //Return Object

                MAP_Building_BuildingUnit mObjDeleteBuilding;

                mObjDeleteBuilding = _db.MAP_Building_BuildingUnit.Find(pObjBuildingUnitNumber.BBUID);

                if (mObjDeleteBuilding == null)
                {
                    mObjFuncResponse.Success = false;
                    if (pObjBuildingUnitNumber.BuildingUnitID != null)
                    {
                        mObjFuncResponse.Message = "Building Already Removed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Building Unit Already Removed";
                    }
                }
                else
                {
                    _db.MAP_Building_BuildingUnit.Remove(mObjDeleteBuilding);

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        if (pObjBuildingUnitNumber.BuildingUnitID != null)
                        {
                            mObjFuncResponse.Message = "Building Removed Successfully";
                        }
                        else
                        {
                            mObjFuncResponse.Message = "Building Unit Removed Successfully";
                        }

                        mObjFuncResponse.AdditionalData = _db.usp_GetBuildingUnitNumberList(pObjBuildingUnitNumber.BuildingID, pObjBuildingUnitNumber.BuildingUnitID).ToList();
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

        //public IList<usp_GetBuildingChart_Result> REP_GetBuildingChart(int pIntChartType)
        //{
        //    using (_db = new EIRSEntities())
        //    {
        //        return _db.usp_GetBuildingChart(pIntChartType).ToList();
        //    }
        //}

        public IList<usp_SearchBuildingForRDMLoad_Result> REP_SearchBuildingDetails(Building pObjBuilding)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchBuildingForRDMLoad(pObjBuilding.BuildingRIN, pObjBuilding.BuildingTagNumber, pObjBuilding.BuildingName, pObjBuilding.BuildingNumber, pObjBuilding.StreetName,
                                  pObjBuilding.OffStreetName, pObjBuilding.TownName, pObjBuilding.LGAName, pObjBuilding.WardName, pObjBuilding.BuildingTypeName, pObjBuilding.BuildingCompletionName, pObjBuilding.BuildingPurposeName,
                                  pObjBuilding.BuildingOwnershipName, pObjBuilding.strNoOfUnits, pObjBuilding.strBuildingSize_Length, pObjBuilding.strBuildingSize_Width, pObjBuilding.Latitude, pObjBuilding.Longitude, pObjBuilding.ActiveText).ToList();
            }
        }

        public IDictionary<string, object> REP_SearchBuilding(Building pObjBuilding)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>();
                dcData["BuildingList"] = _db.usp_SearchBuilding(pObjBuilding.WhereCondition, pObjBuilding.OrderBy, pObjBuilding.OrderByDirection, pObjBuilding.PageNumber, pObjBuilding.PageSize, pObjBuilding.MainFilter,
                                                                pObjBuilding.BuildingRIN, pObjBuilding.BuildingTagNumber, pObjBuilding.BuildingName, pObjBuilding.BuildingNumber, pObjBuilding.StreetName,
                                                                pObjBuilding.OffStreetName, pObjBuilding.TownName, pObjBuilding.LGAName, pObjBuilding.WardName, pObjBuilding.BuildingTypeName, pObjBuilding.BuildingCompletionName, pObjBuilding.BuildingPurposeName,
                                                                pObjBuilding.BuildingOwnershipName, pObjBuilding.strNoOfUnits, pObjBuilding.strBuildingSize_Length, pObjBuilding.strBuildingSize_Width, pObjBuilding.Latitude, pObjBuilding.Longitude, pObjBuilding.ActiveText).ToList();

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(BuildingID) FROM Building").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(BuildingID) ");
                sbFilteredCountQuery.Append(" FROM Building bld ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Town twn ON bld.TownID = twn.TownID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN LGA lga ON bld.LGAID = lga.LGAID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Ward wrd ON bld.WardID = wrd.WardID ");
                sbFilteredCountQuery.Append(" INNER JOIN Asset_Types atype ON bld.AssetTypeID = atype.AssetTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Building_Types btype ON bld.BuildingTypeID = btype.BuildingTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Building_Completion bcomp ON bld.BuildingCompletionID = bcomp.BuildingCompletionID ");
                sbFilteredCountQuery.Append(" INNER JOIN Building_Purpose bpurp ON bld.BuildingPurposeID = bpurp.BuildingPurposeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Building_Ownership bowner ON bld.BuildingOwnershipID = bowner.BuildingOwnershipID  WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjBuilding.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@BuildingRIN",pObjBuilding.BuildingRIN),
                    new SqlParameter("@BuildingTagNumber",pObjBuilding.BuildingTagNumber),
                    new SqlParameter("@BuildingName",pObjBuilding.BuildingName),
                    new SqlParameter("@BuildingNumber",pObjBuilding.BuildingNumber),
                    new SqlParameter("@StreetName",pObjBuilding.StreetName),
                    new SqlParameter("@OffStreetName",pObjBuilding.OffStreetName),
                    new SqlParameter("@TownName",pObjBuilding.TownName),
                    new SqlParameter("@LGAName",pObjBuilding.LGAName),
                    new SqlParameter("@WardName",pObjBuilding.WardName),
                    new SqlParameter("@BuildingTypeName",pObjBuilding.BuildingTypeName),
                    new SqlParameter("@BuildingCompletionName",pObjBuilding.BuildingCompletionName),
                    new SqlParameter("@BuildingPurposeName",pObjBuilding.BuildingPurposeName),
                    new SqlParameter("@BuildingOwnershipName",pObjBuilding.BuildingOwnershipName),
                    new SqlParameter("@NoOfUnits",pObjBuilding.strNoOfUnits),
                    new SqlParameter("@BuildingSize_Length",pObjBuilding.strBuildingSize_Length),
                    new SqlParameter("@BuildingSize_Width",pObjBuilding.strBuildingSize_Width),
                    new SqlParameter("@Latitude",pObjBuilding.Latitude),
                    new SqlParameter("@Longitude",pObjBuilding.Longitude),
                    new SqlParameter("@ActiveText",pObjBuilding.ActiveText),
                    new SqlParameter("@MainFilter", pObjBuilding.MainFilter)
                };

                //Get Filtered Count
                //int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = 0;

                return dcData;
            }
        }

        public IDictionary<string, object> REP_SearchBuildingForSideMenu(Building pObjBuilding)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>();
                dcData["BuildingList"] = _db.usp_SearchBuildingForSideMenu(pObjBuilding.WhereCondition, pObjBuilding.OrderBy, pObjBuilding.OrderByDirection, pObjBuilding.PageNumber, pObjBuilding.PageSize, pObjBuilding.MainFilter).ToList();

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(BuildingID) FROM Building bld INNER JOIN Building_Purpose bpurp ON bld.BuildingPurposeID = bpurp.BuildingPurposeID").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(BuildingID) ");
                sbFilteredCountQuery.Append(" FROM Building bld ");
                sbFilteredCountQuery.Append(" INNER JOIN Building_Purpose bpurp ON bld.BuildingPurposeID = bpurp.BuildingPurposeID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjBuilding.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter", pObjBuilding.MainFilter)
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
