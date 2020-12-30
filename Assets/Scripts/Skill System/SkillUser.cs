using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Skills 
{
    public class SkillUser : MonoBehaviour
    {
        public List<Skill> skills = new List<Skill>();
        private Card card;
        private bool isBeginning;
        private int lastIndex;
        public List<SkillObject> SKILLS = new List<SkillObject>();
        private void Start()
        {
            lastIndex = 0;
            card = GetComponent<Card>();

            if(SKILLS != null && SKILLS.Count > 0)
            {
                for (int i = 0; i < SKILLS.Count; i++)
                {
                    AddSkill(SKILLS[i]);
                }
            }
        }

        public List<string> GetSkillIDs()
        {
            List<string> ids = new List<string>();
            for (int i = 0; i < skills.Count; i++)
            {
                string id = skills[i].SkillName;
                ids.Add(id);
            }
            return ids;
        }

        public void AddSkill(SkillObject skillObject)
        {
            Skill skill = skillObject.Create(transform);
            skill.Initialize(card);

            if (skill.skillType == SkillType.ACTIVE)
                UIManager.Instance.AddSkillToButton(skill);

            UIManager.Instance.AddSkillToSkillSet(skill.IconSmall);

            skills.Add(skill);
        }

        public void ExecuteActiveSkill(Skill skill)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                Skill s = skills[i];

                if (skill.skillType == SkillType.ACTIVE && s == skill)
                    ExecuteSkill(skill, true);
                else
                    s.DecreaseCooldown();
            }
        }

        public void ExecuteAttackerSkills()
        {
            for (int i = 0; i < skills.Count; i++)
            {
                Skill skill = skills[i];

                if(skill.skillType != SkillType.ACTIVE && skill.IsAttacker)
                    ExecuteSkill(skill);
                else
                    skill.DecreaseCooldown();
            }
        }

        public void ExecuteMoverSkills()
        {
            for (int i = 0; i < skills.Count; i++)
            {
                Skill skill = skills[i];

                if (skill.skillType != SkillType.ACTIVE && !skill.IsAttacker)
                    ExecuteSkill(skill);
                else
                    skill.DecreaseCooldown();
            }
        }

        private void ExecuteSkill(Skill skill, bool isActive = false)
        {
            if(card.GetMove().GetCard() == null)
            {
                Move move = new Move(card);
                card.SetMove(move);
            }

            skill.Adjust(card.GetMove());
            skill.Execute(card.GetMove());
        }
    }
}