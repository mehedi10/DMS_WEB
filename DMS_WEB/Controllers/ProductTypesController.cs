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
    public class ProductTypesController : Controller
    {
        private DBEntities db = new DBEntities();

        //
        // GET: /ProductTypes/

        public ActionResult Index()
        {
            return View(db.SET_PRODUCT_TYPES.ToList());
        }

        //
        // GET: /ProductTypes/Details/5

        public ActionResult Details(byte id = 0)
        {
            SET_PRODUCT_TYPES set_product_types = db.SET_PRODUCT_TYPES.Single(s => s.PROD_TYPE_NO == id);
            if (set_product_types == null)
            {
                return HttpNotFound();
            }
            return View(set_product_types);
        }

        //
        // GET: /ProductTypes/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ProductTypes/Create

        [HttpPost]
        public ActionResult Create(SET_PRODUCT_TYPES set_product_types)
        {
            if (ModelState.IsValid)
            {
                db.SET_PRODUCT_TYPES.AddObject(set_product_types);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(set_product_types);
        }

        //
        // GET: /ProductTypes/Edit/5

        public ActionResult Edit(byte id = 0)
        {
            SET_PRODUCT_TYPES set_product_types = db.SET_PRODUCT_TYPES.Single(s => s.PROD_TYPE_NO == id);
            if (set_product_types == null)
            {
                return HttpNotFound();
            }
            return View(set_product_types);
        }

        //
        // POST: /ProductTypes/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_PRODUCT_TYPES set_product_types)
        {
            if (ModelState.IsValid)
            {
                db.SET_PRODUCT_TYPES.Attach(set_product_types);
                db.ObjectStateManager.ChangeObjectState(set_product_types, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(set_product_types);
        }

        //
        // GET: /ProductTypes/Delete/5

        public ActionResult Delete(byte id = 0)
        {
            SET_PRODUCT_TYPES set_product_types = db.SET_PRODUCT_TYPES.Single(s => s.PROD_TYPE_NO == id);
            if (set_product_types == null)
            {
                return HttpNotFound();
            }
            return View(set_product_types);
        }

        //
        // POST: /ProductTypes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(byte id)
        {
            SET_PRODUCT_TYPES set_product_types = db.SET_PRODUCT_TYPES.Single(s => s.PROD_TYPE_NO == id);
            db.SET_PRODUCT_TYPES.DeleteObject(set_product_types);
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