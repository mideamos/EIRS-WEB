using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class TownRepository : ITownRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateTown(Town pObjTown)
        {
            using (_db = new EIRSEntities())
            {
                Town mObjInsertUpdateTown; //Town Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from twn in _db.Towns
                                       where twn.TownName == pObjTown.TownName && twn.LGAID == pObjTown.LGAID && twn.TownID != pObjTown.TownID
                                       select twn);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Town already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Town
                if (pObjTown.TownID != 0)
                {
                    mObjInsertUpdateTown = (from twn in _db.Towns
                                                 where twn.TownID == pObjTown.TownID
                                                 select twn).FirstOrDefault();

                    if (mObjInsertUpdateTown != null)
                    {
                        mObjInsertUpdateTown.ModifiedBy = pObjTown.ModifiedBy;
                        mObjInsertUpdateTown.ModifiedDate = pObjTown.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateTown = new Town();
                        mObjInsertUpdateTown.CreatedBy = pObjTown.CreatedBy;
                        mObjInsertUpdateTown.CreatedDate = pObjTown.CreatedDate;
                    }
                }
                else // Else Insert Town
                {
                    mObjInsertUpdateTown = new Town();
                    mObjInsertUpdateTown.CreatedBy = pObjTown.CreatedBy;
                    mObjInsertUpdateTown.CreatedDate = pObjTown.CreatedDate;
                }

                mObjInsertUpdateTown.TownName = pObjTown.TownName;
                mObjInsertUpdateTown.LGAID = pObjTown.LGAID;
                mObjInsertUpdateTown.Active = pObjTown.Active;

                if (pObjTown.TownID == 0)
                {
                    _db.Towns.Add(mObjInsertUpdateTown);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjTown.TownID == 0)
                        mObjFuncResponse.Message = "Town Added Successfully";
                    else
                        mObjFuncResponse.Message = "Town Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjTown.TownID == 0)
                        mObjFuncResponse.Message = "Town Addition Failed";
                    else
                        mObjFuncResponse.Message = "Town Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetTownList_Result> REP_GetTownList(Town pObjTown)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTownList(pObjTown.TownName, pObjTown.TownID,pObjTown.LGAID, pObjTown.TownIds, pObjTown.intStatus, pObjTown.IncludeTownIds, pObjTown.ExcludeTownIds).ToList();
            }
        }

        public usp_GetTownList_Result REP_GetTownDetails(Town pObjTown)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTownList(pObjTown.TownName, pObjTown.TownID, pObjTown.LGAID, pObjTown.TownIds, pObjTown.intStatus, pObjTown.IncludeTownIds, pObjTown.ExcludeTownIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetTownDropDownList(Town pObjTown)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from Town in _db.usp_GetTownList(pObjTown.TownName, pObjTown.TownID, pObjTown.LGAID, pObjTown.TownIds, pObjTown.intStatus, pObjTown.IncludeTownIds, pObjTown.ExcludeTownIds)
                               select new DropDownListResult()
                               {
                                   id = Town.TownID.GetValueOrDefault(),
                                   text = Town.TownName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Town pObjTown)
        {
            using (_db = new EIRSEntities())
            {
                Town mObjInsertUpdateTown; //Town Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Town
                if (pObjTown.TownID != 0)
                {
                    mObjInsertUpdateTown = (from twn in _db.Towns
                                                 where twn.TownID == pObjTown.TownID
                                                 select twn).FirstOrDefault();

                    if (mObjInsertUpdateTown != null)
                    {
                        mObjInsertUpdateTown.Active = !mObjInsertUpdateTown.Active;
                        mObjInsertUpdateTown.ModifiedBy = pObjTown.ModifiedBy;
                        mObjInsertUpdateTown.ModifiedDate = pObjTown.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Town Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetTownList(pObjTown.TownName, 0, pObjTown.LGAID, pObjTown.TownIds, pObjTown.intStatus, pObjTown.IncludeTownIds, pObjTown.ExcludeTownIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Town Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
