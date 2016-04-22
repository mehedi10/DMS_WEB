using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_WEB.Models;

namespace DMS_WEB.Controllers
{
    public class ClientImageViewMonthlyController : BaseController
    {
        //
        // GET: /ClientImageViewMonthly/

        public ActionResult Index()
        {
            this.LoadSecurity();
            //SET_CLIENT_RPT_CLIENT_MONTHLY_IMAGE_VIEWS_GET_DETAILS_Result model = new SET_CLIENT_RPT_CLIENT_MONTHLY_IMAGE_VIEWS_GET_DETAILS_Result();
            this.LoadCommonJsonData();
            //return View(model);
            return View();
        }


        [HttpPost]
        public JsonResult Save(short CLIENT_NO = 0, int Year = 0, int Month = 0)
        {
            this.LoadSecurity();
            bool is_saved = false;
            string str_error = "";

            List<CHECK_FOR_QTY_IN_TRN_IMAGE_VIEW_Result> checkForQty = new List<CHECK_FOR_QTY_IN_TRN_IMAGE_VIEW_Result>(); 

            try
            {
                using (DBEntities db = new DBEntities())
                {
                    checkForQty = db.CHECK_FOR_QTY_IN_TRN_IMAGE_VIEW(CLIENT_NO, Year, Month).ToList();
                    if (checkForQty.Count > 0)
                    {
                        foreach (var result in checkForQty)
                        {
                            db.SET_CLIENT_RPT_CLIENT_MONTHLY_IMAGE_VIEW_COMPILE_INSERT(result.CLIENT_NO, Year, Month, result.QUANTITY, this.USER_NO, this.LOGON_NO);
                        }

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


        [HttpPost]
        public ActionResult List(int CLIENT_NO = 0, int YEAR = 0, int MONTH = 0)
        {


            List<GET_IMAGE_COMPILATION_HISTORY_Result> image_compilation_list = new List<GET_IMAGE_COMPILATION_HISTORY_Result>();

            using (DBEntities db = new DBEntities())
            {
                image_compilation_list = db.GET_IMAGE_COMPILATION_HISTORY(CLIENT_NO, YEAR, MONTH).ToList();
            }

            if (image_compilation_list == null)
            {
                image_compilation_list = new List<GET_IMAGE_COMPILATION_HISTORY_Result>();
            }
            return View("List", image_compilation_list);

        }

    }
}
