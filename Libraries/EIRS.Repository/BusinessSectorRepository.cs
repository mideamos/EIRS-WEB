using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class BusinessSectorRepository : IBusinessSectorRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateBusinessSector(Business_Sector pObjBusinessSector)
        {
            using (_db = new EIRSEntities())
            {
                Business_Sector mObjInsertUpdateBusinessSector; //Business Sector Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from bsect in _db.Business_Sector
                                       where bsect.BusinessSectorName == pObjBusinessSector.BusinessSectorName && bsect.BusinessTypeID == bsect.BusinessTypeID && bsect.BusinessCategoryID == pObjBusinessSector.BusinessCategoryID && bsect.BusinessSectorID != pObjBusinessSector.BusinessSectorID
                                       select bsect);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Business Sector already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Business Sector
                if (pObjBusinessSector.BusinessSectorID != 0)
                {
                    mObjInsertUpdateBusinessSector = (from bsect in _db.Business_Sector
                                                 where bsect.BusinessSectorID == pObjBusinessSector.BusinessSectorID
                                                 select bsect).FirstOrDefault();

                    if (mObjInsertUpdateBusinessSector != null)
                    {
                        mObjInsertUpdateBusinessSector.ModifiedBy = pObjBusinessSector.ModifiedBy;
                        mObjInsertUpdateBusinessSector.ModifiedDate = pObjBusinessSector.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateBusinessSector = new Business_Sector();
                        mObjInsertUpdateBusinessSector.CreatedBy = pObjBusinessSector.CreatedBy;
                        mObjInsertUpdateBusinessSector.CreatedDate = pObjBusinessSector.CreatedDate;
                    }
                }
                else // Else Insert Business Sector
                {
                    mObjInsertUpdateBusinessSector = new Business_Sector();
                    mObjInsertUpdateBusinessSector.CreatedBy = pObjBusinessSector.CreatedBy;
                    mObjInsertUpdateBusinessSector.CreatedDate = pObjBusinessSector.CreatedDate;
                }

                mObjInsertUpdateBusinessSector.BusinessSectorName = pObjBusinessSector.BusinessSectorName;
                mObjInsertUpdateBusinessSector.BusinessTypeID = pObjBusinessSector.BusinessTypeID;
                mObjInsertUpdateBusinessSector.BusinessCategoryID = pObjBusinessSector.BusinessCategoryID;
                mObjInsertUpdateBusinessSector.Active = pObjBusinessSector.Active;

                if (pObjBusinessSector.BusinessSectorID == 0)
                {
                    _db.Business_Sector.Add(mObjInsertUpdateBusinessSector);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjBusinessSector.BusinessSectorID == 0)
                        mObjFuncResponse.Message = "Business Sector Added Successfully";
                    else
                        mObjFuncResponse.Message = "Business Sector Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjBusinessSector.BusinessSectorID == 0)
                        mObjFuncResponse.Message = "Business Sector Addition Failed";
                    else
                        mObjFuncResponse.Message = "Business Sector Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetBusinessSectorList_Result> REP_GetBusinessSectorList(Business_Sector pObjBusinessSector)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessSectorList(pObjBusinessSector.BusinessSectorName, pObjBusinessSector.BusinessSectorID, pObjBusinessSector.BusinessTypeID, pObjBusinessSector.BusinessCategoryID, pObjBusinessSector.BusinessSectorIds, pObjBusinessSector.intStatus, pObjBusinessSector.IncludeBusinessSectorIds, pObjBusinessSector.ExcludeBusinessSectorIds).ToList();
            }
        }

        public usp_GetBusinessSectorList_Result REP_GetBusinessSectorDetails(Business_Sector pObjBusinessSector)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessSectorList(pObjBusinessSector.BusinessSectorName, pObjBusinessSector.BusinessSectorID, pObjBusinessSector.BusinessTypeID, pObjBusinessSector.BusinessCategoryID, pObjBusinessSector.BusinessSectorIds, pObjBusinessSector.intStatus, pObjBusinessSector.IncludeBusinessSectorIds, pObjBusinessSector.ExcludeBusinessSectorIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetBusinessSectorDropDownList(Business_Sector pObjBusinessSector)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from bsect in _db.usp_GetBusinessSectorList(pObjBusinessSector.BusinessSectorName, pObjBusinessSector.BusinessSectorID, pObjBusinessSector.BusinessTypeID, pObjBusinessSector.BusinessCategoryID, pObjBusinessSector.BusinessSectorIds, pObjBusinessSector.intStatus, pObjBusinessSector.IncludeBusinessSectorIds, pObjBusinessSector.ExcludeBusinessSectorIds)
                               select new DropDownListResult()
                               {
                                   id = bsect.BusinessSectorID.GetValueOrDefault(),
                                   text = bsect.BusinessSectorName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Business_Sector pObjBusinessSector)
        {
            using (_db = new EIRSEntities())
            {
                Business_Sector mObjInsertUpdateBusinessSector; //Business Sector Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load BusinessSector
                if (pObjBusinessSector.BusinessSectorID != 0)
                {
                    mObjInsertUpdateBusinessSector = (from bsect in _db.Business_Sector
                                                 where bsect.BusinessSectorID == pObjBusinessSector.BusinessSectorID
                                                 select bsect).FirstOrDefault();

                    if (mObjInsertUpdateBusinessSector != null)
                    {
                        mObjInsertUpdateBusinessSector.Active = !mObjInsertUpdateBusinessSector.Active;
                        mObjInsertUpdateBusinessSector.ModifiedBy = pObjBusinessSector.ModifiedBy;
                        mObjInsertUpdateBusinessSector.ModifiedDate = pObjBusinessSector.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Business Sector Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetBusinessSectorList(pObjBusinessSector.BusinessSectorName, 0,pObjBusinessSector.BusinessTypeID, pObjBusinessSector.BusinessCategoryID, pObjBusinessSector.BusinessSectorIds, pObjBusinessSector.intStatus, pObjBusinessSector.IncludeBusinessSectorIds, pObjBusinessSector.ExcludeBusinessSectorIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Business Sector Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
