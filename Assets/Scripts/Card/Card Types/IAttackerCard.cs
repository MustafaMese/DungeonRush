
using DungeonRush.Attacking;
using DungeonRush.Data;

namespace DungeonRush.Cards
{
    public interface IAttackerCard
    {
        AttackStyle GetAttackStyle();
        void ExecuteAttack();
        bool CanAttack(Card enemy);
        Move GetMove();
        void SetMove(Move move);
    }
}

