using KardeslerDikimEvi.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KardeslerDikimEvi
{
    public partial class OlcuIslemleri : Form
    {
        private MyBusiness _islemler;
        private static OlcuIslemleri nesne = new OlcuIslemleri();
        private OlcuIslemleri()
        {
            InitializeComponent();
        }
        public static OlcuIslemleri Nesne
        {
            get
            {
                if (nesne == null)
                    nesne = new OlcuIslemleri();

                return nesne;

            }
        }


        private void OlcuIslemleri_Load(object sender, EventArgs e)
        {
       
            Listele();
        }

        private void Listele()
        {
            try
            {
                _islemler = new MyBusiness();
                int toplam = _islemler.musteriListesi().Count;
                txtAltlbl.Text = "Toplam : " + toplam;
                lodingBar.Minimum = 0;
                lodingBar.Maximum = toplam;
                lodingBar.Step = 1;
                lodingBar.Value = 0;
                
                listView1.Items.Clear();
                _islemler.musteriListesi().ToList().ForEach(x =>
                {
                    lodingBar.Value += 1;
                    ListViewItem lst = new ListViewItem(x.AdiSoyadi);
                    lst.SubItems.Add(x.Telefon);
                    lst.SubItems.Add(x.Adres);
                    lst.SubItems.Add(x.Tarih.ToLongDateString());
                    listView1.Items.Add(lst);
                });
            }
            catch
            {
                MessageBox.Show("Müşteriler Listenirken Hata Oluştu.");
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Detaylar d = new Detaylar();
            d.adiSoyadi = listView1.SelectedItems[0].Text;
            d.ShowDialog();
            Listele();
        }


        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                string adisoyad = txtAdiSoyadi.Text.Trim();
                if (adisoyad == null && adisoyad == "")
                {
                    MessageBox.Show("Ad Soyad kısmını boş bırakmayın.");
                }
                Convert.ToDecimal(txtFiyat.Text.Trim());
                Convert.ToDecimal(txtKapora.Text.Trim());


                bool kontrol = _islemler.adsoyadKontrol(adisoyad);
                if (kontrol)
                {
                    MessageBox.Show("Aynı İsimde Müşteri Bulunmaktadır.");
                    return;
                }

                int musteriId = musteriEkle();
                if (musteriId == -1)
                {
                    MessageBox.Show("Müşteri Kaydedilirken Hata Oluştu");
                    return;
                }
                int olcumlerID = OlcumlerEkle(musteriId);
                if (olcumlerID == -1)
                {
                    MessageBox.Show("Hatalı Giriş");
                    return;
                }
                Listele();
                kutulariTemizle();
                tabControl1.SelectedIndex = 0;

            }
            catch (FormatException)
            {
                MessageBox.Show("Verileri Doğru Bir şekilde girin");
            }
            catch
            {
                MessageBox.Show("Kaydetme İşleminde Bir Hata Oluştu.");
            }
        }

        private void kutulariTemizle()
        {
            foreach (Control item in tabPage2.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
        }

        private int OlcumlerEkle(int musteriId)
        {
            try
            {
                Olcumler olcum = new Olcumler();
                olcum.MusteriID = musteriId;
                olcum.Fiyat = Convert.ToDecimal(txtFiyat.Text.Trim());
                olcum.Kapora1 = Convert.ToDecimal(txtKapora.Text.Trim());
                olcum.Degerler = OlcumDegerleriGetir();
                olcum.Not = txtNot.Text.Trim();

                return _islemler.OlcumEkle(olcum);
            }
            catch
            {
                return -1;
            }
        }
        private string OlcumDegerleriGetir()
        {
            string tumDegerler = "";
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    TextBox txtListesi = (TextBox)item;
                    if (txtListesi.Text != null)
                    {
                        tumDegerler += txtListesi.Name + ":" + txtListesi.Text + "-";
                    }
                }
            }
            return tumDegerler;
        }
        private int musteriEkle()
        {
            Musteriler m = new Musteriler();
            m.AdiSoyadi = txtAdiSoyadi.Text.Trim();
            m.Telefon = txtTelefon.Text.Trim();
            m.Adres = txtAdres.Text.Trim();
            int kontrol = _islemler.musteriEkle(m);
            return kontrol;
        }

        private void txtmusteriAra_KeyUp(object sender, KeyEventArgs e)
        {
            List<Musteriler> musteriler = _islemler.musteriAra(txtmusteriAra.Text.Trim());
            if (musteriler != null)
            {
                listView1.Items.Clear();
                musteriler.ToList().ForEach(x =>
                {
                    ListViewItem lst = new ListViewItem(x.AdiSoyadi);
                    lst.SubItems.Add(x.Telefon);
                    lst.SubItems.Add(x.Adres);
                    lst.SubItems.Add(x.Tarih.ToLongDateString());
                    listView1.Items.Add(lst);
                });
            }
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        private void yenileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Listele();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Musteriler> musteriler = _islemler.musteriBorclu().OrderByDescending(x=>x.Tarih).ToList();
            if (musteriler != null)
            {
                listView1.Items.Clear();
                musteriler.ToList().ForEach(x =>
                {
                    ListViewItem lst = new ListViewItem(x.AdiSoyadi);
                    lst.SubItems.Add(x.Telefon);
                    lst.SubItems.Add(x.Adres);
                    lst.SubItems.Add(x.Tarih.ToLongDateString());
                    listView1.Items.Add(lst);
                });
            }
        }

        private void btnTumListe_Click(object sender, EventArgs e)
        {
            Listele();
        }

        private void OlcuIslemleri_FormClosing(object sender, FormClosingEventArgs e)
        {
            nesne = null;
        }

        private void txtmusteriAra_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
