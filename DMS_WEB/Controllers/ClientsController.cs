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
    public class ClientsController : Controller
    {
        private DBEntities db = new DBEntities();

        //
        // GET: /Clients/

        public ActionResult Index()
        {
            return View(db.SET_CLIENTS.ToList());
        }

        //
        // GET: /Clients/Details/5

        public ActionResult Details(short id = 0)
        {
            SET_CLIENTS set_clients = db.SET_CLIENTS.Single(s => s.CLIENT_NO == id);
            if (set_clients == null)
            {
                return HttpNotFound();
            }
            return View(set_clients);
        }

        //
        // GET: /Clients/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Clients/Create

        [HttpPost]
        public ActionResult Create(SET_CLIENTS set_clients)
        {
            if (ModelState.IsValid)
            {
                db.SET_CLIENTS.AddObject(set_clients);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(set_clients);
        }

        //
        // GET: /Clients/Edit/5

        public ActionResult Edit(short id = 0)
        {
            SET_CLIENTS set_clients = db.SET_CLIENTS.Single(s => s.CLIENT_NO == id);
            if (set_clients == null)
            {
                return HttpNotFound();
            }
            return View(set_clients);
        }

        //
        // POST: /Clients/Edit/5

        [HttpPost]
        public ActionResult Edit(SET_CLIENTS set_clients)
        {
            if (ModelState.IsValid)
            {
                db.SET_CLIENTS.Attach(set_clients);
                db.ObjectStateManager.ChangeObjectState(set_clients, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(set_clients);
        }

        //
        // GET: /Clients/Delete/5

        public ActionResult Delete(short id = 0)
        {
            SET_CLIENTS set_clients = db.SET_CLIENTS.Single(s => s.CLIENT_NO == id);
            if (set_clients == null)
            {
                return HttpNotFound();
            }
            return View(set_clients);
        }

        //
        // POST: /Clients/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {
            SET_CLIENTS set_clients = db.SET_CLIENTS.Single(s => s.CLIENT_NO == id);
            db.SET_CLIENTS.DeleteObject(set_clients);
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