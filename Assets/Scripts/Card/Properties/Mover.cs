using UnityEngine;
using DG.Tweening;
using DungeonRush.Cards;
using DungeonRush.Settings;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Controller;
using DungeonRush.Data;
using DungeonRush.Skills;

namespace DungeonRush 
{
    namespace Property
    {
        public class Mover : MonoBehaviour
        {
            public bool isSkillUser;
            public bool startMoving;
            public bool moveFinished = false;

            private Move move;
            private SkillUser skillUser;

            private void Start()
            {
                DOTween.Init();
                move = new Move();

                if (isSkillUser)
                    skillUser = GetComponent<SkillUser>();
            }

            public void Move()
            {
                if (move.GetCard() == null)
                    move = GetComponent<Card>().GetMove();

                if (isSkillUser)
                    skillUser.ExecuteMoverSkills();

                move.GetCard().transform.DOMove(move.GetTargetTile().transform.position, 0.15f).OnComplete(() => TerminateMove());
            }

            public void TerminateMove()
            {
                move.GetCard().isMoving = false;
                move.GetCard().transform.position = move.GetTargetTile().transform.position;
                MoveType moveType = move.GetMoveType();
                Card card = move.GetCard();
                Card item = move.GetTargetTile().GetCard();
                switch (moveType)
                {
                    case MoveType.ITEM:
                        ItemMove(card, item);
                        ChangeTiles(card, false);
                        break;
                    case MoveType.COIN:
                        CoinMove(item);
                        ChangeTiles(card, false);
                        break;
                    case MoveType.EMPTY:
                        ChangeTiles(card, true);
                        break;
                    default:
                        break;
                }

                moveFinished = true;
                move.Reset();
            }

            private void ChangeTiles(Card card, bool isEmpty)
            {
                if (card.GetCardType() == CardType.PLAYER)
                    Tile.ChangeTile(move, isEmpty, true);
                else
                    Tile.ChangeTile(move, isEmpty, false);
            }

            private void CoinMove(Card item)
            {
                FindObjectOfType<CoinCounter>().IncreaseCoin(item.GetHealth());
            }

            private void ItemMove(Card card, Card item)
            {
                if (item.GetItemType() == ItemType.POTION)
                    card.GetComponent<ItemUser>().TakePotion(item);
                else if (item.GetItemType() == ItemType.WEAPON)
                    card.GetComponent<ItemUser>().TakeWeapon(item);
            }
        }
    }
}

