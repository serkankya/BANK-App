using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bankApp
{
    public static class HesapNo_Kontrol
    {
        public static string KontrolEt()
        {
            string HesapNo = HesapNo_Olustur.HesapNoOlustur();

            SQL.Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Hesap_No FROM tbl_hesapBilgileri WHERE Hesap_Durum = @Durum AND Hesap_NO = @NO", SQL.Con);
            cmd.Parameters.AddWithValue("@Durum", true);
            cmd.Parameters.AddWithValue("@NO", HesapNo);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                dr.Close();
                SQL.Con.Close();
                return "Mevcut";
            }
            else
            {
                dr.Close();
                SQL.Con.Close();
                return HesapNo;
            }
        }
    }
}
