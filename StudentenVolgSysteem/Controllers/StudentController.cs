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
    [Authorize(Roles = "Administrator, Docent")]
    public class StudentController : Controller
    {
        private SVSContext db = new SVSContext();

        // GET: Student
        public ActionResult Index()
        {
            return View(db.GetFromDatabase<Student>());
        }

        // GET: Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Every relation that is handled with a link-table isn't automatically
            Student student = db.Studenten
                                        .Include(a => a.Curriculum)
                                        .Include(a => a.Curriculum.Topics.Select(c => c.Topic.Duur))
                                        .Include(a => a.Curriculum.Topics.Select(c => c.Topic.Benodigdheden))
                                        .Include(a => a.Curriculum.Topics.Select(c => c.Topic.Werkvorm))
                                        .Include(a => a.Curriculum.Topics.Select(c => c.Topic.Niveau))
                                        .Where(a => a.Id == id)
                                        .FirstOrDefault();
                                     
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Voornaam,Achternaam")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Studenten.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.GetFromDatabase<Student>(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Voornaam,Achternaam")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.GetFromDatabase<Student>(id); 
            if (student == null )
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // If we want soft-delete to automatically cascade, we cannot use this
            // Student student = db.GetFromDatabase<Student>(id);

            // Remove selected student, and also remove related Curricula
            Student student = db.Studenten.Include("Curricula").Where(s => s.Id == id).FirstOrDefault();
            db.Studenten.Remove(student);
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
