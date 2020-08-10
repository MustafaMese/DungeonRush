using DungeonRush.Attacking;
using DungeonRush.Cards;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Property
{
    public interface IAttacker
    {
        bool CanMove(Card enemy);
        void Attack();
        bool GetAttackFinished();
        void SetAttackFinished(bool b);
        void SetAttackStyle(AttackStyle attackStyle);
        AttackStyle GetAttackStyle();
    }
}