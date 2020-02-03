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
            List<Topic> topicIndexViewModel = new List<Topic>();
            var topics = db.GetFromDatabase<Topic>();

            topics.ToList().ForEach(topic =>
            {
                topicIndexViewModel.Add(new Topic()
                {
                    Naam = topic.Naam,
                    Id = topic.Id,
                    Code = topic.Code,
                    Duur = topic.Duur == null ? new Tijdsduur() : topic.Duur.IsDeleted  ? new Tijdsduur() { } : topic.Duur,
                    Werkvorm = topic.Werkvorm.IsDeleted ? new Werkvorm() { } : topic.Werkvorm,
                    Niveau = topic.Niveau == null ? new Niveau() : topic.Niveau.IsDeleted ? new Niveau() { } : topic.Niveau,
                    Certificeringen = topic.Certificeringen.Where(x => !x.IsDeleted).ToList(),
                });
            });

            if (topics == null)
            {
                return HttpNotFound();
            }

            return View(topicIndexViewModel);
        }

        // GET: Topics/Details/5
        public ActionResult Details(int? id, string returnUrl)
        {
            ViewBag.returnurl = returnUrl;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic topic = db.GetFromDatabase<Topic>(id);

            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Topics/Create
        public ActionResult Create(int? id)
        {
            TopicViewModel topicViewModel = new TopicViewModel();

            Topic topic = new Topic();

            topicViewModel.Topic = topic;
            FillTopicViewModelLists(topicViewModel); //
            return View(topicViewModel);
        }

        // POST: Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TopicViewModel topicViewModel)
        {
            if (ModelState.IsValid)
            {
                Topic topic = new Topic
                {
                    Code = topicViewModel.Topic.Code,
                    Naam = topicViewModel.Topic.Naam,
                    Inhoud = topicViewModel.Topic.Inhoud,
                    Leerdoel = topicViewModel.Topic.Leerdoel,
                    Werkvorm = db.Werkvormen.Find(topicViewModel.Topic.Werkvorm.Id),
                    Niveau = db.Niveaus.Find(topicViewModel.Topic.Niveau.Id),
                    Duur = db.Tijdsduren.Find(topicViewModel.Topic.Duur.Id)
                };

                topic.Benodigdheden = UpdateList(topicViewModel.BenodigdheidIds,  topic.Benodigdheden);              
                topic.Certificeringen = UpdateList(topicViewModel.CertificeringenIds, topic.Certificeringen);
                topic.Voorkennis = UpdateList(topicViewModel.VoorkennisIds, topic.Voorkennis);
                topic.PercipioLinks = UpdateList(topicViewModel.PercipioLinkIds, topic.PercipioLinks);

                db.Topics.Add(topic);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            FillTopicViewModelLists(topicViewModel);

            return View(topicViewModel);
        }

        // GET: Topics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Topic topic = db.GetFromDatabase<Topic>(id);

            if (topic == null)
            {
                return HttpNotFound();
            }

            TopicViewModel tvm = new TopicViewModel();
            tvm.Topic = topic;
            FillTopicViewModelLists(tvm);

            return View(tvm);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TopicViewModel topicViewModel)
        {
            if (ModelState.IsValid)
            {
                Topic topic = db.GetFromDatabase<Topic>(topicViewModel.Topic.Id);

                topic.Code = topicViewModel.Topic.Code;
                topic.Inhoud = topicViewModel.Topic.Inhoud;
                topic.Naam = topicViewModel.Topic.Naam;
                topic.Leerdoel = topicViewModel.Topic.Leerdoel;

                topic.Werkvorm = db.GetFromDatabase<Werkvorm>(topicViewModel.Topic.Werkvorm.Id);
                topic.Niveau = db.GetFromDatabase<Niveau>(topicViewModel.Topic.Niveau.Id);
                topic.Duur = db.GetFromDatabase<Tijdsduur>(topicViewModel.Topic.Duur.Id);

                topic.Benodigdheden = UpdateList(topicViewModel.BenodigdheidIds, topic.Benodigdheden);
                topic.Certificeringen = UpdateList(topicViewModel.CertificeringenIds, topic.Certificeringen);             
                topic.Voorkennis = UpdateList(topicViewModel.VoorkennisIds, topic.Voorkennis);
                topic.PercipioLinks = UpdateList(topicViewModel.PercipioLinkIds, topic.PercipioLinks);

                //Tell the context the topicModel has changed and save changes
                db.Entry(topic).State = EntityState.Modified;
             
                try
                {
                    //if (TryUpdateModel(topicViewModel.Topic, "Topic", new[] { topicViewModel.Topic.Benodigdheden.ToString() }))
                    //{
                    //    if(TryUpdateModel(topicViewModel.Topic.Werkvorm, "", new[] { "" }))
                    //    {

                    //    }
                    //    db.SaveChanges();
                    //}
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "There was an error updating the database");
                }

                return RedirectToAction("Index");
            }

            // If the model is not valid, we need to refill the lists that were filtered by the bind.
            TopicViewModel tvm = new TopicViewModel { Topic = topicViewModel.Topic };
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
            if (topicModel == null)
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
            Topic topic = db.GetFromDatabase<Topic>(id);
            //Topic topicModel = db.Topics.Include("Curricula").Include("Voorkennis").Where(t => t.Id == id).FirstOrDefault();
            db.Topics.Remove(topic);
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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <param name="oldList"></param>
        public ICollection<T> UpdateList<T>(int[] ids, ICollection<T> oldList) where T : class, IDeletable
        {
            List<T> newList = new List<T>();
            if (ids == null)
            {
                if (oldList.Count != 0)
                {
                    return newList;
                }
            }
            else
            {
                foreach (int id in ids)
                {
                    T item = db.GetFromDatabase<T>(id);
                    if (item != null)
                    {
                        newList.Add(item);
                        if (!oldList.Contains(item))
                        {
                            oldList.Add(item);
                        }
                    }
                }
            }

            if (oldList.Count > newList.Count)
            {
                oldList = newList;
            }
            return oldList;
        }

        public List<T> ListUpdate<T>(IEnumerable<int> ids) where T : class, IDeletable
        {
            List<T> list = new List<T>();
            using (IEnumerator<int> emunerator = ids.GetEnumerator())
            {
                while (emunerator.MoveNext())
                {
                    var id = emunerator.Current;
                    list.Add(db.GetFromDatabase<T>(id));
                }
            }
            return list;
        }

        // topic verwijderd, curriculumtopic niet,

        // student verwijderd, 

        /// <summary>
        /// Fills the lists in the TopicViewModel that populate dropdowns in the Topic/Create View.
        /// </summary>
        /// <param name="tvm">TopicViewModel</param>
        /// <returns>TopicViewModel with filled Lists</returns>
        public TopicViewModel FillTopicViewModelLists(TopicViewModel tvm)
        {
            tvm.AlleNiveaus = db.GetFromDatabase<Niveau>();
            tvm.AlleTijdsduren = db.GetFromDatabase<Tijdsduur>();
            tvm.AlleWerkvormen = db.GetFromDatabase<Werkvorm>();
            tvm.AlleCertificeringen = db.GetFromDatabase<Certificering>();
            tvm.AlleTags = db.GetFromDatabase<Tag>();
            tvm.AlleBenodigdheden = db.Benodigdheden.Where(item => !item.IsDeleted).ToList();
            tvm.AllePercipioLinks = db.PercipioLinks.Where(item => !item.IsDeleted).ToList();
            tvm.AlleVoorkennis = db.Topics.Where(item => !item.IsDeleted).ToList();

            return tvm;
        }

    }
}
