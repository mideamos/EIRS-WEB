using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Security.Policy;
using System.Text;
using static EIRS.Repository.DataControlRepository;

namespace EIRS.Repository
{
    public class AssessmentRuleRepository : IAssessmentRuleRepository
    {
        EIRSEntities _db;

        public FuncResponse<Assessment_Rules> REP_InsertUpdateAssessmentRule(Assessment_Rules pObjAssessmentRule)
        {
            using (_db = new EIRSEntities())
            {
                Assessment_Rules mObjInsertUpdateAssessmentRule; //Assessment Rule Insert Update Object
                FuncResponse<Assessment_Rules> mObjFuncResponse = new FuncResponse<Assessment_Rules>(); //Return Object


                //If Update Load Assessment_Rules
                if (pObjAssessmentRule.AssessmentRuleID != 0)
                {
                    mObjInsertUpdateAssessmentRule = (from arule in _db.Assessment_Rules
                                                      where arule.AssessmentRuleID == pObjAssessmentRule.AssessmentRuleID
                                                      select arule).FirstOrDefault();

                    if (mObjInsertUpdateAssessmentRule != null)
                    {
                        mObjInsertUpdateAssessmentRule.ModifiedBy = pObjAssessmentRule.ModifiedBy;
                        mObjInsertUpdateAssessmentRule.ModifiedDate = pObjAssessmentRule.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateAssessmentRule = new Assessment_Rules();
                        mObjInsertUpdateAssessmentRule.CreatedBy = pObjAssessmentRule.CreatedBy;
                        mObjInsertUpdateAssessmentRule.CreatedDate = pObjAssessmentRule.CreatedDate;
                    }
                }
                else // Else Insert Assessment_Rules
                {
                    mObjInsertUpdateAssessmentRule = new Assessment_Rules();
                    mObjInsertUpdateAssessmentRule.CreatedBy = pObjAssessmentRule.CreatedBy;
                    mObjInsertUpdateAssessmentRule.CreatedDate = pObjAssessmentRule.CreatedDate;
                }

                mObjInsertUpdateAssessmentRule.AssessmentRuleName = pObjAssessmentRule.AssessmentRuleName;
                mObjInsertUpdateAssessmentRule.ProfileID = pObjAssessmentRule.ProfileID;
                mObjInsertUpdateAssessmentRule.RuleRunID = pObjAssessmentRule.RuleRunID;
                mObjInsertUpdateAssessmentRule.PaymentFrequencyID = pObjAssessmentRule.PaymentFrequencyID;
                mObjInsertUpdateAssessmentRule.AssessmentAmount = pObjAssessmentRule.AssessmentAmount;
                mObjInsertUpdateAssessmentRule.TaxYear = pObjAssessmentRule.TaxYear;
                mObjInsertUpdateAssessmentRule.PaymentOptionID = pObjAssessmentRule.PaymentOptionID;
                mObjInsertUpdateAssessmentRule.Active = pObjAssessmentRule.Active;

                if (pObjAssessmentRule.AssessmentRuleID == 0)
                {
                    var test = _db.Assessment_Rules.Add(mObjInsertUpdateAssessmentRule);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjAssessmentRule.AssessmentRuleID == 0)
                    {
                        mObjFuncResponse.Message = "Assessment Rule Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Assessment Rule Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateAssessmentRule;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjAssessmentRule.AssessmentRuleID == 0)
                    {
                        mObjFuncResponse.Message = "Assessment Rule Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Assessment Rule Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }
        public FuncResponse<List<Assessment_Rules>> REP_InsertUpdateAssessmentRule(List<AssessmentRuleRollover> roll)
        {
            var currentYear = "2023";
            // var currentYear = DateTime.Now.Year.ToString();
            List<Assessment_Rules> rlollover = new List<Assessment_Rules>();
            string substring = currentYear.Substring(2, 2);
            FuncResponse<List<Assessment_Rules>> mObjFuncResponse = new FuncResponse<List<Assessment_Rules>>(); //Return Object
            using (_db = new EIRSEntities())
            {
                foreach (var rl in roll)
                {
                    var kk = rl.AssessmentRuleCode.ToString();
                    var k2 = kk.Substring(0, kk.Length - 2);
                    var k1 = k2 + substring;
                    Assessment_Rules rollover = new Assessment_Rules();
                    rollover.AssessmentRuleName = rl.AssessmentRuleName;
                    rollover.AssessmentRuleID = rl.AssessmentRuleID;
                    rollover.AssessmentRuleCode = k1;
                    rollover.CreatedDate = DateTime.Now;
                    rollover.PaymentFrequencyID = rl.Paymentfrequencyid;
                    rollover.ProfileID = rl.Profileid;
                    //rollover.a = rl.ARAIID;
                    rollover.AssessmentAmount = rl.AssessmentAmount;
                    rollover.TaxYear = Convert.ToInt32(currentYear);
                    rollover.PaymentOptionID = 1;
                    rollover.RuleRunID = rl.RuleRunId;
                    rollover.CreatedBy = -1;
                    rollover.Active = true;

                    _db.Assessment_Rules.Add(rollover);
                    rlollover.Add(rollover);
                }
                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.AdditionalData = rlollover;
                    mObjFuncResponse.Success = true;
                }
                catch
                {
                    mObjFuncResponse.Success = false;
                }
                return mObjFuncResponse;
            }
        }

        public IList<vw_AssessmentRule> REP_GetAssessmentRuleList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_AssessmentRule.ToList();
            }
        }

        public IList<usp_GetAssessmentRuleList_Result> REP_GetAssessmentRuleList(Assessment_Rules pObjAssessmentRule)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentRuleList(pObjAssessmentRule.AssessmentRuleID, pObjAssessmentRule.ProfileID, pObjAssessmentRule.IntStatus).ToList();
            }
        }

        public usp_GetAssessmentRuleList_Result REP_GetAssessmentRuleDetails(Assessment_Rules pObjAssessmentRule)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentRuleList(pObjAssessmentRule.AssessmentRuleID, pObjAssessmentRule.ProfileID, pObjAssessmentRule.IntStatus).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateStatus(Assessment_Rules pObjAssessmentRule)
        {
            using (_db = new EIRSEntities())
            {
                Assessment_Rules mObjInsertUpdateAssessmentRule; //Assessment_Rules Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Assessment_Rules
                if (pObjAssessmentRule.AssessmentRuleID != 0)
                {
                    mObjInsertUpdateAssessmentRule = (from arule in _db.Assessment_Rules
                                                      where arule.AssessmentRuleID == pObjAssessmentRule.AssessmentRuleID
                                                      select arule).FirstOrDefault();

                    if (mObjInsertUpdateAssessmentRule != null)
                    {
                        mObjInsertUpdateAssessmentRule.Active = !mObjInsertUpdateAssessmentRule.Active;
                        mObjInsertUpdateAssessmentRule.ModifiedBy = pObjAssessmentRule.ModifiedBy;
                        mObjInsertUpdateAssessmentRule.ModifiedDate = pObjAssessmentRule.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Assessment Rule Updated Successfully";
                            //  mObjFuncResponse.AdditionalData = _db.usp_GetAssessmentRuleList(0, pObjAssessmentRule.ProfileID, pObjAssessmentRule.IntStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Assessment Rule Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertSettlementMethod(MAP_AssessmentRule_SettlementMethod pObjSettlementMethod)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from bbf in _db.MAP_AssessmentRule_SettlementMethod
                               where bbf.SettlementMethodID == pObjSettlementMethod.SettlementMethodID && bbf.AssessmentRuleID == pObjSettlementMethod.AssessmentRuleID
                               select bbf);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Settlement Method Already Exists";
                }

                _db.MAP_AssessmentRule_SettlementMethod.Add(pObjSettlementMethod);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_RemoveSettlementMethod(MAP_AssessmentRule_SettlementMethod pObjSettlementMethod)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                MAP_AssessmentRule_SettlementMethod mObjDeleteSettlementMethod;

                mObjDeleteSettlementMethod = _db.MAP_AssessmentRule_SettlementMethod.Find(pObjSettlementMethod.ARSMID);

                if (mObjDeleteSettlementMethod == null)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Settlement Method Already Removed.";
                }
                else
                {
                    _db.MAP_AssessmentRule_SettlementMethod.Remove(mObjDeleteSettlementMethod);

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }
                }

                return mObjResponse;
            }
        }

        public IList<MAP_AssessmentRule_SettlementMethod> REP_GetSettlementMethod(int pIntAssessmentRuleID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.MAP_AssessmentRule_SettlementMethod.Where(t => t.AssessmentRuleID == pIntAssessmentRuleID).ToList();
            }
        }

        public FuncResponse REP_InsertAssessmentItem(MAP_AssessmentRule_AssessmentItem pObjAssessmentItem)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from bbf in _db.MAP_AssessmentRule_AssessmentItem
                               where bbf.AssessmentItemID == pObjAssessmentItem.AssessmentItemID && bbf.AssessmentRuleID == pObjAssessmentItem.AssessmentRuleID
                               select bbf);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Assessment Item Already Exists";
                }

                _db.MAP_AssessmentRule_AssessmentItem.Add(pObjAssessmentItem);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_RemoveAssessmentItem(MAP_AssessmentRule_AssessmentItem pObjAssessmentItem)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                MAP_AssessmentRule_AssessmentItem mObjDeleteAssessmentItem;

                mObjDeleteAssessmentItem = _db.MAP_AssessmentRule_AssessmentItem.Find(pObjAssessmentItem.ARAIID);

                if (mObjDeleteAssessmentItem == null)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Assessment Item Already Removed.";
                }
                else
                {
                    _db.MAP_AssessmentRule_AssessmentItem.Remove(mObjDeleteAssessmentItem);

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }
                }

                return mObjResponse;
            }
        }

        public IList<MAP_AssessmentRule_AssessmentItem> REP_GetAssessmentItem(int pIntAssessmentRuleID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.MAP_AssessmentRule_AssessmentItem.Where(t => t.AssessmentRuleID == pIntAssessmentRuleID).ToList();
            }
        }

        public IList<DropDownListResult> REP_GetSettlementMethodDropDownList(int pIntAssessmentRuleID)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from arsm in _db.MAP_AssessmentRule_SettlementMethod
                               join smth in _db.Settlement_Method on arsm.SettlementMethodID equals smth.SettlementMethodID
                               where arsm.AssessmentRuleID == pIntAssessmentRuleID
                               select new DropDownListResult()
                               {
                                   id = smth.SettlementMethodID,
                                   text = smth.SettlementMethodName
                               });

                return vResult.ToList();
            }
        }


        public IList<usp_GetMASPriceSheet_Result> REP_GetMASPriceSheet(int pIntRequestType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetMASPriceSheet(pIntRequestType).ToList();
            }
        }


        public IList<usp_SearchAssessmentRulesForRDMLoad_Result> REP_SearchAssessmentRuleDetails(Assessment_Rules pObjAssessmentRule)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchAssessmentRulesForRDMLoad(pObjAssessmentRule.AssessmentRuleCode, pObjAssessmentRule.ProfileReferenceNo, pObjAssessmentRule.AssessmentRuleName, pObjAssessmentRule.RuleRunName, pObjAssessmentRule.PaymentFrequencyName, pObjAssessmentRule.AssessmentItemName, pObjAssessmentRule.StrAssessmentAmount, pObjAssessmentRule.StrTaxYear, pObjAssessmentRule.SettlementMethodName, pObjAssessmentRule.PaymentOptionName, pObjAssessmentRule.ActiveText).ToList();
            }
        }

        public IDictionary<string, object> REP_SearchAssessmentRule(Assessment_Rules pObjAssessmentRule)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["AssessmentRuleList"] = _db.usp_SearchAssessmentRules(pObjAssessmentRule.WhereCondition, pObjAssessmentRule.OrderBy, pObjAssessmentRule.OrderByDirection, pObjAssessmentRule.PageNumber, pObjAssessmentRule.PageSize, pObjAssessmentRule.MainFilter,
                                                                        pObjAssessmentRule.AssessmentRuleCode, pObjAssessmentRule.ProfileReferenceNo, pObjAssessmentRule.AssessmentRuleName, pObjAssessmentRule.RuleRunName, pObjAssessmentRule.PaymentFrequencyName, pObjAssessmentRule.AssessmentItemName, pObjAssessmentRule.StrAssessmentAmount, pObjAssessmentRule.StrTaxYear, pObjAssessmentRule.SettlementMethodName, pObjAssessmentRule.PaymentOptionName, pObjAssessmentRule.ActiveText).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(AssessmentRuleID) FROM Assessment_Rules").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(AssessmentRuleID) ");
                sbFilteredCountQuery.Append(" FROM Assessment_Rules arule ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Profiles prf ");
                sbFilteredCountQuery.Append(" INNER JOIN Asset_Types atype ON prf.AssetTypeID = atype.AssetTypeID ");
                sbFilteredCountQuery.Append(" ON arule.ProfileID = prf.ProfileID ");
                sbFilteredCountQuery.Append(" INNER JOIN MST_RuleRun rr ON arule.RuleRunID = rr.RuleRunID ");
                sbFilteredCountQuery.Append(" INNER JOIN Payment_Frequency pfreq ON arule.PaymentFrequencyID = pfreq.PaymentFrequencyID ");
                sbFilteredCountQuery.Append(" INNER JOIN Payment_Options popt ON arule.PaymentOptionID = popt.PaymentOptionID  WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjAssessmentRule.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjAssessmentRule.MainFilter),
                    new SqlParameter("@AssessmentRuleCode",pObjAssessmentRule.AssessmentRuleCode),
                    new SqlParameter("@ProfileReferenceNo",pObjAssessmentRule.ProfileReferenceNo),
                    new SqlParameter("@AssessmentRuleName",pObjAssessmentRule.AssessmentRuleName),
                    new SqlParameter("@RuleRunName",pObjAssessmentRule.RuleRunName),
                    new SqlParameter("@PaymentFrequencyName",pObjAssessmentRule.PaymentFrequencyName),
                    new SqlParameter("@AssessmentItemNames",pObjAssessmentRule.AssessmentItemName),
                    new SqlParameter("@AssessmentAmount",pObjAssessmentRule.StrAssessmentAmount),
                    new SqlParameter("@TaxYear",pObjAssessmentRule.StrTaxYear),
                    new SqlParameter("@SettlementMethodNames",pObjAssessmentRule.SettlementMethodName),
                    new SqlParameter("@PaymentOptionName",pObjAssessmentRule.PaymentOptionName),
                    new SqlParameter("@ActiveText",pObjAssessmentRule.ActiveText)

                };

                //Get Filtered Count
                // int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntTotalCount;

                return dcData;
            }
        }

        public IDictionary<string, object> REP_SearchAssessmentRuleForSideMenu(Assessment_Rules pObjAssessmentRule)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["AssessmentRuleList"] = _db.usp_SearchAssessmentRuleForSideMenu(pObjAssessmentRule.WhereCondition, pObjAssessmentRule.OrderBy, pObjAssessmentRule.OrderByDirection, pObjAssessmentRule.PageNumber, pObjAssessmentRule.PageSize, pObjAssessmentRule.MainFilter).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(AssessmentRuleID) FROM Assessment_Rules arule INNER JOIN MST_RuleRun rr ON arule.RuleRunID = rr.RuleRunID INNER JOIN Payment_Frequency pfreq ON arule.PaymentFrequencyID = pfreq.PaymentFrequencyID").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(AssessmentRuleID) ");
                sbFilteredCountQuery.Append(" FROM Assessment_Rules arule ");
                sbFilteredCountQuery.Append(" INNER JOIN MST_RuleRun rr ON arule.RuleRunID = rr.RuleRunID ");
                sbFilteredCountQuery.Append(" INNER JOIN Payment_Frequency pfreq ON arule.PaymentFrequencyID = pfreq.PaymentFrequencyID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjAssessmentRule.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjAssessmentRule.MainFilter)
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }
    }
}
