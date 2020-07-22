﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Mafa2.Web.Models
{
    public class ProizvodBO
    {
        #region Properties
        [Required(ErrorMessage = "Unesite sifru proizvoda")]
        [RegularExpression("^P[0-9]{4}$", ErrorMessage = "Unesite sifru u formatu Pxxxx (x predstavlja broj)")]
        [Remote("DaLiJeSifraUBazi", "Proizvod", HttpMethod = "POST", ErrorMessage = "Sifra je vec u bazi.")]
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
        #endregion
    }
}