using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mafa2.Web.Models;
using Mafa2.Web.Models.LinqSql;
namespace Mafa2.Web.Controllers
{
    [Authorize(Roles = "Korisnik")]
    public class PredlogBorbeController : Controller
    {
        // GET: PredlogBorbe
        //Puni view PredloziBorbe
        
        public ActionResult PredloziBorbe()
        {

            DataClasses1DataContext dc = new DataClasses1DataContext();

            List<PredlogBorbe> predlozi = dc.PredlogBorbes.Where(m => m.IDKorisnika1 == (int)Session["idKorisnika"] ||
            m.IDKorisnika2 == (int)Session["idKorisnika"] && m.StanjePredloga == false).ToList();

            //sada sledi mapiranje u ViewModel klasu predloga borbe za prikaz u view-u
            List<PredlogBorbeViewModelKorisnik> predloziBorbe = new List<PredlogBorbeViewModelKorisnik>();

            foreach(PredlogBorbe pr in predlozi)
            {
                PredlogBorbeViewModelKorisnik predlog = new PredlogBorbeViewModelKorisnik();
                predlog.IDPredloga = pr.IDPredloga;
                predlog.Korisnik1 = dc.Korisniks.Where(k => k.IDKorisnika == pr.IDKorisnika1).SingleOrDefault();
                predlog.Korisnik2 = dc.Korisniks.Where(k => k.IDKorisnika == pr.IDKorisnika2).SingleOrDefault();
                predlog.DatumVremeBorbe = pr.datumVremeBorbe;
                predlog.TipBorbe = pr.tipBorbe;
                predlog.TezinskaKategorija = pr.tezinskaKategorija;
                predlog.Cena = pr.Cena;
                predlog.vremeBorbe = (TimeSpan)pr.vremeBorbe;
                predlog.Napomene = pr.Napomene;
                predlog.IDAdministratora = pr.IDAdministratora;
                predlog.SportskoBorilackiKlub = dc.SportskoBorilačkiKlubs.Where(s => s.IDSportskoBorilačkogKluba == pr.IDSportskoBorilačkogKluba).SingleOrDefault();

                predloziBorbe.Add(predlog);
            }
           
            
            //ViewBag.predloziBorbe = predloziBorbe;

            return View(predloziBorbe);
        }

        //obradiPredlog metoda za handlovanje prihvatanja ili odbijanja predloga za borbu!
        public ActionResult obradiPredlog(PredlogBorbeViewModelKorisnik model)
        {
            DataClasses1DataContext dc = new DataClasses1DataContext();
            PredlogBorbe predlog = dc.PredlogBorbes.Where(m => m.IDPredloga == model.IDPredloga).SingleOrDefault();
            
            //ukoliko je korisnik prihvatio predlog!
            if(Request.Form["btnPrihvatiPred"] != null)
            {

                if (predlog.PrviOdgovor)
                {
                    predlog.DrugiOdgovor = true;
                }
                else
                {
                    predlog.PrviOdgovor = true;
                }

                //Prihvacen je zahtev od strane oba borca
                if (predlog.PrviOdgovor == true && predlog.DrugiOdgovor == true)
                {
                    predlog.StanjePredloga = true;

                    try
                    {
                        dc.SubmitChanges();
                    }
                    catch(Exception ex)
                    {
                        Session["greskaPotvrdePredloga"] = "Trenutno imamo tehničkih problema sa serverom. Molimo Vas, pokušajte ponovo.";
                        return RedirectToAction("PredloziBorbe");
                    }
                    Session["uspehPotvrdePredlogaDatum"] = model.DatumVremeBorbe;
                    Session["uspehPotvrdePredlogaVreme"] = model.vremeBorbe;
                    Session["uspehPotvrdePredlogaNazivKluba"] = model.SportskoBorilackiKlub.Naziv;
                    Session["uspehPotvrdePredlogaAdresaKluba"] = model.SportskoBorilackiKlub.Adresa;

                    //predlog je zvanicno prihvacen, dakle izbrisati sve ostale predloge za tog korisnika
                    List<PredlogBorbe> predloziZaBrisanje = dc.PredlogBorbes.Where(m=>m.IDKorisnika1 == predlog.IDKorisnika1 ||
                        m.IDKorisnika2 == predlog.IDKorisnika1 || m.IDKorisnika1 == predlog.IDKorisnika2 || m.IDKorisnika2 == predlog.IDKorisnika2).ToList();
                    foreach(PredlogBorbe p in predloziZaBrisanje)
                    {
                        if (p.IDPredloga != model.IDPredloga)
                        {
                            //PredlogBorbe dalijeNasPredlog = dc.PredlogBorbes.Where(model.IDPredloga != p.IDPredloga).SingleOrDefault();
                            dc.PredlogBorbes.DeleteOnSubmit(p);
                        }
                        try
                        {
                            dc.SubmitChanges();
                        }
                        catch(Exception ex)
                        {
                            Session["greskaPotvrdePredloga"] = "Trenutno imamo tehničkih problema sa serverom. Molimo Vas, pokušajte ponovo.";
                            return RedirectToAction("PredloziBorbe");
                        }
                    }//kraj brisanja ostalih predloga

                    return RedirectToAction("PredloziBorbe");
                }

                //nije prihvaćen od strane oba borca, poruka da korisnik sačeka da drugi borac prihvati
                dc.SubmitChanges();
                Session["cekanje"] = "Uspešno ste prihvatili predlog! Čeka se potvrda od strane drugog borca...";
                Session["uspehPotvrdePredlogaDatum1"] = model.DatumVremeBorbe;
                Session["uspehPotvrdePredlogaVreme"] = model.vremeBorbe;
                Session["uspehPotvrdePredlogaNazivKluba"] = model.SportskoBorilackiKlub.Naziv;
                Session["uspehPotvrdePredlogaAdresaKluba"] = model.SportskoBorilackiKlub.Adresa;
                return RedirectToAction("PredloziBorbe");

            }
            //ukoliko je korisnik odbio predlog, obrisi samo taj predlog
            else
            {
                dc.PredlogBorbes.DeleteOnSubmit(predlog);
                try
                {
                    dc.SubmitChanges();
                }
                catch (Exception ex)
                {
                    Session["greskaPotvrdePredloga"] = "Trenutno imamo tehničkih problema sa serverom. Molimo Vas, pokušajte ponovo.";
                    return RedirectToAction("PredloziBorbe");
                }

                Session["odbijenPredlog"] = model.IDPredloga;
                return RedirectToAction("PredloziBorbe");

            }
            //return RedirectToAction("PredloziBorbe");
        }
    }
}