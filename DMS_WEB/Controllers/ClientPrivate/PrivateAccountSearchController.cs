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
    public class PrivateAccountSearchController : BaseController
    {
        //
        // GET: /PrivateAccountSearch/

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
        public ActionResult Search(ClientAccountSearch obj) 
        {
            this.LoadSecurity();

            obj.CLIENT_NO = this.user_info.CLIENT_NO;

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

        public ActionResult GetPaged(int PageNumber) 
        {
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

        public ActionResult ViewDetails(int ACC_NO = 0) 
        {
            this.LoadSecurity();

            Account_Document_Attach_ViewModel model = new Account_Document_Attach_ViewModel();

            using (DBEntities db = new DBEntities()) {
                if (ACC_NO > 0) {
                    model.account_info = db.SET_CLIENT_ACCOUNTS_GET_DETAILS(ACC_NO, this.user_info.CLIENT_NO.Value).FirstOrDefault();

                    if (model.account_info.ACC_NO.HasValue) {
                        model.document_list = db.SET_CLIENT_ACC_DOCS_GET(null, ACC_NO, null, null, null).ToList();

                        if (this.user_info.CAN_VIEW_IMAGE == true)
                        {
                            model.attach_list = db.SET_CLIENT_ATTACHS_GET(null, null, ACC_NO, null, null).ToList();
                        }
                    }

                }

                if (model.account_info == null) {
                    model.account_info = new SET_CLIENT_ACCOUNTS_GET_DETAILS_Result();
                }

                if (model.document_list == null) {
                    model.document_list = new List<SET_CLIENT_ACC_DOCS_GET_Result>();
                }
                if (model.attach_list == null) {
                    model.attach_list = new List<SET_CLIENT_ATTACHS_GET_Result>();
                }
            }

            return View(model);
        }


        public FilePathResult ViewImage(long ATTACH_NO = 0, short DEPT_NO = 0, string FILE_EXT = "", string FILE_TYPE = "")
        {
            this.LoadSecurity();

            short? CLIENT_NO = this.user_info.CLIENT_NO;

            string file_location = this.ACC_ATTACH_UPLOAD + CLIENT_NO.ToString() + "/" + DEPT_NO.ToString() + "/" + ATTACH_NO.ToString() + FILE_EXT;

            return File(file_location, FILE_TYPE);

            /*
            GET_IMAGE_BY_ATTACH_NO_Result attach_list = new GET_IMAGE_BY_ATTACH_NO_Result();

            using (DBEntities db = new DBEntities())
            {
                
                attach_list = db.GET_IMAGE_BY_ATTACH_NO(CLIENT_NO, ATTACH_NO).FirstOrDefault();

                string file_location = attach_list.FILE_LOCATION + "/" + ATTACH_NO.ToString() + attach_list.FILE_EXT;

                if (file_location != null)
                {
                    //return File(file_location, attach_list.FILE_TYPE, attach_list.ATTACH_NAME);
                    return File(file_location, attach_list.FILE_TYPE);
                    //return null;
                }
                else
                {
                    return null;
                }
            }
            */
        }

        //[HttpPost]
        public FilePathResult Imagelog(long ATTACH_NO = 0)
        {

            this.LoadSecurity();

            short? CLIENT_NO = this.user_info.CLIENT_NO;

            GET_IMAGE_BY_ATTACH_NO_Result attach_list = new GET_IMAGE_BY_ATTACH_NO_Result();

            using (DBEntities db = new DBEntities())
            {

                attach_list = db.GET_IMAGE_BY_ATTACH_NO(CLIENT_NO, ATTACH_NO).FirstOrDefault();

                string file_location = attach_list.FILE_LOCATION + "/" + ATTACH_NO.ToString() + attach_list.FILE_EXT;

                if (file_location != null)
                {
                    if (this.user_info.USER_TYPE_NO == 400)
                    {
                        db.SET_CLIENT_IMAGE_DETAILS_INSERT(this.USER_NO, CLIENT_NO, ATTACH_NO, attach_list.DOC_NO,
                      this.LOGON_NO);
                    } 
                    //return File(file_location, attach_list.FILE_TYPE, attach_list.ATTACH_NAME);
                    return File(file_location, attach_list.FILE_TYPE);
                    //return null;
                }
                else
                {
                    return null;
                }
            }

            /*
            this.LoadSecurity();

            List<GET_ALL_IMAGE_URL_Result> allImageUrl = new List<GET_ALL_IMAGE_URL_Result>();

            string image_path = null;

            using (DBEntities db = new DBEntities())
            {
                allImageUrl = db.GET_ALL_IMAGE_URL().ToList();

                foreach (var getImageUrl in allImageUrl)
                {
                    image_path = getImageUrl.IMAGE_URL.Replace("~/", "../");

                    if (IMAGE == image_path)
                    {
                        if (this.user_info.USER_TYPE_NO == 400)
                        {
                            db.SET_CLIENT_IMAGE_DETAILS_INSERT(this.USER_NO, getImageUrl.ATTACH_NO, getImageUrl.DOC_NO,
                          this.LOGON_NO);
                            break;
                        }                      
                    }
                    else
                    {
                        //allImage.IMAGE_URL = null;
                    }
                }
            }
            return View();
            */
        }
    }
}
