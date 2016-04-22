using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_WEB.Models;
using System.Data.Objects;

namespace DMS_WEB.Controllers
{
    public class LoginController : BaseController
    {
        //
        // GET: /Login/

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Index(SEC_USERS item) {
            bool is_login = false;
            ViewBag.LOGIN_ID = item.LOGIN_ID;
            using (DBEntities db = new DBEntities()) {

                this.user_info = db.SEC_USERS_GET(null, null, item.LOGIN_ID.Trim(), item.LOGIN_PASSWORD, null,null, null).FirstOrDefault();
                if (this.user_info == null) {
                    ViewBag.ErrorMessage = "Invalid User Name or Password";
                } else if (user_info.IS_ACTIVE != true) {
                    ViewBag.ErrorMessage = "Account is Inactive";
                } else {
                    is_login = true;

                    long? LOGON_NO = 0;
                    ObjectParameter LOGON_NO_parameter = new ObjectParameter("LOGON_NO", LOGON_NO);

                    db.SEC_USER_LOGONS_INSERT(LOGON_NO_parameter, user_info.USER_NO, Session.SessionID, Utility.GetLanIPAddress(), Utility.GetUserAgent(), this.APP_NAME, this.APP_VER, Utility.GetServerIPAddress() );

                    Session["sess_user_info"] = this.user_info;
                    Session["sess_USER_NO"] = this.user_info.USER_NO;
                    Session["sess_LOGON_NO"] = LOGON_NO_parameter.Value;

                }
                //db.SEC_USER_LOGONS_INSERT(
            }

            if (is_login) {
                return RedirectToAction("Index", "Home");
            } else {
                return View();
            }
        }

        public ActionResult Logout() {
            this.LoadSecurity();
            if (this.LOGON_NO.HasValue) {
                using (DBEntities db = new DBEntities()) {
                    db.SEC_USER_LOGOUT(this.LOGON_NO);
                }
            }

            //Session.RemoveAll();
            Session.Clear();

            return View("Index");
        }

        public ActionResult ChangePassword() {
            this.LoadSecurity();
            ChangePasswordModel model = new ChangePasswordModel();
            model.LOGIN_ID = this.user_info.LOGIN_ID;
            ViewBag.Message = string.Empty;
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model) {
            this.LoadSecurity();
            model.LOGIN_ID = this.user_info.LOGIN_ID;

            if (model.OLD_PASSWORD != this.user_info.LOGIN_PASSWORD) {
                ModelState.AddModelError("OLD_PASSWORD", "Old Password Mismatch");
            }
            if (model.NEW_PASSWORD != model.RETYPE_PASSWORD) {
                ModelState.AddModelError("NEW_PASSWORD", "Retype Password Mismatch");
            }

            if (ModelState.IsValid) {
                try {
                    using (DBEntities db = new DBEntities()) {
                        db.SEC_USERS_CHANGE_PASSWORD(this.USER_NO, model.NEW_PASSWORD, this.USER_NO, this.LOGON_NO);
                        ViewBag.Message = "Password changed successfully";
                    }                    
                } catch (Exception ex) {
                    ViewBag.Message = "Failed to change password";
                }
            }

            return View(model);
        }
    }
}
