// new source code Bailo Marco
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace TDDMicroExercises.TirePressureMonitoringSystem.SomeDependencies
{

    public enum SimulationStatuses : int
    {
        Undefined,
        Started,
        Terminated,
        Interrupted
    };

    public interface IAlarmClient
    {

        event EventHandler<EventArgs> ClientSimulationStarted;
        event EventHandler<EventArgs> ClientSimulationTerminated;
        event EventHandler<EventArgs> ClientSimulationInterrupted;
        event EventHandler<CurrentPsiValueEventArgs> PsiValueRead;
        event EventHandler<EventArgs> AlarmOccurred;

        SimulationStatuses CurrentStatus { get; }

        Dictionary<DateTime, double> PressuresHistory { get; }

        void StartSimulation();

        void InterruptSimulation();

    }

    public interface IDecoratorAlarmClient
    {

        double GetFurtherInfo();

    }

    public class CurrentPsiValueEventArgs : EventArgs
    {

        public double CurrentPsiValue { get; private set; }

        public CurrentPsiValueEventArgs(double currentPsiValue) : base()
        {
            CurrentPsiValue = currentPsiValue;
        }

    }

    public class AlarmClient : IAlarmClient
    {

        private readonly IAlarm _alarm;
        private readonly int _simulationTimeInSec;
        private readonly Timer _simulationTimer;
        private DateTime _timeLimit;
        private SimulationStatuses _simulationStatus;
        private Dictionary<DateTime, double> _pressuresHistory;

        public event EventHandler<EventArgs> ClientSimulationStarted;
        public event EventHandler<EventArgs> ClientSimulationTerminated;
        public event EventHandler<EventArgs> ClientSimulationInterrupted;
        public event EventHandler<CurrentPsiValueEventArgs> PsiValueRead;
        public event EventHandler<EventArgs> AlarmOccurred;

        public SimulationStatuses CurrentStatus { get { return _simulationStatus; } private set { } }
        public Dictionary<DateTime, double> PressuresHistory { get { return _pressuresHistory; } private set { } }

        private void SimulationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (e.SignalTime <= _timeLimit)
            {
                _alarm.Check();
                _pressuresHistory.Add(DateTime.Now, _alarm.LastPsiPressureValue);
                this.PsiValueRead?.Invoke(sender, new CurrentPsiValueEventArgs(_alarm.LastPsiPressureValue));
                if (_alarm.AlarmOn)
                    this.AlarmOccurred?.Invoke(sender, new EventArgs());
            }
            else
                CloseSimulation(false);
        }

        private void CloseSimulation(bool isInterruption)
        {
            _simulationTimer.Enabled = false;
            _simulationTimer.Stop();
            if (isInterruption)
            {
                this.ClientSimulationInterrupted?.Invoke(this, new EventArgs());
                _simulationStatus = SimulationStatuses.Interrupted;
            }
            else
            {
                this.ClientSimulationTerminated?.Invoke(this, new EventArgs());
                _simulationStatus = SimulationStatuses.Terminated;
            }
        }

        public AlarmClient(IAlarm alarm, int simulationTimeInSec = 10, int interval = 100)
        {
            _simulationStatus = SimulationStatuses.Undefined;
            _alarm = alarm;
            _simulationTimeInSec = simulationTimeInSec;
            _simulationTimer = new Timer(interval);
            _simulationTimer.AutoReset = true;
            _simulationTimer.Enabled = false;
            _simulationTimer.Elapsed += SimulationTimer_Elapsed;
        }

        public void StartSimulation()
        {
            _simulationStatus = SimulationStatuses.Started;
            _pressuresHistory = new Dictionary<DateTime, double>();
            _timeLimit = DateTime.Now.AddSeconds(_simulationTimeInSec);
            _simulationTimer.Enabled = true;
            _simulationTimer.Start();
            this.ClientSimulationStarted?.Invoke(this, new EventArgs());
        }

        public void InterruptSimulation()
        {
            CloseSimulation(true);
        }

    }

    public class AvgPressureAlarmClient : IDecoratorAlarmClient
    {

        private IAlarmClient _alarmClient;

        public AvgPressureAlarmClient(IAlarmClient alarmClient)
        {
            _alarmClient = alarmClient;
        }

        public double GetFurtherInfo()
        {
            double _avgPressure = double.MinValue;
            if (_alarmClient.PressuresHistory != null && _alarmClient.PressuresHistory.Any())
                _avgPressure = _alarmClient.PressuresHistory.Values.Where(_flt => _flt > 0).Sum() / _alarmClient.PressuresHistory.Values.Count;
            return _avgPressure;
        }

    }

    public class MaxPressureAlarmClient : IDecoratorAlarmClient
    {

        private IAlarmClient _alarmClient;

        public MaxPressureAlarmClient(IAlarmClient alarmClient)
        {
            _alarmClient = alarmClient;
        }

        public double GetFurtherInfo()
        {
            double _maxPressure = double.MinValue;
            if (_alarmClient.PressuresHistory != null && _alarmClient.PressuresHistory.Any())
                _maxPressure = _alarmClient.PressuresHistory.Values.Where(_flt => _flt > 0).Max();
            return _maxPressure;
        }

    }

    public class MinPressureAlarmClient : IDecoratorAlarmClient
    {

        private IAlarmClient _alarmClient;

        public MinPressureAlarmClient(IAlarmClient alarmClient)
        {
            _alarmClient = alarmClient;
        }

        public double GetFurtherInfo()
        {
            double _minPressure = double.MinValue;
            if (_alarmClient.PressuresHistory != null && _alarmClient.PressuresHistory.Any())
                _minPressure = _alarmClient.PressuresHistory.Values.Where(_flt => _flt > 0).Min();
            return _minPressure;
        }

    }

}

// old source code
/*
using System;
namespace TDDMicroExercises.TirePressureMonitoringSystem.SomeDependencies
{
    public class ASensorClient      
    {
        // A class with the only goal of simulating a dependency on Sensor
        // that has impact on the refactoring.

        public ASensorClient()
        {
            Sensor sensor = new Sensor();

			double value = sensor.PopNextPressurePsiValue();
			value = sensor.PopNextPressurePsiValue();
			value = sensor.PopNextPressurePsiValue();
			value = sensor.PopNextPressurePsiValue();
		}
    }
}
*/
