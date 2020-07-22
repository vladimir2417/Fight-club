using System;
using System.ComponentModel.DataAnnotations;

namespace Mafa2.Web.Models.CustomAnotacije
{
    public class DateRangeAttribute : RangeAttribute
    {
        public DateRangeAttribute() 
            : base(typeof(DateTime), DateTime.Now.AddYears(-50).ToShortDateString(), DateTime.Now.AddYears(-18).ToShortDateString())
        {

        }
    }
}