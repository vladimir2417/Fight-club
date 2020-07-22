using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mafa2.Web.Models.Interfaces
{
    interface INarudzbenicaRepository
    {
        List<NarudzbenicaBO> prikaziNarudzbenice();
        NarudzbenicaBO prikaziNarudzbenicuPoID(string IDNarudzbenice);
        void BrisiNarudzbenicu(string IDNarudzbenice);
        List<StavkeNarudzbeniceBO> PrikaziStavke(string IDNarudzbenice);
    }
}
