using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Skills {
    public class SkillUser : MonoBehaviour
    {
        public Dictionary<Skill, int> moverSkills = new Dictionary<Skill, int>();
        public Dictionary<Skill, int> attackerSkills = new Dictionary<Skill, int>();

        public List<Skill> moverSkillList = new List<Skill>();
        public List<Skill> attackerSkillList = new List<Skill>();

        private Card card;

        private void Start()
        {
            card = GetComponent<Card>();

            for (int i = 0; i < moverSkillList.Count; i++)
            {
                Skill s = moverSkillList[i];
                moverSkills.Add(s, s.cooldown);
            }

            for (int i = 0; i < attackerSkillList.Count; i++)
            {
                Skill s = attackerSkillList[i];
                attackerSkills.Add(s, s.cooldown);
            }

        }

        public void ExecuteMoverSkills()
        {
            List<Skill> list = new List<Skill>(moverSkills.Keys);
            for (int i = 0; i < list.Count; i++)
            {
                Skill s = list[i];
                if (moverSkills[s] == 0)
                {
                    s.Execute(card.GetMove());
                    moverSkills[s] = s.cooldown;
                }
                else
                {
                    moverSkills[s]--;
                }
            }
        }

        public void ExecuteAttackerSkills()
        {
            List<Skill> list = new List<Skill>(attackerSkills.Keys);
            for (int i = 0; i < list.Count; i++)
            {
                Skill s = list[i];
                if (attackerSkills[s] == 0)
                {
                    s.Execute(card.GetMove());
                    attackerSkills[s] = s.cooldown;
                }
                else
                {
                    attackerSkills[s]--;
                }
            }
        }
    }
}