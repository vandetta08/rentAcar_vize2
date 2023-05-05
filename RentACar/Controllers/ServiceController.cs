using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RentACar.Models;
using RentACar.ViewModel;

namespace RentACar.Controllers
{
    public class ServiceController : ApiController
    {
        RentACarDbEntities1 db = new RentACarDbEntities1();
        SonucModel sonuc = new SonucModel();

        #region Kategori

        [HttpGet]
        [Route("api/kategoriliste")]
        public List<KategoriModel> KategoriListe()
        {
            List<KategoriModel> liste = db.Kategori.Select(x => new KategoriModel()
            {
                kategoriId = x.kategoriId,
                kategoriAd = x.kategoriAd
            }).ToList();
            return liste;
        }



        [HttpPost]
        [Route("api/kategoriekle")]
        public SonucModel KategoriEkle(KategoriModel model)
        {
            if (db.Kategori.Count(s => s.kategoriAd == model.kategoriAd) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kategori bilgileri sistemde mevcuttur";
                return sonuc;
            }
            Kategori yeni = new Kategori();
            yeni.kategoriAd = model.kategoriAd;
            db.Kategori.Add(yeni);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kategori bilgileri sisteme eklenmiştir.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/kategoriduzenle")]

        public SonucModel KategoriDuzenle(KategoriModel model)
        {
            Kategori kayit = db.Kategori.Where(s => s.kategoriId == model.kategoriId).FirstOrDefault(); if (kayit == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kategori bilgileri bulunamadı. Lütfen geçerli bir kategori seçiniz.";
                return sonuc;
            }
            kayit.kategoriAd = model.kategoriAd;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kategori bilgileri başarılı bir şekilde düzenlenmiştir.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kategorisil/{kategoriid}")]
        public SonucModel KategoriSil(int kategoriId)
        {
            Kategori kayit = db.Kategori.Where(s => s.kategoriId == kategoriId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kategori bilgileri bulunamadı.";
                return sonuc;
            }
            if (db.Araba.Count(s => s.arabaKategoriId == kategoriId) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Araba kaydı bulunan kategori silinemez. Lütfen araba bilgilerini siliniz.";
                return sonuc;
            }
            db.Kategori.Remove(kayit);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kategori bilgileri başarılı bir şekilde silindi.";
            return sonuc;
        }

        #endregion

        #region Araba

        [HttpGet]
        [Route("api/arabaliste")]
        public List<ArabaModel> ArabaListe()
        {
            List<ArabaModel> liste = db.Araba.Select(x => new ArabaModel()
            {
                arabaId = x.arabaId,
                arabaMarka = x.arabaMarka,
                arabaModel = x.arabaModel,
                arabaRenk = x.arabaRenk,
                arabaKasaTipi = x.arabaKasaTipi,
                arabaYakit = x.arabaYakit,
                arabaKm = x.arabaKm,
                arabaGorsel = x.arabaGorsel,
                arabaFiyat = x.arabaFiyat,
                arabaKategoriId = x.arabaKategoriId,
                arabaKodu = x.arabaKodu,
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/arabalistebykatid/{kategoriId}")]
        public List<ArabaModel> ArabaListeByKatId(int kategoriId)
        {
            List<ArabaModel> liste = db.Araba.Where(s => s.arabaKategoriId == kategoriId).Select(x => new ArabaModel()
            {
                arabaId = x.arabaId,
                arabaMarka = x.arabaMarka,
                arabaModel = x.arabaModel,
                arabaRenk = x.arabaRenk,
                arabaKasaTipi = x.arabaKasaTipi,
                arabaYakit = x.arabaYakit,
                arabaKm = x.arabaKm,
                arabaGorsel = x.arabaGorsel,
                arabaFiyat = x.arabaFiyat,
                arabaKategoriId = x.arabaKategoriId,
                arabaKodu = x.arabaKodu
            }).ToList();
            return liste;
        }
        [HttpGet]
        [Route("api/kategoribyid/{arabaId}")]
        public ArabaModel ArabaById(int arabaId)
        {
            ArabaModel kayit = db.Araba.Where(s => s.arabaId == arabaId).Select(x => new ArabaModel()
            {
                arabaId = x.arabaId,
                arabaMarka = x.arabaMarka,
                arabaModel = x.arabaModel,
                arabaRenk = x.arabaRenk,
                arabaKasaTipi = x.arabaKasaTipi,
                arabaYakit = x.arabaYakit,
                arabaKm = x.arabaKm,
                arabaGorsel = x.arabaGorsel,
                arabaFiyat = x.arabaFiyat,
                arabaKategoriId = x.arabaKategoriId,
                arabaKodu = x.arabaKodu
            }
            ).FirstOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/arabaekle")]
        public SonucModel ArabaEkle(ArabaModel model)
        {
            if (db.Araba.Count(s => s.arabaKodu == model.arabaKodu) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Araba ilgili kategoride mevcuttur.";
                return sonuc;
            }
            Araba yeni = new Araba();
            yeni.arabaMarka = model.arabaMarka;
            yeni.arabaModel = model.arabaModel;
            yeni.arabaRenk = model.arabaRenk;
            yeni.arabaKasaTipi = model.arabaKasaTipi;
            yeni.arabaYakit = model.arabaYakit;
            yeni.arabaKm = model.arabaKm;
            yeni.arabaGorsel = model.arabaGorsel;
            yeni.arabaFiyat = model.arabaFiyat;
            yeni.arabaKategoriId = model.arabaKategoriId;
            yeni.arabaKodu = model.arabaKodu;
            db.Araba.Add(yeni);
            db.SaveChanges();""
            sonuc.Islem = true;
            sonuc.Mesaj = "Araba bilgileri başarılı bir şekilde sisteme eklenmiştir.";
            return sonuc;
        }




        [HttpPut]
        [Route("api/arabaduzenle")]
        public SonucModel ArabaDuzenle(ArabaModel model)
        {
            Araba kayit = db.Araba.Where(s => s.arabaId == model.arabaId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girdiğiniz bilgilere ait araba bilgisi bulunamadı. Geçerli bir araba giriniz.";
                return sonuc;
            }
            kayit.arabaMarka = model.arabaMarka;
            kayit.arabaModel = model.arabaModel;
            kayit.arabaRenk = model.arabaRenk;
            kayit.arabaKasaTipi = model.arabaKasaTipi;
            kayit.arabaYakit = model.arabaYakit;
            kayit.arabaKm = model.arabaKm;
            kayit.arabaGorsel = model.arabaGorsel;
            kayit.arabaFiyat = model.arabaFiyat;
            kayit.arabaKategoriId = model.arabaKategoriId;
            kayit.arabaKodu = model.arabaKodu;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Araba  bilgileri başarılı bir şekilde güncellenmiştir.";
            return sonuc;
        }
        [HttpDelete]
        [Route("api/arabasil/{arabaId}")]
        public SonucModel ArabaSil(int arabaId)
        {
            Araba kayit = db.Araba.Where(s => s.arabaId == arabaId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girdiğiniz bilgilere ait araba bulunamadı.";
                return sonuc;
            }
            db.Araba.Remove(kayit);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Araba bilgileri silinmiştir.";
            return sonuc;
        }


        [HttpPost]
        [Route("api/arabafotoguncelle")]
        public SonucModel ArabaFotoGuncelle(ArabaGorselModel model)
        {
            Araba araba = db.Araba.Where(s => s.arabaId == model.arabaId).SingleOrDefault();
            if (araba == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kayıt bulunamadı.";
                return sonuc;
            }

            if (araba.arabaGorsel != "default.jpg")
            {
                string yol = System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + araba.arabaGorsel);
                if (File.Exists(yol))
                {
                    File.Delete(yol);
                }
            }
            string data = model.gorselData;
            string base64 = data.Substring(data.IndexOf(',') + 1);
            base64 = base64.Trim('\0');
            byte[] imgbyte = Convert.FromBase64String(base64);
            string dosyaAdi = araba.arabaId + model.gorselUzanti.Replace("image/", ".");

            using (var ms = new MemoryStream(imgbyte, 0, imgbyte.Length))
            {
                Image img = Image.FromStream(ms, true);
                img.Save(System.Web.Hosting.HostingEnvironment.MapPath("~/Dosyalar/" + dosyaAdi));
            }
            araba.arabaGorsel = dosyaAdi;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Fotoğraf güncellendi.";
            return sonuc;
        }

        #endregion

        #region Kullanıcı

        [HttpGet]
        [Route("api/kullaniciliste")]
        public List<KullaniciModel> KullaniciListe()
        {
            List<KullaniciModel> liste = db.Kullanici.Select(x => new KullaniciModel()
            {
                kullaniciId = x.kullaniciId,
                kullaniciAdSoyad = x.kullaniciAdSoyad,
                kullaniciTel = x.kullaniciTel,
                kullaniciMail = x.kullaniciMail,
                kullaniciParola = x.kullaniciParola,
                kullaniciYetki = x.kullaniciYetki,
            }).ToList();
            return liste;
        }


        [HttpGet]
        [Route("api/kullanicibyid/{kullaniciId}")]
        public KullaniciModel KullaniciById(int kullaniciId)
        {
            KullaniciModel kayit = db.Kullanici.Where(s => s.kullaniciId == kullaniciId).Select(x => new KullaniciModel()
            {
                kullaniciId = x.kullaniciId,
                kullaniciAdSoyad = x.kullaniciAdSoyad,
                kullaniciTel = x.kullaniciTel,
                kullaniciMail = x.kullaniciMail,
                kullaniciParola = x.kullaniciParola,
                kullaniciYetki = x.kullaniciYetki,
            }).FirstOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/kullaniciekle")]

        public SonucModel KullaniciEkle(KullaniciModel model)
        {
            if (db.Kullanici.Count(s => s.kullaniciMail == model.kullaniciMail) > 0)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girdiğiniz mail adresi ile ilgili kayıt bulunmaktadır.";
                return sonuc;
            }
            Kullanici yeni = new Kullanici();
            yeni.kullaniciAdSoyad = model.kullaniciAdSoyad;
            yeni.kullaniciTel = model.kullaniciTel;
            yeni.kullaniciMail = model.kullaniciMail;
            yeni.kullaniciParola = model.kullaniciParola;
            yeni.kullaniciYetki = model.kullaniciYetki;
            db.Kullanici.Add(yeni);
            db.SaveChanges();

            sonuc.Islem = true;
            sonuc.Mesaj = "Kullanıcı bilgileri sisteme eklenmiştir.";
            return sonuc;
        }

        [HttpPut]
        [Route("api/kullaniciduzenle")]
        public SonucModel KullaniciDuzenle(KullaniciModel model)
        {
            Kullanici kayit = db.Kullanici.Where(s => s.kullaniciId == model.kullaniciId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Girdiğiniz bilgilere ait kullanıcı bilgisi bulunamadı.";
                return sonuc;
            }
            kayit.kullaniciAdSoyad = model.kullaniciAdSoyad;
            kayit.kullaniciTel = model.kullaniciTel;
            kayit.kullaniciMail = model.kullaniciMail;
            kayit.kullaniciParola = model.kullaniciParola;
            kayit.kullaniciYetki = model.kullaniciYetki;
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kullanıcı bilgileri başarılı bir şekilde güncellenmiştir.";
            return sonuc;
        }

        [HttpDelete]
        [Route("api/kullanicisil/{kullaniciId}")]
        public SonucModel KullaniciSil(int kullaniciId)
        {
            Kullanici kayit = db.Kullanici.Where(s => s.kullaniciId == kullaniciId).FirstOrDefault();
            if (kayit == null)
            {
                sonuc.Islem = false;
                sonuc.Mesaj = "Kullanıcı bilgisi bulunamadı.";
                return sonuc;
            }
            db.Kullanici.Remove(kayit);
            db.SaveChanges();
            sonuc.Islem = true;
            sonuc.Mesaj = "Kullanıcı bilgileri sistemden silinmiştir.";
            return sonuc;
        }
        #endregion


        [HttpGet]
        [Route("api/girisyap/{mail}/{sifre}")]
        public KullaniciModel GirisYap(string mail, string sifre)
        {
            KullaniciModel kullanicimodel = db.Kullanici.Where(s => s.kullaniciMail == mail && s.kullaniciParola == sifre).Select(x => new KullaniciModel()
            {
                kullaniciId=x.kullaniciId,
                kullaniciAdSoyad = x.kullaniciAdSoyad,
                kullaniciMail = x.kullaniciMail,
                kullaniciParola = x.kullaniciParola,
                kullaniciYetki = x.kullaniciYetki
            }).SingleOrDefault();
            if (kullanicimodel != null)
            {
                return kullanicimodel;
            }

            return null;
        }

        #region Mesaj

        [HttpGet]
        [Route("api/mesajliste")]
        public List<MesajModel> MesajListe()
        {
            List<MesajModel> liste = db.Mesajlar.Select(x => new MesajModel()
            {
                MesajId = x.MesajId,
                MesajAlanId = x.MesajAlanId,
                MesajGonderenId = x.MesajGonderenId,
                MesajIcerik = x.MesajIcerik,
                MesajKonu = x.MesajKonu,
                MesajTarihi = x.MesajTarihi,
                 MesajAlanAd =x.Kullanici.kullaniciAdSoyad,
                 MesajGonderenAd =x.Kullanici.kullaniciAdSoyad,
                 MesajAlanYetki =x.Kullanici.kullaniciYetki,
                 MesajGonderenYetki =x.Kullanici.kullaniciYetki

            }).ToList();
            return liste;
        }


        [HttpGet]
        [Route("api/mesajbyid/{mesajId}")]
        public MesajModel MesajById(int mesajId)
        {
            MesajModel kayit = db.Mesajlar.Where(s => s.MesajId == mesajId).Select(x => new MesajModel()
            {
                MesajId = x.MesajId,
                MesajAlanId = x.MesajAlanId,
                MesajGonderenId = x.MesajGonderenId,
                MesajIcerik = x.MesajIcerik,
                MesajKonu = x.MesajKonu,
                MesajTarihi = x.MesajTarihi,
                MesajAlanAd = x.Kullanici.kullaniciAdSoyad,
                MesajGonderenAd = x.Kullanici.kullaniciAdSoyad,
                MesajAlanYetki = x.Kullanici.kullaniciYetki,
                MesajGonderenYetki = x.Kullanici.kullaniciYetki
            }).FirstOrDefault();
            return kayit;
        }

        [HttpGet]
        [Route("api/mesajalanbyid/{mesajalanId}")]
        public MesajModel MesajAlanById(int mesajalanId)
        {
            MesajModel kayit = db.Mesajlar.Where(s => s.MesajAlanId == mesajalanId).Select(x => new MesajModel()
            {
                MesajId = x.MesajId,
                MesajAlanId = x.MesajAlanId,
                MesajGonderenId = x.MesajGonderenId,
                MesajIcerik = x.MesajIcerik,
                MesajKonu = x.MesajKonu,
                MesajTarihi = x.MesajTarihi,
                MesajAlanAd = x.Kullanici.kullaniciAdSoyad,
                MesajGonderenAd = x.Kullanici.kullaniciAdSoyad,
                MesajAlanYetki = x.Kullanici.kullaniciYetki,
                MesajGonderenYetki = x.Kullanici.kullaniciYetki
            }).FirstOrDefault();
            return kayit;
        }

        [HttpPost]
        [Route("api/mesajekle")]

        public SonucModel MesajEkle(MesajModel model)
        {
           
            Mesajlar yeni = new Mesajlar();
            yeni.MesajAlanId = model.MesajAlanId;
            yeni.MesajGonderenId = model.MesajGonderenId;
            yeni.MesajIcerik = model.MesajIcerik;
            yeni.MesajKonu = model.MesajKonu;
            yeni.MesajTarihi = model.MesajTarihi;
            db.Mesajlar.Add(yeni);
            db.SaveChanges();

            sonuc.Islem = true;
            sonuc.Mesaj = "Mesaj gönderilmiştir..";
            return sonuc;
        }
               
        #endregion


    }
}
