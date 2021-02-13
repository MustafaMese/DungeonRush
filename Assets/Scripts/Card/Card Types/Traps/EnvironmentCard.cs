using System;
using System.Collections;
using System.Collections.Generic;
using DungeonRush.Attacking;
using DungeonRush.Controller;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using DungeonRush.Traits;
using TMPro;
using UnityEngine;

namespace DungeonRush.Cards
{
    public abstract class EnvironmentCard : Card, IFighter
    {
        [SerializeField] TextMeshPro nameText = null;
        [SerializeField] ElementType elementType;
        [SerializeField] bool isDefinite;
        [SerializeField] float time;

        private Attacker attacker;
        private IMoveController controller;
        private StatusController statusController;

        private IFighter fighter;

        protected abstract EnvironmentCard GetPrefab();
        protected abstract void Impact();

        protected override void Initialize()
        {
            base.Initialize();
            attacker = GetComponent<Attacker>();
            controller = GetComponent<IMoveController>();
            statusController = GetComponent<StatusController>();
            fighter = GetComponent<IFighter>();
        }

        public void EvolveIt(EnvironmentCard changingTrap)
        {
            EnvironmentCard prefab = null;
            if(prefab == null) return;
            changingTrap.Impact();
            Change(changingTrap, prefab);
        }

        public void Evolve(ElementType element)
        {
            EnvironmentCard prefab = null;

            if (prefab == null) return;
            Impact();
            Change(this, prefab);
        }

        private void Change(EnvironmentCard oldTrap, EnvironmentCard prefab)
        {
            oldTrap.GetController().Stop();
            CardManager.Unsubscribe(oldTrap);
            CardManager.RemoveCard(oldTrap.GetTile(), true);
            CardManager.Instance.AddCard(prefab, GetTile());
        }

        public override IFighter GetFighter()
        {
            return fighter;
        }

        public override IMoveController GetController()
        {
            return controller;
        }

        public override StatusController GetStatusController()
        {
            return statusController;
        }

        public bool CanAttack(Card enemy)
        {
            return attacker.CanMove(enemy);
        }

        public void ExecuteAttack()
        {
            attacker.Attack();
        }

        public AttackStyle GetAttackStyle()
        {
            return attacker.GetAttackStyle();        
        }

        public ElementType GetElementType()
        {
            return elementType;
        }

        public List<EnvironmentCard> CheckOtherEnvironmentCards()
        {
            List<EnvironmentCard> cards = new List<EnvironmentCard>();
            Vector2 pos = transform.position;

            EnvironmentCard card;
            Vector2 targetPos = new Vector2(pos.x + 1, pos.y);
            card = GetEnvironmentCardFromPosition(targetPos);
            if (card != null)
                cards.Add(card);

            targetPos = new Vector2(pos.x - 1, pos.y);
            card = GetEnvironmentCardFromPosition(targetPos);
            if (card != null)
                cards.Add(card);

            targetPos = new Vector2(pos.x, pos.y + 1);
            card = GetEnvironmentCardFromPosition(targetPos);
            if (card != null)
                cards.Add(card);

            targetPos = new Vector2(pos.x, pos.y - 1);
            card = GetEnvironmentCardFromPosition(targetPos);
            if (card != null)
                cards.Add(card);

            return cards;
        }

        private EnvironmentCard GetEnvironmentCardFromPosition(Vector2 targetPos)
        {
            EnvironmentCard card = null;
            if (Board.tilesByCoordinates.ContainsKey(targetPos))
                card = Board.tilesByCoordinates[targetPos].GetEnvironmentCard();

            return card;
        }
    }
}

public enum ElementType
{
    FIRE,
    WATER,
    OIL,
    FREEZE,
    GRASS,
    ELECTRICITY,
    POISON,
    STATIC
}
