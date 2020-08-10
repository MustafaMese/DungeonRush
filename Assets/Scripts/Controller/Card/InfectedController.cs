﻿using System.Collections.Generic;
using System.Linq;
using DungeonRush.Cards;
using DungeonRush.Field;
using DungeonRush.Managers;
using UnityEngine;

namespace DungeonRush.Controller
{
    public class InfectedController : AIController
    {
        private enum State { WAIT, ATTACK, MOVE};
        private State state = State.WAIT;

        private EnemyController enemyController;

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
                    print("Ben bi zombiyim ve bekleyeceğim");
                    state = State.MOVE;
                    break;
                case State.ATTACK:
                    print("Ben bi zombiyim ve hareket edicem");
                    state = State.WAIT;
                    break;
                case State.MOVE:
                    print("Ben bi zombiyim ve saldırıcam");
                    state = State.ATTACK;
                    break;
                default:
                    break;
            }
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