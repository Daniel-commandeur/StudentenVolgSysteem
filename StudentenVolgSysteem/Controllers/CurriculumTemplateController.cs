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
    public class CurriculumTemplateController : Controller
    {
        private SVSContext db = new SVSContext();

        // GET: Curriculum
        public ActionResult Index()
        {
            return View(db.GetFromDatabase<CurriculumTemplate>());
        }
        
        // GET: Curriculum/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CurriculumTemplate curriculumTemplate = db.GetFromDatabase<CurriculumTemplate>(id);
            
            if (curriculumTemplate == null)
                return HttpNotFound();

            return View(curriculumTemplate);
        }

        // GET: Curriculum/Create
        public ActionResult Create(int? id)
        {
            List<Topic> theTopics = db.GetFromDatabase<Topic>().ToList();
            
            CurriculumTemplateViewModel curriculumTemplateViewModel = new CurriculumTemplateViewModel { AlleTopics = theTopics };

            return View(curriculumTemplateViewModel);
        }

        // POST: Curriculum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CurriculumTemplateViewModel curriculumTemplateViewModel)
        {
            if (ModelState.IsValid)
            {
                CurriculumTemplate curriculumTemplate = curriculumTemplateViewModel.CurriculumTemplate;              

                foreach (int topicId in curriculumTemplateViewModel.TopicIds)
                {
                    Topic topic = db.GetFromDatabase<Topic>(topicId);
                    curriculumTemplate.Topics.Add(topic);
                }

                db.CurriculumTemplates.Add(curriculumTemplate);
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
            curriculumTemplateViewModel.AlleTopics = db.Topics.Where(t => !t.IsDeleted).ToList();
            curriculumTemplateViewModel.AlleTopics = db.Topics.ToList();
            return View(curriculumTemplateViewModel);
        }

        // GET: Curriculum/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
 
            CurriculumTemplate curriculumTemplate = db.GetFromDatabase<CurriculumTemplate>(id);
              
            if (curriculumTemplate == null)
                return HttpNotFound();

            CurriculumTemplateViewModel cvm = new CurriculumTemplateViewModel
            {
                CurriculumTemplate = curriculumTemplate,
                AlleTopics = db.GetFromDatabase<Topic>().ToList()
            };
            return View(cvm);
        }

        // POST: Curriculum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CurriculumTemplateViewModel curriculumTemplateViewModel)
        {
            if (ModelState.IsValid)
            {
                // Determine curriculum, with topics
                CurriculumTemplate curriculumTemplate = db.GetFromDatabase<CurriculumTemplate>(curriculumTemplateViewModel.CurriculumTemplate.Id);
                    
                // Add topic id's to list
                List<int> topicIds = new List<int>();
                foreach (var topic in curriculumTemplate.Topics)
                {
                    topicIds.Add(topic.Id);
                }

                // Remove topics from the curriculum, if topic is not selected in the curriculumViewModel
                foreach (var topicId in topicIds)
                {
                    if (!curriculumTemplateViewModel.TopicIds.Contains(topicId))
                    {
                        curriculumTemplate.Topics.Remove(curriculumTemplate.Topics.First(t => t.Id == topicId));
                    }
                }

                // Add topics to the curriculum, if topic is selected in the curriculumViewModel
                foreach (int topicId in curriculumTemplateViewModel.TopicIds)
                {
                    Topic topic = db.GetFromDatabase<Topic>(topicId);
                    
                     
                    // Check if curriculum exists, add if it does not.
                    
                    curriculumTemplate.Topics.Add(topic);
                    
                }
                db.Entry(curriculumTemplate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            curriculumTemplateViewModel.AlleTopics = db.Topics.ToList();
            return View(curriculumTemplateViewModel);
        }

        // GET: Curriculum/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurriculumTemplate curriculumTemplate = db.GetFromDatabase<CurriculumTemplate>(id);

            if (curriculumTemplate == null)
            {
                return HttpNotFound();
            }
            return View(curriculumTemplate);
        }

        // POST: Curriculum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CurriculumTemplate curriculumTemplate = db.GetFromDatabase<CurriculumTemplate>(id);
            db.CurriculumTemplates.Remove(curriculumTemplate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult GetPartialDisplayCurriculum(int id)
        {
            CurriculumTemplate curriculumTemplate = db.GetFromDatabase<CurriculumTemplate>(id);
            return PartialView("PartialDisplayCurriculum", curriculumTemplate);
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
