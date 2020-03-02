using StudentenVolgSysteem.DAL;
using StudentenVolgSysteem.Models;
using StudentenVolgSysteem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StudentenVolgSysteem.Controllers
{
    [Authorize(Roles = "Administrator, Docent")]
    public class CurriculumController : Controller
    {
        private SVSContext db = new SVSContext();

        //GET: Curriculum
        public ActionResult Index()
        {
            return new HttpStatusCodeResult(HttpStatusCode.MethodNotAllowed);
        }
        
        public ActionResult Create(int? id)
        { 
            if(id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            CurriculumViewModel curriculumViewModel = new CurriculumViewModel {
                CurriculumTemplates = db.GetFromDatabase<CurriculumTemplate>().ToList(),
                Niveaus = db.GetFromDatabase<Niveau>().ToList(),
                StudentId = (int)id,
                Student = db.GetFromDatabase<Student>(id)
            };

            return View(curriculumViewModel);
            
        }

        //POST: Curriculum/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,NiveauId,CurriculumTemplateId")] CurriculumViewModel curriculumViewModel)
        {
            if (ModelState.IsValid)
            {
                Student student = db.GetFromDatabase<Student>(curriculumViewModel.StudentId);
                int NiveauId = curriculumViewModel.NiveauId;
                var curriculumTemplate = db.GetFromDatabase<CurriculumTemplate>(curriculumViewModel.CurriculumTemplateId);
                var topics = curriculumTemplate.Topics;
                Curriculum curriculum = new Curriculum();
                curriculum.Naam = curriculumTemplate.Naam;
                
                foreach (var topic in topics)
                {
                    var topic1 = db.GetFromDatabase<Topic>(topic.Id);

                    if (topic1.Niveau.Id <= NiveauId)
                    {
                        CurriculumTopic t = new CurriculumTopic { TopicId = topic.Id, Topic = topic, Curriculum = curriculum, CurriculumId = curriculum.Id };
                        curriculum.Topics.Add(t);
                    }
                }


                curriculum.Student = student;
                student.Curricula.Add(curriculum);
                db.Curricula.Add(curriculum);
                db.Entry(student).State = EntityState.Modified;

                db.SaveChanges();

                //// COMMENT: This code can probably be removed
                //if (curriculumViewModel.StudentId != 0)
                //  return RedirectToAction("Details", "Student", new { id = curriculumViewModel.StudentId });
                

                return RedirectToAction("Details", "Student", new { id = curriculumViewModel.StudentId });
            }
            return View();
            //curriculumTemplateViewModel.AlleTopics = db.Topics.Where(t => !t.IsDeleted).ToList();
            //curriculumTemplateViewModel.AlleTopics = db.Topics.ToList();
            //return View(curriculumTemplateViewModel);
        }

        // GET: Curriculum/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculum curriculum = db.GetFromDatabase<Curriculum>(id);
            

            if (curriculum == null)
            {
                return HttpNotFound();
            }

            EditCurriculumViewModel cvm = new EditCurriculumViewModel
            {
                Curriculum = curriculum,
                AlleTopics = db.GetFromDatabase<Topic>().ToList()
            };
            return View(cvm);
        }


        // POST: Curriculum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditCurriculumViewModel cvm)
        {
            if (ModelState.IsValid)
            {
                Curriculum curriculum = db.Curricula.Where(o => o.Id == cvm.Curriculum.Id)
                                                    .Include(o => o.Student)
                                                    .FirstOrDefault();
                //Curriculum curriculum = db.GetFromDatabase<Curriculum>(cvm.Curriculum.Id);
                curriculum.Naam = cvm.Curriculum.Naam;
                var oldList = curriculum.Topics;

                List <Topic> newList = new List<Topic>();
                foreach(int id in cvm.TopicIds)
                {
                    newList.Add(db.GetFromDatabase<Topic>(id));
                    var curriculumTopic = db.CurriculumTopics.Where(ct => ct.Topic.Id == id && ct.Curriculum.Id == curriculum.Id).FirstOrDefault();
                    // if null means a new curriculumtopic needs to be added.
                    if(curriculumTopic == null)
                    {
                        Topic topic = db.GetFromDatabase<Topic>(id);
                        CurriculumTopic t = new CurriculumTopic { TopicId = topic.Id, Topic = topic, Curriculum = curriculum, CurriculumId = curriculum.Id };
                        oldList.Add(t);
                    }
                }

                // oldList has more items than newlist so we need to remove items.
                if(oldList.Count > newList.Count)
                {
                    List<CurriculumTopic> removeList = new List<CurriculumTopic>();
                    foreach(var curriculumTopic1 in oldList)
                    {
                        var a = newList.Where(t => t.Id == curriculumTopic1.TopicId).FirstOrDefault();
                        if (a == null)
                        {
                            // verwijder curriculumtopic uit oudelijst
                            var curriculumTopic = db.CurriculumTopics.Where(ct => ct.Curriculum.Id == curriculum.Id && ct.Topic.Id == curriculumTopic1.TopicId).FirstOrDefault();
                            removeList.Add(curriculumTopic);

                        }
                    }
                    foreach(var item in removeList)
                        oldList.Remove(item);
                }
                
                db.Entry(curriculum).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Student", new { id = curriculum.Student.Id });
            }
            //curriculumTemplateViewModel.AlleTopics = db.Topics.ToList();
            return View();
        }


    }
}