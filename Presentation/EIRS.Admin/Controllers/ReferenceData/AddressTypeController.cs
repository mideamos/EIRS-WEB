using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;

namespace EIRS.Admin.Controllers
{
    public class AddressTypeController : BaseController
    {
        public ActionResult List()
        {
            Address_Types mObjAddressType = new Address_Types()
            {
                intStatus = 2
            };

            IList<usp_GetAddressTypeList_Result> lstAddressType = new BLAddressType().BL_GetAddressTypeList(mObjAddressType);
            return View(lstAddressType);
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(AddressTypeViewModel pObjAddressTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjAddressTypeModel);
            }
            else
            {
                Address_Types mObjAddressType = new Address_Types()
                {
                    AddressTypeID = 0,
                    AddressTypeName = pObjAddressTypeModel.AddressTypeName.Trim(),                    
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAddressType().BL_InsertUpdateAddressType(mObjAddressType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AddressType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAddressTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving address type";
                    return View(pObjAddressTypeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Address_Types mObjAddressType = new Address_Types()
                {
                    AddressTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAddressTypeList_Result mObjAddressTypeData = new BLAddressType().BL_GetAddressTypeDetails(mObjAddressType);

                if (mObjAddressTypeData != null)
                {
                    AddressTypeViewModel mObjAddressTypeModelView = new AddressTypeViewModel()
                    {
                        AddressTypeID = mObjAddressTypeData.AddressTypeID.GetValueOrDefault(),
                        AddressTypeName = mObjAddressTypeData.AddressTypeName,
                        Active = mObjAddressTypeData.Active.GetValueOrDefault(),
                    };

                    return View(mObjAddressTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "AddressType");
                }
            }
            else
            {
                return RedirectToAction("List", "AddressType");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(AddressTypeViewModel pObjAddressTypeModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjAddressTypeModel);
            }
            else
            {
                Address_Types mObjAddressType = new Address_Types()
                {
                    AddressTypeID = pObjAddressTypeModel.AddressTypeID,
                    AddressTypeName = pObjAddressTypeModel.AddressTypeName.Trim(),
                    Active = pObjAddressTypeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLAddressType().BL_InsertUpdateAddressType(mObjAddressType);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "AddressType");
                    }
                    else
                    {
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjAddressTypeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving address type";
                    return View(pObjAddressTypeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Address_Types mObjAddressType = new Address_Types()
                {
                    AddressTypeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetAddressTypeList_Result mObjAddressTypeData = new BLAddressType().BL_GetAddressTypeDetails(mObjAddressType);

                if (mObjAddressTypeData != null)
                {
                    AddressTypeViewModel mObjAddressTypeModelView = new AddressTypeViewModel()
                    {
                        AddressTypeID = mObjAddressTypeData.AddressTypeID.GetValueOrDefault(),
                        AddressTypeName = mObjAddressTypeData.AddressTypeName,
                        ActiveText = mObjAddressTypeData.ActiveText
                    };

                    return View(mObjAddressTypeModelView);
                }
                else
                {
                    return RedirectToAction("List", "AddressType");
                }
            }
            else
            {
                return RedirectToAction("List", "AddressType");
            }
        }

        public JsonResult UpdateStatus(Address_Types pObjAddressTypeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAddressTypeData.AddressTypeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLAddressType().BL_UpdateStatus(pObjAddressTypeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AddressTypeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
    }
}