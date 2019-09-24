using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentenVolgSysteem.Models;

namespace StudentenVolgSysteem.Controllers
{
    public class CertificeringenController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Certificeringen
        public ActionResult Index()
        {
            return View(db.CertificeringenInfras.ToList());
        }

        // GET: Certificeringen/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CertificeringenInfraModel certificeringenInfraModel = db.CertificeringenInfras.Find(id);
            if (certificeringenInfraModel == null)
            {
                return HttpNotFound();
            }
            return View(certificeringenInfraModel);
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
        public ActionResult Create([Bind(Include = "CertificeringenInfraId,Certificering")] CertificeringenInfraModel certificeringenInfraModel)
        {
            if (ModelState.IsValid)
            {
                db.CertificeringenInfras.Add(certificeringenInfraModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(certificeringenInfraModel);
        }

        // GET: Certificeringen/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CertificeringenInfraModel certificeringenInfraModel = db.CertificeringenInfras.Find(id);
            if (certificeringenInfraModel == null)
            {
                return HttpNotFound();
            }
            return View(certificeringenInfraModel);
        }

        // POST: Certificeringen/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CertificeringenInfraId,Certificering")] CertificeringenInfraModel certificeringenInfraModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(certificeringenInfraModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(certificeringenInfraModel);
        }

        // GET: Certificeringen/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CertificeringenInfraModel certificeringenInfraModel = db.CertificeringenInfras.Find(id);
            if (certificeringenInfraModel == null)
            {
                return HttpNotFound();
            }
            return View(certificeringenInfraModel);
        }

        // POST: Certificeringen/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CertificeringenInfraModel certificeringenInfraModel = db.CertificeringenInfras.Find(id);
            db.CertificeringenInfras.Remove(certificeringenInfraModel);
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
