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
    public partial class StatusBoxFejl : Form
    {
        public StatusBoxFejl(string Fejlbesked)
        {
            InitializeComponent();
            labelStatusFejl.Text = Fejlbesked;
        }

        private void StatusBoxFejl_Load(object sender, EventArgs e)
        {
            timerClose.Start();
        }

        private void timerClose_Tick(object sender, EventArgs e)
        {
            Close();
        }
    }
}
