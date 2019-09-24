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
    public class BenodigdhedenController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Benodigdheden
        public ActionResult Index()
        {
            return View(db.Benodigdheden.ToList());
        }

        // GET: Benodigdheden/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BenodigdheidModel benodigdheidModel = db.Benodigdheden.Find(id);
            if (benodigdheidModel == null)
            {
                return HttpNotFound();
            }
            return View(benodigdheidModel);
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
        public ActionResult Create([Bind(Include = "BenodigdheidId,Content")] BenodigdheidModel benodigdheidModel)
        {
            if (ModelState.IsValid)
            {
                db.Benodigdheden.Add(benodigdheidModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(benodigdheidModel);
        }

        // GET: Benodigdheden/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BenodigdheidModel benodigdheidModel = db.Benodigdheden.Find(id);
            if (benodigdheidModel == null)
            {
                return HttpNotFound();
            }
            return View(benodigdheidModel);
        }

        // POST: Benodigdheden/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BenodigdheidId,Content")] BenodigdheidModel benodigdheidModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(benodigdheidModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(benodigdheidModel);
        }

        // GET: Benodigdheden/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BenodigdheidModel benodigdheidModel = db.Benodigdheden.Find(id);
            if (benodigdheidModel == null)
            {
                return HttpNotFound();
            }
            return View(benodigdheidModel);
        }

        // POST: Benodigdheden/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BenodigdheidModel benodigdheidModel = db.Benodigdheden.Find(id);
            db.Benodigdheden.Remove(benodigdheidModel);
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