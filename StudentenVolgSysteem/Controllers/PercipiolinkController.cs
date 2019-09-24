﻿using System;
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
    public class PercipiolinkController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Percipiolink
        public ActionResult Index()
        {
            return View(db.PercipioLinks.ToList());
        }

        // GET: Percipiolink/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PercipiolinkModel percipiolinkModel = db.PercipioLinks.Find(id);
            if (percipiolinkModel == null)
            {
                return HttpNotFound();
            }
            return View(percipiolinkModel);
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
        public ActionResult Create([Bind(Include = "PercipiolinkId,Link")] PercipiolinkModel percipiolinkModel)
        {
            if (ModelState.IsValid)
            {
                db.PercipioLinks.Add(percipiolinkModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(percipiolinkModel);
        }

        // GET: Percipiolink/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PercipiolinkModel percipiolinkModel = db.PercipioLinks.Find(id);
            if (percipiolinkModel == null)
            {
                return HttpNotFound();
            }
            return View(percipiolinkModel);
        }

        // POST: Percipiolink/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PercipiolinkId,Link")] PercipiolinkModel percipiolinkModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(percipiolinkModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(percipiolinkModel);
        }

        // GET: Percipiolink/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PercipiolinkModel percipiolinkModel = db.PercipioLinks.Find(id);
            if (percipiolinkModel == null)
            {
                return HttpNotFound();
            }
            return View(percipiolinkModel);
        }

        // POST: Percipiolink/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PercipiolinkModel percipiolinkModel = db.PercipioLinks.Find(id);
            db.PercipioLinks.Remove(percipiolinkModel);
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