using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Mafa2.Web.Models
{
    public class ValidateDateRange : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // your validation logic
            DateTime unesenDatum = (DateTime)value;
            if (unesenDatum >= Convert.ToDateTime("01/01/1969") && unesenDatum <= Convert.ToDateTime("01/05/2002"))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Date is not in given range.");
            }
        }
    }

}
