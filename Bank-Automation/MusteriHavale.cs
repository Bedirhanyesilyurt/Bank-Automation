using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BankaOtomasyon
{
    public partial class MusteriHavale : Form
    {
        public MusteriHavale()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
         "Database=BankaOtomasyonu; user ID=postgres; password=1234");
        Musteri musteri = new Musteri();

        Islem islemc = new Islem();  
        public int musterId { get; set; }
        public int hesapNumarası { get; set; }
        public double bakiye { get; set; }
        public void MusteriHavale_Load(object sender, EventArgs e)
        {
            try
            {
                
                baglanti.Open();
                comboGonderen.Items.Add(hesapNumarası);

                string Sorgu = "select hesap_id from hesap";
                NpgsqlDataAdapter islem = new NpgsqlDataAdapter(Sorgu, baglanti);
                DataTable dt = new DataTable();
                islem.Fill(dt);

                string sayacSorgu = "select count(hesap_id) from hesap";
                NpgsqlDataAdapter sayislem = new NpgsqlDataAdapter(sayacSorgu, baglanti);
                DataTable dataTable = new DataTable();
                sayislem.Fill(dataTable);

                int comboCounter = Convert.ToInt16(dataTable.Rows[0][0]);

                for (int i = 0; i < comboCounter; i++)
                {
                    if (Convert.ToInt16(dt.Rows[i][0]) == hesapNumarası)
                    {
                        continue;
                    }
                    comboAlıcı.Items.Add(dt.Rows[i][0].ToString());
                }
                baglanti.Close();
            }
            catch (Exception em)
            {
                MessageBox.Show("Exception caught " + em);
                baglanti.Close();
            }
        }
        public void button1_Click(object sender, EventArgs e)
        {
            double gonderenBakiye = this.bakiye; 
            DateTime currentTime = DateTime.Now;
            double havaleUcret = 0;
            islemc.miktar = Convert.ToDouble(txtTutar.Text);
            try
            {
                DialogResult result = MessageBox.Show("Hesabınızdan havale işlemi yapılacaktır onaylıyor musunuz ", "Havale İşlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    baglanti.Open();
                    //havale eden hesaptan para eksilme işlemleri
                    string tipSorgusu = "select musteri_tipi from musteri where musteri_id = (select musterihesap_id from hesap where hesap_id = '" + Convert.ToInt16(comboGonderen.Text) + "')";
                    NpgsqlDataAdapter tipkomutu = new NpgsqlDataAdapter(tipSorgusu, baglanti);
                    DataTable dataTipTable = new DataTable();
                    tipkomutu.Fill(dataTipTable);

                    string musteriTip = Convert.ToString(dataTipTable.Rows[0][0]);

                    //musteri.tipi = (MusteriTipi)(dataTipTable.Rows[0][0]);

                    //bireysel hesaptan kesinti yapılan yer
                    if (musteriTip == "bireysel")
                    {
                        double tutucu = Convert.ToDouble(txtTutar.Text);
                        havaleUcret = (2 * islemc.miktar) / 100;
                        tutucu += havaleUcret;
                        //txtTutar.Text = tutucu.ToString();

                        gonderenBakiye -= tutucu;

                        string sorgu = "update hesap set hesap_bakiye ='" + gonderenBakiye + "' where hesap_id ='" + hesapNumarası + "'";
                        NpgsqlCommand Havale = new NpgsqlCommand(sorgu, baglanti);
                        Havale.ExecuteNonQuery();

                        //işlem geçmişi için
                        string islemSorgu = "insert into islem (islem_tarihi, islem_miktari, islem_tipi, islem_aciklama, yeni_bakiye, islemhesap_id)" +
                            "values (@ptarih, @pMiktar, @pTip, @pislemAciklama, @pyeniBakiye, @pislemhesap_id)";
                        //double aliciBakiyeTutucu = Convert.ToInt16(txtTutar.Text) - havaleUcret;

                        NpgsqlCommand islem = new NpgsqlCommand(islemSorgu, baglanti);
                        islem.Parameters.AddWithValue("@ptarih", currentTime);
                        islem.Parameters.AddWithValue("@pMiktar", tutucu);
                        islem.Parameters.AddWithValue("@pTip", "Havale");
                        islem.Parameters.AddWithValue("@pislemAciklama", "Giden havale İşlemi");
                        islem.Parameters.AddWithValue("@pyeniBakiye", gonderenBakiye);
                        islem.Parameters.AddWithValue("@pislemhesap_id", hesapNumarası);
                        islem.ExecuteNonQuery();
                    }
                    
                    //Diğer hesaba havale etme işlemleri
                    string aliciSorgu = "select hesap_bakiye from hesap where hesap_id = '" + Convert.ToInt16(comboAlıcı.Text) + "'";
                    NpgsqlDataAdapter aliciCom = new NpgsqlDataAdapter(aliciSorgu, baglanti);
                    DataTable dataTable = new DataTable();
                    aliciCom.Fill(dataTable);

                    int aliciBakiye = Convert.ToInt16(dataTable.Rows[0][0]);

                    int aliciTutar = Convert.ToInt16(txtTutar.Text);
                    aliciBakiye += aliciTutar;

                    string aliciSorgu2 = "update hesap set hesap_bakiye ='" + aliciBakiye + "' where hesap_id ='" + Convert.ToInt16(comboAlıcı.Text) + "'";
                    NpgsqlCommand aliciCom2 = new NpgsqlCommand(aliciSorgu2, baglanti);
                    aliciCom2.ExecuteNonQuery();

                    string islemSorgu2 = "insert into islem (islem_tarihi, islem_miktari, islem_tipi, islem_aciklama, yeni_bakiye, islemhesap_id)" +
                            "values (@ptarih, @pMiktar, @pTip, @pislemAciklama, @pyeniBakiye, @pislemhesap_id)";
                    //işlem geçimişi 
                    NpgsqlCommand islem2 = new NpgsqlCommand(islemSorgu2, baglanti);
                    islem2.Parameters.AddWithValue("@ptarih", currentTime);
                    islem2.Parameters.AddWithValue("@pMiktar", Convert.ToInt16(txtTutar.Text));
                    islem2.Parameters.AddWithValue("@pTip", "Havale");
                    islem2.Parameters.AddWithValue("@pislemAciklama", "Gelen havale İşlemi");
                    islem2.Parameters.AddWithValue("@pyeniBakiye", aliciBakiye);
                    islem2.Parameters.AddWithValue("@pislemhesap_id", Convert.ToInt16(comboAlıcı.Text));
                    islem2.ExecuteNonQuery();

                    baglanti.Close();
                    this.Close();
                }
            }
            catch (Exception em)
            {
                MessageBox.Show("Exception caught " + em);
                baglanti.Close();
            }
        }
    }
}
