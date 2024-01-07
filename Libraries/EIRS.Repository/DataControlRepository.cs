using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using EIRS.BOL;


namespace EIRS.Repository
{
    public class DataControlRepository : IDataControlRepository
    {
        EIRSEntities _db;

        public IList<usp_DC_GetTaxPayerWithoutAsset_Result> REP_GetTaxPayerWithoutAsset(string pStrTaxPayerTypeID, string pStrAssetTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_DC_GetTaxPayerWithoutAsset(pStrTaxPayerTypeID, pStrAssetTypeID).ToList();
            }
        }

        public IList<usp_DC_GetProfileWithoutRule_Result> REP_GetProfileWithoutRule()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_DC_GetProfileWithoutRule().ToList();
            }
        }

        public IList<usp_DC_GetAssessmentItemWithoutRule_Result> REP_GetAssessmentItemWithoutRule()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_DC_GetAssessmentItemWithoutRule().ToList();
            }
        }

        public IList<usp_DC_GetIndividualWithoutAssessment_Result> REP_GetIndividualWithoutAssessment()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_DC_GetIndividualWithoutAssessment().ToList();
            }
        }

        public IList<usp_DC_GetCompanyWithoutAssessment_Result> REP_GetCompanyWithoutAssessment()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_DC_GetCompanyWithoutAssessment().ToList();
            }
        }

        public IList<AssessmentAndItemRollOver> GetAssessmentAndItem()
        {
            using (_db = new EIRSEntities())
            {
                List<AssessmentAndItemRollOver> assessmentAndItems = new List<AssessmentAndItemRollOver>();
                var presentYear = DateTime.Now.Year;
                var retVal = (from r in _db.Assessment_Rules
                              join a in _db.MAP_AssessmentRule_AssessmentItem
                              on r.AssessmentRuleID equals a.AssessmentRuleID
                              join b in _db.Assessment_Items
                              on a.AssessmentItemID equals b.AssessmentItemID
                              where r.TaxYear == presentYear
                              select new
                              {
                                  active = r.Active,
                                  taxYear = r.TaxYear,
                                  taxMonth = r.TaxMonth,
                                  percentage = b.Percentage,
                                  taxBaseAmount = b.TaxBaseAmount,
                                  taxAmount = b.TaxAmount,
                                  assessmentItemName = b.AssessmentItemName,
                                  assessmentRuleCode = r.AssessmentRuleCode,
                                  assessmentRuleName = r.AssessmentRuleName,
                                  assessmentAmount = r.AssessmentAmount,
                              }).ToList();

                foreach (var item in retVal)
                {
                    AssessmentAndItemRollOver andItem = new AssessmentAndItemRollOver();

                    andItem.AssessmentAmount = item.assessmentAmount.Value;
                    andItem.TaxYear = item.taxYear.ToString();
                    andItem.TaxMonth = item.taxMonth.ToString();
                    andItem.Active = item.active.Value;
                    andItem.TaxAmount = item.taxAmount.Value;
                    andItem.Percentage = item.percentage.HasValue ? item.percentage.Value : 0;
                    andItem.TaxBaseAmount = item.taxBaseAmount.Value;
                    andItem.AssessmentItemName = item.assessmentItemName;
                    andItem.AssessmentRuleCode = item.assessmentRuleCode;
                    andItem.AssessmentRuleName = item.assessmentRuleName;
                    assessmentAndItems.Add(andItem);
                }

                return assessmentAndItems;
            }
        }
        public IList<AssessmentRuleRollover> GetAssessmentAndRule()
        {
            using (_db = new EIRSEntities())
            {
                List<AssessmentRuleRollover> assessmentAndItems = new List<AssessmentRuleRollover>();
                var presentYear = DateTime.Now.Year;
                var retVal = (from r in _db.Assessment_Rules
                              join a in _db.MAP_AssessmentRule_AssessmentItem
                              on r.AssessmentRuleID equals a.AssessmentRuleID
                              where r.TaxYear == presentYear
                              select new
                              {
                                  assessmentRuleID = r.AssessmentRuleID,
                                  assessmentRuleCode = r.AssessmentRuleCode,
                                  profileId = r.ProfileID,
                                  assessmentRuleName = r.AssessmentRuleName,
                                  assessmentAmount = r.AssessmentAmount,
                                  taxYear = r.TaxYear,
                                  ruleRunID = r.RuleRunID,
                                  paymentFrequencyId = r.PaymentFrequencyID,
                                  active = r.Active,
                                  araiid = a.ARAIID,
                                  assessmentitemID = a.AssessmentItemID
                              }).ToList();

                foreach (var item in retVal)
                {
                    AssessmentRuleRollover andItem = new AssessmentRuleRollover();
                    andItem.AssessmentRuleID = item.assessmentRuleID;
                    andItem.AssessmentRuleCode = item.assessmentRuleCode;
                    andItem.AssessmentRuleName = item.assessmentRuleName;
                    andItem.AssessmentAmount = item.assessmentAmount.Value;
                    andItem.ARAIID = item.araiid;
                    andItem.AssessmentItemID = item.assessmentitemID.Value;
                    andItem.Paymentfrequencyid = item.paymentFrequencyId.Value;
                    andItem.Taxyear = item.taxYear.Value;
                    andItem.Profileid = item.profileId.Value;
                    andItem.RuleRunId = item.ruleRunID.Value;
                    andItem.Active = item.active.ToString();
                    assessmentAndItems.Add(andItem);
                }

                return assessmentAndItems;
            }
        }

        public class AssessmentAndItemRollOver
        {
            public int Id { get; set; }
            public bool Active { get; set; }
            public string TaxYear { get; set; }
            public string TaxMonth { get; set; }
            public decimal TaxAmount { get; set; }
            public decimal Percentage { get; set; }
            public decimal TaxBaseAmount { get; set; }
            public string AssessmentItemName { get; set; }
            public string AssessmentRuleCode { get; set; }
            public string AssessmentRuleName { get; set; }
            public decimal AssessmentAmount { get; set; }
        }

        public class ReturnObject
        {
            public int Id { get; set; }
            public bool Status { get; set; }
        }
        public  class TempAssHolderII
        {
            public int Id { get; set; }
            public string AssessmentRuleCode { get; set; }
            public int? AssessmentRuleId { get; set; }
        }
        public class AssessmentRuleRollover
        {
            public int Id { get; set; }
            public int AssessmentRuleID { get; set; }
            public string AssessmentRuleCode { get; set; }
            public int Profileid { get; set; }
            public string AssessmentRuleName { get; set; }
            public decimal AssessmentAmount { get; set; }
            public int Taxyear { get; set; }
            public int RuleRunId { get; set; }
            public int Paymentfrequencyid { get; set; }
            public string Active { get; set; }
            public int Createdby { get; set; }
            public int ARAIID { get; set; }
            public int AssessmentItemID { get; set; }
            public DateTime Createddate { get; set; }
        }
    }
}
