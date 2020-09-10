using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Status : ScriptableObject
{
    [SerializeField] private int power;
    [SerializeField] private float turnCount;
    [SerializeField] private GameObject effect;
    [SerializeField] private float effectLifeTime;

    public int Power { get => power; set => power = value; }
    public float TurnCount { get => turnCount; set => turnCount = value; }
    public GameObject Effect { get => effect; set => effect = value; }
    public float EffectLifeTime { get => effectLifeTime; set => effectLifeTime = value; }

    public abstract void Execute(Card card);
}
