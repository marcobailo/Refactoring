// new source code Marco Bailo
using System;
using System.Collections.Generic;
using System.Timers;

namespace TDDMicroExercises.TurnTicketDispenser.SomeDependencies
{

    public enum SimulationStatuses : int
    {
        Undefined,
        Started,
        Terminated,
        Interrupted
    };

    public class DispenserTicketRequestEventArgs : EventArgs
    {

        public int QueueNumber { get; private set; }
        public int TicketNumber { get; private set; }

        public DispenserTicketRequestEventArgs(int queueNumber, int ticketNumber) : base()
        {
            QueueNumber = queueNumber;
            TicketNumber = ticketNumber;
        }

    }

    public interface IDispenserClient
    {

        event EventHandler<EventArgs> ClientSimulationStarted;
        event EventHandler<EventArgs> ClientSimulationTerminated;
        event EventHandler<EventArgs> ClientSimulationInterrupted;
        event EventHandler<DispenserTicketRequestEventArgs> TicketRequestedFromQueue;

        SimulationStatuses CurrentStatus { get; }

        Dictionary<int, List<int>> QueuesTicketsSummary { get; }

        void StartSimulation();

        void InterruptSimulation();

    }

    public class DispenserClient : IDispenserClient
    {

        private readonly int _queueCount;
        private readonly int _simulationTimeInSec;
        private readonly Timer _simulationTimer;
        private readonly Random _randomQueue;
        private DateTime _timeLimit;
        private SimulationStatuses _simulationStatus;
        private ITurnNumberSequence _turnNumberSequence;
        private static Dictionary<int, List<int>> _queuesTickets;

        public SimulationStatuses CurrentStatus { get { return _simulationStatus; } private set { } }
        public Dictionary<int, List<int>> QueuesTicketsSummary { get { return _queuesTickets; } private set { } }

        public event EventHandler<EventArgs> ClientSimulationStarted;
        public event EventHandler<EventArgs> ClientSimulationTerminated;
        public event EventHandler<EventArgs> ClientSimulationInterrupted;
        public event EventHandler<DispenserTicketRequestEventArgs> TicketRequestedFromQueue;

        private void SimulationTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (e.SignalTime <= _timeLimit)
            {
                int _queueApplicant = _randomQueue.Next(1, _queueCount + 1);
                int _ticketReleased = TicketDispenser.GetDispenser(_turnNumberSequence).GetTurnTicket().TurnNumber;
                _queuesTickets[_queueApplicant].Add(_ticketReleased);
                this.TicketRequestedFromQueue?.Invoke(sender, new DispenserTicketRequestEventArgs(_queueApplicant, _ticketReleased));
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

        public DispenserClient(ITurnNumberSequence turnNumberSequence, int queueCount, int simulationTimeInSec = 10, int interval = 100)
        {
            _turnNumberSequence = turnNumberSequence;
            _simulationStatus = SimulationStatuses.Undefined;
            _queuesTickets = null;
            _queueCount = queueCount;
            _randomQueue = new Random();
            _simulationTimeInSec = simulationTimeInSec;
            _simulationTimer = new Timer(interval);
            _simulationTimer.AutoReset = true;
            _simulationTimer.Enabled = false;
            _simulationTimer.Elapsed += SimulationTimer_Elapsed;
        }

        public void StartSimulation()
        {
            _simulationStatus = SimulationStatuses.Started;
            _queuesTickets = new Dictionary<int, List<int>>();
            for (int _q = 1; _q <= _queueCount; _q++)
                _queuesTickets.Add(_q, new List<int>());
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

}

// old source code
/*
using System;
namespace TDDMicroExercises.TurnTicketDispenser.SomeDependencies
{
    public class TicketDispenserClient
    {
		// A class with the only goal of simulating a dependency on TicketDispenser
		// that has impact on the refactoring.

		public TicketDispenserClient()
        {
			new TicketDispenser().GetTurnTicket();
			new TicketDispenser().GetTurnTicket();
			new TicketDispenser().GetTurnTicket();
		}
    }
}
*/
