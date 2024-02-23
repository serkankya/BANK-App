using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bankApp
{
    public static class HesapNo_Olustur
    {
        public static string YeniHesapNo()
        {
            string hesapNo = HesapNo_Kontrol.KontrolEt();

            if(hesapNo == "Mevcut")
            {
                MessageBox.Show("HATA");
                return YeniHesapNo(); //İşlem başa dönecek.
            }
            else
            {
                return hesapNo; //İşlem tamamlandı. Iban gönderiliyor.
            }
        }

        public static string HesapNoOlustur()
        {
            Random r = new Random();

            int HesapNo1 = r.Next(1000, 5000);

            string BankaKodu = "0007";

            int hesapNo2 = r.Next(55555, 75556);

            return HesapNo1.ToString() + "-" + BankaKodu + "-" + hesapNo2.ToString();
        }
    }
}
