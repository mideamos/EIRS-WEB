using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EIRS.Repository
{
    public class AssessmentRepository : IAssessmentRepository
    {
        EIRSEntities _db;

        public FuncResponse<Assessment> REP_InsertUpdateAssessment(Assessment pObjAssessment, int ruleId, int assId)
        {
            using (_db = new EIRSEntities())
            {
                Assessment mObjInsertUpdateAssessment; //Assessment Insert Object
                FuncResponse<Assessment> mObjFuncResponse = new FuncResponse<Assessment>(); //Return Object
                decimal? amount=0;
                var arule = _db.Assessment_Rules.FirstOrDefault(x => x.AssessmentRuleID == ruleId);
                if(arule!= null)
                    amount = arule.AssessmentAmount.GetValueOrDefault();
                //Check if Duplicate 
                var vDuplicateCheck = (from assmnt in _db.MAP_Assessment_AssessmentRule
                                       where assmnt.AssessmentRuleID == ruleId
                                       && assmnt.AssessmentAmount == amount
                                       && assmnt.AssetID == assId
                                       && assmnt.CreatedBy == pObjAssessment.CreatedBy
                                       && assmnt.AssessmentID != pObjAssessment.AssessmentID
                                       select assmnt);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Assessment already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Assessment Item
                if (pObjAssessment.AssessmentID != 0)
                {
                    mObjInsertUpdateAssessment = (from assmnt in _db.Assessments
                                                  where assmnt.AssessmentID == pObjAssessment.AssessmentID
                                                  select assmnt).FirstOrDefault();

                    if (mObjInsertUpdateAssessment != null)
                    {
                        mObjInsertUpdateAssessment.ModifiedBy = pObjAssessment.ModifiedBy;
                        mObjInsertUpdateAssessment.ModifiedDate = pObjAssessment.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateAssessment = new Assessment
                        {
                            CreatedBy = pObjAssessment.CreatedBy,
                            CreatedDate = pObjAssessment.CreatedDate
                        };
                    }
                }
                else // Else Insert Assessment Item
                {
                    mObjInsertUpdateAssessment = new Assessment
                    {
                        CreatedBy = pObjAssessment.CreatedBy,
                        CreatedDate = pObjAssessment.CreatedDate
                    };
                }

                mObjInsertUpdateAssessment.AssessmentDate = pObjAssessment.AssessmentDate != null ? pObjAssessment.AssessmentDate : mObjInsertUpdateAssessment.AssessmentDate;
                mObjInsertUpdateAssessment.TaxPayerTypeID = pObjAssessment.TaxPayerTypeID != null ? pObjAssessment.TaxPayerTypeID : mObjInsertUpdateAssessment.TaxPayerTypeID;
                mObjInsertUpdateAssessment.TaxPayerID = pObjAssessment.TaxPayerID != null ? pObjAssessment.TaxPayerID : mObjInsertUpdateAssessment.TaxPayerID;
                mObjInsertUpdateAssessment.AssessmentAmount = pObjAssessment.AssessmentAmount;
                mObjInsertUpdateAssessment.SettlementDate = pObjAssessment.SettlementDate != null ? pObjAssessment.SettlementDate : mObjInsertUpdateAssessment.SettlementDate; ;
                mObjInsertUpdateAssessment.SettlementStatusID = pObjAssessment.SettlementStatusID != null ? pObjAssessment.SettlementStatusID : mObjInsertUpdateAssessment.SettlementStatusID;
                mObjInsertUpdateAssessment.SettlementDueDate = pObjAssessment.SettlementDueDate;
                mObjInsertUpdateAssessment.AssessmentNotes = pObjAssessment.AssessmentNotes;
                mObjInsertUpdateAssessment.Active = pObjAssessment.Active != null ? pObjAssessment.Active : mObjInsertUpdateAssessment.Active;

                if (pObjAssessment.AssessmentID == 0)
                {
                    _db.Assessments.Add(mObjInsertUpdateAssessment);
                }

                try
                {
                    _db.SaveChanges();

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);

                    mObjFuncResponse.Success = true;
                    if (pObjAssessment.AssessmentID == 0)
                    {
                        mObjFuncResponse.Message = "Assessment Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Assessment Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateAssessment;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjAssessment.AssessmentID == 0)
                    {
                        mObjFuncResponse.Message = "Assessment Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Assessment Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }
        public FuncResponse<Assessment> REP_InsertUpdateAssessment(Assessment pObjAssessment, int ruleId)
        {
            using (_db = new EIRSEntities())
            {
                Assessment mObjInsertUpdateAssessment; //Assessment Insert Object
                FuncResponse<Assessment> mObjFuncResponse = new FuncResponse<Assessment>(); //Return Object

                decimal amount = _db.Assessment_Rules.FirstOrDefault(x => x.AssessmentRuleID == ruleId).AssessmentAmount.GetValueOrDefault();
                //Check if Duplicate 
                var vDuplicateCheck = (from assmnt in _db.MAP_Assessment_AssessmentRule
                                       where assmnt.AssessmentRuleID == ruleId
                                       && assmnt.AssessmentAmount == amount
                                       //&& assmnt.AssetID ==
                                       && assmnt.CreatedBy == pObjAssessment.CreatedBy
                                       && assmnt.AssessmentID != pObjAssessment.AssessmentID
                                       select assmnt);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Assessment already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Assessment Item
                if (pObjAssessment.AssessmentID != 0)
                {
                    mObjInsertUpdateAssessment = (from assmnt in _db.Assessments
                                                  where assmnt.AssessmentID == pObjAssessment.AssessmentID
                                                  select assmnt).FirstOrDefault();

                    if (mObjInsertUpdateAssessment != null)
                    {
                        mObjInsertUpdateAssessment.ModifiedBy = pObjAssessment.ModifiedBy;
                        mObjInsertUpdateAssessment.ModifiedDate = pObjAssessment.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateAssessment = new Assessment
                        {
                            CreatedBy = pObjAssessment.CreatedBy,
                            CreatedDate = pObjAssessment.CreatedDate
                        };
                    }
                }
                else // Else Insert Assessment Item
                {
                    mObjInsertUpdateAssessment = new Assessment
                    {
                        CreatedBy = pObjAssessment.CreatedBy,
                        CreatedDate = pObjAssessment.CreatedDate
                    };
                }

                mObjInsertUpdateAssessment.AssessmentDate = pObjAssessment.AssessmentDate != null ? pObjAssessment.AssessmentDate : mObjInsertUpdateAssessment.AssessmentDate;
                mObjInsertUpdateAssessment.TaxPayerTypeID = pObjAssessment.TaxPayerTypeID != null ? pObjAssessment.TaxPayerTypeID : mObjInsertUpdateAssessment.TaxPayerTypeID;
                mObjInsertUpdateAssessment.TaxPayerID = pObjAssessment.TaxPayerID != null ? pObjAssessment.TaxPayerID : mObjInsertUpdateAssessment.TaxPayerID;
                mObjInsertUpdateAssessment.AssessmentAmount = pObjAssessment.AssessmentAmount;
                mObjInsertUpdateAssessment.SettlementDate = pObjAssessment.SettlementDate != null ? pObjAssessment.SettlementDate : mObjInsertUpdateAssessment.SettlementDate; ;
                mObjInsertUpdateAssessment.SettlementStatusID = pObjAssessment.SettlementStatusID != null ? pObjAssessment.SettlementStatusID : mObjInsertUpdateAssessment.SettlementStatusID;
                mObjInsertUpdateAssessment.SettlementDueDate = pObjAssessment.SettlementDueDate;
                mObjInsertUpdateAssessment.AssessmentNotes = pObjAssessment.AssessmentNotes;
                mObjInsertUpdateAssessment.Active = pObjAssessment.Active != null ? pObjAssessment.Active : mObjInsertUpdateAssessment.Active;

                if (pObjAssessment.AssessmentID == 0)
                {
                    _db.Assessments.Add(mObjInsertUpdateAssessment);
                }

                try
                {
                    _db.SaveChanges();

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);

                    mObjFuncResponse.Success = true;
                    if (pObjAssessment.AssessmentID == 0)
                    {
                        mObjFuncResponse.Message = "Assessment Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Assessment Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateAssessment;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjAssessment.AssessmentID == 0)
                    {
                        mObjFuncResponse.Message = "Assessment Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Assessment Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }
        public FuncResponse<Assessment> REP_InsertUpdateAssessment(Assessment pObjAssessment)
        {
            using (_db = new EIRSEntities())
            {
                Assessment mObjInsertUpdateAssessment; //Assessment Insert Object
                FuncResponse<Assessment> mObjFuncResponse = new FuncResponse<Assessment>(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from assmnt in _db.Assessments
                                       where assmnt.TaxPayerID == pObjAssessment.TaxPayerID
                                       && assmnt.TaxPayerTypeID == pObjAssessment.TaxPayerTypeID
                                       && assmnt.AssessmentID != pObjAssessment.AssessmentID

                                       select assmnt).ToList();

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Assessment already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Assessment Item
                if (pObjAssessment.AssessmentID != 0)
                {
                    mObjInsertUpdateAssessment = (from assmnt in _db.Assessments
                                                  where assmnt.AssessmentID == pObjAssessment.AssessmentID
                                                  select assmnt).FirstOrDefault();

                    if (mObjInsertUpdateAssessment != null)
                    {
                        mObjInsertUpdateAssessment.ModifiedBy = pObjAssessment.ModifiedBy;
                        mObjInsertUpdateAssessment.ModifiedDate = pObjAssessment.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateAssessment = new Assessment
                        {
                            CreatedBy = pObjAssessment.CreatedBy,
                            CreatedDate = pObjAssessment.CreatedDate
                        };
                    }
                }
                else // Else Insert Assessment Item
                {
                    mObjInsertUpdateAssessment = new Assessment
                    {
                        CreatedBy = pObjAssessment.CreatedBy,
                        CreatedDate = pObjAssessment.CreatedDate
                    };
                }

                mObjInsertUpdateAssessment.AssessmentDate = pObjAssessment.AssessmentDate != null ? pObjAssessment.AssessmentDate : mObjInsertUpdateAssessment.AssessmentDate;
                mObjInsertUpdateAssessment.TaxPayerTypeID = pObjAssessment.TaxPayerTypeID != null ? pObjAssessment.TaxPayerTypeID : mObjInsertUpdateAssessment.TaxPayerTypeID;
                mObjInsertUpdateAssessment.TaxPayerID = pObjAssessment.TaxPayerID != null ? pObjAssessment.TaxPayerID : mObjInsertUpdateAssessment.TaxPayerID;
                mObjInsertUpdateAssessment.AssessmentAmount = pObjAssessment.AssessmentAmount;
                mObjInsertUpdateAssessment.SettlementDate = pObjAssessment.SettlementDate != null ? pObjAssessment.SettlementDate : mObjInsertUpdateAssessment.SettlementDate; ;
                mObjInsertUpdateAssessment.SettlementStatusID = pObjAssessment.SettlementStatusID != null ? pObjAssessment.SettlementStatusID : mObjInsertUpdateAssessment.SettlementStatusID;
                mObjInsertUpdateAssessment.SettlementDueDate = pObjAssessment.SettlementDueDate;
                mObjInsertUpdateAssessment.AssessmentNotes = pObjAssessment.AssessmentNotes;
                mObjInsertUpdateAssessment.Active = pObjAssessment.Active != null ? pObjAssessment.Active : mObjInsertUpdateAssessment.Active;

                if (pObjAssessment.AssessmentID == 0)
                {
                    _db.Assessments.Add(mObjInsertUpdateAssessment);
                }

                try
                {
                    _db.SaveChanges();

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);

                    mObjFuncResponse.Success = true;
                    if (pObjAssessment.AssessmentID == 0)
                    {
                        mObjFuncResponse.Message = "Assessment Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Assessment Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateAssessment;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjAssessment.AssessmentID == 0)
                    {
                        mObjFuncResponse.Message = "Assessment Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Assessment Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<vw_AssessmentBill> REP_GetAssessmentList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_AssessmentBill.ToList();
            }
        }

        public IList<usp_GetAssessmentList_Result> REP_GetAssessmentList(Assessment pObjAssessment)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentList(pObjAssessment.TaxPayerTypeID, pObjAssessment.TaxPayerID, pObjAssessment.AssessmentID, pObjAssessment.AssessmentRefNo, pObjAssessment.IntStatus).ToList();
            }
        }

        public usp_GetAssessmentList_Result REP_GetAssessmentDetails(Assessment pObjAssessment)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentList(pObjAssessment.TaxPayerTypeID, pObjAssessment.TaxPayerID, pObjAssessment.AssessmentID, pObjAssessment.AssessmentRefNo, pObjAssessment.IntStatus).FirstOrDefault();
            }
        }

        public Assessment REP_GetAssessmentDetailsById(long assessmentId)
        {
            using (_db = new EIRSEntities())
            {
                var assessment = (from ast in _db.Assessments
                                  where ast.AssessmentID == assessmentId
                                  select ast).FirstOrDefault();
                return assessment;
            }
        }

        public IList<usp_GetAssessmentRuleItemList_Result> REP_GetAssessmentRuleItem(int pIntAssessmentID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentRuleItemList(pIntAssessmentID).ToList();
            }
        }

        public usp_GetAssessmentRuleItemDetails_Result REP_GetAssessmentRuleItemDetails(long plngAAIID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentRuleItemDetails(plngAAIID).FirstOrDefault();
            }
        }

        public IList<usp_GetAssessmentAdjustmentList_Result> REP_GetAssessmentAdjustment(int pIntAssessmentID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentAdjustmentList(pIntAssessmentID).ToList();
            }
        }

        public IList<usp_GetAssessmentLateChargeList_Result> REP_GetAssessmentLateCharge(int pIntAssessmentID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentLateChargeList(pIntAssessmentID).ToList();
            }
        }

        public FuncResponse REP_UpdateStatus(Assessment pObjAssessment)
        {
            using (_db = new EIRSEntities())
            {
                Assessment mObjInsertUpdateAssessment; //Assessment_Rules Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Assessment_Rules
                if (pObjAssessment.AssessmentID != 0)
                {
                    mObjInsertUpdateAssessment = (from ast in _db.Assessments
                                                  where ast.AssessmentID == pObjAssessment.AssessmentID
                                                  select ast).FirstOrDefault();

                    if (mObjInsertUpdateAssessment != null)
                    {
                        mObjInsertUpdateAssessment.Active = !mObjInsertUpdateAssessment.Active;
                        mObjInsertUpdateAssessment.ModifiedBy = pObjAssessment.ModifiedBy;
                        mObjInsertUpdateAssessment.ModifiedDate = pObjAssessment.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Assessment Updated Successfully";
                            //mObjFuncResponse.AdditionalData = _db.usp_GetAssessmentList(pObjAssessment.TaxPayerTypeID, pObjAssessment.TaxPayerID, 0, "", pObjAssessment.IntStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Assessment Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_UpdateAssessmentSettlementStatus(Assessment pObjAssessment)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                Assessment mObjUpdateAssessment = _db.Assessments.Find(pObjAssessment.AssessmentID);

                if (mObjUpdateAssessment != null)
                {
                    mObjUpdateAssessment.SettlementDate = pObjAssessment.SettlementDate == null ? mObjUpdateAssessment.SettlementDate : pObjAssessment.SettlementDate;
                    mObjUpdateAssessment.SettlementStatusID = pObjAssessment.SettlementStatusID;
                    mObjUpdateAssessment.ModifiedBy = pObjAssessment.ModifiedBy;
                    mObjUpdateAssessment.ModifiedDate = pObjAssessment.ModifiedDate;
                    mObjUpdateAssessment.AssessmentAmount = pObjAssessment.AssessmentAmount;
                }

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Exception = Ex;
                }

                return mObjResponse;
            }
        }

        public FuncResponse<MAP_Assessment_AssessmentRule> REP_InsertUpdateAssessmentRule(MAP_Assessment_AssessmentRule pObjAssessmentRule)
        {
            using (_db = new EIRSEntities())
            {
                MAP_Assessment_AssessmentRule mObjInsertUpdateAssessmentRule; //Assessment Insert Object
                FuncResponse<MAP_Assessment_AssessmentRule> mObjFuncResponse = new FuncResponse<MAP_Assessment_AssessmentRule>(); //Return Object

                if (pObjAssessmentRule.AARID > 0)
                {
                    mObjInsertUpdateAssessmentRule = _db.MAP_Assessment_AssessmentRule.Find(pObjAssessmentRule.AARID);

                    if (mObjInsertUpdateAssessmentRule != null)
                    {
                        mObjInsertUpdateAssessmentRule.AssessmentAmount = pObjAssessmentRule.AssessmentAmount;
                        mObjInsertUpdateAssessmentRule.ModifiedBy = pObjAssessmentRule.ModifiedBy;
                        mObjInsertUpdateAssessmentRule.ModifiedDate = pObjAssessmentRule.ModifiedDate;

                    }
                    else
                    {
                        throw (new Exception("ARNOTFOUND"));
                    }
                }
                else
                {
                    //Check if already exists for same tax payer and tax payer type

                    var vDuplicateCheck = (from ass in _db.Assessments
                                           join arule in _db.MAP_Assessment_AssessmentRule on ass.AssessmentID equals arule.AssessmentID
                                           where arule.AssessmentRuleID == pObjAssessmentRule.AssessmentRuleID && arule.AssetID == pObjAssessmentRule.AssetID
                                           && arule.AssetTypeID == pObjAssessmentRule.AssetTypeID && arule.ProfileID == pObjAssessmentRule.ProfileID
                                           && ass.TaxPayerID == pObjAssessmentRule.TaxPayerID && ass.TaxPayerTypeID == pObjAssessmentRule.TaxPayerTypeID
                                           && ass.Active == true
                                           select ass);

                    if (vDuplicateCheck.Count() > 0)
                    {
                        throw (new Exception("ARALREADY"));
                    }

                    mObjInsertUpdateAssessmentRule = new MAP_Assessment_AssessmentRule
                    {
                        AssessmentID = pObjAssessmentRule.AssessmentID,
                        AssetTypeID = pObjAssessmentRule.AssetTypeID,
                        AssetID = pObjAssessmentRule.AssetID,
                        ProfileID = pObjAssessmentRule.ProfileID,
                        AssessmentRuleID = pObjAssessmentRule.AssessmentRuleID,
                        AssessmentAmount = pObjAssessmentRule.AssessmentAmount,
                        AssessmentYear = pObjAssessmentRule.AssessmentYear,
                        CreatedBy = pObjAssessmentRule.CreatedBy,
                        CreatedDate = pObjAssessmentRule.CreatedDate,
                    };

                    _db.MAP_Assessment_AssessmentRule.Add(mObjInsertUpdateAssessmentRule);

                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Assessment Rule Added Successfully";
                    mObjFuncResponse.AdditionalData = mObjInsertUpdateAssessmentRule;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    mObjFuncResponse.Message = "Assessment Rule Addition Failed";

                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetAssessment_AssessmentRuleList_Result> REP_GetAssessmentRules(int pIntAssessmentID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessment_AssessmentRuleList(pIntAssessmentID).ToList();
            }
        }

        public IList<usp_GetAssessmentRuleBasedSettlement_Result> REP_GetAssessmentRuleBasedSettlement(int pIntAssessmentID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentRuleBasedSettlement(pIntAssessmentID).ToList();
            }
        }

        public FuncResponse REP_DeleteAssessmentRule(long pIntAARID)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                MAP_Assessment_AssessmentRule mObjDeleteAssessmentRule = _db.MAP_Assessment_AssessmentRule.Find(pIntAARID);

                if (mObjDeleteAssessmentRule != null)
                {
                    _db.MAP_Assessment_AssessmentRule.Remove(mObjDeleteAssessmentRule);

                    // Find Assessment Rule Item 

                    var vAssessmentItems = _db.MAP_Assessment_AssessmentItem.Where(t => t.AARID == pIntAARID);

                    if (vAssessmentItems.Count() > 0)
                    {
                        _db.MAP_Assessment_AssessmentItem.RemoveRange(vAssessmentItems);
                    }

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        mObjFuncResponse.Message = "Assessment Rule Removed Successfully";

                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Exception = Ex;
                        mObjFuncResponse.Message = "Assessment Rule Deletion Failed";

                    }

                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Invalid Assessment Rule";
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertAssessmentItem(IList<MAP_Assessment_AssessmentItem> plstAssessmentItem)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();
                MAP_Assessment_AssessmentItem mObjInsertAssessmentItem;

                foreach (MAP_Assessment_AssessmentItem mObjAIDetail in plstAssessmentItem)
                {
                    if (mObjAIDetail.AAIID > 0)
                    {
                        mObjInsertAssessmentItem = _db.MAP_Assessment_AssessmentItem.Find(mObjAIDetail.AAIID);
                        if (mObjInsertAssessmentItem != null)
                        {
                            mObjInsertAssessmentItem.TaxBaseAmount = mObjAIDetail.TaxBaseAmount;
                            mObjInsertAssessmentItem.Percentage = mObjAIDetail.Percentage;
                            mObjInsertAssessmentItem.TaxAmount = mObjAIDetail.TaxAmount;
                            mObjInsertAssessmentItem.ModifiedBy = mObjAIDetail.ModifiedBy;
                            mObjInsertAssessmentItem.ModifiedDate = mObjAIDetail.ModifiedDate;
                        }
                        else
                        {
                            throw (new Exception("AINOTFOUND"));
                        }
                    }
                    else
                    {

                        mObjInsertAssessmentItem = new MAP_Assessment_AssessmentItem()
                        {
                            AARID = mObjAIDetail.AARID,
                            AssessmentItemID = mObjAIDetail.AssessmentItemID,
                            TaxBaseAmount = mObjAIDetail.TaxBaseAmount,
                            Percentage = mObjAIDetail.Percentage,
                            TaxAmount = mObjAIDetail.TaxAmount,
                            PaymentStatusID = mObjAIDetail.PaymentStatusID,
                            CreatedBy = mObjAIDetail.CreatedBy,
                            CreatedDate = mObjAIDetail.CreatedDate,
                        };

                        _db.MAP_Assessment_AssessmentItem.Add(mObjInsertAssessmentItem);
                    }
                }

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Exception = Ex;
                }

                return mObjResponse;
            }
        }

        public IList<MAP_Assessment_AssessmentItem> REP_GetAssessmentItems(long plngAARID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.MAP_Assessment_AssessmentItem.Include("Assessment_Items").Where(t => t.AARID == plngAARID).ToList();
            }
        }
        public MAP_Assessment_AssessmentItem GetAssessmentItems(long aAIID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.MAP_Assessment_AssessmentItem.Where(t => t.AAIID == aAIID).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateAssessmentItemStatus(MAP_Assessment_AssessmentItem pObjAssessmentItem)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                MAP_Assessment_AssessmentItem mObjUpdateAssessmentItem = _db.MAP_Assessment_AssessmentItem.Find(pObjAssessmentItem.AAIID);

                if (mObjUpdateAssessmentItem != null)
                {
                    mObjUpdateAssessmentItem.PaymentStatusID = pObjAssessmentItem.PaymentStatusID;
                    // mObjUpdateAssessmentItem.TaxBaseAmount = pObjAssessmentItem.TaxBaseAmount;
                    mObjUpdateAssessmentItem.TaxAmount = pObjAssessmentItem.TaxAmount;
                    mObjUpdateAssessmentItem.ModifiedBy = pObjAssessmentItem.ModifiedBy;
                    mObjUpdateAssessmentItem.ModifiedDate = pObjAssessmentItem.ModifiedDate;
                }

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Exception = Ex;
                }

                return mObjResponse;
            }
        }
        public FuncResponse REP_UpdateAssessmentItemStatus2(MAP_Assessment_AssessmentItem pObjAssessmentItem)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();
                decimal? checker = 0;
                MAP_Assessment_AssessmentItem mObjUpdateAssessmentItem = _db.MAP_Assessment_AssessmentItem.Find(pObjAssessmentItem.AAIID);
                
                if (mObjUpdateAssessmentItem != null)
                {
                    List<MAP_Assessment_AssessmentItem> zeroValue = _db.MAP_Assessment_AssessmentItem.Where(k => k.AARID == mObjUpdateAssessmentItem.AARID && k.TaxAmount == checker).ToList();
                    foreach (var zeroValueItem in zeroValue)
                    {
                        zeroValueItem.PaymentStatusID = 3;
                    }
                    mObjUpdateAssessmentItem.PaymentStatusID = pObjAssessmentItem.PaymentStatusID;
                    // mObjUpdateAssessmentItem.TaxBaseAmount = pObjAssessmentItem.TaxBaseAmount;
                    //mObjUpdateAssessmentItem.TaxAmount = pObjAssessmentItem.TaxAmount;
                    mObjUpdateAssessmentItem.ModifiedBy = pObjAssessmentItem.ModifiedBy;
                    mObjUpdateAssessmentItem.ModifiedDate = pObjAssessmentItem.ModifiedDate;
                }

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Exception = Ex;
                }

                return mObjResponse;
            }
        }

        public IList<DropDownListResult> REP_GetSettlementMethodAssessmentRuleBased(long pIntAssessmentID)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from settlmthd in _db.Settlement_Method
                               join sa in _db.MAP_AssessmentRule_SettlementMethod on settlmthd.SettlementMethodID equals sa.SettlementMethodID
                               join arule in _db.MAP_Assessment_AssessmentRule on sa.AssessmentRuleID equals arule.AssessmentRuleID
                               where arule.AssessmentID == pIntAssessmentID
                               select new DropDownListResult()
                               {
                                   id = settlmthd.SettlementMethodID,
                                   text = settlmthd.SettlementMethodName
                               }).Distinct().ToList();

                return vResult;
            }
        }

        public IList<usp_GetTaxPayerBill_Result> REP_GetTaxPayerBill(int pIntTaxPayerID, int pIntTaxPayerTypeID, int pIntStatusID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerBill(pIntTaxPayerID, pIntTaxPayerTypeID, pIntStatusID).ToList();
            }
        }

        public IList<vw_BillForPoASettlement> REP_GetBillForPoASettlementList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_BillForPoASettlement.ToList();
            }
        }

        public IList<usp_GetAssessmentDataForPT_Result> REP_GetAssessmentDataForPT()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentDataForPT().ToList();
            }
        }

        public void REP_UpdateAssessmentAmount(int pIntAssessmentID)
        {
            using (_db = new EIRSEntities())
            {
                _db.usp_UpdateAssessmentAmount(pIntAssessmentID);
            }
        }

        public IDictionary<string, object> REP_SearchAssessment(Assessment pObjAssessment)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["AssessmentList"] = _db.usp_SearchAssessment(pObjAssessment.WhereCondition, pObjAssessment.OrderBy, pObjAssessment.OrderByDirection, pObjAssessment.PageNumber, pObjAssessment.PageSize, pObjAssessment.MainFilter,
                                                                        pObjAssessment.AssessmentRefNo, pObjAssessment.strAssessmentDate, pObjAssessment.TaxPayerTypeName, pObjAssessment.TaxPayerRIN, pObjAssessment.TaxPayerName, pObjAssessment.strAssessmentAmount, pObjAssessment.strSettlementDueDate, pObjAssessment.strSettlementDate, pObjAssessment.SettlementStatusName, pObjAssessment.AssessmentNotes, pObjAssessment.ActiveText).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(AssessmentID) FROM Assessment").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(AssessmentID) ");
                sbFilteredCountQuery.Append(" FROM Assessment ast ");
                sbFilteredCountQuery.Append(" INNER JOIN Settlement_Status ss ON ast.SettlementStatusID = ss.SettlementStatusID ");
                sbFilteredCountQuery.Append(" INNER JOIN TaxPayer_Types tptype ON ast.TaxPayerTypeID = tptype.TaxPayerTypeID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjAssessment.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjAssessment.MainFilter),
                    new SqlParameter("@AssessmentRefNo",pObjAssessment.AssessmentRefNo),
                    new SqlParameter("@AssessmentDate",pObjAssessment.strAssessmentDate),
                    new SqlParameter("@TaxPayerTypeName",pObjAssessment.TaxPayerTypeName),
                    new SqlParameter("@TaxPayerRIN",pObjAssessment.TaxPayerRIN),
                    new SqlParameter("@TaxPayerName",pObjAssessment.TaxPayerName),
                    new SqlParameter("@AssessmentAmount",pObjAssessment.strAssessmentAmount),
                    new SqlParameter("@SettlementDueDate",pObjAssessment.strSettlementDueDate),
                    new SqlParameter("@SettlementDate",pObjAssessment.strSettlementDate),
                    new SqlParameter("@SettlementStatus",pObjAssessment.SettlementStatusName),
                    new SqlParameter("@AssessmentNotes",pObjAssessment.AssessmentNotes),
                    new SqlParameter("@ActiveText",pObjAssessment.ActiveText),
                };

                //Get Filtered Count
                //int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntTotalCount;

                return dcData;
            }
        }

        public FuncResponse REP_InsertAdjustment(MAP_Assessment_Adjustment pObjAdjustment)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();
                MAP_Assessment_Adjustment mObjInsertAdjustment = new MAP_Assessment_Adjustment()
                {
                    AAIID = pObjAdjustment.AAIID,
                    AdjustmentDate = pObjAdjustment.AdjustmentDate,
                    AdjustmentLine = pObjAdjustment.AdjustmentLine,
                    AdjustmentTypeID = pObjAdjustment.AdjustmentTypeID,
                    Amount = pObjAdjustment.Amount,
                    CreatedBy = pObjAdjustment.CreatedBy,
                    CreatedDate = pObjAdjustment.CreatedDate,
                };

                _db.MAP_Assessment_Adjustment.Add(mObjInsertAdjustment);

                try
                {
                    _db.SaveChanges();

                    //Get Assessment Id
                    var vResult = (from aaitem in _db.MAP_Assessment_AssessmentItem
                                   join aarule in _db.MAP_Assessment_AssessmentRule on aaitem.AARID equals aarule.AARID
                                   where aaitem.AAIID == pObjAdjustment.AAIID
                                   select aarule.AssessmentID).FirstOrDefault();

                    mObjResponse.Success = true;
                    mObjResponse.Message = "Adjustment Added Successfully";
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Exception = Ex;
                    mObjResponse.Message = "Adjustment Adding Failed";
                }

                return mObjResponse;
            }
        }

        public IDictionary<string, object> REP_SearchAssessmentForSideMenu(Assessment pObjAssessment)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["AssessmentList"] = _db.usp_SearchAssessmentForSideMenu(pObjAssessment.WhereCondition, pObjAssessment.OrderBy, pObjAssessment.OrderByDirection, pObjAssessment.PageNumber, pObjAssessment.PageSize, pObjAssessment.MainFilter).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(AssessmentID) FROM Assessment ast INNER JOIN Settlement_Status ss ON ast.SettlementStatusID = ss.SettlementStatusID").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(AssessmentID) ");
                sbFilteredCountQuery.Append(" FROM Assessment ast ");
                sbFilteredCountQuery.Append(" INNER JOIN Settlement_Status ss ON ast.SettlementStatusID = ss.SettlementStatusID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjAssessment.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjAssessment.MainFilter)
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

        public IList<usp_GetPAYEAssessmentBill_Result> REP_GetPAYEAssessmentBill(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPAYEAssessmentBill(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }

        
    }
}
