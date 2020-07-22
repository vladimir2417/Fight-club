using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mafa2.Web.Models;
using System.Net;
using System.Net.Mail;  

namespace Mafa2.Web.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class EmailSetupController : Controller
    {
        // GET: EmailSetup
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Mail(Mafa2.Web.Models.ZahteviZaBorbuViewModel model)
        {
            MailMessage mm = new MailMessage("mz.tool2882@gmail", model.To);
            mm.Subject = model.Subject;
            mm.Body = model.Body;
            mm.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient();

            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("mz.tool2882@gmail", "toolove2882");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;
            smtp.Send(mm);

            ViewBag.Message = "Uspješno poslat mail!";

            return RedirectToAction("Prijedlog", "EmailSetup");
        }
    }
}