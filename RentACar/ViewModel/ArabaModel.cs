using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentACar.ViewModel
{
    public class ArabaModel
    {
        public int arabaId { get; set; }
        public string arabaMarka { get; set; }
        public string arabaModel { get; set; }
        public string arabaRenk { get; set; }
        public string arabaKasaTipi { get; set; }
        public string arabaYakit { get; set; }
        public string arabaKm { get; set; }
        public string arabaGorsel { get; set; }
        public decimal arabaFiyat { get; set; }
        public int arabaKategoriId { get; set; }
        public string arabaKodu { get; set; }
    }
}