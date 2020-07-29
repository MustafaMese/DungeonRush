using DungeonRush.Data;
using DungeonRush.Shifting;

namespace DungeonRush.Property
{
    public interface IMover
    {
        void Move();
        Shift GetShift();
        bool IsMoveFinished();
        void SetIsMoveFinished(bool b);
    }
}
