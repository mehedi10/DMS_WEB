using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DMS_WEB.Models;
using System.Web.Script.Serialization;
using System.Data.Objects;
using System.Configuration;
using System.IO;

namespace DMS_WEB.Controllers.ClientPrivate
{
    public class PrivateBulkAccountSearchController : BaseController
    {
        //
        // GET: /PrivateBulkAccountSearch/

        public ActionResult Index()
        {
            this.LoadSecurity();

            List<SET_CLIENT_DEPARTMENTS_GET_Result> dept_list = new List<SET_CLIENT_DEPARTMENTS_GET_Result>();
            using (DBEntities db = new DBEntities()) {
                dept_list = db.SET_CLIENT_DEPARTMENTS_GET(null, this.user_info.CLIENT_NO.Value, null).ToList();
            }
            if (dept_list == null) {
                dept_list = new List<SET_CLIENT_DEPARTMENTS_GET_Result>();
            }

            ViewBag.DEPT_NO = new SelectList(dept_list, "DEPT_NO", "DEPT_NAME");

            return View();
        }

        [HttpPost]
        public ActionResult Search(ClientAccountSearch obj) {
            this.LoadSecurity();

            obj.CLIENT_NO = this.user_info.CLIENT_NO;

            ViewBag.PageNumber = 1;
            TempData["obj_ClientAccountSearch"] = obj;

            List<SET_CLIENT_ACCOUNTS_BULK_GET_Result> batch_list = new List<SET_CLIENT_ACCOUNTS_BULK_GET_Result>();
            obj.TotalRecords = 0;

            using (DBEntities db = new DBEntities()) {
                ObjectParameter CNT_parameter = new ObjectParameter("CNT", obj.TotalRecords);
                db.SET_CLIENT_ACCOUNTS_BULK_GET_PAGED(CNT_parameter, obj.ACC_NO, obj.CLIENT_NO, obj.DEPT_NO, obj.CLIENT_BOX_ID, obj.BATCH_NO, obj.BATCH_ID, obj.ARCHIVE_DATE, obj.ACC_ID, obj.TRN_TYPE_NO, obj.REQ_TYPE_NO, obj.TRANSMIT_NO, obj.IS_OLD_DATA);
                obj.TotalRecords = (long)CNT_parameter.Value;
                batch_list = db.SET_CLIENT_ACCOUNTS_BULK_GET(obj.ACC_NO, obj.CLIENT_NO, obj.DEPT_NO, obj.CLIENT_BOX_ID, obj.BATCH_NO, obj.BATCH_ID, obj.ARCHIVE_DATE, obj.ACC_ID, obj.TRN_TYPE_NO, obj.REQ_TYPE_NO, obj.TRANSMIT_NO, obj.IS_OLD_DATA, obj.START_INDEX, this.PAGE_SIZE * obj.PAGE_NUMBER).ToList();
            }

            if (batch_list == null) {
                batch_list = new List<SET_CLIENT_ACCOUNTS_BULK_GET_Result>();
            }

            ViewBag.TotalRecords = obj.TotalRecords;
            ViewBag.PAGE_SIZE = this.PAGE_SIZE;

            TempData["batch_TotalRecords"] = obj.TotalRecords;
            TempData.Keep();
            return View("List", batch_list);

        }

        public ActionResult GetPaged(int PageNumber) {
            TempData.Keep();
            ClientAccountSearch obj = TempData["obj_ClientAccountSearch"] as ClientAccountSearch;

            if (PageNumber == 0) {
                PageNumber = 1;
            }

            int start_index = (this.PAGE_SIZE * (PageNumber - 1) + 1);

            List<SET_CLIENT_ACCOUNTS_BULK_GET_Result> acc_list = new List<SET_CLIENT_ACCOUNTS_BULK_GET_Result>();
            using (DBEntities db = new DBEntities()) {
                acc_list = db.SET_CLIENT_ACCOUNTS_BULK_GET(obj.ACC_NO, obj.CLIENT_NO, obj.DEPT_NO, obj.CLIENT_BOX_ID, obj.BATCH_NO, obj.BATCH_ID, obj.ARCHIVE_DATE, obj.ACC_ID, obj.TRN_TYPE_NO, obj.REQ_TYPE_NO, obj.TRANSMIT_NO, obj.IS_OLD_DATA, start_index, this.PAGE_SIZE * PageNumber).ToList();
            }

            if (acc_list == null) {
                acc_list = new List<SET_CLIENT_ACCOUNTS_BULK_GET_Result>();
            }

            ViewBag.PageNumber = PageNumber;

            ViewBag.TotalRecords = TempData["batch_TotalRecords"];
            ViewBag.PAGE_SIZE = this.PAGE_SIZE;

            return View("List", acc_list);
        }

    }
}
