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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        internal Anamenu anamenu { get; set; }
        private void btnGiris_Click(object sender, EventArgs e)
        {

            string username = txtuserName.Text.Trim();
            string password = txtpassword.Text.Trim();
            if (username != null && username != "" && password != "" && password != null)
            {

                MyBusiness islem = new MyBusiness();
                bool kontrol = islem.Login(username, password);
                Anamenu menu = anamenu;
                if (kontrol)
                {
                    menu.IsLogin = true;
                    this.Close();
                }
                else
                {
                    menu.IsLogin = false;
                    MessageBox.Show("Kullanıcı Adı veya Şifre Yanlış girildi.");
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı Adı veya Şifre boş girmeyin ");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
