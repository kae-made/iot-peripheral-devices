// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kae.IoT.Device.Sensors
{
    public class MHZ19BSensor : ISensor
    {
        public string SensorName { get { return "MHZ19B"; } }

        private SerialPort serialPort = null;
        public string Port { get; set; }

        public bool Initalize()
        {
            bool succeeded = true;
            if (string.IsNullOrEmpty(Port))
            {
                Console.Write("Port shouldn't be null string!");
                succeeded = false;
            }
            serialPort = new SerialPort(Port, 9600, Parity.None, 8, StopBits.One);
            try
            {
                serialPort.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                succeeded = false;
            }
            return succeeded;
        }

        public IList<(SensorType SensorType, double Value)> Read()
        {
            int co2 = -1;
            if (serialPort != null)
            {
                var command = new byte[] { 0xff, 0x01, 0x86, 0x00, 0x00, 0x00, 0x00, 0x00, 0x79 };
                var readBuf = new byte[9];
                serialPort.Write(command, 0, command.Length);
                var readLen = serialPort.Read(readBuf, 0, readBuf.Length);
                while (readLen < readBuf.Length)
                {
                    if (readLen > 0)
                    {
                        if (readBuf[0] == command[0])
                        {
                            readLen += serialPort.Read(readBuf, readLen, readBuf.Length - readLen);
                        }
                    }
                }
                if (readLen == readBuf.Length)
                {
                    if (readBuf[0] == command[0] && readBuf[1] == command[2])
                        co2 = readBuf[2] * 256 + readBuf[3];
                }
            }
            return new List<(SensorType, double)>() { (SensorType.CarbonDioxideConcentration, (double)co2) };
        }
    }
}
