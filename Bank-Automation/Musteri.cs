using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankaOtomasyon
{
    public enum MusteriTipi
    {
        ticari,
        bireysel
    }
    public class Musteri
    {
        public string Sifre { get; set; }
        public int id { get; set; }
        public string ad { get; set; }
        public MusteriTipi tipi { get; set; }
        public double bankaBakiye{ get; set; }
        public Musteri()
        {
            
        }
    }
}
