
// class utilized to store the NUnit test created for the execise (Bailo Marco)
using System;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using TDDMicroExercises.TirePressureMonitoringSystem.SomeDependencies;

namespace TDDMicroExercises.TirePressureMonitoringSystem.Tests
{

    [TestFixture]
    class NUnitTestPressure
    {

        private IAlarmClient _alarmClient = null;
        private int _clientSimulationDuration;

        private void OnPsiValueRead(object sender, CurrentPsiValueEventArgs e)
        {
            Debug.WriteLine("Current Psi value read: " + e.CurrentPsiValue.ToString());
        }

        private void OnAlarmOccurred(object sender, EventArgs e)
        {
            Debug.WriteLine("Psi pressure alarm occurred!");
        }

        private void OnSimulationInterrupted(object sender, EventArgs e)
        {
            Debug.WriteLine("Simulation interrupted! (" + DateTime.Now.ToLongTimeString() + ")");
        }

        private void OnSimulationTerminated(object sender, EventArgs e)
        {
            Debug.WriteLine("Simulation terminated. (" + DateTime.Now.ToLongTimeString() + ")");
        }

        private void OnSimulationStarted(object sender, EventArgs e)
        {
            Debug.WriteLine("Simulation started... (" + DateTime.Now.ToLongTimeString() + ")");
        }

        [SetUp]
        public void Setup()
        {
            _clientSimulationDuration = 12;
            _alarmClient = new AlarmClient(new Alarm(new Sensor()), simulationTimeInSec: _clientSimulationDuration);
            _alarmClient.AlarmOccurred += OnAlarmOccurred;
            _alarmClient.PsiValueRead += OnPsiValueRead;
            _alarmClient.ClientSimulationStarted += OnSimulationStarted;
            _alarmClient.ClientSimulationTerminated += OnSimulationTerminated;
            _alarmClient.ClientSimulationInterrupted += OnSimulationInterrupted;
        }

        [Test]
        public void ExecuteSimulation()
        {
            var _are = new AutoResetEvent(false);
            _alarmClient.StartSimulation();
            _are.WaitOne(timeout: TimeSpan.FromSeconds(_clientSimulationDuration + 1));
            Assert.IsTrue(_alarmClient.CurrentStatus == SimulationStatuses.Terminated);
        }

        [Test]
        public void InterruptSimulation()
        {
            var _are = new AutoResetEvent(false);
            _alarmClient.StartSimulation();
            _are.WaitOne(timeout: TimeSpan.FromSeconds(_clientSimulationDuration / 2));
            _alarmClient.InterruptSimulation();
            Assert.IsTrue(_alarmClient.CurrentStatus == SimulationStatuses.Interrupted);
        }

        [Test]
        public void MaxPressureInSimulation()
        {
            var _are = new AutoResetEvent(false);
            _alarmClient.StartSimulation();
            _are.WaitOne(timeout: TimeSpan.FromSeconds(_clientSimulationDuration + 1));
            double _furtherInfo = new MaxPressureAlarmClient(_alarmClient).GetFurtherInfo();
            Debug.WriteLine("Maximum pressure detected: " + _furtherInfo.ToString());
            Assert.IsTrue(_furtherInfo != double.MinValue);
        }

        [Test]
        public void MinPressureInSimulation()
        {
            var _are = new AutoResetEvent(false);
            _alarmClient.StartSimulation();
            _are.WaitOne(timeout: TimeSpan.FromSeconds(_clientSimulationDuration + 1));
            double _furtherInfo = new MinPressureAlarmClient(_alarmClient).GetFurtherInfo();
            Debug.WriteLine("Minimum pressure detected: " + _furtherInfo.ToString());
            Assert.IsTrue(_furtherInfo != double.MinValue);

        }

        [Test]
        public void AveragePressureInSimulation()
        {
            var _are = new AutoResetEvent(false);
            _alarmClient.StartSimulation();
            _are.WaitOne(timeout: TimeSpan.FromSeconds(_clientSimulationDuration + 1));
            double _furtherInfo = new AvgPressureAlarmClient(_alarmClient).GetFurtherInfo();
            Debug.WriteLine("Average pressure detected: " + _furtherInfo.ToString());
            Assert.IsTrue(_furtherInfo != double.MinValue);
        }

    }

}

