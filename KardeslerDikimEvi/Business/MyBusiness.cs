using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KardeslerDikimEvi.Business
{
    public class MyBusiness
    {
        private TerziDbEntities _db;
        public MyBusiness()
        {
            _db = new TerziDbEntities();
        }

        public List<Musteriler> musteriListesi()
        {
            List<Musteriler> musteriler = new List<Musteriler>();

            _db.Musteriler.Where(x => x.Aktifmi == true).OrderByDescending(x => x.Tarih).ToList().ForEach(x =>
            {
                musteriler.Add(new Musteriler() { AdiSoyadi = x.AdiSoyadi, Telefon = x.Telefon, Adres = x.Adres, Tarih = x.Tarih });
            });
            return musteriler;
        }

        internal bool adsoyadKontrol(string adisoyad)
        {
            try
            {
                var kontrol = _db.Musteriler.FirstOrDefault(x => x.AdiSoyadi == adisoyad);
                if (kontrol != null)
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        internal int musteriEkle(Musteriler m)
        {
            try
            {
                Musteriler musteri = new Musteriler();
                musteri.AdiSoyadi = m.AdiSoyadi;
                musteri.Telefon = m.Telefon;
                musteri.Adres = m.Adres;
                musteri.Tarih = DateTime.Today;
                musteri.Aktifmi = true;
                _db.Musteriler.Add(musteri);
                _db.SaveChanges();
                return musteri.ID;
            }
            catch
            {
                return -1;
            }
        }

        internal int OlcumEkle(Olcumler olcum)
        {
            Olcumler o = new Olcumler();
            o.Fiyat = olcum.Fiyat;
            o.Kapora1 = olcum.Kapora1;
            o.MusteriID = olcum.MusteriID;
            o.Degerler = olcum.Degerler;
            o.Not = olcum.Not;
            o.Tarih = DateTime.Today;
            _db.Olcumler.Add(o);
            _db.SaveChanges();
            return o.ID;
        }
        internal int Duzenle(Olcumler olcum)
        {


            Olcumler o = _db.Olcumler.SingleOrDefault(x => x.ID == olcum.MusteriID);
            o.Fiyat = olcum.Fiyat;
            o.Kapora1 = olcum.Kapora1;
            o.Kapora2 = olcum.Kapora2;
            o.ID = olcum.MusteriID;
            o.Degerler = olcum.Degerler;
            o.Not = olcum.Not;
            _db.SaveChanges();
            return o.ID;
        }
        internal List<Musteriler> musteriAra(string p)
        {
            List<Musteriler> musteri = _db.Musteriler.Where(x => x.AdiSoyadi.Contains(p) || x.Telefon.Contains(p)).ToList();
            return musteri;
        }

        internal bool Login(string username, string password)
        {
            try
            {
                Users user = _db.Users.SingleOrDefault(x => x.Username == username && x.Password == password);
                if (user == null)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal Musteriler musteriDetails(string detaylar)
        {
            return _db.Musteriler.SingleOrDefault(x => x.AdiSoyadi == detaylar);
        }

        internal List<Olcumler> olcumlerListele(int Id)
        {
            return _db.Olcumler.Where(x => x.MusteriID == Id).OrderByDescending(x => x.Tarih).ToList();
        }

        internal bool kapora2Odendi(int Id)
        {
            try
            {
                var a = _db.Olcumler.SingleOrDefault(x => x.ID == Id);
                a.Kapora2 = a.Fiyat - a.Kapora1;
                _db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        internal Olcumler olcumDetails(int OlcumId)
        {
            var demo = _db.Olcumler.SingleOrDefault(x => x.ID == OlcumId);
            return demo;
        }

        internal List<Musteriler> musteriBorclu()
        {
            List<Musteriler> musteriListesi = new List<Musteriler>();
            _db.Olcumler.Where(x => x.Kapora2 == null).ToList().ForEach(x =>
            {
                var m = _db.Musteriler.SingleOrDefault(z => z.ID == x.MusteriID && z.Aktifmi == true);
                musteriListesi.Add(new Musteriler() { AdiSoyadi = m.AdiSoyadi, Adres = m.Adres, Tarih = m.Tarih, Telefon = m.Telefon, ID = m.ID });
            });
            return musteriListesi;
        }
        internal Musteriler MusteriDetails(int Id)
        {
            List<Musteriler> musteriDetails = new List<Musteriler>();
            var m = _db.Musteriler.Where(x => x.ID == Id).SingleOrDefault();
            return m;
        }





        internal bool kapora2Odendimi(int Id)
        {
            if (_db.Olcumler.SingleOrDefault(x => x.ID == Id).Kapora2 == null || _db.Olcumler.SingleOrDefault(x => x.ID == Id).Kapora2 == 0)
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
