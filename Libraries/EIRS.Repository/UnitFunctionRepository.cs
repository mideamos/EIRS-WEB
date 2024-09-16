using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class UnitFunctionRepository : IUnitFunctionRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateUnitFunction(Unit_Function pObjUnitFunction)
        {
            using (_db = new EIRSEntities())
            {
                Unit_Function mObjInsertUpdateUnitFunction; //Unit Function Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from ufunc in _db.Unit_Function
                                       where ufunc.UnitFunctionName == pObjUnitFunction.UnitFunctionName && ufunc.UnitPurposeID == pObjUnitFunction.UnitPurposeID && ufunc.UnitFunctionID != pObjUnitFunction.UnitFunctionID
                                       select ufunc);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Unit Function already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Unit Function
                if (pObjUnitFunction.UnitFunctionID != 0)
                {
                    mObjInsertUpdateUnitFunction = (from UnitFunction in _db.Unit_Function
                                                        where UnitFunction.UnitFunctionID == pObjUnitFunction.UnitFunctionID
                                                        select UnitFunction).FirstOrDefault();

                    if (mObjInsertUpdateUnitFunction != null)
                    {
                        mObjInsertUpdateUnitFunction.ModifiedBy = pObjUnitFunction.ModifiedBy;
                        mObjInsertUpdateUnitFunction.ModifiedDate = pObjUnitFunction.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateUnitFunction = new Unit_Function();
                        mObjInsertUpdateUnitFunction.CreatedBy = pObjUnitFunction.CreatedBy;
                        mObjInsertUpdateUnitFunction.CreatedDate = pObjUnitFunction.CreatedDate;
                    }
                }
                else // Else Insert Unit Function
                {
                    mObjInsertUpdateUnitFunction = new Unit_Function();
                    mObjInsertUpdateUnitFunction.CreatedBy = pObjUnitFunction.CreatedBy;
                    mObjInsertUpdateUnitFunction.CreatedDate = pObjUnitFunction.CreatedDate;
                }

                mObjInsertUpdateUnitFunction.UnitFunctionName = pObjUnitFunction.UnitFunctionName;
                mObjInsertUpdateUnitFunction.UnitPurposeID = pObjUnitFunction.UnitPurposeID;
                mObjInsertUpdateUnitFunction.Active = pObjUnitFunction.Active;

                if (pObjUnitFunction.UnitFunctionID == 0)
                {
                    _db.Unit_Function.Add(mObjInsertUpdateUnitFunction);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjUnitFunction.UnitFunctionID == 0)
                        mObjFuncResponse.Message = "Unit Function Added Successfully";
                    else
                        mObjFuncResponse.Message = "Unit Function Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjUnitFunction.UnitFunctionID == 0)
                        mObjFuncResponse.Message = "Unit Function Addition Failed";
                    else
                        mObjFuncResponse.Message = "Unit Function Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetUnitFunctionList_Result> REP_GetUnitFunctionList(Unit_Function pObjUnitFunction)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetUnitFunctionList(pObjUnitFunction.UnitFunctionName, pObjUnitFunction.UnitFunctionID, pObjUnitFunction.UnitPurposeID, pObjUnitFunction.UnitFunctionIds, pObjUnitFunction.intStatus, pObjUnitFunction.IncludeUnitFunctionIds, pObjUnitFunction.ExcludeUnitFunctionIds).ToList();
            }
        }

        public usp_GetUnitFunctionList_Result REP_GetUnitFunctionDetails(Unit_Function pObjUnitFunction)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetUnitFunctionList(pObjUnitFunction.UnitFunctionName, pObjUnitFunction.UnitFunctionID, pObjUnitFunction.UnitPurposeID, pObjUnitFunction.UnitFunctionIds, pObjUnitFunction.intStatus, pObjUnitFunction.IncludeUnitFunctionIds, pObjUnitFunction.ExcludeUnitFunctionIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetUnitFunctionDropDownList(Unit_Function pObjUnitFunction)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from ufunc in _db.usp_GetUnitFunctionList(pObjUnitFunction.UnitFunctionName, pObjUnitFunction.UnitFunctionID, pObjUnitFunction.UnitPurposeID, pObjUnitFunction.UnitFunctionIds, pObjUnitFunction.intStatus, pObjUnitFunction.IncludeUnitFunctionIds, pObjUnitFunction.ExcludeUnitFunctionIds)
                               select new DropDownListResult()
                               {
                                   id = ufunc.UnitFunctionID.GetValueOrDefault(),
                                   text = ufunc.UnitFunctionName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Unit_Function pObjUnitFunction)
        {
            using (_db = new EIRSEntities())
            {
                Unit_Function mObjInsertUpdateUnitFunction; //Unit Function Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load UnitFunction
                if (pObjUnitFunction.UnitFunctionID != 0)
                {
                    mObjInsertUpdateUnitFunction = (from ufunc in _db.Unit_Function
                                                        where ufunc.UnitFunctionID == pObjUnitFunction.UnitFunctionID
                                                        select ufunc).FirstOrDefault();

                    if (mObjInsertUpdateUnitFunction != null)
                    {
                        mObjInsertUpdateUnitFunction.Active = !mObjInsertUpdateUnitFunction.Active;
                        mObjInsertUpdateUnitFunction.ModifiedBy = pObjUnitFunction.ModifiedBy;
                        mObjInsertUpdateUnitFunction.ModifiedDate = pObjUnitFunction.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Unit Function Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetUnitFunctionList(pObjUnitFunction.UnitFunctionName, 0, pObjUnitFunction.UnitPurposeID, pObjUnitFunction.UnitFunctionIds, pObjUnitFunction.intStatus, pObjUnitFunction.IncludeUnitFunctionIds, pObjUnitFunction.ExcludeUnitFunctionIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Unit Function Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
