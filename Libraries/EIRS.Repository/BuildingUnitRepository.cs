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
    public class BuildingUnitRepository : IBuildingUnitRepository
    {
        EIRSEntities _db;

        public FuncResponse<Building_Unit> REP_InsertUpdateBuildingUnit(Building_Unit pObjBuildingUnit)
        {
            using (_db = new EIRSEntities())
            {
                Building_Unit mObjInsertUpdateBuildingUnit; //Building Unit Insert Update Object
                FuncResponse<Building_Unit> mObjFuncResponse = new FuncResponse<Building_Unit>(); //Return Object

                ////Check if Duplicate
                //var vDuplicateCheck = (from bpurp in _db.Building_Unit
                //                       where bpurp.UnitNumber == pObjBuildingUnit.UnitNumber && bpurp.BuildingUnitID != pObjBuildingUnit.BuildingUnitID
                //                       select bpurp);

                //if (vDuplicateCheck.Count() > 0)
                //{
                //    mObjFuncResponse.Success = false;
                //    mObjFuncResponse.Message = "Building Unit already exists";
                //    return mObjFuncResponse;
                //}

                //If Update Load Building Unit
                if (pObjBuildingUnit.BuildingUnitID != 0)
                {
                    mObjInsertUpdateBuildingUnit = (from BuildingUnit in _db.Building_Unit
                                                    where BuildingUnit.BuildingUnitID == pObjBuildingUnit.BuildingUnitID
                                                    select BuildingUnit).FirstOrDefault();

                    if (mObjInsertUpdateBuildingUnit != null)
                    {
                        mObjInsertUpdateBuildingUnit.ModifiedBy = pObjBuildingUnit.ModifiedBy;
                        mObjInsertUpdateBuildingUnit.ModifiedDate = pObjBuildingUnit.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateBuildingUnit = new Building_Unit();
                        mObjInsertUpdateBuildingUnit.CreatedBy = pObjBuildingUnit.CreatedBy;
                        mObjInsertUpdateBuildingUnit.CreatedDate = pObjBuildingUnit.CreatedDate;
                    }
                }
                else // Else Insert Building Unit
                {
                    mObjInsertUpdateBuildingUnit = new Building_Unit();
                    mObjInsertUpdateBuildingUnit.CreatedBy = pObjBuildingUnit.CreatedBy;
                    mObjInsertUpdateBuildingUnit.CreatedDate = pObjBuildingUnit.CreatedDate;
                }

                mObjInsertUpdateBuildingUnit.UnitNumber = pObjBuildingUnit.UnitNumber;
                mObjInsertUpdateBuildingUnit.UnitPurposeID = pObjBuildingUnit.UnitPurposeID;
                mObjInsertUpdateBuildingUnit.UnitFunctionID = pObjBuildingUnit.UnitFunctionID;
                mObjInsertUpdateBuildingUnit.UnitOccupancyID = pObjBuildingUnit.UnitOccupancyID;
                mObjInsertUpdateBuildingUnit.SizeID = pObjBuildingUnit.SizeID;
                mObjInsertUpdateBuildingUnit.Active = pObjBuildingUnit.Active;

                if (pObjBuildingUnit.BuildingUnitID == 0)
                {
                    _db.Building_Unit.Add(mObjInsertUpdateBuildingUnit);
                }

                try
                {
                    _db.SaveChanges();

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);

                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.AdditionalData = mObjInsertUpdateBuildingUnit;
                    if (pObjBuildingUnit.BuildingUnitID == 0)
                    {
                        mObjFuncResponse.Message = "Building Unit Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Building Unit Updated Successfully";
                    }
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjBuildingUnit.BuildingUnitID == 0)
                    {
                        mObjFuncResponse.Message = "Building Unit Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Building Unit Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetBuildingUnitList_Result> REP_GetBuildingUnitList(Building_Unit pObjBuildingUnit)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingUnitList(pObjBuildingUnit.UnitNumber, pObjBuildingUnit.BuildingUnitID, pObjBuildingUnit.BuildingUnitIds, pObjBuildingUnit.intStatus, pObjBuildingUnit.IncludeBuildingUnitIds, pObjBuildingUnit.ExcludeBuildingUnitIds).ToList();
            }
        }

        public usp_GetBuildingUnitList_Result REP_GetBuildingUnitDetails(Building_Unit pObjBuildingUnit)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingUnitList(pObjBuildingUnit.UnitNumber, pObjBuildingUnit.BuildingUnitID, pObjBuildingUnit.BuildingUnitIds, pObjBuildingUnit.intStatus, pObjBuildingUnit.IncludeBuildingUnitIds, pObjBuildingUnit.ExcludeBuildingUnitIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetBuildingUnitDropDownList(Building_Unit pObjBuildingUnit)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from bpurp in _db.usp_GetBuildingUnitList(pObjBuildingUnit.UnitNumber, pObjBuildingUnit.BuildingUnitID, pObjBuildingUnit.BuildingUnitIds, pObjBuildingUnit.intStatus, pObjBuildingUnit.IncludeBuildingUnitIds, pObjBuildingUnit.ExcludeBuildingUnitIds)
                               select new DropDownListResult()
                               {
                                   id = bpurp.BuildingUnitID.GetValueOrDefault(),
                                   text = bpurp.UnitNumber
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Building_Unit pObjBuildingUnit)
        {
            using (_db = new EIRSEntities())
            {
                Building_Unit mObjInsertUpdateBuildingUnit; //Building Unit Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load BuildingUnit
                if (pObjBuildingUnit.BuildingUnitID != 0)
                {
                    mObjInsertUpdateBuildingUnit = (from bpurp in _db.Building_Unit
                                                    where bpurp.BuildingUnitID == pObjBuildingUnit.BuildingUnitID
                                                    select bpurp).FirstOrDefault();

                    if (mObjInsertUpdateBuildingUnit != null)
                    {
                        mObjInsertUpdateBuildingUnit.Active = !mObjInsertUpdateBuildingUnit.Active;
                        mObjInsertUpdateBuildingUnit.ModifiedBy = pObjBuildingUnit.ModifiedBy;
                        mObjInsertUpdateBuildingUnit.ModifiedDate = pObjBuildingUnit.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Building Unit Updated Successfully";
                           // mObjFuncResponse.AdditionalData = _db.usp_GetBuildingUnitList(pObjBuildingUnit.UnitNumber, 0, pObjBuildingUnit.BuildingUnitIds, pObjBuildingUnit.intStatus, pObjBuildingUnit.IncludeBuildingUnitIds, pObjBuildingUnit.ExcludeBuildingUnitIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Building Unit Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_SearchBuildingUnitForRDMLoad_Result> REP_SearchBuildingUnitDetails(Building_Unit pObjBuildingUnit)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchBuildingUnitForRDMLoad(pObjBuildingUnit.UnitNumber, pObjBuildingUnit.UnitPurposeName, pObjBuildingUnit.UnitFunctionName, pObjBuildingUnit.UnitOccupancyName, pObjBuildingUnit.SizeName, pObjBuildingUnit.ActiveText).ToList();
            }
        }

        public IDictionary<string, object> REP_SearchBuildingUnit(Building_Unit pObjBuildingUnit)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>();
                dcData["BuildingUnitList"] = _db.usp_SearchBuildingUnit(pObjBuildingUnit.WhereCondition, pObjBuildingUnit.OrderBy, pObjBuildingUnit.OrderByDirection, pObjBuildingUnit.PageNumber, pObjBuildingUnit.PageSize, pObjBuildingUnit.MainFilter,
                                                                        pObjBuildingUnit.UnitNumber, pObjBuildingUnit.UnitPurposeName, pObjBuildingUnit.UnitFunctionName, pObjBuildingUnit.UnitOccupancyName, pObjBuildingUnit.SizeName, pObjBuildingUnit.ActiveText).ToList();

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(BuildingUnitID) FROM Building_Unit").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(BuildingUnitID) ");
                sbFilteredCountQuery.Append(" FROM Building_Unit bunit ");
                sbFilteredCountQuery.Append(" INNER JOIN Unit_Purpose upurp ON bunit.UnitPurposeID = upurp.UnitPurposeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Unit_Function ufunc ON bunit.UnitFunctionID = ufunc.UnitFunctionID ");
                sbFilteredCountQuery.Append(" INNER JOIN Unit_Occupancy uoccp ON bunit.UnitOccupancyID = uoccp.UnitOccupancyID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Sizes sz ON bunit.SizeID = sz.SizeID  WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjBuildingUnit.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjBuildingUnit.MainFilter),
                    new SqlParameter("@UnitNumber",pObjBuildingUnit.UnitNumber),
                    new SqlParameter("@UnitPurposeName",pObjBuildingUnit.UnitPurposeName),
                    new SqlParameter("@UnitFunctionName",pObjBuildingUnit.UnitFunctionName),
                    new SqlParameter("@UnitOccupancyName",pObjBuildingUnit.UnitOccupancyName),
                    new SqlParameter("@SizeName",pObjBuildingUnit.SizeName),
                    new SqlParameter("@ActiveText",pObjBuildingUnit.ActiveText),
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
