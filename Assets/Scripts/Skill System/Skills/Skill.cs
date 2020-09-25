using DungeonRush.Cards;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Skills
{
    public abstract class Skill : ScriptableObject
    {
        [Header("General Options")]
        [SerializeField] private int cooldown;
        [SerializeField] private bool isActive;
        [SerializeField] private bool isAttacker;

        [Header("Effect Options")]
        [Space]
        [SerializeField] private float effectTime;
        [SerializeField] private GameObject effect;
        [SerializeField] private GameObject textPopup;
        
        [Header("Additional Options")]
        [Space]
        [SerializeField] private int power;
        [SerializeField] private int chanceFactor;

        public int Cooldown { get => cooldown; set => cooldown = value; }
        public float EffectTime { get => effectTime; set => effectTime = value; }
        public GameObject Effect { get => effect; set => effect = value; }
        public GameObject TextPopup { get => textPopup; set => textPopup = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public int Power { get => power; set => power = value; }
        public bool IsAttacker { get => isAttacker; set => isAttacker = value; }
        public int ChanceFactor { get => chanceFactor; set => chanceFactor = value; }

        public abstract void Execute(Move move);
        public abstract void PositionEffect(GameObject effect, Move move);
        public abstract Vector3 PositionTextPopup(GameObject textPopup, Move move);
        public virtual int GetGameobjectCount(bool isTextPopup = false) { return 1; }
    }
}
