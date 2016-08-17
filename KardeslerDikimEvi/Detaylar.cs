using KardeslerDikimEvi.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KardeslerDikimEvi
{
    public partial class Detaylar : Form
    {
        internal string adiSoyadi { get; set; }
        public Detaylar()
        {
            InitializeComponent();
        }
        private MyBusiness _islemler = new MyBusiness();
        int Id;
        private void Detaylar_Load(object sender, EventArgs e)
        {
            listView1.Columns[0].Width = 0;
            string detaylar = adiSoyadi;
            lblAdiSoyadi.Text = detaylar;
            Musteriler musteriDetails = _islemler.musteriDetails(detaylar);
            lblAdres.Text = musteriDetails.Adres;
            lblTelefon.Text = musteriDetails.Telefon;
            Id = musteriDetails.ID;
            ListeyiDoldur();
        }
        private void ListeyiDoldur()
        {
            List<string> urunler = new List<string>();
            listView1.Items.Clear();
            _islemler.olcumlerListele(Id).ToList().ForEach(x =>
            {
                ListViewItem lst = new ListViewItem(x.ID.ToString());
                lst.SubItems.Add(x.Tarih.ToLongDateString());
                lst.SubItems.Add(x.Fiyat.ToString("C"));
                lst.SubItems.Add(x.Kapora1.ToString("C"));
                lst.SubItems.Add(x.Kapora2.ToString());


                string[] ilkparca = x.Degerler.Split('-');
                for (int i = 0; i < ilkparca.Length - 1; i++)
                {
                    string[] ikinciparca = ilkparca[i].Split(':');

                    string saglamparca = ikinciparca[0].Remove(0, 3);
                    string saglamparcaSon = saglamparca.Remove((saglamparca.Length - 1));
                    if (urunler.SingleOrDefault(z => z == saglamparcaSon) == null)
                    {
                        urunler.Add(saglamparcaSon);
                    }
                }
                string urun = "";
                urunler.ToList().ForEach(z =>
                {
                    urun += z + " - ";
                });
                lst.SubItems.Add(urun.Remove(urun.Length - 2));
                listView1.Items.Add(lst);
            });
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int olcumId = Convert.ToInt32(listView1.SelectedItems[0].Text);
            OlcumNewDetails olcumDetails = new OlcumNewDetails();
            olcumDetails.DetailsMi = true;
            olcumDetails.OlcumId = olcumId;

            olcumDetails.ShowDialog();
            ListeyiDoldur();
        }

        private void kapora2ÖdendiToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int Id = Convert.ToInt32(listView1.SelectedItems[0].Text);
            if (_islemler.kapora2Odendimi(Id))
            {
                MessageBox.Show("Kapora Zaten Ödenmiş. ", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {


                DialogResult result = MessageBox.Show("Kapora 2 Hesabı Ödemesinden Eminmisiniz.?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    if (_islemler.kapora2Odendi(Id))
                    {
                        MessageBox.Show("Ödeme Tamamlandı.");
                        ListeyiDoldur();
                    }
                    else
                    {
                        MessageBox.Show("Ödeme İşleminde Hata Oluştu.");
                    }
                }
            }
        }



        private void btnyeniDikimEkle_Click(object sender, EventArgs e)
        {
            OlcumNewDetails olcumDetails = new OlcumNewDetails();
            olcumDetails.DetailsMi = false;
            olcumDetails.OlcumId = Id;
            olcumDetails.ShowDialog();
            ListeyiDoldur();
        }

        private void yenileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListeyiDoldur();
        }
    }
}
