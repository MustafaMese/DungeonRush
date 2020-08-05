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
                    print("Ben bi yarasayım ve hareket edicem");
                    state = State.MOVE;
                    break;
                case State.MOVE:
                    print("Ben bi yarasayım ve saldırıcam");
                    state = State.ATTACK;
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

        protected override Swipe SelectTileToAttack(Dictionary<Tile, Swipe> tiles, Card attacker)
        {
            if (state == State.ATTACK)
            {
                List<Tile> list = new List<Tile>(tiles.Keys);
                for (int i = 0; i < list.Count; i++)
                {
                    Tile t = list[i];
                    if (t.GetCard() != null && attacker.GetCharacterType().IsEnemy(t.GetCard().GetCharacterType()))
                    {
                        return tiles[t];
                    }
                }
            }

            var number = tiles.Count;
            number = Random.Range(0, number);

            List<Tile> keys = Enumerable.ToList(tiles.Keys);

            if (keys.Count <= 0 || (state == State.MOVE && keys[number].IsTileOccupied()))
                return Swipe.NONE;
            else
            {
                Tile tile = keys[number];
                return tiles[tile];
            }
        }
    }
}
