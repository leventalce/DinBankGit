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
    public partial class SeUdgifter : Form
    {
        Database db = new Database();
        MetoderPåTværs MTP = new MetoderPåTværs();
        public SeUdgifter()
        {
            InitializeComponent();
            DataGridViewSeUdgift.DataSource = db.VisUdgifterSeUdgifter();
            DataGridViewSeUdgift.Columns[0].Visible = false;
            labelBruger.Text = db.HentBrugerNavn();
            textBoxUdgiftSaldo.Text = db.VisSumUdgift();
        }

        private void ButtonUdgiftSøg_Click(object sender, EventArgs e)
        {
            string UdgiftNavn = TextBoxUdgiftNavn.Text;
            DataGridViewSeUdgift.DataSource = db.VisUdgifterNavn(UdgiftNavn);
            TextBoxUdgiftNavn.Clear();
            textBoxUdgiftSaldo.Text = db.VisSumUdgiftNavn(UdgiftNavn);
        }

        private void ButtonUdgiftRyd_Click(object sender, EventArgs e)
        {
            TextBoxUdgiftNavn.Clear();
            ComboBoxSeUdgKat.SelectedIndex = 0;
            DataGridViewSeUdgift.DataSource = db.VisUdgifterSeUdgifter();
            textBoxUdgiftSaldo.Text = db.VisSumUdgift();
        }

        private void ComboBoxSeUdgKat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ComboBoxSeUdgKat.SelectedIndex == 0)
            {

            }
            else
            {
                string valgtKategori = ComboBoxSeUdgKat.SelectedItem.ToString().ToLower();
                DataGridViewSeUdgift.DataSource = db.VisUdgifterKategori(valgtKategori);
                textBoxUdgiftSaldo.Text = db.VisSumUdgiftKategori(valgtKategori);
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

        private void ButtonSeIndkomster_Click(object sender, EventArgs e)
        {
            SeIndkomster obj = new SeIndkomster();
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

        private void ButtonValutaOmregner_Click(object sender, EventArgs e)
        {
            ValutaOmregner obj = new ValutaOmregner();
            obj.Show();
            this.Hide();
        }
    }
}
