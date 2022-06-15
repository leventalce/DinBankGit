using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace DinBank
{
    class MetoderPåTværs
    {
        Database db = new Database();
        public bool VisKodeOrd(Guna2TextBox textbox, bool VisKode)
        {
            if (VisKode)
            {
                VisKode = false;
                textbox.PasswordChar = '\0';
            }
            else
            {
                VisKode = true;
                textbox.PasswordChar = '*';
            }
            return VisKode;
        }

        public string LaveDatoOm(DateTime Dato)
        {
            string datoString = Dato.ToString();
            string Dage = datoString.Substring(0, 2);
            string Måneder = datoString.Substring(3, 2);
            string År = datoString.Substring(6, 4);
            datoString = Måneder + "-" + Dage + "-" + År;
            return datoString;
        }
        public string BrugerMedStort()
        {
            string bruger = Login.User.ToString();
            bruger = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(bruger.ToLower());
            return bruger;
        }

        public void FejlHaandteringOgBeskedMPT(Exception Ex)
        {
            db.IndsætFejlTilDB(Ex, db.HentBrugerNavn());
            //File.AppendAllText(@"C:\temp\FejlBeskederDinkBank.txt", DateTime.Now.ToString() + "\n" + Ex.ToString() + "\n" + "\n");
            MessageBox.Show("Der er opstået en fejl skriv en besked til supporten på dashboard siden.");
        }
    }
}
