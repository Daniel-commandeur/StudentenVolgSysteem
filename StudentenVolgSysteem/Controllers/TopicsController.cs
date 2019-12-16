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
    public class TopicsController : Controller
    {
        private SVSContext db = new SVSContext();

        // GET: Topics
        public ActionResult Index()
        {
            var tm = db.Topics.Where(t => !t.IsDeleted)
                .Include("Niveau")
                .Include("Duur")
                .Include("Werkvorm")
                .Include("Certificeringen")
                .Include("Voorkennis")
                .Include("Benodigdheden")
                .Include("PercipioLinks")
                .Include("Tags")
                .ToList();
            return View(tm);
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

            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        // GET: Topics/Create
        public ActionResult Create()
        {
            CUTopic cuTopic = new CUTopic();
            cuTopic.CUNiveaus = db.Niveaus.ToList();
            cuTopic.CUTijdsduren = db.Tijdsduren.ToList();
            cuTopic.CUWerkvormen = db.Werkvormen.ToList();
            cuTopic.CUCertificeringen = db.Certificeringen.ToList();
            cuTopic.CUTags = db.Tags.ToList();
            cuTopic.CUBenodigdheden = db.Benodigdheden.ToList();
            cuTopic.CUPercipioLinks = db.PercipioLinks.ToList();
            cuTopic.CUVoorkennis = db.Topics.ToList();
            return View(cuTopic);
        }

        // POST: Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TopicId,Code,NiveauId,Naam,TijdsduurId,WerkvormId,Leerdoel,CertificeringIds,VoorkennisIds,Inhoud,BenodigdhedenIds,PercipioLinkIds")] CUTopic cuTopic)
        {
            if (ModelState.IsValid)
            {
                Topic topic = new Topic();
                topic.CopyCUTopicToTopic(cuTopic, db);

                db.Topics.Add(topic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            cuTopic.CUNiveaus = db.Niveaus.ToList();
            cuTopic.CUTijdsduren = db.Tijdsduren.ToList();
            cuTopic.CUWerkvormen = db.Werkvormen.ToList();
            cuTopic.CUCertificeringen = db.Certificeringen.ToList();
            cuTopic.CUTags = db.Tags.ToList();
            cuTopic.CUBenodigdheden = db.Benodigdheden.ToList();
            cuTopic.CUPercipioLinks = db.PercipioLinks.ToList();
            cuTopic.CUVoorkennis = db.Topics.ToList();
            return View(cuTopic);
        }

        // GET: Topics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Topic topicModel = db.Topics.Where(t => !t.IsDeleted)
                                             .Include(t => t.Duur)
                                             .Include(t => t.Niveau)
                                             .Include(t => t.PercipioLinks)
                                             .Include(t => t.Werkvorm)
                                             .FirstOrDefault(t => t.TopicId == id);

            if (topicModel == null || topicModel.IsDeleted)
            {
                return HttpNotFound();
            }

            CUTopic cuTopicModel = new CUTopic(topicModel);

            return View(cuTopicModel);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TopicId,Code,NiveauId,Naam,TijdsduurId,WerkvormId,Leerdoel,CertificeringIds,VoorkennisIds,Inhoud,BenodigdhedenIds,PercipioLinkIds,TagIds")] CUTopic cuTopic)
        {
            if (ModelState.IsValid)
            {
                Topic topicModel = db.Topics.Find(cuTopic.TopicId);
                topicModel.CopyCUTopicToTopic(cuTopic, db);
                
                ////Make changes to the topicModel here, we have to get all the models from side-tables from the dbcontext by reference.
                ////Otherwise EF will think these are new objects
                //topicModel.Code = cuTopicModel.Code;
                //topicModel.Niveau = db.Niveaus.FirstOrDefault(n => n.NiveauId.ToString() == cuTopicModel.NiveauId);
                //topicModel.Name = cuTopicModel.Name;
                //topicModel.Duur = db.TijdsDuren.FirstOrDefault(d => d.TijdsDuurId.ToString() == cuTopicModel.TijdsDuurId);
                //topicModel.Werkvorm = db.Werkvormen.FirstOrDefault(w => w.WerkvormId.ToString() == cuTopicModel.WerkvormId);
                //topicModel.Leerdoel = cuTopicModel.Leerdoel;
                //topicModel.Inhoud = cuTopicModel.Inhoud;

                ////Certs
                //topicModel.Certificeringen.Clear();
                //try
                //{
                //    foreach (var certId in cuTopicModel.CertificeringIds)
                //    {
                //        topicModel.Certificeringen.Add(db.CertificeringenInfras.Where(c => c.CertificeringenInfraId.ToString() == certId).FirstOrDefault());
                //    }
                //}
                //catch (NullReferenceException e)
                //{

                //}

                ////Voorkennis
                //topicModel.Voorkennis.Clear();
                //try
                //{
                //    foreach (var voorkennisId in cuTopicModel.VoorkennisIds)
                //    {
                //        topicModel.Voorkennis.Add(db.Topics.Where(t => t.TopicId.ToString() == voorkennisId).FirstOrDefault());
                //    }
                //}
                //catch (NullReferenceException e)
                //{

                //}

                ////Benodigdheden
                //topicModel.Benodigdheden.Clear();
                //try
                //{
                //    foreach (var benodigdheidsId in cuTopicModel.BenodigdhedenIds)
                //    {
                //        topicModel.Benodigdheden.Add(db.Benodigdheden.Where(b => b.BenodigdheidId.ToString() == benodigdheidsId).FirstOrDefault());
                //    }
                //}
                //catch (NullReferenceException e)
                //{

                //}

                ////Percipiolinks
                //topicModel.PercipioLinks.Clear();
                //try
                //{
                //    foreach (var percipioId in cuTopicModel.PercipiolinkIds)
                //    {
                //        topicModel.PercipioLinks.Add(db.PercipioLinks.Where(p => p.PercipiolinkId.ToString() == percipioId).FirstOrDefault());
                //    }
                //}
                //catch (NullReferenceException e)
                //{

                //}

                ////Tags
                //topicModel.Tags.Clear();
                //try
                //{
                //    foreach (var tagId in cuTopicModel.TagIds)
                //    {
                //        topicModel.Tags.Add(db.Tags.Where(t => t.TagId.ToString() == tagId).FirstOrDefault());
                //    }
                //}
                //catch (NullReferenceException e)
                //{

                //}

                //Tell the context the topicModel has changed and save changes
                db.Entry(topicModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //If the model is not valid we need to re-do all the things that were filtered out by the bind
            cuTopic.CUNiveaus = db.Niveaus.ToList();
            cuTopic.CUTijdsduren = db.Tijdsduren.ToList();
            cuTopic.CUWerkvormen = db.Werkvormen.ToList();
            cuTopic.CUCertificeringen = db.Certificeringen.ToList();
            cuTopic.CUTags = db.Tags.ToList();
            cuTopic.CUBenodigdheden = db.Benodigdheden.ToList();
            cuTopic.CUPercipioLinks = db.PercipioLinks.ToList();
            cuTopic.CUVoorkennis = db.Topics.ToList();
            return View(cuTopic);
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
            Topic topicModel = db.GetFromDatabase<Topic>(id);
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
    }
}
