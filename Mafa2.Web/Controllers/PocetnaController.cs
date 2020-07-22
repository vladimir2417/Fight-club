using Mafa2.Web.Models;
using Mafa2.Web.Models.LinqSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Web.Security;

namespace Mafa2.Web.Controllers
{
    public class PocetnaController : Controller
    {
        public object rezultat { get; private set; }

        // GET: Pocetna
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(bool checkB)
        {
            if (checkB == true)
            {
                ViewBag.Message = "Potvrdjena izjava!";
                TempData["message"] = null;
                return View("Index");
            }
            else
            {
                //ViewBag.Message = "Molimo Vas da napustite sajt";
                TempData["message"] = "Molimo Vas da napustite sajt!";
                //return View("Izjava");
                return RedirectToAction("Izjava","Izjava");
                //return View("../Izjava/Izjava");
               // return Redirect("Izjava/Izjava"); RADI OVAKO
            }

        }

        public ActionResult Registracija()
        {

            return View();
        }

        [HttpPost]
        public ActionResult RegistracijaKorisnika(RegistracijaViewModel model)
        {
            //Čuvanje unešenih podataka na serveru
            DataClasses1DataContext dc = new DataClasses1DataContext();

            Korisnik korisnik = new Korisnik();
            korisnik.Ime = model.ImeKorisnika;
            korisnik.Prezime = model.PrezimeKorisnika;
            korisnik.Godine = model.Godine;
            korisnik.Tezina = model.Tezina;
            korisnik.Visina = model.Visina;
            korisnik.MestoStanovanja = model.MestoStanovanja;
            korisnik.BrDobijenihBorbi = model.BrDobijenihBorbi;
            korisnik.BrIzgubljenihBorbi = model.BrIzgubljenihBorbi;
            korisnik.Adresa = model.Adresa;
            korisnik.BorilackaVestina = model.BorilackaVestina;
            korisnik.email = model.Email;
            korisnik.brTelefona = model.BrTelefona;

            //provera u bazi da li username vec postoji
            var usernamePostoji = (from k in dc.Korisniks
                                  where k.username == model.UsernameKorisnik
                                  select k.username).SingleOrDefault();
            if(usernamePostoji != null)
            {
                ViewBag.Poruka = "Username koji ste uneli je već zauzet! Molimo Vas unesite neki drugi.";
                // ModelState.Clear();
                return View("Registracija");
            }
            //kraj provere

            korisnik.username = model.UsernameKorisnik;

            //provera da li je korisnik uspesno potvrdio password
            if (!(model.PasswordKorisnika1.Equals(model.PasswordKorisnika2)))
            {
                ViewBag.Poruka = "Neuspešno potvrđen password!";
                return View("Registracija");
            }
            //kraj provere

            korisnik.passwordKorisnika = model.PasswordKorisnika1;
            korisnik.IDUloge = 3;

            //dodeljivanje id-a novom korisniku
            Random random = new Random();
            int randomBr = random.Next();
            foreach(var k in dc.Korisniks)
            {
                if (k.IDKorisnika == randomBr)
                    randomBr++;

            }

            korisnik.IDKorisnika = randomBr;

            //sada moramo da unesemo njegove podatke i u tabelu Pristup (koju koristimo
            //za login)
            Pristup pristupKorisnik = new Pristup();
            int rand  = random.Next(0, 9999999);
            foreach (var p in dc.Pristups)
            {
                if (p.IDUsera.Equals("PR" + rand))
                    rand++;

            }

            pristupKorisnik.IDUsera = "PR" + rand;
            pristupKorisnik.Username = model.UsernameKorisnik;
            pristupKorisnik.Password = model.PasswordKorisnika1;
            pristupKorisnik.IDUloge = 3;


            dc.Korisniks.InsertOnSubmit(korisnik);
            dc.Pristups.InsertOnSubmit(pristupKorisnik);
            try
            {
                dc.SubmitChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }

            ViewBag.usernameKor = model.UsernameKorisnik;
            ModelState.Clear(); //reset model field polja sa forme!!!
            ViewBag.uspehRegistracija = "Uspešno ste se registrovali!";
            return View("Registracija");
        }


        // METODA ZA OTVARANJE STRANICE ZA LOGOVANJE
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginUser(LoginViewModel model)
        {

            DataClasses1DataContext dc = new DataClasses1DataContext();

            Pristup pr= dc.Pristups.SingleOrDefault(x=> x.Username==model.Username && x.Password==model.Password);
            
            string rezultat = "fail";
            if(pr!=null)
            {
                //Session["IDUsera"] = pr.IDUsera;

                Session["loggedIn"] = true;

                if (pr.IDUloge == 3)
                {
                    rezultat = "Korisnik";
                    var idKor = (from user in dc.Korisniks
                                where user.username == model.Username
                                select user.IDKorisnika).SingleOrDefault();
                    Session["idKorisnika"] = idKor;
                    var imePrezime = (from u in dc.Korisniks
                               where u.username == model.Username
                               select new { u.Ime, u.Prezime}).SingleOrDefault();
                    Session["ImePrezimeKor"] = imePrezime.Ime + " " + imePrezime.Prezime;
                    FormsAuthentication.SetAuthCookie(pr.Username, false);
                   return RedirectToAction("Index", "Pocetna");
                }
                if (pr.IDUloge == 2)
                {
                    rezultat = "Prodavac";
                    var idProd = (from prodavac in dc.Prodavacs
                                  where prodavac.usernameProd == model.Username
                                  select prodavac.IDProdavac).SingleOrDefault();
                    Session["idProdavac"] = idProd;
                    //POCETNA ZA PRODAVCA REDIRECT, PITATI BOLETA KOJA JE TO STRANICA
                    FormsAuthentication.SetAuthCookie(pr.Username, false);
                    return RedirectToAction("Index", "PocetnaPRODAVAC");
                }
                if (pr.IDUloge==1)
                { 
                    rezultat = "Administrator";
                    var idAdmin = (from admin in dc.Administrators
                                  where admin.username == model.Username
                                  select admin.IDAdministratora).SingleOrDefault();
                    Session["idAdmin"] = idAdmin;
                    FormsAuthentication.SetAuthCookie(pr.Username, false);
                    //POCETNA ZA ADMINISTRATORA REDIRECT
                    return RedirectToAction("Index", "ZahtevZaBorbu");

                }
            }
            else
            {
                TempData["LoginFail"] = "Username ili password koji ste uneli nisu važeći, " +
                "molimo vas pokušajte ponovo.";
            }

            return RedirectToAction("Login","Pocetna");

        }


        //METODA ZA ODJAVU
        public ActionResult LogOut()
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();

            //UPDATE kolicine proizvoda u bazi, ukoliko je ostalo proizvoda u korpi prilikom odjave!!!
            if (Session["shopping_cart"] != null)
            {

                List<StavkaKorpe> korpa = (List<StavkaKorpe>)Session["shopping_cart"];

                foreach(StavkaKorpe k in korpa)
                {
                    //upit u bazi da pronadjemo odgovarajuci proizvod
                    //da bismo mu vratili kolicinu iz korpe
                    Proizvod proizvod = (from p in dc.Proizvods
                                        where p.SifraProizvoda == k.SifraProizvoda
                                        select p).SingleOrDefault();

                    proizvod.Kolicina += k.IzabranaKol;
                    
                }
            }

            try
            {
                dc.SubmitChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }

            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            // i povratak ponovo na stranicu za Logovanje
            return RedirectToAction("Index", "Pocetna");
        }

        //O NAMA
        public ActionResult ONama()
        {
            return View();
        }

        //ZAHTJEV ZA BORBU
        [Authorize(Roles ="Korisnik")]
        public ActionResult Zahtjev()
        {
            return View();
        }

        //GALERIJA
        public ActionResult Galerija()
        {
            return View();
        }

        //KONTAKT
        public ActionResult Kontakt()
        {
            return View();
              
        }


    }
}