using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mafa2.Web.Models
{
    public class ZahteviZaBorbuViewModel
    {
        public string IDZahtevaKorisnika { get; set; }

        [Required]
        [Display(Name = "ZahtevaniDatum")]
        public DateTime ZahtevaniDatum { get; set; }


        public IEnumerable<SelectListItem> ZahtevaniDatumi { get; set; }

        [Required(ErrorMessage = "Odaberite grad")]
        [Display(Name = "ZahtevanoMesto")]
        public string ZahtevanoMesto { get; set; }
        public IEnumerable<SelectListItem> ZahtevanaMesta { get; set; }



        [Required(ErrorMessage = "Odaberite tezinsku kategoriju")]
        [Display(Name = "tezinskaKategorija")]
        public string tezinskaKategorija { get; set; }

        [Required(ErrorMessage = "Odaberite tip borbe")]
        public string tipBorbe { get; set; }
        public IEnumerable<SelectListItem> tipoviBorbe { get; set; }

        [Required(ErrorMessage = "Odaberite sportski klub")]
        public string IDSportskoBorilačkogKluba { get; set; }

        [Required(ErrorMessage = "Odaberite 1. borca")]
        public int IDKorisnika1 { get; set; }

        [Required(ErrorMessage = "Odaberite 2. borca")]
        public int IDKorisnika2 { get; set; }

        [Required(ErrorMessage = "Odaberite e-mail")]
        public string email { get; set; }

        [Required(ErrorMessage = "Unesite cijenu borbe")]
        [Display(Name = "Cijena")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Moguć je unos samo brojeva!")]
        public int Cena { get; set; }

        [Required(ErrorMessage = "Unesite vrijeme borbe")]
        [Display(Name = "Vrijeme borbe")]
        [RegularExpression("^([0-1][0-9]|[2][0-3]):([0-5][0-9])$", ErrorMessage = "Obavezan unos vremena u formatu hh:mm!")]
        [DisplayFormat(DataFormatString = "{hh:MM:ss}", ApplyFormatInEditMode = true)]
        public System.TimeSpan vremeBorbe { get; set; }

        [Required(ErrorMessage = "Odaberite e-mail")]
        public string emailB1 { get; set; }

        [Required(ErrorMessage = "Odaberite e-mail")]
        public string emailB2 { get; set; }

        public string korisničkeNapomene { get; set; }

        public int IDKorisnik { get; set; }

        public string Naziv { get; set; }

        public string To { get; set; }

        public string ToA { get; set; }
        public string ToB { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }

        [Required(ErrorMessage = "Poslat ćeš prazan email!")]
        [Display(Name = "Tijelo emaila")]
        public string Body { get; set; }

        [Required(ErrorMessage = "Poslat ćeš prazan email!")]
        [Display(Name = "Tijelo emaila")]
        public string Bdy { get; set; }

        public int IDPredloga { get; set; }

        [Required(ErrorMessage = "Odaberi datum borbe!")]
        [Display(Name = "Datum borbe")]
        public DateTime datumBorbe { get; set; }

        [Required(ErrorMessage = "Odaberi datum rezervacije kluba!")]
        [Display(Name = "Datum borbe")]
        public DateTime datRez { get; set; }

        public string Napomene { get; set; }
        public int IDAdministratora { get; set; }

        public bool prihvatio { get; set; }
    }
}