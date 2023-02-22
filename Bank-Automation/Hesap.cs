using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BankaOtomasyon
{
    public class Hesap
    {
        public Hesap()
        {
        }
        public int hesapNumarası { get; set; }
        public double bakiye { get; set; }
        public bool hesapKapalı { get; set; }

    }
}
