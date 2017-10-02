using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Group_Ledger.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About Group Ledger";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Me!";

            return View();
        }
    }
}