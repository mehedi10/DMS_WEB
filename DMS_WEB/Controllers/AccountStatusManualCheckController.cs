using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_WEB.Models;

namespace DMS_WEB.Controllers
{
    public class AccountStatusManualCheckController : BaseController
    {
        //
        // GET: /AccountStatusManualCheck/

        public ActionResult Index()
        {
            this.LoadCommonJsonData();

            Session["sess_Manual_account_list"] = null; 

            List<SET_CLIENTS_GET_Result> client_list = new List<SET_CLIENTS_GET_Result>();

            using (DBEntities db = new DBEntities())
            {
                client_list = db.SET_CLIENTS_GET(null, true).ToList();
            }

            if (client_list == null)
            {
                client_list = new List<SET_CLIENTS_GET_Result>();
            }

            ViewBag.CLIENT_NO = new SelectList(client_list, "CLIENT_NO", "CLIENT_NAME");
            return View();
        }

        [HttpPost]
        public ActionResult Search(AccountStatusSearch obj)
        {
            TempData["obj_AccountStatusSearch"] = obj;

            List<SET_ACCOUNT_STATUS_CHECK_Manual_Result> accountStatusList_Manual = new List<SET_ACCOUNT_STATUS_CHECK_Manual_Result>();

            using (DBEntities db = new DBEntities())
            {
                string myString = obj.ACC_ID.Replace('\n', ',');
                accountStatusList_Manual = db.SET_ACCOUNT_STATUS_CHECK_Manual(obj.CLIENT_NO, myString).ToList();

                Session["sess_Manual_account_list"] = myString;
                Session["sess_account_status_list"] = accountStatusList_Manual;
                Session["CLIENT_NO"] = obj.CLIENT_NO;
                //Session["REQ_TYPE_NO"] = obj.REQ_TYPE_NO;
            }

            if (accountStatusList_Manual == null)
            {
                accountStatusList_Manual = new List<SET_ACCOUNT_STATUS_CHECK_Manual_Result>();
            }
            return View("List", accountStatusList_Manual);

        }

        public ActionResult ShowManualDetails(short TRN_TYPE_NO = 0)
        {

            string myStringList = Session["sess_Manual_account_list"].ToString();
            //String CLIENT_Num = Session["CLIENT_NO"].ToString();
            //String REQ_TYPE_Num = Session["REQ_TYPE_NO"].ToString();

            int CLIENT_NO = Convert.ToInt32(Session["CLIENT_NO"].ToString());
            //int REQ_TYPE_NO = Convert.ToInt32(Session["REQ_TYPE_NO"].ToString());

            List<SET_ACCOUNT_STATUS_CHECK_MANUAL_BY_TRN_TYPE_NO_Result> accountStatusListByTrnTypeNo = new List<SET_ACCOUNT_STATUS_CHECK_MANUAL_BY_TRN_TYPE_NO_Result>();

            using (DBEntities db = new DBEntities())
            {
                accountStatusListByTrnTypeNo = db.SET_ACCOUNT_STATUS_CHECK_MANUAL_BY_TRN_TYPE_NO(CLIENT_NO, TRN_TYPE_NO, myStringList).ToList();
            }

            if (accountStatusListByTrnTypeNo == null)
            {
                accountStatusListByTrnTypeNo = new List<SET_ACCOUNT_STATUS_CHECK_MANUAL_BY_TRN_TYPE_NO_Result>();
            }

            return View("ShowManualDetails", accountStatusListByTrnTypeNo);
        }

        public ActionResult AccountStatusList()
        {

            List<SET_ACCOUNT_STATUS_CHECK_Manual_Result> accountStatusList_Manual = new List<SET_ACCOUNT_STATUS_CHECK_Manual_Result>();

            accountStatusList_Manual =
                Session["sess_account_status_list"] as List<SET_ACCOUNT_STATUS_CHECK_Manual_Result>;

            return View("AccountListByTRNType", accountStatusList_Manual);
        }

    }
}
