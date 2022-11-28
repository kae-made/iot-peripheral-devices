// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kae.IoT.Device.RaspberryPi
{
    public class GrovePiPlus : IDisposable
    {
        public enum PinMode
        {
            Input = 0,
            Output = 1
        }

        public enum Pin
        {
            AnalogPin0 = 0,
            AnalogPin1 = 1,
            AnalogPin2 = 2,
            DigitalPin2 = 2,
            DigitalPin3 = 3,
            DigitalPin4 = 4,
            DigitalPin5 = 5,
            DigitalPin6 = 6,
            DigitalPin7 = 7,
            DigitalPin8 = 8
        }

        private enum Command
        {
            DigitalRead = 1,
            DigitalWrite = 2,
            AnalogRead = 3,
            AnalogWrite = 4,
            PinMode = 5,
            Version = 8
        };

        I2cDevice grovePiPlusDevice;
        bool initailzed = false;

        public bool Initialize(int busId)
        {
            bool succeeded = true;
            try
            {
                var i2cConnectionString = new I2cConnectionSettings(busId, 0x04);
                grovePiPlusDevice = I2cDevice.Create(i2cConnectionString);
                initailzed = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                succeeded = false;
            }
            return succeeded;
        }

        public void DigitalWrite(Pin pin, byte value)
        {
            var buffer = new byte[4] { (byte)Command.DigitalWrite, (byte)pin, value, 0x00 };
            grovePiPlusDevice.Write(buffer);
            Thread.Sleep(10);
        }

        public byte DigitalRead(Pin pin)
        {
            var wbuffer = new byte[4] { (byte)Command.DigitalRead, (byte)pin, 0x00, 0x00 };
            var rBuffer = new byte[1];
            grovePiPlusDevice.Write(wbuffer);
            Thread.Sleep(2);
            var value = grovePiPlusDevice.ReadByte();

            return value;
        }
        public int DigitalRead2(Pin pin)
        {
            var wbuffer = new byte[4] { (byte)Command.DigitalRead, (byte)pin, 0x00, 0x00 };
            var rbuffer = new byte[3];
            grovePiPlusDevice.WriteRead(wbuffer, rbuffer);
            Thread.Sleep(10);
            return (int)(rbuffer[1]) * 256 + (int)rbuffer[2];

        }

        public int AnalogRead(Pin pin)
        {
            var wbuffer = new byte[4] { (byte)Command.AnalogRead, (byte)pin, 0x00, 0x00 };
            var rbuffer = new byte[3];
            grovePiPlusDevice.WriteRead(wbuffer, rbuffer);
            Thread.Sleep(10);
            return ((int)rbuffer[1]) * 256 + (int)rbuffer[2];
        }

        public void AnalogWrite(Pin pin, byte value)
        {
            var buffer = new byte[4] { (byte)Command.AnalogWrite, (byte)pin, value, 0x00 };
            grovePiPlusDevice.Write(buffer);
            Thread.Sleep(10);
        }

        public void SetPinMode(Pin pin, PinMode mode)
        {
            var buffer = new byte[4] { (byte)Command.PinMode, (byte)pin, (byte)mode, 0x00 };
            Thread.Sleep(10);
        }

        public void Dispose()
        {
            grovePiPlusDevice?.Dispose();
        }
    }
}
