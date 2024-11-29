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
        public IHttpActionResult TaxOffices()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                IList<usp_GetTaxOfficeList_Result> lstTaxOffices = new BLTaxOffice().BL_GetTaxOfficeList(new Tax_Offices() { intStatus = 1 });

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Message = "Success";
                mObjAPIResponse.Result = lstTaxOffices;
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
        public IHttpActionResult GetBusinessSectors()
        {
            var response = new APIResponse();

            try
            {
                using (var context = new EIRSEntities())
                {
                    var businessSectors = context.Business_Sector
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
                            CategoryName = sector.Business_Category != null ? sector.Business_Category.BusinessCategoryName : null, // Flatten Category
                            SubSectorNames = sector.Business_SubSector.Select(sub => sub.BusinessSubSectorName).ToList(), // Flatten SubSector collection
                            TypeName = sector.Business_Types != null ? sector.Business_Types.BusinessTypeName : null // Flatten Type
                        })
                        .ToList();

                    response.Success = true;
                    response.Message = "Success";
                    response.Result = businessSectors;
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
