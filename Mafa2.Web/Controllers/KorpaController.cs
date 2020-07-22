using Mafa2.Web.Models;
using Mafa2.Web.Models.LinqSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mafa2.Web.Controllers
{
    [Authorize(Roles = "Korisnik")]
    public class KorpaController : Controller
    {
        // GET: Korpa
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult deleteItem(string sifraP)
        {

            DataClasses1DataContext dc = new DataClasses1DataContext();

            List<StavkaKorpe> korpa = (List<StavkaKorpe>)Session["shopping_cart"];

            //radimo UPDATE KOLICINE u bazi, dakle vracamo
            //ukupnu kolicinu proizvoda koja je dodata u korpu
            Proizvod proizvod = (from p in dc.Proizvods
                              where p.SifraProizvoda == sifraP
                              select p).SingleOrDefault();

            

            for(int i = 0; i < korpa.Count; i++)
            {
                if (korpa[i].SifraProizvoda.Equals(sifraP))
                {
                    //nasli smo proizvod u korpi za brisanje, update-ujemo
                    //bazu i izbacujemo proizvod iz korpe
                    proizvod.Kolicina += korpa[i].IzabranaKol;
                    korpa.RemoveAt(i);
                }
            }

            //sacuvamo promene u BAZI!
            try
            {
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }


            if (korpa.Count == 0)
            {
                Session["shopping_cart"] = null;
            }
            else 
            {
                Session["shopping_cart"] = korpa; 
            }
            return View("Index");
        }
    }
}