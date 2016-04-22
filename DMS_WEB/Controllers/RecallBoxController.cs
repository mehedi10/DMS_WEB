using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_WEB.Models;
using System.Data.Objects;

namespace DMS_WEB.Controllers
{
    public class RecallBoxController : BaseController
    {
        //
        // GET: /RecallBox/

        public ActionResult Index(int id =0)
        {
            SET_CLIENT_DEPT_RECALL_BOXES_GET_DETAILS_Result model = new SET_CLIENT_DEPT_RECALL_BOXES_GET_DETAILS_Result();

            this.LoadCommonJsonData();
            if (id > 0)
            {
                using (DBEntities db = new DBEntities())
                {
                    model = db.SET_CLIENT_DEPT_RECALL_BOXES_GET_DETAILS(Convert.ToInt32(id), null, null).FirstOrDefault();
                }
            }
            if (model == null)
            {
                model = new SET_CLIENT_DEPT_RECALL_BOXES_GET_DETAILS_Result();
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Save(SET_RECALL_BOXES item)
        {
            bool is_saved = false;
            string str_error = "";

            int? RECALL_BOX_NO = 0;

            try
            {
                using (DBEntities db = new DBEntities())
                {
                    if (item.RECALL_BOX_NO > 0)
                    {
                        db.SET_RECALL_BOXES_UPDATE(item.RECALL_BOX_NO, item.CLIENT_NO, item.DEPT_NO, item.RECALL_BOX_ID, item.RECALL_LOCATION, this.USER_NO, this.LOGON_NO);
                    }
                    else
                    {
                        ObjectParameter RECALL_BOX_NO_parameter = new ObjectParameter("RECALL_BOX_NO", RECALL_BOX_NO);
                        db.SET_RECALL_BOXES_INSERT(RECALL_BOX_NO_parameter, item.CLIENT_NO, item.DEPT_NO, item.RECALL_BOX_ID, item.RECALL_LOCATION, this.USER_NO, this.LOGON_NO);
                        RECALL_BOX_NO = (int?)RECALL_BOX_NO_parameter.Value;
                    }
                    is_saved = true;
                }
            }
            catch (Exception ex)
            {
                str_error += Environment.NewLine + ex.Message;
            }

            var ret_val = new
            {
                @is_success = is_saved,
                @msg = str_error,
                @id = RECALL_BOX_NO,
            };

            return Json(ret_val, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Search(RecallBoxSearch obj)
        {

            ViewBag.PageNumber = 1;
            TempData["obj_RecallBoxSearch"] = obj;

            List<SET_RECALL_BOXES_GET_Result> recall_box_list = new List<SET_RECALL_BOXES_GET_Result>();
            obj.TotalRecords = 0;

            using (DBEntities db = new DBEntities())
            {
                ObjectParameter RBT_parameter = new ObjectParameter("RBT", obj.TotalRecords);
                db.SET_RECALL_BOXES_GET_PAGED(RBT_parameter, obj.RECALL_BOX_NO, obj.CLIENT_NO, obj.DEPT_NO,
                    obj.RECALL_BOX_ID);
                obj.TotalRecords = (long)RBT_parameter.Value;
                recall_box_list =
                    db.SET_RECALL_BOXES_GET(obj.RECALL_BOX_NO, obj.CLIENT_NO, obj.DEPT_NO, obj.RECALL_BOX_ID,
                        obj.START_INDEX, this.PAGE_SIZE*obj.PAGE_NUMBER).ToList();
            }

            if (recall_box_list == null)
            {
                recall_box_list = new List<SET_RECALL_BOXES_GET_Result>();
            }

            ViewBag.TotalRecords = obj.TotalRecords;
            ViewBag.PAGE_SIZE = this.PAGE_SIZE;

            TempData["recall_box_TotalRecords"] = obj.TotalRecords;
            TempData.Keep();
            return View("List", recall_box_list);
        }

        public ActionResult GetPaged(int PageNumber)
        {
            TempData.Keep();
            RecallBoxSearch obj = TempData["obj_RecallBoxSearch"] as RecallBoxSearch;

            if (PageNumber == 0)
            {
                PageNumber = 1;
            }

            int start_index = (this.PAGE_SIZE * (PageNumber - 1) + 1);

            List<SET_RECALL_BOXES_GET_Result> recall_box_list = new List<SET_RECALL_BOXES_GET_Result>();
            using (DBEntities db = new DBEntities())
            {
                recall_box_list = db.SET_RECALL_BOXES_GET(obj.RECALL_BOX_NO, obj.CLIENT_NO, obj.DEPT_NO, obj.RECALL_BOX_ID,
                       start_index, this.PAGE_SIZE * PageNumber).ToList();
            }

            if (recall_box_list == null)
            {
                recall_box_list = new List<SET_RECALL_BOXES_GET_Result>();
            }

            ViewBag.PageNumber = PageNumber;

            ViewBag.TotalRecords = TempData["recall_box_TotalRecords"];
            ViewBag.PAGE_SIZE = this.PAGE_SIZE;

            return View("List", recall_box_list);
        }


        public ActionResult Edit(int id = 0)
        {
            SET_CLIENT_DEPT_RECALL_BOXES_GET_DETAILS_Result info = new SET_CLIENT_DEPT_RECALL_BOXES_GET_DETAILS_Result();

            using (DBEntities db = new DBEntities())
            {
                info = db.SET_CLIENT_DEPT_RECALL_BOXES_GET_DETAILS(id, null, null).FirstOrDefault();
            }

            return View("Save", info);
        }

        [HttpPost, ActionName("Delete")]
        public JsonResult Delete(int id = 0)
        {
            bool is_saved = false;
            string str_error = "";

            try
            {
                using (DBEntities db = new DBEntities())
                {
                    db.SET_RECALL_BOXES_DELETE(id);
                }
                is_saved = true;
            }
            catch (Exception ex)
            {
                str_error += Environment.NewLine + ex.Message;
            }

            var ret_val = new
            {
                @is_success = is_saved,
                @msg = str_error,
            };

            return Json(ret_val, JsonRequestBehavior.AllowGet);
        }


    }
}
