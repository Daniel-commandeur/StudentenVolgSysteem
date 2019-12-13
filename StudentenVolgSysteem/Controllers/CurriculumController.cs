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
    public class CurriculumController : Controller
    {
        private SVSContext db = new SVSContext();

        // GET: Curriculum
        public ActionResult Index()
        {
            return View(db.Curricula.Include("Student").ToList());
        }

        // GET: Curriculum/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculum curriculum = db.Curricula.Find(id);
            if (curriculum == null)
            {
                return HttpNotFound();
            }
            return View(curriculum);
        }

        // GET: Curriculum/Create
        public ActionResult Create(int? id)
        {
            List<Topic> theTopics = db.Topics.ToList();
            CUCurriculum cuc = new CUCurriculum() { AlleTopics = theTopics };
            if(id != null)
            {
                cuc.StudentId = db.Studenten.Find(id).StudentId;
                cuc.Student = db.Studenten.Find(id);
            }
            return View(cuc);
        }

        // POST: Curriculum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CurriculumId,StudentId,Topics,alleTopicIds,Naam")] CUCurriculum cuc)
        {
            if (ModelState.IsValid)
            {
                Curriculum cm = new Curriculum();
                cm.Naam = cuc.Naam;
                cm.Student = db.Studenten.Find(cuc.StudentId);
                foreach (var topic in cuc.alleTopicIds)
                {
                    cm.Topics.Add(db.Topics.Where(a => a.TopicId.ToString() == topic).FirstOrDefault());
                }
                db.Curricula.Add(cm);
                db.SaveChanges();
                if (cuc.StudentId != 0)
                {
                    return RedirectToAction("Details", "Student", new { id = cuc.StudentId });
                }
                return RedirectToAction("Index");
            }
            cuc.AlleTopics = db.Topics.ToList();
            return View(cuc);
        }

        // GET: Curriculum/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculum curriculum = db.Curricula.Include(c => c.Topics).Where(c => c.CurriculumId == id).FirstOrDefault();
            if (curriculum == null)
            {
                return HttpNotFound();
            }
            CUCurriculum cuc = new CUCurriculum(curriculum);
            cuc.AlleTopics = db.Topics.ToList();
            return View(cuc);
        }

        // POST: Curriculum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CurriculumId,StudentId,Topics,Naam,alleTopicIds")] CUCurriculum cuc)
        {
            if (ModelState.IsValid)
            {
                Curriculum cm = db.Curricula.Find(cuc.CurriculumId);
                foreach (var topic in cuc.alleTopicIds)
                {
                    cm.Topics.Add(db.Topics.Where(a => a.TopicId.ToString() == topic).FirstOrDefault());
                }
                db.Entry(cm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            cuc.AlleTopics = db.Topics.ToList();
            return View(cuc);
        }

        // GET: Curriculum/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculum curriculum = db.Curricula.Find(id);
            if (curriculum == null)
            {
                return HttpNotFound();
            }
            return View(curriculum);
        }

        // POST: Curriculum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Curriculum curriculum = db.Curricula.Find(id);
            db.Curricula.Remove(curriculum);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetPartialDisplayCurriculum(int id)
        {
            Curriculum curriculum = db.Curricula.Find(id);
            return PartialView("PartialDisplayCurriculum", curriculum);
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
