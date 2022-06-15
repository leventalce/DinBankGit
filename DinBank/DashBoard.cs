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

namespace DinBank
{
    public partial class DashBoard : Form
    {
        Database db = new Database();
        MetoderPåTværs MTP = new MetoderPåTværs();
        public DashBoard()
        {
            InitializeComponent();
            labelBruger.Text = "Velkommen, " + db.HentBrugerNavn();
            labelIndkomstOversigt.Text = db.TotalIndkomst();
            labelUdgiftOversigt.Text = db.TotalUdgift();
            labelBalance.Text = db.FåSaldo();
            labelAntalIndkomster.Text = db.AntalIndkomster().ToString();
            labelAntalUdgifter.Text = db.AntalUdgifter().ToString();
            VisSenesteUdgift();
            VisSenesteIndkomst();
            labelIndkomst30Dage.Text = db.VisIndkomstSidste30Dage();
            labelUdigft30Dage.Text = db.VisUdgiftSidste30Dage();
            labelSaldo30Dage.Text = db.saldo30Dage();
            labelAntalInkomster30Dage.Text = db.VisAntalIndkomstSidste30Dage();
            labelAntalUdgifter30Dage.Text = db.VisAntalUdgiftSidste30Dage();
        }
        public void VisSenesteIndkomst()
        {
            var senesteIndkomst = db.HentSenesteIndkomst();
            if (senesteIndkomst.Rows.Count > 0)
            {
                string datoDashboard = senesteIndkomst.Rows[0].Field<DateTime>("InkDato").ToString().Substring(0, 10);
                string navnDashboard = senesteIndkomst.Rows[0].Field<string>("InkNavn").ToString();
                string kategoriDashboard = senesteIndkomst.Rows[0].Field<string>("InkCat").ToString();
                string beloebDashboard = senesteIndkomst.Rows[0].Field<int>("InkBlb").ToString("N");
                labelSenesteIndkomstDato.Text = "Dato: " + datoDashboard;
                labelSenesteIndkomstNavn.Text = navnDashboard;
                labelSenesteIndkomstKategori.Text = "Kategori: " + kategoriDashboard;
                labelSenesteIndkomstBeloeb.Text = "Beløb DKK: " + beloebDashboard;
            }
        }
        public void VisSenesteUdgift()
        {
            var SenesteUdgift = db.HentSenesteUdgift();
            if (SenesteUdgift.Rows.Count > 0)
            {
                string datoDashboard = SenesteUdgift.Rows[0].Field<DateTime>("UdgDato").ToString().Substring(0, 10);
                string navnDashboard = SenesteUdgift.Rows[0].Field<string>("UdgNavn").ToString();
                string kategoriDashboard = SenesteUdgift.Rows[0].Field<string>("UdgKat").ToString();
                string beloebDashboard = SenesteUdgift.Rows[0].Field<int>("UdgBlb").ToString("N");
                labelSenesteUdgiftDato.Text = "Dato: " + datoDashboard;
                labelSenesteUdgiftNavn.Text = navnDashboard;
                labelSenesteUdgiftKat.Text = "Kategori: " + kategoriDashboard;
                labelSenesteUdgiftBeloeb.Text = "Beløb DKK: " + beloebDashboard;
            }
        }
        private void ButtonIndkomst_Click(object sender, EventArgs e)
        {
            Indkomst obj = new Indkomst();
            obj.Show();
            this.Hide();
        }

        private void ButtonUdgift_Click(object sender, EventArgs e)
        {
            Udgift obj = new Udgift();
            obj.Show();
            this.Hide();
        }

        private void ButtonSeIndkomster_Click(object sender, EventArgs e)
        {
            SeIndkomster obj = new SeIndkomster();
            obj.Show();
            this.Hide();
        }

        private void ButtonSeUdgifter_Click(object sender, EventArgs e)
        {
            SeUdgifter obj = new SeUdgifter();
            obj.Show();
            this.Hide();
        }
       
        private void ButtonLuk_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ButtonLogUd_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void ButtonSupport_Click(object sender, EventArgs e)
        {
            SendEmail obj = new SendEmail();
            obj.Show();
        }

        private void guna2GradientTileButton1_Click(object sender, EventArgs e)
        {
            ValutaOmregner obj = new ValutaOmregner();
            obj.Show();
            this.Hide();
        }
    }
}
