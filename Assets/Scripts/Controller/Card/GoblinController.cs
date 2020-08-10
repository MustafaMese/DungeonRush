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
        private enum State { MOVE, ATTACK1, ATTACK2};
        private State state = State.ATTACK1;

        private EnemyController enemyController;

        protected override void ChangeState()
        {
            switch (state)
            {
                case State.MOVE:
                    print("Ben bir goblinim ve ilerleyeceğim");
                    state = State.ATTACK1;
                    break;
                case State.ATTACK1:
                    print("Ben bir goblinim ve saldıracağım");
                    state = State.ATTACK2;
                    break;
                case State.ATTACK2:
                    print("Ben bir goblinim ve saldıracağim.");
                    //state = State.MOVE;
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
            List<Tile> list = new List<Tile>(tiles.Keys);
            if (state == State.ATTACK1 || state == State.ATTACK2)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Tile t = list[i];
                    print("1");
                    if (t.GetCard() != null && attacker.GetCharacterType().IsEnemy(t.GetCard().GetCharacterType()))
                    {
                        return tiles[t];
                    }
                }
            }

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

        private int GiveRandomEncounter(List<Tile> list, int count)
        {
            int number = Random.Range(0, count);

            if (!list[number].IsTileOccupied())
                return number;

            for (int i = 0; i < list.Count; i++)
            {
                if (i == number) continue;

                Tile t = list[i];
                if (!t.IsTileOccupied())
                    return i;
            }

            return -1;
        }
    }
}
