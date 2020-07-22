using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Mafa2.Web.Models.CustomAnotacije;
using Mafa2.Web.Models;

namespace Mafa2.Web.Models
{
    public class RegistracijaViewModel
    {
        
        public int IdKorisnika { get; set; }

        [Required(ErrorMessage = "Morate uneti username")]
        [Display(Name = "Username korisnika")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Dužina username-a može biti između 5 i 15 karaktera")]
        public string UsernameKorisnik { get; set; }


        [Required(ErrorMessage ="Morate uneti password")]
        [Display(Name = "Password")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Dužina password-a može biti između 5 i 20 karaktera")]
        public string PasswordKorisnika1 { get; set; }


        [Required(ErrorMessage ="Morate ponoviti password korisnika")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Dužina password-a može biti između 5 i 20 karaktera")]
        public string PasswordKorisnika2 { get; set; }


        [Required(ErrorMessage ="Morate uneti ime")]
        [StringLength(50, ErrorMessage = "Sistem ne podržava imena duža od 50 karaktera")]
        public string ImeKorisnika { get; set; }


        [Required(ErrorMessage ="Morate uneti prezime korisnika")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Dužina password-a može biti između 5 i 20 karaktera")]
        public string PrezimeKorisnika { get; set; }


        [Required(ErrorMessage ="Morate uneti adresu")]
        [StringLength(50, ErrorMessage = "Sistem ne podržava imena adresa dužih od 50 karaktera")]
        public string Adresa { get; set; }


        [Required(ErrorMessage = "Morate uneti mesto stanovanja")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Sistem ne podržava da polje mesto stanovanja bude duže od 50 karaktera")]
        public string MestoStanovanja { get; set; }


       
        //[ValidacijaDatuma(ErrorMessage = "Takmičenje je dozvoljeno za osobe između 18 i 50 godina.")]
        //[DateRange(ErrorMessage = "Takmičenje je dozvoljeno za osobe između 18 i 50 godina.")]
        [Required(ErrorMessage = "Morate uneti godine")]
        [Range(18, 50, ErrorMessage = "Takmičenje je dozvoljeno za osobe između 18 i 50 godina.")]
        public int Godine { get; set; }


        [Required(ErrorMessage = "Morate uneti broj telefona")]
        public string BrTelefona { get; set; }


        [Required(ErrorMessage = "Morate uneti email")]
        [EmailAddress]
        public string Email { get; set; }


        [Required(ErrorMessage = "Morate uneti težinu")]
        //ubaciti range za tezinu!!!
        [Range(40, 120, ErrorMessage = "Težina po kojoj se možete takmičiti može biti u opsegu od 40 do 120")]
        public int Tezina { get; set; }


        [Required(ErrorMessage = "Morate uneti visinu")]
        [Range(140, 250, ErrorMessage = "Morate uneti visinu u opsegu između 140 i 250")]
        public int Visina { get; set; }


        [Required(ErrorMessage ="Morate uneti borilačku visinu")]
        public string BorilackaVestina { get; set; }


        [Required(ErrorMessage ="Morate uneti broj dobijenih borbi")]
        [Range(0, 120, ErrorMessage = "Broj dobijenih borbi mora biti u opsegu 0 do 120")]
        public int BrDobijenihBorbi { get; set; }


        [Required(ErrorMessage = "Morate uneti broj izgubljenih borbi")]
        [Range(0, 120, ErrorMessage = "Broj iygubljenih borbi mora biti u opsegu 0 do 120")]
        public int BrIzgubljenihBorbi { get; set; }


        public int BrKartice { get; set; }

        public int IDUloge { get; set; }
    }
}