using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace DungeonRush.Customization
{
    public class PlayerCustomization : MonoBehaviour, ICustomization
    {
        [SerializeField] CharacterHUD characterHUD = null;

        [SerializeField] SpriteRenderer head;
        [SerializeField] SpriteRenderer helmet;
        [SerializeField] SpriteRenderer body;
        [SerializeField] SpriteRenderer bodyArmor;
        [SerializeField] SpriteRenderer armRight;
        [SerializeField] SpriteRenderer armLeft;
        [SerializeField] SpriteRenderer legRight;
        [SerializeField] SpriteRenderer legLeft;
        [SerializeField] SpriteRenderer weaponRight;
        [SerializeField] SpriteRenderer weaponleft;
        
        [SerializeField] GameObject skin;
        private SpriteRenderer[] sprites = new SpriteRenderer[10];

        private void Start() 
        {
            sprites[0] = armRight;
            sprites[1] = helmet;
            sprites[2] = body;
            sprites[3] = bodyArmor;
            sprites[4] = head;
            sprites[5] = armLeft;
            sprites[6] = legLeft;
            sprites[7] = legRight;
            sprites[8] = weaponleft;
            sprites[9] = weaponRight;
        }

        public void ChangeBoneSprite(BoneType bone, Sprite sprite)
        {
            switch (bone)
            {
                case BoneType.HEAD:
                    head.sprite = sprite;
                    break;
                case BoneType.HELMET:
                    helmet.sprite = sprite;
                    break;
                case BoneType.BODY:
                    body.sprite = sprite;
                    break;
                case BoneType.BODY_ARMOR:
                    bodyArmor.sprite = sprite;
                    break;
                case BoneType.WEAPON_RIGHT:
                    weaponRight.sprite = sprite;
                    break;
                case BoneType.WEAPON_LEFT:
                    weaponleft.sprite = sprite;
                    break;
            }   
        }

        public void ChangeBoneSprite(BoneType bone, Sprite rightSprite, Sprite leftSprite)
        {
            switch (bone)
            {
                case BoneType.ARM:
                    armRight.sprite = rightSprite;
                    armLeft.sprite = leftSprite;
                    break;
                case BoneType.LEG:
                    legRight.sprite = rightSprite;
                    legLeft.sprite = leftSprite;
                    break;
                case BoneType.WEAPON_DUAL:
                    weaponRight.sprite = rightSprite;
                    weaponleft.sprite = leftSprite;
                    break;
            }
        }

        private void ChangeLayer(SpriteRenderer sR, bool top, int multiplier = 1)
        {
            if(top)
                sR.sortingOrder += 6 * multiplier;
            else
            {
                sR.sortingOrder -= 6 * multiplier;
            }
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