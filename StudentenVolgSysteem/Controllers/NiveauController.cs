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
    [Authorize(Roles = "Administrator")]
    public class NiveauController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Niveau
        public ActionResult Index()
        {
            return View(db.Niveaus.ToList());
        }

        // GET: Niveau/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NiveauModel niveauModel = db.Niveaus.Find(id);
            if (niveauModel == null)
            {
                return HttpNotFound();
            }
            return View(niveauModel);
        }

        // GET: Niveau/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Niveau/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NiveauId,Niveau")] NiveauModel niveauModel)
        {
            if (ModelState.IsValid)
            {
                db.Niveaus.Add(niveauModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(niveauModel);
        }

        // GET: Niveau/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NiveauModel niveauModel = db.Niveaus.Find(id);
            if (niveauModel == null)
            {
                return HttpNotFound();
            }
            return View(niveauModel);
        }

        // POST: Niveau/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NiveauId,Niveau")] NiveauModel niveauModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(niveauModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(niveauModel);
        }

        // GET: Niveau/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NiveauModel niveauModel = db.Niveaus.Find(id);
            if (niveauModel == null)
            {
                return HttpNotFound();
            }
            return View(niveauModel);
        }

        // POST: Niveau/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            NiveauModel niveauModel = db.Niveaus.Find(id);
            db.Niveaus.Remove(niveauModel);
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
