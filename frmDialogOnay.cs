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
    public partial class frmDialogOnay : Form
    {
        string yapilanIslem, hesapNO;
        double _tutar, _mevcut;
        int kullaniciID;
        public frmDialogOnay(string yapilacakIslem, double IslemTutari, double mevcutTutar, int ID, string hesapNO_)
        {
            InitializeComponent();
            yapilanIslem = yapilacakIslem;
            _tutar = IslemTutari;
            _mevcut = mevcutTutar;
            kullaniciID = ID;
            hesapNO = hesapNO_;
            this.Text = yapilanIslem;
        }

        private void frmDialogOnay_Load(object sender, EventArgs e)
        {
            string OnayKodu = CekimIslemleriOnay.KodOlustur();
            lblKod.Text = OnayKodu;
        }

        private void btnKontrolEt_Click(object sender, EventArgs e)
        {
            string girilenKod = txtKod.Text;
            string onayKodu = lblKod.Text;
            bool kontrol = CekimIslemleriOnay.KontrolEt(girilenKod, onayKodu);
            if (kontrol)
            {
                if (this.Text == "Para Yükleme")
                {
                    _mevcut += _tutar;
                }
                else if (this.Text == "Para Çekme")
                {
                    _mevcut -= _tutar;
                }
                MessageBox.Show($"Kodunuz onaylandı. {yapilanIslem} işleminiz tamamlanıyor.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SQL.Con.Close();
                SQL.Con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE tbl_hesapBilgileri SET Hesap_MevcutBakiye = @yeniTutar WHERE Hesap_ID = @id", SQL.Con);
                cmd.Parameters.AddWithValue("@yeniTutar", _mevcut);
                cmd.Parameters.AddWithValue("@id", kullaniciID);
                cmd.ExecuteNonQuery();

                //Hesap hareketlerine ekleme işlemi
                SqlCommand cmd2 = new SqlCommand("INSERT INTO tbl_hesapHareketleri (Hareket_IslemiYapanKullanici,Hareket_YapilanIslem,Hareket_IslemYapilanHesapNO,Hareket_IslemTutari) VALUES (@Kullanici,@Islem,@HesapNO,@Tutar)", SQL.Con);
                cmd2.Parameters.AddWithValue("@Kullanici", kullaniciID);
                cmd2.Parameters.AddWithValue("@Islem", yapilanIslem);
                cmd2.Parameters.AddWithValue("@HesapNO", hesapNO);
                cmd2.Parameters.AddWithValue("@Tutar", _tutar);
                cmd2.ExecuteNonQuery();

                SQL.Con.Close();
                frmAnaSayfa main = new frmAnaSayfa(kullaniciID,false);
                main.Show();
                this.Hide();
            }
        }
    }
}
