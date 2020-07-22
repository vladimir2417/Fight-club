using Mafa2.Web.Models;
using Mafa2.Web.Models.LinqSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mafa2.Web.Controllers
{
    public class KatalogController : Controller
    {
        // GET: Katalog
        public ActionResult Index()
        {

            DataClasses1DataContext dc = new DataClasses1DataContext();

            List<Proizvod> listaProizvoda = new List<Proizvod>();

            foreach(Proizvod p in dc.Proizvods)
            {
                p.UkupnaCena = p.Cena - (p.Cena * p.Popust / 100);
                listaProizvoda.Add(p);

            }

            dc.SubmitChanges();
            return View(listaProizvoda);
        }

        [Authorize(Roles = "Korisnik")]
        public ActionResult dodajUKorpu(Proizvod model)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();

            StavkaKorpe novaStavka = new StavkaKorpe();
            novaStavka.SifraProizvoda = model.SifraProizvoda;
            novaStavka.Cena = (double)model.UkupnaCena; //cena sa popustom!!!
            novaStavka.IzabranaKol = model.Kolicina;
            novaStavka.Naziv = model.Naziv;

            //prvo uraditi UPDATE KOLICINE u bazi
            Proizvod dodatProizvod = (from p in dc.Proizvods
                                where p.SifraProizvoda == model.SifraProizvoda
                                select p).SingleOrDefault();
            //update kolicine
            dodatProizvod.Kolicina -= model.Kolicina;

            try
            {
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (Session["shopping_cart"] == null)
            {
                List<StavkaKorpe> korpa = new List<StavkaKorpe>();
                korpa.Add(novaStavka);
                Session["shopping_cart"] = korpa;

            }
            else
            {
                List<StavkaKorpe> korpa = (List<StavkaKorpe>)Session["shopping_cart"];

                //cekiramo ukoliko izabrani proizvod vec postoji u korpi!
                for(int i = 0; i < korpa.Count; i++)
                {
                    if (korpa[i].SifraProizvoda.Equals(novaStavka.SifraProizvoda))
                    {
                        //POSTOJI, samo update-ujemo kolicinu!
                        korpa[i].IzabranaKol += novaStavka.IzabranaKol;
                        Session["nazivDodatogProizv"] = model.Naziv;
                        Session["izabranaKol"] = novaStavka.IzabranaKol;
                        return RedirectToAction("Index", "Katalog");
                        //i redirect-ujemo se, pa se samim tim ostatak metode nece izvrsiti
                    }
                }
                korpa.Add(novaStavka);
                Session["shopping_cart"] = korpa;
            }

            Session["nazivDodatogProizv"] = model.Naziv;
            Session["izabranaKol"] = novaStavka.IzabranaKol;
            return RedirectToAction("Index", "Katalog");
        }
    }
}