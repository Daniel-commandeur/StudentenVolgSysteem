using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentenVolgSysteem.Models;
using StudentenVolgSysteem.Models.ViewModels;
using StudentenVolgSysteem.DAL;

namespace StudentenVolgSysteem.Controllers
{
    [Authorize(Roles = "Administrator, Docent")]
    public class TopicsController : Controller
    {
        private SVSContext db = new SVSContext();

        // GET: Topics
        public ActionResult Index()
        {
            var topics = db.Topics.Where(t => !t.IsDeleted)
                .Include("Niveau")
                .Include("Duur")
                .Include("Werkvorm")
                .Include("Certificeringen")
                .Include("Voorkennis")
                .Include("Benodigdheden")
                .Include("PercipioLinks")
                .Include("Tags")
                .ToList();
            return View(topics);
        }

        // GET: Topics/Details/5
        public ActionResult Details(int? id, string returnUrl)
        {
            ViewBag.returnurl = returnUrl;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.Topics.Include(m => m.Duur).Where(m => m.TopicId == id).FirstOrDefault();

            if (topic == null || topic.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Topics/Create
        public ActionResult Create()
        {
            TopicViewModel tvm = new TopicViewModel();

            FillTopicViewModelLists(tvm);

            return View(tvm);
        }

        // POST: Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TopicViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                Topic topic = ViewModelToTopic(tvm);

                db.Topics.Add(topic);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            FillTopicViewModelLists(tvm);

            return View(tvm);
        }

        // GET: Topics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Topic topic = db.Topics.Where(t => !t.IsDeleted)
                                   .Include(t => t.Duur)
                                   .Include(t => t.Niveau)
                                   .Include(t => t.PercipioLinks)
                                   .Include(t => t.Werkvorm)
                                   .FirstOrDefault(t => t.TopicId == id);

            if (topic == null || topic.IsDeleted)
            {
                return HttpNotFound();
            }

            TopicViewModel tvm = TopicToViewModel(topic);
            FillTopicViewModelLists(tvm);

            return View(tvm);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TopicViewModel tvm)
        {
            if (ModelState.IsValid)
            {
                Topic topic = db.Topics.Find(tvm.TopicId);
                topic = ViewModelToTopic(tvm);

                //Tell the context the topicModel has changed and save changes
                db.Entry(topic).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            // If the model is not valid, we need to refill the lists that were filtered by the bind.
            FillTopicViewModelLists(tvm);

            return View(tvm);
        }

        // GET: Topics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topicModel = db.GetFromDatabase<Topic>(id);
            if (topicModel == null || topicModel.IsDeleted)
            {
                return HttpNotFound();
            }
            return View(topicModel);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Topic topicModel = db.Topics.Include("CurriculumTopics").Include("Curricula").Where(t => t.TopicId == id).FirstOrDefault();
            db.Topics.Remove(topicModel);
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

        public ActionResult BackToPrevious(string returnUrl)
        {
            ViewBag.returnurl = returnUrl;
            string[] path = returnUrl.Split('/');
            if (path.Count() == 4)
            {
                return RedirectToAction(path[2], path[1], new { id = path[3] });
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Fills the lists in the TopicViewModel that populate dropdowns in the Topic/Create View.
        /// </summary>
        /// <param name="tvm">TopicViewModel</param>
        /// <returns>TopicViewModel with filled Lists</returns>
        public TopicViewModel FillTopicViewModelLists(TopicViewModel tvm)
        {
            tvm.AlleNiveaus = db.Niveaus.ToList();
            tvm.AlleTijdsduren = db.Tijdsduren.Where(item => !item.IsDeleted).ToList();
            tvm.AlleWerkvormen = db.Werkvormen.Where(item => !item.IsDeleted).ToList();
            tvm.AlleCertificeringen = db.Certificeringen.Where(item => !item.IsDeleted).ToList();
            tvm.AlleTags = db.Tags.Where(item => !item.IsDeleted).ToList();
            tvm.AlleBenodigdheden = db.Benodigdheden.Where(item => !item.IsDeleted).ToList();
            tvm.AllePercipioLinks = db.PercipioLinks.Where(item => !item.IsDeleted).ToList();
            tvm.AlleVoorkennis = db.Topics.Where(item => !item.IsDeleted).ToList();

            return tvm;
        }

        /// <summary>
        /// Converts a Topic to a TopicViewModel.
        /// </summary>
        /// <param name="t">Topic to convert</param>
        /// <returns>A TopicViewModel</returns>
        public TopicViewModel TopicToViewModel(Topic t)
        {
            TopicViewModel tvm = new TopicViewModel();

            tvm.Code = t.Code;
            tvm.Naam = t.Naam;
            tvm.Leerdoel = t.Leerdoel;
            tvm.Inhoud = t.Inhoud;

            tvm.TopicId = t.TopicId;
            tvm.Niveau = t.Niveau.NiveauId;
            tvm.Duur = t.Duur.TijdsduurId;
            tvm.Werkvorm = t.Werkvorm.WerkvormId;

            tvm.Certificeringen = new List<int>();
            foreach (Certificering c in t.Certificeringen)
            {
                tvm.Certificeringen.Add(c.CertificeringId);
            }

            tvm.Voorkennis = new List<int>();
            foreach (Topic c in t.Voorkennis)
            {
                tvm.Voorkennis.Add(c.TopicId);
            }

            tvm.Benodigdheden = new List<int>();
            foreach (Benodigdheid c in t.Benodigdheden)
            {
                tvm.Benodigdheden.Add(c.BenodigdheidId);
            }

            tvm.PercipioLinks = new List<int>();
            foreach (PercipioLink c in t.PercipioLinks)
            {
                tvm.PercipioLinks.Add(c.PercipioLinkId);
            }

            tvm.Tags = new List<int>();
            foreach (Tag c in t.Tags)
            {
                tvm.Tags.Add(c.TagId);
            }

            return tvm;
        }

        /// <summary>
        /// Converts a TopicViewModel back into a Topic.
        /// </summary>
        /// <param name="tvm">The TopicViewModel to convert</param>
        /// <returns>A Topic object</returns>
        public Topic ViewModelToTopic(TopicViewModel tvm)
        {
            Topic t = new Topic();

            if (db.Topics.Find(tvm.TopicId) != null)
            {
                t = db.Topics.Find(tvm.TopicId);
            }
            
            t.Code = tvm.Code;
            t.Naam = tvm.Naam;
            t.Leerdoel = tvm.Leerdoel;
            t.Inhoud = tvm.Inhoud;

            t.Niveau = db.Niveaus.Find(tvm.Niveau);
            t.Duur = db.Tijdsduren.Find(tvm.Duur);
            t.Werkvorm = db.Werkvormen.Find(tvm.Werkvorm);

            if (tvm.Certificeringen != null && tvm.Certificeringen.Count() != 0)
            {
                foreach (int c in tvm.Certificeringen)
                {
                    t.Certificeringen.Add(db.Certificeringen.Find(c));
                }
            }

            if (tvm.Voorkennis != null && tvm.Voorkennis.Count() != 0)
            {
                foreach (int v in tvm.Voorkennis)
                {
                    t.Voorkennis.Add(db.Topics.Find(v));
                }
            }

            if (tvm.Benodigdheden != null && tvm.Benodigdheden.Count() != 0)
            {
                foreach (int b in tvm.Benodigdheden)
                {
                    t.Benodigdheden.Add(db.Benodigdheden.Find(b));
                }
            }

            if (tvm.PercipioLinks != null && tvm.PercipioLinks.Count() != 0)
            {
                foreach (int p in tvm.PercipioLinks)
                {
                    t.PercipioLinks.Add(db.PercipioLinks.Find(p));
                }
            }

            if (tvm.Tags != null && tvm.Tags.Count() != 0)
            {
                foreach (int tag in tvm.Tags)
                {
                    t.Tags.Add(db.Tags.Find(tag));
                }
            }

            return t;
        }
    }
}
