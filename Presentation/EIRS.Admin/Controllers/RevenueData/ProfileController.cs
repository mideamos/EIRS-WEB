using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using System.Linq;
using System.Transactions;
using Elmah;
using System.Linq.Dynamic;

namespace EIRS.Admin.Controllers
{
    public class ProfileController : BaseController
    {
        // GET: Profile
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadData()
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var vFilter = Request.Form.GetValues("search[value]").FirstOrDefault();
            int IntPageSize = vLength != null ? Convert.ToInt32(vLength) : 0;
            int IntSkip = vStart != null ? Convert.ToInt32(vStart) : 0;
            int IntTotalRecords = 0;

            Profile mObjProfile = new Profile()
            {
                ProfileReferenceNo = Request.Form["ProfileReferenceNo"],
                AssetTypeName = Request.Form["AssetTypeName"],
                ProfileSectorName = Request.Form["ProfileSectorName"],
                ProfileSubSectorName = Request.Form["ProfileSubSectorName"],
                ProfileGroupName = Request.Form["ProfileGroupName"],
                ProfileSubGroupName = Request.Form["ProfileSubGroupName"],
                ProfileSectorElementName = Request.Form["ProfileSectorElementName"],
                ProfileSectorSubElementName = Request.Form["ProfileSectorSubElementName"],
                ProfileAttributeName = Request.Form["ProfileAttributeName"],
                ProfileSubAttributeName = Request.Form["ProfileSubAttributeName"],
                TaxPayerTypeName = Request.Form["TaxPayerTypeName"],
                TaxPayerRoleName = Request.Form["TaxPayerRoleName"],
                ProfileDescription = Request.Form["ProfileDescription"],
                ActiveText = Request.Form["ActiveText"],
                IntStatus = 2,
            };

            IList<usp_SearchProfileForRDMLoad_Result> lstProfile = new BLProfile().BL_SearchProfileDetails(mObjProfile);

            if (!string.IsNullOrEmpty(vFilter))
            {
                lstProfile = lstProfile.Where(t => t.ProfileReferenceNo != null && t.ProfileReferenceNo.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.AssetTypeName != null && t.AssetTypeName.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.ProfileSectorNames != null && t.ProfileSectorNames.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.ProfileSubSectorNames != null && t.ProfileSubSectorNames.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.ProfileGroupNames != null && t.ProfileGroupNames.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.ProfileSubGroupNames != null && t.ProfileSubGroupNames.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.ProfileSectorElementNames != null && t.ProfileSectorElementNames.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.ProfileSectorSubElementNames != null && t.ProfileSectorSubElementNames.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.ProfileAttributeNames != null && t.ProfileAttributeNames.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.ProfileSubAttributeNames != null && t.ProfileSubAttributeNames.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TaxPayerTypeNames != null && t.TaxPayerTypeNames.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.TaxPayerRoleNames != null && t.TaxPayerRoleNames.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.ProfileDescription != null && t.ProfileDescription.ToLower().Trim().Contains(vFilter.ToLower().Trim())
                || t.ActiveText.ToLower().Trim().Contains(vFilter.ToLower().Trim())).ToList();
            }


            //>>>Purpose Sorting Data 
            if (!(string.IsNullOrEmpty(vSortColumn) && string.IsNullOrEmpty(vSortColumnDir)))
            {
                lstProfile = lstProfile.OrderBy(vSortColumn + " " + vSortColumnDir).ToList();
            }

            IntTotalRecords = lstProfile.Count();
            var data = lstProfile.Skip(IntSkip).Take(IntPageSize).ToList();
            return Json(new { draw = vDraw, recordsFiltered = IntTotalRecords, recordsTotal = IntTotalRecords, data = data }, JsonRequestBehavior.AllowGet);
        }


        public void UI_FillDropDown(ProfileViewModel pObjProfileModel = null)
        {
            if (pObjProfileModel == null)
                pObjProfileModel = new ProfileViewModel();

            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjProfileModel.AssetTypeID.ToString() });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1 });
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { intStatus = 1, AssetTypeID = pObjProfileModel.AssetTypeID });

            IList<SelectListItem> lstAssetStatus = new List<SelectListItem>();
            lstAssetStatus.Add(new SelectListItem() { Value = "1", Text = "Active" });
            lstAssetStatus.Add(new SelectListItem() { Value = "0", Text = "InActive" });

            ViewBag.AssetStatusList = lstAssetStatus;

            if (pObjProfileModel.AssetTypeID == (int)EnumList.AssetTypes.Building)
            {
                IList<DropDownListResult> lstProfileSector = new List<DropDownListResult>();
                lstProfileSector.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSector.AddRange(new BLBuildingType().BL_GetBuildingTypeDropDownList(new Building_Types() { intStatus = 1 }));
                ViewBag.ProfileSectorList = new SelectList(lstProfileSector, "id", "text");

                IList<DropDownListResult> lstProfileSubSector = new List<DropDownListResult>();
                lstProfileSubSector.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSubSector.AddRange(new BLLGA().BL_GetLGADropDownList(new LGA() { intStatus = 1 }));
                ViewBag.ProfileSubSectorList = new SelectList(lstProfileSubSector, "id", "text");

                IList<DropDownListResult> lstProfileGroup = new List<DropDownListResult>();
                lstProfileGroup.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileGroup.AddRange(new BLBuildingCompletion().BL_GetBuildingCompletionDropDownList(new Building_Completion() { intStatus = 1 }));
                ViewBag.ProfileGroupList = new SelectList(lstProfileGroup, "id", "text");

                IList<DropDownListResult> lstProfileSubGroup = new List<DropDownListResult>();
                lstProfileSubGroup.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSubGroup.AddRange(new BLBuildingPurpose().BL_GetBuildingPurposeDropDownList(new Building_Purpose() { intStatus = 1 }));
                ViewBag.ProfileSubGroupList = new SelectList(lstProfileSubGroup, "id", "text");

                IList<DropDownListResult> lstProfileSectorElement = new List<DropDownListResult>();
                lstProfileSectorElement.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSectorElement.AddRange(new BLUnitPurpose().BL_GetUnitPurposeDropDownList(new Unit_Purpose() { intStatus = 1 }));
                ViewBag.ProfileSectorElementList = new SelectList(lstProfileSectorElement, "id", "text");

                IList<DropDownListResult> lstProfileSectorSubSectorElement = new List<DropDownListResult>();
                lstProfileSectorSubSectorElement.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSectorSubSectorElement.AddRange(new BLBuildingOwnership().BL_GetBuildingOwnershipDropDownList(new Building_Ownership() { intStatus = 1 }));
                ViewBag.ProfileSectorSubElementList = new SelectList(lstProfileSectorSubSectorElement, "id", "text");

                IList<DropDownListResult> lstProfileAttribute = new List<DropDownListResult>();
                lstProfileAttribute.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileAttribute.AddRange(new BLUnitFunction().BL_GetUnitFunctionDropDownList(new Unit_Function() { intStatus = 1 }));
                ViewBag.ProfileAttributeList = new SelectList(lstProfileAttribute, "id", "text");

                IList<DropDownListResult> lstProfileSubAttribute = new List<DropDownListResult>();
                lstProfileSubAttribute.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSubAttribute.AddRange(new BLUnitOccupancy().BL_GetUnitOccupancyDropDownList(new Unit_Occupancy() { intStatus = 1 }));
                ViewBag.ProfileSubAttributeList = new SelectList(lstProfileSubAttribute, "id", "text");
            }
            else if (pObjProfileModel.AssetTypeID == (int)EnumList.AssetTypes.Vehicles)
            {
                IList<DropDownListResult> lstProfileSector = new List<DropDownListResult>();
                lstProfileSector.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSector.AddRange(new BLVehicleType().BL_GetVehicleTypeDropDownList(new Vehicle_Types() { intStatus = 1 }));
                ViewBag.ProfileSectorList = new SelectList(lstProfileSector, "id", "text");

                IList<DropDownListResult> lstProfileSubSector = new List<DropDownListResult>();
                lstProfileSubSector.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSubSector.AddRange(new BLLGA().BL_GetLGADropDownList(new LGA() { intStatus = 1 }));
                ViewBag.ProfileSubSectorList = new SelectList(lstProfileSubSector, "id", "text");

                IList<DropDownListResult> lstProfileGroup = new List<DropDownListResult>();
                lstProfileGroup.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileGroup.AddRange(new BLVehiclePurpose().BL_GetVehiclePurposeDropDownList(new Vehicle_Purpose() { intStatus = 1 }));
                ViewBag.ProfileGroupList = new SelectList(lstProfileGroup, "id", "text");

                IList<DropDownListResult> lstProfileSubGroup = new List<DropDownListResult>();
                lstProfileSubGroup.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSubGroup.AddRange(new BLVehicleFunction().BL_GetVehicleFunctionDropDownList(new Vehicle_Function() { intStatus = 1 }));
                ViewBag.ProfileSubGroupList = new SelectList(lstProfileSubGroup, "id", "text");

                IList<DropDownListResult> lstProfileSectorElement = new List<DropDownListResult>();
                lstProfileSectorElement.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSectorElement.AddRange(new BLVehicleOwnership().BL_GetVehicleOwnershipDropDownList(new Vehicle_Ownership() { intStatus = 1 }));
                ViewBag.ProfileSectorElementList = new SelectList(lstProfileSectorElement, "id", "text");

                IList<DropDownListResult> lstProfileSectorSubSectorElement = new List<DropDownListResult>
                {
                    new DropDownListResult() { id = -1, text = "All" }
                };
                lstProfileSectorSubSectorElement.AddRange(new BLVehicleSubType().BL_GetVehicleSubTypeDropDownList(new Vehicle_SubTypes() { intStatus = 1 }));
                ViewBag.ProfileSectorSubElementList = new SelectList(lstProfileSectorSubSectorElement, "id", "text");

                IList<DropDownListResult> lstProfileAttribute = new List<DropDownListResult>();
                lstProfileAttribute.Add(new DropDownListResult() { id = -1, text = "All" });
                ViewBag.ProfileAttributeList = new SelectList(lstProfileAttribute, "id", "text");

                IList<DropDownListResult> lstProfileSubAttribute = new List<DropDownListResult>();
                lstProfileSubAttribute.Add(new DropDownListResult() { id = -1, text = "All" });
                ViewBag.ProfileSubAttributeList = new SelectList(lstProfileSubAttribute, "id", "text");

            }
            else if (pObjProfileModel.AssetTypeID == (int)EnumList.AssetTypes.Business)
            {
                IList<DropDownListResult> lstProfileSector = new List<DropDownListResult>();
                lstProfileSector.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSector.AddRange(new BLBusinessType().BL_GetBusinessTypeDropDownList(new Business_Types() { intStatus = 1 }));
                ViewBag.ProfileSectorList = new SelectList(lstProfileSector, "id", "text");

                IList<DropDownListResult> lstProfileSubSector = new List<DropDownListResult>();
                lstProfileSubSector.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSubSector.AddRange(new BLLGA().BL_GetLGADropDownList(new LGA() { intStatus = 1 }));
                ViewBag.ProfileSubSectorList = new SelectList(lstProfileSubSector, "id", "text");

                IList<DropDownListResult> lstProfileGroup = new List<DropDownListResult>();
                lstProfileGroup.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileGroup.AddRange(new BLBusinessCategory().BL_GetBusinessCategoryDropDownList(new Business_Category() { intStatus = 1 }));
                ViewBag.ProfileGroupList = new SelectList(lstProfileGroup, "id", "text");

                IList<DropDownListResult> lstProfileSubGroup = new List<DropDownListResult>();
                lstProfileSubGroup.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSubGroup.AddRange(new BLBusinessSector().BL_GetBusinessSectorDropDownList(new Business_Sector() { intStatus = 1 }));
                ViewBag.ProfileSubGroupList = new SelectList(lstProfileSubGroup, "id", "text");

                IList<DropDownListResult> lstProfileSectorElement = new List<DropDownListResult>();
                lstProfileSectorElement.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSectorElement.AddRange(new BLBusinessSubSector().BL_GetBusinessSubSectorDropDownList(new Business_SubSector() { intStatus = 1 }));
                ViewBag.ProfileSectorElementList = new SelectList(lstProfileSectorElement, "id", "text");

                IList<DropDownListResult> lstProfileSectorSubSectorElement = new List<DropDownListResult>();
                lstProfileSectorSubSectorElement.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSectorSubSectorElement.AddRange(new BLBusinessStructure().BL_GetBusinessStructureDropDownList(new Business_Structure() { intStatus = 1 }));
                ViewBag.ProfileSectorSubElementList = new SelectList(lstProfileSectorSubSectorElement, "id", "text");

                IList<DropDownListResult> lstProfileAttribute = new List<DropDownListResult>();
                lstProfileAttribute.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileAttribute.AddRange(new BLBusinessOperation().BL_GetBusinessOperationDropDownList(new Business_Operation() { intStatus = 1 }));
                ViewBag.ProfileAttributeList = new SelectList(lstProfileAttribute, "id", "text");

                IList<DropDownListResult> lstProfileSubAttribute = new List<DropDownListResult>();
                lstProfileSubAttribute.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSubAttribute.AddRange(new BLSize().BL_GetSizeDropDownList(new Size() { intStatus = 1 }));
                ViewBag.ProfileSubAttributeList = new SelectList(lstProfileSubAttribute, "id", "text");
            }
            else if (pObjProfileModel.AssetTypeID == (int)EnumList.AssetTypes.Land)
            {
                IList<DropDownListResult> lstProfileSector = new List<DropDownListResult>();
                lstProfileSector.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSector.AddRange(new BLLGA().BL_GetLGADropDownList(new LGA() { intStatus = 1 }));
                ViewBag.ProfileSectorList = new SelectList(lstProfileSector, "id", "text");

                IList<DropDownListResult> lstProfileSubSector = new List<DropDownListResult>();
                lstProfileSubSector.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSubSector.AddRange(new BLLandPurpose().BL_GetLandPurposeDropDownList(new Land_Purpose() { intStatus = 1 }));
                ViewBag.ProfileSubSectorList = new SelectList(lstProfileSubSector, "id", "text");

                IList<DropDownListResult> lstProfileGroup = new List<DropDownListResult>();
                lstProfileGroup.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileGroup.AddRange(new BLLandOwnership().BL_GetLandOwnershipDropDownList(new Land_Ownership() { intStatus = 1 }));
                ViewBag.ProfileGroupList = new SelectList(lstProfileGroup, "id", "text");

                IList<DropDownListResult> lstProfileSubGroup = new List<DropDownListResult>();
                lstProfileSubGroup.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSubGroup.AddRange(new BLLandDevelopment().BL_GetLandDevelopmentDropDownList(new Land_Development() { intStatus = 1 }));
                ViewBag.ProfileSubGroupList = new SelectList(lstProfileSubGroup, "id", "text");

                IList<DropDownListResult> lstProfileSectorElement = new List<DropDownListResult>();
                lstProfileSectorElement.Add(new DropDownListResult() { id = -1, text = "All" });
                lstProfileSectorElement.AddRange(new BLLandFunction().BL_GetLandFunctionDropDownList(new Land_Function() { intStatus = 1 }));
                ViewBag.ProfileSectorElementList = new SelectList(lstProfileSectorElement, "id", "text");

                IList<DropDownListResult> lstProfileSectorSubSectorElement = new List<DropDownListResult>();
                lstProfileSectorSubSectorElement.Add(new DropDownListResult() { id = -1, text = "All" });
                ViewBag.ProfileSectorSubElementList = new SelectList(lstProfileSectorSubSectorElement, "id", "text");

                IList<DropDownListResult> lstProfileAttribute = new List<DropDownListResult>();
                lstProfileAttribute.Add(new DropDownListResult() { id = -1, text = "All" });
                ViewBag.ProfileAttributeList = new SelectList(lstProfileAttribute, "id", "text");

                IList<DropDownListResult> lstProfileSubAttribute = new List<DropDownListResult>();
                lstProfileSubAttribute.Add(new DropDownListResult() { id = -1, text = "All" });
                ViewBag.ProfileSubAttributeList = new SelectList(lstProfileSubAttribute, "id", "text");
            }
            else
            {
                IList<DropDownListResult> lstProfileSector = new List<DropDownListResult>();
                lstProfileSector.Add(new DropDownListResult() { id = -1, text = "All" });
                ViewBag.ProfileSectorList = new SelectList(lstProfileSector, "id", "text");

                IList<DropDownListResult> lstProfileSubSector = new List<DropDownListResult>();
                lstProfileSubSector.Add(new DropDownListResult() { id = -1, text = "All" });
                ViewBag.ProfileSubSectorList = new SelectList(lstProfileSubSector, "id", "text");

                IList<DropDownListResult> lstProfileGroup = new List<DropDownListResult>();
                lstProfileGroup.Add(new DropDownListResult() { id = -1, text = "All" });
                ViewBag.ProfileGroupList = new SelectList(lstProfileGroup, "id", "text");

                IList<DropDownListResult> lstProfileSubGroup = new List<DropDownListResult>();
                lstProfileSubGroup.Add(new DropDownListResult() { id = -1, text = "All" });
                ViewBag.ProfileSubGroupList = new SelectList(lstProfileSubGroup, "id", "text");

                IList<DropDownListResult> lstProfileSectorElement = new List<DropDownListResult>();
                lstProfileSectorElement.Add(new DropDownListResult() { id = -1, text = "All" });
                ViewBag.ProfileSectorElementList = new SelectList(lstProfileSectorElement, "id", "text");

                IList<DropDownListResult> lstProfileSectorSubSectorElement = new List<DropDownListResult>();
                lstProfileSectorSubSectorElement.Add(new DropDownListResult() { id = -1, text = "All" });
                ViewBag.ProfileSectorSubElementList = new SelectList(lstProfileSectorSubSectorElement, "id", "text");

                IList<DropDownListResult> lstProfileAttribute = new List<DropDownListResult>();
                lstProfileAttribute.Add(new DropDownListResult() { id = -1, text = "All" });
                ViewBag.ProfileAttributeList = new SelectList(lstProfileAttribute, "id", "text");

                IList<DropDownListResult> lstProfileSubAttribute = new List<DropDownListResult>();
                lstProfileSubAttribute.Add(new DropDownListResult() { id = -1, text = "All" });
                ViewBag.ProfileSubAttributeList = new SelectList(lstProfileSubAttribute, "id", "text");
            }
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(ProfileViewModel pObjProfileModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjProfileModel);
                return View(pObjProfileModel);
            }
            else
            {
                using (TransactionScope mObjTransactionScope = new TransactionScope())
                {
                    BLProfile mObjBLProfile = new BLProfile();

                    Profile mObjProfile = new Profile()
                    {
                        ProfileID = 0,
                        AssetTypeID = pObjProfileModel.AssetTypeID,
                        AssetTypeStatus = pObjProfileModel.AssetTypeStatus,
                        ProfileDescription = pObjProfileModel.ProfileDescription,
                        Active = true,
                        CreatedBy = SessionManager.SystemUserID,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    try
                    {
                        FuncResponse<Profile> mObjResponse = mObjBLProfile.BL_InsertUpdateProfile(mObjProfile);

                        if (mObjResponse.Success)
                        {
                            if (pObjProfileModel.ProfileSectorIds != null && pObjProfileModel.ProfileSectorIds.Length > 0)
                            {
                                foreach (int intSectorID in pObjProfileModel.ProfileSectorIds)
                                {
                                    ProfileSector mObjProfileSector = new ProfileSector()
                                    {
                                        SectorID = intSectorID,
                                        ProfileID = mObjResponse.AdditionalData.ProfileID,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = DateTime.Now
                                    };

                                    mObjBLProfile.BL_InsertProfileSector(mObjProfileSector);
                                }
                            }

                            if (pObjProfileModel.ProfileSubSectorIds != null && pObjProfileModel.ProfileSubSectorIds.Length > 0)
                            {
                                foreach (int intSubSectorID in pObjProfileModel.ProfileSubSectorIds)
                                {
                                    ProfileSubSector mObjProfileSubSector = new ProfileSubSector()
                                    {
                                        SubSectorID = intSubSectorID,
                                        ProfileID = mObjResponse.AdditionalData.ProfileID,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = DateTime.Now
                                    };

                                    mObjBLProfile.BL_InsertProfileSubSector(mObjProfileSubSector);
                                }
                            }

                            if (pObjProfileModel.ProfileGroupIds != null && pObjProfileModel.ProfileGroupIds.Length > 0)
                            {
                                foreach (int intGroupID in pObjProfileModel.ProfileGroupIds)
                                {
                                    ProfileGroup mObjProfileGroup = new ProfileGroup()
                                    {
                                        GroupID = intGroupID,
                                        ProfileID = mObjResponse.AdditionalData.ProfileID,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = DateTime.Now
                                    };

                                    mObjBLProfile.BL_InsertProfileGroup(mObjProfileGroup);
                                }
                            }

                            if (pObjProfileModel.ProfileSubGroupIds != null && pObjProfileModel.ProfileSubGroupIds.Length > 0)
                            {
                                foreach (int intSubGroupID in pObjProfileModel.ProfileSubGroupIds)
                                {
                                    ProfileSubGroup mObjProfileSubGroup = new ProfileSubGroup()
                                    {
                                        SubGroupID = intSubGroupID,
                                        ProfileID = mObjResponse.AdditionalData.ProfileID,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = DateTime.Now
                                    };

                                    mObjBLProfile.BL_InsertProfileSubGroup(mObjProfileSubGroup);
                                }
                            }

                            if (pObjProfileModel.ProfileSectorElementIds != null && pObjProfileModel.ProfileSectorElementIds.Length > 0)
                            {
                                foreach (int intSectorElementID in pObjProfileModel.ProfileSectorElementIds)
                                {
                                    ProfileSectorElement mObjProfileSectorElement = new ProfileSectorElement()
                                    {
                                        SectorElementID = intSectorElementID,
                                        ProfileID = mObjResponse.AdditionalData.ProfileID,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = DateTime.Now
                                    };

                                    mObjBLProfile.BL_InsertProfileSectorElement(mObjProfileSectorElement);
                                }
                            }

                            if (pObjProfileModel.ProfileSectorSubElementIds != null && pObjProfileModel.ProfileSectorSubElementIds.Length > 0)
                            {
                                foreach (int intSectorSubElementID in pObjProfileModel.ProfileSectorSubElementIds)
                                {
                                    ProfileSectorSubElement mObjProfileSectorSubElement = new ProfileSectorSubElement()
                                    {
                                        SectorSubElementID = intSectorSubElementID,
                                        ProfileID = mObjResponse.AdditionalData.ProfileID,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = DateTime.Now
                                    };

                                    mObjBLProfile.BL_InsertProfileSectorSubElement(mObjProfileSectorSubElement);
                                }
                            }

                            if (pObjProfileModel.ProfileAttributeIds != null && pObjProfileModel.ProfileAttributeIds.Length > 0)
                            {
                                foreach (int intAttributeID in pObjProfileModel.ProfileAttributeIds)
                                {
                                    ProfileAttribute mObjProfileAttribute = new ProfileAttribute()
                                    {
                                        AttributeID = intAttributeID,
                                        ProfileID = mObjResponse.AdditionalData.ProfileID,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = DateTime.Now
                                    };

                                    mObjBLProfile.BL_InsertProfileAttribute(mObjProfileAttribute);
                                }
                            }

                            if (pObjProfileModel.ProfileSubAttributeIds != null && pObjProfileModel.ProfileSubAttributeIds.Length > 0)
                            {
                                foreach (int intSubAttributeID in pObjProfileModel.ProfileSubAttributeIds)
                                {
                                    ProfileSubAttribute mObjProfileSubAttribute = new ProfileSubAttribute()
                                    {
                                        SubAttributeID = intSubAttributeID,
                                        ProfileID = mObjResponse.AdditionalData.ProfileID,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = DateTime.Now
                                    };

                                    mObjBLProfile.BL_InsertProfileSubAttribute(mObjProfileSubAttribute);
                                }
                            }

                            if (pObjProfileModel.TaxPayerTypeIds != null && pObjProfileModel.TaxPayerTypeIds.Length > 0)
                            {
                                foreach (int intTaxPayerTypeID in pObjProfileModel.TaxPayerTypeIds)
                                {
                                    ProfileTaxPayerType mObjProfileTaxPayerType = new ProfileTaxPayerType()
                                    {
                                        TaxPayerTypeID = intTaxPayerTypeID,
                                        ProfileID = mObjResponse.AdditionalData.ProfileID,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = DateTime.Now
                                    };

                                    mObjBLProfile.BL_InsertProfileTaxPayerType(mObjProfileTaxPayerType);
                                }
                            }

                            if (pObjProfileModel.TaxPayerRoleIds != null && pObjProfileModel.TaxPayerRoleIds.Length > 0)
                            {
                                foreach (int intTaxPayerRoleID in pObjProfileModel.TaxPayerRoleIds)
                                {
                                    ProfileTaxPayerRole mObjProfileTaxPayerRole = new ProfileTaxPayerRole()
                                    {
                                        TaxPayerRoleID = intTaxPayerRoleID,
                                        ProfileID = mObjResponse.AdditionalData.ProfileID,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = DateTime.Now
                                    };

                                    mObjBLProfile.BL_InsertProfileTaxPayerRole(mObjProfileTaxPayerRole);
                                }
                            }


                            mObjTransactionScope.Complete();
                            FlashMessage.Info(mObjResponse.Message);
                            return RedirectToAction("List", "Profile");
                        }
                        else
                        {
                            UI_FillDropDown(pObjProfileModel);
                            Transaction.Current.Rollback();
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjProfileModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillDropDown(pObjProfileModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving profile";
                        return View(pObjProfileModel);
                    }
                }
            }
        }


        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Profile mObjProfile = new Profile()
                {
                    ProfileID = id.GetValueOrDefault(),
                    IntStatus = 2
                };

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(mObjProfile);

                if (mObjProfileData != null)
                {
                    ProfileViewModel mObjProfileModelView = new ProfileViewModel()
                    {
                        ProfileID = mObjProfileData.ProfileID.GetValueOrDefault(),
                        ProfileReferenceNo = mObjProfileData.ProfileReferenceNo,
                        AssetTypeID = mObjProfileData.AssetTypeID.GetValueOrDefault(),
                        TaxPayerTypeIds = TrynParse.parseIntArray(mObjProfileData.TaxPayerTypeIds),
                        TaxPayerRoleIds = TrynParse.parseIntArray(mObjProfileData.TaxPayerRoleIds),
                        AssetTypeStatus = mObjProfileData.AssetTypeStatusID.GetValueOrDefault(),
                        ProfileDescription = mObjProfileData.ProfileDescription,
                        Active = mObjProfileData.Active.GetValueOrDefault(),
                        ProfileSectorIds = TrynParse.parseIntArray(mObjProfileData.ProfileSectorIds),
                        ProfileSubSectorIds = TrynParse.parseIntArray(mObjProfileData.ProfileSubSectorIds),
                        ProfileGroupIds = TrynParse.parseIntArray(mObjProfileData.ProfileGroupIds),
                        ProfileSubGroupIds = TrynParse.parseIntArray(mObjProfileData.ProfileSubGroupIds),
                        ProfileSectorElementIds = TrynParse.parseIntArray(mObjProfileData.ProfileSectorElementIds),
                        ProfileSectorSubElementIds = TrynParse.parseIntArray(mObjProfileData.ProfileSectorSubElementIds),
                        ProfileAttributeIds = TrynParse.parseIntArray(mObjProfileData.ProfileAttributeIds),
                        ProfileSubAttributeIds = TrynParse.parseIntArray(mObjProfileData.ProfileSubAttributeIds),
                    };

                    UI_FillDropDown(mObjProfileModelView);
                    return View(mObjProfileModelView);
                }
                else
                {
                    return RedirectToAction("List", "Profile");
                }
            }
            else
            {
                return RedirectToAction("List", "Profile");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(ProfileViewModel pObjProfileModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjProfileModel);
                return View(pObjProfileModel);
            }
            else
            {
                using (TransactionScope mObjTransactionScope = new TransactionScope())
                {
                    BLProfile mObjBLProfile = new BLProfile();

                    Profile mObjProfile = new Profile()
                    {
                        ProfileID = pObjProfileModel.ProfileID,
                        AssetTypeID = pObjProfileModel.AssetTypeID,
                        AssetTypeStatus = pObjProfileModel.AssetTypeStatus,
                        ProfileDescription = pObjProfileModel.ProfileDescription,
                        Active = pObjProfileModel.Active,
                        ModifiedBy = SessionManager.SystemUserID,
                        ModifiedDate = CommUtil.GetCurrentDateTime()
                    };

                    try
                    {

                        FuncResponse<Profile> mObjResponse = mObjBLProfile.BL_InsertUpdateProfile(mObjProfile);

                        if (mObjResponse.Success)
                        {
                            IList<ProfileTaxPayerType> lstOldProfileTaxPayerType = mObjBLProfile.BL_GetProfileTaxPayerType(pObjProfileModel.ProfileID);

                            int[] intRemovedProfileTaxPayerType = new int[] { };
                            int[] intAddedProfileTaxPayerType = new int[] { };

                            if (pObjProfileModel.TaxPayerTypeIds == null)
                            {
                                intRemovedProfileTaxPayerType = lstOldProfileTaxPayerType.Select(t => t.ProfileTaxPayerTypeID).ToArray();
                            }
                            else
                            {
                                intRemovedProfileTaxPayerType = lstOldProfileTaxPayerType.Where(t => !pObjProfileModel.TaxPayerTypeIds.Contains(t.TaxPayerTypeID.GetValueOrDefault())).Select(t => t.ProfileTaxPayerTypeID).ToArray();

                                if (lstOldProfileTaxPayerType == null || lstOldProfileTaxPayerType.Count() == 0)
                                {
                                    intAddedProfileTaxPayerType = pObjProfileModel.TaxPayerTypeIds;
                                }
                                else
                                {
                                    int[] intProfileTaxPayerTypeID = lstOldProfileTaxPayerType.Select(t => t.TaxPayerTypeID.GetValueOrDefault()).ToArray();
                                    intAddedProfileTaxPayerType = pObjProfileModel.TaxPayerTypeIds.Except(intProfileTaxPayerTypeID).ToArray();
                                }
                            }

                            foreach (int intProfileTaxPayerTypeID in intRemovedProfileTaxPayerType)
                            {
                                ProfileTaxPayerType mObjProfileTaxPayerType = new ProfileTaxPayerType()
                                {
                                    ProfileTaxPayerTypeID = intProfileTaxPayerTypeID
                                };

                                mObjBLProfile.BL_RemoveProfileTaxPayerType(mObjProfileTaxPayerType);
                            }

                            foreach (int intProfileTaxPayerTypeID in intAddedProfileTaxPayerType)
                            {
                                ProfileTaxPayerType mObjProfileTaxPayerType = new ProfileTaxPayerType()
                                {
                                    ProfileID = pObjProfileModel.ProfileID,
                                    TaxPayerTypeID = intProfileTaxPayerTypeID,
                                    CreatedBy = SessionManager.SystemUserID,
                                    CreatedDate = DateTime.Now
                                };

                                mObjBLProfile.BL_InsertProfileTaxPayerType(mObjProfileTaxPayerType);
                            }

                            IList<ProfileTaxPayerRole> lstOldProfileTaxPayerRole = mObjBLProfile.BL_GetProfileTaxPayerRole(pObjProfileModel.ProfileID);

                            int[] intRemovedProfileTaxPayerRole = new int[] { };
                            int[] intAddedProfileTaxPayerRole = new int[] { };

                            if (pObjProfileModel.TaxPayerRoleIds == null)
                            {
                                intRemovedProfileTaxPayerRole = lstOldProfileTaxPayerRole.Select(t => t.ProfileTaxPayerRoleID).ToArray();
                            }
                            else
                            {
                                intRemovedProfileTaxPayerRole = lstOldProfileTaxPayerRole.Where(t => !pObjProfileModel.TaxPayerRoleIds.Contains(t.TaxPayerRoleID.GetValueOrDefault())).Select(t => t.ProfileTaxPayerRoleID).ToArray();

                                if (lstOldProfileTaxPayerRole == null || lstOldProfileTaxPayerRole.Count() == 0)
                                {
                                    intAddedProfileTaxPayerRole = pObjProfileModel.TaxPayerRoleIds;
                                }
                                else
                                {
                                    int[] intProfileTaxPayerRoleID = lstOldProfileTaxPayerRole.Select(t => t.TaxPayerRoleID.GetValueOrDefault()).ToArray();
                                    intAddedProfileTaxPayerRole = pObjProfileModel.TaxPayerRoleIds.Except(intProfileTaxPayerRoleID).ToArray();
                                }
                            }

                            foreach (int intProfileTaxPayerRoleID in intRemovedProfileTaxPayerRole)
                            {
                                ProfileTaxPayerRole mObjProfileTaxPayerRole = new ProfileTaxPayerRole()
                                {
                                    ProfileTaxPayerRoleID = intProfileTaxPayerRoleID
                                };

                                mObjBLProfile.BL_RemoveProfileTaxPayerRole(mObjProfileTaxPayerRole);
                            }

                            foreach (int intProfileTaxPayerRoleID in intAddedProfileTaxPayerRole)
                            {
                                ProfileTaxPayerRole mObjProfileTaxPayerRole = new ProfileTaxPayerRole()
                                {
                                    ProfileID = pObjProfileModel.ProfileID,
                                    TaxPayerRoleID = intProfileTaxPayerRoleID,
                                    CreatedBy = SessionManager.SystemUserID,
                                    CreatedDate = DateTime.Now
                                };

                                mObjBLProfile.BL_InsertProfileTaxPayerRole(mObjProfileTaxPayerRole);
                            }

                            IList<ProfileSector> lstOldProfileSector = mObjBLProfile.BL_GetProfileSector(pObjProfileModel.ProfileID);

                            int[] intRemovedProfileSector = new int[] { };
                            int[] intAddedProfileSector = new int[] { };

                            if (pObjProfileModel.ProfileSectorIds == null)
                            {
                                intRemovedProfileSector = lstOldProfileSector.Select(t => t.ProfileSectorID).ToArray();
                            }
                            else
                            {
                                intRemovedProfileSector = lstOldProfileSector.Where(t => !pObjProfileModel.ProfileSectorIds.Contains(t.SectorID.GetValueOrDefault())).Select(t => t.ProfileSectorID).ToArray();

                                if (lstOldProfileSector == null || lstOldProfileSector.Count() == 0)
                                {
                                    intAddedProfileSector = pObjProfileModel.ProfileSectorIds;
                                }
                                else
                                {
                                    int[] intProfileSectorID = lstOldProfileSector.Select(t => t.SectorID.GetValueOrDefault()).ToArray();
                                    intAddedProfileSector = pObjProfileModel.ProfileSectorIds.Except(intProfileSectorID).ToArray();
                                }
                            }

                            foreach (int intProfileSectorID in intRemovedProfileSector)
                            {
                                ProfileSector mObjProfileSector = new ProfileSector()
                                {
                                    ProfileSectorID = intProfileSectorID
                                };

                                mObjBLProfile.BL_RemoveProfileSector(mObjProfileSector);
                            }

                            foreach (int intProfileSectorID in intAddedProfileSector)
                            {
                                ProfileSector mObjProfileSector = new ProfileSector()
                                {
                                    ProfileID = pObjProfileModel.ProfileID,
                                    SectorID = intProfileSectorID,
                                    CreatedBy = SessionManager.SystemUserID,
                                    CreatedDate = DateTime.Now
                                };

                                mObjBLProfile.BL_InsertProfileSector(mObjProfileSector);
                            }


                            IList<ProfileSubSector> lstOldProfileSubSector = mObjBLProfile.BL_GetProfileSubSector(pObjProfileModel.ProfileID);

                            int[] intRemovedProfileSubSector = new int[] { };
                            int[] intAddedProfileSubSector = new int[] { };

                            if (pObjProfileModel.ProfileSubSectorIds == null)
                            {
                                intRemovedProfileSubSector = lstOldProfileSubSector.Select(t => t.ProfileSubSectorID).ToArray();
                            }
                            else
                            {
                                intRemovedProfileSubSector = lstOldProfileSubSector.Where(t => !pObjProfileModel.ProfileSubSectorIds.Contains(t.SubSectorID.GetValueOrDefault())).Select(t => t.ProfileSubSectorID).ToArray();

                                if (lstOldProfileSubSector == null || lstOldProfileSubSector.Count() == 0)
                                {
                                    intAddedProfileSubSector = pObjProfileModel.ProfileSubSectorIds;
                                }
                                else
                                {
                                    int[] intProfileSubSectorID = lstOldProfileSubSector.Select(t => t.SubSectorID.GetValueOrDefault()).ToArray();
                                    intAddedProfileSubSector = pObjProfileModel.ProfileSubSectorIds.Except(intProfileSubSectorID).ToArray();
                                }
                            }

                            foreach (int intProfileSubSectorID in intRemovedProfileSubSector)
                            {
                                ProfileSubSector mObjProfileSubSector = new ProfileSubSector()
                                {
                                    ProfileSubSectorID = intProfileSubSectorID
                                };

                                mObjBLProfile.BL_RemoveProfileSubSector(mObjProfileSubSector);
                            }

                            foreach (int intProfileSubSectorID in intAddedProfileSubSector)
                            {
                                ProfileSubSector mObjProfileSubSector = new ProfileSubSector()
                                {
                                    ProfileID = pObjProfileModel.ProfileID,
                                    SubSectorID = intProfileSubSectorID,
                                    CreatedBy = SessionManager.SystemUserID,
                                    CreatedDate = DateTime.Now
                                };

                                mObjBLProfile.BL_InsertProfileSubSector(mObjProfileSubSector);
                            }


                            IList<ProfileGroup> lstOldProfileGroup = mObjBLProfile.BL_GetProfileGroup(pObjProfileModel.ProfileID);

                            int[] intRemovedProfileGroup = new int[] { };
                            int[] intAddedProfileGroup = new int[] { };

                            if (pObjProfileModel.ProfileGroupIds == null)
                            {
                                intRemovedProfileGroup = lstOldProfileGroup.Select(t => t.ProfileGroupID).ToArray();
                            }
                            else
                            {
                                intRemovedProfileGroup = lstOldProfileGroup.Where(t => !pObjProfileModel.ProfileGroupIds.Contains(t.GroupID.GetValueOrDefault())).Select(t => t.ProfileGroupID).ToArray();

                                if (lstOldProfileGroup == null || lstOldProfileGroup.Count() == 0)
                                {
                                    intAddedProfileGroup = pObjProfileModel.ProfileGroupIds;
                                }
                                else
                                {
                                    int[] intProfileGroupID = lstOldProfileGroup.Select(t => t.GroupID.GetValueOrDefault()).ToArray();
                                    intAddedProfileGroup = pObjProfileModel.ProfileGroupIds.Except(intProfileGroupID).ToArray();
                                }
                            }

                            foreach (int intProfileGroupID in intRemovedProfileGroup)
                            {
                                ProfileGroup mObjProfileGroup = new ProfileGroup()
                                {
                                    ProfileGroupID = intProfileGroupID
                                };

                                mObjBLProfile.BL_RemoveProfileGroup(mObjProfileGroup);
                            }

                            foreach (int intProfileGroupID in intAddedProfileGroup)
                            {
                                ProfileGroup mObjProfileGroup = new ProfileGroup()
                                {
                                    ProfileID = pObjProfileModel.ProfileID,
                                    GroupID = intProfileGroupID,
                                    CreatedBy = SessionManager.SystemUserID,
                                    CreatedDate = DateTime.Now
                                };

                                mObjBLProfile.BL_InsertProfileGroup(mObjProfileGroup);
                            }


                            IList<ProfileSubGroup> lstOldProfileSubGroup = mObjBLProfile.BL_GetProfileSubGroup(pObjProfileModel.ProfileID);

                            int[] intRemovedProfileSubGroup = new int[] { };
                            int[] intAddedProfileSubGroup = new int[] { };

                            if (pObjProfileModel.ProfileSubGroupIds == null)
                            {
                                intRemovedProfileSubGroup = lstOldProfileSubGroup.Select(t => t.ProfileSubGroupID).ToArray();
                            }
                            else
                            {
                                intRemovedProfileSubGroup = lstOldProfileSubGroup.Where(t => !pObjProfileModel.ProfileSubGroupIds.Contains(t.SubGroupID.GetValueOrDefault())).Select(t => t.ProfileSubGroupID).ToArray();

                                if (lstOldProfileSubGroup == null || lstOldProfileSubGroup.Count() == 0)
                                {
                                    intAddedProfileSubGroup = pObjProfileModel.ProfileSubGroupIds;
                                }
                                else
                                {
                                    int[] intProfileSubGroupID = lstOldProfileSubGroup.Select(t => t.SubGroupID.GetValueOrDefault()).ToArray();
                                    intAddedProfileSubGroup = pObjProfileModel.ProfileSubGroupIds.Except(intProfileSubGroupID).ToArray();
                                }
                            }

                            foreach (int intProfileSubGroupID in intRemovedProfileSubGroup)
                            {
                                ProfileSubGroup mObjProfileSubGroup = new ProfileSubGroup()
                                {
                                    ProfileSubGroupID = intProfileSubGroupID
                                };

                                mObjBLProfile.BL_RemoveProfileSubGroup(mObjProfileSubGroup);
                            }

                            foreach (int intProfileSubGroupID in intAddedProfileSubGroup)
                            {
                                ProfileSubGroup mObjProfileSubGroup = new ProfileSubGroup()
                                {
                                    ProfileID = pObjProfileModel.ProfileID,
                                    SubGroupID = intProfileSubGroupID,
                                    CreatedBy = SessionManager.SystemUserID,
                                    CreatedDate = DateTime.Now
                                };

                                mObjBLProfile.BL_InsertProfileSubGroup(mObjProfileSubGroup);
                            }

                            IList<ProfileSectorElement> lstOldProfileSectorElement = mObjBLProfile.BL_GetProfileSectorElement(pObjProfileModel.ProfileID);

                            int[] intRemovedProfileSectorElement = new int[] { };
                            int[] intAddedProfileSectorElement = new int[] { };

                            if (pObjProfileModel.ProfileSectorElementIds == null)
                            {
                                intRemovedProfileSectorElement = lstOldProfileSectorElement.Select(t => t.ProfileSectorElementID).ToArray();
                            }
                            else
                            {
                                intRemovedProfileSectorElement = lstOldProfileSectorElement.Where(t => !pObjProfileModel.ProfileSectorElementIds.Contains(t.SectorElementID.GetValueOrDefault())).Select(t => t.ProfileSectorElementID).ToArray();

                                if (lstOldProfileSectorElement == null || lstOldProfileSectorElement.Count() == 0)
                                {
                                    intAddedProfileSectorElement = pObjProfileModel.ProfileSectorElementIds;
                                }
                                else
                                {
                                    int[] intProfileSectorElementID = lstOldProfileSectorElement.Select(t => t.SectorElementID.GetValueOrDefault()).ToArray();
                                    intAddedProfileSectorElement = pObjProfileModel.ProfileSectorElementIds.Except(intProfileSectorElementID).ToArray();
                                }
                            }

                            foreach (int intProfileSectorElementID in intRemovedProfileSectorElement)
                            {
                                ProfileSectorElement mObjProfileSectorElement = new ProfileSectorElement()
                                {
                                    ProfileSectorElementID = intProfileSectorElementID
                                };

                                mObjBLProfile.BL_RemoveProfileSectorElement(mObjProfileSectorElement);
                            }

                            foreach (int intProfileSectorElementID in intAddedProfileSectorElement)
                            {
                                ProfileSectorElement mObjProfileSectorElement = new ProfileSectorElement()
                                {
                                    ProfileID = pObjProfileModel.ProfileID,
                                    SectorElementID = intProfileSectorElementID,
                                    CreatedBy = SessionManager.SystemUserID,
                                    CreatedDate = DateTime.Now
                                };

                                mObjBLProfile.BL_InsertProfileSectorElement(mObjProfileSectorElement);
                            }

                            IList<ProfileSectorSubElement> lstOldProfileSectorSubElement = mObjBLProfile.BL_GetProfileSectorSubElement(pObjProfileModel.ProfileID);

                            int[] intRemovedProfileSectorSubElement = new int[] { };
                            int[] intAddedProfileSectorSubElement = new int[] { };

                            if (pObjProfileModel.ProfileSectorSubElementIds == null)
                            {
                                intRemovedProfileSectorSubElement = lstOldProfileSectorSubElement.Select(t => t.ProfileSectorSubElementID).ToArray();
                            }
                            else
                            {
                                intRemovedProfileSectorSubElement = lstOldProfileSectorSubElement.Where(t => !pObjProfileModel.ProfileSectorSubElementIds.Contains(t.SectorSubElementID.GetValueOrDefault())).Select(t => t.ProfileSectorSubElementID).ToArray();

                                if (lstOldProfileSectorSubElement == null || lstOldProfileSectorSubElement.Count() == 0)
                                {
                                    intAddedProfileSectorSubElement = pObjProfileModel.ProfileSectorSubElementIds;
                                }
                                else
                                {
                                    int[] intProfileSectorSubElementID = lstOldProfileSectorSubElement.Select(t => t.SectorSubElementID.GetValueOrDefault()).ToArray();
                                    intAddedProfileSectorSubElement = pObjProfileModel.ProfileSectorSubElementIds.Except(intProfileSectorSubElementID).ToArray();
                                }
                            }

                            foreach (int intProfileSectorSubElementID in intRemovedProfileSectorSubElement)
                            {
                                ProfileSectorSubElement mObjProfileSectorSubElement = new ProfileSectorSubElement()
                                {
                                    ProfileSectorSubElementID = intProfileSectorSubElementID
                                };

                                mObjBLProfile.BL_RemoveProfileSectorSubElement(mObjProfileSectorSubElement);
                            }

                            foreach (int intProfileSectorSubElementID in intAddedProfileSectorSubElement)
                            {
                                ProfileSectorSubElement mObjProfileSectorSubElement = new ProfileSectorSubElement()
                                {
                                    ProfileID = pObjProfileModel.ProfileID,
                                    SectorSubElementID = intProfileSectorSubElementID,
                                    CreatedBy = SessionManager.SystemUserID,
                                    CreatedDate = DateTime.Now
                                };

                                mObjBLProfile.BL_InsertProfileSectorSubElement(mObjProfileSectorSubElement);
                            }

                            IList<ProfileAttribute> lstOldProfileAttribute = mObjBLProfile.BL_GetProfileAttribute(pObjProfileModel.ProfileID);

                            int[] intRemovedProfileAttribute = new int[] { };
                            int[] intAddedProfileAttribute = new int[] { };

                            if (pObjProfileModel.ProfileAttributeIds == null)
                            {
                                intRemovedProfileAttribute = lstOldProfileAttribute.Select(t => t.ProfileAttributeID).ToArray();
                            }
                            else
                            {
                                intRemovedProfileAttribute = lstOldProfileAttribute.Where(t => !pObjProfileModel.ProfileAttributeIds.Contains(t.AttributeID.GetValueOrDefault())).Select(t => t.ProfileAttributeID).ToArray();

                                if (lstOldProfileAttribute == null || lstOldProfileAttribute.Count() == 0)
                                {
                                    intAddedProfileAttribute = pObjProfileModel.ProfileAttributeIds;
                                }
                                else
                                {
                                    int[] intProfileAttributeID = lstOldProfileAttribute.Select(t => t.AttributeID.GetValueOrDefault()).ToArray();
                                    intAddedProfileAttribute = pObjProfileModel.ProfileAttributeIds.Except(intProfileAttributeID).ToArray();
                                }
                            }

                            foreach (int intProfileAttributeID in intRemovedProfileAttribute)
                            {
                                ProfileAttribute mObjProfileAttribute = new ProfileAttribute()
                                {
                                    ProfileAttributeID = intProfileAttributeID
                                };

                                mObjBLProfile.BL_RemoveProfileAttribute(mObjProfileAttribute);
                            }

                            foreach (int intProfileAttributeID in intAddedProfileAttribute)
                            {
                                ProfileAttribute mObjProfileAttribute = new ProfileAttribute()
                                {
                                    ProfileID = pObjProfileModel.ProfileID,
                                    AttributeID = intProfileAttributeID,
                                    CreatedBy = SessionManager.SystemUserID,
                                    CreatedDate = DateTime.Now
                                };

                                mObjBLProfile.BL_InsertProfileAttribute(mObjProfileAttribute);
                            }

                            IList<ProfileSubAttribute> lstOldProfileSubAttribute = mObjBLProfile.BL_GetProfileSubAttribute(pObjProfileModel.ProfileID);

                            int[] intRemovedProfileSubAttribute = new int[] { };
                            int[] intAddedProfileSubAttribute = new int[] { };

                            if (pObjProfileModel.ProfileSubAttributeIds == null)
                            {
                                intRemovedProfileSubAttribute = lstOldProfileSubAttribute.Select(t => t.ProfileSubAttributeID).ToArray();
                            }
                            else
                            {
                                intRemovedProfileSubAttribute = lstOldProfileSubAttribute.Where(t => !pObjProfileModel.ProfileSubAttributeIds.Contains(t.SubAttributeID.GetValueOrDefault())).Select(t => t.ProfileSubAttributeID).ToArray();

                                if (lstOldProfileSubAttribute == null || lstOldProfileSubAttribute.Count() == 0)
                                {
                                    intAddedProfileSubAttribute = pObjProfileModel.ProfileSubAttributeIds;
                                }
                                else
                                {
                                    int[] intProfileSubAttributeID = lstOldProfileSubAttribute.Select(t => t.SubAttributeID.GetValueOrDefault()).ToArray();
                                    intAddedProfileSubAttribute = pObjProfileModel.ProfileSubAttributeIds.Except(intProfileSubAttributeID).ToArray();
                                }
                            }

                            foreach (int intProfileSubAttributeID in intRemovedProfileSubAttribute)
                            {
                                ProfileSubAttribute mObjProfileSubAttribute = new ProfileSubAttribute()
                                {
                                    ProfileSubAttributeID = intProfileSubAttributeID
                                };

                                mObjBLProfile.BL_RemoveProfileSubAttribute(mObjProfileSubAttribute);
                            }

                            foreach (int intProfileSubAttributeID in intAddedProfileSubAttribute)
                            {
                                ProfileSubAttribute mObjProfileSubAttribute = new ProfileSubAttribute()
                                {
                                    ProfileID = pObjProfileModel.ProfileID,
                                    SubAttributeID = intProfileSubAttributeID,
                                    CreatedBy = SessionManager.SystemUserID,
                                    CreatedDate = DateTime.Now
                                };

                                mObjBLProfile.BL_InsertProfileSubAttribute(mObjProfileSubAttribute);
                            }

                            mObjTransactionScope.Complete();
                            FlashMessage.Info(mObjResponse.Message);
                            return RedirectToAction("List", "Profile");
                        }
                        else
                        {
                            UI_FillDropDown(pObjProfileModel);
                            Transaction.Current.Rollback();
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjProfileModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillDropDown(pObjProfileModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving directorate";
                        return View(pObjProfileModel);
                    }
                }
            }
        }


        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Profile mObjProfile = new Profile()
                {
                    ProfileID = id.GetValueOrDefault(),
                    IntStatus = 2
                };

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(mObjProfile);

                if (mObjProfileData != null)
                {
                    ProfileViewModel mObjProfileModelView = new ProfileViewModel()
                    {
                        ProfileID = mObjProfileData.ProfileID.GetValueOrDefault(),
                        ProfileReferenceNo = mObjProfileData.ProfileReferenceNo,
                        AssetTypeName = mObjProfileData.AssetTypeName,
                        TaxPayerTypeNames = mObjProfileData.TaxPayerTypeNames,
                        TaxPayerRoleNames = mObjProfileData.TaxPayerRoleNames,
                        AssetTypeStatusName = mObjProfileData.AssetTypeStatusName,
                        ProfileDescription = mObjProfileData.ProfileDescription,
                        ActiveText = mObjProfileData.ActiveText,
                        ProfileSectorNames = mObjProfileData.ProfileSectorNames,
                        ProfileSubSectorNames = mObjProfileData.ProfileSubSectorNames,
                        ProfileGroupNames = mObjProfileData.ProfileGroupNames,
                        ProfileSubGroupNames = mObjProfileData.ProfileSubGroupNames,
                        ProfileSectorElementNames = mObjProfileData.ProfileSectorElementNames,
                        ProfileSectorSubElementNames = mObjProfileData.ProfileSectorSubElementNames,
                        ProfileAttributeNames = mObjProfileData.ProfileAttributeNames,
                        ProfileSubAttributeNames = mObjProfileData.ProfileSubAttributeNames,
                    };

                    UI_FillDropDown(mObjProfileModelView);
                    return View(mObjProfileModelView);
                }
                else
                {
                    return RedirectToAction("List", "Profile");
                }
            }
            else
            {
                return RedirectToAction("List", "Profile");
            }
        }

        public JsonResult UpdateStatus(Profile pObjProfileData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjProfileData.ProfileID != 0)
            {
                FuncResponse mObjFuncResponse = new BLProfile().BL_UpdateStatus(pObjProfileData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["ProfileList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDropDownList(int AssetTypeID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>
            {
                ["TaxPayerRoleList"] = new BLTaxPayerRole().BL_GetTaxPayerRoleDropDownList(new TaxPayer_Roles() { AssetTypeID = AssetTypeID, intStatus = 1 })
            };

            if (AssetTypeID == (int)EnumList.AssetTypes.Building)
            {
                dcResponse["ProfileSectorList"] = new BLBuildingType().BL_GetBuildingTypeDropDownList(new Building_Types() { intStatus = 1 });
                dcResponse["ProfileSubSectorList"] = new BLLGA().BL_GetLGADropDownList(new LGA() { intStatus = 1 });
                dcResponse["ProfileGroupList"] = new BLBuildingCompletion().BL_GetBuildingCompletionDropDownList(new Building_Completion() { intStatus = 1 });
                dcResponse["ProfileSubGroupList"] = new BLBuildingPurpose().BL_GetBuildingPurposeDropDownList(new Building_Purpose() { intStatus = 1 });
                dcResponse["ProfileSectorElementList"] = new BLUnitPurpose().BL_GetUnitPurposeDropDownList(new Unit_Purpose() { intStatus = 1 });
                dcResponse["ProfileSectorSubElementList"] = new BLBuildingOwnership().BL_GetBuildingOwnershipDropDownList(new Building_Ownership() { intStatus = 1 });
                dcResponse["ProfileAttributeList"] = new BLUnitFunction().BL_GetUnitFunctionDropDownList(new Unit_Function() { intStatus = 1 });
                dcResponse["ProfileSubAttributeList"] = new BLUnitOccupancy().BL_GetUnitOccupancyDropDownList(new Unit_Occupancy() { intStatus = 1 });
            }
            else if (AssetTypeID == (int)EnumList.AssetTypes.Vehicles)
            {
                dcResponse["ProfileSectorList"] = new BLVehicleType().BL_GetVehicleTypeDropDownList(new Vehicle_Types() { intStatus = 1 });
                dcResponse["ProfileSubSectorList"] = new BLLGA().BL_GetLGADropDownList(new LGA() { intStatus = 1 });
                dcResponse["ProfileGroupList"] = new BLVehiclePurpose().BL_GetVehiclePurposeDropDownList(new Vehicle_Purpose() { intStatus = 1 });
                dcResponse["ProfileSubGroupList"] = new BLVehicleFunction().BL_GetVehicleFunctionDropDownList(new Vehicle_Function() { intStatus = 1 });
                dcResponse["ProfileSectorElementList"] = new BLVehicleOwnership().BL_GetVehicleOwnershipDropDownList(new Vehicle_Ownership() { intStatus = 1 });
                dcResponse["ProfileSectorSubElementList"] = new BLVehicleSubType().BL_GetVehicleSubTypeDropDownList(new Vehicle_SubTypes() { intStatus = 1 });
            }
            else if (AssetTypeID == (int)EnumList.AssetTypes.Business)
            {
                dcResponse["ProfileSectorList"] = new BLBusinessType().BL_GetBusinessTypeDropDownList(new Business_Types() { intStatus = 1 });
                dcResponse["ProfileSubSectorList"] = new BLLGA().BL_GetLGADropDownList(new LGA() { intStatus = 1 });
                dcResponse["ProfileGroupList"] = new BLBusinessCategory().BL_GetBusinessCategoryDropDownList(new Business_Category() { intStatus = 1 });
                dcResponse["ProfileSubGroupList"] = new BLBusinessSector().BL_GetBusinessSectorDropDownList(new Business_Sector() { intStatus = 1 });
                dcResponse["ProfileSectorElementList"] = new BLBusinessSubSector().BL_GetBusinessSubSectorDropDownList(new Business_SubSector() { intStatus = 1 });
                dcResponse["ProfileSectorSubElementList"] = new BLBusinessStructure().BL_GetBusinessStructureDropDownList(new Business_Structure() { intStatus = 1 });
                dcResponse["ProfileAttributeList"] = new BLBusinessOperation().BL_GetBusinessOperationDropDownList(new Business_Operation() { intStatus = 1 });
                dcResponse["ProfileSubAttributeList"] = new BLSize().BL_GetSizeDropDownList(new Size() { intStatus = 1 });

            }
            else if (AssetTypeID == (int)EnumList.AssetTypes.Land)
            {
                dcResponse["ProfileSectorList"] = new BLLGA().BL_GetLGADropDownList(new LGA() { intStatus = 1 });
                dcResponse["ProfileSubSectorList"] = new BLLandPurpose().BL_GetLandPurposeDropDownList(new Land_Purpose() { intStatus = 1 });
                dcResponse["ProfileGroupList"] = new BLLandOwnership().BL_GetLandOwnershipDropDownList(new Land_Ownership() { intStatus = 1 });
                dcResponse["ProfileSubGroupList"] = new BLLandDevelopment().BL_GetLandDevelopmentDropDownList(new Land_Development() { intStatus = 1 });
                dcResponse["ProfileSectorElementList"] = new BLLandFunction().BL_GetLandFunctionDropDownList(new Land_Function() { intStatus = 1 });
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
    }
}