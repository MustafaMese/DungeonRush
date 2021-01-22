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
        [SerializeField] List<SkillObject> skillObjects = new List<SkillObject>();

        private List<Skill> skills = new List<Skill>();
        private Card card;
        private bool isBeginning;
        private int lastIndex;

        private void Start()
        {
            lastIndex = 0;
            card = GetComponent<Card>();

            for (var i = 0; i < skillObjects.Count; i++)
            {
                SkillObject o = skillObjects[i];
                skillObjects.Remove(o);
                AddSkill(o);
            }
        }

        public List<string> GetSkillIDs()
        {
            List<string> ids = new List<string>();
            for (int i = 0; i < skillObjects.Count; i++)
            {
                string id = skillObjects[i].GetID();
                ids.Add(id);
            }
            return ids;
        }

        public void AddSkill(SkillObject skillObject, bool openPanel = true)
        {
            skillObjects.Add(skillObject);

            Skill skill = skillObject.Create(transform);
            skill.Initialize(card);

            if (skill.skillType == SkillType.ACTIVE)
                UIManager.Instance.AddSkillToButton(skill);

            if(openPanel)
                UIManager.Instance.EnableSkillCanvas(skill);

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