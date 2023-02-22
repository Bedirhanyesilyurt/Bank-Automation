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
    public partial class ParaCekme : Form
    {
        public ParaCekme()
        {
            InitializeComponent();
        }
        public int hesapnumarası { get; set; }
        public double bakiye { get; set; }
        public Islem islem { get; set; }


        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
             "Database=BankaOtomasyonu; user ID=postgres; password=1234");
        public void button1_Click(object sender, EventArgs e)
        {
            DateTime currentTime = DateTime.Now;
            try
            {
                DialogResult result = MessageBox.Show("Hesabınıza Para çekme işlemi yapılacaktır onaylıyor musunuz ", "Para Çekme İşlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (Convert.ToInt16(txtParaCekme.Text) >= 750)
                    {
                        MessageBox.Show("Günlük maksimum para çekme limiti 750 Tl'dir");
                    }
                    else
                    {
                        bakiye -= Convert.ToInt16(txtParaCekme.Text);

                        baglanti.Open();
                        string sorgu = "update hesap set hesap_bakiye ='" + bakiye + "' where hesap_id ='" + hesapnumarası + "'";
                        NpgsqlCommand Parayatirma = new NpgsqlCommand(sorgu, baglanti);
                        Parayatirma.ExecuteNonQuery();

                        string islemSorgu = "insert into islem (islem_tarihi, islem_miktari,islem_tipi,islem_aciklama,yeni_bakiye,islemhesap_id)" +
                            "values (@ptarih, @pMiktar, @pTip, @pislemAciklama, @pyeniBakiye, @pislemhesap_id)";

                        NpgsqlCommand islem = new NpgsqlCommand(islemSorgu, baglanti);
                        islem.Parameters.AddWithValue("@ptarih", currentTime);
                        islem.Parameters.AddWithValue("@pMiktar", Convert.ToInt32(txtParaCekme.Text));
                        islem.Parameters.AddWithValue("@pTip", "Para Çekme");
                        islem.Parameters.AddWithValue("@pislemAciklama", "Hesabınızdan para çekilmiştir");
                        islem.Parameters.AddWithValue("@pyeniBakiye", bakiye);
                        islem.Parameters.AddWithValue("@pislemhesap_id", hesapnumarası);
                        islem.ExecuteNonQuery();

                        this.Close();
                    }
                }
            }
            catch (Exception em)
            {
                MessageBox.Show("Exception caught " + em);
            }
            this.Close();
        }
        private void musteriParaCekme_Load(object sender, EventArgs e)
        {
            comboCekilenhesap.Items.Add(hesapnumarası);
        }
    }
}

