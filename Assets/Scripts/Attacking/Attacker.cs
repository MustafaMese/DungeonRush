using DungeonRush.Cards;
using DungeonRush.Managers;
using UnityEngine;
using DG.Tweening;
using DungeonRush.Data;
using System.Collections;
using DungeonRush.Skills;
using System;
using DungeonRush.Attacking;

namespace DungeonRush
{
    namespace Property
    {
        public class Attacker : MonoBehaviour
        {
            [HideInInspector] public bool attackFinished = false;
            [SerializeField] bool isItemUser = false;
            [SerializeField] bool isSkillUser = false;
            [SerializeField] float range = 0.8f;
            [SerializeField] int power = 5;
            [SerializeField] AttackStyle attackStyle;

            [SerializeField] GameObject particulPrefab;
            private GameObject particulPrefabInstance;

            private Card card;
            private ItemUser itemUser;
            private SkillUser skillUser;
            [SerializeField] Animator animator;

            private void Start()
            {
                DOTween.Init();
                card = GetComponent<Card>();
                if(isItemUser)
                    itemUser = GetComponent<ItemUser>();
                if (isSkillUser)
                    skillUser = GetComponent<SkillUser>();
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

                Vector2 targetPos = new Vector2(move.GetCardTile().GetCoordinate().x + dir.x * range, move.GetCardTile().GetCoordinate().y + dir.y * range);
                UpdateAnimation(true, false);
                move.GetCard().transform.DOMove(targetPos, 0.1f).OnComplete(() => StartCoroutine(FinishAttack(move)));
            }

            public void Attack()
            {
                attackFinished = false;
                if (isSkillUser)
                    skillUser.ExecuteAttackerSkills();
                MoveToAttackRange();
            }

            private void Damage(Move move)
            {
                int itemDamage = 0;
                if (itemUser && itemUser.GetItem() != null)
                    itemDamage = itemUser.GetItem().GetDamage();
                int totalDamage = itemDamage + power;
                attackStyle.Attack(move, totalDamage);
            }

            private IEnumerator FinishAttack(Move move)
            {
                UpdateAnimation(false, false);

                UpdateAnimation(true, true);

                Damage(move);
                yield return new WaitForSeconds(0.1f);
                if (particulPrefabInstance == null)
                    InitializeParticulEffect(move);
                else
                    EnableParticulEffect(move);

                move.GetCard().transform.DOMove(move.GetCardTile().GetCoordinate(), 0.1f).OnComplete(() => FinaliseAttack());
            }

            #region FINALIZE ATTACK AND EFFECTS
            private void EnableParticulEffect(Move move)
            {
                particulPrefabInstance.SetActive(true);
                particulPrefabInstance.transform.position = move.GetTargetTile().GetCoordinate();
            }

            private void InitializeParticulEffect(Move move)
            {
                particulPrefabInstance = Instantiate(particulPrefab, move.GetTargetTile().GetCoordinate(), Quaternion.identity, this.transform);
            }


            private void FinaliseAttack()
            {
                particulPrefabInstance.SetActive(false);
                attackFinished = true;
            }

            #endregion

            private Vector3 GetDirection(Move move)
            {
                var heading = move.GetTargetTile().GetCoordinate() - move.GetCardTile().GetCoordinate();
                var distance = heading.magnitude;
                var direction = heading / distance;
                return direction;
            }

            private void UpdateAnimation(bool play, bool isAttack)
            {
                if(card.GetCardType() != CardType.TRAP)
                    if (isAttack)
                        animator.SetTrigger("attack");
                    else
                        animator.SetBool("walk", play);
            }
        }
    }
}
