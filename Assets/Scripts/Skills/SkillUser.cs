using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Skills {
    public class SkillUser : MonoBehaviour
    {
        public Dictionary<Skill, int> moverSkills = new Dictionary<Skill, int>();
        public Dictionary<Skill, int> tempMoverSkills = new Dictionary<Skill, int>();

        public Dictionary<Skill, int> attackerSkills = new Dictionary<Skill, int>();
        public Dictionary<Skill, int> tempAttackerSkills = new Dictionary<Skill, int>();

        public List<Skill> moverSkillList = new List<Skill>();
        public List<Skill> attackerSkillList = new List<Skill>();

        private Card card;

        private void Start()
        {
            card = GetComponent<Card>();

            foreach (var s in moverSkillList)
            {
                moverSkills.Add(s, s.cooldown);
            }

            foreach (var s in attackerSkillList)
            {
                attackerSkills.Add(s, s.cooldown);
            }
        }

        public void ExecuteMoverSkills()
        {
            tempMoverSkills = new Dictionary<Skill, int>(moverSkills);
            foreach (var s in tempMoverSkills.Keys)
            {
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
            tempAttackerSkills = new Dictionary<Skill, int>(attackerSkills);
            foreach (var s in tempAttackerSkills.Keys)
            {
                if(attackerSkills[s] == 0)
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