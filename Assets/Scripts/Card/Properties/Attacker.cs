using DungeonRush.Cards;
using DungeonRush.Managers;
using UnityEngine;
using DG.Tweening;
using DungeonRush.Data;
using System.Collections;

namespace DungeonRush
{
    namespace Property
    {
        public class Attacker : MonoBehaviour
        {
            [SerializeField] bool isItemUser = false;
            [SerializeField] float range = 0.8f;
            // TODO Bunu da car properties e ekle.
            [SerializeField] int power = 5;
            private Card card;
            private ItemUser itemUser;

            public bool attackFinished = false;

            private void Start()
            {
                DOTween.Init();
                card = GetComponent<Card>();
                if(isItemUser)
                    itemUser = GetComponent<ItemUser>();
            }

            // Saldırı eylemi için false, ilerleme eyleme için true.
            public bool CanMove(Card enemy)
            {
                if (enemy.GetCardType() != CardType.ENEMY)
                    return true;
                return false;
            }

            private void MoveToAttackRange()
            {
                Move move = card.GetMove();
                var dir = GetDirection(move);
                Vector2 targetPos = new Vector2(move.GetCardTile().transform.position.x + dir.x * range, move.GetCardTile().transform.position.y + dir.y * range);
                // Yolun yarısına kadar yürüyecek.
                move.GetCard().transform.DOMove(targetPos, 0.15f).OnComplete(() => StartCoroutine(FinishAttack(move)));
            }

            public void Attack()
            {
                attackFinished = false;
                MoveToAttackRange();
            }


            private void Damage(Card enemy)
            {
                int itemDamage = 0;
                if (itemUser && itemUser.GetItem().exist)
                    itemDamage = itemUser.GetItem().GetHealth();

                int totalDamage = itemDamage + power;
                enemy.DecreaseHealth(totalDamage);
            }

            private IEnumerator FinishAttack(Move move)
            {
                print("Anim başlar");
                yield return new WaitForSeconds(0.2f);
                Damage(move.GetTargetTile().GetCard());
                print("Anim biter");
                move.GetCard().transform.DOMove(move.GetCardTile().transform.position, 0.15f);
                attackFinished = true;
            }

            private Vector3 GetDirection(Move move)
            {
                var heading = move.GetTargetTile().transform.position - move.GetCardTile().transform.position;
                var distance = heading.magnitude;
                var direction = heading / distance;
                return direction;
            }


            public void LoadLoseScene()
            {
                LoadManager.LoadLoseScene();
            }
        }
    }
}
