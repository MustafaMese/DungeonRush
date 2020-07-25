using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonRush.Customization
{
    public interface ICustomization
    {
        void RemoveShadow();
        void OverShadow();
        void Change();
    }
}