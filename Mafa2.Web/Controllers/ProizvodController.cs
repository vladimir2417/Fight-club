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
    public class ProizvodController : Controller
    {
        private DataClasses1DataContext proizvodiDataContext = new DataClasses1DataContext();

        private IProizvodRepository proizvodRepository;
        public ProizvodController()
        {
            proizvodRepository = new ProizvodSqlRepository();
        }

        public ActionResult PrikazProizvoda()
        {
            ViewBag.Proizvodi = proizvodRepository.prikaziProizvode();
            return View(proizvodRepository.prikaziProizvode());
        }

        public ActionResult UnosProizvoda()
        {
            @ViewBag.Katalozi = proizvodRepository.PrikaziKatalog();
            return View();
        }

        [HttpPost]
        public ActionResult Unesi(ProizvodBO proizvodBO)
        {
            proizvodRepository.UnesiProizvod(proizvodBO);
            return RedirectToAction("PrikazProizvoda");
        }

        public ActionResult AzuriranjeProizvoda(string SifraProizvoda)
        {
            @ViewBag.Katalozi = proizvodRepository.PrikaziKatalog();
            ProizvodBOzaAzuriranje proizvod = proizvodRepository.prikaziProizvodePoId(SifraProizvoda);
            return View(proizvod);
        }
        [HttpPost]
        public ActionResult Azuriraj(ProizvodBOzaAzuriranje proizvod)
        {

            proizvodRepository.IzmeniProizvod(proizvod);

            return RedirectToAction("PrikazProizvoda");
        }
        public ActionResult BrisanjeProizvoda(string SifraProizvoda)
        {
            List<string> idjeviNar = new List<string>();
            List<StavkaNarudzbenice> stavkeZaBrisanje = proizvodiDataContext.StavkaNarudzbenices.Where(t => t.SifraProizvoda == SifraProizvoda).ToList();
            foreach (StavkaNarudzbenice s in stavkeZaBrisanje)
            {
                if (!idjeviNar.Contains(s.IDNarudzbenice))
                    idjeviNar.Add(s.IDNarudzbenice);
                //proizvodiDataContext.StavkaNarudzbenices.DeleteOnSubmit(s);
            }

            string prikazIdjeva = "";
            if (idjeviNar.Count > 0) //ima narudzbenica, poruka obrisite prvo narudzbenice
            {
                foreach (string n in idjeviNar)
                {
                    prikazIdjeva += n + ", ";
                }

                Session["idjeviNar"] = prikazIdjeva;
                return RedirectToAction("PrikazProizvoda");
            }
            else
            {
                proizvodRepository.BrisiProizvod(SifraProizvoda);
                return RedirectToAction("PrikazProizvoda");
            }
        }
        [HttpPost]
        public JsonResult DaLiJeSifraUBazi(string SifraProizvoda)
        {

            return Json(DaLiJeDostupna(SifraProizvoda));

        }

        public bool DaLiJeDostupna(string sp)
        {

            var spprovera = (from s in proizvodiDataContext.Proizvods
                             where s.SifraProizvoda == sp
                             select new { sp }).FirstOrDefault();

            bool status;
            if (spprovera != null)
            {
                //Already registered  
                status = false;
            }
            else
            {
                //Available to use  
                status = true;
            }

            return status;
        }



        public JsonResult DaLiJeDobraSifra(string sp)
        {
            System.Threading.Thread.Sleep(200);
            var pro = proizvodiDataContext.Proizvods.Where(p => p.SifraProizvoda == sp).SingleOrDefault();
            if (pro != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }
    }
}