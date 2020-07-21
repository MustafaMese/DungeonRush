using System.Collections;
using System.Collections.Generic;
using DungeonRush.Attacking;
using DungeonRush.Cards;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Property
{
    public class StaticAttacker : MonoBehaviour, IAttacker
    {
        [SerializeField] int power = 2;
        [SerializeField] AttackStyle attackStyle;
        
        [SerializeField] GameObject particulPrefab;
        private GameObject particulPrefabInstance;

        private Card card;
        private bool attackFinished = false;
        private void Start()
        {
            card = GetComponent<Card>();
        }

        public void Attack()
        {
            attackFinished = false;
            Move move = card.GetMove();
            StartCoroutine(Damage(move));
        }

        private IEnumerator Damage(Move move)
        {
            print("hobb");
            attackStyle.Attack(move, power);
            yield return new WaitForSeconds(0.2f);

            if (particulPrefabInstance == null)
                InitializeParticulEffect(move);
            else
                EnableParticulEffect(move);
            FinaliseAttack();
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
