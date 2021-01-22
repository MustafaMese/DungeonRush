using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonRush.UI.HUD
{
    public class CharacterCanvas : MonoBehaviour
    {
        [SerializeField] GameObject statusPanel;

        public Image AddImageToPanel(Sprite sprite)
        {
            var obj = new GameObject("Image").AddComponent<Image>();
            obj.sprite = sprite;
            return Instantiate(obj, statusPanel.transform);
        }
    }
}
