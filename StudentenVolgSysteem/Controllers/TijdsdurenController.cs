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
    public class TijdsdurenController : Controller
    {
        private SVSContext db = new SVSContext();

        // GET: Tijdsduren
        public ActionResult Index()
        {
            //return View(db.Tijdsduren.ToList()); 
            return View(db.Tijdsduren.Where(td => !td.IsDeleted).ToList());
        }

        // GET: Tijdsduren/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tijdsduur tijdsduur = db.Tijdsduren.Find(id);
            if (tijdsduur == null)
            {
                return HttpNotFound();
            }
            return View(tijdsduur);
        }

        // GET: Tijdsduren/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tijdsduren/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TijdsduurId,Eenheid")] Tijdsduur tijdsduur)
        {
            if (ModelState.IsValid)
            {
                db.Tijdsduren.Add(tijdsduur);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tijdsduur);
        }

        // GET: Tijdsduren/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tijdsduur tijdsduur = db.Tijdsduren.Find(id);
            if (tijdsduur == null)
            {
                return HttpNotFound();
            }
            return View(tijdsduur);
        }

        // POST: Tijdsduren/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TijdsduurId,Eenheid")] Tijdsduur tijdsduur)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tijdsduur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tijdsduur);
        }

        // GET: Tijdsduren/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tijdsduur tijdsduur = db.Tijdsduren.Find(id);
            if (tijdsduur == null)
            {
                return HttpNotFound();
            }
            return View(tijdsduur);
        }

        // POST: Tijdsduren/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tijdsduur tijdsduur = db.Tijdsduren.Find(id);
            db.Tijdsduren.Remove(tijdsduur);
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
