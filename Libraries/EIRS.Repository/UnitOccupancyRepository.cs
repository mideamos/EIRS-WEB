using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class UnitOccupancyRepository : IUnitOccupancyRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateUnitOccupancy(Unit_Occupancy pObjUnitOccupancy)
        {
            using (_db = new EIRSEntities())
            {
                Unit_Occupancy mObjInsertUpdateUnitOccupancy; //Unit Occupancy Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from uoccup in _db.Unit_Occupancy
                                       where uoccup.UnitOccupancyName == pObjUnitOccupancy.UnitOccupancyName && uoccup.UnitOccupancyID != pObjUnitOccupancy.UnitOccupancyID
                                       select uoccup);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Unit Occupancy already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Unit Occupancy
                if (pObjUnitOccupancy.UnitOccupancyID != 0)
                {
                    mObjInsertUpdateUnitOccupancy = (from UnitOccupancy in _db.Unit_Occupancy
                                                 where UnitOccupancy.UnitOccupancyID == pObjUnitOccupancy.UnitOccupancyID
                                                 select UnitOccupancy).FirstOrDefault();

                    if (mObjInsertUpdateUnitOccupancy != null)
                    {
                        mObjInsertUpdateUnitOccupancy.ModifiedBy = pObjUnitOccupancy.ModifiedBy;
                        mObjInsertUpdateUnitOccupancy.ModifiedDate = pObjUnitOccupancy.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateUnitOccupancy = new Unit_Occupancy();
                        mObjInsertUpdateUnitOccupancy.CreatedBy = pObjUnitOccupancy.CreatedBy;
                        mObjInsertUpdateUnitOccupancy.CreatedDate = pObjUnitOccupancy.CreatedDate;
                    }
                }
                else // Else Insert Unit Occupancy
                {
                    mObjInsertUpdateUnitOccupancy = new Unit_Occupancy();
                    mObjInsertUpdateUnitOccupancy.CreatedBy = pObjUnitOccupancy.CreatedBy;
                    mObjInsertUpdateUnitOccupancy.CreatedDate = pObjUnitOccupancy.CreatedDate;
                }

                mObjInsertUpdateUnitOccupancy.UnitOccupancyName = pObjUnitOccupancy.UnitOccupancyName;
                mObjInsertUpdateUnitOccupancy.Active = pObjUnitOccupancy.Active;

                if (pObjUnitOccupancy.UnitOccupancyID == 0)
                {
                    _db.Unit_Occupancy.Add(mObjInsertUpdateUnitOccupancy);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjUnitOccupancy.UnitOccupancyID == 0)
                        mObjFuncResponse.Message = "Unit Occupancy Added Successfully";
                    else
                        mObjFuncResponse.Message = "Unit Occupancy Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjUnitOccupancy.UnitOccupancyID == 0)
                        mObjFuncResponse.Message = "Unit Occupancy Addition Failed";
                    else
                        mObjFuncResponse.Message = "Unit Occupancy Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetUnitOccupancyList_Result> REP_GetUnitOccupancyList(Unit_Occupancy pObjUnitOccupancy)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetUnitOccupancyList(pObjUnitOccupancy.UnitOccupancyName, pObjUnitOccupancy.UnitOccupancyID, pObjUnitOccupancy.UnitOccupancyIds, pObjUnitOccupancy.intStatus, pObjUnitOccupancy.IncludeUnitOccupancyIds, pObjUnitOccupancy.ExcludeUnitOccupancyIds).ToList();
            }
        }

        public usp_GetUnitOccupancyList_Result REP_GetUnitOccupancyDetails(Unit_Occupancy pObjUnitOccupancy)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetUnitOccupancyList(pObjUnitOccupancy.UnitOccupancyName, pObjUnitOccupancy.UnitOccupancyID, pObjUnitOccupancy.UnitOccupancyIds, pObjUnitOccupancy.intStatus, pObjUnitOccupancy.IncludeUnitOccupancyIds, pObjUnitOccupancy.ExcludeUnitOccupancyIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetUnitOccupancyDropDownList(Unit_Occupancy pObjUnitOccupancy)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from uoccup in _db.usp_GetUnitOccupancyList(pObjUnitOccupancy.UnitOccupancyName, pObjUnitOccupancy.UnitOccupancyID, pObjUnitOccupancy.UnitOccupancyIds, pObjUnitOccupancy.intStatus, pObjUnitOccupancy.IncludeUnitOccupancyIds, pObjUnitOccupancy.ExcludeUnitOccupancyIds)
                               select new DropDownListResult()
                               {
                                   id = uoccup.UnitOccupancyID.GetValueOrDefault(),
                                   text = uoccup.UnitOccupancyName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Unit_Occupancy pObjUnitOccupancy)
        {
            using (_db = new EIRSEntities())
            {
                Unit_Occupancy mObjInsertUpdateUnitOccupancy; //Unit Occupancy Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load UnitOccupancy
                if (pObjUnitOccupancy.UnitOccupancyID != 0)
                {
                    mObjInsertUpdateUnitOccupancy = (from uoccup in _db.Unit_Occupancy
                                                 where uoccup.UnitOccupancyID == pObjUnitOccupancy.UnitOccupancyID
                                                 select uoccup).FirstOrDefault();

                    if (mObjInsertUpdateUnitOccupancy != null)
                    {
                        mObjInsertUpdateUnitOccupancy.Active = !mObjInsertUpdateUnitOccupancy.Active;
                        mObjInsertUpdateUnitOccupancy.ModifiedBy = pObjUnitOccupancy.ModifiedBy;
                        mObjInsertUpdateUnitOccupancy.ModifiedDate = pObjUnitOccupancy.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Unit Occupancy Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetUnitOccupancyList(pObjUnitOccupancy.UnitOccupancyName, 0, pObjUnitOccupancy.UnitOccupancyIds, pObjUnitOccupancy.intStatus, pObjUnitOccupancy.IncludeUnitOccupancyIds, pObjUnitOccupancy.ExcludeUnitOccupancyIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Unit Occupancy Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
