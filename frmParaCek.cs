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
    public partial class frmParaCek : Form
    {
        int KullaniciID_;
        public frmParaCek(int kullaniciID)
        {
            InitializeComponent();
            KullaniciID_ = kullaniciID;
        }

        double cekilecekTutar = 0, guncelTutar;
        string hesapNO;
        private void frmParaCek_Load(object sender, EventArgs e)
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

        private void btnCekimOnayla_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(mTxtPara.Text))
            {
                cekilecekTutar = Convert.ToDouble(mTxtPara.Text);
                lblCekilecekMiktar.Text = cekilecekTutar.ToString();
                frmDialogOnay onayKodu = new frmDialogOnay("Para Çekme", cekilecekTutar, guncelTutar, KullaniciID_, hesapNO);
                onayKodu.ShowDialog();
                this.Hide();

            }
            else
            {
                frmDialogOnay onayKodu = new frmDialogOnay("Para Çekme", cekilecekTutar, guncelTutar, KullaniciID_, hesapNO);
                onayKodu.ShowDialog();
                this.Hide();
            }
        }
    }
}
