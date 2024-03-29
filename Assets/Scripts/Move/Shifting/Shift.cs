﻿using DungeonRush.Cards;
using DungeonRush.Field;
using DungeonRush.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Shifting
{
    public abstract class Shift : ScriptableObject
    {
        [SerializeField] GameObject effect;

        public abstract bool Define(Card card, Swipe swipe);
        public abstract Dictionary<Tile, Swipe> GetAvaibleTiles(Card card);
        public GameObject GetEffect() { return effect; }
    }
}
