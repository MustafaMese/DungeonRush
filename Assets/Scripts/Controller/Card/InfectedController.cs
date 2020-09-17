using System.Collections.Generic;
using System.Linq;
using DungeonRush.Cards;
using DungeonRush.Field;
using DungeonRush.Managers;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class InfectedController : AIController
    {
        public enum State { WAIT, ATTACK, MOVE};
        public State state = State.WAIT;

        protected override void Initialize()
        {
            base.Initialize();
            exclamation.SetActive(false);
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
            if (state == State.WAIT) return Swipe.NONE;

            if (state == State.ATTACK && statusAct.canAttack)
                return SelectTileToAttack(card);

            if (statusAct.canMove)
                return SelectTileToMove(card);

            return Swipe.NONE;
        }
        protected override void ChangeState()
        {
            if (statusAct.anger)
            {
                state = State.ATTACK;
                exclamation.SetActive(false);
                return;
            }

            switch (state)
            {
                case State.WAIT:
                    state = State.MOVE;
                    break;
                case State.ATTACK:
                    exclamation.SetActive(false);
                    state = State.WAIT;
                    break;
                case State.MOVE:
                    exclamation.SetActive(true);
                    state = State.ATTACK;
                    break;
                default:
                    break;
            }
        }
        
    }
}