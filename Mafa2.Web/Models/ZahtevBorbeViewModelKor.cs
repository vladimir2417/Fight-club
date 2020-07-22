using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Mafa2.Web.Models.CustomAnotacije;

namespace Mafa2.Web.Models
{
    public class ZahtevBorbeViewModelKor
    {
        //[DataType(DataType.Date)]
        [Required(ErrorMessage = "Morate izabrati željeni datum borbe")]
        //[ValidacijaZahtevanogDatuma(ErrorMessage = "Datum nije u dozvojlenom opsegu!")]
        public DateTime ZahtevaniDatum { get; set; }

        [Required(ErrorMessage = "Morate uneti željeno mesto")]
        [StringLength(50, ErrorMessage = "Sistem ne podržava da polje zahtevano mesto bude duže od 50 karaktera")]
        public string ZahtevanoMesto { get; set; }

        [Required(ErrorMessage = "Morate izabrati tip borbe")]
        public string TipBorbe { get; set; }

        [Required(ErrorMessage = "Morate izabrati težinsku kategoriju")]
        public string TezinskaKategorija { get; set; }

        [StringLength(2000, ErrorMessage = "Napomene su predugačke")]
        public string KorisnickeNapomene { get; set; }
        public int IDKorisnika { get; set; }

    }
}