using DungeonRush.UI.HUD;
using UnityEngine;

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
        [SerializeField] SpriteRenderer weaponLeft;
        [SerializeField] SpriteRenderer shadow;
        
        [SerializeField] GameObject skin;

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
                    weaponLeft.sprite = null;
                    break;
                case BoneType.WEAPON_LEFT:
                    weaponLeft.sprite = sprite;
                    weaponRight.sprite = null;
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
                    weaponLeft.sprite = leftSprite;
                    break;
            }
        }

        private void ChangeLayer(SpriteRenderer sR, bool top, int multiplier = 1)
        {
            if(top)
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
            ChangeLayer(head, top, multiplier);
            ChangeLayer(helmet, top, multiplier);
            ChangeLayer(body, top, multiplier);
            //ChangeLayer(bodyArmor, top, multiplier);
            ChangeLayer(armRight, top, multiplier);
            ChangeLayer(armLeft, top, multiplier);
            ChangeLayer(legRight, top, multiplier);
            ChangeLayer(legLeft, top, multiplier);
            ChangeLayer(weaponRight, top, multiplier);
            ChangeLayer(weaponLeft, top, multiplier);
            ChangeLayer(shadow, top, multiplier);

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