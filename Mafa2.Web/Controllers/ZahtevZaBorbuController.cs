using Mafa2.Web.Models;
using Mafa2.Web.Models.LinqSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using System.Data; // za dropdownlist using ViewBag
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Ajax.Utilities;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script;
using System.Web.Mvc.Ajax;
using System.Net.Sockets;
using System.Web;
using WebGrease.Css.Extensions;

namespace Mafa2.Web.Controllers
{
    public class ZahtevZaBorbuController : Controller
    {

        //METODA VEZANA ZA UROSA (KORISNIK)
        [Authorize(Roles = "Korisnik")]
        public ActionResult submitZahtev(ZahtevBorbeViewModelKor model)
        {
            if (model.ZahtevaniDatum < DateTime.Now || model.ZahtevaniDatum > DateTime.Now.AddMonths(2))
            {
                ModelState.AddModelError("ZahtevaniDatum", "Datum nije u dozvoljenom opsegu!");
                
            }
            if (!ModelState.IsValid)
            {
                Session["PogresanDatum"] = model.ZahtevaniDatum;
                return RedirectToAction("Zahtjev", "Pocetna");
                
            }
            else
            {
            
                try
                {
                    DataClasses1DataContext dc = new DataClasses1DataContext();

                    ZahtevZaBorbu noviZahtev = new ZahtevZaBorbu();

                    //generisanje id-ja za noviZahtev
                    Random rand = new Random();
                    int randomBr = rand.Next(1000000);
                    int i = 0; //brojac samo
                    while (i <= dc.ZahtevZaBorbus.Count()) //provera da li idZahtevaZaBorbu sa tim brojem vec postoji!!!
                    {
                        foreach (var z in dc.ZahtevZaBorbus)
                        {
                            if (z.IDZahtevaKorisnika.Equals(randomBr + ""))
                                randomBr = rand.Next(1000000);
                        }
                        i++;
                    }
                    noviZahtev.IDZahtevaKorisnika = randomBr.ToString();
                    //kraj generisanje id-ja za noviZahtev

                    noviZahtev.IDKorisnik = (int)Session["idKorisnika"];

                    noviZahtev.ZahtevaniDatum = model.ZahtevaniDatum;
                    noviZahtev.ZahtevanoMesto = model.ZahtevanoMesto;
                    noviZahtev.tezinskaKategorija = model.TezinskaKategorija;
                    noviZahtev.tipBorbe = model.TipBorbe;
                    noviZahtev.korisničkeNapomene = model.KorisnickeNapomene;
                 
                    dc.ZahtevZaBorbus.InsertOnSubmit(noviZahtev);
                    dc.SubmitChanges();
                    Session["zahtevDatum"] = noviZahtev.ZahtevaniDatum;
                    Session["zahtevMesto"] = noviZahtev.ZahtevanoMesto;
                    Session["zahtevTipBorbe"] = noviZahtev.tipBorbe;
                    Session["zahtevKategorija"] = noviZahtev.tezinskaKategorija;
                    Session["zahtevSubmitovan"] = "Uspešno ste poslali zahtev! (Podaci: zahtevani datum: <b>" + noviZahtev.ZahtevaniDatum +
                        "</b>, zahtevano mesto: <b>" + noviZahtev.ZahtevanoMesto + "</b>, kategorija: <b>" + noviZahtev.tezinskaKategorija
                        + "</b>, tip borbe: <b>" + noviZahtev.tipBorbe + "</b>)";

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                return Redirect("/Pocetna/Zahtjev");
            }
        }


        //MLADJIN DEO (ADMINISTRATOR DO KRAJA)
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();

            List<ZahteviZaBorbuViewModel> listZah = dc.ZahtevZaBorbus.Select(x => new ZahteviZaBorbuViewModel { IDZahtevaKorisnika = x.IDZahtevaKorisnika, ZahtevaniDatum = x.ZahtevaniDatum, ZahtevanoMesto = x.ZahtevanoMesto, tipBorbe = x.tipBorbe, tezinskaKategorija = x.tezinskaKategorija, korisničkeNapomene = x.korisničkeNapomene, IDKorisnik = x.IDKorisnik }).ToList();
            //List<ZahtevZaBorbu> listZah1 = dc.ZahtevZaBorbus.ToList();           
            ViewBag.ListaZahtjeva = listZah;

            string mainconn = ConfigurationManager.ConnectionStrings["MAFAConnectionString"].ConnectionString;
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string sqlquery = "SELECT * FROM ZahtevZaBorbu  ";
            SqlCommand sqlcomand = new SqlCommand(sqlquery, sqlconn);
            sqlconn.Open();
            SqlDataAdapter sda = new SqlDataAdapter(sqlcomand);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            ViewBag.kategorija = ds.Tables[0];

            List<SelectListItem> getkategorija = new List<SelectListItem>();
            foreach (System.Data.DataRow dr in ViewBag.kategorija.Rows)
            {
                getkategorija.Add(new SelectListItem { Text = @dr["tezinskaKategorija"].ToString(), Value = @dr["tezinskaKategorija"].ToString() });
            }

            ViewBag.Kategorije = getkategorija;

            sqlconn.Close();
            return View();

        }



        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public JsonResult BrisiZahtjev(string IDZahtevaKorisnika)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();

            bool rezultat = true;

            var brisi = (from red in dc.ZahtevZaBorbus
                         where red.IDZahtevaKorisnika == IDZahtevaKorisnika
                         select red).SingleOrDefault();

            if (brisi != null)
            {
                dc.ZahtevZaBorbus.DeleteOnSubmit(brisi);
            }

            try
            {
                dc.SubmitChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
            return Json(rezultat, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Prijedlog()
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();

            //Lista zahtjeva za stranicu "Prijedlog"
            List<ZahteviZaBorbuViewModel> listZah2 = dc.ZahtevZaBorbus.Select(x => new ZahteviZaBorbuViewModel { IDZahtevaKorisnika = x.IDZahtevaKorisnika, ZahtevaniDatum = x.ZahtevaniDatum, ZahtevanoMesto = x.ZahtevanoMesto, tipBorbe = x.tipBorbe, IDKorisnik = x.IDKorisnik }).ToList();

            ViewBag.ListaZahtjeva2 = listZah2;

            //pravimo listu tezinskih kategorija 

            var getKategorije = (from z in dc.ZahtevZaBorbus
                                 select z).DistinctBy(z => z.tezinskaKategorija).ToList();
            SelectList listaKategorija = new SelectList(getKategorije, "tezinskaKategorija", "tezinskaKategorija");
            ViewBag.listaKategorija = listaKategorija;


            //pravimo listu gradova koji su izabrani iz zahteva za borbu
            var getGradovi = (from z in dc.ZahtevZaBorbus
                              select z).DistinctBy(z => z.ZahtevanoMesto).ToList();
            SelectList listaGradova = new SelectList(getGradovi, "ZahtevanoMesto", "ZahtevanoMesto");

            ViewBag.gradoviIzZahteva = listaGradova;

            //Lista tipova borbi iz zahtjeva
            var getTipBorbe = (from z in dc.ZahtevZaBorbus
                               select z).DistinctBy(z => z.tipBorbe).ToList();
            SelectList listaTipovaBorbi = new SelectList(getTipBorbe, "tipBorbe", "tipBorbe");

            ViewBag.tipoviBorbi = listaTipovaBorbi;

            //Lista klubova iz zahtjeva
            var getKlub = (from z in dc.ZahtevZaBorbus
                           join g in dc.SportskoBorilačkiKlubs on z.ZahtevanoMesto equals g.Grad
                           //where g.Grad == listaGradova.SelectedValue()
                           select g).DistinctBy(g => g.Naziv).ToList();
            SelectList listaKlubova = new SelectList(getKlub, "Naziv", "Naziv");

            ViewBag.Klubova = listaKlubova;

            ZahteviZaBorbuViewModel model = new ZahteviZaBorbuViewModel();

            Session["IDB1"] = model.IDKorisnika1;
            Session["IDB2"] = model.IDKorisnika2;
            return View();


        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Prijedlog(ZahteviZaBorbuViewModel model)
        {
            return View(model);
        }


        // UPARIVANJE 
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult popuniBorce(string tezinskaKategorija, string tipBorbe, string grad, string IDSportskoBorilačkogKluba)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();


            var borci = (from z in dc.ZahtevZaBorbus
                         join k in dc.Korisniks on z.IDKorisnik equals
k.IDKorisnika
                         where z.tezinskaKategorija == tezinskaKategorija && z.tipBorbe == tipBorbe
           && z.ZahtevanoMesto == grad
                         select new { z.IDZahtevaKorisnika, k.IDKorisnika, k.Ime, k.Prezime, k.BrDobijenihBorbi, k.BrIzgubljenihBorbi }
                         ).ToList();


            List<SelectListItem> listaBoraca = new List<SelectListItem>();
            int count = 0;
            foreach (var b in borci)
            {
                listaBoraca.Add(new SelectListItem() { Text = "ID Zahtjeva=" + b.IDZahtevaKorisnika + ", " + b.Ime + " " + b.Prezime + " (Dobijene borbe: " + b.BrDobijenihBorbi + " Izgubljene borbe: " + b.BrIzgubljenihBorbi + ")", Value = b.IDKorisnika.ToString() });
                count++;
            }

            ZahteviZaBorbuViewModel model = new ZahteviZaBorbuViewModel();

            Session["listaBoraca"] = listaBoraca;
            Session["countBorce"] = count;
            Session["tezinskaKategorija"] = tezinskaKategorija;
            Session["tipBorbe"] = tipBorbe;
            Session["grad"] = grad;
            Session["IDSportskoBorilačkogKluba"] = IDSportskoBorilačkogKluba;
            Session["IDB1"] = model.IDKorisnika1;
            Session["IDB2"] = model.IDKorisnika2;


            var kluboviPoGradu = (from klub in dc.SportskoBorilačkiKlubs
                                  where klub.Grad == grad
                                  select klub).ToList();

            List<SelectListItem> listaKlubovaPoGradu = new List<SelectListItem>();

            foreach (var klub in kluboviPoGradu)
            {
                listaKlubovaPoGradu.Add(new SelectListItem() { Text = klub.Naziv + ", " + klub.Grad, Value = klub.IDSportskoBorilačkogKluba.ToString() });
            }

            Session["listaKlubovaPoGradu"] = listaKlubovaPoGradu;


            return RedirectToAction("Prijedlog", "ZahtevZaBorbu");

        }



        // FUNKCIJA ZA PUNJENJE NAZIVA KLUBOVA (KASKADNO)
        [Authorize(Roles = "Administrator")]
        public ActionResult GetKlub(string ZahtevanoMesto)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();

            List<SportskoBorilačkiKlub> nazivKluba = dc.SportskoBorilačkiKlubs.Where(x => x.Grad == ZahtevanoMesto).ToList();

            // KREIRANJE PARTIAL VIEW-a

            ViewBag.NazivKluba = new SelectList(nazivKluba, "Naziv", "Naziv");

            return PartialView("NazivKlubaPartial");
        }

        // FUNKCIJA ZA PUNJENJE e-maila KLUBOVA (KASKADNO)
        [Authorize(Roles = "Administrator")]
        public ActionResult GetMail(string IDSportskoBorilačkogKluba)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();

            List<SportskoBorilačkiKlub> mailKluba = dc.SportskoBorilačkiKlubs.Where(x => x.IDSportskoBorilačkogKluba == IDSportskoBorilačkogKluba).ToList();

            // KREIRANJE PARTIAL VIEW-a

            ViewBag.MailKluba = new SelectList(mailKluba, "email", "email");

            return PartialView("EmailKlubaPartial");
        }

        // FUNKCIJA ZA PUNJENJE e-maila 1. borca (KASKADNO)
        [Authorize(Roles = "Administrator")]
        public ActionResult EmailB1(int IDKorisnika1)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();

            List<Korisnik> emailB1 = dc.Korisniks.Where(x => x.IDKorisnika == IDKorisnika1).ToList();

            // KREIRANJE PARTIAL VIEW-a

            ViewBag.EmailB1 = new SelectList(emailB1, "email", "email");

            return PartialView("EmailB1Partial");
        }

        // FUNKCIJA ZA PUNJENJE e-maila 2. borca (KASKADNO)
        [Authorize(Roles = "Administrator")]
        public ActionResult EmailB2(int IDKorisnika2)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();

            List<Korisnik> emailB2 = dc.Korisniks.Where(x => x.IDKorisnika == IDKorisnika2).ToList();

            // KREIRANJE PARTIAL VIEW-a

            ViewBag.EmailB2 = new SelectList(emailB2, "email", "email");

            return PartialView("EmailB2Partial");
        }



        // FUNKCIJA ZA PUNJENJE ID KLUBOVA (KASKADNO)
        [Authorize(Roles = "Administrator")]
        public ActionResult GetKlub2(string grad)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();

            List<SportskoBorilačkiKlub> ID = dc.SportskoBorilačkiKlubs.Where(x => x.Grad == grad).ToList();

            // KREIRANJE PARTIAL VIEW-a

            ViewBag.NazivKluba = new SelectList(ID, "IDSportskoBorilačkogKluba", "Naziv");

            return PartialView("IDKlubaPartial");
        }

        // FUNKCIJA ZA PUNJENJE e-maila u TextBox
        [Authorize(Roles = "Administrator")]
        public ActionResult GetMailUText(string email)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();

            string mailKluba = dc.SportskoBorilačkiKlubs.Where(x => x.email == email).ToString();

            // KREIRANJE PARTIAL VIEW-a

            ViewBag.EMailKluba = mailKluba;

            return PartialView("EmailKlubaZaTextPartial");
        }




        //SLANJE ZAHTJEVA KLUBU
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Mail(Mafa2.Web.Models.ZahteviZaBorbuViewModel model)
        {
            MailMessage mm = new MailMessage("mladen56717@its.edu.rs", model.To);
            mm.Subject = model.Subject;
            mm.Body = model.Body;
            mm.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();

            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("mladen56717@its.edu.rs", "mladenzv");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;

            try
            {
                smtp.Send(mm);
            }
            catch
            {

                //throw ex;
                Session["greskaMail"] = "Neuspešno poslat mejl";
                return RedirectToAction("Prijedlog", "ZahtevZaBorbu");

            }


            ViewBag.Message = "Uspješno poslat mail!";

            Session["mail"] = model.To;

            return RedirectToAction("Prijedlog", "ZahtevZaBorbu");
        }

        //SLANJE PRIJEDLOGA BORCIMA
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult MailPR(Mafa2.Web.Models.ZahteviZaBorbuViewModel model)
        {


            MailMessage mm1 = new MailMessage("mladen56717@its.edu.rs", model.ToA);
            MailMessage mm2 = new MailMessage("mladen56717@its.edu.rs", model.ToB);
            mm1.Subject = model.Subject;
            mm1.Body = model.Bdy;
            mm1.IsBodyHtml = true;

            mm2.Subject = model.Subject;
            mm2.Body = model.Bdy;
            mm2.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();

            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("mladen56717@its.edu.rs", "mladenzv");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;




            Session["Cena"] = model.Cena;
            Session["Satnica"] = model.vremeBorbe;
            Session["Napomena"] = model.Napomene;
            Session["Datum"] = model.datumBorbe;


            Session["IDB1"] = model.IDKorisnika1;
            Session["IDB2"] = model.IDKorisnika2;

            try
            {
                smtp.Send(mm1);
                smtp.Send(mm2);
            }
            catch
            {

                Session["greskaMail"] = "Neuspješno poslati prijedlozi borcima";
                return RedirectToAction("Prijedlog", "ZahtevZaBorbu");

            }


            ViewBag.Message = "Uspješno poslat prijedlog borcima!";

            Session["mail"] = model.ToA;
            Session["mail2"] = model.ToB;
            return RedirectToAction("Prijedlog", "ZahtevZaBorbu");


        }



        // UNOS PRIJEDLOGA U BAZU
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult PunjenjePrijedlogaZaBorbu(ZahteviZaBorbuViewModel model)
        {
            //Čuvanje unešenih podataka na serveru
            DataClasses1DataContext dc = new DataClasses1DataContext();

            PredlogBorbe predlog = new PredlogBorbe();


            predlog.IDKorisnika1 = model.IDKorisnika1;
            predlog.IDKorisnika2 = model.IDKorisnika2;
            predlog.tipBorbe = model.tipBorbe;
            predlog.tezinskaKategorija = model.tezinskaKategorija;
            predlog.IDSportskoBorilačkogKluba = model.IDSportskoBorilačkogKluba;
            predlog.Cena = model.Cena;
            predlog.datumVremeBorbe = model.datumBorbe;
            predlog.Napomene = model.Napomene;
            predlog.IDAdministratora = 7777;
            predlog.vremeBorbe = model.vremeBorbe;
            predlog.PrviOdgovor = false;
            predlog.DrugiOdgovor = false;
            predlog.StanjePredloga = false;
            //predlog.prihvacen = false;

            Random random = new Random();
            int randomBr = random.Next();
            foreach (var k in dc.PredlogBorbes)
            {
                if (k.IDPredloga.Equals("PR" + randomBr))
                    randomBr++;
            }

            predlog.IDPredloga = "PR" + randomBr;


            try
            {
                dc.PredlogBorbes.InsertOnSubmit(predlog);
                dc.SubmitChanges();
            }
            catch
            {
                Session["greskaBaza1"] = "Neuspješno pohranjivanje prijedloga!";
                return RedirectToAction("Prijedlog", "ZahtevZaBorbu");
            }

            Session["prijedlog"] = model.IDKorisnika1;
            return RedirectToAction("Prijedlog", "ZahtevZaBorbu");
        }

        // Korisnička validacija cijene
        [Authorize(Roles = "Administrator")]
        public JsonResult DaLiJeDobraCijena(int sp)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();

            System.Threading.Thread.Sleep(200);

            var ci = dc.PredlogBorbes.Where(y => y.Cena == sp).SingleOrDefault();
            if (ci != null)
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