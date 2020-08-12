using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Field;
using DungeonRush.Managers;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class GoblinController : AIController
    {
        public enum State { MOVE, ATTACK1, ATTACK2};
        public State state = State.ATTACK1;

        private EnemyController enemyController;

        protected override void ChangeState()
        {
            switch (state)
            {
                case State.MOVE:
                    state = State.ATTACK1;
                    break;
                case State.ATTACK1:
                    state = State.ATTACK2;
                    break;
                case State.ATTACK2:
                    state = State.MOVE;
                    break;
                default:
                    break;
            }
        }

        protected override void ChooseController()
        {
            enemyController = FindObjectOfType<EnemyController>();
            EnemyController.subscribedEnemies.Add(this);
        }

        protected override void Notify()
        {
            enemyController.OnNotify();
        }

        protected override Swipe SelectTileToAttack(Card card)
        {
            List<Tile> list;
            Dictionary<Tile, Swipe> tiles;
            if (state == State.ATTACK1 || state == State.ATTACK2)
            {
                tiles = card.GetAttackStyle().GetAvaibleTiles(card);
                list = new List<Tile>(tiles.Keys);

                for (int i = 0; i < list.Count; i++)
                {
                    Tile t = list[i];
                    if (t.GetCard() != null && card.GetCharacterType().IsEnemy(t.GetCard().GetCharacterType()))
                    {
                        isMoving = false;
                        return tiles[t];
                    }
                }
            }

            tiles = card.GetShift().GetAvaibleTiles(card);
            list = new List<Tile>(tiles.Keys);
            isMoving = true;
            int count = tiles.Count;
            int number = GiveRandomEncounter(list, count);
            if (number != -1)
            {
                Tile t = list[number];
                return tiles[t];
            }
            else
                return Swipe.NONE;
        }
    }
}
