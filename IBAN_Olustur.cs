using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bankApp
{
    public static class IBAN_Olustur
    {
        public static string YeniIban()
        {
            string iban = IBAN_Kontrol.KontrolEt();

            if (iban == "Mevcut")
            {   
                return YeniIban(); //İşlem başa dönecek.
            }
            else
            {
                return iban; //İşlem tamamlandı. Iban gönderiliyor.
            }
        }

        public static string IbanKoduOlustur()
        {
            Random r = new Random();

            int KontrolBasamagi = r.Next(10, 100);

            string BankaKodu = "0007";

            int Rezerv = r.Next(1000, 1010);

            int HesapNo1 = r.Next(1000, 2000);
            int HesapNo2 = r.Next(6000, 7000);
            int HesapNo3 = r.Next(8000, 9000);
            int HesapNo4 = r.Next(10, 100);

            return "TR" + KontrolBasamagi.ToString() + " " + BankaKodu + " " + Rezerv.ToString() + " " +
                HesapNo1.ToString() + " " + HesapNo2.ToString() + " " + HesapNo3.ToString() + " " + HesapNo4.ToString();
        }
    }
}
