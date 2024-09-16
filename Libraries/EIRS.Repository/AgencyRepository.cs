using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class AgencyRepository : IAgencyRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateAgency(Agency pObjAgency)
        {
            using (_db = new EIRSEntities())
            {
                Agency mObjInsertUpdateAgency; //Agency Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from agny in _db.Agencies
                                       where agny.AgencyName == pObjAgency.AgencyName && agny.AgencyTypeID == pObjAgency.AgencyTypeID && agny.AgencyID != pObjAgency.AgencyID
                                       select agny);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Agency already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Agency
                if (pObjAgency.AgencyID != 0)
                {
                    mObjInsertUpdateAgency = (from agny in _db.Agencies
                                                 where agny.AgencyID == pObjAgency.AgencyID
                                                 select agny).FirstOrDefault();

                    if (mObjInsertUpdateAgency != null)
                    {
                        mObjInsertUpdateAgency.ModifiedBy = pObjAgency.ModifiedBy;
                        mObjInsertUpdateAgency.ModifiedDate = pObjAgency.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateAgency = new Agency();
                        mObjInsertUpdateAgency.CreatedBy = pObjAgency.CreatedBy;
                        mObjInsertUpdateAgency.CreatedDate = pObjAgency.CreatedDate;
                    }
                }
                else // Else Insert Agency
                {
                    mObjInsertUpdateAgency = new Agency();
                    mObjInsertUpdateAgency.CreatedBy = pObjAgency.CreatedBy;
                    mObjInsertUpdateAgency.CreatedDate = pObjAgency.CreatedDate;
                }

                mObjInsertUpdateAgency.AgencyName = pObjAgency.AgencyName;
                mObjInsertUpdateAgency.AgencyTypeID = pObjAgency.AgencyTypeID;
                mObjInsertUpdateAgency.Active = pObjAgency.Active;

                if (pObjAgency.AgencyID == 0)
                {
                    _db.Agencies.Add(mObjInsertUpdateAgency);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjAgency.AgencyID == 0)
                        mObjFuncResponse.Message = "Agency Added Successfully";
                    else
                        mObjFuncResponse.Message = "Agency Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjAgency.AgencyID == 0)
                        mObjFuncResponse.Message = "Agency Addition Failed";
                    else
                        mObjFuncResponse.Message = "Agency Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetAgencyList_Result> REP_GetAgencyList(Agency pObjAgency)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAgencyList(pObjAgency.AgencyName, pObjAgency.AgencyID, pObjAgency.AgencyTypeID, pObjAgency.AgencyIds, pObjAgency.intStatus, pObjAgency.IncludeAgencyIds, pObjAgency.ExcludeAgencyIds).ToList();
            }
        }

        public usp_GetAgencyList_Result REP_GetAgencyDetails(Agency pObjAgency)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAgencyList(pObjAgency.AgencyName, pObjAgency.AgencyID, pObjAgency.AgencyTypeID, pObjAgency.AgencyIds, pObjAgency.intStatus, pObjAgency.IncludeAgencyIds, pObjAgency.ExcludeAgencyIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetAgencyDropDownList(Agency pObjAgency)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from agny in _db.usp_GetAgencyList(pObjAgency.AgencyName, pObjAgency.AgencyID, pObjAgency.AgencyTypeID, pObjAgency.AgencyIds, pObjAgency.intStatus, pObjAgency.IncludeAgencyIds, pObjAgency.ExcludeAgencyIds)
                               select new DropDownListResult()
                               {
                                   id = agny.AgencyID.GetValueOrDefault(),
                                   text = agny.AgencyName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Agency pObjAgency)
        {
            using (_db = new EIRSEntities())
            {
                Agency mObjInsertUpdateAgency; //Agency Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Agency
                if (pObjAgency.AgencyID != 0)
                {
                    mObjInsertUpdateAgency = (from agny in _db.Agencies
                                                 where agny.AgencyID == pObjAgency.AgencyID
                                                 select agny).FirstOrDefault();

                    if (mObjInsertUpdateAgency != null)
                    {
                        mObjInsertUpdateAgency.Active = !mObjInsertUpdateAgency.Active;
                        mObjInsertUpdateAgency.ModifiedBy = pObjAgency.ModifiedBy;
                        mObjInsertUpdateAgency.ModifiedDate = pObjAgency.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Agency Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetAgencyList(pObjAgency.AgencyName, 0, pObjAgency.AgencyTypeID, pObjAgency.AgencyIds, pObjAgency.intStatus, pObjAgency.IncludeAgencyIds, pObjAgency.ExcludeAgencyIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Agency Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
