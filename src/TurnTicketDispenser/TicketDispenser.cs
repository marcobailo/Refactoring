
// new source Marco Bailo

namespace TDDMicroExercises.TurnTicketDispenser
{

    public class TicketDispenser
    {

        private static TicketDispenser _dispenserInstance;
        private static readonly object _objLocker;
        private static ITurnNumberSequence _turnNumberSequence;

        static TicketDispenser()
        {
            _dispenserInstance = null;
            _objLocker = new object();
        }

        private TicketDispenser() { }

        public static TicketDispenser GetDispenser(ITurnNumberSequence turnNumberSequence) // singleton object
        {
            lock (_objLocker)
            {
                if (_dispenserInstance == null)
                {
                    _dispenserInstance = new TicketDispenser();
                    _turnNumberSequence = turnNumberSequence;
                }
                return _dispenserInstance;
            }
        }

        public ITurnTicket GetTurnTicket()
        {
            lock (_objLocker)
            {
                return new TurnTicket(_turnNumberSequence.GetNextTurnNumber());
            }
        }

    }

}

// old source
/*
namespace TDDMicroExercises.TurnTicketDispenser
{
    public class TicketDispenser
    {
        public TurnTicket GetTurnTicket()
        {
            int newTurnNumber = TurnNumberSequence.GetNextTurnNumber();
            var newTurnTicket = new TurnTicket(newTurnNumber);

            return newTurnTicket;
        }
    }
}
*/
