using DG.Tweening;
using DungeonRush.Attacking;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Items;
using DungeonRush.Skills;
using System.Collections;
using UnityEngine;

namespace DungeonRush.Property
{
    public class PlayerAttacker : MonoBehaviour, IAttacker
    {
        [Header("Attacker Properties")]
        [SerializeField] bool isSkillUser = false;
        [SerializeField] float range = 0.8f;
        [SerializeField] int power = 5;
        [SerializeField] AttackStyle attackStyle = null;

        [Header("Animation Variables")]
        [SerializeField] float closingToEnemyTime = 0.1f;
        [SerializeField] float damageTime = 0.1f;
        [SerializeField] float getBackTime = 0.1f;
        [SerializeField] Animator animator = null;
        [SerializeField] GameObject particul = null;
        [SerializeField] float particulTime = 1f;

        private ObjectPool poolForParticul = new ObjectPool();
        private ObjectPool poolForAttackStyle = new ObjectPool();
        private GameObject effectObject = null;

        private bool attackFinished = false;
        private Card card = null;
        private ItemUser itemUser = null;
        private SkillUser skillUser = null;

        private void Start()
        {
            DOTween.Init();
            card = GetComponent<Card>();
            if (GetComponent<ItemUser>())
                itemUser = GetComponent<ItemUser>();
            if (isSkillUser)
                skillUser = GetComponent<SkillUser>();

            FillThePool(poolForParticul, particul, 3);

            effectObject = attackStyle.GetEffect();
            FillThePool(poolForAttackStyle, effectObject, 2);
        }

        private void FillThePool(ObjectPool pool, GameObject effect, int objectCount)
        {
            pool.SetObject(effect);
            pool.FillPool(objectCount);
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

            Transform card = move.GetCard().transform;
            Transform target = move.GetTargetTile().transform;
            Vector3 tPos = move.GetTargetTile().GetCoordinate();
            float time = attackStyle.GetAnimationTime();

            attackStyle.Attack(move, totalDamage);
            StartCoroutine(StartAttackAnimation(poolForAttackStyle, tPos, card, target, time));
            StartCoroutine(StartAttackAnimation(poolForParticul, tPos, card, null, particulTime));
            
        }

        private IEnumerator StartAttackAnimation(ObjectPool pool, Vector3 tPos, Transform card, Transform target, float time)
        {
            GameObject obj = pool.PullObjectFromPool();
            attackStyle.SetEffectPosition(obj, tPos, target);
            yield return new WaitForSeconds(time);
            attackStyle.SetEffectPosition(obj, tPos, card);
            pool.AddObjectToPool(obj);
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

        private Vector3 GetDirection(Move move)
        {
            var heading = move.GetTargetTile().GetCoordinate() - move.GetCardTile().GetCoordinate();
            var distance = heading.magnitude;
            var direction = heading / distance;
            return direction;
        }

        private void UpdateAnimation(bool play, bool isAttack)
        {
            if (card.GetCardType() != CardType.TRAP)
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

        public void SetAttackStyle(AttackStyle attackStyle)
        {
            poolForAttackStyle.DeleteObjectsInPool();
            this.attackStyle = attackStyle;
            effectObject = attackStyle.GetEffect();
            FillThePool(poolForAttackStyle, effectObject, 2);
        }

        public AttackStyle GetAttackStyle()
        {
            return attackStyle;
        }
    }
}
