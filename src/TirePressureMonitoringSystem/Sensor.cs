//new source code Bailo Marco
using System;

namespace TDDMicroExercises.TirePressureMonitoringSystem
{

    public interface ISensor
    {

        double PopNextPressurePsiValue();

    }

    public class Sensor : ISensor
    {

        private const double Offset = 16;
        private readonly Random _randomPressureSampleSimulator;

        public Sensor()
        {
            _randomPressureSampleSimulator = new Random();
        }

        public double PopNextPressurePsiValue()
        {
            double _pressureTelemetryValue = ReadPressureSample();
            return Offset + _pressureTelemetryValue;
        }

        private double ReadPressureSample()
        {
            // Simulate info read from a real sensor in a real tire
            return 6 * _randomPressureSampleSimulator.NextDouble() * _randomPressureSampleSimulator.NextDouble();
        }

    }

}

// old source code
/*
using System;

namespace TDDMicroExercises.TirePressureMonitoringSystem
{
    public class Sensor
    {
        //
        // The reading of the pressure value from the sensor is simulated in this implementation.
        // Because the focus of the exercise is on the other class.
        //

        const double Offset = 16;
        readonly Random _randomPressureSampleSimulator = new Random();

        public double PopNextPressurePsiValue()
        {
            double pressureTelemetryValue = ReadPressureSample();

            return Offset + pressureTelemetryValue;
        }

        private double ReadPressureSample()
        {
            // Simulate info read from a real sensor in a real tire
            return 6 * _randomPressureSampleSimulator.NextDouble() * _randomPressureSampleSimulator.NextDouble();
        }
    }
}
*/
