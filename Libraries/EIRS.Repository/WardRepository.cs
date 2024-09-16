using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class WardRepository : IWardRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateWard(Ward pObjWard)
        {
            using (_db = new EIRSEntities())
            {
                Ward mObjInsertUpdateWard; //Ward Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from wrd in _db.Wards
                                       where wrd.WardName == pObjWard.WardName && wrd.LGAID == pObjWard.LGAID && wrd.WardID != pObjWard.WardID
                                       select wrd);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Ward already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Ward
                if (pObjWard.WardID != 0)
                {
                    mObjInsertUpdateWard = (from wrd in _db.Wards
                                                 where wrd.WardID == pObjWard.WardID
                                                 select wrd).FirstOrDefault();

                    if (mObjInsertUpdateWard != null)
                    {
                        mObjInsertUpdateWard.ModifiedBy = pObjWard.ModifiedBy;
                        mObjInsertUpdateWard.ModifiedDate = pObjWard.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateWard = new Ward();
                        mObjInsertUpdateWard.CreatedBy = pObjWard.CreatedBy;
                        mObjInsertUpdateWard.CreatedDate = pObjWard.CreatedDate;
                    }
                }
                else // Else Insert Ward
                {
                    mObjInsertUpdateWard = new Ward();
                    mObjInsertUpdateWard.CreatedBy = pObjWard.CreatedBy;
                    mObjInsertUpdateWard.CreatedDate = pObjWard.CreatedDate;
                }

                mObjInsertUpdateWard.WardName = pObjWard.WardName;
                mObjInsertUpdateWard.LGAID = pObjWard.LGAID;
                mObjInsertUpdateWard.Active = pObjWard.Active;

                if (pObjWard.WardID == 0)
                {
                    _db.Wards.Add(mObjInsertUpdateWard);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjWard.WardID == 0)
                        mObjFuncResponse.Message = "Ward Added Successfully";
                    else
                        mObjFuncResponse.Message = "Ward Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjWard.WardID == 0)
                        mObjFuncResponse.Message = "Ward Addition Failed";
                    else
                        mObjFuncResponse.Message = "Ward Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetWardList_Result> REP_GetWardList(Ward pObjWard)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetWardList(pObjWard.WardName, pObjWard.WardID, pObjWard.LGAID, pObjWard.WardIds, pObjWard.intStatus, pObjWard.IncludeWardIds, pObjWard.ExcludeWardIds).ToList();
            }
        }

        public usp_GetWardList_Result REP_GetWardDetails(Ward pObjWard)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetWardList(pObjWard.WardName, pObjWard.WardID, pObjWard.LGAID, pObjWard.WardIds, pObjWard.intStatus, pObjWard.IncludeWardIds, pObjWard.ExcludeWardIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetWardDropDownList(Ward pObjWard)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from Ward in _db.usp_GetWardList(pObjWard.WardName, pObjWard.WardID, pObjWard.LGAID, pObjWard.WardIds, pObjWard.intStatus, pObjWard.IncludeWardIds, pObjWard.ExcludeWardIds)
                               select new DropDownListResult()
                               {
                                   id = Ward.WardID.GetValueOrDefault(),
                                   text = Ward.WardName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Ward pObjWard)
        {
            using (_db = new EIRSEntities())
            {
                Ward mObjInsertUpdateWard; //Ward Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Ward
                if (pObjWard.WardID != 0)
                {
                    mObjInsertUpdateWard = (from wrd in _db.Wards
                                                 where wrd.WardID == pObjWard.WardID
                                                 select wrd).FirstOrDefault();

                    if (mObjInsertUpdateWard != null)
                    {
                        mObjInsertUpdateWard.Active = !mObjInsertUpdateWard.Active;
                        mObjInsertUpdateWard.ModifiedBy = pObjWard.ModifiedBy;
                        mObjInsertUpdateWard.ModifiedDate = pObjWard.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Ward Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetWardList(pObjWard.WardName, 0, pObjWard.LGAID, pObjWard.WardIds, pObjWard.intStatus, pObjWard.IncludeWardIds, pObjWard.ExcludeWardIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Ward Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
