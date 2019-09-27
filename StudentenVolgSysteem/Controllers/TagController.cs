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
    public class TagController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Tag
        public ActionResult Index()
        {
            return View(db.Tags.ToList());
        }

        // GET: Tag/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagModel tagModel = db.Tags.Find(id);
            if (tagModel == null)
            {
                return HttpNotFound();
            }
            return View(tagModel);
        }

        // GET: Tag/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tag/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TagId,Naam")] TagModel tagModel)
        {
            if (ModelState.IsValid)
            {
                db.Tags.Add(tagModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tagModel);
        }

        // GET: Tag/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagModel tagModel = db.Tags.Find(id);
            if (tagModel == null)
            {
                return HttpNotFound();
            }
            return View(tagModel);
        }

        // POST: Tag/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TagId,Naam")] TagModel tagModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tagModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tagModel);
        }

        // GET: Tag/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TagModel tagModel = db.Tags.Find(id);
            if (tagModel == null)
            {
                return HttpNotFound();
            }
            return View(tagModel);
        }

        // POST: Tag/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TagModel tagModel = db.Tags.Find(id);
            db.Tags.Remove(tagModel);
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
