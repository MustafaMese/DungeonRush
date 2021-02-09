using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace DungeonRush.Customization
{
    public class EnemyCustomization : MonoBehaviour, ICustomization
    {
        [SerializeField] CharacterHUD characterHUD = null;
        
        [SerializeField] SpriteRenderer[] sprites;
        [SerializeField] GameObject skin;

        private void ChangeLayer(SpriteRenderer sR, bool top, int multiplier = 1)
        {
            if (top)
                sR.sortingOrder += 6 * multiplier;
            else
                sR.sortingOrder -= 6 * multiplier;
        }

        private void ChangeLayer(CharacterHUD c, bool top, int multiplier = 1)
        {
            if (top)
            {
                c.BarBG.sortingOrder += 6 * multiplier;
                c.BarSprite.sortingOrder += 6 * multiplier;
                c.GetName().sortingOrder += 6 * multiplier;
            }
            else
            {
                c.BarBG.sortingOrder -= 6 * multiplier;
                c.BarSprite.sortingOrder -= 6 * multiplier;
                c.GetName().sortingOrder -= 6 * multiplier;
            }
        }

        public void ChangeLayer(bool top, int multiplier = 1)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                print("1");
                ChangeLayer(sprites[i], top, multiplier);
            }
            if (characterHUD != null)
                ChangeLayer(characterHUD, top, multiplier);
        }

        public void OverShadow()
        {
            characterHUD.gameObject.SetActive(false);
        }

        public void RemoveShadow()
        {
            characterHUD.gameObject.SetActive(true);
        }

        public void ChangeSkinState(bool state)
        {
            skin.SetActive(state);
        }
    }
}