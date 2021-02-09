using DungeonRush.Data;
using DungeonRush.Shifting;

namespace DungeonRush.Cards
{
    public interface IMovable
    {
        void SetIsMoveFinished(bool b);
        bool IsMoveFinished();
        Shift GetShift();
        void ExecuteMove();
        Move GetMove();
        void SetMove(Move move);
    }
}

