using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class LandStreetConditionRepository : ILandStreetConditionRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateLandStreetCondition(Land_StreetCondition pObjLandStreetCondition)
        {
            using (_db = new EIRSEntities())
            {
                Land_StreetCondition mObjInsertUpdateLandStreetCondition; //Land Street Condition Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from scond in _db.Land_StreetCondition
                                       where scond.LandStreetConditionName == pObjLandStreetCondition.LandStreetConditionName && scond.LandStreetConditionID != pObjLandStreetCondition.LandStreetConditionID
                                       select scond);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Land StreetCondition already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Land StreetCondition
                if (pObjLandStreetCondition.LandStreetConditionID != 0)
                {
                    mObjInsertUpdateLandStreetCondition = (from LandStreetCondition in _db.Land_StreetCondition
                                                           where LandStreetCondition.LandStreetConditionID == pObjLandStreetCondition.LandStreetConditionID
                                                           select LandStreetCondition).FirstOrDefault();

                    if (mObjInsertUpdateLandStreetCondition != null)
                    {
                        mObjInsertUpdateLandStreetCondition.ModifiedBy = pObjLandStreetCondition.ModifiedBy;
                        mObjInsertUpdateLandStreetCondition.ModifiedDate = pObjLandStreetCondition.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateLandStreetCondition = new Land_StreetCondition();
                        mObjInsertUpdateLandStreetCondition.CreatedBy = pObjLandStreetCondition.CreatedBy;
                        mObjInsertUpdateLandStreetCondition.CreatedDate = pObjLandStreetCondition.CreatedDate;
                    }
                }
                else // Else Insert Land StreetCondition
                {
                    mObjInsertUpdateLandStreetCondition = new Land_StreetCondition();
                    mObjInsertUpdateLandStreetCondition.CreatedBy = pObjLandStreetCondition.CreatedBy;
                    mObjInsertUpdateLandStreetCondition.CreatedDate = pObjLandStreetCondition.CreatedDate;
                }

                mObjInsertUpdateLandStreetCondition.LandStreetConditionName = pObjLandStreetCondition.LandStreetConditionName;
                mObjInsertUpdateLandStreetCondition.Active = pObjLandStreetCondition.Active;

                if (pObjLandStreetCondition.LandStreetConditionID == 0)
                {
                    _db.Land_StreetCondition.Add(mObjInsertUpdateLandStreetCondition);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjLandStreetCondition.LandStreetConditionID == 0)
                        mObjFuncResponse.Message = "Land Street Condition Added Successfully";
                    else
                        mObjFuncResponse.Message = "Land Street Condition Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjLandStreetCondition.LandStreetConditionID == 0)
                        mObjFuncResponse.Message = "Land Street Condition Addition Failed";
                    else
                        mObjFuncResponse.Message = "Land Street Condition Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetLandStreetConditionList_Result> REP_GetLandStreetConditionList(Land_StreetCondition pObjLandStreetCondition)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLandStreetConditionList(pObjLandStreetCondition.LandStreetConditionName, pObjLandStreetCondition.LandStreetConditionID, pObjLandStreetCondition.LandStreetConditionIds, pObjLandStreetCondition.intStatus, pObjLandStreetCondition.IncludeLandStreetConditionIds, pObjLandStreetCondition.ExcludeLandStreetConditionIds).ToList();
            }
        }

        public usp_GetLandStreetConditionList_Result REP_GetLandStreetConditionDetails(Land_StreetCondition pObjLandStreetCondition)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLandStreetConditionList(pObjLandStreetCondition.LandStreetConditionName, pObjLandStreetCondition.LandStreetConditionID, pObjLandStreetCondition.LandStreetConditionIds, pObjLandStreetCondition.intStatus, pObjLandStreetCondition.IncludeLandStreetConditionIds, pObjLandStreetCondition.ExcludeLandStreetConditionIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetLandStreetConditionDropDownList(Land_StreetCondition pObjLandStreetCondition)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from scond in _db.usp_GetLandStreetConditionList(pObjLandStreetCondition.LandStreetConditionName, pObjLandStreetCondition.LandStreetConditionID, pObjLandStreetCondition.LandStreetConditionIds, pObjLandStreetCondition.intStatus, pObjLandStreetCondition.IncludeLandStreetConditionIds, pObjLandStreetCondition.ExcludeLandStreetConditionIds)
                               select new DropDownListResult()
                               {
                                   id = scond.LandStreetConditionID.GetValueOrDefault(),
                                   text = scond.LandStreetConditionName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Land_StreetCondition pObjLandStreetCondition)
        {
            using (_db = new EIRSEntities())
            {
                Land_StreetCondition mObjInsertUpdateLandStreetCondition; //Land StreetCondition Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load LandStreetCondition
                if (pObjLandStreetCondition.LandStreetConditionID != 0)
                {
                    mObjInsertUpdateLandStreetCondition = (from scond in _db.Land_StreetCondition
                                                           where scond.LandStreetConditionID == pObjLandStreetCondition.LandStreetConditionID
                                                           select scond).FirstOrDefault();

                    if (mObjInsertUpdateLandStreetCondition != null)
                    {
                        mObjInsertUpdateLandStreetCondition.Active = !mObjInsertUpdateLandStreetCondition.Active;
                        mObjInsertUpdateLandStreetCondition.ModifiedBy = pObjLandStreetCondition.ModifiedBy;
                        mObjInsertUpdateLandStreetCondition.ModifiedDate = pObjLandStreetCondition.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Land Street Condition Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetLandStreetConditionList(pObjLandStreetCondition.LandStreetConditionName, 0, pObjLandStreetCondition.LandStreetConditionIds, pObjLandStreetCondition.intStatus, pObjLandStreetCondition.IncludeLandStreetConditionIds, pObjLandStreetCondition.ExcludeLandStreetConditionIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Land Street Condition Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
