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

            public SkillData(Skill skill, Transform t)
            {
                this.skill = skill;

                if (skill.Effect != null)
                {
                    poolForEffect = new ObjectPool();
                    poolForEffect.SetObject(skill.Effect);
                    poolForEffect.FillPool(1, t);
                }

                if(skill.TextPopup != null)
                {
                    poolForTextPopup = new ObjectPool();
                    poolForTextPopup.SetObject(skill.TextPopup);
                    poolForTextPopup.FillPool(1, t);
                }

                if (skill.IsActive)
                    tempCooldown = skill.Cooldown;
            }
        }

        public List<SkillData> skills = new List<SkillData>();

        private Card card;

        public Skill[] SKILL;

        private void Start()
        {
            card = GetComponent<Card>();
            for (int i = 0; i < SKILL.Length; i++)
            {
                AddSkill(SKILL[i]);
            }
        }

        public void AddSkill(Skill skill)
        {
            SkillData s = new SkillData(skill, transform);
            skills.Add(s);
        }

        public void ExecuteAttackerSkills()
        {
            for (int i = 0; i < skills.Count; i++)
            {
                SkillData skillData = skills[i];
                if(skillData.skill.IsAttacker)
                    ExecuteSkill(skillData);
                else
                    DecreaseCooldown(skillData);
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
            if (!skillData.skill.IsActive && skillData.tempCooldown != -1)
            {
                print(skillData.tempCooldown);

                skillData.tempCooldown = -1;
                skillData.skill.Execute(card.GetMove());
            }
            else if(skillData.skill.IsActive)
            {
                if (CooldownControl(skillData))
                {
                    skillData.skill.Execute(card.GetMove());

                    if (skillData.skill.Effect != null)
                        StartCoroutine(Animate(skillData, card.GetMove()));
                    if (skillData.skill.TextPopup != null)
                        StartCoroutine(TextPopup(skillData, card.GetMove()));

                    IncreaseCooldown(skillData);
                }
                else
                    DecreaseCooldown(skillData);
            }
        }

        private IEnumerator Animate(SkillData skillData, Move move)
        {
            GameObject obj;
            List<GameObject> objects = new List<GameObject>();

            int count = skillData.skill.GetGameobjectCount();
            for (int i = 0; i < count; i++)
            {
                obj = skillData.poolForEffect.PullObjectFromPool(transform);
                skillData.skill.PositionEffect(obj, move);
                objects.Add(obj);
            }
            
            yield return new WaitForSeconds(skillData.skill.EffectTime);

            for (int i = 0; i < count; i++)
            {
                obj = objects[i];
                obj.transform.SetParent(transform);
                skillData.poolForEffect.AddObjectToPool(obj);
            }
        }

        private IEnumerator TextPopup(SkillData skillData, Move move)
        {
            GameObject obj;
            List<GameObject> objects = new List<GameObject>();

            int count = skillData.skill.GetGameobjectCount(true);
            for (int i = 0; i < count; i++)
            {
                obj = skillData.poolForTextPopup.PullObjectFromPool(transform);
                Vector3 pos = skillData.skill.PositionTextPopup(obj, move);
                TextPopup objTxt = obj.GetComponent<TextPopup>();
                string power = skillData.skill.Power.ToString();
                objTxt.Setup(power, pos);
                objects.Add(obj);
            }

            yield return new WaitForSeconds(skillData.skill.EffectTime);

            for (int i = 0; i < count; i++)
            {
                obj = objects[i];
                obj.transform.SetParent(transform);
                skillData.poolForTextPopup.AddObjectToPool(obj);
            }
            
        }

        public bool CooldownControl(SkillData skillData)
        {
            if (skillData.tempCooldown > 0)
                return false;
            else
            {
                if (skillData.skill.ChanceFactor != 0)
                    return CalculateChance(skillData);
                return true;
            }
        }

        private bool CalculateChance(SkillData skillData)
        {
            int number = UnityEngine.Random.Range(0, 101);
            if (skillData.skill.ChanceFactor <= number)
                return true;
            return false;
        }

        private void DecreaseCooldown(SkillData skillData)
        {
            if (skillData.tempCooldown > 0 && skillData.skill.IsActive)
                skillData.tempCooldown--;
        }

        private void IncreaseCooldown(SkillData skillData)
        {
            if(skillData.skill.IsActive)
                skillData.tempCooldown = skillData.skill.Cooldown;
        }
    }
}