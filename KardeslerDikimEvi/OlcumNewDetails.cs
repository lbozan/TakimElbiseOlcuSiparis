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
    public partial class OlcumNewDetails : Form
    {
        public OlcumNewDetails()
        {
            InitializeComponent();
        }
        internal int OlcumId { get; set; } // Hem ölçüm Id'sidir Hem Müşteri Id'sidir.
        internal bool DetailsMi { get; set; }
        MyBusiness _islem = new MyBusiness();
        private void OlcumNewDetails_Load(object sender, EventArgs e)
        {

            if (DetailsMi)
            {
                int musteriId = _islem.olcumDetails(OlcumId).MusteriID;

                lblAdi.Text = _islem.MusteriDetails(musteriId).AdiSoyadi;
                lblTelefon.Text = _islem.MusteriDetails(musteriId).Telefon;
                // this.Width = 579;
                //this.Height = 189;
                //panel1.Visible = false;
                btnIptal.Visible = false;
                btnKaydet.Visible = false;
                btnDuzenle.Visible = true;
                kutulariKapat();
            }
            else
            {
                btnDuzenle.Visible = false;
                this.Width = 579;
                this.Height = 469;
                panel1.Visible = true;
                lblAdi.Text = _islem.MusteriDetails(OlcumId).AdiSoyadi;
                lblTelefon.Text = _islem.MusteriDetails(OlcumId).Telefon;
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
        private void kutulariKapat()
        {
            string olcum = _islem.olcumDetails(OlcumId).Degerler;
            List<Parcalar> parcaListesi = new List<Parcalar>();
            string[] ilkparca = olcum.Split('-');

            for (int i = 0; i < ilkparca.Length - 1; i++)
            {
                string[] parcaiki = ilkparca[i].Split(':');
                parcaListesi.Add(new Parcalar() { txtName = parcaiki[0], value = parcaiki[1] });
            }

            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    item.Enabled = false;
                    if (parcaListesi.SingleOrDefault(x => x.txtName == item.Name) != null)
                    {
                        item.Text = parcaListesi.SingleOrDefault(x => x.txtName == item.Name).value;
                    }
                }
            }
            Olcumler olcumDetail = new Olcumler();
            olcumDetail = _islem.olcumDetails(OlcumId);
            txtFiyat.Text = olcumDetail.Fiyat.ToString("C");
            txtNot.Text = olcumDetail.Not;
            txtKapora.Text = olcumDetail.Kapora1.ToString();
            txtKapora2.Text = olcumDetail.Kapora2.ToString();
            txtKapora2.Enabled = false;
            txtNot.Enabled = false;
            txtKapora.Enabled = false;
            txtFiyat.Enabled = false;

        }
        internal int duzenDurum = 0;
        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            duzenDurum = 1;
            foreach (Control item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    item.Enabled = true;

                }
                txtNot.Enabled = true;
                txtKapora.Enabled = true;
                txtFiyat.Enabled = true;
                txtKapora2.Enabled = false;

                btnKaydet.Visible = true;
            }
        }
        private void btnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                decimal fiyat = 0;
                decimal kapora = 0;
                decimal kapora2 = 0;

                if (txtFiyat.Text != "" && txtFiyat.Text != null)
                    fiyat = Convert.ToDecimal(txtFiyat.Text.Trim().Remove(txtFiyat.TextLength - 2));
                if (txtKapora.Text != "" && txtKapora != null)
                    kapora = Convert.ToDecimal(txtKapora.Text.Trim());
                if (txtKapora2.Text != "" && txtKapora2 != null)
                    kapora2 = Convert.ToDecimal(txtKapora2.Text.Trim());

                Olcumler olcum = new Olcumler();
                olcum.MusteriID = OlcumId;

                olcum.Fiyat = fiyat;
                olcum.Kapora1 = kapora;
                olcum.Kapora2 = kapora2;
                olcum.Degerler = OlcumDegerleriGetir();
                olcum.Not = txtNot.Text.Trim();
                if (duzenDurum == 1)
                {
                    int id = _islem.Duzenle(olcum);
                }
                else
                {
                    _islem.OlcumEkle(olcum);
                }
                this.Close();
            }
            catch
            {
                MessageBox.Show("Hatalı Giriş.");
            }
        }

        private void btnIptal_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OlcumNewDetails_FormClosed(object sender, FormClosedEventArgs e)
        {

            this.Dispose(true);

        }




    }
    public class Parcalar
    {
        public string txtName { get; set; }
        public string value { get; set; }
    }
}
