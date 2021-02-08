using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace DungeonRush.Customization
{
    public class EnemyCustomization : MonoBehaviour, ICustomization
    {
        private const string r = "Row ";
        [SerializeField] Material shadow = null;
        [SerializeField] Material lighted = null;
        [SerializeField] Canvas characterCanvas = null;
        
        [SerializeField] List<SpriteRenderer> sprites = new List<SpriteRenderer>();
        [SerializeField] List<SpriteSkin> skins = new List<SpriteSkin>();

        private void ChangeLayer(SpriteRenderer sR, bool top, int multiplier = 1)
        {
            if(top)
                sR.sortingOrder += 6 * multiplier;
            else
                sR.sortingOrder -= 6 * multiplier;
        }

        private void ChangeLayer(Canvas c, bool top, int multiplier = 1)
        {
            if (top)
                c.sortingOrder += 6 * multiplier;
            else
                c.sortingOrder -= 6 * multiplier;
        }

        public void ChangeLayer(bool top, int multiplier = 1)
        {
            //int layer = (int)Math.Truncate(posY);

            for (int i = 0; i < sprites.Count; i++)
                ChangeLayer(sprites[i], top, multiplier);

            if (characterCanvas != null)
                ChangeLayer(characterCanvas, top, multiplier);
        }

        public void OverShadow()
        {
            // for (int i = 0; i < sprites.Count; i++)
            //     sprites[i].material = shadow;

            characterCanvas.gameObject.SetActive(false);
        }

        public void RemoveShadow()
        {
            // for (int i = 0; i < sprites.Count; i++)
            // sprites[i].material = lighted;

            characterCanvas.gameObject.SetActive(true);
        }

        public void ChangeSkinState(bool state)
        {
            for (int i = 0; i < skins.Count; i++)
                skins[i].enabled = state;
        }
    }
}