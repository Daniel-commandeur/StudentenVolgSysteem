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
    public class PercipiolinkController : Controller
    {
        private SVSContext db = new SVSContext();

        // GET: Percipiolink
        public ActionResult Index()
        {
            return View(db.PercipioLinks.Where(p => !p.IsDeleted).ToList());
        }

        // GET: Percipiolink/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PercipioLink percipioLink = db.GetFromDatabase<PercipioLink>(id);
            if (percipioLink == null || percipioLink.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(percipioLink);
        }

        // GET: Percipiolink/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Percipiolink/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PercipioLinkId,Link")] PercipioLink percipioLink)
        {
            if (ModelState.IsValid)
            {
                db.PercipioLinks.Add(percipioLink);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(percipioLink);
        }

        // GET: Percipiolink/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PercipioLink percipioLink = db.GetFromDatabase<PercipioLink>(id);
            if (percipioLink == null || percipioLink.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(percipioLink);
        }

        // POST: Percipiolink/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PercipioLinkId,Link")] PercipioLink percipioLink)
        {
            if (ModelState.IsValid)
            {
                db.Entry(percipioLink).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(percipioLink);
        }

        // GET: Percipiolink/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PercipioLink percipioLink = db.GetFromDatabase<PercipioLink>(id);
            if (percipioLink == null || percipioLink.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(percipioLink);
        }

        // POST: Percipiolink/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PercipioLink percipioLink = db.GetFromDatabase<PercipioLink>(id);
            db.PercipioLinks.Remove(percipioLink);
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
