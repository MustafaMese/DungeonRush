using System.Collections;
using System.Collections.Generic;
using DungeonRush.Cards;
using DungeonRush.Data;
using UnityEngine;

public interface ISkill
{
    void Initialize(Card card);
    void Execute(Move move);
    void Adjust(Move move);
}

public enum SkillType
{
    ACTIVE,     // Çoklu ve aktif kullanım
    PASSIVE,    // Çoklu ve pasif kullanım
    ONESHOT     // Tekil kullanım
}