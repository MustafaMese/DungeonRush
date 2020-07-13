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

            [SerializeField] GameObject slashPrefab;
            private GameObject slashPrefabInstance;

            [SerializeField] GameObject particulPrefab;
            private GameObject particulPrefabInstance;

            private Card card;
            private ItemUser itemUser;
            private SkillUser skillUser;

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
                Vector2 targetPos = new Vector2(move.GetCardTile().transform.position.x + dir.x * range, move.GetCardTile().transform.position.y + dir.y * range);
                move.GetCard().transform.DOMove(targetPos, 0.15f).OnComplete(() => StartCoroutine(FinishAttack(move)));
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
                if (itemUser && itemUser.GetItem().exist)
                    itemDamage = itemUser.GetItem().GetHealth();
                int totalDamage = itemDamage + power;

                attackStyle.Attack(move, totalDamage);
            }

            private IEnumerator FinishAttack(Move move)
            {
                if (slashPrefabInstance == null)
                    InitializeSlashInstance(move);
                else
                    EnableSlashPrefabInstance(move);

                yield return new WaitForSeconds(0.2f);

                if (particulPrefabInstance == null)
                    InitializeParticulEffect(move);
                else
                    EnableParticulEffect(move);
                Damage(move);
                move.GetCard().transform.DOMove(move.GetCardTile().transform.position, 0.2f).OnComplete(() => FinaliseAttack());
            }

            #region FINALIZE ATTACK AND EFFECTS
            private void EnableParticulEffect(Move move)
            {
                particulPrefabInstance.SetActive(true);
                particulPrefabInstance.transform.position = move.GetTargetTile().transform.position;
            }

            private void InitializeParticulEffect(Move move)
            {
                particulPrefabInstance = Instantiate(particulPrefab, move.GetTargetTile().transform.position, Quaternion.identity, this.transform);
            }

            private void InitializeSlashInstance(Move move)
            {
                slashPrefabInstance = Instantiate(slashPrefab, move.GetTargetTile().transform.position, Quaternion.identity, this.transform);
            }

            private void EnableSlashPrefabInstance(Move move)
            {
                slashPrefabInstance.SetActive(true);
                slashPrefabInstance.transform.position = move.GetTargetTile().transform.position;
            }

            private void FinaliseAttack()
            {
                slashPrefabInstance.SetActive(false);
                particulPrefabInstance.SetActive(false);
                attackFinished = true;
            }

            #endregion

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
