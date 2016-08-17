using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace KardeslerDikimEvi
{
    public partial class Anamenu : Form
    {
        internal bool IsLogin { get; set; }
        public Anamenu()
        {
            IsLogin = false;
            InitializeComponent();
            this.Opacity = 0.0;
            Login loginForm = new Login();
            loginForm.anamenu = this;
            loginForm.ShowDialog();


        }


        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Anamenu_Load(object sender, EventArgs e)
        {
            if (IsLogin)
            {
                this.Opacity = 100;
            }
            else
            {
                this.Close();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OlcuIslemleri sha = OlcuIslemleri.Nesne;
            sha.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yapım Aşamasında...");
        }

        private void Anamenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SqlConnection connect = new SqlConnection("Server=.; Database= TerziDb; Integrated Security=SSPI;");
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string hedefDizin = saveFileDialog1.FileName + "--" + DateTime.Now.ToShortDateString();
                try
                {

                    connect.Open();
                    SqlCommand command;
                    command = new SqlCommand(@"backup database TerziDb to disk ='" + hedefDizin + ".bak' with init,stats=10", connect);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Yedekleme Başarılı..");
                }
                catch (Exception)
                {
                    MessageBox.Show("Hata: Yedekleme Başarısız.");
                }
                finally
                {
                    connect.Close();
                }

            }


        }

        private void hakkımızdaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Developer Lokman \n lbozan@outlook.com ", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }




    }
}
