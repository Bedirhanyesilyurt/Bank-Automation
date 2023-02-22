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
    public partial class MusteriHesapKapama : Form
    {
        public MusteriHesapKapama()
        {
            InitializeComponent();
        }
        
        NpgsqlConnection baglanti = new NpgsqlConnection("server=localHost; port=5432;" +
          "Database=BankaOtomasyonu; user ID=postgres; password=1234");
        public int hesapNumarası { get; set; }
        public int musterId { get; set; }
        public static bool hesapKapalı { get; set; }
        

        private void MusteriHesapKapama_Load(object sender, EventArgs e)
        {
            comboSilme.Items.Add(hesapNumarası);
        }

        private void btnHesapKapama_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Hesabınıza Silme işlemi yapılacaktır onaylıyor musunuz ", "Hesap Silme İşlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    baglanti.Open();

                    //Önce işlem tablosundan sonra hesap tablosundan siliniyor.
                    string Sorgu1 = "delete from islem where islemhesap_id= '" + Convert.ToInt16(comboSilme.Text) + "'";
                    string Sorgu2 = "delete from hesap where hesap_id= '" + Convert.ToInt16(comboSilme.Text) + "'";

                    NpgsqlCommand islem = new NpgsqlCommand(Sorgu1, baglanti);
                    islem.ExecuteNonQuery();
                    NpgsqlCommand islem2 = new NpgsqlCommand(Sorgu2, baglanti);
                    islem2.ExecuteNonQuery();
                    
                    baglanti.Close();
                    this.Close();

                    /*foreach (Form form in Application.OpenForms)
                    {
                        if (form.Name == "Anasayfa")
                        {
                            form.Close();
                        }
                    }*/
                    hesapKapalı = true;
                }
            }
            catch (Exception em)
            {
                MessageBox.Show("Exception caught " + em);
            }
        }
    }
}
