using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentenVolgSysteem.Models;
using StudentenVolgSysteem.DAL;

namespace StudentenVolgSysteem.Controllers
{
    [Authorize(Roles = "Administrator, Docent")]
    public class HomeController : Controller
    {
        private readonly SVSContext db = new SVSContext();

        // GET: Home
        public ActionResult Index()
        {
            return View("Home");
        }
    }
}