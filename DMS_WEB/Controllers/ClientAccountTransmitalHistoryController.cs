using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_WEB.Models;

namespace DMS_WEB.Controllers
{
    public class ClientAccountTransmitalHistoryController : Controller
    {
        //
        // GET: /ClientAccountTransmitalHistory/

        public ActionResult Index(int ACC_NO = 0)
        {
            AccountTransmitalHistoryViewModel model = new AccountTransmitalHistoryViewModel();
            using (DBEntities db = new DBEntities())
            {
                if (ACC_NO > 0)
                {
                    model.account_info = db.SET_CLIENT_ACCOUNTS_GET_DETAILS(ACC_NO, null).FirstOrDefault();
                    model.transmit_history_list = db.GET_ACCOUNT_TRANSMIT_HISTORY(ACC_NO).ToList();
                }
            }
            if (model.account_info == null)
            {
                model.account_info = new SET_CLIENT_ACCOUNTS_GET_DETAILS_Result();
            }
            if (model.transmit_history_list == null)
            {
                model.transmit_history_list = new List<GET_ACCOUNT_TRANSMIT_HISTORY_Result>();
            }
            return View(model);
        }

    }
}
