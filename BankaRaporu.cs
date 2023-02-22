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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BankaOtomasyon
{
    public partial class BankaRaporu : Form
    {
        public BankaRaporu()
        {
            InitializeComponent();
        }

        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
          "Database=BankaOtomasyonu; user ID=postgres; password=1234");
        public int musteriid { get; set; }

        Musteri musteri = new Musteri();
        private void BankaRaporu_Load(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                string tabloSorgu = "select hesap_id, hesap_bakiye from hesap where musterihesap_id= '"+ musteriid +"'";
                NpgsqlDataAdapter islem1 = new NpgsqlDataAdapter(tabloSorgu, baglanti);
                DataTable dt = new DataTable();
                islem1.Fill(dt);
                dataGridBanka.DataSource = dt;
            
                string sayacSorgu = "select count(hesap_id) from hesap where musterihesap_id='" + musteriid + "'";
                NpgsqlDataAdapter sayislem = new NpgsqlDataAdapter(sayacSorgu, baglanti);
                DataTable dataTable = new DataTable();
                sayislem.Fill(dataTable);

                int comboCounter = Convert.ToInt16(dataTable.Rows[0][0]);

                for (int i = 0; i < comboCounter; i++)
                {
                    musteri.bankaBakiye += Convert.ToDouble(dt.Rows[i][1]);
                }
                baglanti.Close();

                labelToplamPara.Text = musteri.bankaBakiye.ToString() + "TL"; 
            }
            catch (Exception em)
            {
                MessageBox.Show("Hata: " + em);
            }
            baglanti.Close();
        }
    }
}
