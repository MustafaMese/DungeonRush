using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Skills {

    [Serializable]
    public class SkillData
    {
        public Skill skill;
        public ObjectPool poolForEffect;
        public ObjectPool poolForTextPopup;
        public int tempCooldown;

        public int listnumber;

        public SkillData(Skill skill, Transform t, int listnumber)
        {
            this.skill = skill;

            if (skill.Effect != null)
            {
                poolForEffect = new ObjectPool();
                poolForEffect.SetObject(skill.Effect);
                poolForEffect.FillPool(1, t);
            }

            if (skill.TextPopup != null)
            {
                poolForTextPopup = new ObjectPool();
                poolForTextPopup.SetObject(skill.TextPopup);
                poolForTextPopup.FillPool(1, t);
            }

            if (skill.IsMultiUse)
                tempCooldown = skill.Cooldown;

            this.listnumber = listnumber;
        }
    }

    public class SkillUser : MonoBehaviour
    {
        public List<SkillData> skills = new List<SkillData>();
        private Card card;
        public Skill[] SKILL;

        private int lastIndex;

        private void Start()
        {
            lastIndex = 0;
            card = GetComponent<Card>();
            for (int i = 0; i < SKILL.Length; i++)
            {
                //AddSkill(SKILL[i]);
            }
        }

        public List<string> GetSkillIDs()
        {
            List<string> ids = new List<string>();
            for (int i = 0; i < skills.Count; i++)
            {
                string id = skills[i].skill.SkillName;
                ids.Add(id);
            }
            return ids;
        }

        public void AddSkill(Skill skill)
        {
            SkillData s = new SkillData(skill, transform, lastIndex);
            if (skill.IsActive)
                UIManager.Instance.AddSkillToButton(s);
            skills.Add(s);
            lastIndex++;
        }

        public void AddSkill(SkillData skillData)
        {
            if (skillData.skill.IsActive)
                UIManager.Instance.AddSkillToButton(skillData);

            skills.Add(skillData);
            lastIndex = skillData.listnumber + 1;
        }

        public void ExecuteActiveSkill(SkillData skillD)
        {
            for (int i = 0; i < skills.Count; i++)
            {
                SkillData skillData = skills[i];
                if (skillData.skill.IsActive && skillData.listnumber == skillD.listnumber)
                    ExecuteSkill(skillData, true);
                else
                    DecreaseCooldown(skillData);
            }
        }

        public void ExecuteAttackerSkills()
        {
            for (int i = 0; i < skills.Count; i++)
            {
                SkillData skillData = skills[i];
                if(skillData.skill.IsAttacker && !skillData.skill.IsActive)
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
                if (!skillData.skill.IsAttacker && !skillData.skill.IsActive)
                    ExecuteSkill(skillData);
                else
                    DecreaseCooldown(skillData);
            }
        }

        private void ExecuteSkill(SkillData skillData, bool isActive = false)
        {
            if (!skillData.skill.IsMultiUse && skillData.tempCooldown != -1)
            {
                skillData.tempCooldown = -1;
                skillData.skill.Execute(card.GetMove());
            }
            else if(skillData.skill.IsMultiUse)
            {
                if (CooldownControl(skillData))
                {
                    if (!skillData.skill.IsActive)
                    {
                        skillData.skill.Execute(card.GetMove());
                        PlayAnimation(skillData, card.GetMove());
                        IncreaseCooldown(skillData);

                    }
                    else
                    {
                        if (isActive)
                        {
                            Move move = new Move(card);
                            skillData.skill.Execute(move);
                            PlayAnimation(skillData, move);
                            IncreaseCooldown(skillData);
                            card.Controller.Stop();
                        }
                        else return;
                    }
                }
                else
                    DecreaseCooldown(skillData);
            }

            SkillButtonControl(skillData);
        }

        private void PlayAnimation(SkillData skillData, Move move)
        {
            if (skillData.skill.Effect != null)
                StartCoroutine(Animate(skillData, move));
            if (skillData.skill.TextPopup != null)
                StartCoroutine(TextPopup(skillData, move));
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
            if (skillData.tempCooldown > 0 && skillData.skill.IsMultiUse)
                skillData.tempCooldown--;

            SkillButtonControl(skillData);

        }

        //  bu static olabilir
        private void SkillButtonControl(SkillData skillData)
        {
            if (skillData.skill.IsActive)
            {
                if (skillData.tempCooldown <= 0)
                    UIManager.Instance.ButtonControl(skillData, true);
                else
                    UIManager.Instance.ButtonControl(skillData, false);
            }
        }

        private void IncreaseCooldown(SkillData skillData)
        {
            if(skillData.skill.IsMultiUse)
                skillData.tempCooldown = skillData.skill.Cooldown;
        }


    }
}