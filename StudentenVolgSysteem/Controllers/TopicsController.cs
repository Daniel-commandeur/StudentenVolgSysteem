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
        public ActionResult Details(int? id)
        {
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
            CUTopicModel model = new CUTopicModel()
            {
                CUCertificeringenInfras = db.CertificeringenInfras.ToList(),
                CUNiveaus = db.Niveaus.ToList(),
                CUTags = db.Tags.ToList(),
                CUTijdsDuren = db.TijdsDuren.ToList(),
                CUwerkvormen = db.Werkvormen.ToList()
            };
            return View(model);
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

            CUTopicModel cuTopicModel = new CUTopicModel()
            {
                Benodigdheden = topicModel.Benodigdheden,
                Certificeringen = topicModel.Certificeringen,
                Code = topicModel.Code,
                Duur = topicModel.Duur,
                Inhoud = topicModel.Inhoud,
                Leerdoel = topicModel.Leerdoel,
                Name = topicModel.Name,
                Niveau = topicModel.Niveau,
                PercipioLinks = topicModel.PercipioLinks,
                Tags = topicModel.Tags,
                TopicId = topicModel.TopicId,
                Voorkennis = topicModel.Voorkennis,
                Werkvorm = topicModel.Werkvorm,
                CUCertificeringenInfras = db.CertificeringenInfras.ToList(),
                VoorkennisTopics = db.Topics.ToList(),
                CUNiveaus = db.Niveaus.ToList(),
                CUTags = db.Tags.ToList(),
                CUTijdsDuren = db.TijdsDuren.ToList(),
                CUwerkvormen = db.Werkvormen.ToList()
            };
            
            return View(cuTopicModel);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TopicId,Code,Name,Leerdoel,Inhoud")] TopicModel topicModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topicModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(topicModel);
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
    }
}
