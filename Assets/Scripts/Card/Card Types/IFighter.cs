
using DungeonRush.Attacking;

namespace DungeonRush.Cards
{
    public interface IFighter
    {
        AttackStyle GetAttackStyle();
        void ExecuteAttack();
        bool CanAttack(Card enemy);
    }
}

