using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankaOtomasyon
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
        private void btnTicariHesapGecis_Click(object sender, EventArgs e)
        {
            MusteriLogin musteriLogin = new MusteriLogin();
            musteriLogin.Show();
            this.Hide();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void labelHesapAcGecis_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            KayitOlustur kayitOlustur = new KayitOlustur();
            kayitOlustur.Show();
        }
    }
}
