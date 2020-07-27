using DungeonRush.Data;
using DungeonRush.Shifting;

namespace DungeonRush.Property
{
    public interface IMover
    {
        void Move();
        Move GetMove();
        void SetMove(Move move);
        Shift GetShift();
        bool IsMoveFinished();
        void SetIsMoveFinished(bool b);
    }
}
