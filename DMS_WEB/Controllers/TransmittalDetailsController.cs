using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_WEB.Models;

namespace DMS_WEB.Controllers
{
    public class TransmittalDetailsController : Controller
    {
        //
        // GET: /TransmittalDetails/

        public ActionResult Index()
        {
            TRN_TRANSMITTALS_GET_Result model = new TRN_TRANSMITTALS_GET_Result();
            return View(model);
        }

        //[HttpPost]
        //public ActionResult Box_Transmital_history_Search(string TRANSMITTAL_ID = null)
        //{
        //    List<GET_CLIENT_BOX_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result> client_box_transmit_history_details = new List<GET_CLIENT_BOX_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result>();

        //    using (DBEntities db = new DBEntities())
        //    {
        //        client_box_transmit_history_details = db.GET_CLIENT_BOX_TRANSMIT_DETAIL_BY_TRANSMIT_ID(TRANSMITTAL_ID).ToList();
        //    }

        //    if (client_box_transmit_history_details == null)
        //    {
        //        client_box_transmit_history_details = new List<GET_CLIENT_BOX_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result>();
        //    }
        //    return View("BoxList", client_box_transmit_history_details);
        //}

        //[HttpPost]
        //public ActionResult Account_Transmital_history_Search(string TRANSMITTAL_ID = null)
        //{
        //    List<GET_CLIENT_ACCOUNT_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result> client_account_transmit_history_details = new List<GET_CLIENT_ACCOUNT_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result>();

        //    using (DBEntities db = new DBEntities())
        //    {
        //            client_account_transmit_history_details = db.GET_CLIENT_ACCOUNT_TRANSMIT_DETAIL_BY_TRANSMIT_ID(TRANSMITTAL_ID).ToList();
        //    }

        //    if (client_account_transmit_history_details == null)
        //    {
        //        client_account_transmit_history_details = new List<GET_CLIENT_ACCOUNT_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result>();
        //    }
        //    return View("AccountList", client_account_transmit_history_details);
        //}

        //[HttpPost]
        //public ActionResult Account__document_Transmital_history_Search(string TRANSMITTAL_ID = null)
        //{
        //    List<GET_CLIENT_ACCOUNT_DOC_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result> client_account_document_transmit_history_details = new List<GET_CLIENT_ACCOUNT_DOC_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result>();

        //    using (DBEntities db = new DBEntities())
        //    {
        //        client_account_document_transmit_history_details = db.GET_CLIENT_ACCOUNT_DOC_TRANSMIT_DETAIL_BY_TRANSMIT_ID(TRANSMITTAL_ID).ToList();
        //    }

        //    if (client_account_document_transmit_history_details == null)
        //    {
        //        client_account_document_transmit_history_details = new List<GET_CLIENT_ACCOUNT_DOC_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result>();
        //    }
        //    return View("AccountDocumentList", client_account_document_transmit_history_details);
        //}

        [HttpPost]
        public ActionResult Search(string Transmitall_ID = null)
        {

            //List<GET_CLIENT_BOX_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result> client_box_transmit_history_details = new List<GET_CLIENT_BOX_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result>();
            //List<GET_CLIENT_ACCOUNT_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result> client_account_transmit_history_details = new List<GET_CLIENT_ACCOUNT_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result>();
            //List<GET_CLIENT_ACCOUNT_DOC_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result> client_account_document_transmit_history_details = new List<GET_CLIENT_ACCOUNT_DOC_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result>();

            TransmitalDetailsViewModel model = new TransmitalDetailsViewModel();


            using (DBEntities db = new DBEntities())
            {
                model.client_box_transmit_history_details = db.GET_CLIENT_BOX_TRANSMIT_DETAIL_BY_TRANSMIT_ID(Transmitall_ID).ToList();
                model.client_account_transmit_history_details = db.GET_CLIENT_ACCOUNT_TRANSMIT_DETAIL_BY_TRANSMIT_ID(Transmitall_ID).ToList();
                model.client_account_document_transmit_history_details = db.GET_CLIENT_ACCOUNT_DOC_TRANSMIT_DETAIL_BY_TRANSMIT_ID(Transmitall_ID).ToList();


            }

            if (model.client_box_transmit_history_details == null)
            {
                model.client_box_transmit_history_details = new List<GET_CLIENT_BOX_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result>();

            }

            if (model.client_account_transmit_history_details == null)
            {
                model.client_account_transmit_history_details = new List<GET_CLIENT_ACCOUNT_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result>();
            }

            if (model.client_account_document_transmit_history_details == null)
            {
                model.client_account_document_transmit_history_details = new List<GET_CLIENT_ACCOUNT_DOC_TRANSMIT_DETAIL_BY_TRANSMIT_ID_Result>();
            }

            return View("List", model);
        }

    }
}
