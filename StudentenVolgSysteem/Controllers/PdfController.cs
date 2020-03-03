using StudentenVolgSysteem.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using SelectListItem = System.Web.WebPages.Html.SelectListItem;
using StudentenVolgSysteem.DAL;
using StudentenVolgSysteem.Documents;

namespace StudentenVolgSysteem.Controllers
{
    [Authorize(Roles = "Administrator, Docent")]
    public class PdfController : Controller
    {
        SVSContext db = new SVSContext();

        [NonAction]
        private List<SelectListItem> GetCourses()
        {
            throw new NotImplementedException();
        }

        [NonAction]
        public List<SelectListItem> GetStudents()
        {
            List<SelectListItem> listStudents = new List<SelectListItem>();
            listStudents.Add(new SelectListItem { Text = "Select", Value = "0" });

            foreach (var item in db.Studenten)
            {
                listStudents.Add(new SelectListItem { Text = item.VolledigeNaam, Value = item.Id.ToString() });
            }
            return listStudents;
        }

        [NonAction]
        public List<SelectListItem> GetNiveaus()
        {
            List<SelectListItem> listNiveaus = new List<SelectListItem>();
            listNiveaus.Add(new SelectListItem { Text = "Select", Value = "0" });

            foreach (var item in db.Niveaus)
            {
                listNiveaus.Add(new SelectListItem { Text = item.Naam, Value = item.Id.ToString() });
            }
            return listNiveaus;
        }

        [NonAction]
        public SelectListItem GetCurriculum(int curriculumId)
        {
            Curriculum curriculum = db.Curricula.FirstOrDefault(o => o.Id == curriculumId);
            Student student = db.GetFromDatabase<Student>(curriculum.Id);
            SelectListItem curriculum_selectitem = new SelectListItem
            {
                Text = curriculum.Naam,
                Value = curriculum.Id.ToString()
            };
            
            return curriculum_selectitem;
        }

        [HttpGet]
        public ActionResult CreatePDF(int? curriculumId)
        {
            if (curriculumId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Curriculum curriculum = db.GetFromDatabase<Curriculum>(curriculumId);
            Student student = curriculum.Student;

            if (curriculum == null)
            {
                return HttpNotFound();
            }

            PdfViewModel pdfViewModel = new PdfViewModel();

            pdfViewModel.StudentId = student.Id;
            pdfViewModel.StudentNaam = student.VolledigeNaam;
            pdfViewModel.CurriculumId = curriculum.Id;
            pdfViewModel.NiveauList = GetNiveaus();
            pdfViewModel.Course = curriculum.Naam;
            //pdfViewModel.Niveau = student.Curriculum.
            
            return View(pdfViewModel);
        }

        [HttpPost]
        public ActionResult CreatePDF(PdfViewModel pdfViewModel)
        {
            pdfViewModel.NiveauList = GetNiveaus();
            if (ModelState.IsValid)
            {
                //Student student = db.Studenten.Where(o => o.Id == pdfViewModel.StudentId).First();
                Curriculum curriculum = db.Curricula.Where(o => o.Id == pdfViewModel.CurriculumId)
                                        .Include(a => a.Topics.Select(c => c.Topic.Duur))
                                        .Include(a => a.Topics.Select(c => c.Topic.Benodigdheden))
                                        .Include(a => a.Topics.Select(c => c.Topic.Werkvorm))
                                        .Include(a => a.Topics.Select(c => c.Topic.Niveau))
                                        .First();
                Student student = curriculum.Student;
                //Student student = db.Studenten.Include("")
                //Curriculum curriculum = db.Curricula.Include("Student").Include("Topics.Topic").Where(m => m.Id == pdfViewModel.CurriculumId).FirstOrDefault();
                PDF.Create(pdfViewModel, curriculum);
            }
            return View(pdfViewModel);
        }
    }
    }