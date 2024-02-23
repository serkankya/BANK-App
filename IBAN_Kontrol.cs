using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bankApp
{
    public static class IBAN_Kontrol
    {
        public static string KontrolEt()
        {
            string IBAN = IBAN_Olustur.IbanKoduOlustur();

            SQL.Con.Open(); 
            SqlCommand cmd = new SqlCommand("SELECT Hesap_IBAN FROM tbl_hesapBilgileri WHERE Hesap_Durum = @Durum AND Hesap_IBAN = @Iban", SQL.Con);
            cmd.Parameters.AddWithValue("@Durum", true);
            cmd.Parameters.AddWithValue("@Iban", IBAN);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                dr.Close();
                SQL.Con.Close();
                return "Mevcut"; //Aynı IBAN mevcut olduğu için yenisi oluşturulacak.
            }
            else
            {
                dr.Close();
                SQL.Con.Close();
                return IBAN; //Aynı IBAN yok, devam edecek işlem.
            }          
        }
    }
}
