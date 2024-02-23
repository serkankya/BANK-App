using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bankApp
{
    public partial class frmHesapHareketleri : Form
    {
        int KullaniciID;
        public frmHesapHareketleri(int KullaniciID_)
        {
            InitializeComponent();
            KullaniciID = KullaniciID_;
        }

        private void frmHesapHareketleri_Load(object sender, EventArgs e)
        {
            hareketleriListele();
        }

        string Islem;
        int IslemiYapanKullanici, IslemiAlanKullanici, IslemiGonderenKullanici;
        double IslemTutar;
        DateTime IslemTarih;
        string AdSoyad, AlanKisi;
        void hareketleriListele()
        {
            lbHareketler.Items.Clear();
            SQL.Con.Close();
            SQL.Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_hesapHareketleri WHERE Hareket_IslemiYapanKullanici = @Kullanici OR Hareket_AlanKullanici = @AlanKullanici", SQL.Con);
            cmd.Parameters.AddWithValue("@Kullanici", KullaniciID);
            cmd.Parameters.AddWithValue("@AlanKullanici", KullaniciID);

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                object objIslemiYapanKullanici = dr["Hareket_IslemiYapanKullanici"];
                object objIslemiAlanKullanici = dr["Hareket_AlanKullanici"];
                object objIslemiGonderenKullanici = dr["Hareket_GonderenKullanici"];

                Islem = dr["Hareket_YapilanIslem"].ToString();
                IslemTutar = Convert.ToDouble(dr["Hareket_IslemTutari"]);
                IslemTarih = Convert.ToDateTime(dr["Hareket_IslemTarihi"]);

                if (objIslemiYapanKullanici != DBNull.Value)
                    IslemiYapanKullanici = Convert.ToInt32(objIslemiYapanKullanici);
                else
                    IslemiYapanKullanici = 0;

                if (objIslemiAlanKullanici != DBNull.Value)
                    IslemiAlanKullanici = Convert.ToInt32(objIslemiAlanKullanici);
                else
                    IslemiAlanKullanici = 0;

                if (objIslemiGonderenKullanici != DBNull.Value)
                    IslemiGonderenKullanici = Convert.ToInt32(objIslemiGonderenKullanici);
                else
                    IslemiGonderenKullanici = 0;

                //İşlemi yapan kişinin adını soyadını almak için yapılan INNERJoin
                SqlCommand komut = new SqlCommand("SELECT Hesap_KisiAd + ' ' + Hesap_KisiSoyad AS IslemSahibi FROM tbl_hesapBilgileri INNER JOIN tbl_hesapHareketleri ON Hesap_ID = Hareket_IslemiYapanKullanici", SQL.Con);

                SqlDataReader dr2 = komut.ExecuteReader();

                while (dr2.Read())
                {
                    AdSoyad = dr2["IslemSahibi"].ToString();
                }
                dr2.Close();
                //Yapan Kişi - Son



                //İşlemi alan kişinin adını soyadını almak için yapılan INNERJoin
                SqlCommand komut2 = new SqlCommand("SELECT Hesap_KisiAd + ' ' + Hesap_KisiSoyad AS AlanKisi FROM tbl_hesapBilgileri INNER JOIN tbl_hesapHareketleri ON Hesap_ID = Hareket_AlanKullanici",SQL.Con);

                SqlDataReader dr3 = komut2.ExecuteReader();

                while (dr3.Read())
                {
                    AlanKisi = dr3["AlanKisi"].ToString();
                }

                dr3.Close();
                //Alan Kişi - Son

                if (IslemiAlanKullanici == 0)
                    lbHareketler.Items.Add("Yapan : " + AdSoyad + " / " + Islem + " / " + IslemTutar + " TL / " + IslemTarih);
                else if (IslemiGonderenKullanici == 0 && IslemiAlanKullanici == 0)
                    lbHareketler.Items.Add("Yapan : " + AdSoyad + " / " + Islem + " / " + IslemTutar + " TL / " + IslemTarih);
                else
                    lbHareketler.Items.Add("Yapan : " + AdSoyad + " / " + "Alan : " + AlanKisi + " / " + Islem + " / " + IslemTutar + " TL / " + IslemTarih);
            }

            dr.Close();
            SQL.Con.Close();        
        }
    }
}
