using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bankApp
{
    public partial class frmParaTransfer : Form
    {
        int gonderenKullaniciID;
        public frmParaTransfer(int gonderenKullaniciID_)
        {
            InitializeComponent();
            gonderenKullaniciID = gonderenKullaniciID_;
        }

        //Gerekli değişken atamaları
        double gonderileceKTutar, mevcutTutar, olacakTutar, gonderenKisiMevcutBakiye, gonderenKisiYeniBakiye;
        string gonderilecekIBAN, gonderilecekHesapID, gonderilecekKisiAd, gonderilecekKisiSoyad, gonderilecekKisiAdSoyad;

        //Onayla butonu işlemleri
        private void btnTransferOnayla_Click(object sender, EventArgs e)
        {
            gonderileceKTutar = Convert.ToDouble(numTutar.Value);
            gonderilecekIBAN = txtIBAN.Text;
            gonderilecekKisiAd = txtGonderilecekKisiAd.Text;
            gonderilecekKisiSoyad = txtGonderilecekKisiSoyad.Text;

            DialogResult onay = MessageBox.Show($"{gonderilecekKisiAd + " " + gonderilecekKisiSoyad} adlı kişiye {gonderileceKTutar.ToString()} TL göndermek istediğinize emin misiniz ?", "Para Transferi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (onay == DialogResult.Yes)
            {
                SQL.Con.Close();
                SQL.Con.Open();

                //Öncelikle IBAN ve Kullanıcı mevcut mu kontrolü yapılıyor
                SqlCommand cmd2 = new SqlCommand("SELECT Hesap_ID,Hesap_KisiAd,Hesap_KisiSoyad,Hesap_MevcutBakiye FROM tbl_hesapBilgileri WHERE Hesap_IBAN = @IBANKontrol AND Hesap_KisiAd = @AdKontrol AND Hesap_KisiSoyad = @SoyadKontrol", SQL.Con);
                cmd2.Parameters.AddWithValue("@IBANKontrol", gonderilecekIBAN);
                cmd2.Parameters.AddWithValue("@AdKontrol", gonderilecekKisiAd);
                cmd2.Parameters.AddWithValue("@SoyadKontrol", gonderilecekKisiSoyad);

                SqlDataReader dr = cmd2.ExecuteReader();
                if (dr.Read())
                {
                    gonderilecekHesapID = dr["Hesap_ID"].ToString();
                    gonderilecekKisiAdSoyad = dr["Hesap_KisiAd"] + " " + dr["Hesap_KisiSoyad"];
                    mevcutTutar = Convert.ToDouble(dr["Hesap_MevcutBakiye"]);

                    //Iban ve Kişi mevcut ise olacak işlemler bundan sonra tamamlanıyor
                    olacakTutar = mevcutTutar + gonderileceKTutar;

                    SqlCommand cmd = new SqlCommand("UPDATE tbl_hesapBilgileri SET Hesap_MevcutBakiye = @Tutar WHERE Hesap_IBAN = @IBAN", SQL.Con);
                    cmd.Parameters.AddWithValue("@Tutar", olacakTutar);
                    cmd.Parameters.AddWithValue("@IBAN", gonderilecekIBAN);
                    cmd.ExecuteNonQuery();
                    SQL.Con.Close();

                    gonderenKisiTutarEksilt(); //Gönderen kişinin bakiyesi düşmesi için olan metot

                    MessageBox.Show($"{gonderilecekKisiAd + " " + gonderilecekKisiSoyad} adlı kişiye {gonderileceKTutar} miktarında para transferi gerçekleştirilmiştir.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    hesapHareketiEkle();

                    frmAnaSayfa main = new frmAnaSayfa(gonderenKullaniciID, false);
                    main.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("böyle bir IBAN veya kullanıcı bulunamadı.");
                }

                dr.Close();
                SQL.Con.Close();

            }
            else
            {
                MessageBox.Show("Para transferi işlemi iptal edilmiştir!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        void hesapHareketiEkle()
        {
            SQL.Con.Close();
            SQL.Con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO tbl_hesapHareketleri (Hareket_AlanKullanici,Hareket_GonderenKullanici,Hareket_IslemiYapanKullanici,Hareket_YapilanIslem,Hareket_IslemTutari) VALUES (@AlanKullanici,@GonderenKullanici,@IslemiYapanKullanici,@YapilanIslem,@Tutar)", SQL.Con);
            cmd.Parameters.AddWithValue("@AlanKullanici", Convert.ToInt32(gonderilecekHesapID));
            cmd.Parameters.AddWithValue("@GonderenKullanici", gonderenKullaniciID);
            cmd.Parameters.AddWithValue("@IslemiYapanKullanici", gonderenKullaniciID);
            cmd.Parameters.AddWithValue("@YapilanIslem", "Para Transferi");
            cmd.Parameters.AddWithValue("@Tutar", gonderileceKTutar);
            cmd.ExecuteNonQuery();
            SQL.Con.Close();
        }

        void gonderenKisiTutarEksilt()
        {
            SQL.Con.Close();
            SQL.Con.Open();
            //Öncelikle kullanıcının bakiyesi alınıyor
            SqlCommand cmd = new SqlCommand("SELECT Hesap_MevcutBakiye FROM tbl_hesapBilgileri WHERE Hesap_ID = @ID", SQL.Con);
            cmd.Parameters.AddWithValue("@ID", gonderenKullaniciID);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                gonderenKisiMevcutBakiye = Convert.ToDouble(dr["Hesap_MevcutBakiye"]);
            }
            dr.Close();

            gonderenKisiYeniBakiye = gonderenKisiMevcutBakiye - gonderileceKTutar;

            //Azaltılmış bakiye şeklinde ayarlanıyor.
            SqlCommand cmd2 = new SqlCommand("UPDATE tbl_hesapBilgileri SET Hesap_MevcutBakiye = @YeniBakiye WHERE Hesap_ID = @ID_", SQL.Con);
            cmd2.Parameters.AddWithValue("@ID_", gonderenKullaniciID);
            cmd2.Parameters.AddWithValue("@YeniBakiye", gonderenKisiYeniBakiye);
            cmd2.ExecuteNonQuery();

            SQL.Con.Close();
        }
    }
}
