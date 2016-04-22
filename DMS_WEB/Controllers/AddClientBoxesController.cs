using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_WEB.Models;

namespace DMS_WEB.Controllers
{
    public class AddClientBoxesController : BaseController
    {
        //
        // GET: /AddClientBoxes/

        public ActionResult Index(int RECALL_BOX_NO = 0, int CLIENT_NO = 0, int BOX_NO = 0)
        {
            this.LoadCommonJsonData();

            AddClientBoxViewModel model = new AddClientBoxViewModel();
            using (DBEntities db = new DBEntities())
            {
                if (RECALL_BOX_NO > 0)
                {
                    model.recall_box_info = db.SET_CLIENT_DEPT_RECALL_BOXES_GET_DETAILS(Convert.ToInt32(RECALL_BOX_NO), null, null).FirstOrDefault();
                    model.recall_box_dept_list =
                        db.SET_CLIENT_DEPT_BOXES_GET(null, Convert.ToSByte(CLIENT_NO), null, null, null, null, null, null, null, null,
                            Convert.ToInt32(RECALL_BOX_NO), null, null).ToList();
                }

                if (BOX_NO > 0)
                {
                    model.recall_box_dept_info =
                        db.SET_CLIENT_DEPT_BOXES_GET(null, null, null, null, null, null, null, null, null, null,
                            null, null, null).FirstOrDefault();
                }
            }
            if (model.recall_box_info == null)
            {
                model.recall_box_info = new SET_CLIENT_DEPT_RECALL_BOXES_GET_DETAILS_Result();
            }
            if (model.recall_box_dept_list == null)
            {
                model.recall_box_dept_list = new List<SET_CLIENT_DEPT_BOXES_GET_Result>();
            }
            if (model.recall_box_dept_info == null)
            {
                model.recall_box_dept_info = new SET_CLIENT_DEPT_BOXES_GET_Result();
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult Save(SET_CLIENT_DEPT_BOXES item)
        {
            bool is_saved = false;
            string str_error = "";

            try
            {
                GET_RECALL_BOX_NO_BY_CLIENT_BOX_ID_Result box_no_by_client_box_id = new GET_RECALL_BOX_NO_BY_CLIENT_BOX_ID_Result();
                using (DBEntities db = new DBEntities())
                {
                    box_no_by_client_box_id = db.GET_RECALL_BOX_NO_BY_CLIENT_BOX_ID(item.CLIENT_BOX_ID, item.CLIENT_NO).FirstOrDefault();

                        if (box_no_by_client_box_id.CLIENT_NO == item.CLIENT_NO)
                        {
                            db.SET_CLIENT_DEPT_BOXES_RECALL_UPDATE(item.CLIENT_NO, box_no_by_client_box_id.DEPT_NO, item.CLIENT_BOX_ID, item.RECALL_BOX_NO, this.USER_NO, this.LOGON_NO);
                            is_saved = true;
                        }
                        else
                        {
                            is_saved = false;   
                        }                         
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
            };

            return Json(ret_val, JsonRequestBehavior.AllowGet);
        }


        public ActionResult List(long RECALL_BOX_NO = 0, long CLIENT_NO = 0)
        {

            List<SET_CLIENT_DEPT_BOXES_GET_Result> doc_list = new List<SET_CLIENT_DEPT_BOXES_GET_Result>();
            using (DBEntities db = new DBEntities())
            {
                doc_list = db.SET_CLIENT_DEPT_BOXES_GET(null, Convert.ToSByte(CLIENT_NO), null, null, null, null, null, null, null, null,
                            Convert.ToInt32(RECALL_BOX_NO), null, null).ToList();
            }

            if (doc_list == null)
            {
                doc_list = new List<SET_CLIENT_DEPT_BOXES_GET_Result>();
            }

            return View("List", doc_list);
        }


        [HttpPost, ActionName("Delete")]
        public JsonResult Delete(int id = 0, short client_no = 0, short dept_no = 0, string client_box_id = null)
        {
            bool is_saved = false;
            string str_error = "";

            try
            {
                using (DBEntities db = new DBEntities())
                {
                    db.SET_CLIENT_DEPT_BOXES_RECALL_DELETE(client_no, dept_no, client_box_id, id, this.USER_NO,
                        this.LOGON_NO);
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

            ViewBag.Recall_box_no = id;
            ViewBag.client_no = client_no;

            return Json(ret_val, JsonRequestBehavior.AllowGet);
        }
    }
}
