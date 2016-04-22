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
    public class ReasonController : Controller
    {
        private DBEntities db = new DBEntities();

        //
        // GET: /Reason/

        public ActionResult Index()
        {
            var set_reasons = db.SET_REASONS.Include("SET_CLIENTS");
            return View(set_reasons.ToList());
        }

        //
        // GET: /Reason/Details/5

        public ActionResult Details(short id = 0)
        {
            SET_REASONS set_reasons = db.SET_REASONS.Single(s => s.REASON_NO == id);
            if (set_reasons == null)
            {
                return HttpNotFound();
            }
            return View(set_reasons);
        }

        //
        // GET: /Reason/Create

        public ActionResult Create()
        {
            ViewBag.CLIENT_NO = new SelectList(db.SET_CLIENTS, "CLIENT_NO", "CLIENT_NAME");
            return View();
        }

        //
        // POST: /Reason/Create

        [HttpPost]
        public ActionResult Create(SET_REASONS set_reasons)
        {
            if (ModelState.IsValid)
            {
                db.SET_REASONS.AddObject(set_reasons);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CLIENT_NO = new SelectList(db.SET_CLIENTS, "CLIENT_NO", "CLIENT_NAME", set_reasons.CLIENT_NO);
            return View(set_reasons);
        }

        //
        // GET: /Reason/Edit/5

        public ActionResult Edit(short id = 0)
        {
            SET_REASONS set_reasons = db.SET_REASONS.Single(s => s.REASON_NO == id);
            if (set_reasons == null)
            {
                return HttpNotFound();
            }
            ViewBag.CLIENT_NO = new SelectList(db.SET_CLIENTS, "CLIENT_NO", "CLIENT_NAME", set_reasons.CLIENT_NO);
            return View(set_reasons);
        }

        //
        // POST: /Reason/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_REASONS set_reasons)
        {
            if (ModelState.IsValid)
            {
                db.SET_REASONS.Attach(set_reasons);
                db.ObjectStateManager.ChangeObjectState(set_reasons, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CLIENT_NO = new SelectList(db.SET_CLIENTS, "CLIENT_NO", "CLIENT_NAME", set_reasons.CLIENT_NO);
            return View(set_reasons);
        }

        //
        // GET: /Reason/Delete/5

        public ActionResult Delete(short id = 0)
        {
            SET_REASONS set_reasons = db.SET_REASONS.Single(s => s.REASON_NO == id);
            if (set_reasons == null)
            {
                return HttpNotFound();
            }
            return View(set_reasons);
        }

        //
        // POST: /Reason/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {
            SET_REASONS set_reasons = db.SET_REASONS.Single(s => s.REASON_NO == id);
            db.SET_REASONS.DeleteObject(set_reasons);
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