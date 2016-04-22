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
    public class DocumentTypesController : Controller
    {
        private DBEntities db = new DBEntities();

        //
        // GET: /DocumentTypes/

        public ActionResult Index()
        {
            return View(db.SET_DOCS_TYPES.ToList());
        }

        //
        // GET: /DocumentTypes/Details/5

        public ActionResult Details(short id = 0)
        {
            SET_DOCS_TYPES set_docs_types = db.SET_DOCS_TYPES.Single(s => s.DOC_TYPE_NO == id);
            if (set_docs_types == null)
            {
                return HttpNotFound();
            }
            return View(set_docs_types);
        }

        //
        // GET: /DocumentTypes/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DocumentTypes/Create

        [HttpPost]
        public ActionResult Create(SET_DOCS_TYPES set_docs_types)
        {
            if (ModelState.IsValid)
            {
                db.SET_DOCS_TYPES.AddObject(set_docs_types);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(set_docs_types);
        }

        //
        // GET: /DocumentTypes/Edit/5

        public ActionResult Edit(short id = 0)
        {
            SET_DOCS_TYPES set_docs_types = db.SET_DOCS_TYPES.Single(s => s.DOC_TYPE_NO == id);
            if (set_docs_types == null)
            {
                return HttpNotFound();
            }
            return View(set_docs_types);
        }

        //
        // POST: /DocumentTypes/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_DOCS_TYPES set_docs_types)
        {
            if (ModelState.IsValid)
            {
                db.SET_DOCS_TYPES.Attach(set_docs_types);
                db.ObjectStateManager.ChangeObjectState(set_docs_types, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(set_docs_types);
        }

        //
        // GET: /DocumentTypes/Delete/5

        public ActionResult Delete(short id = 0)
        {
            SET_DOCS_TYPES set_docs_types = db.SET_DOCS_TYPES.Single(s => s.DOC_TYPE_NO == id);
            if (set_docs_types == null)
            {
                return HttpNotFound();
            }
            return View(set_docs_types);
        }

        //
        // POST: /DocumentTypes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {
            SET_DOCS_TYPES set_docs_types = db.SET_DOCS_TYPES.Single(s => s.DOC_TYPE_NO == id);
            db.SET_DOCS_TYPES.DeleteObject(set_docs_types);
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