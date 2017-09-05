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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TmonNano
{
    public partial class TempDisplay : UserControl
    {
        List<double> samples;
        public double min;
        public double max;
        public string TmpDisp
        {
            get{return label1.Text;}
            set{ label1.Text = value;}
        }
        public string Min
        {
            get { return label2.Text; }
            set { label2.Text = value; }
        }
        public string Max
        {
            get { return label3.Text; }
            set { label3.Text = value; }
        }
        public string tId
        {
            get { return label4.Text; }
            set { label4.Text = value; }
        }

        public TempDisplay()
        {
            InitializeComponent();
            samples = new List<double>();
            min = 100;
            max = 0;
        }

        public void SetTmp(double value)
        {
            double average = samples.Sum() / samples.Count;
            string pp = "    ";
            if (value > average)
            {
                pp = " ↑";
            }
            else if (value < average)
            {
                pp = " ↓";
            }
            if (average == value || average == value + 00.01 || average == value - 00.01)
            {
                pp = "    ";
            }
            if (value > max) // max
                max = value;
            if (value < min)
                min = value;
            label1.Text = "  " + value.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) + " °C" + pp;
            label2.Text = "Min: " + min;
            label3.Text = "Max: " + max;
            samples.Add(value);
            if (samples.Count > 100)
            {
                samples.RemoveAt(0);
            }
            
            
        }

    }
}
