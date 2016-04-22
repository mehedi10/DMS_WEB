using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using DMS_WEB.Models;
using System.Web.Script.Serialization;
using System.Data.Objects;
using System.Configuration;
using System.IO;


namespace DMS_WEB.Controllers {
    public class UserController : BaseController
    {
        //
        // GET: /User/

        public ActionResult Index(int id = 0)
        {
            SEC_USERS_GET_Result model = new SEC_USERS_GET_Result();

            List<SET_CLIENTS_GET_Result> client_list = new List<SET_CLIENTS_GET_Result>();
            List<GEN_USER_TYPES_GET_Result> user_type_list = new List<GEN_USER_TYPES_GET_Result>();

            using (DBEntities db = new DBEntities())
            {
                client_list = db.SET_CLIENTS_GET(null, true).ToList();
                user_type_list = db.GEN_USER_TYPES_GET(null, null, null).ToList();
            }

            if (client_list == null)
            {
                client_list = new List<SET_CLIENTS_GET_Result>();
            }

            var setup_info = new
            {
                @client_list = client_list,
                @user_type_list = user_type_list,
            };

            ViewBag.setup_info = new JavaScriptSerializer().Serialize(setup_info);

            using (DBEntities db = new DBEntities())
            {
                model = db.SEC_USERS_GET(id, null, null, null, null, null, null).FirstOrDefault();
            }
            if (model == null)
            {
                model = new SEC_USERS_GET_Result();
            }

            return View(model);
        }


        [HttpPost]
        public JsonResult Save(SEC_USERS item)
        {
            bool is_saved = false;
            string str_error = "";

            long? USER_NO = 0;

            try
            {
                using (DBEntities db = new DBEntities())
                {
                    if (item.USER_NO > 0)
                    {
                        USER_NO = item.USER_NO;
                        db.SEC_USERS_UPDATE(item.USER_NO, item.USER_TYPE_NO, item.FIRST_NAME, item.MID_NAME,
                            item.LAST_NAME, item.LOGIN_ID, item.PASS_SALT, item.LOGIN_PASSWORD, item.IS_ACTIVE,
                            Convert.ToDateTime(item.ACTIVE_FROM), Convert.ToDateTime(item.ACTIVE_TO), item.IS_LOCKED,
                            item.LOCK_REASON, item.LOCK_IP_ADDR, item.LOCK_TIME, item.CLIENT_NO, item.CAN_VIEW_DATA,
                            item.CAN_VIEW_IMAGE, item.CAN_DO_REQUEST, item.CREATE_USER_NO, item.CREATE_LOGON_NO,
                            item.CREATE_TIME, item.UPDATE_USER_NO, item.UPDATE_LOGON_NO, item.UPDATE_TIME);
                    }
                    else
                    {
                        ObjectParameter USER_NO_parameter = new ObjectParameter("USER_NO", USER_NO);
                        db.SEC_USERS_INSERT(USER_NO_parameter, item.USER_TYPE_NO, item.FIRST_NAME, item.MID_NAME,
                            item.LAST_NAME, item.LOGIN_ID, item.PASS_SALT, item.LOGIN_PASSWORD, item.IS_ACTIVE,
                            Convert.ToDateTime(item.ACTIVE_FROM), Convert.ToDateTime(item.ACTIVE_TO), item.IS_LOCKED,
                            item.LOCK_REASON, item.LOCK_IP_ADDR, item.LOCK_TIME, item.CLIENT_NO, item.CAN_VIEW_DATA,
                            item.CAN_VIEW_IMAGE, item.CAN_DO_REQUEST, item.CREATE_USER_NO, item.CREATE_LOGON_NO,
                            item.CREATE_TIME, item.UPDATE_USER_NO, item.UPDATE_LOGON_NO, item.UPDATE_TIME);
                        USER_NO = (long?) USER_NO_parameter.Value;
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
                @id = USER_NO,
            };

            return Json(ret_val, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult Search(UserSearch obj)
        {
            TempData["obj_UserSearch"] = obj;

            List<SEC_USERS_GET_Result> user_list = new List<SEC_USERS_GET_Result>();

            using (DBEntities db = new DBEntities())
            {
                user_list = db.SEC_USERS_GET(obj.USER_NO, obj.USER_TYPE_NO, obj.LOGIN_ID, obj.LOGIN_PASSWORD, obj.IS_ACTIVE, obj.IS_LOCKED, obj.CLIENT_NO).ToList();

                if (user_list == null)
                {
                    user_list = new List<SEC_USERS_GET_Result>();
                }

                return View("List", user_list);
            }
        }

        public ActionResult Edit(int id = 0)
        {
            SEC_USERS_GET_Result info = new SEC_USERS_GET_Result();

            using (DBEntities db = new DBEntities())
            {
                info = db.SEC_USERS_GET(id, null, null, null, null, null, null).FirstOrDefault();
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
                    db.SEC_USERS_DELETE(id);
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

        //public ActionResult RenderView(int id)
        //{
        //    if (id == 400)
        //    {
        //        return PartialView("restof");
        //    }
        //    else
        //    {
        //        return Index();
        //    }
        //}
    }
}
