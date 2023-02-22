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
    public partial class müsteriHesapRapor : Form
    {
        public müsteriHesapRapor()
        {
            InitializeComponent();
        }
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
          "Database=BankaOtomasyonu; user ID=postgres; password=1234");
        public int musteriid { get; set; }
        public DateTime currentTime { get; set; }
        private void müsteriHesapRapor_Load(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                currentTime = DateTime.Now;
                txtBaslangıc.Text = "01.01.2023";
                txtBitis.Text = currentTime.ToString();
                string Sorgu = "select hesap_id from hesap where musterihesap_id='" + musteriid + "' ";
                NpgsqlDataAdapter islem = new NpgsqlDataAdapter(Sorgu, baglanti);
                DataTable dt = new DataTable();
                islem.Fill(dt);

                string sayacSorgu = "select count(hesap_id) from hesap where musterihesap_id='" + musteriid + "'";
                NpgsqlDataAdapter sayislem = new NpgsqlDataAdapter(sayacSorgu, baglanti);
                DataTable dataTable = new DataTable();
                sayislem.Fill(dataTable);

                int comboCounter = Convert.ToInt16(dataTable.Rows[0][0]);

                for (int i = 0; i < comboCounter; i++)
                {
                    comboBox1.Items.Add(dt.Rows[i][0].ToString());
                }
                baglanti.Close();
            }
            catch(Exception em)
            {
                MessageBox.Show("Hata: " + em);
            }
        }

        private void btnhesapGecmis_Click(object sender, EventArgs e)
        {
            try
            {
                baglanti.Open();
                string Sorgu = "select * from islem where islemhesap_id='" + Convert.ToInt16(comboBox1.Text) + "' and islem_tarihi >='" +txtBaslangıc.Text + "' and islem_tarihi <= '" + txtBitis.Text + "' order by islem_tarihi asc ";
                NpgsqlDataAdapter islem = new NpgsqlDataAdapter(Sorgu, baglanti);
                DataSet ds = new DataSet();
                islem.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                baglanti.Close();
            }
            catch (Exception em)
            {
                MessageBox.Show("Hata: " + em);
            }
        }
    }
}
