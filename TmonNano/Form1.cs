//    TmonN 
//    Arduino Thermistor reader
//    Copyright (C) <2017>  <Xander Jansen>
//
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU Affero General Public License as published
//    by the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU Affero General Public License for more details.
//
//    You should have received a copy of the GNU Affero General Public License
//    along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace TmonNano
{
    public partial class TMonN : Form
    {

        SerialPort serialPort1;
        ArduinoControllerMain ArduinoController;
        int SensorAmount;
        bool flipper;
        public TMonN()
        {
            InitializeComponent();
            SensorAmount = 0;
            ArduinoController = new ArduinoControllerMain();
            this.FormClosing += Form1_FormClosing;
            flipper = false;
            if (Properties.Settings.Default.ConnectOnStartup)
            {
                Connect();
            }
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            this.BeginInvoke(new LineReceivedEvent(LineReceived), serialPort1.ReadLine());
            serialPort1.DiscardInBuffer();
        }

        private delegate void LineReceivedEvent(string line);
        private void LineReceived(string line) // update arduino to send sensor labels so we know what sensors we are getting
        {
            if (!String.Equals(line, "HELLO FROM ARDUINO"))
            {
                string[] sVals = line.Split(','); 
                if (sVals.Count() != SensorAmount) // regenerate list on sensor # change
                {
                    flowLayoutPanel1.Controls.Clear();
                    for (int i = 0; i < sVals.Count(); i++)
                    {
                        TempDisplay Disp = new TempDisplay();
                        Disp.Name = "TmpSens"+i.ToString();
                        Disp.tId = i.ToString();
                        flowLayoutPanel1.Controls.Add(Disp);
                    }
                    SensorAmount = sVals.Count();
                }
                
                for (int i =0; i < sVals.Count(); i++)// update list items
                { 
                    TempDisplay tmp =(TempDisplay)flowLayoutPanel1.Controls["TmpSens" + i];
                    tmp.SetTmp(double.Parse(sVals[i], CultureInfo.InvariantCulture));
                }
                Flip();
            }
        }
        void Flip()
        {
            flipper = !flipper;
            if(flipper)
                toolStripStatusLabel1.Text = "Arduino on " + ArduinoController.FoundPortName;
            else
                toolStripStatusLabel1.Text = "Arduino on " + ArduinoController.FoundPortName + ".";
        }
        void Found()
        {
            toolStripStatusLabel1.Text = "Arduino on "+ArduinoController.FoundPortName;
            toolStripMenuItem1.Enabled = false;
            serialPort1 = new SerialPort();
            serialPort1.PortName = ArduinoController.FoundPortName;
            serialPort1.BaudRate = 9600;
            serialPort1.NewLine = "\r\n";
            serialPort1.DtrEnable = true;
            serialPort1.Open();
            serialPort1.DataReceived += serialPort1_DataReceived;
        }
        void Connect()
        {
            toolStripStatusLabel1.Text = "searching";
            Application.DoEvents();
            ArduinoController.SetComPort();
            if (ArduinoController.portFound)
            {
                Found();
            }
            else
            {
                ArduinoController.SetComPort();
                if (ArduinoController.portFound)
                {
                    Found();
                }
                else
                {
                    toolStripStatusLabel1.Text = "Nothing Found";
                }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            serialPort1.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Connect();
        }

        private void alwaysOnTopToolStripMenuItem_Click(object sender, EventArgs e)
        {
                alwaysOnTopToolStripMenuItem.Checked = !alwaysOnTopToolStripMenuItem.Checked;
            this.TopMost = alwaysOnTopToolStripMenuItem.Checked;
        }

        private void connectOnStartupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            connectOnStartupToolStripMenuItem.Checked = !connectOnStartupToolStripMenuItem.Checked;
            Properties.Settings.Default.ConnectOnStartup = connectOnStartupToolStripMenuItem.Checked;
        }
       
    }
}
