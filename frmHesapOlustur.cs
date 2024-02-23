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
    public partial class frmHesapOlustur : Form
    {
        public frmHesapOlustur()
        {
            InitializeComponent();
        }

        private void frmHesapOlustur_Load(object sender, EventArgs e)
        {
            txtSifre.MaxLength = 50;
            txtSoyad.MaxLength = 50;
            txtMail.MaxLength = 50;
            txtKizlikSoyadi.MaxLength = 30;
            txtAdres.MaxLength = 200;
        }

        bool textKontrol()
        {
            bool kontrol = false;

            foreach (Control item in Controls)
            {
                if (item is TextBox)
                {
                    if (item.Text == string.Empty || item.Text == " " || item.Text == "  ")
                    {
                        kontrol = true;
                        break;
                    }
                }
            }

            if (kontrol == false)
            {
                foreach (Control item in Controls)
                {
                    if (item is MaskedTextBox)
                    {
                        if (item.Text == string.Empty || item.Text == " " || item.Text == "  ")
                        {
                            kontrol = true;
                            break;
                        }
                    }
                }
            }

            if (kontrol == false)
                return true; //Eğer kontrol false ise yani her şarta uyuyorsa true dönecek.
            else
                return false; //Eğer kontrol true ise hata var demek. False dönecek.
        }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            if (adresSayac >= 60)
            {
                if (txtMail.Text.Contains("@") && txtMail.Text.Contains("."))
                {
                    bool textKontrolSonuc = textKontrol();
                    if (textKontrolSonuc)
                    {
                        string IBAN = IBAN_Olustur.YeniIban();
                        string IBANNO = IBAN.Replace(" ", "");
                        string HesapNO = HesapNo_Olustur.YeniHesapNo();

                        SQL.Con.Open();
                        SqlCommand cmd = new SqlCommand("INSERT INTO tbl_hesapBilgileri (Hesap_TCKimlik,Hesap_KisiAd,Hesap_KisiSoyad,Hesap_Sifre,Hesap_TelefonNo,Hesap_DogumTarihi,Hesap_Email,Hesap_AnneKizlikSoyismi,Hesap_Adres,Hesap_IBAN,Hesap_IBANNO,Hesap_NO) VALUES (@TCKimlik,@Ad,@Soyad,@Sifre,@Telefon,@DogumTarihi,@Email,@AnneKizlikSoyadi,@Adres,@IBAN,@IBANNO,@HesapNo)", SQL.Con);
                        cmd.Parameters.AddWithValue("@TCKimlik", mtxtTCKimlik.Text);
                        cmd.Parameters.AddWithValue("@Ad", txtAd.Text);
                        cmd.Parameters.AddWithValue("@Soyad", txtSoyad.Text);
                        cmd.Parameters.AddWithValue("@Sifre", txtSifre.Text);
                        cmd.Parameters.AddWithValue("@Telefon", mtxtTelNo.Text);
                        cmd.Parameters.AddWithValue("@DogumTarihi", dtDogumTarihi.Value);
                        cmd.Parameters.AddWithValue("@Email", txtMail.Text);
                        cmd.Parameters.AddWithValue("@AnneKizlikSoyadi", txtKizlikSoyadi.Text);
                        cmd.Parameters.AddWithValue("@Adres", txtAdres.Text);
                        cmd.Parameters.AddWithValue("@IBAN", IBAN);
                        cmd.Parameters.AddWithValue("@IBANNO", IBANNO);
                        cmd.Parameters.AddWithValue("@HesapNo", HesapNO);

                        cmd.ExecuteNonQuery();
                        SQL.Con.Close();

                        MessageBox.Show("Hesabınız başarı ile oluşturuldu.\nGiriş sayfasına aktarılıyorsunuz.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        frmGiris login = new frmGiris();
                        login.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Lütfen ilgili yerleri doldurunuz.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen mail adresinizi tekrar gözden geçiriniz.\nMail adresi '@' ve '.' içermek zorundadır.", "UYARI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Adres bilginiz en az 60 haneli olmalıdır.", "HATA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        int adresSayac = 0;
        private void txtAdres_TextChanged(object sender, EventArgs e)
        {
            adresSayac = txtAdres.Text.Length;

            if (adresSayac < 10)
                lblAdresSayac.Text = "00" + adresSayac.ToString();
            else if (adresSayac < 100)
                lblAdresSayac.Text = "0" + adresSayac.ToString();
            else if (adresSayac <= 200)
                lblAdresSayac.Text = adresSayac.ToString();
        }
    }
}
