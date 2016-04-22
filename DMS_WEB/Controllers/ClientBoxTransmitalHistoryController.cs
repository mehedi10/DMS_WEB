using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_WEB.Models;

namespace DMS_WEB.Controllers
{
    public class ClientBoxTransmitalHistoryController : Controller
    {
        //
        // GET: /ClientBoxTransmitalHistory/

        public ActionResult Index(int BOX_NO = 0)
        {
            ClientBoxTransmitalHistoryViewModel model = new ClientBoxTransmitalHistoryViewModel();
            using (DBEntities db = new DBEntities())
            {
                if (BOX_NO > 0)
                {
                    model.client_box_info =
                        db.SET_CLIENT_DEPT_BOXES_GET(BOX_NO, null, null, null, null, null, null, null, null, null, null,
                            null, null).FirstOrDefault();
                    model.transmit_history_list = db.GET_CLIENT_BOX_TRANSMIT_HISTORY(BOX_NO).ToList();
                }
            }
            if (model.client_box_info == null)
            {
                model.client_box_info = new SET_CLIENT_DEPT_BOXES_GET_Result();
            }
            if (model.transmit_history_list == null)
            {
                model.transmit_history_list = new List<GET_CLIENT_BOX_TRANSMIT_HISTORY_Result>();
            }
            return View(model);
        }

    }
}
