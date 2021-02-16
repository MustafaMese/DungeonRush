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
    public class EnvironmentCard : Card, IFighter
    {
        [SerializeField] TextMeshPro nameText = null;
        [SerializeField] ElementType elementType;
        [SerializeField] bool isDefinite;
        [SerializeField] float time;

        [SerializeField] List<ImpactElementList> impacts;

        private Attacker attacker;
        private IMoveController controller;
        private StatusController statusController;

        private IFighter fighter;

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
            print("7");
            bool delete;
            EnvironmentCard prefab = GetPrefab(changingTrap.GetElementType(), elementType, out delete);
            if(prefab == null) 
            {
                if(!delete) return;
                else
                {
                    Remove(changingTrap);
                    return;
                }
            }
            changingTrap.Impact(elementType);
            Change(changingTrap, prefab, changingTrap.GetTile());
        }

        public void Evolve(ElementType element)
        {
            bool delete;
            EnvironmentCard prefab = GetPrefab(elementType, element, out delete);
            if (prefab == null)
            {
                if (!delete) return;
                else
                {
                    Remove(this);
                    return;
                }
            }
            Impact(element);
            Change(this, prefab, GetTile());
        }

        private void Impact(ElementType element)
        {
            print("8");
            for (var i = 0; i < impacts.Count; i++)
            {
                if(impacts[i].element == element)
                {
                    ElementalStatus status = (ElementalStatus)impacts[i].impact.Create(null);

                    status.Initialize(transform.position);
                    status.Execute(GetTile().GetCard(), GetTile());
                    return;
                }

            }
        }

        protected EnvironmentCard GetPrefab(ElementType changed, ElementType changer, out bool delete)
        {
            delete = false;

            if(changer == ElementType.FIRE && (changed == ElementType.OIL || changed == ElementType.GRASS)) return EnvironmentManager.Instance.GetEnvironmentCard(changer);
            else if(changer == ElementType.FIRE && (changed == ElementType.WATER || changed == ElementType.FREEZE)) 
            {
                delete = true;
                return null;
            }
            else if(changer == ElementType.FIRE && changed == ElementType.POISON) return EnvironmentManager.Instance.GetEnvironmentCard(changer);
            else if(changer == ElementType.WATER && (changed == ElementType.FIRE)) 
            {
                delete = true;
                return null;
            }
            else if(changer == ElementType.WATER && (changed == ElementType.OIL)) return EnvironmentManager.Instance.GetEnvironmentCard(changer);
            else if(changer == ElementType.FREEZE && (changed == ElementType.FIRE)) 
            {
                delete = true;
                return null;
            }
            else if(changer == ElementType.FREEZE && (changed == ElementType.WATER)) return EnvironmentManager.Instance.GetEnvironmentCard(changer);
            else if(changer == ElementType.ELECTRICITY && (changed == ElementType.WATER)) return EnvironmentManager.Instance.GetEnvironmentCard(changer);
            else if(changer == ElementType.POISON && (changed == ElementType.FIRE)) return EnvironmentManager.Instance.GetEnvironmentCard(changer);
            else return null;
        }

        private void Change(EnvironmentCard oldCard, EnvironmentCard prefab, Tile tile)
        {
            Remove(oldCard);
            CardManager.Instance.AddCard(prefab, tile);
        }

        private void Remove(EnvironmentCard card)
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
    STATIC
}
