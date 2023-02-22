using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankaOtomasyon
{
    public class Islem : Hesap
    {
        public double miktar { get; set; }
        public DateTime tarih { get; set; }
        public string islemTipi{ get; set; }
        public string islemAciklama { get; set; }
        public double yeniBakiye { get; set; }
        public Islem()
        {
        }
    }
}
