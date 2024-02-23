using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bankApp
{
    public static class CekimIslemleriOnay
    {
        public static bool KontrolEt(string gelenKod, string olusturulanKod)
        {
            if(gelenKod == olusturulanKod)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Hatalı kod girişi!","HATA",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
        }

        public static string KodOlustur()
        {
            char[] karakterler = { 'a', 'B', 'j', 'q', 'L', 'm', 'M', 'p', 'u', 'i', 'z', 'x', 'b', 'F', 'g', 'G' };
            Random r = new Random();
            int s1 = r.Next(0, 10);
            int s2 = r.Next(0, 10);
            int s3 = r.Next(0, 10);

            int h1 = r.Next(0, karakterler.Length);
            int h2 = r.Next(0, karakterler.Length);
            int h3 = r.Next(0, karakterler.Length);

            char harf1, harf2, harf3;

            harf1 = karakterler[h1];
            harf2 = karakterler[h2];
            harf3 = karakterler[h3];

            string kod = s1.ToString() + s2.ToString() + s3.ToString() +
                harf1 + harf2 + harf3;

            return kod;
        }
    }
}
