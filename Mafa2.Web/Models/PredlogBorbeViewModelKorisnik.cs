using Mafa2.Web.Models.LinqSql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mafa2.Web.Models
{
    public class PredlogBorbeViewModelKorisnik
    {
        public string IDPredloga { get; set; }
        public Korisnik Korisnik1 { get; set; }
        public Korisnik Korisnik2 { get; set; }
        public DateTime DatumVremeBorbe { get; set; }
        public string TipBorbe { get; set; }
        public string TezinskaKategorija { get; set; }
        public int Cena { get; set; }
        public string Napomene { get; set; }
        public int IDAdministratora  { get; set; }
        public SportskoBorilačkiKlub SportskoBorilackiKlub { get; set; }
        public TimeSpan vremeBorbe { get; set; }

    }
}