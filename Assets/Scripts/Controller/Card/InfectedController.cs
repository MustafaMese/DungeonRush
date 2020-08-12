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

        private EnemyController enemyController;

        protected override void Initialize()
        {
            base.Initialize();
            exclamation.SetActive(false);
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

        protected override Swipe SelectTileToAttack(Card attacker)
        {
            if (state == State.WAIT) return Swipe.NONE;

            List<Tile> list;
            Dictionary<Tile, Swipe> tiles;

            if (state == State.ATTACK)
            {
                tiles = card.GetAttackStyle().GetAvaibleTiles(card);
                list = new List<Tile>(tiles.Keys);
                for (int i = 0; i < list.Count; i++)
                {
                    Tile t = list[i];
                    if (t.GetCard() != null && attacker.GetCharacterType().IsEnemy(t.GetCard().GetCharacterType()))
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
        protected override void ChangeState()
        {
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