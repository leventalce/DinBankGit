using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace DinBank
{
    public partial class Udgift : Form
    {
        Database db = new Database();
        MetoderPåTværs MTP = new MetoderPåTværs();
        public Udgift()
        {
            InitializeComponent();
            DateTimePickerUdgiftDato.Value = DateTime.Now;
            labelBruger.Text = db.HentBrugerNavn();
        }

        private void ButtonOpretUdgift_Click(object sender, EventArgs e)
        {
            if (TextBoxUdgiftNavn.Text == "" || TextBoxUdgiftBeloeb.Text == "" || ComboBoxUdgiftKategori.SelectedIndex == -1)
            {
                MessageBox.Show("Du mangler at udfylde nogle felter", "Mangler tekst");
                return;
            }
            else if (Regex.IsMatch(TextBoxUdgiftBeloeb.Text, ".*?[a-zA-Z].*?"))
            {
                MessageBox.Show("Ingen bogstaver i beløb feltet", "Bogstav i boksen");
                TextBoxUdgiftBeloeb.Text = "";
                return;
            }
            else
            {
                try
                {
                    db.GemUdgiftIDb(TextBoxUdgiftNavn.Text, TextBoxUdgiftBeloeb.Text, ComboBoxUdgiftKategori.SelectedItem.ToString(), 
                    MTP.LaveDatoOm(DateTimePickerUdgiftDato.Value), TextBoxUdgiftBeskrivelse.Text, db.HentBrugerNavn());
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
            TextBoxUdgiftNavn.Text = "";
            TextBoxUdgiftBeskrivelse.Text = "";
            TextBoxUdgiftBeloeb.Text = "";
            ComboBoxUdgiftKategori.SelectedIndex = 0;
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
