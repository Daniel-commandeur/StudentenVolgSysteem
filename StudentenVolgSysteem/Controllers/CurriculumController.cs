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
using StudentenVolgSysteem.Models.ViewModels;

namespace StudentenVolgSysteem.Controllers
{
    [Authorize(Roles = "Administrator, Docent")]
    public class CurriculumController : Controller
    {
        private SVSContext db = new SVSContext();

        // GET: Curriculum
        public ActionResult Index()
        {
            return View(db.Curricula.Where(c => !c.IsDeleted).Include("Student").Include("Topics.Topic").ToList());
        }

        // GET: Curriculum/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculum curriculum = db.Curricula.Where(c => !c.IsDeleted).Include("Student").Include("Topics.Topic").Where(c => c.CurriculumId == id).FirstOrDefault();

            if (curriculum == null || curriculum.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(curriculum);
        }

        // GET: Curriculum/Create
        public ActionResult Create(int? id)
        {
            List<Topic> theTopics = db.Topics.Where(t => !t.IsDeleted).ToList();
            List<Student> studenten = db.Studenten.Where(s => !s.IsDeleted).ToList();
            CurriculumViewModel cvm = new CurriculumViewModel { AlleTopics = theTopics, AlleStudenten = studenten };          

            if(id != null)
            {
                cvm.StudentId = db.Studenten.Find(id).StudentId;
                cvm.Student = db.Studenten.Find(id);
            } 

            //Cur cuc = new CUCurriculum() { AlleTopics = theTopics };
            //if(id != null)
            //{
            //    //cvm.Curriculum = db.Curricula.Where(c => c.Student.StudentId == cvm.StudentId).First();
            //    cvm.StudentId = db.Studenten.Find(id).StudentId;

            //    cuc.StudentId = db.Studenten.Find(id).StudentId;
            //    cuc.Student = db.Studenten.Find(id);
            //}
            return View(cvm);
        }

        // POST: Curriculum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CurriculumViewModel cvm)
        {
            if (ModelState.IsValid)
            {
                Curriculum cm = new Curriculum();
                cm = cvm.Curriculum;
                //cm.CurrNaam = cvm.Naam;
                cm.Student = db.Studenten.Find(cvm.StudentId);             

                foreach(int topic in cvm.TopicIds)
                {
                    Topic t = db.Topics.Find(topic);
                    CurriculumTopic ct = new CurriculumTopic { TopicId = topic, Topic = t, Curriculum = cm, CurriculumId = cm.CurriculumId };
                    db.CurriculumTopics.Add(ct);
                    cm.Topics.Add(ct);
                }

                db.Curricula.Add(cm);
                
                db.SaveChanges();
                if (cvm.StudentId != 0)
                {
                    return RedirectToAction("Details", "Student", new { id = cvm.StudentId });
                }
                return RedirectToAction("Index");
            }
            cvm.AlleTopics = db.Topics.ToList();
            return View(cvm);
        }

        // GET: Curriculum/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculum curriculum = db.Curricula.Include(c => c.Topics).Where(c => c.CurriculumId == id).FirstOrDefault();
            if (curriculum == null || curriculum.IsDeleted)
            {
                return HttpNotFound();
            }
            CurriculumViewModel cvm = new CurriculumViewModel { Curriculum = curriculum, AlleTopics = db.Topics.ToList() };
            //cvm.AlleTopics = db.Topics.ToList();
            return View(cvm);
        }

        // POST: Curriculum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CurriculumViewModel cvm)
        {
            if (ModelState.IsValid)
            {
                Curriculum cm = db.Curricula.Find(cvm.Curriculum.CurriculumId);
                List<CurriculumTopic> cts = db.CurriculumTopics.Where(ct => ct.CurriculumId == cm.CurriculumId).ToList();       
                
                foreach (int topic in cvm.TopicIds)
                {                                      
                    CurriculumTopic ct = cts.Where(c => c.TopicId == topic).FirstOrDefault();
                        //new CurriculumTopic { TopicId = topic, Topic = t, Curriculum = cm, CurriculumId = cm.CurriculumId };
                    db.Entry(ct).State = EntityState.Modified;
                    cm.Topics.Add(ct);
                }
                db.Entry(cts).State = EntityState.Modified;
                db.Entry(cm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            cvm.AlleTopics = db.Topics.ToList();
            return View(cvm);
        }

        // GET: Curriculum/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculum curriculum = db.Curricula.Include("Student").Include("Topics.Topic").Where(c => c.CurriculumId == id).FirstOrDefault();
            if (curriculum == null || curriculum.IsDeleted)
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
            //db.Curricula.Remove(curriculum);
            curriculum.IsDeleted = true;

            // Tijdelijke oplossing update Property (studentId raakte verloren).
            db.Curricula.Attach(curriculum);
            db.Entry(curriculum).Property(x => x.IsDeleted).IsModified = true;
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
