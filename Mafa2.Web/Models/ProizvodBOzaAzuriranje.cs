using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mafa2.Web.Models
{
    public class ProizvodBOzaAzuriranje
    {
        public string SifraProizvoda { get; set; }
        [Required(ErrorMessage = "Unesite naziv proizvoda")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Unesite kolicinu proizvoda")]
        public int Kolicina { get; set; }
        public string Opis { get; set; }
        [Required(ErrorMessage = "Unesite cenu proizvoda")]
        public int Cena { get; set; }
        [Required(ErrorMessage = "Unesite proizvodjaca proizvoda")]
        public string Proizvodjac { get; set; }
        [Range(1, 100, ErrorMessage = "Popust moze biti od 1 do 100 % ")]
        public int Popust { get; set; }
        public KatalogBO Katalog { get; set; }
        [DisplayName("Dodaj sliku")]
        public string Slika { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public string AltSlika { get; set; }
        public double UkupnaCena { get; set; }
    }
}