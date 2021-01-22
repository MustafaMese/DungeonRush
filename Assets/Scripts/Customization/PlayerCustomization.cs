using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace DungeonRush.Customization
{
    public class PlayerCustomization : MonoBehaviour, ICustomization
    {
        private const string r = "Row ";

        [SerializeField] Canvas characterCanvas = null;

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
        
        [SerializeField] List<SpriteSkin> skins = new List<SpriteSkin>();
        private List<SpriteRenderer> sprites = new List<SpriteRenderer>();

        private void Start() 
        {
            sprites.Add(head);
            sprites.Add(helmet);    
            sprites.Add(body);    
            sprites.Add(bodyArmor);    
            sprites.Add(armRight);    
            sprites.Add(armLeft);    
            sprites.Add(legLeft);    
            sprites.Add(legRight);    
            sprites.Add(weaponleft);    
            sprites.Add(weaponRight);
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


        private void ChangeLayer(SpriteRenderer sR, int layer)
        {
            string sth = String.Concat(r, layer);
            sR.sortingLayerName = sth;
        }

        private void ChangeLayer(Canvas c, int layer)
        {
            string sth = String.Concat(r, layer);
            c.sortingLayerName = sth;
        }

        public void ChangeLayer(float posY)
        {
            int layer = (int)Math.Truncate(posY);

            for (int i = 0; i < sprites.Count; i++)
                ChangeLayer(sprites[i], layer);

            if (characterCanvas != null)
                ChangeLayer(characterCanvas, layer);
        }

        public void OverShadow()
        {
            //for (int i = 0; i < sprites.Count; i++)
            //    sprites[i].material = shadow;

            characterCanvas.gameObject.SetActive(false);
        }

        public void RemoveShadow()
        {
            //for (int i = 0; i < sprites.Count; i++)
            //    sprites[i].material = lighted;

            characterCanvas.gameObject.SetActive(true);
        }

        public void ChangeSkinState(bool state)
        {
            for (int i = 0; i < skins.Count; i++)
                skins[i].enabled = state;
        }
    }
}