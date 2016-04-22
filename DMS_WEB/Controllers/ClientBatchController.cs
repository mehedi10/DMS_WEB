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

namespace DMS_WEB.Controllers
{
    public class ClientBatchController : BaseController
    {
        //
        // GET: /ClientBatch/

        public ActionResult Index()
        {
            SET_CLIENT_BATCHES_GET_Result model = new SET_CLIENT_BATCHES_GET_Result();

            this.LoadCommonJsonData();
            return View(model);
        }

        [HttpPost]
        public JsonResult Save(SET_CLIENT_BATCHES item) {
            bool is_saved = false;
            string str_error = "";

            int? BATCH_NO = 0;

            try {
                using (DBEntities db = new DBEntities()) {
                    if (item.BATCH_NO > 0) {
                        db.SET_CLIENT_BATCHES_UPDATE(item.BATCH_NO, item.CLIENT_NO, item.BOX_NO, item.ARCHIVE_DATE, item.BATCH_ID, item.BATCH_DETAILS, item.TRN_TYPE_NO, item.REQ_TYPE_NO, item.TRANSMIT_NO, item.LAST_TRN_DATE, item.IS_OLD_DATA, this.USER_NO, this.LOGON_NO);
                    } else {
                        ObjectParameter BATCH_NO_parameter = new ObjectParameter("BATCH_NO", BATCH_NO);
                        db.SET_CLIENT_BATCHES_INSERT(BATCH_NO_parameter,  item.CLIENT_NO, item.BOX_NO, item.ARCHIVE_DATE, item.BATCH_ID, item.BATCH_DETAILS, item.TRN_TYPE_NO, item.REQ_TYPE_NO, item.TRANSMIT_NO, item.LAST_TRN_DATE, item.IS_OLD_DATA, this.USER_NO, this.LOGON_NO);
                        BATCH_NO = (int?)BATCH_NO_parameter.Value;
                    }
                    is_saved = true;
                }
            } catch (Exception ex) {
                str_error += Environment.NewLine + ex.Message;
            }

            var ret_val = new {
                @is_success = is_saved,
                @msg = str_error,
            };

            return Json(ret_val, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Search(ClientBatchSearch obj) {

            ViewBag.PageNumber = 1;
            TempData["obj_ClientBatchSearch"] = obj;

            List<SET_CLIENT_BATCHES_GET_Result> batch_list = new List<SET_CLIENT_BATCHES_GET_Result>();
            obj.TotalRecords = 0;

            using (DBEntities db = new DBEntities()) {
                ObjectParameter CNT_parameter = new ObjectParameter("CNT", obj.TotalRecords);
                db.SET_CLIENT_BATCHES_GET_PAGED(CNT_parameter, obj.BATCH_NO, obj.BATCH_ID, obj.BOX_NO, obj.CLIENT_BOX_ID, obj.CLIENT_NO, obj.ARCHIVE_DATE, obj.TRN_TYPE_NO, obj.REQ_TYPE_NO, obj.TRANSMIT_NO, obj.IS_OLD_DATA);
                obj.TotalRecords = (long)CNT_parameter.Value;
                batch_list = db.SET_CLIENT_BATCHES_GET(obj.BATCH_NO, obj.BATCH_ID, obj.BOX_NO, obj.CLIENT_BOX_ID, obj.CLIENT_NO, obj.ARCHIVE_DATE, obj.TRN_TYPE_NO, obj.REQ_TYPE_NO, obj.TRANSMIT_NO, obj.IS_OLD_DATA, obj.START_INDEX, this.PAGE_SIZE * obj.PAGE_NUMBER).ToList();
            }

            if (batch_list == null) {
                batch_list = new List<SET_CLIENT_BATCHES_GET_Result>();
            }

            ViewBag.TotalRecords = obj.TotalRecords;
            ViewBag.PAGE_SIZE = this.PAGE_SIZE;

            TempData["batch_TotalRecords"] = obj.TotalRecords;
            TempData.Keep();
            return View("List", batch_list);

        }

        public ActionResult GetPaged(int PageNumber) {
            TempData.Keep();
            ClientBatchSearch obj = TempData["obj_ClientBatchSearch"] as ClientBatchSearch;

            if (PageNumber == 0) {
                PageNumber = 1;
            }

            int start_index = (this.PAGE_SIZE * (PageNumber - 1) + 1);

            List<SET_CLIENT_BATCHES_GET_Result> batch_list = new List<SET_CLIENT_BATCHES_GET_Result>();
            using (DBEntities db = new DBEntities()) {
                batch_list = db.SET_CLIENT_BATCHES_GET(obj.BATCH_NO, obj.BATCH_ID, obj.BOX_NO, obj.CLIENT_BOX_ID, obj.CLIENT_NO, obj.ARCHIVE_DATE, obj.TRN_TYPE_NO, obj.REQ_TYPE_NO, obj.TRANSMIT_NO, obj.IS_OLD_DATA, start_index, this.PAGE_SIZE * PageNumber).ToList();
            }

            if (batch_list == null) {
                batch_list = new List<SET_CLIENT_BATCHES_GET_Result>();
            }

            ViewBag.PageNumber = PageNumber;

            ViewBag.TotalRecords = TempData["batch_TotalRecords"];
            ViewBag.PAGE_SIZE = this.PAGE_SIZE;

            return View("List", batch_list);
        }

        public ActionResult Edit(int id = 0) {
            SET_CLIENT_BATCHES_GET_Result info = new SET_CLIENT_BATCHES_GET_Result();
            using (DBEntities db = new DBEntities()) 
            {
                info = db.SET_CLIENT_BATCHES_GET(id, null, null, null, null, null, null, null, null, null, null, null).FirstOrDefault();
            }
            return View("Save", info);
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult Delete(int id = 0) {
            bool is_saved = false;
            string str_error = "";

            try {
                using (DBEntities db = new DBEntities()) {
                    db.SET_CLIENT_BATCHES_DELETE(id);
                }
                is_saved = true;
            } catch (Exception ex) {
                str_error += Environment.NewLine + ex.Message;
            }

            var ret_val = new {
                @is_success = is_saved,
                @msg = str_error,
            };

            return Json(ret_val, JsonRequestBehavior.AllowGet);
        }


    }
}
