using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using Topshelf;

namespace DinBank
{
    public partial class SendEmail : Form
    {
        Database db = new Database();
        public SendEmail()
        {
            InitializeComponent();
        }

        private void ButtonLuk_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ButtonSendEmail_Click(object sender, EventArgs e)
        {
            if (TextBoxBesked.Text == "" || TextBoxEmail.Text == "" || TextBoxEmne.Text == "")
            {
                MessageBox.Show("Du har ikke udfyldt alle felterne", "Fejl");
                return;
            }
            try
            {
                string from = "hverdagdinajensen@gmail.com";
                string password = "wrljgimtfynxvode";

                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(TextBoxEmail.Text);
                msg.Subject = TextBoxEmne.Text;
                msg.To.Add(new MailAddress("hverdagdinajensen@gmail.com"));
                msg.Body = TextBoxBesked.Text;
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(from, password),
                    EnableSsl = true
                };
                smtpClient.Send(msg.From.ToString(), msg.From.ToString(), msg.Subject, msg.Body);
                clear();
                MessageBox.Show("Email sendt");
            }
            catch (Exception Ex)
            {
                db.FejlHaandteringOgBesked(Ex);
            }
        }

        private void clear()
        {
            TextBoxBesked.Clear();
            TextBoxEmail.Clear();
            TextBoxEmne.Clear();
        }
    }
}
