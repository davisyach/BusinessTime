using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessHoursCalculator;

namespace BusinessHoursTestClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetTimeZone_Click(object sender, EventArgs e)
        {
            string address = txtAddress.Text;

            TimeZoneConverter.GetTimeZone(null, null, address);
        }
    }
}
