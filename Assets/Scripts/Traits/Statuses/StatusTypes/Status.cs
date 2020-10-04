using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Traits
{
    public abstract class Status : ScriptableObject
    {
        [SerializeField] StatusType statusType;
        [SerializeField] private int power;
        [SerializeField] private int turnCount;
        [SerializeField] private float effectLifeTime;
        [SerializeField] private GameObject effect;
        [SerializeField] private GameObject textPopUp;
        [SerializeField] Sprite icon;

        public int Power { get => power; set => power = value; }
        public int TurnCount { get => turnCount; set => turnCount = value; }
        public float EffectLifeTime { get => effectLifeTime; set => effectLifeTime = value; }
        public GameObject Effect { get => effect; set => effect = value; }
        public GameObject TextPopUp { get => textPopUp; set => textPopUp = value; }
        public StatusType StatusType { get => statusType; set => statusType = value; }
        public Sprite Icon { get => icon; set => icon = value; }

        public abstract void Execute(Card card);
    }

    public enum StatusType
    {
        BLEEDING,       // Kanama hasarı alır.
        BURNING,        // Yanma hasarı alır.
        POISONED,       // Zehir hasarı alır.
        ENTANGLED,      // Yere bağlanma durumu, sadece saldırabilir.
        DISARMED,       // Sadece ilerleyebilir.
        STUNNED,        // Eylem yapamaz.
        FROZEN,         // Buz hasarı alır.
        BLINDED,        // Görüş alanını 1 kare kısacak.
        ACID,           // Zırhı sıfırlayacak.
        SLOWED,         // Kritik/Dodge şansını düşürecek.
        ANGER,          // Sürekli saldırı halinde olacak.
        HEALING,        // Her tur iyileşme alacak.
        HASTE,          // Kritik/Dodge şansını arttırır.
        LUCKY_CHARM,    // Loot şansı artar.
        UNLUCKY_CHARM   // Loot şansı düşer.
    }
}