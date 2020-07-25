using UnityEngine;
using DG.Tweening;
using DungeonRush.Cards;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Data;
using DungeonRush.Skills;

namespace DungeonRush 
{
    namespace Property
    {
        public class Mover : MonoBehaviour
        {
            public bool isSkillUser = false;
            public bool startMoving = false;
            public bool moveFinished = false;

            private Move move;
            private SkillUser skillUser = null;
            [SerializeField] Animator animator = null;

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
                // YÜRÜ.
                UpdateAnimation(true);
                move.GetCard().transform.DOMove(move.GetTargetTile().GetCoordinate(), 0.15f).OnComplete(() => TerminateMove());
            }

            public void TerminateMove()
            {
                // YÜRÜMEYİ BİTİR.
                UpdateAnimation(false);
                move.GetCard().isMoving = false;
                move.GetCard().transform.position = move.GetTargetTile().GetCoordinate();
                MoveType moveType = move.GetMoveType();
                Card card = move.GetCard();
                Card item = move.GetTargetTile().GetCard();
                switch (moveType)
                {
                    case MoveType.ITEM:
                        ItemMove(card, item);
                        ChangeTiles(card, true);
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
                Item i = item.GetComponent<Item>();

                if (i.GetItemType() == ItemType.POTION)
                    card.GetComponent<ItemUser>().TakePotion(i);
                else if (i.GetItemType() == ItemType.WEAPON)
                    card.GetComponent<ItemUser>().TakeWeapon(i);
                else if (i.GetItemType() == ItemType.ARMOR)
                    card.GetComponent<ItemUser>().TakeArmor(i);

            }

            private void UpdateAnimation(bool b)
            {  
                if(move.GetCard().GetCardType() != CardType.TRAP)
                   animator.SetBool("walk", b);
            }
        }
    }
}

