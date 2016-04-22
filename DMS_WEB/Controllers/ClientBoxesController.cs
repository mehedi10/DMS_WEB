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
    public class ClientBoxesController : BaseController
    {
        //
        // GET: /ClientBoxes/

        public ActionResult Index()
        {
            SET_CLIENT_DEPT_BOXES_GET_Result model = new SET_CLIENT_DEPT_BOXES_GET_Result();

            this.LoadCommonJsonData();
            return View(model);
        }

        [HttpPost]
        public JsonResult Save(SET_CLIENT_DEPT_BOXES item) {
            bool is_saved = false;
            string str_error = "";

            int? BOX_NO = 0;

            try {
                using (DBEntities db = new DBEntities()) {
                    if (item.BOX_NO > 0) {
                        db.SET_CLIENT_DEPT_BOXES_UPDATE(item.BOX_NO, item.CLIENT_NO, item.DEPT_NO, item.PROD_TYPE_NO, item.ARCHIVE_DATE, item.CLIENT_BOX_ID, item.CHALLAN_NO, item.TRN_TYPE_NO, item.REQ_TYPE_NO, item.TRANSMIT_NO, item.IS_OLD_DATA, item.RECALL_BOX_NO, this.USER_NO, this.LOGON_NO);
                    } else {
                        ObjectParameter BOX_NO_parameter = new ObjectParameter("BOX_NO", BOX_NO);
                        db.SET_CLIENT_DEPT_BOXES_INSERT(BOX_NO_parameter, item.CLIENT_NO, item.DEPT_NO, item.PROD_TYPE_NO, item.ARCHIVE_DATE, item.CLIENT_BOX_ID, item.CHALLAN_NO, item.TRN_TYPE_NO, item.REQ_TYPE_NO, item.TRANSMIT_NO, item.IS_OLD_DATA, item.RECALL_BOX_NO, this.USER_NO, this.LOGON_NO);
                        BOX_NO = (int?)BOX_NO_parameter.Value;
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
        public ActionResult Search(ClientBoxesSearch obj) {

            ViewBag.PageNumber = 1;
            TempData["obj_ClientBoxesSearch"] = obj;

            List<SET_CLIENT_DEPT_BOXES_GET_Result> box_list = new List<SET_CLIENT_DEPT_BOXES_GET_Result>();
            obj.TotalRecords = 0;

            using (DBEntities db = new DBEntities()) {
                ObjectParameter CNT_parameter = new ObjectParameter("CNT", obj.TotalRecords);                
                db.SET_CLIENT_DEPT_BOXES_GET_PAGED(CNT_parameter, obj.BOX_NO, obj.CLIENT_NO, obj.DEPT_NO, obj.PROD_TYPE_NO, obj.CLIENT_BOX_ID, obj.CHALLAN_NO, obj.TRN_TYPE_NO, obj.REQ_TYPE_NO, obj.TRANSMIT_NO, obj.IS_OLD_DATA, obj.RECALL_BOX_NO);
                obj.TotalRecords = (long)CNT_parameter.Value;
                box_list = db.SET_CLIENT_DEPT_BOXES_GET(obj.BOX_NO,obj.CLIENT_NO,obj.DEPT_NO,obj.PROD_TYPE_NO,obj.CLIENT_BOX_ID,obj.CHALLAN_NO,obj.TRN_TYPE_NO,obj.REQ_TYPE_NO,obj.TRANSMIT_NO,obj.IS_OLD_DATA,obj.RECALL_BOX_NO, obj.START_INDEX, this.PAGE_SIZE * obj.PAGE_NUMBER).ToList();
            }

            if (box_list == null) {
                box_list = new List<SET_CLIENT_DEPT_BOXES_GET_Result>();
            }

            ViewBag.TotalRecords = obj.TotalRecords;
            ViewBag.PAGE_SIZE = this.PAGE_SIZE;

            TempData["box_TotalRecords"] = obj.TotalRecords;
            TempData.Keep();
            return View("List", box_list);

        }

        public ActionResult GetPaged(int PageNumber) {
            TempData.Keep();
            ClientBoxesSearch obj = TempData["obj_ClientBoxesSearch"] as ClientBoxesSearch;

            if (PageNumber == 0) {
                PageNumber = 1;
            }

            int start_index = (this.PAGE_SIZE * (PageNumber - 1) + 1);

            List<SET_CLIENT_DEPT_BOXES_GET_Result> box_list = new List<SET_CLIENT_DEPT_BOXES_GET_Result>();
            using (DBEntities db = new DBEntities()) {
                box_list = db.SET_CLIENT_DEPT_BOXES_GET(obj.BOX_NO, obj.CLIENT_NO, obj.DEPT_NO, obj.PROD_TYPE_NO, obj.CLIENT_BOX_ID, obj.CHALLAN_NO, obj.TRN_TYPE_NO, obj.REQ_TYPE_NO, obj.TRANSMIT_NO, obj.IS_OLD_DATA, obj.RECALL_BOX_NO, start_index, this.PAGE_SIZE * PageNumber).ToList();
            }

            if (box_list == null) {
                box_list = new List<SET_CLIENT_DEPT_BOXES_GET_Result>();
            }

            ViewBag.PageNumber = PageNumber;

            ViewBag.TotalRecords = TempData["box_TotalRecords"];
            ViewBag.PAGE_SIZE = this.PAGE_SIZE;

            return View("List", box_list);
        }

        public ActionResult Edit(int id = 0) {
            SET_CLIENT_DEPT_BOXES_GET_Result info = new SET_CLIENT_DEPT_BOXES_GET_Result();
            using (DBEntities db = new DBEntities()) {
                info = db.SET_CLIENT_DEPT_BOXES_GET(id, null, null, null, null, null, null, null, null, null, null, null, null).FirstOrDefault();
            }
            return View("Save", info);
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult Delete(int id = 0) {
            bool is_saved = false;
            string str_error = "";

            try {
                using (DBEntities db = new DBEntities()) {
                    db.SET_CLIENT_DEPT_BOXES_DELETE(id);
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


        //public ActionResult ClientBoxTransmitalHistory(int BOX_NO = 0)
        //{

        //    List<GET_CLIENT_BOX_TRANSMIT_HISTORY_Result> client_box_trans_history_list = new List<GET_CLIENT_BOX_TRANSMIT_HISTORY_Result>();

        //    using (DBEntities db = new DBEntities())
        //    {
        //        client_box_trans_history_list = db.GET_CLIENT_BOX_TRANSMIT_HISTORY(BOX_NO).ToList();
        //    }

        //    if (client_box_trans_history_list == null)
        //    {
        //        client_box_trans_history_list = new List<GET_CLIENT_BOX_TRANSMIT_HISTORY_Result>();
        //    }

        //    return View("TransmitalHistoryList", client_box_trans_history_list);

        //}

    }
}