using DungeonRush.Cards;
using DungeonRush.Managers;

namespace DungeonRush.Controller
{
    public class GoblinController : AIController
    {
        public enum State { MOVE, ATTACK1, ATTACK2};
        public State state = State.ATTACK1;

        protected override void ChangeState()
        {
            if (statusAct.anger)
            {
                state = State.ATTACK2;
                exclamation.SetActive(false);
                return;
            }

            switch (state)
            {
                case State.MOVE:
                    state = State.ATTACK1;
                    exclamation.SetActive(true);
                    break;
                case State.ATTACK1:
                    exclamation.SetActive(true);
                    state = State.ATTACK2;
                    break;
                case State.ATTACK2:
                    exclamation.SetActive(false);
                    state = State.MOVE;
                    break;
                default:
                    break;
            }
        }

        protected override void ChooseController()
        {
            EnemyController.subscribedEnemies.Add(this);
        }

        protected override void Notify()
        {
            if (card.InstantMoveCount > 0)
            {
                Run();
                card.InstantMoveCount--;
            }
            else
            {
                card.InstantMoveCount = card.TotalMoveCount;
                MoveSchedular.Instance.enemyController.OnNotify();
            }
        }

        protected override Swipe SelectTileForSwipe(Card card)
        {
            if ((state == State.ATTACK1 || state == State.ATTACK2) && statusAct.canAttack)
                return SelectTileToAttack(card);

            if (statusAct.canMove)
                return SelectTileToMove(card);

            return Swipe.NONE;
        }
    }
}
