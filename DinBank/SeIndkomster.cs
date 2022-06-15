using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DinBank
{
    public partial class SeIndkomster : Form
    {
        Database db = new Database();
        MetoderPåTværs MTP = new MetoderPåTværs();
        public SeIndkomster()
        {
            InitializeComponent();
            DataGridViewSeIndkomst.DataSource = db.VisIndkomsterSeIndkomster();
            DataGridViewSeIndkomst.Columns[0].Visible = false;
            labelBruger.Text = db.HentBrugerNavn();
            textBoxIndkomstSaldo.Text = db.VisSumIndkomst();
        }

        private void ButtonSeIndkomsterSøg_Click(object sender, EventArgs e)
        {
            string IndkomstNavn = TextBoxIndkomstNavn.Text;
            DataGridViewSeIndkomst.DataSource = db.VisIndkomstNavn(IndkomstNavn);
            textBoxIndkomstSaldo.Text = db.VisSumIndkomstNavn(IndkomstNavn);
            ComboBoxSeInkKat.SelectedIndex = 0;
            TextBoxIndkomstNavn.Clear();
        }

        private void ButtonSeIndkomsterRyd_Click(object sender, EventArgs e)
        {
            TextBoxIndkomstNavn.Clear();
            ComboBoxSeInkKat.SelectedIndex = 0;
            DataGridViewSeIndkomst.DataSource = db.VisIndkomsterSeIndkomster();
            textBoxIndkomstSaldo.Text = db.VisSumIndkomst();
        }

        private void ComboBoxSeInkKat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxSeInkKat.SelectedIndex == 0)
            {

            }
            else
            {
                string valgtKategori = ComboBoxSeInkKat.SelectedItem.ToString().ToLower();
                DataGridViewSeIndkomst.DataSource = db.VisIndkomsterSeIndkomsterKategori(valgtKategori);
                textBoxIndkomstSaldo.Text = db.VisSaldoIndkomstKategori(valgtKategori);
            }
        }

        private void ButtonDashboard_Click(object sender, EventArgs e)
        {
            DashBoard obj = new DashBoard();
            obj.Show();
            this.Hide();
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

        private void ButtonSeUdgifter_Click(object sender, EventArgs e)
        {
            SeUdgifter obj = new SeUdgifter();
            obj.Show();
            this.Hide();
        }

        private void ButtonLogUd_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void ButtonLuk_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ButtonValutaOmregner_Click(object sender, EventArgs e)
        {
            ValutaOmregner obj = new ValutaOmregner();
            obj.Show();
            this.Hide();
        }
    }
}
