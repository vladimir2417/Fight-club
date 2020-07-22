using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mafa2.Web.Models
{
    public class PotvrdeViewModel
    {
        public string Napomene { get; set; }

        [Required(ErrorMessage = "Unesite datum borbe")]
        [Display(Name = "Created Date")]
        //[DataType(DataType.DateTime)]
        //[DisplayFormat(DataFormatString = "{mm/dd/yy hh:MM:ss}", ApplyFormatInEditMode = true)]
        public DateTime DatBorbe { get; set; }

        [Required(ErrorMessage = "Unesite vrijeme borbe")]
        [Display(Name = "Vrieme borbe")]
        [RegularExpression("^([0-1][0-9]|[2][0-3]):([0-5][0-9]):([0-5][0-9])$", ErrorMessage = "Obavezan unos vremena u formatu hh:mm:ss!")]
        [DisplayFormat(DataFormatString = "{hh:MM:ss}", ApplyFormatInEditMode = true)]
        public System.TimeSpan Satnica { get; set; }
        public string ToA { get; set; }
        public string ToB { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }

        [Required(ErrorMessage = "Odaberite email 1. borca")]
        [Display(Name = "email1")]
        public string emailB1 { get; set; }
        [Required(ErrorMessage = "Odaberite email 2. borca")]
        [Display(Name = "email2")]
        public string emailB2 { get; set; }

        [Required(ErrorMessage = "Zar ćeš slati praznu potvrdu ljudima?!")]
        [Display(Name = "potvrda")]
        public string Bdy { get; set; }
    }
}