using Mafa2.Web.Models;
using Mafa2.Web.Models.LinqSql;
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
    public class RezervacijaController : Controller
    {
        // GET: Rezervacija
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rezervacija()
        {
            return View("Rezervacija");
        }

        //SLANJE REZERVACIJE KLUBU
        [HttpPost]
        public ActionResult Mail(Mafa2.Web.Models.ZahteviZaBorbuViewModel model)
        {
            MailMessage mm = new MailMessage("mladen56717@its.edu.rs", model.To);
            mm.Subject = model.Subject;
            mm.Body = model.Bdy;
            mm.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();

            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("mladen56717@its.edu.rs", "mladenzv");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;


            Session["IDSk"] = model.IDSportskoBorilačkogKluba;
            Session["datumBorbe"] = model.datumBorbe;


            try
            {
                smtp.Send(mm);
            }
            catch
            {
                Session["greskaMail"] = "Neuspješno poslata rezervacija klubu";
                return RedirectToAction("Rezervacija", "Rezervacija");

            }

            Session["mail"] = model.To;


            return RedirectToAction("Rezervacija", "Rezervacija");
        }

        // UNOS REZERVACIJE U BAZU
        [HttpPost]
        public ActionResult PunjenjeRezervacije(ZahteviZaBorbuViewModel model)
        {
            //Čuvanje unešenih podataka na serveru
            DataClasses1DataContext dc = new DataClasses1DataContext();

            Rezervacija rez = new Rezervacija();


            rez.IDSportskoBorilačkogKluba = model.IDSportskoBorilačkogKluba;
            rez.Datum = model.datumBorbe;
            rez.IDAdministratora = 7777;


            Random random = new Random();
            int randomBr = random.Next();
            foreach (var r in dc.Rezervacijas)
            {
                if (r.IDRezervacije.Equals(randomBr))
                    randomBr++;
            }

            rez.IDRezervacije = randomBr;


            try
            {
                dc.Rezervacijas.InsertOnSubmit(rez);
                dc.SubmitChanges();
            }
            catch
            {
                Session["greskaBaza2"] = "Neuspješno pohranjivanje rezervacije!";
                return RedirectToAction("Rezervacija", "Rezervacija");
            }

            Session["rezerv"] = model.IDSportskoBorilačkogKluba;


            return View("Rezervacija");
        }
    }
}