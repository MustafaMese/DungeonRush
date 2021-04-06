using System;
using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using DungeonRush.Managers;
using UnityEngine;

namespace DungeonRush.Skills
{
    public abstract class Skill : MonoBehaviour, ISkill
    {
        [Multiline(2)]
        [SerializeField]
        private string notes = "- General options and effect options will be using for every skills. \n" +
            "- isAttacker determines using at attacking or moving action.";

        [Header("General Options")]
        [SerializeField] private string skillName;
        [Multiline(2), SerializeField] private string explanation;
        [SerializeField] private int cooldown;
        [SerializeField] private bool isAttacker;
        [SerializeField] protected bool isUsingTextPopup;
        
        [Header("Effect Options")]
        [Space]
        [SerializeField] private float effectTime;
        [SerializeField] protected GameObject effect;

        [Header("Icon Options")]
        [Space]
        [SerializeField] Sprite iconSmallForActivePanel;
        [SerializeField] Sprite iconBigForPickCanvas;

        [Header("Additional Options")]
        [Space]
        [SerializeField] private int power;
        
        protected ObjectPool pool;
        protected int tempCooldown;
        protected Card card;

        [HideInInspector] public SkillType skillType;

        public float EffectTime { get => effectTime; }
        public int Power { get => power; set => power = value; }
        public bool IsAttacker { get => isAttacker; set => isAttacker = value; }
        public Sprite IconSmall { get => iconSmallForActivePanel; }
        public Sprite IconBig { get => iconBigForPickCanvas; }
        public string SkillName { get => skillName; set => skillName = value; }
        public string Explanation { get => explanation; set => explanation = value; }

        public abstract void Execute(Move move);
        public abstract void Adjust(Move move);

        public virtual void PositionEffect(GameObject obj, Move move) { return; }
        public virtual int GetGameobjectCount(bool isTextPopup = false) { return 1; }

        public virtual void Initialize(Card card)
        {
            tempCooldown = 0;
            this.card = card;

            pool = new ObjectPool();
            FillPool(pool, effect, 2);
        }

        private void FillPool(ObjectPool pool, GameObject effect, int objectCount)
        {
            pool.SetObject(effect);
            pool.Fill(objectCount, transform);
        }

        public virtual IEnumerator Animate(Move move)
        {
            GameObject obj;
            List<GameObject> objects = new List<GameObject>();
            int count = GetGameobjectCount();
            for (int i = 0; i < count; i++)
            {
                obj = pool.Pull(transform);
                obj.SetActive(true);

                PositionEffect(obj, move);
                objects.Add(obj);

                if (isUsingTextPopup)
                    TextPopupManager.Instance.TextPopup(obj.transform.position, power.ToString());
            }

            yield return new WaitForSeconds(EffectTime);

            for (int i = 0; i < count; i++)
            {
                obj = objects[i];
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                pool.Push(obj);
            }
        }

        protected virtual void IncreaseCooldown()
        {
            tempCooldown = cooldown;
        }

        public virtual void DecreaseCooldown()
        {
            tempCooldown--;
        }
    }
}
