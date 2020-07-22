using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mafa2.Web.Models
{
    public class StavkaNarudzbenicaIspis
    {
        public int RedniBr { get; set; }
        public int IzabranaKolicina { get; set; }
        public double UkupnaCenaStavke { get; set; }
        public string Naziv { get; set; }
        public string Slika { get; set; }
        public string AltSlika { get; set; }
        public double JedinicnaCena { get; set; }
        public int Popust { get; set; }

    }
}