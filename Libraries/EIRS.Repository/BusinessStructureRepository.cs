using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class BusinessStructureRepository : IBusinessStructureRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateBusinessStructure(Business_Structure pObjBusinessStructure)
        {
            using (_db = new EIRSEntities())
            {
                Business_Structure mObjInsertUpdateBusinessStructure; //Business Structure Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from bstruct in _db.Business_Structure
                                       where bstruct.BusinessStructureName == pObjBusinessStructure.BusinessStructureName && bstruct.BusinessTypeID == pObjBusinessStructure.BusinessStructureID 
                                       && bstruct.BusinessStructureID != pObjBusinessStructure.BusinessStructureID
                                       select bstruct);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Business Structure already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Business Structure
                if (pObjBusinessStructure.BusinessStructureID != 0)
                {
                    mObjInsertUpdateBusinessStructure = (from bstruct in _db.Business_Structure
                                                 where bstruct.BusinessStructureID == pObjBusinessStructure.BusinessStructureID
                                                 select bstruct).FirstOrDefault();

                    if (mObjInsertUpdateBusinessStructure != null)
                    {
                        mObjInsertUpdateBusinessStructure.ModifiedBy = pObjBusinessStructure.ModifiedBy;
                        mObjInsertUpdateBusinessStructure.ModifiedDate = pObjBusinessStructure.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateBusinessStructure = new Business_Structure();
                        mObjInsertUpdateBusinessStructure.CreatedBy = pObjBusinessStructure.CreatedBy;
                        mObjInsertUpdateBusinessStructure.CreatedDate = pObjBusinessStructure.CreatedDate;
                    }
                }
                else // Else Insert Business Structure
                {
                    mObjInsertUpdateBusinessStructure = new Business_Structure();
                    mObjInsertUpdateBusinessStructure.CreatedBy = pObjBusinessStructure.CreatedBy;
                    mObjInsertUpdateBusinessStructure.CreatedDate = pObjBusinessStructure.CreatedDate;
                }

                mObjInsertUpdateBusinessStructure.BusinessStructureName = pObjBusinessStructure.BusinessStructureName;
                mObjInsertUpdateBusinessStructure.BusinessTypeID = pObjBusinessStructure.BusinessTypeID;
                mObjInsertUpdateBusinessStructure.Active = pObjBusinessStructure.Active;

                if (pObjBusinessStructure.BusinessStructureID == 0)
                {
                    _db.Business_Structure.Add(mObjInsertUpdateBusinessStructure);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjBusinessStructure.BusinessStructureID == 0)
                        mObjFuncResponse.Message = "Business Structure Added Successfully";
                    else
                        mObjFuncResponse.Message = "Business Structure Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjBusinessStructure.BusinessStructureID == 0)
                        mObjFuncResponse.Message = "Business Structure Addition Failed";
                    else
                        mObjFuncResponse.Message = "Business Structure Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetBusinessStructureList_Result> REP_GetBusinessStructureList(Business_Structure pObjBusinessStructure)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessStructureList(pObjBusinessStructure.BusinessStructureName, pObjBusinessStructure.BusinessStructureID, pObjBusinessStructure.BusinessTypeID, pObjBusinessStructure.BusinessStructureIds, pObjBusinessStructure.intStatus, pObjBusinessStructure.IncludeBusinessStructureIds, pObjBusinessStructure.ExcludeBusinessStructureIds).ToList();
            }
        }

        public usp_GetBusinessStructureList_Result REP_GetBusinessStructureDetails(Business_Structure pObjBusinessStructure)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessStructureList(pObjBusinessStructure.BusinessStructureName, pObjBusinessStructure.BusinessStructureID, pObjBusinessStructure.BusinessTypeID, pObjBusinessStructure.BusinessStructureIds, pObjBusinessStructure.intStatus, pObjBusinessStructure.IncludeBusinessStructureIds, pObjBusinessStructure.ExcludeBusinessStructureIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetBusinessStructureDropDownList(Business_Structure pObjBusinessStructure)
        {
            using (_db = new EIRSEntities())
            {
                //var vResult = (from bstruct in _db.usp_GetBusinessStructureList(pObjBusinessStructure.BusinessStructureName, pObjBusinessStructure.BusinessStructureID, pObjBusinessStructure.BusinessTypeID, pObjBusinessStructure.BusinessStructureIds, pObjBusinessStructure.intStatus, pObjBusinessStructure.IncludeBusinessStructureIds, pObjBusinessStructure.ExcludeBusinessStructureIds)
                //               select new DropDownListResult()
                //               {
                //                   id = bstruct.BusinessStructureID.GetValueOrDefault(),
                //                   text = bstruct.BusinessStructureName
                //               }).ToList();

                //return vResult;


                var vResult = from pro in _db.Business_Structure
                              where pro.BusinessTypeID == pObjBusinessStructure.BusinessTypeID
                              select new DropDownListResult() { id = pro.BusinessStructureID, text = pro.BusinessStructureName };

                return vResult.ToList();
            }
        }

        public FuncResponse REP_UpdateStatus(Business_Structure pObjBusinessStructure)
        {
            using (_db = new EIRSEntities())
            {
                Business_Structure mObjInsertUpdateBusinessStructure; //Business Structure Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load BusinessStructure
                if (pObjBusinessStructure.BusinessStructureID != 0)
                {
                    mObjInsertUpdateBusinessStructure = (from bstruct in _db.Business_Structure
                                                 where bstruct.BusinessStructureID == pObjBusinessStructure.BusinessStructureID
                                                 select bstruct).FirstOrDefault();

                    if (mObjInsertUpdateBusinessStructure != null)
                    {
                        mObjInsertUpdateBusinessStructure.Active = !mObjInsertUpdateBusinessStructure.Active;
                        mObjInsertUpdateBusinessStructure.ModifiedBy = pObjBusinessStructure.ModifiedBy;
                        mObjInsertUpdateBusinessStructure.ModifiedDate = pObjBusinessStructure.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Business Structure Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetBusinessStructureList(pObjBusinessStructure.BusinessStructureName, 0,pObjBusinessStructure.BusinessTypeID, pObjBusinessStructure.BusinessStructureIds, pObjBusinessStructure.intStatus, pObjBusinessStructure.IncludeBusinessStructureIds, pObjBusinessStructure.ExcludeBusinessStructureIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Business Structure Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
