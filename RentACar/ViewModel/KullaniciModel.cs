using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentACar.ViewModel
{
    public class KullaniciModel
    {
        public int kullaniciId { get; set; }
        public string kullaniciAdSoyad { get; set; }
        public string kullaniciTel { get; set; }
        public string kullaniciMail { get; set; }
        public string kullaniciParola { get; set; }
        public string kullaniciYetki { get; set; }
    }
}