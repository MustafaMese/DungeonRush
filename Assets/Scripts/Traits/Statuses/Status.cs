using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : ScriptableObject
{
    [SerializeField] private int power;
    [SerializeField] private int turnCount;
    [SerializeField] private float effectLifeTime;
    [SerializeField] private GameObject effect;
    [SerializeField] private GameObject textPopUp;

    public int Power { get => power; set => power = value; }
    public int TurnCount { get => turnCount; set => turnCount = value; }
    public float EffectLifeTime { get => effectLifeTime; set => effectLifeTime = value; }
    public GameObject Effect { get => effect; set => effect = value; }
    public GameObject TextPopUp { get => textPopUp; set => textPopUp = value; }

    public abstract void Execute(Card card);
}
