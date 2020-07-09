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
            [HideInInspector] public bool attackFinished = false;
            [SerializeField] bool isItemUser = false;
            [SerializeField] float range = 0.8f;
            [SerializeField] int power = 5;

            [SerializeField] GameObject slashPrefab;
            private GameObject slashPrefabInstance;

            [SerializeField] GameObject particulPrefab;
            private GameObject particulPrefabInstance;

            private Card card;
            private ItemUser itemUser;

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
                switch (enemy.GetCardType())
                {
                    case CardType.PLAYER:
                        return false;
                    case CardType.ENEMY:
                        return false;
                }

                return true;
            }

            private void MoveToAttackRange()
            {
                Move move = card.GetMove();
                var dir = GetDirection(move);
                Vector2 targetPos = new Vector2(move.GetCardTile().transform.position.x + dir.x * range, move.GetCardTile().transform.position.y + dir.y * range);
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
                slashPrefabInstance = Instantiate(slashPrefab, move.GetTargetTile().transform);
                yield return new WaitForSeconds(0.2f);
                particulPrefabInstance = Instantiate(particulPrefab, move.GetTargetTile().transform);
                Damage(move.GetTargetTile().GetCard());
                move.GetCard().transform.DOMove(move.GetCardTile().transform.position, 0.2f).OnComplete(() =>  FinaliseAttack());
            }

            private void FinaliseAttack()
            {
                Destroy(slashPrefabInstance);
                Destroy(particulPrefabInstance);
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
