using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_WEB.Models;


namespace DMS_WEB.Controllers
{
    public class ReferenceLoaderController : Controller
    {
        //
        // GET: /ReferenceLoader/

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetDepartmentList(DepartmentSearch item) {
            List<SET_CLIENT_DEPARTMENTS_GET_Result> dept_list = new List<SET_CLIENT_DEPARTMENTS_GET_Result>();

            using (DBEntities db = new DBEntities()) {
                dept_list = db.SET_CLIENT_DEPARTMENTS_GET(item.DEPT_NO, item.CLIENT_NO, item.DEPT_NAME).ToList();
            }

            var ret_dept_list = (from d in dept_list  
                                 select new {
                                     @DEPT_NO = d.DEPT_NO ,
                                     @CLIENT_NO = d.CLIENT_NO,
                                     @CLIENT_NAME = d.CLIENT_NAME,
                                     @CLIENT_ALIAS = d.CLIENT_ALIAS,
                                     @DEPT_NAME = d.DEPT_NAME,
                                     @DEPT_ADDR = d.DEPT_ADDR,
                                     @DEPT_CONTACT = d.DEPT_CONTACT,
                                     @DEPT_EMAIL = d.DEPT_EMAIL,
                                     @DELIVERY_LOCATION = d.DELIVERY_LOCATION,
                                     @CONTACT_PERSON = d.CONTACT_PERSON,
                                     @CONTACT_ADDR = d.CONTACT_ADDR,
                                     @DESIGNATION = d.DESIGNATION,
                                     @ATTENTION = d.ATTENTION,
                                     @IS_NEW_TRANSMIT = d.IS_NEW_TRANSMIT,
                                     @IS_AUTO_ID = d.IS_AUTO_ID,
                                     @NEXT_ID = d.NEXT_ID,
                                     @ID_LENGHT = d.ID_LENGHT,
                                     @ID_PREFIX = d.ID_PREFIX,                                     
                                 }
                );

            return Json(ret_dept_list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransmittal(TransmittalSearch item) {
            List<TRN_TRANSMITTALS_GET_Result> transmittal_list = new List<TRN_TRANSMITTALS_GET_Result>();

            using (DBEntities db = new DBEntities()) {
                transmittal_list = db.TRN_TRANSMITTALS_GET(item.TRANSMIT_NO, item.TRN_TYPE_NO, item.CLIENT_NO, item.DEPT_NO, item.TRANSMIT_ID, item.START_INDEX, item.END_INDEX).ToList();
            }

            var ret_transmittal_list = (from d in transmittal_list
                                 select new {
                                     @TRANSMIT_NO = d.TRANSMIT_NO,
                                     @TRN_TYPE_NO = d.TRN_TYPE_NO, 
                                     @CLIENT_NO = d.CLIENT_NO,
                                     @DEPT_NO = d.DEPT_NO,
                                     @TRANSMIT_ID = d.TRANSMIT_ID,
                                     @TRN_DATE = d.TRN_DATE,
                                     @PROD_TYPE_NO = d.PROD_TYPE_NO,
                                     @REMARKS = d.REMARKS,
                                     @REASON_NO = d.REASON_NO,
                                     @REQUESTER_NO = d.REQUESTER_NO,
                                     @IS_NEW_TRANSMIT = d.IS_NEW_TRANSMIT,
                                     @IS_OLD_DATA = d.IS_OLD_DATA,
                                 }
                );

            return Json(ret_transmittal_list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetChallan(ChallanSearch item) {
            List<TRN_CHALLANS_GET_Result> challan_list = new List<TRN_CHALLANS_GET_Result>();

            using (DBEntities db = new DBEntities()) {
                challan_list = db.TRN_CHALLANS_GET(item.CHALLAN_NO, item.CLIENT_NO, item.DEPT_NO, item.CHALLAN_ID, item.TRN_TYPE_NO, item.REQ_TYPE_NO, item.TRANSMIT_NO, item.IS_OLD_DATA, item.START_INDEX, item.END_INDEX).ToList();
            }

            
            var ret_challan_list = (from d in challan_list
                                        select new {
                                            @CHALLAN_NO = d.CHALLAN_NO,
                                            @CHALLAN_ID = d.CHALLAN_ID,
                                            @REC_DATE = Utility.GetDateString(d.REC_DATE, "dd/MM/yyyy"),
                                            @TRN_TYPE_NO = d.TRN_TYPE_NO,
                                            @CLIENT_NO = d.CLIENT_NO,
                                            @DEPT_NO = d.DEPT_NO,
                                            @IS_OLD_DATA = d.IS_OLD_DATA,
                                        }
                );

            return Json(ret_challan_list, JsonRequestBehavior.AllowGet);
            
        }

        public JsonResult GetRecallBox(RecallBoxSearch item) {
            List<SET_RECALL_BOXES_GET_Result> recall_box_list = new List<SET_RECALL_BOXES_GET_Result>();

            using (DBEntities db = new DBEntities())
            {
                recall_box_list = db.SET_RECALL_BOXES_GET(item.RECALL_BOX_NO, item.CLIENT_NO, item.DEPT_NO, item.RECALL_BOX_ID, item.START_INDEX, item.END_INDEX).ToList();
            }


            var ret_recall_box_list = (from d in recall_box_list
                                    select new
                                    {
                                        @RECALL_BOX_NO = d.RECALL_BOX_NO,
                                        @CLIENT_NO = d.CLIENT_NO,                                        
                                        @DEPT_NO = d.DEPT_NO,
                                        @RECALL_BOX_ID = d.RECALL_BOX_ID,
                                        @RECALL_LOCATION = d.RECALL_LOCATION, 
                                    }
                );

            return Json(ret_recall_box_list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClientBox(ClientBoxesSearch item) {
            List<SET_CLIENT_DEPT_BOXES_GET_Result> box_list = new List<SET_CLIENT_DEPT_BOXES_GET_Result>();

            using (DBEntities db = new DBEntities()) {
                box_list = db.SET_CLIENT_DEPT_BOXES_GET(item.BOX_NO,item.CLIENT_NO, item.DEPT_NO, item.PROD_TYPE_NO, item.CLIENT_BOX_ID, item.CHALLAN_NO,item.TRN_TYPE_NO, item.REQ_TYPE_NO, item.TRANSMIT_NO, item.IS_OLD_DATA, item.RECALL_BOX_NO , item.START_INDEX, item.END_INDEX).ToList();
            }

            var ret_list = (from d in box_list
                                    select new {
                                        @BOX_NO = d.BOX_NO,
                                        @CLIENT_BOX_ID = d.CLIENT_BOX_ID,
                                        @ARCHIVE_DATE = Utility.GetDateString(d.ARCHIVE_DATE, "dd/MM/yyyy"),
                                        @TRN_TYPE_NO = d.TRN_TYPE_NO,
                                        @CLIENT_NO = d.CLIENT_NO,
                                        @DEPT_NO = d.DEPT_NO,
                                        @IS_OLD_DATA = d.IS_OLD_DATA,
                                    }
                );

            return Json(ret_list, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetRecallBoxIDByClientBoxId(ClientBoxesSearch item)
        {
            List<GET_RECALL_BOX_NO_BY_CLIENT_BOX_ID_Result> recall_box_no = new List<GET_RECALL_BOX_NO_BY_CLIENT_BOX_ID_Result>();

            using (DBEntities db = new DBEntities())
            {
                recall_box_no = db.GET_RECALL_BOX_NO_BY_CLIENT_BOX_ID(item.CLIENT_BOX_ID, item.CLIENT_NO).ToList();
            }


            var ret_recall_box_no = (from d in recall_box_no
                                       select new
                                       {
                                           @BOX_NO = d.BOX_NO,
                                           @RECALL_BOX_NO = d.RECALL_BOX_NO,
                                           @CLIENT_BOX_ID = d.CLIENT_BOX_ID,
                                           @CLIENT_NO = d.CLIENT_NO,
                                           @DEPT_NO = d.DEPT_NO,
                                           @RECALL_BOX_ID = d.RECALL_BOX_ID,
                                       }
                );

            return Json(ret_recall_box_no, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBatch(ClientBatchSearch item) {
            List<SET_CLIENT_BATCHES_GET_Result> data_list = new List<SET_CLIENT_BATCHES_GET_Result>();

            using (DBEntities db = new DBEntities()) {
                data_list = db.SET_CLIENT_BATCHES_GET(item.BATCH_NO, item.BATCH_ID, item.BOX_NO, item.CLIENT_BOX_ID, item.CLIENT_NO, item.ARCHIVE_DATE, item.TRN_TYPE_NO, item.REQ_TYPE_NO, item.TRANSMIT_NO, item.IS_OLD_DATA, item.START_INDEX, item.END_INDEX).ToList();
            }

            var ret_list = (from d in data_list
                            select new {
                                @BATCH_NO = d.BATCH_NO, 
                                @BATCH_ID = d.BATCH_ID, 
                                @BOX_NO = d.BOX_NO,
                                @CLIENT_BOX_ID = d.CLIENT_BOX_ID,
                                @ARCHIVE_DATE = Utility.GetDateString(d.ARCHIVE_DATE, "dd/MM/yyyy"),
                                @TRN_TYPE_NO = d.TRN_TYPE_NO,
                                @CLIENT_NO = d.CLIENT_NO,
                                @DEPT_NO = d.DEPT_NO,
                                @IS_OLD_DATA = d.IS_OLD_DATA,
                            }
                );

            return Json(ret_list, JsonRequestBehavior.AllowGet);
        }
    }
}
