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
    public partial class frmParaYukle : Form
    {
        int KullaniciID_;
        public frmParaYukle(int KullaniciID)
        {
            InitializeComponent();
            KullaniciID_ = KullaniciID;
        }

        double yuklenecekTutar = 0, guncelTutar;
        string hesapNO;
        private void frmParaYukle_Load(object sender, EventArgs e)
        {
            SQL.Con.Close();
            SQL.Con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Hesap_MevcutBakiye,Hesap_NO FROM tbl_hesapBilgileri WHERE Hesap_ID = @ID", SQL.Con);
            cmd.Parameters.AddWithValue("@ID", KullaniciID_);

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                guncelTutar = Convert.ToDouble(dr["Hesap_MevcutBakiye"]);
                hesapNO = dr["Hesap_NO"].ToString();
            }
            dr.Close();
            SQL.Con.Close();
        }

        private void btnYuklemeOnayla_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(mTxtPara.Text))
            {
                yuklenecekTutar = Convert.ToDouble(mTxtPara.Text);
                lblYuklenecekMiktar.Text = yuklenecekTutar.ToString();

                frmDialogOnay KodOnayi = new frmDialogOnay("Para Yükleme", yuklenecekTutar, guncelTutar, KullaniciID_, hesapNO);
                KodOnayi.ShowDialog();
                this.Hide();
            }
            else
            {
                frmDialogOnay KodOnayi = new frmDialogOnay("Para Yükleme", yuklenecekTutar, guncelTutar, KullaniciID_, hesapNO);
                KodOnayi.ShowDialog();
                this.Hide();
            }
        }

        private void btn50TL_Click(object sender, EventArgs e)
        {
            yuklenecekTutar = 50;
            lblYuklenecekMiktar.Text = yuklenecekTutar.ToString();
        }

        private void btn100TL_Click(object sender, EventArgs e)
        {
            yuklenecekTutar = 100;
            lblYuklenecekMiktar.Text = yuklenecekTutar.ToString();

        }

        private void btn200TL_Click(object sender, EventArgs e)
        {
            yuklenecekTutar = 200;
            lblYuklenecekMiktar.Text = yuklenecekTutar.ToString();
        }

        private void btn300TL_Click(object sender, EventArgs e)
        {
            yuklenecekTutar = 300;
            lblYuklenecekMiktar.Text = yuklenecekTutar.ToString();
        }

        private void btn500TL_Click(object sender, EventArgs e)
        {
            yuklenecekTutar = 500;
            lblYuklenecekMiktar.Text = yuklenecekTutar.ToString();

        }

        private void btn1000TL_Click(object sender, EventArgs e)
        {
            yuklenecekTutar = 1000;
            lblYuklenecekMiktar.Text = yuklenecekTutar.ToString();

        }
    }
}
