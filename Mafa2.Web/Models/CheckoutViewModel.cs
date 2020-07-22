using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Mafa2.Web.Models
{
    public class CheckoutViewModel
    {

        [Required(ErrorMessage = "Morate uneti adresu za isporuku")]
        [StringLength(100, ErrorMessage = "Predugačka adresa za isporuku")]
        public string AdresaZaIsporuku { get; set; }

        [Required(ErrorMessage = "Morate uneti polje za grad")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Predugačak naziv grada")]
        public string Grad { get; set; }

        [Required(ErrorMessage = "Morate uneti vaš poštanski kod.")]
        [StringLength(5, ErrorMessage = "Poštanski broj mora imati tačno 5 cifara")]
        public string ZipCode { get; set; }

        public DateTime DatumVreme { get; set; }
        public double totalPrice { get; set; }
    }
}