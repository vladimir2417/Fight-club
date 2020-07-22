using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mafa2.Web.Controllers
{
    public class PocetnaPRODAVACController : Controller
    {
        // GET: PocetnaPRODAVAC
        [Authorize(Roles = "Prodavac")]
        public ActionResult Index()
        {
            return View();
        }
    }
}