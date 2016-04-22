using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_WEB.Models;
using System.Data.Objects;
using System.Configuration;
using System.Web.Script.Serialization;

namespace DMS_WEB.Controllers
{
    public class BaseController : Controller
    {

        protected SEC_USERS_GET_Result user_info = null;
        protected short? USER_NO = null;
        protected long? LOGON_NO = null;

        protected int PAGE_SIZE = int.Parse(ConfigurationManager.AppSettings["PAGE_SIZE"]);

        protected string UploadPath = ConfigurationManager.AppSettings["UploadPath"].Trim();
        protected string ACC_ATTACH_UPLOAD = ConfigurationManager.AppSettings["ACC_ATTACH_UPLOAD"].Trim();        
        protected string ExportPath = ConfigurationManager.AppSettings["ExportPath"].Trim();
        protected string DBName = ConfigurationManager.AppSettings["DBName"].Trim();

        protected string LOC_SEPARATOR = ConfigurationManager.AppSettings["LOC_SEPARATOR"].Trim();
        protected string APP_NAME = ConfigurationManager.AppSettings["APP_NAME"].Trim();
        protected string APP_VER = ConfigurationManager.AppSettings["APP_VER"].Trim();

        protected string THUMB_HEIGHT = ConfigurationManager.AppSettings["THUMB_HEIGHT"].Trim();
        protected string THUMB_WIDTH = ConfigurationManager.AppSettings["THUMB_WIDTH"].Trim();
        protected string FULL_HEIGHT = ConfigurationManager.AppSettings["FULL_HEIGHT"].Trim();
        protected string FULL_WIDTH = ConfigurationManager.AppSettings["FULL_WIDTH"].Trim();



        DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public BaseController() {

        }

        protected void LoadSecurity() {
            try {
                this.user_info = Session["sess_user_info"] as SEC_USERS_GET_Result;
                this.USER_NO = short.Parse(Session["sess_USER_NO"].ToString());
                this.LOGON_NO = long.Parse(Session["sess_LOGON_NO"].ToString());
            } catch (Exception ex) {
            }
        }

        protected string GetTime(DateTime date_val) {
            return (date_val - Jan1st1970).TotalMilliseconds.ToString();
        }

        protected void LoadCommonJsonData() {
            List<SET_CLIENTS_GET_Result> client_list = new List<SET_CLIENTS_GET_Result>();
            List<SET_CLIENT_DEPARTMENTS_GET_Result> dept_list = new List<SET_CLIENT_DEPARTMENTS_GET_Result>();
            List<SET_PRODUCT_TYPES_GET_Result> product_type_list = new List<SET_PRODUCT_TYPES_GET_Result>();
            List<GEN_TRANSMIT_TYPES_GET_Result> trans_type_list = new List<GEN_TRANSMIT_TYPES_GET_Result>();
            List<GEN_TRANSMIT_REQUEST_TYPES_GET_Result> req_type_list = new List<GEN_TRANSMIT_REQUEST_TYPES_GET_Result>();
            List<SET_DOCS_TYPES_GET_Result> doc_type_list = new List<SET_DOCS_TYPES_GET_Result>();
            List<SET_REQUESTERS_GET_Result> requesters_list = new List<SET_REQUESTERS_GET_Result>();

            using (DBEntities db = new DBEntities()) {
                client_list = db.SET_CLIENTS_GET(null, true).ToList();
                dept_list = db.SET_CLIENT_DEPARTMENTS_GET(null, null, null).ToList();
                product_type_list = db.SET_PRODUCT_TYPES_GET(null, null, null).ToList();
                trans_type_list = db.GEN_TRANSMIT_TYPES_GET(null, null, null).ToList();
                req_type_list = db.GEN_TRANSMIT_REQUEST_TYPES_GET(null, null, null).ToList();

                doc_type_list = db.SET_DOCS_TYPES_GET(null,null,null).ToList();
                requesters_list = db.SET_REQUESTERS_GET(null).ToList();
            }

            if (client_list == null) {
                client_list = new List<SET_CLIENTS_GET_Result>();
            }

            var setup_info = new {
                @client_list = client_list,
                @dept_list = dept_list,
                @product_type_list = product_type_list,
                @trans_type_list = trans_type_list,
                @req_type_list = req_type_list,

                @doc_type_list = doc_type_list.ToList(),
                @requesters_list = requesters_list,
            };

            ViewBag.setup_info = new JavaScriptSerializer().Serialize(setup_info);
        }

    }
}
