using UnityEngine;
using DG.Tweening;
using DungeonRush.Cards;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Data;
using DungeonRush.Skills;
using DungeonRush.Shifting;

namespace DungeonRush 
{
    namespace Property
    {
        // TODO İlerleme şekilleri de değişiklik gösterebilir.
        public class PlayerMover : MonoBehaviour, IMover
        {
            private Move move;
            private bool isMoveFinished = false;

            public bool isSkillUser = false;
            private SkillUser skillUser = null;

            [SerializeField] Shift shifting = null;
            [SerializeField] float movingTime = 0.2f;
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
                if (isSkillUser)
                    skillUser.ExecuteMoverSkills();
                // YÜRÜ.
                UpdateAnimation(true);
                move.GetCard().transform.DOMove(move.GetTargetTile().GetCoordinate(), movingTime).OnComplete(() => TerminateMove());
            }

            private void TerminateMove()
            {
                // YÜRÜMEYİ BİTİR.
                UpdateAnimation(false);
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

                isMoveFinished = true;
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

            public Move GetMove()
            {
                return move;
            }

            public void SetMove(Move move)
            {
                this.move = move;
            }

            public Shift GetShift()
            {
                return shifting;
            }

            public bool IsMoveFinished()
            {
                return isMoveFinished;
            }

            public void SetIsMoveFinished(bool b)
            {
                isMoveFinished = b;
            }
        }
    }
}

