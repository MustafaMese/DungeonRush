using DungeonRush.Cards;
using DungeonRush.Data;
using UnityEngine;

namespace DungeonRush.Skills
{
    public abstract class Skill : ScriptableObject
    {
        [SerializeField] private int cooldown;
        [SerializeField] private float effectTime;
        [SerializeField] private GameObject effect;
        [SerializeField] private GameObject textPopup;
        [SerializeField] private bool isActive;
        [SerializeField] private bool isAttacker;
        [SerializeField] private int power;

        public int Cooldown { get => cooldown; set => cooldown = value; }
        public float EffectTime { get => effectTime; set => effectTime = value; }
        public GameObject Effect { get => effect; set => effect = value; }
        public GameObject TextPopup { get => textPopup; set => textPopup = value; }
        public bool IsActive { get => isActive; set => isActive = value; }
        public int Power { get => power; set => power = value; }
        public bool IsAttacker { get => isAttacker; set => isAttacker = value; }

        public abstract void Execute(Move move);
        public abstract void PositionEffect(GameObject effect, Move move);
        public abstract Vector3 PositionTextPopup(GameObject textPopup, Move move);
    }
}
