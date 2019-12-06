using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AvestaSpectrometr
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            callBackDataReadyHandler = onCallBack;
        }

        private static UsbCCD.CallBackDataReady callBackDataReadyHandler;

        UsbCCD.TCCDUSBParams Params;
        UsbCCD.TCCDUSBExtendParams ExtendParams;
        int ID = 0;
        string serium = "";
        bool inited = false;

        private void Button1_Click(object sender, EventArgs e)
        {
            InitCCDUSB();
        }

        private void Button2_Click(object sender, EventArgs e)
        {

            if(!inited)
            {
                MessageBox.Show("Avesta not inited");
                return;
            }

            int pixels = ExtendParams.nNumPixelsH * ExtendParams.nNumPixelsV * ExtendParams.nNumReadOuts;
            int cb = pixels * sizeof(uint);
            IntPtr pData = Marshal.AllocHGlobal(cb);

            UsbCCD.CCD_InitMeasuringData(0, pData);
            UsbCCD.CCD_StartWaitMeasuring(0);
            int[] data = new int[pixels];

            Marshal.Copy(pData, data, 0, data.Length);

            for(int i = 0; i < data.Length; i++)
            {
                int val = (int)data[i];
                //double y = val;
                chart1.Series[0].Points.AddXY(i, val);
            }

            Marshal.FreeHGlobal(pData);
        }

        private void InitCCDUSB()
        {
            int id = 0;
            string str = "";
            inited = UsbCCD.CCD_Init(Handle, str, ref id);
            //Get serium
            serium = "";
            inited = UsbCCD.CCD_GetSerialNum(0, ref serium);
            listBox1.Items.Add("Serium number: " + serium);
            listBox1.Items.Add("SensorName: " + UsbCCD.CCD_GetSensorName(0));
            //Get ID
            inited = UsbCCD.CCD_GetID(serium, ref ID);
            listBox1.Items.Add("ID: " + ID.ToString());
            //Fill params
            ExtendParams = new UsbCCD.TCCDUSBExtendParams();
            inited = UsbCCD.CCD_GetExtendParameters(0, ref ExtendParams);
            Params = new UsbCCD.TCCDUSBParams();
            inited = UsbCCD.CCD_GetParameters(0, ref Params);
            /*
            uint status = 0;
            UsbCCD.CCD_GetMeasureStatus(0, ref status);
            listBox1.Items.Add(status.ToString());
            */
            if (!inited)
                MessageBox.Show("InitError");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitCCDUSB();
        }

        private void onCallBack()
        {
            if(InvokeRequired)
            {
                BeginInvoke(new Action(() => listBox1.Items.Add("ee")));
            }
            
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (!inited)
            {
                MessageBox.Show("Avesta not inited");
                return;
            }

            bool res = UsbCCD.CCD_InitMeasuring(0);
            //res = UsbCCD.CCD_InitMeasuringCallBack(0, callBackDataReadyHandler);
            //listBox1.Items.Add(res);
            
            int pixels = ExtendParams.nNumPixelsH * ExtendParams.nNumPixelsV * ExtendParams.nNumReadOuts;
            int cb = pixels * sizeof(uint);
            IntPtr pData = Marshal.AllocHGlobal(pixels);

            UsbCCD.CCD_GetData(0, pData);
            int[] data = new int[cb];

            Marshal.Copy(pData, data, 0, data.Length);
            Marshal.FreeHGlobal(pData);

            chart1.Series[0].Points.Clear();
            for (int i = 0; i < data.Length; i++)
            {
                uint val = (uint)data[i];
                //double y = val;
                chart1.Series[0].Points.AddXY(i, val);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UsbCCD.CCD_DoneMeasuring(0);
        }
    }
}
