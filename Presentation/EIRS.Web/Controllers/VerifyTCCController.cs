using EIRS.BLL;
using EIRS.Common;
using EIRS.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIRS.Web.Controllers
{
    public class VerifyTCCController : Controller
    {
        // GET: VerifyTCC
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        [HttpGet]
        public ActionResult VerifyFileUpload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult VerifyFileUpload(VerifyTCCFileUploadViewModel pObjVerifyTCCModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjVerifyTCCModel);
            }
            else
            {
                string strDirectory = GlobalDefaultValues.DocumentLocation + "Temp/" + DateTime.Now.ToString("ddMMyyyyhhmmss");

                string mstrFileName = "Document_" + DateTime.Now.ToString("ddMMyyyyhhmmss_") + Path.GetFileName(pObjVerifyTCCModel.DocumentFile.FileName);
                if (!Directory.Exists(strDirectory))
                {
                    Directory.CreateDirectory(strDirectory);
                }

                string mstrDocumentFilePath = Path.Combine(strDirectory, mstrFileName);
                pObjVerifyTCCModel.DocumentFile.SaveAs(mstrDocumentFilePath);

                ModelState.Clear();
                return View();

            }
        }

        [HttpGet]
        public ActionResult VerifyReferenceNumber()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult VerifyReferenceNumber(VerifyTCCReferenceNumberViewModel pObjVerifyTCCModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjVerifyTCCModel);
            }
            else
            {
                bool isTCCValid = new BLTCC().BL_VerifyTCCByReferenceNumber(pObjVerifyTCCModel.ReferenceNumber);
                ViewBag.TCCValid = isTCCValid;

                ModelState.Clear();
                return View();
            }
        }

    }
}