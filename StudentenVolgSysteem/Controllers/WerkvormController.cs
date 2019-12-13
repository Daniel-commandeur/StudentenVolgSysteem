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
    [Authorize (Roles = "Administrator")]
    public class WerkvormController : Controller
    {
        private SVSContext db = new SVSContext();

        // GET: Werkvorm
        public ActionResult Index()
        {
            return View(db.Werkvormen.ToList());
        }

        // GET: Werkvorm/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Werkvorm werkvorm = db.Werkvormen.Find(id);
            if (werkvorm == null)
            {
                return HttpNotFound();
            }
            return View(werkvorm);
        }

        // GET: Werkvorm/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Werkvorm/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WerkvormId,Naam")] Werkvorm werkvorm)
        {
            if (ModelState.IsValid)
            {
                db.Werkvormen.Add(werkvorm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(werkvorm);
        }

        // GET: Werkvorm/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Werkvorm werkvorm = db.Werkvormen.Find(id);
            if (werkvorm == null)
            {
                return HttpNotFound();
            }
            return View(werkvorm);
        }

        // POST: Werkvorm/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WerkvormId,Naam")] Werkvorm werkvorm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(werkvorm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(werkvorm);
        }

        // GET: Werkvorm/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Werkvorm werkvorm = db.Werkvormen.Find(id);
            if (werkvorm == null)
            {
                return HttpNotFound();
            }
            return View(werkvorm);
        }

        // POST: Werkvorm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Werkvorm werkvorm = db.Werkvormen.Find(id);
            db.Werkvormen.Remove(werkvorm);
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
