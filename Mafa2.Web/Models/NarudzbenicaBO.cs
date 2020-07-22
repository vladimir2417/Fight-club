using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mafa2.Web.Models
{
    public class NarudzbenicaBO
    {
        public string IDNarudzbenice { get; set; }
        public string DatumVreme { get; set; }
        public string AdresaZaIsporuku { get; set; }
        public string Grad { get; set; }
        public string ZipCode { get; set; }
        public double TotalCena { get; set; }
        public List<StavkeNarudzbeniceBO> StavkeNarudzbenice { get; set; }
    }
}