using UnityEngine;
using DungeonRush.Attacking;
using DungeonRush.Cards;

namespace DungeonRush.Items
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Item/Weapon")]
    public class Weapon : Item
    {
        [SerializeField] AttackStyle attackStyle;
        [SerializeField] BoneType bone;
        [Space]
        [Header("If this is a dual item, then fill the following varible.")]
        [SerializeField] Sprite secondarySprite;

        public override void Execute(Card card)
        {
            ItemUser itemUser = card.GetComponent<ItemUser>();
            SetWeapon(itemUser);
        }

        private void SetWeapon(ItemUser itemUser)
        {
            if (itemUser != null && itemUser.isWeaponUser)
            {
                itemUser.weapon = this;
                itemUser.weaponBone.sprite = GetPrimarySprite();
            }
        }

        public override int GetPower()
        {
            return attackStyle.GetPower();
        }

        public override BoneType GetBoneType()
        {
            return bone;
        }

        public override Sprite GetSecondarySprite()
        {
            return secondarySprite;
        }
    }
}