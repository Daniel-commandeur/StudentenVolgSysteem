using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentenVolgSysteem.Models;

namespace StudentenVolgSysteem.Controllers
{
    [Authorize(Roles = "Administrator, Docent")]
    public class TopicsController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Topics
        public ActionResult Index()
        {
            var tm = db.Topics
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
            TopicModel topicModel = db.Topics.Include(m => m.Duur).Where(m => m.TopicId == id).FirstOrDefault();

            if (topicModel == null)
            {
                return HttpNotFound();
            }
            return View(topicModel);
        }

        // GET: Topics/Create
        public ActionResult Create()
        {
            CUTopicModel cuTopicModel = new CUTopicModel();
            cuTopicModel.CUNiveaus = db.Niveaus.ToList();
            cuTopicModel.CUTijdsDuren = db.TijdsDuren.ToList();
            cuTopicModel.CUwerkvormen = db.Werkvormen.ToList();
            cuTopicModel.CUCertificeringenInfras = db.CertificeringenInfras.ToList();
            cuTopicModel.CUTags = db.Tags.ToList();
            cuTopicModel.CUBenodigdheden = db.Benodigdheden.ToList();
            cuTopicModel.CUPercipiolinks = db.PercipioLinks.ToList();
            cuTopicModel.VoorkennisTopics = db.Topics.ToList();
            return View(cuTopicModel);
        }

        // POST: Topics/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TopicId,Code,Name,Leerdoel,Inhoud")] TopicModel topicModel)
        {
            if (ModelState.IsValid)
            {
                db.Topics.Add(topicModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(topicModel);
        }

        // GET: Topics/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TopicModel topicModel = db.Topics.Find(id);

            if (topicModel == null)
            {
                return HttpNotFound();
            }

            CUTopicModel cuTopicModel = new CUTopicModel(topicModel);

            return View(cuTopicModel);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TopicId,Code,NiveauId,Name,TijdsDuurId,WerkvormId,Leerdoel,CertificeringIds,VoorkennisIds,Inhoud,BenodigdhedenIds,PercipiolinkIds,TagIds")] CUTopicModel cuTopicModel)
        {
            if (ModelState.IsValid)
            {
                TopicModel topicModel = db.Topics.Find(cuTopicModel.TopicId);

                //Make changes to the topicModel here, we have to get all the models from side-tables from the dbcontext by reference.
                //Otherwise EF will think these are new objects
                topicModel.Code = cuTopicModel.Code;
                topicModel.Niveau = db.Niveaus.Where(n => n.Niveau == cuTopicModel.Niveau.Niveau).FirstOrDefault();
                topicModel.Name = cuTopicModel.Name;
                topicModel.Duur = db.TijdsDuren.Where(d => d.Eenheid == cuTopicModel.Duur.Eenheid).FirstOrDefault();
                topicModel.Werkvorm = db.Werkvormen.Where(w => w.Werkvorm == cuTopicModel.Werkvorm.Werkvorm).FirstOrDefault();
                topicModel.Leerdoel = cuTopicModel.Leerdoel;
                topicModel.Inhoud = cuTopicModel.Inhoud;

                //Certs
                topicModel.Certificeringen.Clear();
                foreach (var certId in cuTopicModel.CertificeringIds)
                {
                    topicModel.Certificeringen.Add(db.CertificeringenInfras.Where(c => c.CertificeringenInfraId.ToString() == certId).FirstOrDefault());
                }

                //Voorkennis
                topicModel.Voorkennis.Clear();
                foreach (var voorkennisId in cuTopicModel.VoorkennisIds)
                {
                    topicModel.Voorkennis.Add(db.Topics.Where(t => t.TopicId.ToString() == voorkennisId).FirstOrDefault());
                }

                //Benodigdheden
                topicModel.Benodigdheden.Clear();
                foreach (var benodigdheidsId in cuTopicModel.BenodigdhedenIds)
                {
                    topicModel.Benodigdheden.Add(db.Benodigdheden.Where(b => b.BenodigdheidId.ToString() == benodigdheidsId).FirstOrDefault());
                }

                //Percipiolinks
                topicModel.PercipioLinks.Clear();
                foreach (var percipioId in cuTopicModel.PercipiolinkIds)
                {
                    topicModel.PercipioLinks.Add(db.PercipioLinks.Where(p => p.PercipiolinkId.ToString() == percipioId).FirstOrDefault());
                }

                //Tags
                topicModel.Tags.Clear();
                foreach (var tagId in cuTopicModel.TagIds)
                {
                    topicModel.Tags.Add(db.Tags.Where(t => t.TagId.ToString() == tagId).FirstOrDefault());
                }

                //Tell the context the topicModel has changed and save changes
                db.Entry(topicModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //If the model is not valid we need to re-do all the things that were filtered out by the bind
            cuTopicModel.CUNiveaus = db.Niveaus.ToList();
            cuTopicModel.CUTijdsDuren = db.TijdsDuren.ToList();
            cuTopicModel.CUwerkvormen = db.Werkvormen.ToList();
            cuTopicModel.CUCertificeringenInfras = db.CertificeringenInfras.ToList();
            cuTopicModel.CUTags = db.Tags.ToList();
            cuTopicModel.CUBenodigdheden = db.Benodigdheden.ToList();
            cuTopicModel.CUPercipiolinks = db.PercipioLinks.ToList();
            cuTopicModel.VoorkennisTopics = db.Topics.ToList();
            return View(cuTopicModel);
        }

        // GET: Topics/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TopicModel topicModel = db.Topics.Find(id);
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
            TopicModel topicModel = db.Topics.Find(id);
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
                return RedirectToAction(returnUrl);
            }
            
        }
    }
}
