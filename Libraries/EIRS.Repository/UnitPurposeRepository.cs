using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class UnitPurposeRepository : IUnitPurposeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateUnitPurpose(Unit_Purpose pObjUnitPurpose)
        {
            using (_db = new EIRSEntities())
            {
                Unit_Purpose mObjInsertUpdateUnitPurpose; //Unit Purpose Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from upurp in _db.Unit_Purpose
                                       where upurp.UnitPurposeName == pObjUnitPurpose.UnitPurposeName && upurp.UnitPurposeID != pObjUnitPurpose.UnitPurposeID
                                       select upurp);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Unit Purpose already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Unit Purpose
                if (pObjUnitPurpose.UnitPurposeID != 0)
                {
                    mObjInsertUpdateUnitPurpose = (from UnitPurpose in _db.Unit_Purpose
                                                 where UnitPurpose.UnitPurposeID == pObjUnitPurpose.UnitPurposeID
                                                 select UnitPurpose).FirstOrDefault();

                    if (mObjInsertUpdateUnitPurpose != null)
                    {
                        mObjInsertUpdateUnitPurpose.ModifiedBy = pObjUnitPurpose.ModifiedBy;
                        mObjInsertUpdateUnitPurpose.ModifiedDate = pObjUnitPurpose.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateUnitPurpose = new Unit_Purpose();
                        mObjInsertUpdateUnitPurpose.CreatedBy = pObjUnitPurpose.CreatedBy;
                        mObjInsertUpdateUnitPurpose.CreatedDate = pObjUnitPurpose.CreatedDate;
                    }
                }
                else // Else Insert Unit Purpose
                {
                    mObjInsertUpdateUnitPurpose = new Unit_Purpose();
                    mObjInsertUpdateUnitPurpose.CreatedBy = pObjUnitPurpose.CreatedBy;
                    mObjInsertUpdateUnitPurpose.CreatedDate = pObjUnitPurpose.CreatedDate;
                }

                mObjInsertUpdateUnitPurpose.UnitPurposeName = pObjUnitPurpose.UnitPurposeName;
                mObjInsertUpdateUnitPurpose.Active = pObjUnitPurpose.Active;

                if (pObjUnitPurpose.UnitPurposeID == 0)
                {
                    _db.Unit_Purpose.Add(mObjInsertUpdateUnitPurpose);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjUnitPurpose.UnitPurposeID == 0)
                        mObjFuncResponse.Message = "Unit Purpose Added Successfully";
                    else
                        mObjFuncResponse.Message = "Unit Purpose Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjUnitPurpose.UnitPurposeID == 0)
                        mObjFuncResponse.Message = "Unit Purpose Addition Failed";
                    else
                        mObjFuncResponse.Message = "Unit Purpose Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetUnitPurposeList_Result> REP_GetUnitPurposeList(Unit_Purpose pObjUnitPurpose)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetUnitPurposeList(pObjUnitPurpose.UnitPurposeName, pObjUnitPurpose.UnitPurposeID, pObjUnitPurpose.UnitPurposeIds, pObjUnitPurpose.intStatus, pObjUnitPurpose.IncludeUnitPurposeIds, pObjUnitPurpose.ExcludeUnitPurposeIds).ToList();
            }
        }

        public usp_GetUnitPurposeList_Result REP_GetUnitPurposeDetails(Unit_Purpose pObjUnitPurpose)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetUnitPurposeList(pObjUnitPurpose.UnitPurposeName, pObjUnitPurpose.UnitPurposeID, pObjUnitPurpose.UnitPurposeIds, pObjUnitPurpose.intStatus, pObjUnitPurpose.IncludeUnitPurposeIds, pObjUnitPurpose.ExcludeUnitPurposeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetUnitPurposeDropDownList(Unit_Purpose pObjUnitPurpose)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from upurp in _db.usp_GetUnitPurposeList(pObjUnitPurpose.UnitPurposeName, pObjUnitPurpose.UnitPurposeID, pObjUnitPurpose.UnitPurposeIds, pObjUnitPurpose.intStatus, pObjUnitPurpose.IncludeUnitPurposeIds, pObjUnitPurpose.ExcludeUnitPurposeIds)
                               select new DropDownListResult()
                               {
                                   id = upurp.UnitPurposeID.GetValueOrDefault(),
                                   text = upurp.UnitPurposeName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Unit_Purpose pObjUnitPurpose)
        {
            using (_db = new EIRSEntities())
            {
                Unit_Purpose mObjInsertUpdateUnitPurpose; //Unit Purpose Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load UnitPurpose
                if (pObjUnitPurpose.UnitPurposeID != 0)
                {
                    mObjInsertUpdateUnitPurpose = (from upurp in _db.Unit_Purpose
                                                 where upurp.UnitPurposeID == pObjUnitPurpose.UnitPurposeID
                                                 select upurp).FirstOrDefault();

                    if (mObjInsertUpdateUnitPurpose != null)
                    {
                        mObjInsertUpdateUnitPurpose.Active = !mObjInsertUpdateUnitPurpose.Active;
                        mObjInsertUpdateUnitPurpose.ModifiedBy = pObjUnitPurpose.ModifiedBy;
                        mObjInsertUpdateUnitPurpose.ModifiedDate = pObjUnitPurpose.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Unit Purpose Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetUnitPurposeList(pObjUnitPurpose.UnitPurposeName, 0, pObjUnitPurpose.UnitPurposeIds, pObjUnitPurpose.intStatus, pObjUnitPurpose.IncludeUnitPurposeIds, pObjUnitPurpose.ExcludeUnitPurposeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Unit Purpose Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
