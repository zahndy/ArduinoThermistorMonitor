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
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.IO;

namespace TmonNano
{
    public class ArduinoControllerMain {

        public SerialPort currentPort;
        public string FoundPortName;
        public bool portFound;
        public void SetComPort()
	    {
	        try
	        {
		        string[] ports = SerialPort.GetPortNames();
		        foreach (string port in ports)
		        {
		            currentPort = new SerialPort(port, 9600);
		            if (DetectArduino())
		            {
			            portFound = true;
			            break;
		            }
		            else
		            {
			            portFound = false;
		            }
		        }
	        }
	        catch (Exception e)
	        {
	        }
	    }
        public bool DetectArduino()
	    {
	        try
	        {
		        // Hello handshake
		        byte[] buffer = new byte[5];
		        buffer[0] = Convert.ToByte(16);
		        buffer[1] = Convert.ToByte(128);
		        int intReturnASCII = 0;
		        char charReturnValue = (Char)intReturnASCII;
		        currentPort.Open();
		        currentPort.Write(buffer, 0, 2);
		        Thread.Sleep(800);
		        int count = currentPort.BytesToRead;
		        string returnMessage = "";
		        while (count > 0)
		        {
		            intReturnASCII = currentPort.ReadByte();
		            returnMessage = returnMessage + Convert.ToChar(intReturnASCII);
		            count--;
		        }
                FoundPortName = currentPort.PortName;
		        currentPort.Close();
		        if (returnMessage.Contains("HELLO FROM ARDUINO"))
		        {
		            return true;
		        }
		        else
		        {
		            return false;
		        }
	        }
	        catch (Exception e)
	        {
		        return false;
	        }
        }
   }
}
