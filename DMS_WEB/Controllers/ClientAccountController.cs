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
    public class ClientAccountController : BaseController
    {
        //
        // GET: /ClientAccount/

        public ActionResult Index(int id=0)
        {
            SET_CLIENT_ACCOUNTS_GET_DETAILS_Result model = new SET_CLIENT_ACCOUNTS_GET_DETAILS_Result();
            
            this.LoadCommonJsonData();
            if (id > 0) {
                using (DBEntities db = new DBEntities()) {
                    model = db.SET_CLIENT_ACCOUNTS_GET_DETAILS(id, null).FirstOrDefault();
                }                
            }
            if (model == null) {
                model = new SET_CLIENT_ACCOUNTS_GET_DETAILS_Result();
            }
            return View(model);
        }


        [HttpPost]
        public JsonResult Save(SET_CLIENT_ACCOUNTS item) {
            bool is_saved = false;
            string str_error = "";

            long? ACC_NO = 0;

            try {
                using (DBEntities db = new DBEntities()) {
                    if (item.ACC_NO > 0) {
                        ACC_NO = item.ACC_NO;
                        db.SET_CLIENT_ACCOUNTS_UPDATE(item.ACC_NO, item.CLIENT_NO, item.DEPT_NO, item.BATCH_NO, item.ARCHIVE_DATE, item.ACC_ID, item.ACC_NAME, item.ACC_DETAILS, item.TRN_TYPE_NO, item.REQ_TYPE_NO, item.TRANSMIT_NO, item.LAST_TRN_DATE, item.IS_OLD_DATA, this.USER_NO, this.LOGON_NO);
                    } else {
                        ObjectParameter ACC_NO_parameter = new ObjectParameter("ACC_NO", ACC_NO);
                        db.SET_CLIENT_ACCOUNTS_INSERT(ACC_NO_parameter, item.CLIENT_NO, item.DEPT_NO, item.BATCH_NO, item.ARCHIVE_DATE, item.ACC_ID, item.ACC_NAME, item.ACC_DETAILS, item.TRN_TYPE_NO, item.REQ_TYPE_NO, item.TRANSMIT_NO, item.LAST_TRN_DATE, item.IS_OLD_DATA, this.USER_NO, this.LOGON_NO);
                        ACC_NO = (long?)ACC_NO_parameter.Value;
                    }
                    is_saved = true;
                }
            } catch (Exception ex) {
                str_error += Environment.NewLine + ex.Message;
            }

            var ret_val = new {
                @is_success = is_saved,
                @msg = str_error,
                @id = ACC_NO, 
            };

            return Json(ret_val, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Search(ClientAccountSearch obj) {

            ViewBag.PageNumber = 1;
            TempData["obj_ClientAccountSearch"] = obj;

            List<SET_CLIENT_ACCOUNTS_GET_Result> batch_list = new List<SET_CLIENT_ACCOUNTS_GET_Result>();
            obj.TotalRecords = 0;

            using (DBEntities db = new DBEntities()) {
                ObjectParameter CNT_parameter = new ObjectParameter("CNT", obj.TotalRecords);
                db.SET_CLIENT_ACCOUNTS_GET_PAGED(CNT_parameter, obj.ACC_NO, obj.CLIENT_NO, obj.DEPT_NO, obj.CLIENT_BOX_ID, obj.BATCH_NO, obj.BATCH_ID, obj.ARCHIVE_DATE, obj.ACC_ID, obj.TRN_TYPE_NO, obj.REQ_TYPE_NO, obj.TRANSMIT_NO, obj.IS_OLD_DATA);
                obj.TotalRecords = (long)CNT_parameter.Value;
                batch_list = db.SET_CLIENT_ACCOUNTS_GET(obj.ACC_NO, obj.CLIENT_NO, obj.DEPT_NO, obj.CLIENT_BOX_ID, obj.BATCH_NO, obj.BATCH_ID, obj.ARCHIVE_DATE, obj.ACC_ID, obj.TRN_TYPE_NO, obj.REQ_TYPE_NO, obj.TRANSMIT_NO, obj.IS_OLD_DATA, obj.START_INDEX, this.PAGE_SIZE * obj.PAGE_NUMBER).ToList();
            }

            if (batch_list == null) {
                batch_list = new List<SET_CLIENT_ACCOUNTS_GET_Result>();
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

            List<SET_CLIENT_ACCOUNTS_GET_Result> acc_list = new List<SET_CLIENT_ACCOUNTS_GET_Result>();
            using (DBEntities db = new DBEntities()) {
                acc_list = db.SET_CLIENT_ACCOUNTS_GET(obj.ACC_NO, obj.CLIENT_NO, obj.DEPT_NO, obj.CLIENT_BOX_ID, obj.BATCH_NO, obj.BATCH_ID, obj.ARCHIVE_DATE, obj.ACC_ID, obj.TRN_TYPE_NO, obj.REQ_TYPE_NO, obj.TRANSMIT_NO, obj.IS_OLD_DATA, start_index, this.PAGE_SIZE * PageNumber).ToList();
            }

            if (acc_list == null) {
                acc_list = new List<SET_CLIENT_ACCOUNTS_GET_Result>();
            }

            ViewBag.PageNumber = PageNumber;

            ViewBag.TotalRecords = TempData["batch_TotalRecords"];
            ViewBag.PAGE_SIZE = this.PAGE_SIZE;

            return View("List", acc_list);
        }

        public ActionResult Edit(int id = 0) {
            SET_CLIENT_ACCOUNTS_GET_DETAILS_Result info = new SET_CLIENT_ACCOUNTS_GET_DETAILS_Result();
            using (DBEntities db = new DBEntities()) {
                info = db.SET_CLIENT_ACCOUNTS_GET_DETAILS(id, null).FirstOrDefault();
            }
            return View("Save", info);
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult Delete(long id = 0) {
            bool is_saved = false;
            string str_error = "";

            try {
                using (DBEntities db = new DBEntities()) {
                    db.SET_CLIENT_ACCOUNTS_DELETE(id);
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


        //public ActionResult ClientAccountTransmitalHistory(int ACC_NO = 0)
        //{

        //    List<GET_ACCOUNT_TRANSMIT_HISTORY_Result> client_account_trans_history_list = new List<GET_ACCOUNT_TRANSMIT_HISTORY_Result>();

        //    using (DBEntities db = new DBEntities())
        //    {
        //        client_account_trans_history_list = db.GET_ACCOUNT_TRANSMIT_HISTORY(ACC_NO).ToList();
        //    }

        //    if (client_account_trans_history_list == null)
        //    {
        //        client_account_trans_history_list = new List<GET_ACCOUNT_TRANSMIT_HISTORY_Result>();
        //    }

        //    return View("TransmitalHistoryList", client_account_trans_history_list);

        //}

        

    }
}
