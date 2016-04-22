using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMS_WEB.Controllers
{
    public class RequisitionDetailsController : BaseController
    {
        //
        // GET: /RequisitionDetails/

        public ActionResult Index()
        {
            this.LoadCommonJsonData();
            return View();
        }

        [HttpPost]
        public ActionResult Search()
        {
            return View();
        }

    }
}
