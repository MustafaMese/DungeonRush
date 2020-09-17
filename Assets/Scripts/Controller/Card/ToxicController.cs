using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DungeonRush.Cards;
using DungeonRush.Field;
using DungeonRush.Managers;
using UnityEngine;

namespace DungeonRush.Controller {
    public class ToxicController : AIController
    {
        private enum State { ATTACK, MOVE};
        private State state = State.MOVE;

        private EnemyController enemyController;

        protected override void ChangeState()
        {
            switch (state)
            {
                case State.ATTACK:
                    exclamation.SetActive(false);
                    state = State.MOVE;
                    break;
                case State.MOVE:
                    exclamation.SetActive(true);
                    state = State.ATTACK;
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
            MoveSchedular.Instance.enemyController.OnNotify();
        }

        protected override Swipe SelectTileForSwipe(Card attacker)
        {
            if (state == State.ATTACK && statusAct.canAttack)
                return SelectTileToAttack(card);

            if (statusAct.canMove)
                return SelectTileToMove(card);

            return Swipe.NONE;
        }
    }
}
