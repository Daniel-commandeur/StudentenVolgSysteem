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
    public class TijdsdurenController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Tijdsduren
        public ActionResult Index()
        {
            return View(db.TijdsDuren.ToList());
        }

        // GET: Tijdsduren/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TijdsDuurModel tijdsDuurModel = db.TijdsDuren.Find(id);
            if (tijdsDuurModel == null)
            {
                return HttpNotFound();
            }
            return View(tijdsDuurModel);
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
        public ActionResult Create([Bind(Include = "TijdsDuurId,Eenheid")] TijdsDuurModel tijdsDuurModel)
        {
            if (ModelState.IsValid)
            {
                db.TijdsDuren.Add(tijdsDuurModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tijdsDuurModel);
        }

        // GET: Tijdsduren/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TijdsDuurModel tijdsDuurModel = db.TijdsDuren.Find(id);
            if (tijdsDuurModel == null)
            {
                return HttpNotFound();
            }
            return View(tijdsDuurModel);
        }

        // POST: Tijdsduren/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TijdsDuurId,Eenheid")] TijdsDuurModel tijdsDuurModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tijdsDuurModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tijdsDuurModel);
        }

        // GET: Tijdsduren/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TijdsDuurModel tijdsDuurModel = db.TijdsDuren.Find(id);
            if (tijdsDuurModel == null)
            {
                return HttpNotFound();
            }
            return View(tijdsDuurModel);
        }

        // POST: Tijdsduren/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TijdsDuurModel tijdsDuurModel = db.TijdsDuren.Find(id);
            db.TijdsDuren.Remove(tijdsDuurModel);
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
