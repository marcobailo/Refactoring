
// New source code Bailo Marco

namespace TDDMicroExercises.TirePressureMonitoringSystem
{

    public interface IAlarm
    {

        bool AlarmOn { get; }
        double LastPsiPressureValue { get; }

        void Check();

    }

    public class Alarm : IAlarm
    {

        private const double LowPressureThreshold = 17;
        private const double HighPressureThreshold = 21;

        private readonly ISensor _sensor;

        private double _psiPressureValue;
        private bool _alarmOn;

        public Alarm(ISensor sensor)
        {
            _alarmOn = false;
            _psiPressureValue = double.MinValue;
            _sensor = sensor;
        }

        public void Check()
        {
            _psiPressureValue = _sensor.PopNextPressurePsiValue();
            _alarmOn = _psiPressureValue < LowPressureThreshold || _psiPressureValue > HighPressureThreshold;
        }

        public bool AlarmOn
        {
            get { return _alarmOn; }
        }

        public double LastPsiPressureValue { get { return _psiPressureValue; } private set { } }

    }

}

// old source code
/*
namespace TDDMicroExercises.TirePressureMonitoringSystem
{
    public class Alarm
    {
        private const double LowPressureThreshold = 17;
        private const double HighPressureThreshold = 21;

        readonly Sensor _sensor = new Sensor();

        bool _alarmOn = false;

        public void Check()
        {
            double psiPressureValue = _sensor.PopNextPressurePsiValue();

            if (psiPressureValue < LowPressureThreshold || HighPressureThreshold < psiPressureValue)
            {
                _alarmOn = true;
            }
        }

        public bool AlarmOn
        {
            get { return _alarmOn; }
        }

    }
}
*/
