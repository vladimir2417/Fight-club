using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mafa2.Web.Models
{
    public class KatalogBO
    {
        public string NazivKataloga { get; set; }
        public int IDKatalog { get; set; }

        public override string ToString()
        {
            return NazivKataloga;
        }
    }
}