using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class GovernmentTypeRepository : IGovernmentTypeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateGovernmentType(Government_Types pObjGovernmentType)
        {
            using (_db = new EIRSEntities())
            {
                Government_Types mObjInsertUpdateGovernmentType; //Government Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from GovernmentType in _db.Government_Types
                                       where GovernmentType.GovernmentTypeName == pObjGovernmentType.GovernmentTypeName && GovernmentType.GovernmentTypeID != pObjGovernmentType.GovernmentTypeID
                                       select GovernmentType);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Government Type already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Government Type
                if (pObjGovernmentType.GovernmentTypeID != 0)
                {
                    mObjInsertUpdateGovernmentType = (from GovernmentType in _db.Government_Types
                                                 where GovernmentType.GovernmentTypeID == pObjGovernmentType.GovernmentTypeID
                                                 select GovernmentType).FirstOrDefault();

                    if (mObjInsertUpdateGovernmentType != null)
                    {
                        mObjInsertUpdateGovernmentType.ModifiedBy = pObjGovernmentType.ModifiedBy;
                        mObjInsertUpdateGovernmentType.ModifiedDate = pObjGovernmentType.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateGovernmentType = new Government_Types();
                        mObjInsertUpdateGovernmentType.CreatedBy = pObjGovernmentType.CreatedBy;
                        mObjInsertUpdateGovernmentType.CreatedDate = pObjGovernmentType.CreatedDate;
                    }
                }
                else // Else Insert Government Type
                {
                    mObjInsertUpdateGovernmentType = new Government_Types();
                    mObjInsertUpdateGovernmentType.CreatedBy = pObjGovernmentType.CreatedBy;
                    mObjInsertUpdateGovernmentType.CreatedDate = pObjGovernmentType.CreatedDate;
                }

                mObjInsertUpdateGovernmentType.GovernmentTypeName = pObjGovernmentType.GovernmentTypeName;
                mObjInsertUpdateGovernmentType.Active = pObjGovernmentType.Active;

                if (pObjGovernmentType.GovernmentTypeID == 0)
                {
                    _db.Government_Types.Add(mObjInsertUpdateGovernmentType);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjGovernmentType.GovernmentTypeID == 0)
                        mObjFuncResponse.Message = "Government Type Added Successfully";
                    else
                        mObjFuncResponse.Message = "Government Type Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjGovernmentType.GovernmentTypeID == 0)
                        mObjFuncResponse.Message = "Government Type Addition Failed";
                    else
                        mObjFuncResponse.Message = "Government Type Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetGovernmentTypeList_Result> REP_GetGovernmentTypeList(Government_Types pObjGovernmentType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetGovernmentTypeList(pObjGovernmentType.GovernmentTypeName, pObjGovernmentType.GovernmentTypeID, pObjGovernmentType.GovernmentTypeIds, pObjGovernmentType.intStatus, pObjGovernmentType.IncludeGovernmentTypeIds, pObjGovernmentType.ExcludeGovernmentTypeIds).ToList();
            }
        }

        public usp_GetGovernmentTypeList_Result REP_GetGovernmentTypeDetails(Government_Types pObjGovernmentType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetGovernmentTypeList(pObjGovernmentType.GovernmentTypeName, pObjGovernmentType.GovernmentTypeID, pObjGovernmentType.GovernmentTypeIds, pObjGovernmentType.intStatus, pObjGovernmentType.IncludeGovernmentTypeIds, pObjGovernmentType.ExcludeGovernmentTypeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetGovernmentTypeDropDownList(Government_Types pObjGovernmentType)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = from pro in _db.Government_Types
                              select new DropDownListResult() { id = pro.GovernmentTypeID, text = pro.GovernmentTypeName };

                return vResult.ToList();
                //var vResult = (from GovernmentType in _db.usp_GetGovernmentTypeList(pObjGovernmentType.GovernmentTypeName, pObjGovernmentType.GovernmentTypeID, pObjGovernmentType.GovernmentTypeIds, pObjGovernmentType.intStatus, pObjGovernmentType.IncludeGovernmentTypeIds, pObjGovernmentType.ExcludeGovernmentTypeIds)
                //               select new DropDownListResult()
                //               {
                //                   id = GovernmentType.GovernmentTypeID.GetValueOrDefault(),
                //                   text = GovernmentType.GovernmentTypeName
                //               }).ToList();

                //return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Government_Types pObjGovernmentType)
        {
            using (_db = new EIRSEntities())
            {
                Government_Types mObjInsertUpdateGovernmentType; //Government Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load GovernmentType
                if (pObjGovernmentType.GovernmentTypeID != 0)
                {
                    mObjInsertUpdateGovernmentType = (from GovernmentType in _db.Government_Types
                                                 where GovernmentType.GovernmentTypeID == pObjGovernmentType.GovernmentTypeID
                                                 select GovernmentType).FirstOrDefault();

                    if (mObjInsertUpdateGovernmentType != null)
                    {
                        mObjInsertUpdateGovernmentType.Active = !mObjInsertUpdateGovernmentType.Active;
                        mObjInsertUpdateGovernmentType.ModifiedBy = pObjGovernmentType.ModifiedBy;
                        mObjInsertUpdateGovernmentType.ModifiedDate = pObjGovernmentType.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Government Type Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetGovernmentTypeList(pObjGovernmentType.GovernmentTypeName, 0, pObjGovernmentType.GovernmentTypeIds, pObjGovernmentType.intStatus, pObjGovernmentType.IncludeGovernmentTypeIds, pObjGovernmentType.ExcludeGovernmentTypeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Government Type Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

    }
}
