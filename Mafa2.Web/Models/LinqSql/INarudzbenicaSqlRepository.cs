using Mafa2.Web.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mafa2.Web.Models.LinqSql
{
    public class INarudzbenicaSqlRepository : INarudzbenicaRepository
    {
        private DataClasses1DataContext narudzbenicaDataContext = new DataClasses1DataContext();

        public List<NarudzbenicaBO> prikaziNarudzbenice()
        {
            List<NarudzbenicaBO> narudzbenice = new List<NarudzbenicaBO>();
            foreach (Narudzbenica narudzbenica in narudzbenicaDataContext.Narudzbenicas)
            {
                NarudzbenicaBO narudzbenicaBO = new NarudzbenicaBO();
                narudzbenicaBO.IDNarudzbenice = narudzbenica.IDNarudzbenice;
                narudzbenicaBO.DatumVreme = narudzbenica.DatumVreme.ToString();
                narudzbenicaBO.AdresaZaIsporuku = narudzbenica.AdresaZaIsporuku;
                narudzbenicaBO.Grad = narudzbenica.Grad;
                narudzbenicaBO.ZipCode = narudzbenica.ZipCode;
                narudzbenicaBO.TotalCena = narudzbenica.TotalCena;
                narudzbenicaBO.StavkeNarudzbenice = PrikaziStavke(narudzbenica.IDNarudzbenice);
                narudzbenice.Add(narudzbenicaBO);
            }
            return narudzbenice;
        }
        public NarudzbenicaBO prikaziNarudzbenicuPoID(string IDNarudzbenice)
        {
            Narudzbenica narudzbenica = narudzbenicaDataContext.Narudzbenicas.FirstOrDefault(n => n.IDNarudzbenice == IDNarudzbenice);
            NarudzbenicaBO narudzbenicaBO = new NarudzbenicaBO();
            if (narudzbenica == null) return narudzbenicaBO;
            narudzbenicaBO.IDNarudzbenice = narudzbenica.IDNarudzbenice;
            narudzbenicaBO.DatumVreme = narudzbenica.DatumVreme.ToString();
            narudzbenicaBO.AdresaZaIsporuku = narudzbenica.AdresaZaIsporuku;
            narudzbenicaBO.Grad = narudzbenica.Grad;
            narudzbenicaBO.ZipCode = narudzbenica.ZipCode;
            narudzbenicaBO.TotalCena = narudzbenica.TotalCena;
            narudzbenicaBO.StavkeNarudzbenice = PrikaziStavke(IDNarudzbenice);
            return narudzbenicaBO;
        }


        public List<StavkeNarudzbeniceBO> PrikaziStavke(string IDNarudzbenice)
        {
            List<StavkeNarudzbeniceBO> SveStavkeNarudzbenice = new List<StavkeNarudzbeniceBO>();
            foreach (StavkaNarudzbenice stavkaNarudzbenice in narudzbenicaDataContext.StavkaNarudzbenices.Where(s => s.IDNarudzbenice == IDNarudzbenice))
            {
                StavkeNarudzbeniceBO stavkeNarudzbeniceBO = new StavkeNarudzbeniceBO();
                stavkeNarudzbeniceBO.IDNarudzbenice = stavkaNarudzbenice.IDNarudzbenice;
                stavkeNarudzbeniceBO.SifraProizvoda = stavkaNarudzbenice.SifraProizvoda;
                stavkeNarudzbeniceBO.RedniBr = stavkaNarudzbenice.RedniBr;
                stavkeNarudzbeniceBO.IzabranaKolicina = stavkaNarudzbenice.IzabranaKolicina;
                stavkeNarudzbeniceBO.UkupnaCenaStavke = (float)stavkaNarudzbenice.UkupnaCenaStavke;
                SveStavkeNarudzbenice.Add(stavkeNarudzbeniceBO);
            }
            return SveStavkeNarudzbenice;
        }
        public void BrisiNarudzbenicu(string IDNarudzbenice)
        {
            if (!IDNarudzbenice.Equals("0"))
            {
                List<StavkaNarudzbenice> stavkeZaBrisanje = narudzbenicaDataContext.StavkaNarudzbenices.Where(t => t.IDNarudzbenice == IDNarudzbenice).ToList();
                foreach (StavkaNarudzbenice s in stavkeZaBrisanje)
                {
                    narudzbenicaDataContext.StavkaNarudzbenices.DeleteOnSubmit(s);
                    narudzbenicaDataContext.SubmitChanges();
                }
                Narudzbenica narudzbenica = narudzbenicaDataContext.Narudzbenicas.Where(n => n.IDNarudzbenice == IDNarudzbenice).FirstOrDefault();
                narudzbenicaDataContext.Narudzbenicas.DeleteOnSubmit(narudzbenica);
                narudzbenicaDataContext.SubmitChanges();
            }

        }
    }
}