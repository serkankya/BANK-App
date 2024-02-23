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
    public partial class frmAnaSayfa : Form
    {
        int AktifKullaniciID;
        bool ParaBildirimiKontrol;

        public frmAnaSayfa(int GelenAktifKullaniciID,bool _ParaBildirimiKontrol)
        {
            AktifKullaniciID = GelenAktifKullaniciID;
            ParaBildirimiKontrol = _ParaBildirimiKontrol;
            InitializeComponent();
        }

        private void frmAnaSayfa_Load(object sender, EventArgs e)
        {
            if (ParaBildirimiKontrol)
            {
                MessageBox.Show("Hesabınıza para geldi.");
            }

            SQL.Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_hesapBilgileri WHERE Hesap_Durum = @Durum AND Hesap_ID = @ID", SQL.Con);
            cmd.Parameters.AddWithValue("@Durum", true);
            cmd.Parameters.AddWithValue("@ID", AktifKullaniciID);

            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                lblAd.Text = dr["Hesap_KisiAd"].ToString();
                lblSoyad.Text = dr["Hesap_KisiSoyad"].ToString();
                lblIBAN.Text = dr["Hesap_IBAN"].ToString();
                decimal bakiye = Convert.ToDecimal(dr["Hesap_MevcutBakiye"].ToString());
                lblMevcutBakiye.Text = String.Format("{0:N2} TL", bakiye);
                lblHesapNo.Text = dr["Hesap_NO"].ToString();
            }

            dr.Close();
            SQL.Con.Close();
        }

        private void btnParaYukle_Click(object sender, EventArgs e)
        {
            frmParaYukle paraYukle = new frmParaYukle(AktifKullaniciID);
            paraYukle.Show();
            this.Hide();
        }

        private void btnParaCek_Click(object sender, EventArgs e)
        {
            frmParaCek paraCek = new frmParaCek(AktifKullaniciID);
            paraCek.Show();
            this.Hide();

        }

        private void btnParaTransferi_Click(object sender, EventArgs e)
        {
            frmParaTransfer transfer = new frmParaTransfer(AktifKullaniciID);
            transfer.Show();
            this.Hide();
        }

        private void btnHesapHareketleri_Click(object sender, EventArgs e)
        {
            frmHesapHareketleri hareketler = new frmHesapHareketleri(AktifKullaniciID);
            hareketler.Show();
            this.Hide();
        }
    }
}
