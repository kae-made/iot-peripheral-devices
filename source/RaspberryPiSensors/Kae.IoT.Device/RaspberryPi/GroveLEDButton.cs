// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kae.IoT.Device.RaspberryPi
{
    public class GroveLEDButton : IDisposable
    {
        GrovePiPlus grovePiPlusDevice;
        GrovePiPlus.Pin ledPin;
        GrovePiPlus.Pin buttonPin;

        public GroveLEDButton(GrovePiPlus shield, int ledPin, int buttonPin)
        {
            grovePiPlusDevice = shield;


            this.ledPin = (GrovePiPlus.Pin)ledPin;
            this.buttonPin = (GrovePiPlus.Pin)buttonPin;
        }

        public bool Initialize()
        {
            bool succeeded = true;
            try
            {
                grovePiPlusDevice.SetPinMode(this.ledPin, GrovePiPlus.PinMode.Output);
                grovePiPlusDevice.SetPinMode(this.buttonPin, GrovePiPlus.PinMode.Input);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                succeeded = false;
            }
            return succeeded;
        }

        public void TurnOn()
        {
            grovePiPlusDevice.DigitalWrite(this.ledPin, 1);
        }
        public void TurnOff()
        {
            grovePiPlusDevice.DigitalWrite(this.ledPin, 0);
        }

        public PinValue ReadButtonStatus()
        {
            //var status = grovePiPlusDevice.DigitalRead(this.buttonPin);
            var status = grovePiPlusDevice.DigitalRead2(this.buttonPin);
            return status;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
