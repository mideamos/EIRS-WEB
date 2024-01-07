using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    public class JTBController : BaseController
    {
        //
        public ActionResult IndividualList()
        {
            return View();
        }

        public JsonResult GetIndividualData()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                //Get parameters

                // get Start (paging start index) and length (page size for paging)
                var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
                var vStart = Request.Form.GetValues("start").FirstOrDefault();
                var vLength = Request.Form.GetValues("length").FirstOrDefault();
                //Get Sort columns value
                var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
                var vFilter = Request.Form.GetValues("search[value]")[0];
                int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
                int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
                int IntTotalRecords = 0;

                // Loading. 
                IList<JTB_Individual> lstIndividualData = new BLJTB().BL_GetIndividualList();

                // Total record count.   
                int totalRecords = lstIndividualData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(vFilter) && !string.IsNullOrWhiteSpace(vFilter))
                {
                    // Apply search   
                    lstIndividualData = lstIndividualData.Where(p =>
                        p.tin.ToString().ToLower().Contains(vFilter.ToLower()) ||
                        p.bvn.ToString().ToLower().Contains(vFilter.ToLower()) ||
                        (p.nin != null && p.nin.ToLower().Contains(vFilter.ToLower())) ||
                        (p.SBIRt_name != null && p.SBIRt_name.ToLower().Contains(vFilter.ToLower())) ||
                        (p.middle_name != null && p.middle_name.ToLower().Contains(vFilter.ToLower())) ||
                        (p.last_name != null && p.last_name.ToLower().Contains(vFilter.ToLower())) ||
                        (p.GenderName != null && p.GenderName.ToLower().Contains(vFilter.ToLower())) ||
                        (p.StateOfOrigin != null && p.StateOfOrigin.ToLower().Contains(vFilter.ToLower())) ||
                        (p.date_of_birth != null && p.date_of_birth.Value.ToString("dd-MMM-yyyy").ToLower().Contains(vFilter.ToLower())) ||
                        (p.MaritalStatus != null && p.MaritalStatus.ToLower().Contains(vFilter.ToLower())) ||
                        (p.Occupation != null && p.Occupation.ToLower().Contains(vFilter.ToLower())) ||
                        (p.nationality_name != null && p.nationality_name.ToLower().Contains(vFilter.ToLower())) ||
                        (p.phone_no_1 != null && p.phone_no_1.ToLower().Contains(vFilter.ToLower())) ||
                        (p.phone_no_2 != null && p.phone_no_2.ToLower().Contains(vFilter.ToLower())) ||
                        (p.email_address != null && p.email_address.ToLower().Contains(vFilter.ToLower())) ||
                        (p.date_of_registration != null && p.date_of_registration.Value.ToString("dd-MMM-yyyy").ToLower().Contains(vFilter.ToLower())) ||
                        (p.house_number != null && p.house_number.ToLower().Contains(vFilter.ToLower())) ||
                        (p.street_name != null && p.street_name.ToLower().Contains(vFilter.ToLower())) ||
                        (p.city != null && p.city.ToLower().Contains(vFilter.ToLower())) ||
                        (p.LGAName != null && p.LGAName.ToLower().Contains(vFilter.ToLower())) ||
                        (p.StateName != null && p.StateName.ToLower().Contains(vFilter.ToLower())) ||
                        (p.CountryName != null && p.CountryName.ToLower().Contains(vFilter.ToLower())) ||
                        (p.TaxAuthorityCode != null && p.TaxAuthorityCode.ToLower().Contains(vFilter.ToLower())) ||
                        (p.TaxAuthorityName != null && p.TaxAuthorityName.ToLower().Contains(vFilter.ToLower())) ||
                        (p.TaxpayerStatus != null && p.TaxpayerStatus.ToLower().Contains(vFilter.ToLower()))

                        ).ToList();
                }

                //Purpose Sorting Data 
                if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
                {
                    lstIndividualData = lstIndividualData.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
                }

                IntTotalRecords = lstIndividualData.Count();
                var data = lstIndividualData.Skip(IntSkip).Take(IntPageSize).ToList();
                return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJTBIndividualDetails(int IndividualID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            JTB_Individual mObjIndividualData = new BLJTB().BL_GetIndividualDetails(IndividualID);

            if (mObjIndividualData != null)
            {
                dcResponse["success"] = true;
                dcResponse["TaxPayerDetails"] = mObjIndividualData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "No Record Found";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }


        public ActionResult IndividualETLData(int id)
        {
            JTB_Individual mObjJTBIndividualData = new BLJTB().BL_GetIndividualDetails(id);

            //Search Individual Based On Mobile Number

            string strPhoneNumber = mObjJTBIndividualData.phone_no_1.Trim();

            strPhoneNumber = strPhoneNumber.Length > 10 ? strPhoneNumber.Substring(strPhoneNumber.Length - 10, 10) : strPhoneNumber;

            usp_GetIndividualList_Result mObjERASIndividual = new BLIndividual().BL_GetIndividualDetails(new Individual() { MobileNumber1 = strPhoneNumber, intStatus = 2 });

            ViewBag.IndividualData = mObjERASIndividual;

            return View(mObjJTBIndividualData);
        }

        //
        public ActionResult NonIndividualList()
        {
            return View();
        }

        
        public JsonResult GetNonIndividualData()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                //Get parameters

                // get Start (paging start index) and length (page size for paging)
                var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
                var vStart = Request.Form.GetValues("start").FirstOrDefault();
                var vLength = Request.Form.GetValues("length").FirstOrDefault();
                //Get Sort columns value
                var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                var vSortColumnDir = Request.Form.GetValues("order[0][dir]")[0];
                var vFilter = Request.Form.GetValues("search[value]")[0];
                int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
                int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
                int IntTotalRecords = 0;

                // Loading. 
                IList<JTB_NonIndividual> lstNonIndividualData = new BLJTB().BL_GetNonIndividualList();

                // Total record count.   
                int totalRecords = lstNonIndividualData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(vFilter) && !string.IsNullOrWhiteSpace(vFilter))
                {
                    // Apply search   
                    lstNonIndividualData = lstNonIndividualData.Where(p =>
                        p.tin.ToString().ToLower().Contains(vFilter.ToLower()) ||
                        p.registered_name.ToString().ToLower().Contains(vFilter.ToLower()) ||
                        (p.main_trade_name != null && p.main_trade_name.ToLower().Contains(vFilter.ToLower())) ||
                        (p.org_name != null && p.org_name.ToLower().Contains(vFilter.ToLower())) ||
                        (p.registration_number != null && p.registration_number.ToLower().Contains(vFilter.ToLower())) ||
                        (p.phone_no_1 != null && p.phone_no_1.ToLower().Contains(vFilter.ToLower())) ||
                        (p.phone_no_2 != null && p.phone_no_2.ToLower().Contains(vFilter.ToLower())) ||
                        (p.email_address != null && p.email_address.ToLower().Contains(vFilter.ToLower())) ||
                        (p.line_of_business_code != null && p.line_of_business_code.ToLower().Contains(vFilter.ToLower())) ||
                       (p.lob_name != null && p.lob_name.ToLower().Contains(vFilter.ToLower())) ||
                        (p.date_of_registration != null && p.date_of_registration.Value.ToString("dd-MMM-yyyy").ToLower().Contains(vFilter.ToLower())) ||
                        (p.commencement_date != null && p.date_of_registration.Value.ToString("dd-MMM-yyyy").ToLower().Contains(vFilter.ToLower())) ||
                        (p.date_of_incorporation != null && p.date_of_registration.Value.ToString("dd-MMM-yyyy").ToLower().Contains(vFilter.ToLower())) ||
                        (p.house_number != null && p.house_number.ToLower().Contains(vFilter.ToLower())) ||
                        (p.street_name != null && p.street_name.ToLower().Contains(vFilter.ToLower())) ||
                        (p.city != null && p.city.ToLower().Contains(vFilter.ToLower())) ||
                        (p.LGAName != null && p.LGAName.ToLower().Contains(vFilter.ToLower())) ||
                        (p.StateName != null && p.StateName.ToLower().Contains(vFilter.ToLower())) ||
                        (p.CountryName != null && p.CountryName.ToLower().Contains(vFilter.ToLower())) ||
                       (p.FinYrBegin != null && p.FinYrBegin.ToLower().Contains(vFilter.ToLower())) ||
                        (p.FinYrEnd != null && p.FinYrEnd.ToLower().Contains(vFilter.ToLower())) ||
                        (p.director_name != null && p.director_name.ToLower().Contains(vFilter.ToLower())) ||
                        (p.director_phone != null && p.director_phone.ToLower().Contains(vFilter.ToLower())) ||
                        (p.director_email != null && p.director_email.ToLower().Contains(vFilter.ToLower())) ||
                        (p.TaxAuthorityCode != null && p.TaxAuthorityCode.ToLower().Contains(vFilter.ToLower())) ||
                        (p.TaxAuthorityName != null && p.TaxAuthorityName.ToLower().Contains(vFilter.ToLower())) ||
                        (p.TaxpayerStatus != null && p.TaxpayerStatus.ToLower().Contains(vFilter.ToLower()))

                        ).ToList();
                }

                //Purpose Sorting Data 
                if (!string.IsNullOrEmpty(vSortColumn) && !string.IsNullOrEmpty(vSortColumnDir))
                {
                    lstNonIndividualData = lstNonIndividualData.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
                }

                IntTotalRecords = lstNonIndividualData.Count();
                var data = lstNonIndividualData.Skip(IntSkip).Take(IntPageSize).ToList();
                return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJTBNonIndividualDetails(int IndividualID)
        {
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            JTB_NonIndividual mObjNonIndividualData = new BLJTB().BL_GetNonIndividualDetails(IndividualID);

            if (mObjNonIndividualData != null)
            {
                dcResponse["success"] = true;
                dcResponse["TaxPayerDetails"] = mObjNonIndividualData;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "No Record Found";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
    }
}