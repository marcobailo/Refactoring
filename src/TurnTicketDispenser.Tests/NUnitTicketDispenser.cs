using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using NUnit.Framework;
using TDDMicroExercises.TurnTicketDispenser;
using TDDMicroExercises.TurnTicketDispenser.SomeDependencies;

namespace TestProject
{

    [TestFixture]
    class NUnitTicketDispenser
    {

        private IDispenserClient _dispenserClient = null;
        private int _clientSimulationDuration;

        private void OnTicketRequestedFromQueue(object sender, DispenserTicketRequestEventArgs e)
        {
            Debug.WriteLine(string.Format("Ticket number: {0}, request from the queue: {1}", e.TicketNumber, e.QueueNumber));
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
            _dispenserClient = new DispenserClient(new TurnNumberSequence(), 3, simulationTimeInSec: _clientSimulationDuration);
            _dispenserClient.TicketRequestedFromQueue += OnTicketRequestedFromQueue;
            _dispenserClient.ClientSimulationStarted += OnSimulationStarted;
            _dispenserClient.ClientSimulationTerminated += OnSimulationTerminated;
            _dispenserClient.ClientSimulationInterrupted += OnSimulationInterrupted;
        }

        [Test]
        public void ExecuteSimulation()
        {
            var _are = new AutoResetEvent(false);
            _dispenserClient.StartSimulation();
            _are.WaitOne(timeout: TimeSpan.FromSeconds(_clientSimulationDuration + 1));
            Assert.IsTrue(_dispenserClient.CurrentStatus == SimulationStatuses.Terminated);
        }

        [Test]
        public void ShowSimulationSummary()
        {
            bool _hasAValidQueue = false;
            var _are = new AutoResetEvent(false);
            _dispenserClient.StartSimulation();
            _are.WaitOne(timeout: TimeSpan.FromSeconds(_clientSimulationDuration + 1));
            if (_hasAValidQueue = _dispenserClient.CurrentStatus == SimulationStatuses.Terminated && _dispenserClient.QueuesTicketsSummary != null && _dispenserClient.QueuesTicketsSummary.Any())
            {
                foreach (KeyValuePair<int, List<int>> _queueTickets in _dispenserClient.QueuesTicketsSummary)
                {
                    Debug.Write("Queue number: " + _queueTickets.Key.ToString() + " Tickets count: " + _queueTickets.Value.Count.ToString());
                    if (_queueTickets.Value.Count > 0)
                    {
                        Debug.Write(" (");
                        int _current = 1;
                        foreach (int _ticket in _queueTickets.Value)
                        {
                            if (_current == _queueTickets.Value.Count)
                                Debug.WriteLine(_ticket.ToString() + ")");
                            else
                                Debug.Write(_ticket.ToString() + ", ");
                            _current++;
                        }
                    }
                }
            }
            Assert.IsTrue(_hasAValidQueue);
        }

        [Test]
        public void InterruptSimulation()
        {
            var _are = new AutoResetEvent(false);
            _dispenserClient.StartSimulation();
            _are.WaitOne(timeout: TimeSpan.FromSeconds(_clientSimulationDuration / 2));
            _dispenserClient.InterruptSimulation();
            Assert.IsTrue(_dispenserClient.CurrentStatus == SimulationStatuses.Interrupted);
        }

    }

}
