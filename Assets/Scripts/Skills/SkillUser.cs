using DungeonRush.Cards;
using DungeonRush.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Skills {
    public class SkillUser : MonoBehaviour
    {
        [Serializable]
        public class SkillData
        {
            public Skill skill;
            public ObjectPool poolForEffect;
            public ObjectPool poolForTextPopup;
            public int tempCooldown;

            public SkillData(Skill skill)
            {
                this.skill = skill;

                if (skill.Effect != null)
                {
                    poolForEffect = new ObjectPool();
                    poolForEffect.SetObject(skill.Effect);
                    poolForEffect.FillPool(1);
                }

                if(skill.TextPopup != null)
                {
                    poolForTextPopup = new ObjectPool();
                    poolForTextPopup.SetObject(skill.TextPopup);
                    poolForTextPopup.FillPool(1);
                }

                if (skill.IsActive)
                    tempCooldown = skill.Cooldown;
            }
        }

        public List<SkillData> skills = new List<SkillData>();

        private Card card;

        public Skill SKILL;

        private void Start()
        {
            card = GetComponent<Card>();
            AddSkill(SKILL);
        }

        public void AddSkill(Skill skill)
        {
            SkillData s = new SkillData(skill);
            skills.Add(s);
        }

        public void ExecuteAttackerSkills()
        {
            for (int i = 0; i < skills.Count; i++)
            {
                SkillData skillData = skills[i];
                if(skillData.skill.IsAttacker)
                    ExecuteSkill(skillData);
            }
        }

        public void ExecuteMoverSkills()
        {
            for (int i = 0; i < skills.Count; i++)
            {
                SkillData skillData = skills[i];
                if (!skillData.skill.IsAttacker)
                    ExecuteSkill(skillData);
                else
                    DecreaseCooldown(skillData);
            }
        }

        private void ExecuteSkill(SkillData skillData)
        {
            if (CooldownControl(skillData))
            {
                skillData.skill.Execute(card.GetMove());

                if(skillData.skill.Effect != null)
                    StartCoroutine(Animate(skillData, card.GetMove()));
                if(skillData.skill.TextPopup != null)
                    StartCoroutine(TextPopup(skillData, card.GetMove()));

                IncreaseCooldown(skillData);
            }
            else
                DecreaseCooldown(skillData);
        }

        private IEnumerator Animate(SkillData skillData, Move move)
        {
            GameObject obj = skillData.poolForEffect.PullObjectFromPool();
            skillData.skill.PositionEffect(obj, move);
            yield return new WaitForSeconds(skillData.skill.EffectTime);
            obj.transform.SetParent(transform);
            skillData.poolForEffect.AddObjectToPool(obj);
        }

        private IEnumerator TextPopup(SkillData skillData, Move move)
        {
            GameObject obj = skillData.poolForTextPopup.PullObjectFromPool();
            Vector3 pos = skillData.skill.PositionTextPopup(obj, move);
            TextPopup objTxt = obj.GetComponent<TextPopup>();
            string power = skillData.skill.Power.ToString();
            objTxt.Setup(power, pos);
            yield return new WaitForSeconds(skillData.skill.EffectTime);
            obj.transform.SetParent(transform);
            skillData.poolForTextPopup.AddObjectToPool(obj);
        }

        public bool CooldownControl(SkillData skillData)
        {
            if (skillData.tempCooldown != 0)
                return false;
            else
                return true;
        }

        private void DecreaseCooldown(SkillData skillData)
        {
            if (skillData.tempCooldown > 0)
                skillData.tempCooldown--;
        }

        private void IncreaseCooldown(SkillData skillData)
        {
            if (skillData.skill.IsActive)
                skillData.tempCooldown = skillData.skill.Cooldown;
            else
                skillData.tempCooldown = -1;
        }
    }
}