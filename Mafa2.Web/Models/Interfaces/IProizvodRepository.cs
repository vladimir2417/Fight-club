using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mafa2.Web.Models.Interfaces
{
   public interface IProizvodRepository
    {
        List<ProizvodBO> prikaziProizvode();
        ProizvodBOzaAzuriranje prikaziProizvodePoId(string SifraProizvoda);
        void UnesiProizvod(ProizvodBO proizvod);
        void IzmeniProizvod(ProizvodBOzaAzuriranje proizvod);
        void BrisiProizvod(string SifraProizvoda);
        List<KatalogBO> PrikaziKatalog();
    }
}
