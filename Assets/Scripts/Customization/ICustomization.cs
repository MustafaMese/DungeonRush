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
        void Change(float posY);
    }
}