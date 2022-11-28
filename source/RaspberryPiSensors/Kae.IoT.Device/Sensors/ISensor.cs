// Copyright (c) Knowledge & Experience. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kae.IoT.Device.Sensors
{
    public enum SensorType
    {
        Temperature,
        Humidity,
        Pressure,
        AccelerometerX,
        AccelerometerY,
        AccelerometerZ,
        Brightness,
        CarbonDioxideConcentration
    }

    public interface ISensor
    {
        string SensorName { get; }
        public bool Initalize();

        public IList<(SensorType SensorType, double Value)> Read();

    }
}
