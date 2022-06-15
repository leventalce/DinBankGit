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
    public partial class Login : Form
    {
        Database db = new Database();
        MetoderPåTværs MPT = new MetoderPåTværs();
        bool visKodeOrd = true;
        public Login()
        {
            InitializeComponent();
        }

        public static string User;
        private void ButtonLogInd_Click(object sender, EventArgs e)
        {
            db.KommerFraRegistrerEllerLogin = true;
            if (TextBoxBrugerNavn.Text == "" || TextBoxKodeOrd.Text == "")
            {
                VisStatusFejl();
            }
            else
            {
                bool KorrektLogin = false;
                if (db.Login(TextBoxBrugerNavn.Text, TextBoxKodeOrd.Text, KorrektLogin))
                {
                    User = TextBoxBrugerNavn.Text;
                    DashBoard Obj = new DashBoard();
                    Obj.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Fokert brugernavn eller kodeord");
                    TextBoxBrugerNavn.Text = "";
                    TextBoxKodeOrd.Text = "";
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Registrer obj = new Registrer();
            obj.Show();
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

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            MPT.VisKodeOrd(TextBoxKodeOrd, visKodeOrd);
            visKodeOrd = MPT.VisKodeOrd(TextBoxKodeOrd, visKodeOrd);
            
        }

        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            List<string> brugernavne = db.HentAlleBrugerNavne();
            foreach (var navne in brugernavne)
            {
                TextBoxBrugerNavn.Text += navne + " ";
            }
        }
    }
}
