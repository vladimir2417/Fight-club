using Mafa2.Web.Models;
using Mafa2.Web.Models.Interfaces;
using Mafa2.Web.Models.LinqSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mafa2.Web.Controllers
{
    [Authorize(Roles = "Prodavac")]
    public class NarudzbenicaProdController : Controller
    {
        private DataClasses1DataContext narudzbenicaDataContext = new DataClasses1DataContext();

        private INarudzbenicaRepository narudzbenicaRepository;
        public NarudzbenicaProdController()
        {
            narudzbenicaRepository = new INarudzbenicaSqlRepository();
        }

        public ActionResult PrikazNarudzbenice()
        {
            ViewBag.Proizvodi = narudzbenicaRepository.prikaziNarudzbenice();
            return View(narudzbenicaRepository.prikaziNarudzbenice());
        }
        public ActionResult PrikaziStavke(string id)
        {
            return PartialView("PrikaziStavke", narudzbenicaRepository.PrikaziStavke(id));
        }
        public ActionResult BrisiNarudzbenicu(string IDNar)
        {
            narudzbenicaRepository.BrisiNarudzbenicu(IDNar);
            return RedirectToAction("PrikazNarudzbenice");
        }

        public ActionResult NarudzbenicaZaBrisanje(string id)
        {
            NarudzbenicaBO narudzbenicaBO = narudzbenicaRepository.prikaziNarudzbenicuPoID(id);
            return View(narudzbenicaBO);
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}