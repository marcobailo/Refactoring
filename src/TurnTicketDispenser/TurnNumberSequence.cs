// new source Marco Bailo
namespace TDDMicroExercises.TurnTicketDispenser
{

    public interface ITurnNumberSequence
    {

        int GetNextTurnNumber();

    }

    public class TurnNumberSequence : ITurnNumberSequence
    {

        private static int _turnNumber;
        private static int _offset = 1;
        private static readonly object _lockObj;

        static TurnNumberSequence()
        {
            _lockObj = new object();
            _turnNumber = 0;
            _offset = 1;
        }

        public TurnNumberSequence(int offSet = 1)
        {
            _offset = offSet;
        }

        public int GetNextTurnNumber()
        {
            lock (_lockObj)
            {
                _turnNumber += _offset;
                return _turnNumber;
            }
        }

    }

}

// old source...
/* 

namespace TDDMicroExercises.TurnTicketDispenser
{
    public static class TurnNumberSequence
    {
        private static int _turnNumber = 0;

        public static int GetNextTurnNumber()
        {
            return _turnNumber++;
        }
    }
}

*/
