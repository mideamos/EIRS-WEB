using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class EconomicActivitiesRepository : IEconomicActivitiesRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateEconomicActivities(Economic_Activities pObjEconomicActivities)
        {
            using (_db = new EIRSEntities())
            {
                Economic_Activities mObjInsertUpdateEconomicActivities; //Economic Activities Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from eact in _db.Economic_Activities
                                       where eact.EconomicActivitiesName == pObjEconomicActivities.EconomicActivitiesName && eact.EconomicActivitiesID != pObjEconomicActivities.EconomicActivitiesID
                                       select eact);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Economic Activities already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Economic Activities
                if (pObjEconomicActivities.EconomicActivitiesID != 0)
                {
                    mObjInsertUpdateEconomicActivities = (from eact in _db.Economic_Activities
                                                 where eact.EconomicActivitiesID == pObjEconomicActivities.EconomicActivitiesID
                                                 select eact).FirstOrDefault();

                    if (mObjInsertUpdateEconomicActivities != null)
                    {
                        mObjInsertUpdateEconomicActivities.ModifiedBy = pObjEconomicActivities.ModifiedBy;
                        mObjInsertUpdateEconomicActivities.ModifiedDate = pObjEconomicActivities.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateEconomicActivities = new Economic_Activities();
                        mObjInsertUpdateEconomicActivities.CreatedBy = pObjEconomicActivities.CreatedBy;
                        mObjInsertUpdateEconomicActivities.CreatedDate = pObjEconomicActivities.CreatedDate;
                    }
                }
                else // Else Insert Economic Activities
                {
                    mObjInsertUpdateEconomicActivities = new Economic_Activities();
                    mObjInsertUpdateEconomicActivities.CreatedBy = pObjEconomicActivities.CreatedBy;
                    mObjInsertUpdateEconomicActivities.CreatedDate = pObjEconomicActivities.CreatedDate;
                }

                mObjInsertUpdateEconomicActivities.EconomicActivitiesName = pObjEconomicActivities.EconomicActivitiesName;
                mObjInsertUpdateEconomicActivities.TaxPayerTypeID = pObjEconomicActivities.TaxPayerTypeID;
                mObjInsertUpdateEconomicActivities.Active = pObjEconomicActivities.Active;

                if (pObjEconomicActivities.EconomicActivitiesID == 0)
                {
                    _db.Economic_Activities.Add(mObjInsertUpdateEconomicActivities);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjEconomicActivities.EconomicActivitiesID == 0)
                        mObjFuncResponse.Message = "Economic Activities Added Successfully";
                    else
                        mObjFuncResponse.Message = "Economic Activities Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjEconomicActivities.EconomicActivitiesID == 0)
                        mObjFuncResponse.Message = "Economic Activities Addition Failed";
                    else
                        mObjFuncResponse.Message = "Economic Activities Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetEconomicActivitiesList_Result> REP_GetEconomicActivitiesList(Economic_Activities pObjEconomicActivities)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetEconomicActivitiesList(pObjEconomicActivities.EconomicActivitiesName, pObjEconomicActivities.EconomicActivitiesID, pObjEconomicActivities.TaxPayerTypeID, pObjEconomicActivities.EconomicActivitiesIds, pObjEconomicActivities.intStatus, pObjEconomicActivities.IncludeEconomicActivitiesIds, pObjEconomicActivities.ExcludeEconomicActivitiesIds).ToList();
            }
        }

        public usp_GetEconomicActivitiesList_Result REP_GetEconomicActivitiesDetails(Economic_Activities pObjEconomicActivities)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetEconomicActivitiesList(pObjEconomicActivities.EconomicActivitiesName, pObjEconomicActivities.EconomicActivitiesID, pObjEconomicActivities.TaxPayerTypeID, pObjEconomicActivities.EconomicActivitiesIds, pObjEconomicActivities.intStatus, pObjEconomicActivities.IncludeEconomicActivitiesIds, pObjEconomicActivities.ExcludeEconomicActivitiesIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetEconomicActivitiesDropDownList(Economic_Activities pObjEconomicActivities)
        {
            using (_db = new EIRSEntities())
            {

                var vResult = from pro in _db.Economic_Activities where (pro.TaxPayerTypeID == pObjEconomicActivities.TaxPayerTypeID)
                              select new DropDownListResult() { id = pro.EconomicActivitiesID, text = pro.EconomicActivitiesName };

                return vResult.ToList();
                //var vResult = (from eact in _db.usp_GetEconomicActivitiesList(pObjEconomicActivities.EconomicActivitiesName,  pObjEconomicActivities.EconomicActivitiesID, pObjEconomicActivities.TaxPayerTypeID, pObjEconomicActivities.EconomicActivitiesIds, pObjEconomicActivities.intStatus, pObjEconomicActivities.IncludeEconomicActivitiesIds, pObjEconomicActivities.ExcludeEconomicActivitiesIds)
                //               select new DropDownListResult()
                //               {
                //                   id = eact.EconomicActivitiesID.GetValueOrDefault(),
                //                   text = eact.EconomicActivitiesName
                //               }).ToList();

                //return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Economic_Activities pObjEconomicActivities)
        {
            using (_db = new EIRSEntities())
            {
                Economic_Activities mObjInsertUpdateEconomicActivities; //Economic Activities Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load EconomicActivities
                if (pObjEconomicActivities.EconomicActivitiesID != 0)
                {
                    mObjInsertUpdateEconomicActivities = (from eact in _db.Economic_Activities
                                                 where eact.EconomicActivitiesID == pObjEconomicActivities.EconomicActivitiesID
                                                 select eact).FirstOrDefault();

                    if (mObjInsertUpdateEconomicActivities != null)
                    {
                        mObjInsertUpdateEconomicActivities.Active = !mObjInsertUpdateEconomicActivities.Active;
                        mObjInsertUpdateEconomicActivities.ModifiedBy = pObjEconomicActivities.ModifiedBy;
                        mObjInsertUpdateEconomicActivities.ModifiedDate = pObjEconomicActivities.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Economic Activities Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetEconomicActivitiesList(pObjEconomicActivities.EconomicActivitiesName, 0, pObjEconomicActivities.TaxPayerTypeID, pObjEconomicActivities.EconomicActivitiesIds, pObjEconomicActivities.intStatus, pObjEconomicActivities.IncludeEconomicActivitiesIds, pObjEconomicActivities.ExcludeEconomicActivitiesIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Economic Activities Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
