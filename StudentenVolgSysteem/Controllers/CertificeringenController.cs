using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentenVolgSysteem.Models;
using StudentenVolgSysteem.DAL;

namespace StudentenVolgSysteem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CertificeringenController : Controller
    {
        private SVSContext db = new SVSContext();

        // GET: Certificeringen
        public ActionResult Index()
        {
            return View(db.Certificeringen.ToList());
        }

        // GET: Certificeringen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certificering certificering = db.Certificeringen.Find(id);
            if (certificering == null)
            {
                return HttpNotFound();
            }
            return View(certificering);
        }

        // GET: Certificeringen/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Certificeringen/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Certificering,Naam")] Certificering certificering)
        {
            if (ModelState.IsValid)
            {
                db.Certificeringen.Add(certificering);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(certificering);
        }

        // GET: Certificeringen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certificering certificering = db.Certificeringen.Find(id);
            if (certificering == null)
            {
                return HttpNotFound();
            }
            return View(certificering);
        }

        // POST: Certificeringen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CertificeringId,Naam")] Certificering certificering)
        {
            if (ModelState.IsValid)
            {
                db.Entry(certificering).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(certificering);
        }

        // GET: Certificeringen/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Certificering certificering = db.Certificeringen.Find(id);
            if (certificering == null)
            {
                return HttpNotFound();
            }
            return View(certificering);
        }

        // POST: Certificeringen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Certificering certificering = db.Certificeringen.Find(id);
            db.Certificeringen.Remove(certificering);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
