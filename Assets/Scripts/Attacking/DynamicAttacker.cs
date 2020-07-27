using DungeonRush.Cards;
using UnityEngine;
using DG.Tweening;
using DungeonRush.Data;
using System.Collections;
using DungeonRush.Skills;
using DungeonRush.Attacking;

namespace DungeonRush
{
    namespace Property
    {
        public class DynamicAttacker : MonoBehaviour, IAttacker
        {
            [Header("Attacker Properties")]
            [SerializeField] bool isSkillUser = false;
            [SerializeField] float range = 0.8f;
            [SerializeField] int power = 5;
            [SerializeField] AttackStyle attackStyle = null;

            [Header("Animation Varibles")]
            [SerializeField] float closingToEnemyTime = 0.1f;
            [SerializeField] float damageTime = 0.1f;
            [SerializeField] float getBackTime = 0.1f;
            [SerializeField] Animator animator = null;
            [SerializeField] GameObject particulPrefab = null;
            private GameObject particulPrefabInstance = null;

            private bool attackFinished = false;
            private Card card = null;
            private ItemUser itemUser = null;
            private SkillUser skillUser = null;

            private void Start()
            {
                DOTween.Init();
                card = GetComponent<Card>();
                if(GetComponent<ItemUser>())
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
                move.GetCard().transform.DOMove(targetPos, closingToEnemyTime).OnComplete(() => StartCoroutine(FinishAttack(move)));
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
                if (itemUser && itemUser.GetWeapon() != null)
                    itemDamage = itemUser.GetWeapon().GetPower();
                int totalDamage = itemDamage + power;
                attackStyle.Attack(move, totalDamage);
            }

            private IEnumerator FinishAttack(Move move)
            {
                UpdateAnimation(false, false);

                UpdateAnimation(true, true);

                Damage(move);
                yield return new WaitForSeconds(damageTime);
                if (particulPrefabInstance == null)
                    InitializeParticulEffect(move);
                else
                    EnableParticulEffect(move);

                move.GetCard().transform.DOMove(move.GetCardTile().GetCoordinate(), getBackTime).OnComplete(() => FinaliseAttack());
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

            public bool GetAttackFinished()
            {
                return attackFinished;
            }

            public void SetAttackFinished(bool b)
            {
                attackFinished = b;
            }
        }
    }
}
