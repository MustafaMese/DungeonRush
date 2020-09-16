using DG.Tweening;
using DungeonRush.Attacking;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Field;
using DungeonRush.Items;
using DungeonRush.Skills;
using System.Collections;
using UnityEngine;

namespace DungeonRush.Property
{
    public class PlayerAttacker : Attacker
    {
        [Header("Attacker Properties")]
        [SerializeField] bool isSkillUser = false;
        [SerializeField] float range = 0.8f;
        
        [Header("Animation Variables")]
        [SerializeField] float closingToEnemyTime = 0.1f;
        [SerializeField] float damageTime = 0.1f;
        [SerializeField] float getBackTime = 0.1f;
        
        private SkillUser skillUser = null;

        protected override void Initialize()
        {
            base.Initialize();
            if (isSkillUser)
                skillUser = GetComponent<SkillUser>();
        }

        private void MoveToAttackRange()
        {
            Move move = card.GetMove();
            var dir = GetDirection(move);

            Vector2 targetPos = new Vector2(move.GetCardTile().GetCoordinate().x + dir.x * range, move.GetCardTile().GetCoordinate().y + dir.y * range);
            UpdateAnimation(true, false);
            move.GetCard().transform.DOMove(targetPos, closingToEnemyTime).OnComplete(() => StartCoroutine(FinishAttack(move)));
        }

        public override void Attack()
        {
            attackFinished = false;
            if (isSkillUser)
                skillUser.ExecuteAttackerSkills();
            MoveToAttackRange();
        }

        private void Damage(Move move)
        {
            Tile target = move.GetTargetTile();
            float time = attackStyle.GetAnimationTime();

            StartCoroutine(StartAttackAnimation(poolForAttackStyle, move, time));
            AttackAction(move);

            if(target.GetCard() == null || target.GetCard().GetHealth() <= 0)
                CollectableManager.Instance.AddCoins(target.transform.position, target.GetCard().GetLevel());

        }

        public int GetDamage()
        {
            return power;
        }
        
        private IEnumerator FinishAttack(Move move)
        {
            UpdateAnimation(false, false);
            UpdateAnimation(true, true);
            Damage(move);
            yield return new WaitForSeconds(damageTime);
            move.GetCard().transform.DOMove(move.GetCardTile().GetCoordinate(), getBackTime).OnComplete(() => FinaliseAttack());
        }
        private void FinaliseAttack()
        {
            attackFinished = true;
        }
    }
}
