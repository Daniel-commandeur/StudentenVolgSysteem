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
          
            var topics = db.GetFromDatabase<Topic>();
            if (topics == null)
            {
                return HttpNotFound();
            }

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

                var benodigheden = topicViewModel.Topic.Benodigdheden;

                if (topicViewModel.BenodigdheidIds != null)
                {
                    UpdateList(topicViewModel.BenodigdheidIds, ref benodigheden);
                }
                topic.Benodigdheden = benodigheden;

                var certificeringen = topicViewModel.Topic.Certificeringen;

                if (topicViewModel.CertificeringenIds != null)
                {
                    UpdateList(topicViewModel.CertificeringenIds, ref certificeringen);
                }

                topic.Certificeringen = certificeringen;

                var voorkennis = topicViewModel.Topic.Voorkennis;

                if (topicViewModel.VoorkennisIds != null)
                {
                    UpdateList(topicViewModel.VoorkennisIds, ref voorkennis);
                }

                topic.Voorkennis = voorkennis;

                var percipioLinks = topicViewModel.Topic.PercipioLinks;

                if (topicViewModel.PercipioLinkIds != null)
                {
                    UpdateList(topicViewModel.PercipioLinkIds, ref percipioLinks);
                }

                topic.PercipioLinks = percipioLinks;

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
                Topic topic1 = db.GetFromDatabase<Topic>(topicViewModel.Topic.Id);

                // TODO: Refractor topic copy

                //Topic topic1 = db.Topics.Find(topicViewModel.Topic.TopicId);
                //topic1 = topicViewModel.Topic.ShallowCopy();
                topic1.Code = topicViewModel.Topic.Code;
                topic1.Inhoud = topicViewModel.Topic.Inhoud;
                topic1.Naam = topicViewModel.Topic.Naam;
                topic1.Leerdoel = topicViewModel.Topic.Leerdoel;
                
                topic1.Werkvorm = db.GetFromDatabase<Werkvorm>(topicViewModel.Topic.Werkvorm.Id);
                topic1.Niveau = db.GetFromDatabase<Niveau>(topicViewModel.Topic.Niveau.Id);              
                topic1.Duur = db.GetFromDatabase<Tijdsduur>(topicViewModel.Topic.Duur.Id);

                var benodigheden = topic1.Benodigdheden;

                if (topicViewModel.BenodigdheidIds != null)
                {
                    UpdateList(topicViewModel.BenodigdheidIds, ref benodigheden);
                }
                topic1.Benodigdheden = benodigheden;

                var certificeringen = topic1.Certificeringen;

                if (topicViewModel.CertificeringenIds != null)
                {
                    UpdateList(topicViewModel.CertificeringenIds, ref certificeringen);
                }

                topic1.Certificeringen = certificeringen;

                var voorkennis = topic1.Voorkennis;

                if(topicViewModel.VoorkennisIds != null) {
                    UpdateList(topicViewModel.VoorkennisIds, ref voorkennis);    
                }

                topic1.Voorkennis = voorkennis;

                var percipioLinks = topic1.PercipioLinks;

                if (topicViewModel.PercipioLinkIds != null)
                {
                    UpdateList(topicViewModel.PercipioLinkIds, ref percipioLinks);
                }

                topic1.PercipioLinks = percipioLinks;
             
                //Tell the context the topicModel has changed and save changes
                db.Entry(topic1).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch(Exception ex)
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
        public void UpdateList<T>(int[] ids, ref ICollection<T> oldList) where T: class, IDeletable 
        {
            List<T> newList = new List<T>();
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
            if(oldList.Count > newList.Count)
            {
                oldList = newList;
            }                
        }

        public List<T> ListUpdate<T>(IEnumerable<int> ids) where T: class,IDeletable
        {
            List<T> list = new List<T>();
            using (IEnumerator<int> emunerator =  ids.GetEnumerator())
            {
                while(emunerator.MoveNext())
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

    }
}
