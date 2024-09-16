using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class AssessmentSubGroupRepository : IAssessmentSubGroupRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateAssessmentSubGroup(Assessment_SubGroup pObjAssessmentSubGroup)
        {
            using (_db = new EIRSEntities())
            {
                Assessment_SubGroup mObjInsertUpdateAssessmentSubGroup; //Assessment Sub Group Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from rsstrm in _db.Assessment_SubGroup
                                       where rsstrm.AssessmentSubGroupName == pObjAssessmentSubGroup.AssessmentSubGroupName && rsstrm.AssessmentGroupID == pObjAssessmentSubGroup.AssessmentGroupID && rsstrm.AssessmentSubGroupID != pObjAssessmentSubGroup.AssessmentSubGroupID
                                       select rsstrm);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Assessment Sub Group already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Assessment Sub Group
                if (pObjAssessmentSubGroup.AssessmentSubGroupID != 0)
                {
                    mObjInsertUpdateAssessmentSubGroup = (from rsstrm in _db.Assessment_SubGroup
                                                        where rsstrm.AssessmentSubGroupID == pObjAssessmentSubGroup.AssessmentSubGroupID
                                                        select rsstrm).FirstOrDefault();

                    if (mObjInsertUpdateAssessmentSubGroup != null)
                    {
                        mObjInsertUpdateAssessmentSubGroup.ModifiedBy = pObjAssessmentSubGroup.ModifiedBy;
                        mObjInsertUpdateAssessmentSubGroup.ModifiedDate = pObjAssessmentSubGroup.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateAssessmentSubGroup = new Assessment_SubGroup();
                        mObjInsertUpdateAssessmentSubGroup.CreatedBy = pObjAssessmentSubGroup.CreatedBy;
                        mObjInsertUpdateAssessmentSubGroup.CreatedDate = pObjAssessmentSubGroup.CreatedDate;
                    }
                }
                else // Else Insert Assessment Sub Group
                {
                    mObjInsertUpdateAssessmentSubGroup = new Assessment_SubGroup();
                    mObjInsertUpdateAssessmentSubGroup.CreatedBy = pObjAssessmentSubGroup.CreatedBy;
                    mObjInsertUpdateAssessmentSubGroup.CreatedDate = pObjAssessmentSubGroup.CreatedDate;
                }

                mObjInsertUpdateAssessmentSubGroup.AssessmentSubGroupName = pObjAssessmentSubGroup.AssessmentSubGroupName;
                mObjInsertUpdateAssessmentSubGroup.AssessmentGroupID = pObjAssessmentSubGroup.AssessmentGroupID;
                mObjInsertUpdateAssessmentSubGroup.Active = pObjAssessmentSubGroup.Active;

                if (pObjAssessmentSubGroup.AssessmentSubGroupID == 0)
                {
                    _db.Assessment_SubGroup.Add(mObjInsertUpdateAssessmentSubGroup);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjAssessmentSubGroup.AssessmentSubGroupID == 0)
                        mObjFuncResponse.Message = "Assessment Sub Group Added Successfully";
                    else
                        mObjFuncResponse.Message = "Assessment Sub Group Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjAssessmentSubGroup.AssessmentSubGroupID == 0)
                        mObjFuncResponse.Message = "Assessment Sub Group Addition Failed";
                    else
                        mObjFuncResponse.Message = "Assessment Sub Group Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetAssessmentSubGroupList_Result> REP_GetAssessmentSubGroupList(Assessment_SubGroup pObjAssessmentSubGroup)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentSubGroupList(pObjAssessmentSubGroup.AssessmentSubGroupName, pObjAssessmentSubGroup.AssessmentSubGroupID, pObjAssessmentSubGroup.AssetTypeID, pObjAssessmentSubGroup.AssessmentGroupID, pObjAssessmentSubGroup.AssessmentSubGroupIds, pObjAssessmentSubGroup.intStatus, pObjAssessmentSubGroup.IncludeAssessmentSubGroupIds, pObjAssessmentSubGroup.ExcludeAssessmentSubGroupIds).ToList();
            }
        }

        public usp_GetAssessmentSubGroupList_Result REP_GetAssessmentSubGroupDetails(Assessment_SubGroup pObjAssessmentSubGroup)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentSubGroupList(pObjAssessmentSubGroup.AssessmentSubGroupName, pObjAssessmentSubGroup.AssessmentSubGroupID,
                    pObjAssessmentSubGroup.AssetTypeID, pObjAssessmentSubGroup.AssessmentGroupID, pObjAssessmentSubGroup.AssessmentSubGroupIds, pObjAssessmentSubGroup.intStatus, pObjAssessmentSubGroup.IncludeAssessmentSubGroupIds, pObjAssessmentSubGroup.ExcludeAssessmentSubGroupIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetAssessmentSubGroupDropDownList(Assessment_SubGroup pObjAssessmentSubGroup)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from rsstrm in _db.usp_GetAssessmentSubGroupList(pObjAssessmentSubGroup.AssessmentSubGroupName, pObjAssessmentSubGroup.AssessmentSubGroupID, pObjAssessmentSubGroup.AssetTypeID, pObjAssessmentSubGroup.AssessmentGroupID, pObjAssessmentSubGroup.AssessmentSubGroupIds, pObjAssessmentSubGroup.intStatus, pObjAssessmentSubGroup.IncludeAssessmentSubGroupIds, pObjAssessmentSubGroup.ExcludeAssessmentSubGroupIds)
                               select new DropDownListResult()
                               {
                                   id = rsstrm.AssessmentSubGroupID.GetValueOrDefault(),
                                   text = rsstrm.AssessmentSubGroupName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Assessment_SubGroup pObjAssessmentSubGroup)
        {
            using (_db = new EIRSEntities())
            {
                Assessment_SubGroup mObjInsertUpdateAssessmentSubGroup; //Assessment Sub Group Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load AssessmentSubGroup
                if (pObjAssessmentSubGroup.AssessmentSubGroupID != 0)
                {
                    mObjInsertUpdateAssessmentSubGroup = (from rsstrm in _db.Assessment_SubGroup
                                                        where rsstrm.AssessmentSubGroupID == pObjAssessmentSubGroup.AssessmentSubGroupID
                                                        select rsstrm).FirstOrDefault();

                    if (mObjInsertUpdateAssessmentSubGroup != null)
                    {
                        mObjInsertUpdateAssessmentSubGroup.Active = !mObjInsertUpdateAssessmentSubGroup.Active;
                        mObjInsertUpdateAssessmentSubGroup.ModifiedBy = pObjAssessmentSubGroup.ModifiedBy;
                        mObjInsertUpdateAssessmentSubGroup.ModifiedDate = pObjAssessmentSubGroup.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Assessment Sub Group Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetAssessmentSubGroupList(pObjAssessmentSubGroup.AssessmentSubGroupName, 0, pObjAssessmentSubGroup.AssetTypeID, pObjAssessmentSubGroup.AssessmentGroupID, pObjAssessmentSubGroup.AssessmentSubGroupIds, pObjAssessmentSubGroup.intStatus, pObjAssessmentSubGroup.IncludeAssessmentSubGroupIds, pObjAssessmentSubGroup.ExcludeAssessmentSubGroupIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Assessment Sub Group Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
