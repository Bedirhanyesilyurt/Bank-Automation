using Npgsql;
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
    public partial class ParaYatirma : Form
    {
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
          "Database=BankaOtomasyonu; user ID=postgres; password=1234");
        public ParaYatirma()
        {
            InitializeComponent();
        }
        public int hesapnumarası { get; set; }
        public double bakiye { get; set; } 
        public Islem islem { get; set; }

        public void button1_Click(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            try
            {
                DialogResult result = MessageBox.Show("Hesabınıza Para yatırma işlemi yapılacaktır onaylıyor musunuz ", "Para Yatırma İşlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    //islem.tarih = currentTime;
                    bakiye += Convert.ToInt16(txtParayatir.Text);

                    string sorgu = "update hesap set hesap_bakiye ='" + bakiye + "' where hesap_id ='" + hesapnumarası + "'";

                    baglanti.Open();
                    NpgsqlCommand Parayatirma = new NpgsqlCommand(sorgu, baglanti);
                    Parayatirma.ExecuteNonQuery();

                    string islemSorgu = "insert into islem (islem_tarihi, islem_miktari,islem_tipi,islem_aciklama,yeni_bakiye,islemhesap_id)" +
                        "values (@ptarih, @pMiktar, @pTip, @pislemAciklama, @pyeniBakiye, @pislemhesap_id)";

                    NpgsqlCommand islem = new NpgsqlCommand(islemSorgu, baglanti);
                    islem.Parameters.AddWithValue("@ptarih", currentTime);
                    islem.Parameters.AddWithValue("@pMiktar", Convert.ToInt16(txtParayatir.Text));
                    islem.Parameters.AddWithValue("@pTip", "Para Yatırma");
                    islem.Parameters.AddWithValue("@pislemAciklama", "Hesabınıza para gelmiştir");
                    islem.Parameters.AddWithValue("@pyeniBakiye", bakiye);
                    islem.Parameters.AddWithValue("@pislemhesap_id", hesapnumarası);
                    islem.ExecuteNonQuery();

                    baglanti.Close();

                    if (!this.IsDisposed)
                    {
                        this.Close();
                    }
                }
            }
            catch (Exception em)
            {
                MessageBox.Show("Exception caught " + em);
            }
        }
        private void paraYatirma_Load(object sender, EventArgs e)
        {
            comboAlıcıhesap.Items.Add(hesapnumarası);
        }
    }
}
