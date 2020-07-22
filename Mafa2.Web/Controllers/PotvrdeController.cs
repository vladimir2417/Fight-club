using Mafa2.Web.Models;
using Mafa2.Web.Models.LinqSql;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace Mafa2.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class PotvrdeController : Controller
    {

        // 2. verzija
        public ActionResult Potvrde()
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();

            var getEmail = (from z in dc.Korisniks
                            join p in dc.PredlogBorbes on z.IDKorisnika equals p.IDKorisnika1
                            where p.StanjePredloga == true
                            select z).DistinctBy(z => z.email).ToList();
            SelectList listaMailova = new SelectList(getEmail, "email", "email");
            ViewBag.email = listaMailova;

            var getEmail2 = (from z in dc.Korisniks
                             join p in dc.PredlogBorbes on z.IDKorisnika equals p.IDKorisnika2
                             where p.StanjePredloga == true
                             select z).DistinctBy(z => z.email).ToList();
            SelectList listaMailova2 = new SelectList(getEmail2, "email", "email");
            ViewBag.email2 = listaMailova2;

            return View();
        }

        public ActionResult Vrati()
        {
            return View();
        }



        //SLANJE POTVRDE BORCIMA
        [HttpPost]
        public ActionResult MailPOT(Mafa2.Web.Models.PotvrdeViewModel model)
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

            Session["Sat"] = model.Satnica;
            Session["Napomene"] = model.Napomene;
            Session["Datum2"] = model.DatBorbe;


            ViewBag.Message = "Uspješno poslata potvrda borcima!";

            try
            {
                smtp.Send(mm1);
                smtp.Send(mm2);
            }
            catch
            {

                Session["greskaMail"] = "Neuspješno poslate potvrde borcima!";
                return RedirectToAction("Potvrde", "Potvrde");
            }


            ViewBag.Message = "Uspješno poslat prijedlog borcima!";

            Session["mail"] = model.ToA;
            Session["mail2"] = model.ToB;

            return RedirectToAction("Potvrde", "Potvrde");

        }

        // UNOS POTVRDE U BAZU
        [HttpPost]
        public ActionResult PunjenjePotvrde(PotvrdeViewModel model)
        {
            //Čuvanje unešenih podataka na serveru
            DataClasses1DataContext dc = new DataClasses1DataContext();

            PotvrdaDogađaja potvrda = new PotvrdaDogađaja();


            potvrda.Satnica = (TimeSpan)Session["Sat"];
            potvrda.Napomene = (string)Session["Napomene"];
            potvrda.datumBorbe = (DateTime)Session["Datum2"];
            potvrda.IDAdministratora = 7777;


            Random random = new Random();
            int randomBr = random.Next();
            foreach (var r in dc.PotvrdaDogađajas)
            {
                if (r.IDPotvrde.Equals(randomBr))
                    randomBr++;
            }

            potvrda.IDPotvrde = randomBr;


            try
            {
                dc.PotvrdaDogađajas.InsertOnSubmit(potvrda);
                dc.SubmitChanges();
            }
            catch(Exception ex)
            {
                Session["greskaBaza3"] = "Neuspješno pohranjivanje potvrde!";
                return RedirectToAction("Potvrde", "Potvrde");
            }

            Session["pootvrda"] = model.Satnica;

            //return View("Potvrde");
            return RedirectToAction("Potvrde", "Potvrde");

        }
    }
}