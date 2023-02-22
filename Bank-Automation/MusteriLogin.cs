using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankaOtomasyon
{
    public partial class MusteriLogin : Form
    {
        public MusteriLogin()
        {
            InitializeComponent();
        }
      
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
          "Database=BankaOtomasyonu; user ID=postgres; password=1234");
        public int Idkontrol { get; set; }
        public void btnMusteriAnasayfaGecis_Click(object sender, EventArgs e)
        {
            login login = new login();
            login.Show();
            this.Close();
        }
        public void btnLoginPageGecis_Click(object sender, EventArgs e)
        {
            login welcomePageGecis = new login();
            welcomePageGecis.Show();
            this.Close();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void btnHesabaGiris_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                Idkontrol = int.Parse(txtIdkontrol.Text);

                string sorgu = "select count(*) from musteri where musteri_id='" + txtIdkontrol.Text + "'" +
                    " and musteri_sifre='" + txtSifreKontrol.Text + "'";

                NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                npgsqlDataAdapter.Fill(dt);

                // Eğer sorgu sonucunda bir kayıt döndürülürse, giriş başarılıdır
                if (dt.Rows[0][0].ToString() == "1")
                {
                    // Giriş işlemine devam edin
                    HesapGecis hesapGecis = new HesapGecis();
                    hesapGecis.musteriid = this.Idkontrol;
                    hesapGecis.Show();

                    baglanti.Close();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Hatalı giriş ");
                }
            }
            catch (Exception em)
            {
                MessageBox.Show("Hata: " + em);
            }
        }
    }
}
