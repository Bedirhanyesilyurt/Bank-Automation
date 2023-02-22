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
    public partial class Anasayfa : Form
    {
        public Anasayfa()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
          "Database=BankaOtomasyonu; user ID=postgres; password=1234");

        public Hesap Anahesap { get; set; }
        public int hesapnumarası { get; set; }
        public int musterIdkontrol { get; set; }
        public double anaSayfabakiye { get; set; }

        private void hesapKapaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MusteriHesapKapama musteriHesapKapa = new MusteriHesapKapama();
            musteriHesapKapa.MdiParent = this;

            musteriHesapKapa.hesapNumarası = this.hesapnumarası;
            musteriHesapKapa.musterId = this.musterIdkontrol;
            musteriHesapKapa.Show();
        }

        private void hesapÖzetiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            müsteriHesapRapor musteriHesaprapor = new müsteriHesapRapor();
            musteriHesaprapor.MdiParent = this;
            musteriHesaprapor.musteriid = this.musterIdkontrol;
            musteriHesaprapor.Show();
        }

        private void hesabaHavaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MusteriHavale musteriHavale = new MusteriHavale();
            musteriHavale.MdiParent = this;
            musteriHavale.hesapNumarası = this.hesapnumarası;
            musteriHavale.bakiye = this.anaSayfabakiye;
            musteriHavale.musterId = this.musterIdkontrol;
            musteriHavale.Show();
        }

        private void hesabaParaYatırmaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParaYatirma paraYatırma = new ParaYatirma();
            paraYatırma.MdiParent = this;
            paraYatırma.hesapnumarası = this.hesapnumarası;
            paraYatırma.bakiye = this.anaSayfabakiye;
            paraYatırma.Show();
        }

        public void paraÇekmeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ParaCekme müsteriParaCekme = new ParaCekme();
            müsteriParaCekme.MdiParent = this;
            müsteriParaCekme.hesapnumarası = this.hesapnumarası;
            müsteriParaCekme.bakiye = this.anaSayfabakiye;
            müsteriParaCekme.Show();
        }

        private void transferGeçmişiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            müsteriHesapRapor musteriHesaprapor = new müsteriHesapRapor();
            musteriHesaprapor.MdiParent = this;
            musteriHesaprapor.musteriid = this.musterIdkontrol;
            musteriHesaprapor.Show();
        }
        private void bankaRaporToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BankaRaporu bankaRaporu = new BankaRaporu();
            bankaRaporu.MdiParent = this;
            bankaRaporu.musteriid = this.musterIdkontrol;
            bankaRaporu.Show();
        }

        public void musteriAnasayfa_Load(object sender, EventArgs e)
        {
            try
            {
                //Sayfa yüklendiğinde hesap bilgilerini labellara yazma işlemi
                string sorgu = "select hesap_id, hesap_bakiye from hesap where hesap_id ='" + hesapnumarası + "'";

                NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                npgsqlDataAdapter.Fill(dt);

                hesapnumarası = Convert.ToInt32(dt.Rows[0][0]);
                anaSayfabakiye = Convert.ToInt32(dt.Rows[0][1]);

                labehesapNumarası1.Text = hesapnumarası.ToString();
                labelBakiye1.Text = anaSayfabakiye.ToString();
            }
            catch (Exception em)
            {
                MessageBox.Show("Exception caught " + em);
            }
        }
        public void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //Anasayfadaki labellara yazdırma işlemleri
                string sorgu = "select hesap_id, hesap_bakiye from hesap where hesap_id ='" + hesapnumarası + "'";

                NpgsqlDataAdapter npgsqlDataAdapter = new NpgsqlDataAdapter(sorgu, baglanti);
                DataTable dt = new DataTable();
                npgsqlDataAdapter.Fill(dt);

                hesapnumarası = Convert.ToInt32(dt.Rows[0][0]);
                anaSayfabakiye = Convert.ToInt32(dt.Rows[0][1]);

                labehesapNumarası1.Text = hesapnumarası.ToString();
                labelBakiye1.Text = anaSayfabakiye.ToString();

                if (Anahesap.hesapKapalı == true)
                {
                    this.Close();
                }
                this.Refresh();
            }
            catch (Exception em)
            {
                MessageBox.Show("Exception caught " + em);
            }
        }
        private void btnAnasayfaHesapgec_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnCıkıs_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }  
}
