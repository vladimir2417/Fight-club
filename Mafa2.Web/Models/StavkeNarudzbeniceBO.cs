using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mafa2.Web.Models
{
    public class StavkeNarudzbeniceBO
    {
        public string IDNarudzbenice { get; set; }
        public string SifraProizvoda { get; set; }
        public int RedniBr { get; set; }
        public int IzabranaKolicina { get; set; }
        public float UkupnaCenaStavke { get; set; }
    }
}