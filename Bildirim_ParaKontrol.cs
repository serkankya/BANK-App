using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bankApp
{
    public static class Bildirim_ParaKontrol
    {
        public static bool ParaGeldiMi(int _ID)
        {
            SQL.Con.Close();
            SQL.Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Hesap_SonGiris FROM tbl_hesapBilgileri WHERE Hesap_ID = @ID", SQL.Con);
            cmd.Parameters.AddWithValue("@ID", _ID);

            DateTime sonGiris = DateTime.Now, suAn;
            suAn = DateTime.Now;

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                sonGiris = Convert.ToDateTime(dr["Hesap_SonGiris"]);
            }

            SQL.Con.Close();
            dr.Close();

            DateTime sonHareketSaati = DateTime.Now;

            SQL.Con.Open();
            SqlCommand cmd2 = new SqlCommand("SELECT Hareket_IslemTarihi FROM tbl_hesapHareketleri WHERE Hareket_AlanKullanici = @HesapID",SQL.Con);
            cmd2.Parameters.AddWithValue("@HesapID", _ID);

            SqlDataReader dr2 = cmd2.ExecuteReader();
            if(dr2.Read())
            {
                sonHareketSaati = Convert.ToDateTime(dr2["Hareket_IslemTarihi"]);
            }

            SQL.Con.Close();
            dr2.Close();

            if (sonGiris < suAn && sonHareketSaati > sonGiris)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
