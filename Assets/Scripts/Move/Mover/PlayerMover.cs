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
        public class PlayerMover : MonoBehaviour, IMover
        {
            private Move move;
            private bool isMoveFinished = false;

            public bool isSkillUser = false;
            private SkillUser skillUser = null;

            [Header("Shifting Properties")]
            [SerializeField] Shift shifting = null;
            [SerializeField] float movingTime = 0.2f;
            [SerializeField] Animator animator = null;
            [SerializeField] GameObject walkParticul = null;
            [SerializeField] float particulTime = 0;
            private ObjectPool pool = new ObjectPool();

            private Card card;

            private void Start()
            {
                DOTween.Init();
                move = new Move();
                card = GetComponent<Card>();
                if (isSkillUser)
                    skillUser = GetComponent<SkillUser>();

                pool.SetObject(walkParticul);
                pool.FillPool(4);
            }

            public void Move()
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

            private IEnumerator StartMoveAnimation(Vector3 pos, float time)
            {
                GameObject obj = pool.PullObjectFromPool();
                obj.transform.position = pos;
                yield return new WaitForSeconds(time);
                pool.AddObjectToPool(obj);
            }

            private void TerminateMove()
            {
                // YÜRÜMEYİ BİTİR.
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

            private void UpdateAnimation(bool b)
            {  
                animator.SetBool("walk", b);
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

