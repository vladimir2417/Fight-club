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
    public class NarudzbenicaController : Controller
    {
        // GET: Narudzbenica
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CheckOut(CheckoutViewModel model)
        {

            DataClasses1DataContext dc = new DataClasses1DataContext();

            Narudzbenica novaNarudz = new Narudzbenica();

            //pravljenje random idNarudzbenice!
            Random random = new Random();
            int randomBr = random.Next(0, 9999999);
            //provera da li takav idNarudzbenice vec postoji
            foreach (var n in dc.Narudzbenicas)
            {
                if (n.IDNarudzbenice.Equals("NAR" + randomBr))
                    randomBr = random.Next(0, 9999999);

            }
            novaNarudz.IDNarudzbenice = "NAR" + randomBr;

            model.totalPrice = (double)Session["totalPrice"];

            novaNarudz.DatumVreme = model.DatumVreme;
            novaNarudz.AdresaZaIsporuku = model.AdresaZaIsporuku;
            novaNarudz.Grad = model.Grad;
            novaNarudz.ZipCode = model.ZipCode;
            novaNarudz.TotalCena = model.totalPrice;
            novaNarudz.IDKorisnika = (int)Session["idKorisnika"];

            //kraj pravljenja Narudzbenice

            //prvo moramo da je ubacimo u bazu, da bismo mogli da pravimo i njene stavke
            dc.Narudzbenicas.InsertOnSubmit(novaNarudz);
            try
            {
                dc.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //sada pravimo STAVKE narudzbenice
            List<StavkaKorpe> korpa = (List<StavkaKorpe>)Session["shopping_cart"];

            int redniBr = 1;
            //List<StavkaNarudzbenice> stavkeNarudz = new List<StavkaNarudzbenice>();
            foreach (StavkaKorpe stavkaKorpe in korpa)
            {
                double ukupnaCenaStavke = stavkaKorpe.Cena * stavkaKorpe.IzabranaKol;
                StavkaNarudzbenice novaStavka = new StavkaNarudzbenice();
                novaStavka.IDNarudzbenice = novaNarudz.IDNarudzbenice;
                novaStavka.SifraProizvoda = stavkaKorpe.SifraProizvoda;
                novaStavka.RedniBr = redniBr;
                novaStavka.IzabranaKolicina = stavkaKorpe.IzabranaKol;
                novaStavka.UkupnaCenaStavke = ukupnaCenaStavke;

                dc.StavkaNarudzbenices.InsertOnSubmit(novaStavka);
                try
                {
                    dc.SubmitChanges();
                }
                catch(Exception ex)
                {
                    throw ex;
                }

                //stavkeNarudz.Add(novaStavka);
                redniBr++;

            }

            //unsetujemo Session, jer smo izvrsili porudzbinu
            //(NE TREBA DA UPDATE-UJEMO KOLICINU JER JE KOLICINA VEC UPDATE-OVANA
            //PRILIKOM DODAVANJA U KORPU)
            List<StavkaNarudzbenicaIspis> stavkeNarIspis = (from st in dc.StavkaNarudzbenices
                                 join p in dc.Proizvods
                                 on st.SifraProizvoda equals p.SifraProizvoda
                                 where st.IDNarudzbenice == novaNarudz.IDNarudzbenice
                                 orderby st.RedniBr
                                 select new StavkaNarudzbenicaIspis() {
                                 RedniBr =  st.RedniBr,
                                 IzabranaKolicina = st.IzabranaKolicina,
                                 UkupnaCenaStavke = st.UkupnaCenaStavke,
                                 Naziv = p.Naziv,
                                 Slika = p.Slika,
                                 AltSlika = p.AltSlika, 
                                 JedinicnaCena = (double)p.UkupnaCena,
                                 Popust = (int)p.Popust}).ToList();

            //TO-DO: U Narudzbenica/Index view-u treba ispisati detalje narudzbenice i njene stavke
            //Proslediti mu sve njegove stavke (mozda preko ViewBag-a?)
            ViewBag.stavkeNarudzIspis = stavkeNarIspis;
            Session["shopping_cart"] = null;
            return View("Index", novaNarudz);
        }
    }
}