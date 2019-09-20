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
    public class CuriculumController : Controller
    {
        private MyDbContext db = new MyDbContext();

        // GET: Curiculum
        public ActionResult Index()
        {
            return View(db.Curiculums.ToList());
        }

        // GET: Curiculum/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CuriculumModel curiculumModel = db.Curiculums.Find(id);
            if (curiculumModel == null)
            {
                return HttpNotFound();
            }
            return View(curiculumModel);
        }

        // GET: Curiculum/Create
        public ActionResult Create()
        {
            List<TopicModel> theTopics = db.Topics.ToList();
            CUCuriculumModel cuc = new CUCuriculumModel() { AllTopics = theTopics };
            return View(cuc);
        }

        // POST: Curiculum/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CuriculumId,StudentId,Topics,allTopicIds,Name")] CUCuriculumModel curiculumModel)
        {
            if (ModelState.IsValid)
            {
                CuriculumModel cm = new CuriculumModel();
                cm.Name = curiculumModel.Name;
                cm.StudentId = curiculumModel.StudentId;
                foreach (var topic in curiculumModel.allTopicIds)
                {
                    cm.Topics.Add(db.Topics.Where(a => a.TopicId.ToString() == topic).FirstOrDefault());
                }
                db.Curiculums.Add(cm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            curiculumModel.AllTopics = db.Topics.ToList();
            return View(curiculumModel);
        }

        // GET: Curiculum/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CuriculumModel curiculumModel = db.Curiculums.Find(id);
            if (curiculumModel == null)
            {
                return HttpNotFound();
            }
            CUCuriculumModel cuc = new CUCuriculumModel(curiculumModel);
            cuc.AllTopics = db.Topics.ToList();
            return View(cuc);
        }

        // POST: Curiculum/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CuriculumId,StudentId,Topics,Name,AllTopics,allTopicIds")] CUCuriculumModel curiculumModel)
        {
            if (ModelState.IsValid)
            {
                CuriculumModel cm = db.Curiculums.Find(curiculumModel.CuriculumId);
                foreach (var topic in curiculumModel.allTopicIds)
                {
                    cm.Topics.Add(db.Topics.Where(a => a.TopicId.ToString() == topic).FirstOrDefault());
                }
                db.Entry(cm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            curiculumModel.AllTopics = db.Topics.ToList();
            return View(curiculumModel);
        }

        // GET: Curiculum/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CuriculumModel curiculumModel = db.Curiculums.Find(id);
            if (curiculumModel == null)
            {
                return HttpNotFound();
            }
            return View(curiculumModel);
        }

        // POST: Curiculum/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CuriculumModel curiculumModel = db.Curiculums.Find(id);
            db.Curiculums.Remove(curiculumModel);
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
