using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;

namespace EIRS.Web.Controllers
{
    public class ERASManualController : BaseController
    {
        // GET: ERASManual
        public ActionResult List()
        {
            return View();
        }

        public ActionResult DataSource()
        {
            IList<usp_EM_GetDataSourceList_Result> lstDataSource = new BLEMDataImport().BL_GetDataSourceList();
            return View(lstDataSource);
        }
    }
}