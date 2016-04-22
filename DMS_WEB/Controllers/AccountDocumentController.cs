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


namespace DMS_WEB.Controllers {
    public class AccountDocumentController : BaseController {
        //
        // GET: /AccountDocument/

        public ActionResult Index(int ACC_NO = 0, long DOC_NO = 0) {
            this.LoadCommonJsonData();

            AccountDocumentViewModel model = new AccountDocumentViewModel();
            using (DBEntities db = new DBEntities()) {
                if (ACC_NO > 0) {
                    model.account_info = db.SET_CLIENT_ACCOUNTS_GET_DETAILS(ACC_NO, null).FirstOrDefault();
                    model.document_list = db.SET_CLIENT_ACC_DOCS_GET(null, ACC_NO, null, null, null).ToList(); 
                }
                if (DOC_NO > 0) {
                    model.document_info = db.SET_CLIENT_ACC_DOCS_GET(DOC_NO, null, null, null, null).FirstOrDefault();
                }
            }
            if (model.account_info == null) {
                model.account_info = new SET_CLIENT_ACCOUNTS_GET_DETAILS_Result();
            }
            if (model.document_list == null) {
                model.document_list = new List<SET_CLIENT_ACC_DOCS_GET_Result>();
            }
            if (model.document_info == null) {
                model.document_info = new SET_CLIENT_ACC_DOCS_GET_Result();
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Save(SET_CLIENT_ACC_DOCS item) {
            bool is_saved = false;
            string str_error = "";

            long? DOC_NO = 0;

            try {
                using (DBEntities db = new DBEntities()) {
                    if (item.DOC_NO > 0) {
                        DOC_NO = item.DOC_NO;
                        db.SET_CLIENT_ACC_DOCS_UPDATE(item.DOC_NO, item.ACC_NO, item.DOC_TYPE_NO, item.ARCHIVE_DATE, item.DOC_DETAILS, item.TRN_TYPE_NO, item.REQ_TYPE_NO, item.TRANSMIT_NO, item.LAST_TRN_DATE, item.IS_OLD_DATA, this.USER_NO, this.LOGON_NO);
                    } else {
                        ObjectParameter DOC_NO_parameter = new ObjectParameter("DOC_NO", DOC_NO);
                        db.SET_CLIENT_ACC_DOCS_INSERT(DOC_NO_parameter, item.ACC_NO, item.DOC_TYPE_NO, item.ARCHIVE_DATE, item.DOC_DETAILS, item.TRN_TYPE_NO, item.REQ_TYPE_NO, item.TRANSMIT_NO, item.LAST_TRN_DATE, item.IS_OLD_DATA, this.USER_NO, this.LOGON_NO);
                        DOC_NO = (long?)DOC_NO_parameter.Value;
                    }
                    is_saved = true;
                }
            } catch (Exception ex) {
                str_error += Environment.NewLine + ex.Message;
            }

            var ret_val = new {
                @is_success = is_saved,
                @msg = str_error,
                @id = DOC_NO,
            };

            return Json(ret_val, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Edit(long id = 0) {
            SET_CLIENT_ACC_DOCS_GET_Result info = new SET_CLIENT_ACC_DOCS_GET_Result();
            using (DBEntities db = new DBEntities()) {
                info = db.SET_CLIENT_ACC_DOCS_GET(id, null, null, null, null).FirstOrDefault();
            }
            return View("Save", info);
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult Delete(long id = 0) {
            bool is_saved = false;
            string str_error = "";

            try {
                using (DBEntities db = new DBEntities()) {
                    db.SET_CLIENT_ACC_DOCS_DELETE(id);
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

        public ActionResult List(long ACC_NO = 0) {

            List<SET_CLIENT_ACC_DOCS_GET_Result> doc_list = new List<SET_CLIENT_ACC_DOCS_GET_Result>();
            using(DBEntities db = new DBEntities()) {
                doc_list = db.SET_CLIENT_ACC_DOCS_GET(null, ACC_NO, null, null, null).ToList();
            }

            if (doc_list == null) {
                doc_list = new List<SET_CLIENT_ACC_DOCS_GET_Result>();
            }

            return View("List", doc_list);
        }


        //public ActionResult ClientDocumentTransmitalHistory(int DOC_NO = 0)
        //{

        //    List<GET_ACCOUNT_DOC_TRANSMIT_HISTORY_Result> client_doc_trans_history_list = new List<GET_ACCOUNT_DOC_TRANSMIT_HISTORY_Result>();

        //    using (DBEntities db = new DBEntities())
        //    {
        //        client_doc_trans_history_list = db.GET_ACCOUNT_DOC_TRANSMIT_HISTORY(DOC_NO).ToList();
        //    }

        //    if (client_doc_trans_history_list == null)
        //    {
        //        client_doc_trans_history_list = new List<GET_ACCOUNT_DOC_TRANSMIT_HISTORY_Result>();
        //    }

        //    return View("TransmitalHistoryList", client_doc_trans_history_list);

        //}
    }
}
