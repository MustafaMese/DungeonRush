using UnityEngine;
using DG.Tweening;
using DungeonRush.Cards;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Data;
using DungeonRush.Skills;
using DungeonRush.Shifting;
using System.Collections;
using DungeonRush.Items;

namespace DungeonRush 
{
    namespace Property
    {
        public class PlayerMover : Mover
        {
            public bool isSkillUser = false;
            private SkillUser skillUser = null;
            
            protected override void Initialize()
            {
                if (isSkillUser)
                    skillUser = GetComponent<SkillUser>();
            }

            public override void Move()
            {
                if (move.GetCard() == null)
                    move = card.GetMove();

                if (isSkillUser)
                    skillUser.ExecuteMoverSkills();

                Vector3 pos = move.GetCardTile().GetCoordinate();

                UpdateAnimation(true);
                StartCoroutine(StartMoveAnimation(pos, particulTime));
                move.GetCard().transform.DOMove(move.GetTargetTile().GetCoordinate(), movingTime).OnComplete(() => TerminateMove());
            }

            private void TerminateMove()
            {
                UpdateAnimation(false);
                move.GetCard().transform.position = move.GetTargetTile().GetCoordinate();
                MoveType moveType = move.GetMoveType();
                Card item = move.GetTargetTile().GetCard();
                switch (moveType)
                {
                    case MoveType.ITEM:
                        ItemMove(move.GetCard(), item);
                        ChangeTiles(true);
                        break;
                    case MoveType.COIN:
                        CoinMove(item);
                        ChangeTiles(false);
                        break;
                    case MoveType.EMPTY:
                        ChangeTiles(true);
                        break;
                    default:
                        break;
                }

                isMoveFinished = true;
                move.Reset();
            }

            private void ChangeTiles(bool isEmpty)
            {
                Tile.ChangeTile(move, isEmpty, true);
            }

            private void CoinMove(Card item)
            {
                FindObjectOfType<CoinCounter>().IncreaseCoin(item.GetHealth());
            }

            private void ItemMove(Card card, Card item)
            {
                Item i = item.GetComponent<Item>();
                card.GetComponent<ItemUser>().TakeItem(i);

            }
            
        }
    }
}

