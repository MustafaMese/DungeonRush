using System;
using System.Collections;
using System.Collections.Generic;
using DungeonRush.Attacking;
using DungeonRush.Controller;
using DungeonRush.Customization;
using DungeonRush.Field;
using DungeonRush.Managers;
using DungeonRush.Property;
using DungeonRush.Traits;
using TMPro;
using UnityEngine;

namespace DungeonRush.Cards
{
    public class EnvironmentCard : Card, IFighter
    {
        [SerializeField] TextMeshPro nameText = null;
        [SerializeField] ElementType elementType;
        [SerializeField] bool isDefinite;
        [SerializeField] float time;

        private Attacker attacker;
        private IMoveController controller;
        private StatusController statusController;
        private ICustomization customization;

        private IFighter fighter;

        protected override void Initialize()
        {
            base.Initialize();

            attacker = GetComponent<Attacker>();
            controller = GetComponent<IMoveController>();
            statusController = GetComponent<StatusController>();
            fighter = GetComponent<IFighter>();
            customization = GetComponent<ICustomization>();

            if (transform.position.y < 0)
                customization.ChangeLayer(false, (int)transform.position.y);
            else if (transform.position.y > 0)
                customization.ChangeLayer(true, (int)transform.position.y);
        }

        public void EvolveIt(EnvironmentCard changingTrap)
        {
            if(changingTrap == null) return;

            Tile target = changingTrap.GetTile();

            Impact impactPrefab = EnvironmentManager.Instance.GetImpactPrefab(changingTrap.GetElementType(), elementType);
            if(impactPrefab != null)
            {
                Impact impact = Instantiate(impactPrefab, transform.position, Quaternion.identity);

                impact.Initialize(transform.position);
                impact.Execute(target);
            }

            EnvironmentCard eCard = EnvironmentManager.Instance.GetElementPrefab(changingTrap.GetElementType(), elementType);
            if(eCard != null)
            {
                Remove(changingTrap);
                CardManager.Instance.AddCard(eCard, target);
            }
        }

        public bool CheckTime()
        {
            if(!isDefinite) return true;
            else if(isDefinite && time >= 0) 
            {
                time--;
                return true;
            }
            else
                return false;
        }

        public void Change(ElementType changer, EnvironmentCard changed, Tile tile)
        {
            if(changed == null) return;

            EnvironmentCard prefab = EnvironmentManager.Instance.GetElementPrefab(changer, changed.GetElementType());
            
            if(prefab != null)
            {
                Remove(changed);
                CardManager.Instance.AddCard(prefab, tile);
            }
        }

        public void Remove(EnvironmentCard card)
        {
            card.GetController().Stop();
            CardManager.Unsubscribe(card);
            CardManager.RemoveCard(card.GetTile(), true);
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
    BURNED,
    NONE
}
