// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Kae.IoT.Device.Sensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kae.IoT.Device.RaspberryPi.Sensors
{
    public class GrovePiLightSensor
    {
        public string SensorName { get { return "GrovePiLight"; } }
        GrovePiPlus grovePiPlusDevice;
        GrovePiPlus.Pin sensorPin;

        public GrovePiLightSensor(GrovePiPlus device, int sensorPin)
        {
            this.grovePiPlusDevice = device;
            this.sensorPin = (GrovePiPlus.Pin)sensorPin;
        }

        public bool Initalize()
        {
            bool succeeded = true;
            try
            {
                grovePiPlusDevice.SetPinMode(sensorPin, GrovePiPlus.PinMode.Input);
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
            double sensorValue = grovePiPlusDevice.AnalogRead(sensorPin);
            double brightness = (double)(1023 - sensorValue) * 10 / sensorValue;

            return new List<(SensorType, double)>() { (SensorType.Brightness, brightness) };
        }
    }
}
