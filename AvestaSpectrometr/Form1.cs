using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AvestaSpectrometr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            int ID = -1;
            StringBuilder sb = new StringBuilder();
            bool res = UsbCCD.CCD_Init(IntPtr.Zero, sb, ref ID);
            label1.Text = ID.ToString();
            label1.Text += ' ' + sb.ToString();

            float prm = 0;
            UsbCCD.CCD_GetParameter(ID, 7, ref prm);
            listBox1.Items.Add(prm.ToString());
            //CCDUSBDCOM01dotNET.CCD_HitTest(0);
        }
    }
}
