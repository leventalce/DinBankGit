using Newtonsoft.Json;
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
    public partial class ValutaOmregner : Form
    {
        Database db = new Database();
        public ValutaOmregner()
        {
            InitializeComponent();
            labelBruger.Text = db.HentBrugerNavn();
        }

        private void ButtonBeregn_Click(object sender, EventArgs e)
        {
            string valgtValuta = ComboBoxValutaer.SelectedItem.ToString();
            var kurs = Rates.FindKurs(valgtValuta);
            if (Regex.IsMatch(TextBoxBeloeb.Text, ".*?[a-zA-Z].*?"))
            {
                MessageBox.Show("Ingen bogstaver er tilladt", "Bogstaver i boksen");
                return;
            }
            double beløb = Convert.ToDouble(TextBoxBeloeb.Text);
            double udregning = beløb * kurs;
            beløbet.Text = udregning.ToString();
        }
        public class Rates
        {
            public static double FindKurs(string KursNavn)
            {
                try
                {
                    double kursen = 0.0;
                    String URLString = "https://v6.exchangerate-api.com/v6/93d0b22281f4938a10d2df37/latest/DKK";
                    using (var webClient = new System.Net.WebClient())
                    {
                        var json = webClient.DownloadString(URLString);
                        API_Obj Test = JsonConvert.DeserializeObject<API_Obj>(json);

                        List<ConversionRate> AlleKurser = new List<ConversionRate>();
                        AlleKurser.Add(Test.conversion_rates);

                        if (KursNavn.ToLower() == "eur")
                        {
                            kursen = Test.conversion_rates.EUR;
                        }
                        else if (KursNavn.ToLower() == "gbp")
                        {
                            kursen = Test.conversion_rates.GBP;
                        }
                        else if (KursNavn.ToLower() == "aed")
                        {
                            kursen = Test.conversion_rates.AED;
                        }
                        else if (KursNavn.ToLower() == "sek")
                        {
                            kursen = Test.conversion_rates.SEK;
                        }
                        else if (KursNavn.ToLower() == "nok")
                        {
                            kursen = Test.conversion_rates.NOK;
                        }
                        else if (KursNavn.ToLower() == "usd")
                        {
                            kursen = Test.conversion_rates.USD;
                        }
                        else if (KursNavn.ToLower() == "try")
                        {
                            kursen = Test.conversion_rates.TRY;
                        }
                        else if (KursNavn.ToLower() == "aud")
                        {
                            kursen = Test.conversion_rates.AUD;
                        }
                        else if (KursNavn.ToLower() == "brl")
                        {
                            kursen = Test.conversion_rates.BRL;
                        }
                        else if (KursNavn.ToLower() == "jpy")
                        {
                            kursen = Test.conversion_rates.JPY;
                        }
                        return kursen;
                    }
                }
                catch (Exception)
                {
                    return 0.0;
                }
            }
        }

        public class API_Obj
        {
            public string result { get; set; }
            public string documentation { get; set; }
            public string terms_of_use { get; set; }
            public string time_last_update_unix { get; set; }
            public string time_last_update_utc { get; set; }
            public string time_next_update_unix { get; set; }
            public string time_next_update_utc { get; set; }
            public string base_code { get; set; }
            public ConversionRate conversion_rates { get; set; }
        }

        public class ConversionRate
        {
            public double AED { get; set; }
            public double ARS { get; set; }
            public double AUD { get; set; }
            public double BGN { get; set; }
            public double BRL { get; set; }
            public double BSD { get; set; }
            public double CAD { get; set; }
            public double CHF { get; set; }
            public double CLP { get; set; }
            public double CNY { get; set; }
            public double COP { get; set; }
            public double CZK { get; set; }
            public double DKK { get; set; }
            public double DOP { get; set; }
            public double EGP { get; set; }
            public double EUR { get; set; }
            public double FJD { get; set; }
            public double GBP { get; set; }
            public double GTQ { get; set; }
            public double HKD { get; set; }
            public double HRK { get; set; }
            public double HUF { get; set; }
            public double IDR { get; set; }
            public double ILS { get; set; }
            public double INR { get; set; }
            public double ISK { get; set; }
            public double JPY { get; set; }
            public double KRW { get; set; }
            public double KZT { get; set; }
            public double MXN { get; set; }
            public double MYR { get; set; }
            public double NOK { get; set; }
            public double NZD { get; set; }
            public double PAB { get; set; }
            public double PEN { get; set; }
            public double PHP { get; set; }
            public double PKR { get; set; }
            public double PLN { get; set; }
            public double PYG { get; set; }
            public double RON { get; set; }
            public double RUB { get; set; }
            public double SAR { get; set; }
            public double SEK { get; set; }
            public double SGD { get; set; }
            public double THB { get; set; }
            public double TRY { get; set; }
            public double TWD { get; set; }
            public double UAH { get; set; }
            public double USD { get; set; }
            public double UYU { get; set; }
            public double ZAR { get; set; }
        }

        private void ButtonLuk_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ButtonIndkomst_Click(object sender, EventArgs e)
        {
            Indkomst obj = new Indkomst();
            obj.Show();
            this.Hide();
        }

        private void guna2GradientTileButton3_Click(object sender, EventArgs e)
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
            SeIndkomster obj = new SeIndkomster();
            obj.Show();
            this.Hide();
        }

        private void ButtonLogUd_Click(object sender, EventArgs e)
        {
            Login obj = new Login();
            obj.Show();
            this.Hide();
        }

        private void ButtonDashboard_Click(object sender, EventArgs e)
        {
            DashBoard obj = new DashBoard();
            obj.Show();
            this.Hide();
        }
    }
}
