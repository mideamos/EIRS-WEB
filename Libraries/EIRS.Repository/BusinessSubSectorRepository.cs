using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class BusinessSubSectorRepository : IBusinessSubSectorRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateBusinessSubSector(Business_SubSector pObjBusinessSubSector)
        {
            using (_db = new EIRSEntities())
            {
                Business_SubSector mObjInsertUpdateBusinessSubSector; //Business Sub Sector Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from bssect in _db.Business_SubSector
                                       where bssect.BusinessSubSectorName == pObjBusinessSubSector.BusinessSubSectorName && bssect.BusinessSectorID == pObjBusinessSubSector.BusinessSectorID && bssect.BusinessSubSectorID != pObjBusinessSubSector.BusinessSubSectorID
                                       select bssect);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Business Sub Sector already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Business Sub Sector
                if (pObjBusinessSubSector.BusinessSubSectorID != 0)
                {
                    mObjInsertUpdateBusinessSubSector = (from bssect in _db.Business_SubSector
                                                 where bssect.BusinessSubSectorID == pObjBusinessSubSector.BusinessSubSectorID
                                                 select bssect).FirstOrDefault();

                    if (mObjInsertUpdateBusinessSubSector != null)
                    {
                        mObjInsertUpdateBusinessSubSector.ModifiedBy = pObjBusinessSubSector.ModifiedBy;
                        mObjInsertUpdateBusinessSubSector.ModifiedDate = pObjBusinessSubSector.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateBusinessSubSector = new Business_SubSector();
                        mObjInsertUpdateBusinessSubSector.CreatedBy = pObjBusinessSubSector.CreatedBy;
                        mObjInsertUpdateBusinessSubSector.CreatedDate = pObjBusinessSubSector.CreatedDate;
                    }
                }
                else // Else Insert Business Sub Sector
                {
                    mObjInsertUpdateBusinessSubSector = new Business_SubSector();
                    mObjInsertUpdateBusinessSubSector.CreatedBy = pObjBusinessSubSector.CreatedBy;
                    mObjInsertUpdateBusinessSubSector.CreatedDate = pObjBusinessSubSector.CreatedDate;
                }

                mObjInsertUpdateBusinessSubSector.BusinessSubSectorName = pObjBusinessSubSector.BusinessSubSectorName;
                mObjInsertUpdateBusinessSubSector.BusinessSectorID = pObjBusinessSubSector.BusinessSectorID;
                mObjInsertUpdateBusinessSubSector.Active = pObjBusinessSubSector.Active;

                if (pObjBusinessSubSector.BusinessSubSectorID == 0)
                {
                    _db.Business_SubSector.Add(mObjInsertUpdateBusinessSubSector);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjBusinessSubSector.BusinessSubSectorID == 0)
                        mObjFuncResponse.Message = "Business Sub Sector Added Successfully";
                    else
                        mObjFuncResponse.Message = "Business Sub Sector Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjBusinessSubSector.BusinessSubSectorID == 0)
                        mObjFuncResponse.Message = "Business Sub Sector Addition Failed";
                    else
                        mObjFuncResponse.Message = "Business Sub Sector Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetBusinessSubSectorList_Result> REP_GetBusinessSubSectorList(Business_SubSector pObjBusinessSubSector)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessSubSectorList(pObjBusinessSubSector.BusinessSubSectorName, pObjBusinessSubSector.BusinessSubSectorID, pObjBusinessSubSector.BusinessTypeID, pObjBusinessSubSector.BusinessCategoryID, pObjBusinessSubSector.BusinessSectorID, pObjBusinessSubSector.BusinessSubSectorIds, pObjBusinessSubSector.intStatus, pObjBusinessSubSector.IncludeBusinessSubSectorIds, pObjBusinessSubSector.ExcludeBusinessSubSectorIds).ToList();
            }
        }

        public usp_GetBusinessSubSectorList_Result REP_GetBusinessSubSectorDetails(Business_SubSector pObjBusinessSubSector)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessSubSectorList(pObjBusinessSubSector.BusinessSubSectorName, pObjBusinessSubSector.BusinessSubSectorID, pObjBusinessSubSector.BusinessTypeID, pObjBusinessSubSector.BusinessCategoryID, pObjBusinessSubSector.BusinessSectorID, pObjBusinessSubSector.BusinessSubSectorIds, pObjBusinessSubSector.intStatus, pObjBusinessSubSector.IncludeBusinessSubSectorIds, pObjBusinessSubSector.ExcludeBusinessSubSectorIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetBusinessSubSectorDropDownList(Business_SubSector pObjBusinessSubSector)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from bssect in _db.usp_GetBusinessSubSectorList(pObjBusinessSubSector.BusinessSubSectorName, pObjBusinessSubSector.BusinessSubSectorID, pObjBusinessSubSector.BusinessTypeID, pObjBusinessSubSector.BusinessCategoryID, pObjBusinessSubSector.BusinessSectorID,pObjBusinessSubSector.BusinessSubSectorIds, pObjBusinessSubSector.intStatus, pObjBusinessSubSector.IncludeBusinessSubSectorIds, pObjBusinessSubSector.ExcludeBusinessSubSectorIds)
                               select new DropDownListResult()
                               {
                                   id = bssect.BusinessSubSectorID.GetValueOrDefault(),
                                   text = bssect.BusinessSubSectorName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Business_SubSector pObjBusinessSubSector)
        {
            using (_db = new EIRSEntities())
            {
                Business_SubSector mObjInsertUpdateBusinessSubSector; //Business Sub Sector Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load BusinessSubSector
                if (pObjBusinessSubSector.BusinessSubSectorID != 0)
                {
                    mObjInsertUpdateBusinessSubSector = (from bssect in _db.Business_SubSector
                                                 where bssect.BusinessSubSectorID == pObjBusinessSubSector.BusinessSubSectorID
                                                 select bssect).FirstOrDefault();

                    if (mObjInsertUpdateBusinessSubSector != null)
                    {
                        mObjInsertUpdateBusinessSubSector.Active = !mObjInsertUpdateBusinessSubSector.Active;
                        mObjInsertUpdateBusinessSubSector.ModifiedBy = pObjBusinessSubSector.ModifiedBy;
                        mObjInsertUpdateBusinessSubSector.ModifiedDate = pObjBusinessSubSector.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Business Sub Sector Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetBusinessSubSectorList(pObjBusinessSubSector.BusinessSubSectorName, 0,pObjBusinessSubSector.BusinessTypeID, pObjBusinessSubSector.BusinessCategoryID, pObjBusinessSubSector.BusinessSectorID, pObjBusinessSubSector.BusinessSubSectorIds, pObjBusinessSubSector.intStatus, pObjBusinessSubSector.IncludeBusinessSubSectorIds, pObjBusinessSubSector.ExcludeBusinessSubSectorIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Business Sub Sector Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
