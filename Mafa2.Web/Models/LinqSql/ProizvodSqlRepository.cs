using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mafa2.Web.Models.LinqSql;
using System.IO;
using System.Web.Helpers;
using System.Web.Mvc;
using Mafa2.Web.Models.Interfaces;

namespace Mafa2.Web.Models.LinqSql
{
    public class ProizvodSqlRepository: IProizvodRepository
    {
        private DataClasses1DataContext proizvodiDataContext = new DataClasses1DataContext();

        public List<ProizvodBO> prikaziProizvode()
        {
            List<ProizvodBO> proizvodi = new List<ProizvodBO>();

            foreach (Proizvod proizvod in proizvodiDataContext.Proizvods)
            {
                ProizvodBO proizvodBO = new ProizvodBO();
                proizvodBO.SifraProizvoda = proizvod.SifraProizvoda;
                proizvodBO.Naziv = proizvod.Naziv;
                proizvodBO.Kolicina = proizvod.Kolicina;
                proizvodBO.Opis = proizvod.Opis;
                proizvodBO.Cena = proizvod.Cena;
                proizvodBO.Proizvodjac = proizvod.Proizvodjac;
                proizvodBO.Popust = proizvod.Popust.GetValueOrDefault();
                proizvodBO.Katalog = new KatalogBO() { IDKatalog = proizvod.Katalog.IDKatalog, NazivKataloga = proizvod.Katalog.NazivKataloga };
                proizvodBO.Slika = proizvod.Slika;
                proizvodBO.AltSlika = proizvod.AltSlika;
                proizvodBO.UkupnaCena = (double)proizvod.UkupnaCena;
                proizvodi.Add(proizvodBO);
            }
            return proizvodi;
        }

        public ProizvodBOzaAzuriranje prikaziProizvodePoId(string SifraProizvoda)
        {
            Proizvod proizvod = proizvodiDataContext.Proizvods.FirstOrDefault(p => p.SifraProizvoda == SifraProizvoda);
            ProizvodBOzaAzuriranje proizvodBO = new ProizvodBOzaAzuriranje();
            proizvodBO.SifraProizvoda = proizvod.SifraProizvoda;
            proizvodBO.Naziv = proizvod.Naziv;
            proizvodBO.Kolicina = proizvod.Kolicina;
            proizvodBO.Opis = proizvod.Opis;
            proizvodBO.Cena = proizvod.Cena;
            proizvodBO.Proizvodjac = proizvod.Proizvodjac;
            proizvodBO.Popust = proizvod.Popust.GetValueOrDefault();
            proizvodBO.Katalog = new KatalogBO() { IDKatalog = proizvod.Katalog.IDKatalog, NazivKataloga = proizvod.Katalog.NazivKataloga };
            proizvodBO.Slika = proizvod.Slika;
            proizvodBO.AltSlika = proizvod.AltSlika;
            proizvodBO.UkupnaCena = (double)proizvod.UkupnaCena;
            return proizvodBO;
        }


        public void UnesiProizvod(ProizvodBO proizvodBO)
        {
            string fileName = Path.GetFileNameWithoutExtension(proizvodBO.ImageFile.FileName);
            string fileName2;
            string extension = Path.GetExtension(proizvodBO.ImageFile.FileName);
            fileName = fileName + extension;
            fileName2 = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/SlikeProizvoda/"), fileName);
            proizvodBO.ImageFile.SaveAs(fileName2);
            Proizvod proizvodi = new Proizvod
            {
                SifraProizvoda = proizvodBO.SifraProizvoda,
                Naziv = proizvodBO.Naziv,
                Kolicina = proizvodBO.Kolicina,
                Opis = proizvodBO.Opis,
                Cena = proizvodBO.Cena,
                Proizvodjac = proizvodBO.Proizvodjac,
                Popust = proizvodBO.Popust,
                IDKatalog = proizvodBO.Katalog.IDKatalog,
                Slika = "~/Content/SlikeProizvoda/" + fileName,
                AltSlika = proizvodBO.AltSlika,
                UkupnaCena = proizvodBO.Cena - (proizvodBO.Cena * proizvodBO.Popust) / 100
            };
            proizvodiDataContext.Proizvods.InsertOnSubmit(proizvodi);
            proizvodiDataContext.SubmitChanges();
        }

        public void IzmeniProizvod(ProizvodBOzaAzuriranje proizvodBO)
        {
            Proizvod proizvodZaAzuriranje = proizvodiDataContext.Proizvods.FirstOrDefault(t => t.SifraProizvoda == proizvodBO.SifraProizvoda);
            if (proizvodZaAzuriranje == null) return;
            proizvodZaAzuriranje.SifraProizvoda = proizvodBO.SifraProizvoda;
            proizvodZaAzuriranje.Naziv = proizvodBO.Naziv;
            proizvodZaAzuriranje.Kolicina = proizvodBO.Kolicina;
            proizvodZaAzuriranje.Opis = proizvodBO.Opis;
            proizvodZaAzuriranje.Cena = proizvodBO.Cena;
            proizvodZaAzuriranje.Proizvodjac = proizvodBO.Proizvodjac;
            proizvodZaAzuriranje.Popust = proizvodBO.Popust;
            proizvodZaAzuriranje.IDKatalog = proizvodBO.Katalog.IDKatalog;
            if (proizvodBO.ImageFile != null)
            {
                string fileName = Path.GetFileNameWithoutExtension(proizvodBO.ImageFile.FileName);
                string fileName2;
                string extension = Path.GetExtension(proizvodBO.ImageFile.FileName);
                fileName = fileName + extension;
                fileName2 = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/SlikeProizvoda/"), fileName);
                proizvodBO.ImageFile.SaveAs(fileName2);
                proizvodZaAzuriranje.Slika = "~/Content/SlikeProizvoda/" + fileName;
            }
            proizvodZaAzuriranje.AltSlika = proizvodBO.AltSlika;
            proizvodZaAzuriranje.UkupnaCena = proizvodBO.Cena - (proizvodBO.Cena * proizvodBO.Popust) / 100;
            proizvodiDataContext.SubmitChanges();
        }

        public void BrisiProizvod(string SifraProizvoda)
        {
            Proizvod proizvodZaBrisanje = proizvodiDataContext.Proizvods.FirstOrDefault(t => t.SifraProizvoda == SifraProizvoda);
            proizvodiDataContext.Proizvods.DeleteOnSubmit(proizvodZaBrisanje);
            proizvodiDataContext.SubmitChanges();
        }

        public List<KatalogBO> PrikaziKatalog()
        {
            List<KatalogBO> katalozi = new List<KatalogBO>();
            foreach (Katalog katalog in proizvodiDataContext.Katalogs)
            {
                KatalogBO katalogBO = new KatalogBO()
                {
                    IDKatalog = katalog.IDKatalog,
                    NazivKataloga = katalog.NazivKataloga
                };
                katalozi.Add(katalogBO);
            }
            return katalozi;
        }
    }
}