using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DMS_WEB.Models;


namespace DMS_WEB.Controllers
{
    public class CompanyController : BaseController
    {
        //
        // GET: /Company/

        public ActionResult Index()
        {
            GEN_COMPANY_INFO_GET_Result company_info = new GEN_COMPANY_INFO_GET_Result(); 
            using (DBEntities db = new DBEntities()) {
                company_info = db.GEN_COMPANY_INFO_GET().FirstOrDefault();
            }
            return View(company_info);
        }

        [HttpPost]
        public ActionResult Index(GEN_COMPANY_INFO_GET_Result company_info) {
            GEN_COMPANY_INFO_GET_Result company_info_updated = new GEN_COMPANY_INFO_GET_Result(); 

            using (DBEntities db = new DBEntities())
            {
                db.GEN_COMPANY_INFO_UPDATE(company_info.COMP_NAME, company_info.KEY_FEATURE, company_info.ADDR,
                    company_info.PHONE, company_info.MOBILE, company_info.FAX, company_info.EMAIL_ADDR,
                    company_info.LAST_NEW_TRANS_ID, company_info.LAST_TRANS_IN_ID, company_info.LAST_TRANS_OUT_ID);
                company_info_updated = db.GEN_COMPANY_INFO_GET().FirstOrDefault();
            }
            return View(company_info_updated);
        }
    }
}
