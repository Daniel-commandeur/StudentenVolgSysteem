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
    public class AanwezigheidController : Controller
    {
        public SVSContext db = new SVSContext();

        // GET: Afwezigheid
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DeelnemerList()
        {
            //string date_format = "ddd dd-MM-yyyy";
            //date = date == null ? DateTime.Today : date;
            DateTime date = DateTime.Today;
            //List<PresentieEntryModel> presentieEntrys = new List<PresentieEntryModel>();
            List<Student> deelnemers = db.Studenten.Where(s => s.IsDeleted == false).ToList();
            // Todo: Make it possible to chose different weeks
            DateTime date_monday = firstDayOfWeek(date);
            var presentieEntryRows = new List<PresentieEntryRow>();


            //var dates = new List<string>();
            foreach (var deelnemer in deelnemers)
            {
                presentieEntryRows.Add(new PresentieEntryRow
                {
                    PresentieEntrys = new List<PresentieEntryModel>()
                    
                });


                foreach (var day in daysOfWeek(date_monday))
                {
                    var presentieEntry = db.PresentieEntrys.Where(e => e.Date == day).Where(d => d.Deelnemer.Id == deelnemer.Id).FirstOrDefault();
                    if (presentieEntry != null)
                    {
                        presentieEntryRows.Last().PresentieEntrys.Add(presentieEntry);
                    }
                    else
                    {
                        presentieEntryRows.Last().PresentieEntrys.Add(new PresentieEntryModel 
                        {
                            Date = day,
                            Deelnemer = deelnemer,
                            IsDeleted = false
                        });
                    }
                    
                }
                
                //dates.Add(day.ToString(date_format));
            };
            //var afwezigheden = db.PresentieEntrys.Where(a => a.IsDeleted == false).ToList();
            var aanwezigheidsViewModel = new AanwezigheidViewModel()
            {
                AanwezigheidsOptions = db.AanwezigheidOptions.Where(ao => ao.IsDeleted == false).ToList(),
                PresentieEntryRows = presentieEntryRows

            };
            return View(aanwezigheidsViewModel);
        }

        [HttpPost]
        public ActionResult DeelnemerList(int selected_Value, int studentId, DateTime selectedDate)
        {
            //DateTime selectedDate = daysOfWeek(firstDayOfWeek(DateTime.Today))[dateId];
            Debug.WriteLine($"{selected_Value} --- {studentId} --- {selectedDate}");
            db.PresentieEntrys.Add(new PresentieEntryModel() {
                
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
        public ActionResult AddAfwezigheidOptions(AanwezigheidOptionModel insertedOption)
        {
            if (insertedOption == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            db.AanwezigheidOptions.Add(insertedOption);
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
