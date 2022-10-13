namespace TDDMicroExercises.TurnTicketDispenser
{

    // new source Bailo Marco

    public interface ITurnTicket
    {

        int TurnNumber { get; }

    }

    public class TurnTicket : ITurnTicket
    {

        private readonly int _turnNumber;

        public TurnTicket(int turnNumber)
        {
            _turnNumber = turnNumber;
        }

        public int TurnNumber
        {
            get { return _turnNumber; }
        }

    }

    // old source
    /*
    public class TurnTicket
    {
        private readonly int _turnNumber;

        public TurnTicket(int turnNumber)
        {
            _turnNumber = turnNumber;
        }

        public int TurnNumber
        {
            get { return _turnNumber; }
        }

    }
    */
}