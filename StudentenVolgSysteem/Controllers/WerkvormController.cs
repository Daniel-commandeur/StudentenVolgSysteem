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
    [Authorize (Roles = "Administrator")]
    public class WerkvormController : Controller
    {
        private MyDbContext db = new MyDbContext();

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
            WerkvormModel werkvormModel = db.Werkvormen.Find(id);
            if (werkvormModel == null)
            {
                return HttpNotFound();
            }
            return View(werkvormModel);
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
        public ActionResult Create([Bind(Include = "WerkvormId,Werkvorm")] WerkvormModel werkvormModel)
        {
            if (ModelState.IsValid)
            {
                db.Werkvormen.Add(werkvormModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(werkvormModel);
        }

        // GET: Werkvorm/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WerkvormModel werkvormModel = db.Werkvormen.Find(id);
            if (werkvormModel == null)
            {
                return HttpNotFound();
            }
            return View(werkvormModel);
        }

        // POST: Werkvorm/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WerkvormId,Werkvorm")] WerkvormModel werkvormModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(werkvormModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(werkvormModel);
        }

        // GET: Werkvorm/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WerkvormModel werkvormModel = db.Werkvormen.Find(id);
            if (werkvormModel == null)
            {
                return HttpNotFound();
            }
            return View(werkvormModel);
        }

        // POST: Werkvorm/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WerkvormModel werkvormModel = db.Werkvormen.Find(id);
            db.Werkvormen.Remove(werkvormModel);
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
