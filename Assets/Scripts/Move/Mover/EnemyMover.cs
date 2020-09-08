using DG.Tweening;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Shifting;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Property
{
    public class EnemyMover : Mover
    {
        public override void Move()
        {
            if (move.GetCard() == null)
                move = card.GetMove();

            Vector3 pos = move.GetCardTile().GetCoordinate();
            UpdateAnimation(true);
            StartCoroutine(StartMoveAnimation(pos, particulTime));
            move.GetCard().transform.DOMove(move.GetTargetTile().GetCoordinate(), movingTime).OnComplete(() => TerminateMove());
        }

        private void TerminateMove()
        {
            UpdateAnimation(false);
            move.GetCard().transform.position = move.GetTargetTile().GetCoordinate();
            Tile.ChangeTile(move, true, false);
            isMoveFinished = true;
            move.Reset();
        }

    }
}
