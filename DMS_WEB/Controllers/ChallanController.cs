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
    public class ChallanController : BaseController {
        //
        // GET: /Challan/

        public ActionResult Index()
        {
            TRN_CHALLANS_GET_Result challan = new TRN_CHALLANS_GET_Result();           

            this.LoadCommonJsonData();
            return View(challan);
        }

        [HttpPost]
        public JsonResult Save(TRN_CHALLANS item) {

            //string UploadPath = ConfigurationManager.AppSettings["UploadPath"];
            bool is_saved = false;
            string str_error = "";

            //string[] allowdFile = { ".xls", ".xlsx" };

            //HttpPostedFileBase file = Request.Files["UPLOAD_FILE_NAME"];
            //string fileExtension = string.Empty;

            //if (file != null) {
            //    fileExtension = System.IO.Path.GetExtension(file.FileName);
            //    if (allowdFile.Contains(fileExtension)) {

            //        string directory_path = UploadPath + item.CLIENT_NO.ToString();
            //        bool is_directory_exists = Directory.Exists(Server.MapPath(directory_path));
            //        if (!is_directory_exists) {
            //            Directory.CreateDirectory(Server.MapPath(directory_path));
            //        }

                    int? CHALLAN_NO = 0;
                    try {

                        using (DBEntities db = new DBEntities())
                        {
                            if (item.CHALLAN_NO > 0)
                            {
                                db.TRN_CHALLANS_UPDATE(item.CHALLAN_NO, item.CLIENT_NO, item.DEPT_NO, item.CHALLAN_ID, item.REC_DATE, item.TRN_TYPE_NO, item.REQ_TYPE_NO, item.TRANSMIT_NO, item.LAST_TRN_DATE, item.IS_OLD_DATA, this.USER_NO, this.LOGON_NO);
                            }
                            else
                            {
                            
                                ObjectParameter CHALLAN_NO_parameter = new ObjectParameter("CHALLAN_NO", CHALLAN_NO);
                                db.TRN_CHALLANS_INSERT(CHALLAN_NO_parameter, item.CLIENT_NO, item.DEPT_NO, item.CHALLAN_ID, item.REC_DATE, item.TRN_TYPE_NO, item.REQ_TYPE_NO, item.TRANSMIT_NO, item.LAST_TRN_DATE, item.IS_OLD_DATA, this.USER_NO, this.LOGON_NO);
                                CHALLAN_NO = (int?)CHALLAN_NO_parameter.Value;
                            }
                        }
                        is_saved = true;
                        

                        //if (CHALLAN_NO > 0) {
                        //    is_saved = true;
                        //    //string FilePath = Server.MapPath(directory_path + @"/" + CHALLAN_NO.Value.ToString() + fileExtension);
                        //    //file.SaveAs(FilePath);
                        //}
                    } catch (Exception ex) {
                        str_error += Environment.NewLine + ex.Message;
                    }
                //}
            //}

            var ret_val = new { 
                @is_success = is_saved,  
                @msg = str_error,
            };

            return Json(ret_val, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Search(ChallanSearch obj) {

            ViewBag.PageNumber = 1;
            TempData["obj_ChallanSearch"] = obj;
            
            List<TRN_CHALLANS_GET_Result> challan_list = new List<TRN_CHALLANS_GET_Result>();
            obj.TotalRecords = 0;

            using (DBEntities db = new DBEntities()) {
                ObjectParameter CNT_parameter = new ObjectParameter("CNT", obj.TotalRecords);

                //obj.TotalRecords = (int) db.TRN_CHALLANS_GET_PAGED(obj.CHALLAN_NO, obj.CLIENT_NO, obj.DEPT_NO, obj.CHALLAN_ID, obj.TRN_TYPE_NO, obj.REQ_TYPE_NO, obj.TRANSMIT_NO, obj.IS_OLD_DATA).FirstOrDefault().Value;
                db.TRN_CHALLANS_GET_PAGED(CNT_parameter, obj.CHALLAN_NO, obj.CLIENT_NO, obj.DEPT_NO, obj.CHALLAN_ID, obj.TRN_TYPE_NO, obj.REQ_TYPE_NO, obj.TRANSMIT_NO, obj.IS_OLD_DATA);
                obj.TotalRecords = (long) CNT_parameter.Value;
                challan_list = db.TRN_CHALLANS_GET(obj.CHALLAN_NO, obj.CLIENT_NO, obj.DEPT_NO, obj.CHALLAN_ID, obj.TRN_TYPE_NO, obj.REQ_TYPE_NO, obj.TRANSMIT_NO, obj.IS_OLD_DATA, obj.START_INDEX, this.PAGE_SIZE * obj.PAGE_NUMBER).ToList();
            }

            if (challan_list == null) {
                challan_list = new List<TRN_CHALLANS_GET_Result>();
            }

            ViewBag.TotalRecords = obj.TotalRecords;
            ViewBag.PAGE_SIZE = this.PAGE_SIZE;

            TempData["challan_TotalRecords"] = obj.TotalRecords;
            TempData.Keep();
            return View("List", challan_list);

        }

        public ActionResult GetPaged(int PageNumber) {
            TempData.Keep();
            ChallanSearch obj = TempData["obj_ChallanSearch"] as ChallanSearch;

            if (PageNumber == 0) {
                PageNumber = 1;
            }

            int start_index = (this.PAGE_SIZE * (PageNumber - 1) + 1);

            List<TRN_CHALLANS_GET_Result> challan_list = new List<TRN_CHALLANS_GET_Result>();
            using (DBEntities db = new DBEntities()) {
                challan_list = db.TRN_CHALLANS_GET(obj.CHALLAN_NO, obj.CLIENT_NO, obj.DEPT_NO, obj.CHALLAN_ID, obj.TRN_TYPE_NO, obj.REQ_TYPE_NO, obj.TRANSMIT_NO, obj.IS_OLD_DATA, start_index, this.PAGE_SIZE * PageNumber).ToList();                
            }

            if (challan_list == null) {
                challan_list = new List<TRN_CHALLANS_GET_Result>();
            }

            /*
            if (((challan_list.Count == 0) && (PageNumber > 0)) || (challan_list.Count < this.PAGE_SIZE)) {
                PageNumber--;
                if (PageNumber <= 0) {
                    PageNumber = 1;
                }
            }
            */

            ViewBag.PageNumber = PageNumber;

            ViewBag.TotalRecords = TempData["challan_TotalRecords"];
            ViewBag.PAGE_SIZE = this.PAGE_SIZE;

            return View("List", challan_list);
        }

        public ActionResult Edit(int id = 0)
        {
            TRN_CHALLANS_GET_Result info = new TRN_CHALLANS_GET_Result();
            using (DBEntities db = new DBEntities())
            {
                info = db.TRN_CHALLANS_GET(id, null, null, null, null, null, null, null, null, null).FirstOrDefault();
            }
            return View("Save", info);
        }

    }
}
