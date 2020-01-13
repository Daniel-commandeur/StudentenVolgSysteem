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
    public class BenodigdhedenController : Controller
    {
        private SVSContext db = new SVSContext();

        // GET: Benodigdheden
        public ActionResult Index()
        {
            return View(db.Benodigdheden.Where(b => !b.IsDeleted).ToList());
        }

        // GET: Benodigdheden/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Benodigdheid benodigdheid = db.GetFromDatabase<Benodigdheid>(id);
            if (benodigdheid == null || benodigdheid.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(benodigdheid);
        }

        // GET: Benodigdheden/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Benodigdheden/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BenodigdheidId,Naam")] Benodigdheid benodigdheid)
        {
            if (ModelState.IsValid)
            {
                db.Benodigdheden.Add(benodigdheid);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(benodigdheid);
        }

        // GET: Benodigdheden/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Benodigdheid benodigdheid = db.GetFromDatabase<Benodigdheid>(id); 
            if (benodigdheid == null || benodigdheid.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(benodigdheid);
        }

        // POST: Benodigdheden/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BenodigdheidId,Naam")] Benodigdheid benodigdheid)
        {
            if (ModelState.IsValid)
            {
                db.Entry(benodigdheid).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(benodigdheid);
        }

        // GET: Benodigdheden/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Benodigdheid benodigdheid = db.GetFromDatabase<Benodigdheid>(id);
            if (benodigdheid == null || benodigdheid.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(benodigdheid);
        }

        // POST: Benodigdheden/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Benodigdheid benodigdheid = db.GetFromDatabase<Benodigdheid>(id);
            db.Benodigdheden.Remove(benodigdheid);
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
