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
    public class DocumentAttachmentController : BaseController {
        //
        // GET: /DocumentAttachment/

        public ActionResult Index(int ACC_NO = 0, long DOC_NO = 0) {
            this.LoadCommonJsonData();

            AccountAttachmentViewModel model = new AccountAttachmentViewModel();

            using (DBEntities db = new DBEntities()) {
                if (ACC_NO > 0) {
                    model.account_info = db.SET_CLIENT_ACCOUNTS_GET_DETAILS(ACC_NO, null).FirstOrDefault();                    
                }
                if (DOC_NO > 0) {
                    model.document_info = db.SET_CLIENT_ACC_DOCS_GET(DOC_NO, null, null, null, null).FirstOrDefault();
                    model.attach_list = db.SET_CLIENT_ATTACHS_GET(null, DOC_NO, ACC_NO, null, null).ToList();
                }
            }

            if (model.account_info == null) {
                model.account_info = new SET_CLIENT_ACCOUNTS_GET_DETAILS_Result();
            }
            if (model.document_info == null) {
                model.document_info = new SET_CLIENT_ACC_DOCS_GET_Result();
            }
            if (model.attach_info == null) {
                model.attach_info = new SET_CLIENT_ATTACHS_GET_Result();
            }
            if (model.attach_list == null) {
                model.attach_list = new List<SET_CLIENT_ATTACHS_GET_Result>();
            }

            model.attach_info.CLIENT_NO = model.account_info.CLIENT_NO;
            model.attach_info.DEPT_NO = model.account_info.DEPT_NO;
            
            return View(model);
        }

        [HttpPost]
        public JsonResult Save(SET_CLIENT_ATTACHS_GET_Result item) 
        {

            //string UploadPath = ConfigurationManager.AppSettings["UploadPath"];
            //this.ACC_ATTACH_UPLOAD

            bool is_saved = false;
            string str_error = "";

            string[] allowdFile = { ".jpg", ".gif", ".tiff", ".bmp" };

            HttpPostedFileBase file = Request.Files["ATTACH_NAME"];
            string fileExtension = string.Empty;

            if (file != null) {
                fileExtension = System.IO.Path.GetExtension(file.FileName);
                if (allowdFile.Contains(fileExtension)) {

                    string directory_path = ACC_ATTACH_UPLOAD + item.CLIENT_NO.ToString() + @"/" + item.DEPT_NO.ToString();
                    bool is_directory_exists = Directory.Exists(Server.MapPath(directory_path));
                    if (!is_directory_exists) {
                        Directory.CreateDirectory(Server.MapPath(directory_path));
                    }

                    long? ATTACH_NO = 0;
                    try {
                        using (DBEntities db = new DBEntities()) {
                            ObjectParameter ATTACH_NO_parameter = new ObjectParameter("ATTACH_NO", ATTACH_NO);
                            db.SET_CLIENT_ATTACHS_INSERT(ATTACH_NO_parameter, item.DOC_NO, file.FileName, directory_path, file.ContentType, fileExtension);
                            ATTACH_NO = (long?)ATTACH_NO_parameter.Value;
                        }

                        if (ATTACH_NO > 0) {
                            is_saved = true;
                            string FilePath = Server.MapPath(directory_path + @"/" + ATTACH_NO.Value.ToString() + fileExtension);
                            file.SaveAs(FilePath);
                        }
                    } catch (Exception ex) {
                        str_error += Environment.NewLine + ex.Message;
                    }
                }
            }

            var ret_val = new {
                @is_success = is_saved,
                @msg = str_error,
            };

            return Json(ret_val, JsonRequestBehavior.AllowGet);
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult Delete(long id = 0) {
            bool is_saved = false;
            string str_error = "";

            SET_CLIENT_ATTACHS_GET_Result attach_info = new SET_CLIENT_ATTACHS_GET_Result();

            try {
                using (DBEntities db = new DBEntities()) {

                    attach_info = db.SET_CLIENT_ATTACHS_GET(id, null, null, null, null).FirstOrDefault();

                    string image_path = attach_info.FILE_LOCATION + @"/" + attach_info.ATTACH_NO.Value.ToString() + attach_info.FILE_EXT;

                    System.IO.File.Delete(Server.MapPath(image_path));

                    db.SET_CLIENT_ATTACHS_DELETE(id);
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

        public ActionResult List(long DOC_NO = 0) {
            List<SET_CLIENT_ATTACHS_GET_Result> attach_list = new List<SET_CLIENT_ATTACHS_GET_Result>();
            using (DBEntities db = new DBEntities()) {
                attach_list = db.SET_CLIENT_ATTACHS_GET(null, DOC_NO, null, null, null).ToList();
            }

            if (attach_list == null) {
                attach_list = new List<SET_CLIENT_ATTACHS_GET_Result>();
            }

            return View("List", attach_list);
        }

    }
}
