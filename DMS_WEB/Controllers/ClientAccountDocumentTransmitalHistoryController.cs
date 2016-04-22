using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_WEB.Models;

namespace DMS_WEB.Controllers
{
    public class ClientAccountDocumentTransmitalHistoryController : Controller
    {
        //
        // GET: /ClientAccountDocumentTransmitalHistory/

        public ActionResult Index(int ACC_NO = 0, int DOC_NO = 0)
        {

            AccountDocumentTransmitalHistoryViewModel model = new AccountDocumentTransmitalHistoryViewModel();
            using (DBEntities db = new DBEntities())
            {
                if (ACC_NO > 0)
                {
                    model.account_doc_info = db.SET_CLIENT_ACC_DOCS_GET_DETAILS(DOC_NO, ACC_NO).FirstOrDefault();
                    model.transmit_history_list = db.GET_ACCOUNT_DOC_TRANSMIT_HISTORY(DOC_NO).ToList();
                }
            }
            if (model.account_doc_info == null)
            {
                model.account_doc_info = new SET_CLIENT_ACC_DOCS_GET_DETAILS_Result();
            }
            if (model.transmit_history_list == null)
            {
                model.transmit_history_list = new List<GET_ACCOUNT_DOC_TRANSMIT_HISTORY_Result>();
            }
            return View(model);
        }

    }
}
