using StudentenVolgSysteem.DAL;
using StudentenVolgSysteem.Models;
using StudentenVolgSysteem.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentenVolgSysteem.Controllers
{
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
            List <DeelnemerAfwezigheidListViewModel> tempList = new List<DeelnemerAfwezigheidListViewModel>();
            var studenten = db.Studenten.Where(s => s.IsDeleted == false).ToList();
            var Awezigheden = db.Afwezigheden.Where(a => a.Date == DateTime.Now.)
            var AOpties = db.AfwezigheidOptions.Where(ao => ao.IsDeleted == false).ToList();
            foreach (var item in studenten)
            {
                var tempDeelnemerALVM = new DeelnemerAfwezigheidListViewModel();
                tempDeelnemerALVM.Student = item;
                tempDeelnemerALVM.OptionsSelectList = AOpties.Select(a => new SelectListItem
                {
                    Text = a.Naam,
                    Value = a.Id.ToString()
                });
                tempDeelnemerALVM.Date = DateTime.Today;
                tempList.Add(tempDeelnemerALVM);
            }
            return View(tempList);
        }
    }
}