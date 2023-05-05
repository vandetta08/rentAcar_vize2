using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentACar.ViewModel
{
    public class MesajModel
    {
            public int MesajId { get; set; }
            public int MesajGonderenId { get; set; }
            public int MesajAlanId { get; set; }
            public string MesajIcerik { get; set; }
            public string MesajTarihi { get; set; }
            public string MesajKonu { get; set; }

        public string MesajAlanAd { get; set; }
        public string MesajAlanYetki { get; set; }

        public string MesajGonderenYetki { get; set; }
        public string MesajGonderenAd { get; set; }


    }
}