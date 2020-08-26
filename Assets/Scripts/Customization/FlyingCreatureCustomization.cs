using DungeonRush.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace DungeonRush.Customization
{
    public class FlyingCreatureCustomization : MonoBehaviour, ICustomization
    {
        [SerializeField] Canvas characterCanvas = null;

        [SerializeField] List<SpriteRenderer> sprites = new List<SpriteRenderer>();
        [SerializeField] List<SpriteSkin> skins = new List<SpriteSkin>();

        [SerializeField] Material shadow = null;
        [SerializeField] Material lighted = null;

        private string r = "Row ";

        private void ChangeLayer(Canvas c, int layer)
        {
            string sth = String.Concat(r, layer);
            c.sortingLayerName = sth;
        }

        private void ChangeLayer(SpriteRenderer sR, int layer)
        {
            string sth = String.Concat(r, layer);
            sR.sortingLayerName = sth;
        }

        public void Change(float posY)
        {
            int layer = (int)Math.Truncate(posY);

            for (int i = 0; i < sprites.Count; i++)
                ChangeLayer(sprites[i], layer);

            if (characterCanvas != null)
                ChangeLayer(characterCanvas, layer);
        }

        public void OverShadow()
        {
            for (int i = 0; i < sprites.Count; i++)
                sprites[i].material = shadow;
        }

        public void RemoveShadow()
        {
            for (int i = 0; i < sprites.Count; i++)
                sprites[i].material = lighted;
        }

        public void ChangeSkinState(bool state)
        {
            for (int i = 0; i < skins.Count; i++)
                skins[i].enabled = state;
        }
    }
}