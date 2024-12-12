using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace EIRS.API.Controllers.User
{
    [RoutePrefix("User")]

    public class UserController : BaseController
    {
        EIRSEntities _db;

        [HttpGet]
        [Route("TaxOffices")]
        public IHttpActionResult TaxOffices(int pageNumber = 1, int pageSize = 10)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                IList<usp_GetTaxOfficeList_Result> lstTaxOffices = new BLTaxOffice().BL_GetTaxOfficeList(new Tax_Offices() { intStatus = 1 });

                int totalRecords = lstTaxOffices.Count;

                var paginatedResult = lstTaxOffices
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var paginationMetadata = new
                {
                    TotalRecords = totalRecords,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                };

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Message = "Success";
                mObjAPIResponse.Result = new
                {
                    Data = paginatedResult,
                    Pagination = paginationMetadata
                };
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }


        [HttpGet]
        [Route("TaxOfficer")]
        public IHttpActionResult TaxOfficer(string EmailAddress)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                // IList<usp_GetUserList_Result> lstTaxOfficer = new BLUser().BL_GetUserList(new MST_Users() { intStatus = 2, UserTypeID = 2 });
                usp_GetUserList_Result lstTaxOfficer = new BLUser().BL_GetUserDetails(new MST_Users() { intStatus = 2, UserTypeID = 2, EmailAddress = EmailAddress });

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Message = "Success";
                mObjAPIResponse.Result = lstTaxOfficer;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LGAs")]
        public IHttpActionResult GetLGAs()
        {
            var response = new APIResponse();

            try
            {
                using (var dbContext = new EIRSEntities())
                {
                    // Option 1: Explicitly load the required properties
                    var localGovernmentAreas = dbContext.LGAs
                        .Select(lga => new  // Project to anonymous type with only needed properties
                        {
                            lga.LGAID,
                            lga.LGAName
                            // Add other specific properties you need
                        })
                        .ToList();

                    response.Success = true;
                    response.Message = "Success";
                    response.Result = localGovernmentAreas;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }


        [HttpGet]
        [Route("BusinessSectors")]
        public IHttpActionResult GetBusinessSectors(int pageNumber = 1, int pageSize = 10)
        {
            var response = new APIResponse();

            try
            {
                using (var context = new EIRSEntities())
                {
                    var totalRecords = context.Business_Sector.Count();

                    var businessSectors = context.Business_Sector
                        .OrderBy(sector => sector.BusinessSectorName)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .Select(sector => new
                        {
                            Id = sector.BusinessSectorID,
                            Name = sector.BusinessSectorName,
                            CategoryId = sector.BusinessCategoryID,
                            TypeId = sector.BusinessTypeID,
                            CreatedDate = sector.CreatedDate,
                            CreatedBy = sector.CreatedBy,
                            LastModifiedDate = sector.ModifiedDate,
                            LastModifiedBy = sector.ModifiedBy,
                            IsActive = sector.Active,
                            CategoryName = sector.Business_Category != null ? sector.Business_Category.BusinessCategoryName : null,
                            SubSectorNames = sector.Business_SubSector.Select(sub => sub.BusinessSubSectorName).ToList(),
                            TypeName = sector.Business_Types != null ? sector.Business_Types.BusinessTypeName : null
                        })
                        .ToList();

                    var paginationMetadata = new
                    {
                        TotalRecords = totalRecords,
                        PageSize = pageSize,
                        CurrentPage = pageNumber,
                        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize)
                    };

                    response.Success = true;
                    response.Message = "Success";
                    response.Result = new
                    {
                        Data = businessSectors,
                        Pagination = paginationMetadata
                    };
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }


        [HttpGet]
        [Route("GetAssessentRules")]
        public IHttpActionResult GetAssessentRules(int pageNumber = 1, int pageSize = 10)
        {
            var response = new APIResponse();

            try
            {
                using (var context = new EIRSEntities())
                {
                    var totalRecords = context.Assessment_Rules
                        .Count(ar => ar.ProfileID == 1277);

                    var GetAssessentRules = context.Assessment_Rules
                        .Where(ar => ar.ProfileID == 1277)
                        .OrderBy(ar => ar.TaxYear)
                        .ThenBy(ar => ar.TaxMonth)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .Select(AssRule => new
                        {
                            AssessmentRuleID = AssRule.AssessmentRuleID,
                            AssessmentRuleCode = AssRule.AssessmentRuleCode,
                            ProfileID = AssRule.ProfileID,
                            AssessmentRuleName = AssRule.AssessmentRuleName,
                            RuleRunID = AssRule.RuleRunID,
                            PaymentFrequencyID = AssRule.PaymentFrequencyID,
                            AssessmentAmount = AssRule.AssessmentAmount,
                            TaxYear = AssRule.TaxYear,
                            PaymentOptionID = AssRule.PaymentOptionID,
                            Active = AssRule.Active,
                            CreatedBy = AssRule.CreatedBy,
                            CreatedDate = AssRule.CreatedDate,
                            TaxMonth = AssRule.TaxMonth
                        })
                        .ToList();

                    var paginationMetadata = new
                    {
                        TotalRecords = totalRecords,
                        PageSize = pageSize,
                        CurrentPage = pageNumber,
                        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                    };

                    response.Success = true;
                    response.Message = "Success";
                    response.Result = new
                    {
                        Data = GetAssessentRules,
                        Pagination = paginationMetadata
                    };
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("GetAssessentItems")]
        public IHttpActionResult GetAssessentItems(int pageNumber = 1, int pageSize = 10, int RevId = 0)
        {
            var response = new APIResponse();

            try
            {
                using (var context = new EIRSEntities())
                {
                    var totalRecords = context.Assessment_Items
                        .Count(ar => ar.RevenueStreamID == RevId);

                    var GetAssessentItems = context.Assessment_Items
                        .Where(ar => ar.RevenueStreamID == 8)
                        .OrderBy(AssItem => AssItem.AssessmentItemID)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .Select(AssItem => new
                        {
                            AssessmentItemID = AssItem.AssessmentItemID,
                            AssessmentItemReferenceNo = AssItem.AssessmentItemReferenceNo,
                            AssetTypeID = AssItem.AssetTypeID,
                            AssessmentGroupID = AssItem.AssessmentGroupID,
                            AssessmentSubGroupID = AssItem.AssessmentSubGroupID,
                            RevenueStreamID = AssItem.RevenueStreamID,
                            RevenueSubStreamID = AssItem.RevenueSubStreamID,
                            AssessmentItemCategoryID = AssItem.AssessmentItemCategoryID,
                            AssessmentItemSubCategoryID = AssItem.AssessmentItemSubCategoryID,
                            AgencyID = AssItem.AgencyID,
                            AssessmentItemName = AssItem.AssessmentItemName,
                            ComputationID = AssItem.ComputationID,
                            TaxBaseAmount = AssItem.TaxBaseAmount,
                            Percentage = AssItem.Percentage,
                            TaxAmount = AssItem.TaxAmount,
                            Active = AssItem.Active,
                            CreatedBy = AssItem.CreatedBy,
                            CreatedDate = AssItem.CreatedDate,
                        })
                        .ToList();

                    var paginationMetadata = new
                    {
                        TotalRecords = totalRecords,
                        PageSize = pageSize,
                        CurrentPage = pageNumber,
                        TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                    };

                    response.Success = true;
                    response.Message = "Success";
                    response.Result = new
                    {
                        Data = GetAssessentItems,
                        Pagination = paginationMetadata
                    };
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }


    }
}
