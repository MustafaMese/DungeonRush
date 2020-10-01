using DungeonRush.Cards;
using DungeonRush.Items;
using DungeonRush.Managers;
using DungeonRush.Skills;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Cards
{
    public class SkillEventCard : EventCard
    {
        [SerializeField] Skill skill = null;
        [SerializeField] float disapperTime = 0;
        [SerializeField] SpriteRenderer sprite;

        public override void GetEvent(Card card)
        {
            SkillMove(card, skill);
            StartCoroutine(Disapper());
        }

        protected override void Initialize()
        {
            skill = ItemDB.Instance.GetRandomSkill();
        }

        private void SkillMove(Card card, Skill s)
        {
            if (s == null) return;

            card.GetComponent<SkillUser>().AddSkill(s);
        }

        private IEnumerator Disapper()
        {
            Color c;
            while (sprite.color.a > 0)
            {
                c = sprite.color;
                c.a -= Time.deltaTime / disapperTime;
                sprite.color = c;
                yield return null;
            }
            CardManager.RemoveCardForAttacker(GetTile().GetCoordinate());
        }
    }
}