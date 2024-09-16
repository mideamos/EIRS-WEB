using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    
    public class AssessmentGroupRepository : IAssessmentGroupRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateAssessmentGroup(Assessment_Group pObjAssessmentGroup)
        {
            using (_db = new EIRSEntities())
            {
                Assessment_Group mObjInsertUpdateAssessmentGroup; //Assessment Group Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from agrp in _db.Assessment_Group
                                       where agrp.AssessmentGroupName == pObjAssessmentGroup.AssessmentGroupName && agrp.AssetTypeID == pObjAssessmentGroup.AssetTypeID && agrp.AssessmentGroupID != pObjAssessmentGroup.AssessmentGroupID
                                       select agrp);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Assessment Group already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Assessment Group
                if (pObjAssessmentGroup.AssessmentGroupID != 0)
                {
                    mObjInsertUpdateAssessmentGroup = (from agrp in _db.Assessment_Group
                                                     where agrp.AssessmentGroupID == pObjAssessmentGroup.AssessmentGroupID
                                                     select agrp).FirstOrDefault();

                    if (mObjInsertUpdateAssessmentGroup != null)
                    {
                        mObjInsertUpdateAssessmentGroup.ModifiedBy = pObjAssessmentGroup.ModifiedBy;
                        mObjInsertUpdateAssessmentGroup.ModifiedDate = pObjAssessmentGroup.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateAssessmentGroup = new Assessment_Group();
                        mObjInsertUpdateAssessmentGroup.CreatedBy = pObjAssessmentGroup.CreatedBy;
                        mObjInsertUpdateAssessmentGroup.CreatedDate = pObjAssessmentGroup.CreatedDate;
                    }
                }
                else // Else Insert Assessment Group
                {
                    mObjInsertUpdateAssessmentGroup = new Assessment_Group();
                    mObjInsertUpdateAssessmentGroup.CreatedBy = pObjAssessmentGroup.CreatedBy;
                    mObjInsertUpdateAssessmentGroup.CreatedDate = pObjAssessmentGroup.CreatedDate;
                }

                mObjInsertUpdateAssessmentGroup.AssessmentGroupName = pObjAssessmentGroup.AssessmentGroupName;
                mObjInsertUpdateAssessmentGroup.AssetTypeID = pObjAssessmentGroup.AssetTypeID;
                mObjInsertUpdateAssessmentGroup.Active = pObjAssessmentGroup.Active;

                if (pObjAssessmentGroup.AssessmentGroupID == 0)
                {
                    _db.Assessment_Group.Add(mObjInsertUpdateAssessmentGroup);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjAssessmentGroup.AssessmentGroupID == 0)
                        mObjFuncResponse.Message = "Assessment Group Added Successfully";
                    else
                        mObjFuncResponse.Message = "Assessment Group Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjAssessmentGroup.AssessmentGroupID == 0)
                        mObjFuncResponse.Message = "Assessment Group Addition Failed";
                    else
                        mObjFuncResponse.Message = "Assessment Group Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetAssessmentGroupList_Result> REP_GetAssessmentGroupList(Assessment_Group pObjAssessmentGroup)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentGroupList(pObjAssessmentGroup.AssessmentGroupName, pObjAssessmentGroup.AssessmentGroupID, pObjAssessmentGroup.AssetTypeID, pObjAssessmentGroup.AssessmentGroupIds, pObjAssessmentGroup.intStatus, pObjAssessmentGroup.IncludeAssessmentGroupIds, pObjAssessmentGroup.ExcludeAssessmentGroupIds).ToList();
            }
        }

        public usp_GetAssessmentGroupList_Result REP_GetAssessmentGroupDetails(Assessment_Group pObjAssessmentGroup)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentGroupList(pObjAssessmentGroup.AssessmentGroupName, pObjAssessmentGroup.AssessmentGroupID, pObjAssessmentGroup.AssetTypeID, pObjAssessmentGroup.AssessmentGroupIds, pObjAssessmentGroup.intStatus, pObjAssessmentGroup.IncludeAssessmentGroupIds, pObjAssessmentGroup.ExcludeAssessmentGroupIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetAssessmentGroupDropDownList(Assessment_Group pObjAssessmentGroup)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from agrp in _db.usp_GetAssessmentGroupList(pObjAssessmentGroup.AssessmentGroupName, pObjAssessmentGroup.AssessmentGroupID, pObjAssessmentGroup.AssetTypeID, pObjAssessmentGroup.AssessmentGroupIds, pObjAssessmentGroup.intStatus, pObjAssessmentGroup.IncludeAssessmentGroupIds, pObjAssessmentGroup.ExcludeAssessmentGroupIds)
                               select new DropDownListResult()
                               {
                                   id = agrp.AssessmentGroupID.GetValueOrDefault(),
                                   text = agrp.AssessmentGroupName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Assessment_Group pObjAssessmentGroup)
        {
            using (_db = new EIRSEntities())
            {
                Assessment_Group mObjInsertUpdateAssessmentGroup; //Assessment Group Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load AssessmentGroup
                if (pObjAssessmentGroup.AssessmentGroupID != 0)
                {
                    mObjInsertUpdateAssessmentGroup = (from agrp in _db.Assessment_Group
                                                     where agrp.AssessmentGroupID == pObjAssessmentGroup.AssessmentGroupID
                                                     select agrp).FirstOrDefault();

                    if (mObjInsertUpdateAssessmentGroup != null)
                    {
                        mObjInsertUpdateAssessmentGroup.Active = !mObjInsertUpdateAssessmentGroup.Active;
                        mObjInsertUpdateAssessmentGroup.ModifiedBy = pObjAssessmentGroup.ModifiedBy;
                        mObjInsertUpdateAssessmentGroup.ModifiedDate = pObjAssessmentGroup.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Assessment Group Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetAssessmentGroupList(pObjAssessmentGroup.AssessmentGroupName, 0, pObjAssessmentGroup.AssetTypeID, pObjAssessmentGroup.AssessmentGroupIds, pObjAssessmentGroup.intStatus, pObjAssessmentGroup.IncludeAssessmentGroupIds, pObjAssessmentGroup.ExcludeAssessmentGroupIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Assessment Group Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
