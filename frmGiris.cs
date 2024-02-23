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
    public partial class frmGiris : Form
    {
        public frmGiris()
        {
            InitializeComponent();
        }

        private void btnGiris_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTCKimlik.Text) && !string.IsNullOrWhiteSpace(txtSifre.Text))
            {
                string ad, soyad, ID;

                SQL.Con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Hesap_ID,Hesap_KisiAd,Hesap_KisiSoyad,Hesap_TCKimlik,Hesap_Sifre FROM tbl_HesapBilgileri WHERE Hesap_Durum = @Durum AND Hesap_TCKimlik = @TcKimlik AND Hesap_Sifre = @Sifre", SQL.Con);
                cmd.Parameters.AddWithValue("@Durum", true);
                cmd.Parameters.AddWithValue("@TcKimlik", txtTCKimlik.Text);
                cmd.Parameters.AddWithValue("@Sifre", txtSifre.Text);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    ad = dr["Hesap_KisiAd"].ToString();
                    soyad = dr["Hesap_KisiSoyad"].ToString();
                    ID = dr["Hesap_ID"].ToString();
                    MessageBox.Show("Hoş geldiniz, sayın " + ad + " " + soyad);

                    dr.Close();
                    SQL.Con.Close();

                    bool ParaGeldiMi = Bildirim_ParaKontrol.ParaGeldiMi(Convert.ToInt32(ID));

                    SQL.Con.Open();
                    SqlCommand cmd2 = new SqlCommand("UPDATE tbl_hesapBilgileri SET Hesap_SonGiris = @sonGiris WHERE Hesap_ID = @HesapID", SQL.Con);
                    cmd2.Parameters.AddWithValue("@sonGiris", DateTime.Now);
                    cmd2.Parameters.AddWithValue("@HesapID", ID);
                    cmd2.ExecuteNonQuery();

                    SQL.Con.Close();

                    frmAnaSayfa main = new frmAnaSayfa(Convert.ToInt32(ID), ParaGeldiMi);
                    main.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Hatalı kullanıcı adı veya şifre.");
                    dr.Close();
                    SQL.Con.Close();
                }
            }
            else
            {
                MessageBox.Show("HATA");
            }
        }

        private void btnKayitOl_Click(object sender, EventArgs e)
        {
            frmHesapOlustur register = new frmHesapOlustur();
            register.Show();
            this.Hide();
        }
    }
}
