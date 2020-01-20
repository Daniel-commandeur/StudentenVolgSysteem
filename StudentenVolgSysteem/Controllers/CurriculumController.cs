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
            List<Curriculum> curricula = db.Curricula.Where(c => !c.IsDeleted)
                                                     .Include("Student")
                                                     .Include("Topics.Topic").ToList();
            return View(curricula);
        }

        // GET: Curriculum/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculum curriculum = db.Curricula.Where(c => !c.IsDeleted)
                                                .Include("Student")
                                                .Include("Topics.Topic")
                                                .Where(c => c.Id == id)
                                                .FirstOrDefault();

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
            
            CurriculumViewModel curriculumViewModel = new CurriculumViewModel { AlleTopics = theTopics, AlleStudenten = studenten };

            if (id != null)
            {
                curriculumViewModel.StudentId = db.Studenten.Find(id).Id;
                curriculumViewModel.Student = db.Studenten.Find(id);
            }

            return View(curriculumViewModel);
        }

        // POST: Curriculum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CurriculumViewModel curriculumViewModel)
        {
            if (ModelState.IsValid)
            {
                Curriculum curriculum = curriculumViewModel.Curriculum;
                curriculum.Student = db.Studenten.Find(curriculumViewModel.StudentId);

                foreach (int topicId in curriculumViewModel.TopicIds)
                {
                    Topic topic = db.Topics.Find(topicId);
                    CurriculumTopic curriculumTopic = new CurriculumTopic 
                    { 
                        TopicId = topicId, 
                        Topic = topic, 
                        Curriculum = curriculum, 
                        CurriculumId = curriculum.Id 
                    };
                    db.CurriculumTopics.Add(curriculumTopic);
                    curriculum.Topics.Add(curriculumTopic);
                }

                db.Curricula.Add(curriculum);
                db.SaveChanges();

                // COMMENT: This code can probably be removed
                /*
                if (curriculumViewModel.StudentId != 0)
                {
                    return RedirectToAction("Details", "Student", new { id = curriculumViewModel.StudentId });
                }
                */

                return RedirectToAction("Index");
            }
            curriculumViewModel.AlleTopics = db.Topics.Where(t => !t.IsDeleted).ToList();
            curriculumViewModel.AlleStudenten = db.Studenten.Where(s => !s.IsDeleted).ToList();
            curriculumViewModel.AlleTopics = db.Topics.ToList();
            return View(curriculumViewModel);
        }

        // GET: Curriculum/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculum curriculum = db.Curricula.Include(c => c.Topics)
                                                .Where(c => c.Id == id)
                                                .FirstOrDefault();

            if (curriculum == null || curriculum.IsDeleted)
            {
                return HttpNotFound();
            }
            CurriculumViewModel cvm = new CurriculumViewModel 
            { 
                Curriculum = curriculum, 
                AlleTopics = db.Topics.ToList() 
            };
            return View(cvm);
        }

        // POST: Curriculum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CurriculumViewModel curriculumViewModel)
        {
            if (ModelState.IsValid)
            {
                // Determine curriculum, with topics
                Curriculum curriculum = db.Curricula.Include("Topics")
                                                    .First(c => c.Id == curriculumViewModel.Curriculum.Id);

                // Add topic id's to list
                List<int> topicIds = new List<int>();
                foreach (var topic in curriculum.Topics)
                {
                    topicIds.Add(topic.TopicId);
                }

                // Remove topics from the curriculum, if topic is not selected in the curriculumViewModel
                foreach (var topicId in topicIds)
                {
                    if (!curriculumViewModel.TopicIds.Contains(topicId))
                    {
                        curriculum.Topics.Remove(curriculum.Topics.First(t => t.TopicId == topicId));
                    }
                }

                // Add topics to the curriculum, if topic is selected in the curriculumViewModel
                foreach (int topicId in curriculumViewModel.TopicIds)
                {
                    Topic topic = db.Topics.Find(topicId);
                    CurriculumTopic curriculumTopic = new CurriculumTopic 
                    { 
                        TopicId = topicId, 
                        Topic = topic, 
                        Curriculum = curriculum, 
                        CurriculumId = curriculum.Id 
                    };
                     
                    // Check if curriculum exists, add if it does not.
                    if (db.CurriculumTopics.Find(curriculumTopic.CurriculumId, curriculumTopic.TopicId) == null)
                    {
                        curriculum.Topics.Add(curriculumTopic);
                    }
                }
                db.Entry(curriculum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            curriculumViewModel.AlleTopics = db.Topics.ToList();
            return View(curriculumViewModel);
        }

        // GET: Curriculum/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculum curriculum = db.Curricula.Include("Student")
                                                .Include("Topics.Topic")
                                                .Where(c => c.Id == id)
                                                .FirstOrDefault();

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
