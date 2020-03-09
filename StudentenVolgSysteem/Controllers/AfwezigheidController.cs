using StudentenVolgSysteem.DAL;
using StudentenVolgSysteem.Models;
using StudentenVolgSysteem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace StudentenVolgSysteem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AfwezigheidController : Controller
    {
        public SVSContext db = new SVSContext();

        // GET: Afwezigheid
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DeelnemerList()
        {
            var studenten = db.Studenten.Where(s => s.IsDeleted == false).ToList();
            var date_monday = firstDayOfWeek(DateTime.Today);

            var date_format = "ddd dd-MM-yyyy";
            var dates = new List<string>();
            foreach (var item in daysOfWeek(firstDayOfWeek(DateTime.Today)))
            {
                dates.Add(item.ToString(date_format));
            };
            var afwezigheden = db.Afwezigheden.Where(a => a.IsDeleted == false).ToList();
            var AOpties = db.AfwezigheidOptions.Where(ao => ao.IsDeleted == false).ToList();

            return View(new DeelnemerAfwezigheidListViewModel()
            {
                Dates = dates,
                OptionsSelectList = AOpties.Select(a => new SelectListItem
                {
                    Text = a.Naam,
                    Value = a.Id.ToString(),
                   
                }),
                Students = studenten,
                Afwezigheid = afwezigheden
            });
        }

        [HttpPost]
        public ActionResult DeelnemerList(int selected_Value, int studentId, int dateId)
        {
            DateTime selectedDate = daysOfWeek(firstDayOfWeek(DateTime.Today))[dateId];
            Debug.WriteLine($"{selected_Value} --- {studentId} --- {selectedDate}");
            db.Afwezigheden.Add(new AfwezigheidModel() {
                
                Date = selectedDate,
                OptionsId = selected_Value,
                DeelnemerId = studentId,
            });
            db.SaveChanges();
            return RedirectToAction("DeelnemerList");
        }

        public ActionResult AddAfwezigheidOptions()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAfwezigheidOptions(AfwezigheidOptionsModel insertedOption)
        {
            if (insertedOption == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.AfwezigheidOptions.Add(insertedOption);
            db.SaveChanges();
            ModelState.Clear();
            return View();
        }

        public DateTime firstDayOfWeek(DateTime dateTime)
        {
            switch (dateTime.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return dateTime.AddDays(-6);

                case DayOfWeek.Monday:
                    return dateTime.AddDays(-0);

                case DayOfWeek.Tuesday:
                    return dateTime.AddDays(-1);

                case DayOfWeek.Wednesday:
                    return dateTime.AddDays(-2);

                case DayOfWeek.Thursday:
                    return dateTime.AddDays(-3);

                case DayOfWeek.Friday:
                    return dateTime.AddDays(-4);

                case DayOfWeek.Saturday:
                    return dateTime.AddDays(-5);

                default:
                    throw new ArgumentException();
            }
        }

        public List<DateTime> daysOfWeek(DateTime beginDate)
        {
            var tempList = new List<DateTime>()
            {
                beginDate,
                beginDate.AddDays(1),
                beginDate.AddDays(2),
                beginDate.AddDays(3),
                beginDate.AddDays(4)
            };
            return tempList;
        }
    }
}
