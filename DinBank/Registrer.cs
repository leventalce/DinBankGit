using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DinBank
{
    public partial class Registrer : Form
    {
        Database db = new Database();
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Bruger\OneDrive\Dokumenter\Dinbank\FinansDbFinal.mdf;Integrated Security=True;Connect Timeout=30");
        MetoderPåTværs MPT = new MetoderPåTværs();
        bool visKodeOrd = true;
        public Registrer()
        {
            InitializeComponent();
            DateTimePickerFøds.Value = DateTime.Now;
        }
        private void Clear()
        {
            TextBoxBrugerNavn.Text = "";
            TextBoxKodeOrd.Text = "";
            TextBoxTelefon.Text = "";
            TextBoxAdresse.Text = "";
        }

        private void ButtonOpret_Click(object sender, EventArgs e)
        {
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{8,15}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
            if (TextBoxBrugerNavn.Text == "" || TextBoxTelefon.Text == "" || TextBoxAdresse.Text == "" || TextBoxKodeOrd.Text == "")
            {
                MessageBox.Show("Du mangler at udfylde nogle af felterne");
                TextBoxKodeOrd.Text = "";
                return;
            }
            else if (!hasLowerChar.IsMatch(TextBoxKodeOrd.Text))
            {
                MessageBox.Show("Kodeordet skal indeholde et lille bogstav");
                TextBoxKodeOrd.Text = "";
                return;
            }
            else if (!hasUpperChar.IsMatch(TextBoxKodeOrd.Text))
            {
                MessageBox.Show("Kodeordet skal indeholde et stort bogstav");
                TextBoxKodeOrd.Text = "";
                return;
            }
            else if (!hasMiniMaxChars.IsMatch(TextBoxKodeOrd.Text))
            {
                MessageBox.Show("Kodeordet skal være mindst 8 og højst 15 tegn");
                TextBoxKodeOrd.Text = "";
                return;
            }
            else if (!hasNumber.IsMatch(TextBoxKodeOrd.Text))
            {
                MessageBox.Show("Kodeordet skal indeholde mindst et tal");
                TextBoxKodeOrd.Text = "";
                return;
            }
            else if (!hasMiniMaxChars.IsMatch(TextBoxBrugerNavn.Text))
            {
                MessageBox.Show("Dit brugernavn skal være mellem 8 og 15 tegn");
                TextBoxBrugerNavn.Text = "";
                return;
            }
            else
            {
                try
                {
                    List<string> brugernavne = db.HentAlleBrugerNavne();
                    foreach (var navne in brugernavne)
                    {
                        if (navne.ToLower() == TextBoxBrugerNavn.Text.ToLower())
                        {
                            MessageBox.Show("Brugernavnet findes allerede", "Fejl");
                            Clear();
                            return;
                        }
                    }

                    db.KommerFraRegistrerEllerLogin = true;
                    //db.OpretRegister(TextBoxBrugerNavn.Text, MPT.LaveDatoOm(DateTimePickerFøds.Value), TextBoxTelefon.Text, TextBoxKodeOrd.Text, TextBoxAdresse.Text);
                    OpretIDB(TextBoxBrugerNavn.Text, MPT.LaveDatoOm(DateTimePickerFøds.Value), TextBoxTelefon.Text, TextBoxKodeOrd.Text, TextBoxAdresse.Text);
                    VisStatusSuccess();
                    Clear();
                }
                catch (Exception ex)
                {
                    db.FejlHaandteringOgBesked(ex);
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void labelTilbage_Click(object sender, EventArgs e)
        {
                this.Hide();
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
            this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            MPT.VisKodeOrd(TextBoxKodeOrd, visKodeOrd);
            visKodeOrd = MPT.VisKodeOrd(TextBoxKodeOrd, visKodeOrd);
        }

        private void OpretIDB(string Navn, string Fødselsdag, string Telefon, string KodeOrd, string Adresse)
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("insert into BrugerTbl(BrgNavn,BrgFød,BrgTlf,BrgKode,BrgAdd)values(@BN,@BF,@BT,@BK,@BA)", Con);
                cmd.Parameters.AddWithValue("@BN", Navn);
                cmd.Parameters.AddWithValue("@BF", Fødselsdag);
                cmd.Parameters.AddWithValue("@BT", Telefon);
                cmd.Parameters.AddWithValue("@BK", KodeOrd);
                cmd.Parameters.AddWithValue("@BA", Adresse);
                cmd.ExecuteNonQuery();
            }
            catch (Exception Ex)
            {
                db.FejlHaandteringOgBesked(Ex);
            }
            finally
            {
                Con.Close();
            }
        }
    }
}
