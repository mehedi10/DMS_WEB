using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DMS_WEB.Models;

namespace DMS_WEB.Controllers
{
    public class ClientDepartmentController : Controller
    {
        private DBEntities db = new DBEntities();

        //
        // GET: /ClientDepartment/

        public ActionResult Index()
        {
            var set_client_departments = db.SET_CLIENT_DEPARTMENTS.Include("SET_CLIENTS");
            return View(set_client_departments.ToList());
        }

        //
        // GET: /ClientDepartment/Details/5

        public ActionResult Details(short id = 0)
        {
            SET_CLIENT_DEPARTMENTS set_client_departments = db.SET_CLIENT_DEPARTMENTS.Single(s => s.DEPT_NO == id);
            if (set_client_departments == null)
            {
                return HttpNotFound();
            }
            return View(set_client_departments);
        }

        //
        // GET: /ClientDepartment/Create

        public ActionResult Create()
        {
            ViewBag.CLIENT_NO = new SelectList(db.SET_CLIENTS, "CLIENT_NO", "CLIENT_NAME");
            return View();
        }

        //
        // POST: /ClientDepartment/Create

        [HttpPost]
        public ActionResult Create(SET_CLIENT_DEPARTMENTS set_client_departments)
        {
            if (ModelState.IsValid)
            {
                db.SET_CLIENT_DEPARTMENTS.AddObject(set_client_departments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CLIENT_NO = new SelectList(db.SET_CLIENTS, "CLIENT_NO", "CLIENT_NAME", set_client_departments.CLIENT_NO);
            return View(set_client_departments);
        }

        //
        // GET: /ClientDepartment/Edit/5

        public ActionResult Edit(short id = 0)
        {
            SET_CLIENT_DEPARTMENTS set_client_departments = db.SET_CLIENT_DEPARTMENTS.Single(s => s.DEPT_NO == id);
            if (set_client_departments == null)
            {
                return HttpNotFound();
            }
            ViewBag.CLIENT_NO = new SelectList(db.SET_CLIENTS, "CLIENT_NO", "CLIENT_NAME", set_client_departments.CLIENT_NO);
            return View(set_client_departments);
        }

        //
        // POST: /ClientDepartment/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_CLIENT_DEPARTMENTS set_client_departments)
        {
            if (ModelState.IsValid)
            {
                db.SET_CLIENT_DEPARTMENTS.Attach(set_client_departments);
                db.ObjectStateManager.ChangeObjectState(set_client_departments, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CLIENT_NO = new SelectList(db.SET_CLIENTS, "CLIENT_NO", "CLIENT_NAME", set_client_departments.CLIENT_NO);
            return View(set_client_departments);
        }

        //
        // GET: /ClientDepartment/Delete/5

        public ActionResult Delete(short id = 0)
        {
            SET_CLIENT_DEPARTMENTS set_client_departments = db.SET_CLIENT_DEPARTMENTS.Single(s => s.DEPT_NO == id);
            if (set_client_departments == null)
            {
                return HttpNotFound();
            }
            return View(set_client_departments);
        }

        //
        // POST: /ClientDepartment/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {
            SET_CLIENT_DEPARTMENTS set_client_departments = db.SET_CLIENT_DEPARTMENTS.Single(s => s.DEPT_NO == id);
            db.SET_CLIENT_DEPARTMENTS.DeleteObject(set_client_departments);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}