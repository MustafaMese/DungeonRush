using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Skills
{
    public abstract class Skill : MonoBehaviour, ISkill
    {
        [Multiline(8)]
        [Tooltip("A string using the MultiLine attribute")]
        [SerializeField]
        private string notes = "- General options and effect options will be using for every skills. \n" +
            "- isAttacker determines using at attacking or moving action.";

        [Header("General Options")]
        [SerializeField] private string skillName;
        [SerializeField] private int cooldown;
        [SerializeField] private bool isAttacker;
        [SerializeField] protected bool isUsingTextPopup;

        [Header("Effect Options")]
        [Space]
        [SerializeField] private float effectTime;
        [SerializeField] private GameObject effect;

        [Header("Icon Options")]
        [Space]
        [SerializeField] Sprite iconSmallForActivePanel;
        [SerializeField] Sprite iconBigForPickCanvas;

        [Header("Additional Options")]
        [Space]
        [SerializeField] private int power;
        
        protected ObjectPool<GameObject> effectPool;
        protected int tempCooldown;
        protected bool canExecute;
        protected Card card;

        public SkillType skillType;

        public float EffectTime { get => effectTime; }
        public int Power { get => power; set => power = value; }
        public bool IsAttacker { get => isAttacker; set => isAttacker = value; }
        public Sprite IconSmall { get => iconSmallForActivePanel; }
        public Sprite IconBig { get => iconBigForPickCanvas; }
        public string SkillName { get => skillName; set => skillName = value; }

        public abstract void Execute(Move move);
        public abstract void Adjust(Move move);

        public virtual void PositionEffect(GameObject obj, Move move) { return; }
        public virtual int GetGameobjectCount(bool isTextPopup = false) { return 1; }

        public virtual void Initialize(Card card)
        {
            tempCooldown = cooldown;
            canExecute = false;
            this.card = card;

            effectPool = new ObjectPool<GameObject>();
            FillPool(effectPool, effect, 2);
        }

        private void FillPool(ObjectPool<GameObject> pool, GameObject effect, int objectCount)
        {
            pool.SetObject(effect);
            pool.FillPool(objectCount, transform);
        }

        protected IEnumerator Animate(Move move)
        {
            GameObject obj;
            List<GameObject> objects = new List<GameObject>();
            int count = GetGameobjectCount();
            for (int i = 0; i < count; i++)
            {
                obj = effectPool.Pull(transform);
                obj.SetActive(true);

                PositionEffect(obj, move);
                objects.Add(obj);

                if(isUsingTextPopup)
                    TextPopupManager.Instance.TextPopup(obj.transform.position, power.ToString());
            }

            yield return new WaitForSeconds(EffectTime);

            for (int i = 0; i < count; i++)
            {
                obj = objects[i];
                obj.transform.SetParent(transform);
                obj.SetActive(false);
                effectPool.AddObjectToPool(obj);
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
