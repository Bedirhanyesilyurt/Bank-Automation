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
    public partial class HesapGecis : Form,BankaServisi
    {
        public HesapGecis()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
          "Database=BankaOtomasyonu; user ID=postgres; password=1234");

        Hesap hesap = new Hesap();
        public int hesapid { get; set; }
        public int musteriHesapid { get; set; }
        public double bakiye { get; set; }
        public int randomSayi { get; set; }
        public int idKontrol { get; set; }
        public int musteriid{ get; set; }

        public void Listele()
        {
            string sorgu = "select * from hesap where musterihesap_id='" +musteriid + "'";
            NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter(sorgu, baglanti);
            DataSet ds = new DataSet();
            npgsqlDataAdapter.Fill(ds);
            dtgridHesap.DataSource = ds.Tables[0];
        }

        private void HesapGecis_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void btnhesabagec_Click(object sender, EventArgs e)
        {
            try
            {
                //hesap oluşturma işlemleri
                baglanti.Open();
                NpgsqlCommand hesapEkle = new NpgsqlCommand("insert into hesap (hesap_id, hesap_bakiye, musterihesap_id)" +
                    " values (@pid, @pBakiye,@pHid)", baglanti);

                Random rand = new Random();
                randomSayi = rand.Next(1, 1000);

                double bakiye = 0;

                hesapEkle.Parameters.AddWithValue("@pid", randomSayi);
                hesapEkle.Parameters.AddWithValue("@pBakiye", bakiye);
                hesapEkle.Parameters.AddWithValue("@pHid", musteriid);
                hesapEkle.ExecuteNonQuery();
                baglanti.Close();
            }
            catch (Exception em)
            {
                MessageBox.Show("Exception caught " + em);
            }
        }
        private void btnGec_Click(object sender, EventArgs e)
        {
            try
            {
                //Hesaba ait bilgilerin anasayfa formuna aktarılması
                int hesapid = Convert.ToInt32(dtgridHesap.CurrentRow.Cells[0].Value);
                double bakiye = Convert.ToDouble(dtgridHesap.CurrentRow.Cells[1].Value);
                int musteriHesapid = Convert.ToInt32(dtgridHesap.CurrentRow.Cells[2].Value);

                Anasayfa anasayfa = new Anasayfa();
                anasayfa.musterIdkontrol = this.musteriid;
                anasayfa.Anahesap = this.hesap;
                anasayfa.hesapnumarası = hesapid;
                anasayfa.Show();
            }
            catch (Exception em)
            {
                MessageBox.Show("Exception caught " + em);
            }
        }
        private void btnListele_Click(object sender, EventArgs e)
        {
            Listele();
        }
    }
}
