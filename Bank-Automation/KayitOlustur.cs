using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
using Npgsql;

namespace BankaOtomasyon
{
    public partial class KayitOlustur : Form
    {
        public KayitOlustur()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
           "Database=BankaOtomasyonu; user ID=postgres; password=1234");
        private void KayitOlustur_Load(object sender, EventArgs e)
        {
            comboBox1.Items.Add(MusteriTipi.bireysel);
            comboBox1.Items.Add(MusteriTipi.ticari);
        }
        private void kayitOl_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                NpgsqlCommand komutEkle = new NpgsqlCommand("insert into musteri (musteri_id, musteri_ad, musteri_tipi, musteri_sifre)" +
                    " values (@pid, @pAd, @pTip, @pSifre)", baglanti);

                komutEkle.Parameters.AddWithValue("@pid", Convert.ToInt32(txtId.Text));
                komutEkle.Parameters.AddWithValue("@pAd", txtAd.Text);
                komutEkle.Parameters.AddWithValue("@pTip", comboBox1.Text);
                komutEkle.Parameters.AddWithValue("@pSifre", txtSifre.Text);
                komutEkle.ExecuteNonQuery();
                baglanti.Close();

                MusteriLogin musteriLogin = new MusteriLogin();
                musteriLogin.Show();
                this.Close();
            }
            catch (Exception em)
            {
                MessageBox.Show("Exception caught " + em);
            }
        }
    }
}
