using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DinBank
{
    public partial class Indkomst : Form
    {
        Database db = new Database();
        MetoderPåTværs MTP = new MetoderPåTværs();
        
        public Indkomst()
        {
            InitializeComponent();
            DateTimePickerIndkomstDato.Value = DateTime.Now;
            labelBruger.Text = db.HentBrugerNavn();
        }


        private void ButtonOpretIndkomst_Click(object sender, EventArgs e)
        {
            if (TextBoxIndkomstNavn.Text == "" || TextBoxIndkomstBeloeb.Text == "" || ComboBoxIndkomstKategori.SelectedIndex == -1)
            {
                MessageBox.Show("Du mangler at udfylde nogle felter", "Mangler tekst");
                return;
            }
            else if (Regex.IsMatch(TextBoxIndkomstBeloeb.Text, ".*?[a-zA-Z].*?"))
            {
                MessageBox.Show("Ingen bogstaver i beløb feltet", "Bogstav i boksen");
                TextBoxIndkomstBeloeb.Text = "";
                return;
            }
            else
            {
                try
                {
                    db.GemIndkomstIDb(TextBoxIndkomstNavn.Text, TextBoxIndkomstBeloeb.Text, ComboBoxIndkomstKategori.SelectedItem.ToString(),
                        MTP.LaveDatoOm(DateTimePickerIndkomstDato.Value), TextBoxIndkomstBeskrivelse.Text, db.HentBrugerNavn());
                    Clear();
                    VisStatusSuccess();
                }
                catch (Exception Ex)
                {
                    db.FejlHaandteringOgBesked(Ex);
                }

            }
        }
       
        private void Clear()
        {
            TextBoxIndkomstNavn.Text = "";
            TextBoxIndkomstBeskrivelse.Text = "";
            TextBoxIndkomstBeloeb.Text = "";
            ComboBoxIndkomstKategori.SelectedIndex = 0;
        }

        private void VisStatusSuccess()
        {
            StatusBoxSuccess status = new StatusBoxSuccess();
            status.Show();
        }

        private void VisStatusFejl()
        {
            StatusBoxFejl status = new StatusBoxFejl("Mangler information!");
            status.Show();
        }

        private void ButtonLuk_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ButtonDashBoard_Click(object sender, EventArgs e)
        {
            DashBoard obj = new DashBoard();
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
