using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Mafa2.Web.Models.CustomAnotacije
{

    public class ValidacijaZahtevanogDatumaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime d = Convert.ToDateTime(value);
            //dozvoljen je da izabere datum od sutra do 2 meseca unapred
            //unesenDatum > DateTime.Now && unesenDatum < DateTime.Now.AddMonths(2)
           if(d < DateTime.Now || d > DateTime.Now.AddMonths(2))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}