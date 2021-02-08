using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Customization
{
    public interface ICustomization
    {
        void ChangeSkinState(bool state);
        void RemoveShadow();
        void OverShadow();
        void ChangeLayer(bool moveToTop, int multiplier = 1);
    }
}

public enum BoneType
{
    HEAD,
    HELMET,
    BODY,
    BODY_ARMOR,
    ARM,
    LEG,
    WEAPON_RIGHT,
    WEAPON_LEFT,
    WEAPON_DUAL,
    NONE
}